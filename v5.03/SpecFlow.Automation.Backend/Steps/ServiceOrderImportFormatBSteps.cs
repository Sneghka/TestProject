using Cwc.Ordering;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using Specflow.Automation.Backend.Helpers;
using Specflow.Automation.Backend.Model;
using System;
using System.Globalization;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Specflow.Automation.Backend.Steps
{
    [Binding]
    public class ServiceOrderImportFormatBSteps
    {

        public ServiceOrderImportFormaBContainer formatBContainer;
        private ServiceOrderImportFormatBHelper formatBHelper = new ServiceOrderImportFormatBHelper();
        protected Order serviceOrder;
        public ServiceOrderImportFormatBSteps(ServiceOrderImportFormaBContainer container)
        {
            formatBContainer = container;
        }

        [Given(@"Import Express Delivery Order File is created with following Leading record")]
        public void GivenImportExpressDeliveryOrderFileIsCreatedWithFollowingLeadingRecord(Table table)
        {
            formatBContainer.LeadingRecord = table.CreateInstance<ServiceOrderImportFormatBLeadingRecord>();
            formatBContainer.LeadingRecord.SequenceNumberOfFile = ServiceOrderImportFormatBHelper.TakeJobSettings().LastFileSequenceNumber + 1;
            formatBContainer.LeadingRecord.Date = DateTime.Now;
            formatBContainer.LeadingRecord.Time = DateTime.Now;
            ServiceOrderImportFormatBHelper.FilePath = formatBHelper.ConfigureFileName(formatBContainer.LeadingRecord);
        }

        [Given("Import Express Delivery Order File is created with following Order record")]
        public void GivenImportExpressDeliveryOrderFileIsCreatedWithFollowingOrderRecord(Table table)
        {
            formatBContainer.OrderRecord = table.CreateInstance<ServiceOrderImportFormatBOrderRecord>();
            formatBContainer.OrderRecord.DeliveryDate = DateTime.Now.AddDays(2).ToString("yyyyMMdd");
        }

        [Given("Import Express Delivery Order File is created with following Order item record")]
        public void GivenImportExpressDeliveryOrderFileIsCreatedWithFollowingOrderItemRecord(Table table)
        {
            foreach (var item in table.CreateSet<ServiceOrderImportFormatBOrderItemRecord>())
            {
                formatBContainer.OrderItemRecord.Add(item);
            }
        }

        [Given("Import Express Delivery Order File is created with following Ordered delivery information record")]
        public void GivenImportExpressDeliveryOrderFileIsCreatedWithFollowingOrderedDeliveryInformationRecord(Table table)
        {
            formatBContainer.OrderedDeliveryInfoRecord = table.CreateInstance<ServiceOrderImportFormatBOrderedDeliveryInfo>();
        }

        [Given("Import Express Delivery Order File is created with following Close record")]
        public void ImportExpressDeliveryOrderFileIsCreatedWithFollowingCloseRecord(Table table)
        {
            formatBContainer.CloseRecord = table.CreateInstance<ServiceOrderImportFormatBCloseRecord>();
            formatBHelper.CreateFile(formatBContainer.LeadingRecord, formatBContainer.OrderRecord, formatBContainer.OrderItemRecord, formatBContainer.OrderedDeliveryInfoRecord, formatBContainer.CloseRecord);
        }

        [When("Service Order Import Format B Interface processes file")]
        public void WhenServiceOrderImportFormatBInterfaceProcessesFile()
        {
            formatBHelper.RunOrderImportExpressDeliveryOrderFormatBJob();
        }

        [Then("Service order was created with correct attributes")]
        public void ServiceOrderWasCreatedWithCorrectAttributes()
        {
            using (var orderingContext = new AutomationOrderingDataContext())
            {
                var serviceOrder = orderingContext.Orders.Where(so => so.ReferenceID == formatBContainer.OrderRecord.AccountNumber).OrderByDescending(so=>so.DateCreated).FirstOrDefault();
                var serviceDate = DateTime.ParseExact(formatBContainer.OrderRecord.DeliveryDate, "yyyyMMdd", CultureInfo.InvariantCulture);

                Assert.True(serviceOrder.ReferenceID == formatBContainer.OrderRecord.AccountNumber, "AccountNumber was created with error");
                Assert.True(serviceOrder.BankReference == formatBContainer.OrderRecord.BankReference, "BankReference was created with error");
                Assert.True(serviceOrder.ServiceDate == serviceDate, "ServiceDate was created with error");

            }
        }

        [Then("Service products was created correctly")]
        public void ServiceProductsWasCreatedCorrectly()
        {        
            foreach (var orderItemRecord in formatBContainer.OrderItemRecord)
            {
                var ImportFormatBItemList = ServiceOrderImportFormatBHelper.TakeOrderImportFormatBItem(serviceOrder, formatBContainer.OrderRecord, orderItemRecord);
                foreach( var item in ImportFormatBItemList)
                {
                    Assert.True(item.OrderProductNumber == orderItemRecord.Quantity,"ProductQuantity was created with error");
                    Assert.True(item.BankProductCode == orderItemRecord.ArticleCode, "ArticleCode was created with error");
                }
            }       
        }
    }
}
