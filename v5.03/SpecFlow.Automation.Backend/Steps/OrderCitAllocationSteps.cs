using Cwc.BaseData;
using Cwc.Ordering;
using Cwc.Transport.Model;
using CWC.AutoTests.Enums;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using Specflow.Automation.Backend.Helpers;
using Specflow.Automation.Backend.Model;
using Specflow.Automation.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace Specflow.Automation.Backend.Steps
{
    [Binding]
    public class OrderCitAllocationSteps
    {
        ServiceOrderContainer serviceOrderContainer;
        TransportOrder transportOrder;
        OrderCitAllocationHelper helper = new OrderCitAllocationHelper();
        string ServiceOrderId => serviceOrderContainer.ServiceOrder.ID;
        int ServiceOrderServiceTypeId => serviceOrderContainer.ServiceOrder.ServiceType.ID;
        int ServiceOrderOrderType => (int)serviceOrderContainer.ServiceOrder.OrderType;
        DateTime ServiceOrderServiceDate => serviceOrderContainer.ServiceOrder.ServiceDate;

        public OrderCitAllocationSteps(ServiceOrderContainer container)
        {
            serviceOrderContainer = container;
        }


        [Given(@"(\w+) service order with service date in (.*), (.*) and following content is created for (.*)")]
        [Scope(Tag = "cit-order-allocation")]
        public void GivenServiceOrderIsCreated(string serviceType, DateTime serviceDate, GenericStatus status, Location location, Table table)
        {
            var serviceTypeCode = serviceType.ConvertToServiceTypeCode();
            var deliveryContent = table.ToDeliveryContentList();
            serviceOrderContainer.ServiceOrder = HelperFacade.TransportHelper.CreateServiceOrderWithContentLite(serviceTypeCode, serviceDate, status, location, deliveryContent);
        }

        [When(@"CIT Allocation job processes Service Order")]
        public void WhenCITAllocationJobProcessesServiceOrder()
        {
            helper.RunOrderCitAllocation();
        }

        [Then(@"System creates correct Transport Order")]
        public void ThenSystemCreatesCorrectTransportOrder()
        {
            using (var context = new AutomationTransportDataContext())
            {
                transportOrder = context.TransportOrders.AsNoTracking().FirstOrDefault(t => t.ServiceOrderID == ServiceOrderId);

                try
                {
                    Assert.NotNull(transportOrder);
                }
                catch (Xunit.Sdk.NotNullException)
                {
                    var logRecord = context.OrderCITAllocationLogs.OrderByDescending(x => x.ID).AsNoTracking().FirstOrDefault(x => x.ServiceOrderID == ServiceOrderId);
                    throw new Exception(logRecord != null ? logRecord.Message : "Expected transport order was not created.");
                }

                Assert.True(transportOrder.ServiceTypeID == ServiceOrderServiceTypeId, "Transport Order servise type is not match Service Order service type");
                Assert.True((int)transportOrder.OrderType == ServiceOrderOrderType, "Transport Order order type is not match Service Order order type");
                Assert.True(transportOrder.TransportDate == ServiceOrderServiceDate, "Transport Order transport date is not match Service Order service date");
                //add other assertions for the transport order
            }
        }

        [Then(@"System creates Transport Order for (.*)  with (.*) service type")]
        public void ThenSystemCreatesCorrectTransportOrderForLocationAndServiceType(Location location, string serviceType)
        {
            using (var context = new AutomationTransportDataContext())
            {
                transportOrder = context.TransportOrders.AsNoTracking().FirstOrDefault(t => t.ServiceOrderID == ServiceOrderId);

                try
                {
                    Assert.NotNull(transportOrder);
                }
                catch (Xunit.Sdk.NotNullException)
                {
                    var logRecord = context.OrderCITAllocationLogs.OrderByDescending(x => x.ID).AsNoTracking().FirstOrDefault(x => x.ServiceOrderID == ServiceOrderId);
                    throw new Exception(logRecord != null ? logRecord.Message : "Expected transport order was not created.");
                }

                Assert.Equal(ServiceOrderId, transportOrder.ServiceOrderID);
                Assert.Equal(location.ID, transportOrder.LocationID);
            }
        }

        [Then(@"System creates correct Transport Order products")]
        public void ThenSystemCreatesCorrectTransportOrderProducts()
        {
            var serviceOrderProductIdList = new List<int>();
            var transportOrderProductIdList = new List<int>();

            using (var context = new AutomationOrderingDataContext())
            {

                var orderLineId = ServiceOrderId + "-1";
                var serviceOrderProductCodeList = context.SOProduct.AsNoTracking().Where(x => x.OrderLine_ID == orderLineId && x.IsLoose == true).Select(x => x.ProductCode).ToList();
                try
                {
                    Assert.NotNull(serviceOrderProductCodeList);
                }
                catch (Xunit.Sdk.NotNullException)
                {
                    throw new Exception("SOproducts with IsLoose = 'yes' are not found in database. Please check product configuration 'IsLoose = true' in the feature file.");
                }

                using (var baseContext = new AutomationBaseDataContext())
                {
                    foreach (var code in serviceOrderProductCodeList)
                    {
                        var id = baseContext.Products.Where(x => x.ProductCode == code).FirstOrDefault().ID;
                        serviceOrderProductIdList.Add(id);
                    }
                }
            }
            using (var context = new AutomationTransportDataContext())
            {
                transportOrderProductIdList = context.TransportOrderProducts.AsNoTracking().Where(x => x.TransportOrderID == transportOrder.ID).Select(t => t.ProductID).ToList();
                try
                {
                    Assert.NotNull(transportOrderProductIdList);
                }
                catch (Xunit.Sdk.NotNullException)
                {
                    throw new Exception("Transport Order Products are not found in database. Please check product configuration 'IsLoose = true' in the feature file.");
                }

                Assert.Equal(serviceOrderProductIdList.Count, transportOrderProductIdList.Count);
                foreach (var id in serviceOrderProductIdList)
                {
                    Assert.Contains(id, transportOrderProductIdList);
                    //add other assertions for the transport order products
                }
            }
        }

        [Then(@"System creates correct Transport Order products")]
        public void ThenSystemCreatesCorrectTransportOrderProducts(Table table)
        {
            var transportOrderProductIdList = new List<int>();
            var deliveryContent = table.ToDeliveryContentList();

            using (var context = new AutomationTransportDataContext())
            {
                transportOrderProductIdList = context.TransportOrderProducts.AsNoTracking().Where(x => x.TransportOrderID == transportOrder.ID).Select(t => t.ProductID).ToList();
                try
                {
                    Assert.NotNull(transportOrderProductIdList);
                }
                catch (Xunit.Sdk.NotNullException)
                {
                    throw new Exception("Transport Order Products are not found in database. Please check product configuration 'IsLoose = true' in the feature file.");
                }

                Assert.Equal(transportOrder.ServiceOrderID, ServiceOrderId);
                Assert.Equal(deliveryContent.Count, transportOrderProductIdList.Count);
                foreach (var content in deliveryContent)
                {
                    var transportOrderProductQuantiy = context.TransportOrderProducts.AsNoTracking().Where(x => x.ProductID == content.Product.ID).FirstOrDefault().OrderedQuantity;
                    Assert.Contains(content.Product.ID, transportOrderProductIdList);
                    Assert.Equal(content.Quantity, transportOrderProductQuantiy);
                }
            }
        }

        [Then(@"System creates successful Order CIT Allocation Job Log record with result OK for current Transport Order")]
        public void ThenSystemCreatesOrderCITAllocationJobLogRecordWithServiceOrderNumberTransportOrderNumberCITSiteCode()
        {
            using (var context = new AutomationTransportDataContext())
            {
                var log = context.OrderCITAllocationLogs.OrderByDescending(x => x.DateCreated).FirstOrDefault();
                Assert.Equal(ServiceOrderId, log.ServiceOrderID);
                Assert.Equal(transportOrder.ID, log.TransportOrderID);
                Assert.Equal(0, (int)log.Result);
            }
        }
    }
}
