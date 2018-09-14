using Cwc.Integration.OrderImportFormatA;
using Cwc.Integration.OrderImportFormatA.Enums;
using Cwc.Ordering;
using CWC.AutoTests.ObjectBuilder;
using Specflow.Automation.Backend.Helpers;
using Specflow.Automation.Backend.Objects;
using Specflow.Automation.Backend.Utils;
using System;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Specflow.Automation.Backend.Steps
{
    [Binding]
    class ServiceOrderImportFormatASteps
    {
        private readonly ServiceOrderImportFormatAContainer formatAContainer;
        private ServiceOrderImportFormatARow orderLine;
        private Order serviceOrder;
        private ServiceOrderImportFormatAHelper formatAHelper = new ServiceOrderImportFormatAHelper();        
        
        ServiceOrderImportFormatASteps(ServiceOrderImportFormatAContainer container)
        {
            formatAContainer = container;            
        }

        [Given(@"Excel file is created with following header attributes and service date in (.*)")]
        public void GivenExcelFileIsCreatedWithFollowingHeaderAttributesAndServiceDate(DateTime serviceDate, Table table)
        {            
            var headerDictionary = table.ToDictionary();
            formatAContainer.ServiceDate = serviceDate;
            formatAContainer.CompanyName = headerDictionary["Company Name"].ToString();
            formatAContainer.CompanyNumber = Convert.ToInt32(headerDictionary["Company Number"]);
            formatAContainer.ServiceType = headerDictionary["Service Type"].ToString();            
        }

        [Given(@"Row with following attributes is added to created excel file")]
        public void GivenRowWithFollowingAttributesIsAddedToCreatedExcelFile(Table table)
        {            
            formatAContainer.FormatARowList.Add(table.CreateInstance<ServiceOrderImportFormatARow>());
            formatAContainer.Compose().Save();            
        }        
        
        [When(@"Service Order Import Format A Interface processes excel file")]
        public void WhenServiceOrderImportFormatAInterfaceProcesseExcelFile()
        {
            formatAHelper.RunOrderImportFormatAJob();
        }

        [Then(@"System creates service order with correct attributes")]
        public void ThenSystemCreatesServiceOrderWithCorrectAttributes()
        {
            orderLine = formatAContainer.FormatARowList[0];
            serviceOrder = DataFacade.Order.Take(o => o.ServiceDate == formatAContainer.ServiceDate).Build();

            Assert.True(serviceOrder.LocationCode == $"{formatAContainer.CompanyNumber}{orderLine.LocationCode}", "Service order is created with wrong location code.");
            Assert.True(serviceOrder.ServiceTypeCode == formatAContainer.ServiceType, "Service order is created with wrong service type.");
            Assert.True(serviceOrder.GenericStatus == GenericStatus.Confirmed, "Service order is created with wrong status.");
            Assert.True(serviceOrder.BankReference == orderLine.LocationName, "Service order is created with wrong bank reference.");
            Assert.True(serviceOrder.CurrencyCode == "EUR", "Service order is created with wrong currency.");   // implied EUR is the default currency
        }

        [Then(@"System creates service products correctly")]
        public void ThenSystemCreatesServiceProductsCorrectly()
        {
            var serviceOrderProductSar500 = OrderingFacade.SOProductService.LoadSOProductByOrderAndProductCode(serviceOrder.ID, ServiceOrderImportFormatAContainer.Sar500Product.ProductCode, null);
            var serviceOrderProductSar100 = OrderingFacade.SOProductService.LoadSOProductByOrderAndProductCode(serviceOrder.ID, ServiceOrderImportFormatAContainer.Sar100Product.ProductCode, null);
            var serviceOrderProductSar50 = OrderingFacade.SOProductService.LoadSOProductByOrderAndProductCode(serviceOrder.ID, ServiceOrderImportFormatAContainer.Sar50Product.ProductCode, null);
            var serviceOrderProductSar10 = OrderingFacade.SOProductService.LoadSOProductByOrderAndProductCode(serviceOrder.ID, ServiceOrderImportFormatAContainer.Sar10Product.ProductCode, null);
            var serviceOrderProductUsd100 = OrderingFacade.SOProductService.LoadSOProductByOrderAndProductCode(serviceOrder.ID, ServiceOrderImportFormatAContainer.Usd100Product.ProductCode, null);
            Assert.True(ServiceOrderImportFormatAContainer.Sar500Product.Value * orderLine.Sar500 == serviceOrderProductSar500.OrderProductValue, "SOProduct record for SAR500 mapped product is created with wrong value.");
            Assert.True(ServiceOrderImportFormatAContainer.Sar100Product.Value * orderLine.Sar100 == serviceOrderProductSar100.OrderProductValue, "SOProduct record for SAR100 mapped product is created with wrong value.");
            Assert.True(ServiceOrderImportFormatAContainer.Sar50Product.Value * orderLine.Sar50 == serviceOrderProductSar50.OrderProductValue, "SOProduct record for SAR50 mapped product is created with wrong value.");
            Assert.True(ServiceOrderImportFormatAContainer.Sar10Product.Value * orderLine.Sar10 == serviceOrderProductSar10.OrderProductValue, "SOProduct record for SAR10 mapped product is created with wrong value.");
            Assert.True(ServiceOrderImportFormatAContainer.Usd100Product.Value * orderLine.Usd100 == serviceOrderProductUsd100.OrderProductValue, "SOProduct record for USD100 mapped product is created with wrong value.");
        }

        [Then(@"Service Order Import Format A Log record is created with '(.*)' result")]
        public void ThenServiceOrderImportFormatALogRecordIsCreatedWithResult(string result)
        {
            using (var context = new OrderImportFormatADataContext())
            {
                var logRecord = context.OrderImportFormatAJobLog.OrderByDescending(l => l.ID).First();
                switch (result)
                {
                    case "Ok":
                        Assert.True(logRecord.Result == (OrderImportFormatAResult)Enum.Parse(typeof(OrderImportFormatAResult), result));
                        Assert.True(logRecord.Message == "Line 1");
                        Assert.True(logRecord.ServiceOrderID == serviceOrder.IdentityID);
                        break;
                    case "Failed":
                        break;
                    default:
                        throw new Exception("Invalid value of result enum.");
                }

                Assert.True(logRecord.IncomingFileName == Path.Combine(ServiceOrderImportFormatAContainer.FolderPath, ServiceOrderImportFormatAContainer.FileName));                
            }
        }
    }
}
