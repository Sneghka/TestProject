using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.CashCenter.Classes.Logs;
using Cwc.Ordering;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using Specflow.Automation.Backend.Helpers;
using Specflow.Automation.Backend.Model;
using Specflow.Automation.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace Specflow.Automation.Backend.Steps
{
    [Binding]
    public class CashCenterOrderAllocationSteps
    {        
        StockOrder stockOrder; // used when single stock order should be created for the service order
        List<StockOrder> stockOrderList; // used when multiple stock orders should be created for the same service order
        List<OrderAllocationLog> logRecordList;
        ServiceOrderContainer serviceOrderContainer;
        CashCenterOrderAllocationHelper helper = new CashCenterOrderAllocationHelper();
        string ServiceOrderId => serviceOrderContainer.ServiceOrder.ID;

        public CashCenterOrderAllocationSteps(ServiceOrderContainer container)
        {
            serviceOrderContainer = container;
        }

        [When(@"CC Orders Allocation job processes service order")]
        public void WhenCCOrdersAllocationJobProcessesServiceOrder()
        {
            helper.RunCashCenterOrderAllocation();
        }

        [Then(@"System creates outbound order for (.*) and (.*)")]
        public void ThenSystemCreatesOutboundOrderForCashCenterAndLocation(Site cashCenter, Location location)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                stockOrder = context.StockOrders.AsNoTracking().FirstOrDefault(x => x.ServiceOrder_id == ServiceOrderId);
                
                try
                {
                    Assert.NotNull(stockOrder);
                }
                catch (Xunit.Sdk.NotNullException)
                {
                    var logRecord = context.OrderAllocationLogs.AsNoTracking().OrderByDescending(x => x.ID).FirstOrDefault(x => x.ServiceOrderID == ServiceOrderId);
                    throw new Exception(logRecord != null ? logRecord.Message : "Expected stock order was not created.");
                }
                
                Assert.True(cashCenter.ID == stockOrder.Site_id, "Outbound order has unexpected Site.");
                Assert.True(location.ID == stockOrder.LocationTo_id, "Outbound order has unexpected Location To.");
            }
        }

        [Then(@"System creates (.*) outbound orders for current service order")]
        public void ThenSystemCreatesOutboundOrdersForCurrentServiceOrder(int number)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                stockOrderList = context.StockOrders.AsNoTracking().Where(x => x.ServiceOrder_id == ServiceOrderId).ToList();

                Assert.True(stockOrderList.Count == number, $"Unexpected number of outbound orders were created for {ServiceOrderId} service order.");
            }
        }

        [Then(@"System creates (\d+) CC Orders Allocation Log record(?:s)? with '(\w+)' result for current service order")]
        public void ThenCCOrdersAllocationLogRecordIsCreated(int number, string result)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                logRecordList = context.OrderAllocationLogs.Where(x => x.ServiceOrderID == ServiceOrderId).ToList();
                var count = logRecordList.Where(x => x.Result == (OrderAllocationResult)Enum.Parse(typeof(OrderAllocationResult), result)).Count();

                Assert.True(number == count, $"Unexpected number of CC Orders Allocation Log records with '{result}' result were created.");                           
            }
        }

        [Then(@"CC Orders Allocation Log record got following message")]
        public void ThenCCOrdersAllocationLogRecordGotFollowingMessage(Table table)
        {
            var unformattedMessage = table.Rows[0]["Message"];
            var expectedMessage = String.Format(unformattedMessage, ServiceOrderId);
            List<OrderAllocationLog> list;

            if (logRecordList != null)
            {
                list = logRecordList;
            }
            else
            {
                using (var context = new AutomationCashCenterDataContext())
                {
                    list = context.OrderAllocationLogs.Where(x => x.ServiceOrderID == ServiceOrderId).ToList();
                }
            }   

            Assert.True(list.Any(x => x.Message == expectedMessage), $"There is no CC Orders Allocation Log record with message = {expectedMessage}");
        }

        [Then(@"Outbound order got correct quantity, value and weight")]
        public void ThenOutboundOrderGotCorrectQuantityValueAndWeight(Table table)
        {
            var dict = table.ToDictionary();
            
            Assert.True(stockOrder.TotalQuantity == Convert.ToInt32(dict["Quantity"]));
            Assert.True(stockOrder.TotalValue == Convert.ToDecimal(dict["Value"]));
            Assert.True(stockOrder.TotalWeight == Convert.ToDecimal(dict["Weight"]));
        }

        [Then(@"Outbound order for (.*) and (.*) products got correct quantity, value and weight")]
        public void ThenOutboundOrderForCashCenterGotCorrectQuantityValueAndWeight(Site cashCenter, string type, Table table)
        {
            var stockOrderType = type.ConvertToStockOrderType();
            var dict = table.ToDictionary();
            var stockOrder = stockOrderList.FirstOrDefault(x => x.Site_id == cashCenter.ID && x.Type == stockOrderType);

            try
            {
                Assert.NotNull(stockOrder);
            }
            catch (Xunit.Sdk.NotNullException)
            {                
                throw new Exception($"Expected stock order for '{cashCenter.Branch_cd}' site and '{type}' products type was not created.");
            }

            Assert.True(stockOrder.TotalQuantity == Convert.ToInt32(dict["Quantity"]));
            Assert.True(stockOrder.TotalValue == Convert.ToDecimal(dict["Value"]));
            Assert.True(stockOrder.TotalWeight == Convert.ToDecimal(dict["Weight"]));
        }
    }
}
