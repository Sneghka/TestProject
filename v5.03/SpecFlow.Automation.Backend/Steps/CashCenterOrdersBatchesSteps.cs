using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.Common;
using Cwc.Ordering;
using Cwc.Security;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using Specflow.Automation.Backend.Hooks;
using Specflow.Automation.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace Specflow.Automation.Backend.Steps
{
    [Binding]
    public sealed class CashCenterOrdersBatchesSteps
    {
        DateTime today = DateTime.Today;        
        List<StockOrder> stockOrderList = new List<StockOrder>();
        OrdersBatch batch;
        Result linkageResult;
        Result validationResult;
        UserParams userParams = new UserParams(SecurityFacade.LoginService.GetAdministratorLogin());

        long[] StockOrderIds => stockOrderList.Select(x => x.ID).ToArray();
        string BatchDescription { get; } = DateTime.Now.ToString("ddMMyyyyHHmmssfff");

        [Given(@"Stock order with '(.*)' type is created for (.*)")]
        public void GivenStockOrderWithTypeIsCreatedForLocation(string type, Location location)
        {
            try
            {
                using (var context = new AutomationCashCenterDataContext())
                {
                    var stockOrderType = type.ConvertToStockOrderType();
                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrderWithContentLite("DELV", today, GenericStatus.Registered, location, new List<DeliveryProductSpecification>());
                    var stockOrder = new StockOrder
                    {
                        ReferenceNumber = serviceOrder.ID,
                        ServiceDate = serviceOrder.ServiceDate,
                        ServiceType_id = CashCenterDataConfigurationHooks.OutboundServiceType.ID,
                        Site_id = location.NotesSiteID != null ? location.NotesSiteID.Value : location.CoinsSiteID.Value,
                        LocationTo_id = location.ID,
                        ServiceOrder_id = serviceOrder.ID,
                        CustomerID = location.CompanyID
                    };

                    stockOrder.InitDefault();
                    stockOrder.SetNumber(DateTime.Now.ToString("ddMMyyyyHHmmssfff"));
                    stockOrder.SetType(stockOrderType);
                    context.StockOrders.Add(stockOrder);
                    context.SaveChanges();

                    stockOrderList.Add(context.StockOrders.AsNoTracking().First(x => x.ServiceOrder_id == serviceOrder.ID));
                }
            }
            catch
            {
                throw;
            }
        }

        [Given(@"Service order of created stock order is not yet replicated")]
        public void GivenServiceOrderOfCreatedStockOrderIsNotYetReplicated()
        {
            /// delete service order from the db to emulate unfinished replication from HQ to local CC
            using (var context = new AutomationOrderingDataContext())
            {
                var serviceOrderId = stockOrderList.First().ServiceOrder_id;
                var serviceOrder = context.Orders.Single(x => x.ID == serviceOrderId);
                context.Orders.Remove(serviceOrder);
                context.SaveChanges();
            }
        }

        [Given(@"Servicing CIT depot of (.*) is set to (.*)")]
        [Then (@"Servicing CIT depot of (.*) is set to (.*)")]
        public void GivenServicingCITDepotOfLocationIsSetToCITDepotForNotesCC(Location location, Site site)
        {
            location.ServicingDepotID = site.ID;
            BaseDataConfigurationHooks.LocationDict[location.Code] = location;
        }

        [Given(@"Stock order for (.*) has 'in progress' status")]
        public void GivenStockOrderForLocationHasInProgressStatus(Location location)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                var inMemoryOrder = stockOrderList.Single(x => x.LocationTo_id == location.ID);
                var dbOrder = context.StockOrders.Single(x => x.ID == inMemoryOrder.ID);

                dbOrder.SetStatus(StockOrderStatus.InProgress);
                context.SaveChanges();
            }
        }


        [Given(@"Stock orders for the next locations are linked to (\w+) orders batch")]
        [When(@"User links stock orders for the next locations to (\w+) orders batch")]
        public void WhenUserLinksStockOrdersForTheNextLocationsToOrdersBatch(bool isNew, Table table)
        {
            List<Location> locationList = table.ToLocationList();
            var stockOrderIds = stockOrderList.Where(x => locationList.Any(l => l.ID == x.LocationTo_id)).Select(x => x.ID).ToArray();

            /// linkage to new orders batch
            if (isNew)
            {
                var result = CashCenterFacade.OrdersBatchService.LinkMultipleStockOrdersToOrdersBatch(stockOrderIds, null, BatchDescription, userParams, null);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Unexpected error on linking stock orders to orders batch. Reason: {result.GetMessage()}");
                }
                
                using (var context = new AutomationCashCenterDataContext())
                {
                    /// define orders batch to use in later steps
                    batch = context.OrdersBatches.SingleOrDefault(x => x.Description == BatchDescription);
                    if (batch == null)
                    {
                        throw new EntryPointNotFoundException("Expected batch is not found in the database.");
                    }
                }
            }
            /// linkage to existing orders batch
            else
            {
                linkageResult = CashCenterFacade.OrdersBatchService.LinkMultipleStockOrdersToOrdersBatch(stockOrderIds, batch.ID, BatchDescription, userParams, null);
            }
        }

        [When(@"User validates linkage of created stock order(?:s)?")]
        public void WhenUserValidatesLinkageOfCreatedStockOrders()
        {
            try
            {
                validationResult = CashCenterFacade.OrdersBatchService.ValidateLinkageOfStockOrdersToOrdersBatch(StockOrderIds, userParams, null);
            }
            catch
            {
                throw;
            }
        }        

        [Then(@"Validation is successful")]
        public void ThenValidationIsSuccessful()
        {
            Assert.True(validationResult.IsSuccess, $"Validation failed. Reason: {validationResult.Messages[0]}");
        }

        [Then(@"Validation is failed")]
        public void ThenValidationIsFailed(Table table)
        {
            if (validationResult.IsSuccess || !validationResult.Messages.Any())
            {
                throw new InvalidOperationException("Expected error message is not displayed.");
            }

            var actualMessage = validationResult.Messages[0];
            Assert.True(table.Rows[0]["Message"] == actualMessage, $"Unexpected error message is displayed: {actualMessage}");
        }

        [Then(@"Validation is failed with parameter")]
        public void ThenValidationIsFailedWithParameter(Table table)
        {
            var unformattedMessage = table.Rows[0]["Message"];
            var expectedMessage = String.Format(unformattedMessage, String.Join("\n", stockOrderList.Select(x => x.Number).ToArray()));
            var actualMessage = validationResult.Messages[0];

            Assert.True(expectedMessage == actualMessage, $"Unexpected error message is displayed: {actualMessage}");
        }

        [Then(@"Linkage is failed")]
        public void ThenLinkageIsFailed(Table table)
        {
            if (linkageResult.IsSuccess || !linkageResult.Messages.Any())
            {
                throw new InvalidOperationException("Expected error message is not displayed.");
            }

            var actualMessage = linkageResult.Messages[0];
            Assert.True(table.Rows[0]["Message"] == actualMessage, $"Unexpected error message is displayed: {actualMessage}");
        }

        [Then(@"System links stock orders for the next locations to the orders batch")]
        public void ThenSystemLinksStockOrdersForTheNextLocationsToTheOrdersBatch(Table table)
        {
            /// case of the linkage to existing orders batch
            if (linkageResult != null)
            {
                Assert.True(linkageResult.IsSuccess, $"Unexpected error on linking stock orders to orders batch.Reason: {linkageResult.GetMessage()}");
            }

            List<Location> locationList = table.ToLocationList();
            var stockOrderIds = stockOrderList.Where(x => locationList.Any(l => l.ID == x.LocationTo_id)).Select(x => x.ID).ToArray();

            using (var context = new AutomationCashCenterDataContext())
            {
                foreach (var id in stockOrderIds)
                {
                    var stockOrder = context.StockOrders.First(x => x.ID == id);
                    Assert.True(stockOrder.OrdersBatchId == batch.ID, $"Stock order '{stockOrder.Number}' is not linked to the expected orders batch.");
                }
            }
        }
    }
}
