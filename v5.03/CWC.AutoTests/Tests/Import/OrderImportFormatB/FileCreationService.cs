using Cwc.BaseData;
using Cwc.Integration.OrderImportFormatB;
using Cwc.Integration.OrderImportFormatB.Model;
using Cwc.Integration.OrderImportFormatB.Services;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edsson.WebPortal.AutoTests.Tests.Import.OrderImportFormatB
{
    public class FileCreationService
    {
        Customer customer;
        Location location;
        BagType bagType;
        Product product;
        BankAccount bankAccount;
        DateTime definedDeliveryDate;
        DateTime fixedDate;
        public OrderImportFormatBJobSettings settings;
        public StringBuilder file;
        public FileCreationService()
        {
            customer = DataFacade.Customer.Take(x => x.ReferenceNumber == "1101");
            location = DataFacade.Location.Take(x => x.Code == "SP02");
            bankAccount = DataFacade.BankAccount.Take(x => x.ID == location.BankAccount_Id);
            bagType = DataFacade.ContainerType.Take(x => x.ID == 1001);
            product = DataFacade.Product.Take(x => x.Description == "Bundel 20 EUR");
            definedDeliveryDate = DateTime.Today;
            fixedDate = new DateTime(1991, 1, 10);

            settings = new OrderImportFormatBJobSettings()
            {
                CompanyID = this.customer.IdentityID,
                IncomingFileFolder = @"D:\OrderImport\FormatB",
                IncomingFilePrefix = "BRGE",
                LastFileSequenceNumber = 0,
                IsCreateVisitAddress = false
            };

            file = new StringBuilder();
        }

        public void CreateLeadingRecord()
        {
            var leadRecord = new StringBuilder();
            leadRecord.Append("1", 0, 1);
            leadRecord.AppendFormat("{0, 5}", this.GetSequnceNumber());
            leadRecord.AppendFormat("{0, 8}", DateTime.Now.ToString("yyyyMMdd"));
            leadRecord.AppendFormat("{0, 6}", DateTime.Now.TimeOfDay.ToString("hhmmss"));
            leadRecord.Append("100020");
            leadRecord.Append(' ', 174);

            file.AppendLine(leadRecord.ToString());
        }

        public void CreateOrderRecord()
        {
            var orderRecord = new StringBuilder();
            orderRecord.Append("2");
            orderRecord.Append("A18840");
            orderRecord.Append(' ', 8);
            orderRecord.AppendFormat("{0, 8}", bankAccount.Number);
            orderRecord.Append(definedDeliveryDate.ToString("yyyyMMdd"));
            orderRecord.AppendFormat("{0, 10}", bagType.Code);
            orderRecord.Append(ValueGenerator.GenerateString("Reference", 10));
            orderRecord.Append("9");
            orderRecord.Append(" ");
            orderRecord.Append("9");
            orderRecord.Append((ValueGenerator.GenerateString("OBRef", 20)));
            orderRecord.Append(' ', 124);

            file.AppendLine(orderRecord.ToString());
        }

        public void CreateOrderItemRecord()
        {
            var orderItemRecord = new StringBuilder();
            orderItemRecord.Append("3");
            orderItemRecord.AppendFormat("{0, 10}", "BO5000");
            orderItemRecord.AppendFormat("{0, 6}", "1");

            file.AppendLine(orderItemRecord.ToString());
        }

        public void CreateOrderDeliveryInformationRecord()
        {
            var deliveryInformationRecord = new StringBuilder();
            deliveryInformationRecord.Append("4");
            deliveryInformationRecord.AppendFormat("{0, 73}", location.Name);
            deliveryInformationRecord.Append(fixedDate.ToString("yyyyMMdd"));
            deliveryInformationRecord.Append("123456789012345");
            deliveryInformationRecord.AppendFormat("{0, 30}", "Street");
            deliveryInformationRecord.Append(' ', 5);
            deliveryInformationRecord.Append(' ', 4);
            deliveryInformationRecord.Append("123456");
            deliveryInformationRecord.AppendFormat("{0, 30}", "City");
            deliveryInformationRecord.Append(' ', 28);

            file.AppendLine(deliveryInformationRecord.ToString());
        }

        public void CreateCloseRecord()
        {
            var closeRecord = new StringBuilder();
            closeRecord.Append("9");
            closeRecord.AppendFormat("{0, 6}", file.ToString().Split('\n').Count() - 2);
            closeRecord.Append(' ', 193);

            file.AppendLine(closeRecord.ToString());
        }    

        private int GetSequnceNumber()
        {
            var sequenceNumber = -1;

            using (var context = new OrderImportFormatBDataContext())
            {
                sequenceNumber = context.OrderImportFormatBJobSettings.Select(x => x.LastFileSequenceNumber).First();
            }

            if(sequenceNumber == -1)
            {
                throw new ArgumentNullException("Sequence number wasn't found");
            }

            return sequenceNumber;
        }
    }
}
