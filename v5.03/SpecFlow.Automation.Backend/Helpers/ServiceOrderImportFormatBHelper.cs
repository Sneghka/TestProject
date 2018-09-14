using Cwc.Common;
using Cwc.Integration.OrderImportFormatB;
using Cwc.Integration.OrderImportFormatB.Model;
using Cwc.Ordering;
using CWC.AutoTests.Model;
using Specflow.Automation.Backend.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Specflow.Automation.Backend.Helpers
{
    class ServiceOrderImportFormatBHelper
    {
        DateTime tempDate;
        DateTime tempTime;

        public static string FilePath { get; set; }
        public static OrderImportFormatBJobSettings settings = null;
        string FilePathTemp = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Exchange\\ServiceOrderImportFormatB"));

        public ServiceOrderImportFormatBHelper()
        {
            settings = TakeJobSettings();
        }

        public static OrderImportFormatBJobSettings TakeJobSettings()
        {
            using (var context = new AutomationOrderImportFormatBDataContext())
            {
                return OrderImportFormatBFacade.OrderImportFormatBJobSettingsService.LoadFirstSetting(context);
            }
        }

        public void RunOrderImportExpressDeliveryOrderFormatBJob()
        {
            OrderImportFormatBFacade.OrderImportFormatBJobService.ProcessImportExpressDeliveryOrder(settings);
        }

        public void RunOrderImportPostCodeFormatBJob()
        {

            OrderImportFormatBFacade.OrderImportFormatBJobService.ProcessPostCodeIncomingFile(settings);
        }

        private string LeadingRecordLineBuilder(ServiceOrderImportFormatBLeadingRecord record)
        {
            return $"1" +
                $"{record.SequenceNumberOfFile}".PadLeft(SpacesNumber(5)) +
                $"{record.Date.ToString("yyyyMMdd")}".PadLeft(SpacesNumber(8)) +
                $"{record.Time.TimeOfDay.ToString("hhmmss")}".PadLeft(SpacesNumber(6)) +
                $"{record.BankIdentification}".PadLeft(SpacesNumber(6)) +
                $"".PadLeft(SpacesNumber(174));
        }

        private string OrderRecordLineBuilder(ServiceOrderImportFormatBOrderRecord record)
        {
            return $"2" +
                $"{record.AddressCode}".PadLeft(SpacesNumber(6)) +
                $"".PadLeft(SpacesNumber(8)) +
                $"{record.AccountNumber}".PadLeft(SpacesNumber(10)) +
                $"{record.DeliveryDate}".PadLeft(SpacesNumber(8)) +
                $"{record.BagType}".PadLeft(SpacesNumber(10)) +
                $"{record.Reference}".PadLeft(SpacesNumber(10)) +
                $"9" +
                $"".PadLeft(SpacesNumber(1)) +
                $"9" +
                $"{record.BankReference}".PadLeft(SpacesNumber(20)) +
                $"".PadLeft(SpacesNumber(124));
        }

        private string OrderItemRecordLineBuilder(ServiceOrderImportFormatBOrderItemRecord record)
        {
            return $"3" +
                $"{record.ArticleCode}".PadLeft(SpacesNumber(10)) +
                $"{record.Quantity}".PadLeft(SpacesNumber(6)) +
                $"".PadLeft(SpacesNumber(183));
        }

        private string OrderedDeliveryInformationRecordLineBuilder(ServiceOrderImportFormatBOrderedDeliveryInfo record)
        {
            var tempDateOfBirth = DateTime.ParseExact(record.DateOfBirth, "yyyyMMdd", CultureInfo.InvariantCulture);
            return $"4" +
                $"{record.Name}".PadLeft(SpacesNumber(73)) +
                $"{tempDateOfBirth.Date.ToString("yyyyMMdd")}".PadLeft(SpacesNumber(8)) +
                $"{record.PhoneNumber}".PadLeft(SpacesNumber(15)) +
                $"{record.Street}".PadLeft(SpacesNumber(30)) +
                $"{record.HouseNumber}".PadLeft(SpacesNumber(5)) +
                $"{record.HouseNumberAddition}".PadLeft(SpacesNumber(4)) +
                $"{record.PostCode}".PadLeft(SpacesNumber(6)) +
                $"{record.City}".PadLeft(SpacesNumber(30)) +
                $"".PadLeft(SpacesNumber(28));
        }

        private string CloseRecordLineBuilderLineBuilder(ServiceOrderImportFormatBCloseRecord record)
        {
            return $"9" +
                $"{record.NumberOfDetailRecords}".PadLeft(SpacesNumber(6)) +
                $"".PadLeft(SpacesNumber(193));
        }

        public void CreateFile(ServiceOrderImportFormatBLeadingRecord leadingRecord, ServiceOrderImportFormatBOrderRecord orderRecord, List<ServiceOrderImportFormatBOrderItemRecord> orderItemRecord, ServiceOrderImportFormatBOrderedDeliveryInfo orderedDeliveryInfoRecord, ServiceOrderImportFormatBCloseRecord closeRecord)
        {
            using (StreamWriter fileTemp = new StreamWriter(FilePath, false, System.Text.Encoding.UTF8))
            {
                fileTemp.WriteLine(LeadingRecordLineBuilder(leadingRecord));
                fileTemp.WriteLine(OrderRecordLineBuilder(orderRecord));
                foreach (var orderItem in orderItemRecord)
                {
                    fileTemp.WriteLine(OrderItemRecordLineBuilder(orderItem));
                }
                fileTemp.WriteLine(OrderedDeliveryInformationRecordLineBuilder(orderedDeliveryInfoRecord));
                fileTemp.WriteLine(CloseRecordLineBuilderLineBuilder(closeRecord));
            }
        }

        private int SpacesNumber(int fieldFullSize, int fieldActualSize = 0)
        {
            var spaceNumber = fieldFullSize - fieldActualSize;
            return spaceNumber;
        }

        public static List<OrderImportFormatBItem> TakeOrderImportFormatBItem(Order serviceOrder, ServiceOrderImportFormatBOrderRecord orderRecord, ServiceOrderImportFormatBOrderItemRecord orderItemRecord, AutomationOrderingDataContext orderingContext = null)
        {
            using (orderingContext = DataContext.Create<AutomationOrderingDataContext>(orderingContext))
            {
                using (AutomationOrderImportFormatBDataContext formatBContext = DataContext.Create<AutomationOrderImportFormatBDataContext, AutomationOrderingDataContext>(new[] { orderingContext.GetType() }, orderingContext))
                {

                    var orderImportFormatBJobProductLinksSet = formatBContext.Set<OrderImportFormatBJobProductLink>();
                    var orderProducts = (from serviceOrders in orderingContext.Orders
                                         join soLine in orderingContext.OrderLines on serviceOrders.ID equals soLine.OrderID
                                         join soProduct in orderingContext.SOProduct on soLine.ID equals soProduct.OrderLine_ID
                                         join jpl in orderImportFormatBJobProductLinksSet on soProduct.ID equals jpl.ProductID
                                         where serviceOrders.ReferenceID == orderRecord.Reference 
                                         && serviceOrders.ID == serviceOrder.ID 
                                         && jpl.BankProductCode == orderItemRecord.ArticleCode
                                         group soProduct by new
                                         {
                                             soProduct.OrderProductNumber,
                                             jpl.BankProductCode
                                         } into grp
                                         select new OrderImportFormatBItem()
                                         {
                                             OrderProductNumber = grp.Key.OrderProductNumber,
                                             BankProductCode = grp.Key.BankProductCode
                                         }).ToList();

                    return orderProducts;
                }
            }            
        }

        public string ConfigureFileName(ServiceOrderImportFormatBLeadingRecord leadingRecord)
        {
            FilePath = Path.Combine(FilePathTemp, $"{leadingRecord.BankIdentification}{settings.IncomingFilePrefix}{leadingRecord.SequenceNumberOfFile:D5}.in");
            return FilePath;
        }
    }
}
