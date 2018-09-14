using Cwc.Ordering;
using Cwc.Transport.Enums;
using CWC.AutoTests.Enums;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.Tests.BasicImport;
using CWC.AutoTests.Tests.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests
{
    public class OrderCitAllocationTests : IClassFixture<OrderCitAllocationTestFixture>, IClassFixture<BasicImportFixture>, IDisposable
    {
        OrderCitAllocationTestFixture orderCitAllocationTestFixture;
        BasicImportFixture basicImportFixture;
        DateTime today = DateTime.Today;

        public OrderCitAllocationTests(OrderCitAllocationTestFixture setupFixture1, BasicImportFixture setupFixture2)
        {
            // Add code to execute before each test
            orderCitAllocationTestFixture = setupFixture1;
            basicImportFixture = setupFixture2;
        }

        public void Dispose()
        {
            HelperFacade.TransportHelper.ClearTestData();
        }
        
        #region CREATE tests

        [Theory(DisplayName = "Order CIT Allocation - Allocate valid 'delivery' service order")]    // test case 7812 
        [InlineData(GenericStatus.Unconfirmed, "DELV", 0)]
        [InlineData(GenericStatus.Confirmed, "DELV", 0)]
        [InlineData(GenericStatus.Registered, "DELV", 0)]
        [InlineData(GenericStatus.Unconfirmed, "DELV", 1)]
        [InlineData(GenericStatus.Confirmed, "DELV", 1)]
        [InlineData(GenericStatus.Registered, "DELV", 1)]
        public void AllocateValidDeliveryServiceOrder(GenericStatus status, string serviceType, int leadTime)
        {
            try
            {
                HelperFacade.ContractHelper.SetLeadTimeSettings(orderCitAllocationTestFixture.ExternalLocation, serviceType, "EUR", leadTime);
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: false);            
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);
                var logRecord = HelperFacade.TransportHelper.FindOrderCitAllocationLogRecord(serviceOrder.ID);
                     
                Assert.Equal(TransportOrderStatus.Registered, transportOrder.Status);
                Assert.Equal(transportOrder.ServiceTypeID, DataFacade.ServiceType.Take(st => st.Code == serviceType).Build().ID);
                Assert.Equal(serviceOrder.ReferenceID, transportOrder.ReferenceCode);
                Assert.Equal(serviceOrder.OrderType, transportOrder.OrderType);
                Assert.Equal(serviceOrder.ServiceDate, transportOrder.TransportDate);
                Assert.Equal(serviceOrder.ServiceDate, transportOrder.ServiceDate);
                Assert.Equal(serviceOrder.BranchCode, DataFacade.Site.Take(s => s.ID == transportOrder.SiteID).Build().Branch_cd);
                Assert.Equal(serviceOrder.LocationID.ToString(), transportOrder.LocationID.ToString());
                Assert.False(transportOrder.IsWithException);
                Assert.False(transportOrder.IsPdaAdHoc);
                Assert.True(transportOrder.IsBillable);
                Assert.Equal(2, transportOrderProducts.Count());
                Assert.Equal(serviceOrder.OrderedValue, transportOrderProducts.Sum(p => p.OrderedValue));
                Assert.True(transportOrderProducts.All(p => p.CurrencyID == "EUR"));
                Assert.Equal(OrderCitAllocationResult.Ok, logRecord.Result);
                Assert.True(logRecord.Message == null);
                Assert.Equal(serviceOrder.ID, logRecord.ServiceOrderID);
                Assert.Equal(transportOrder.ID, logRecord.TransportOrderID);
            }
            catch
            {
                throw;
            }
        }
        
        [Theory(DisplayName = "Order CIT Allocation - Allocate valid 'replenishment' service order")]   // test case 7812
        [InlineData(GenericStatus.Unconfirmed, "REPL")]
        [InlineData(GenericStatus.Confirmed, "REPL")]
        [InlineData(GenericStatus.Registered, "REPL")]
        public void AllocateValidReplenishmentServiceOrder(GenericStatus status, string serviceType)
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: true);            
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);
                var transportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(transportOrder.ID);

                Assert.True(transportOrder.IsBillable);
                Assert.Equal(2, transportOrderProducts.Count());
                Assert.Equal(serviceOrder.OrderedValue, transportOrderProducts.Sum(p => p.OrderedValue));                
                Assert.Equal(2, transportOrderServices.Count());
                Assert.True(transportOrderServices.All(s => s.IsPlanned));
                Assert.False(transportOrderServices.All(s => s.IsPerformed));
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Order CIT Allocation - Allocate valid 'collect' service order")]     // test case 7812
        [InlineData(GenericStatus.Unconfirmed, "COLL")]
        [InlineData(GenericStatus.Confirmed, "COLL")]
        [InlineData(GenericStatus.Registered, "COLL")]
        public void AllocateValidCollectServiceOrder(GenericStatus status, string serviceType)
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                
                Assert.NotNull(transportOrder);                               
                Assert.True(transportOrder.IsBillable);
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Order CIT Allocation - Allocate valid 'servicing' service order")]   // test case 7812
        [InlineData(GenericStatus.Unconfirmed, "SERV")]
        [InlineData(GenericStatus.Confirmed, "SERV")]
        [InlineData(GenericStatus.Registered, "SERV")]
        public void AllocateValidServicingServiceOrder(GenericStatus status, string serviceType)
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(transportOrder.ID);            
                
                Assert.NotNull(transportOrder);
                Assert.True(transportOrder.IsBillable);
                Assert.Equal(2, transportOrderServices.Count());
                Assert.True(transportOrderServices.All(s => s.IsPlanned));
                Assert.False(transportOrderServices.All(s => s.IsPerformed));
            }
            catch
            {
                throw;
            }
        }
        
        [Theory(DisplayName = "Order CIT Allocation - Allocate service order with service date in the past")]    // test case 7815-1
        [InlineData(GenericStatus.Unconfirmed, "DELV")]
        [InlineData(GenericStatus.Confirmed, "DELV")]
        [InlineData(GenericStatus.Registered, "DELV")]
        [InlineData(GenericStatus.Unconfirmed, "REPL")]
        [InlineData(GenericStatus.Confirmed, "REPL")]
        [InlineData(GenericStatus.Registered, "REPL")]
        [InlineData(GenericStatus.Unconfirmed, "COLL")]
        [InlineData(GenericStatus.Confirmed, "COLL")]
        [InlineData(GenericStatus.Registered, "COLL")]
        [InlineData(GenericStatus.Unconfirmed, "SERV")]
        [InlineData(GenericStatus.Confirmed, "SERV")]
        [InlineData(GenericStatus.Registered, "SERV")]
        public void AllocateServiceOrderWithServiceDateInThePast(GenericStatus status, string serviceType)
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(serviceOrder.ID, DateTime.Now.AddDays(-1));
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                Assert.Null(transportOrder);    // transport order should not be created for service order with invalid service date (service date in the past)                
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Order CIT Allocation - Allocate 'canceled' service order")]    // test case 7815-2
        [InlineData(GenericStatus.Unconfirmed, "DELV")]
        [InlineData(GenericStatus.Unconfirmed, "REPL")]
        [InlineData(GenericStatus.Unconfirmed, "COLL")]
        [InlineData(GenericStatus.Unconfirmed, "SERV")]
        public void AllocateCanceledServiceOrder(GenericStatus status, string serviceType)
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Cancelled);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);            
                Assert.Null(transportOrder); // transport order should not be created for service order with 'canceled' generic status
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Order CIT Allocation - Allocate 'completed' service order")]    // test case 7815-3
        [InlineData(GenericStatus.Unconfirmed, "DELV")]
        [InlineData(GenericStatus.Unconfirmed, "REPL")]
        [InlineData(GenericStatus.Unconfirmed, "COLL")]
        [InlineData(GenericStatus.Unconfirmed, "SERV")]
        public void AllocateCompletedServiceOrder(GenericStatus status, string serviceType)
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);            
                Assert.Null(transportOrder); // transport order should not be created for service order with 'completed' generic status
            }
            catch
            {
                throw;
            }
        }
        
        [Theory(DisplayName = "Order CIT Allocation - Allocate service order with external CIT depot")]    // test case 7815-5
        [InlineData(GenericStatus.Unconfirmed, "DELV")]
        [InlineData(GenericStatus.Confirmed, "DELV")]
        [InlineData(GenericStatus.Registered, "DELV")]
        [InlineData(GenericStatus.Unconfirmed, "REPL")]
        [InlineData(GenericStatus.Confirmed, "REPL")]
        [InlineData(GenericStatus.Registered, "REPL")]
        [InlineData(GenericStatus.Unconfirmed, "COLL")]
        [InlineData(GenericStatus.Confirmed, "COLL")]
        [InlineData(GenericStatus.Registered, "COLL")]
        [InlineData(GenericStatus.Unconfirmed, "SERV")]
        [InlineData(GenericStatus.Confirmed, "SERV")]
        [InlineData(GenericStatus.Registered, "SERV")]
        public void AllocateServiceOrderWithExternalCitDepot(GenericStatus status, string serviceType)
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalDepotLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();            
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var logRecord = HelperFacade.TransportHelper.FindOrderCitAllocationLogRecord(serviceOrder.ID);            
                Assert.Null(transportOrder);    // transport order should not be created for service order that has location with external CIT depot
                Assert.Equal(OrderCitAllocationResult.Failed, logRecord.Result);
                Assert.Equal(String.Format("Location is serviced by external CIT Site on {0}.", serviceOrder.ServiceDate.DayOfWeek), logRecord.Message);                
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Order CIT Allocation - Allocate service order with barcoded products")]    
        [InlineData(GenericStatus.Unconfirmed, "DELV")]
        [InlineData(GenericStatus.Unconfirmed, "REPL")]
        public void AllocateServiceOrderWithBarcodedProducts(GenericStatus status, string serviceType)
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: false, withServices: false, withBarcodedProducts: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);            
                Assert.Equal(1, transportOrderProducts.Count);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Order CIT Allocation - Allocate service order with empty sites for normal location")]    // SC1698 (OUTDATED test case 7818-1)
        public void AllocateServiceOrderWithEmptySitesForNormalLocation()
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Unconfirmed, "DELV", orderCitAllocationTestFixture.LocationCase1, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);            
                Assert.Equal(2, transportOrderProducts.Count);                
            }
            catch 
            {
                throw;
            }
        }

        [Fact(DisplayName = "Order CIT Allocation - Allocate service order with internal sites for normal location")]    //SC1698 (OUTDATED test case 7818-2)
        public void AllocateServiceOrderWithInternalSitesForNormalLocation()
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Unconfirmed, "DELV", orderCitAllocationTestFixture.LocationCase2, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);            
                Assert.Equal(2, transportOrderProducts.Count);                
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Order CIT Allocation - Allocate service order with internal notes site and empty coins site for normal location")]    // SC1698 (OUTDATED test case 7818-3)
        public void AllocateServiceOrderWithInternalNotesSiteAndEmptyCoinSiteForNormalLocation()
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Unconfirmed, "DELV", orderCitAllocationTestFixture.LocationCase3, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);            
                Assert.Equal(2, transportOrderProducts.Count);
                Assert.NotNull(transportOrder);
            }
            catch 
            {
                throw;
            }
        }

        [Fact(DisplayName = "Order CIT Allocation - Allocate service order with internal coins site and empty notes site for normal location")]    // SC1698 (OUTDATED test case 7818-4)
        public void AllocateServiceOrderWithInternalCoinsSiteAndEmptyNoteSiteForNormalLocation()
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Unconfirmed, "DELV", orderCitAllocationTestFixture.LocationCase4, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);            
                Assert.Equal(2, transportOrderProducts.Count);                
            }
            catch 
            {
                throw;
            }
        }

        [Fact(DisplayName = "Order CIT Allocation - Allocate service order with internal sites for bank location")]    // test case 7818-5
        public void AllocateServiceOrderWithInternalSitesForBankLocation()
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Unconfirmed, "DELV", orderCitAllocationTestFixture.LocationCase5, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);
                Assert.Equal(2, transportOrderProducts.Count);    // transport order products should be created when Location -> Notes Site -> Is External = "no", Location -> Coins Site -> Is External = "no", Location -> Location type -> Category == 'Bank'.
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Order CIT Allocation - Allocate service order with empty sites for bank location")]    // test case 7818-6
        public void AllocateServiceOrderWithEmptySitesForBankLocation()
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Unconfirmed, "DELV", orderCitAllocationTestFixture.LocationCase6, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);            
                Assert.Equal(2, transportOrderProducts.Count);    // transport order products should be created when Location -> Notes Site is empty, Location -> Coins Site -> is empty, Location -> Location type -> Category == 'Bank'.
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Order CIT Allocation - Allocate service order with external sites for bank location")]    // test case 7818-7
        public void AllocateServiceOrderWithExternalSitesForBankLocation()
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Unconfirmed, "DELV", orderCitAllocationTestFixture.LocationCase7, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);            
                Assert.Equal(2, transportOrderProducts.Count);    // transport order products should be created when Location -> Notes Site -> Is External = "yes", Location -> Coins Site -> Is External = "yes", Location -> Location type -> Category == 'Bank'.
            }
            catch
            {
               throw;
            }
        }

        [Fact(DisplayName = "Order CIT Allocation - Allocate service order with external sites for normal location")]    // test case 7818-8
        public void AllocateServiceOrderWithExternalSitesForNormalLocation()
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Unconfirmed, "DELV", orderCitAllocationTestFixture.LocationCase8, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);            
                Assert.Equal(2, transportOrderProducts.Count);    // transport order products should be created when Location -> Notes Site -> Is External = "yes", Location -> Coins Site -> Is External = "yes", Location -> Location type -> Category != 'Bank'.
            }
            catch 
            {
                throw;
            }
        }

        [Fact(DisplayName = "Order CIT Allocation - Allocate 'collect' service order with products")]    // SC1698
        public void AllocateCollectServiceOrderWithProducts()
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Unconfirmed, "COLL", orderCitAllocationTestFixture.LocationCase5, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);            
                Assert.Equal(2, transportOrderProducts.Count);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Order CIT Allocation - Allocate 'servicing' service order with products")]    //SC1698 (OUTDATED test case 7818-10)
        public void AllocateServicingServiceOrderWithProducts()
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Unconfirmed, "SERV", orderCitAllocationTestFixture.LocationCase5, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var transportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(transportOrder.ID);            
                Assert.Equal(2, transportOrderProducts.Count);
            }
            catch 
            {
                throw;
            }
        }

        [Fact(DisplayName = "Order CIT Allocation - Allocate service order without order line")]
        public void AllocateServiceOrderWithoutOrderLine()
        {
            try
            {
                string orderId = "JG" + DateTime.Now.ToString("ddMMyyyy");
                string tableName = String.Concat("dbo.", BasicImportEntity.ServiceOrder);
                var idColumnData = new Dictionary<string, object> { { "Order_ID", orderId } };
                var content = String.Format(
                    @"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?> 
                        <DocumentElement>
                            <ServiceOrder act='0'>
                            <WP_branch_cd>{5}</WP_branch_cd>
                            <Order_ID>{0}</Order_ID>
                            <Cus_nr>{4}</Cus_nr>
                            <Service_Date>{1}</Service_Date>
                            <Order_Status>REGISTERED</Order_Status>
                            <Order_Type>2</Order_Type>
                            <Order_Level>1</Order_Level>
                            <WP_loc_nr>{2}</WP_loc_nr>
                            <WP_ref_loc_nr>{3}</WP_ref_loc_nr>
                            <WP_CurrencyCode>EUR</WP_CurrencyCode>                                                
                            <WP_ServiceType_Code>{6}</WP_ServiceType_Code>
                            <WP_DateCreated>{1}</WP_DateCreated>
                            </ServiceOrder>
                        </DocumentElement>", 
                    orderId, DateTime.Today.ToString("yyyy-MM-dd 00:00:00"),
                    orderCitAllocationTestFixture.ExternalLocation.ID, orderCitAllocationTestFixture.ExternalLocation.Code,
                    orderCitAllocationTestFixture.ExternalLocation.CompanyID,
                    DataFacade.Site.Take(s => s.ID == orderCitAllocationTestFixture.ExternalLocation.ServicingDepotID).Build().Branch_cd,
                    "COLL");
                var basicImportHelper = new BasicImportHelper(basicImportFixture.FolderPath);
                basicImportHelper.SaveXml(BasicImportEntity.ServiceOrder, content);
                basicImportHelper.Import();
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(orderId);
                var logRecord = HelperFacade.TransportHelper.FindOrderCitAllocationLogRecord(orderId);
                
                Assert.Null(transportOrder);    // transport order should not be created for service order without order line
                Assert.Null(logRecord);         // log record should not be created                
            }
            catch
            {
                throw;
            }
        }
        #endregion
            
        #region UPDATE tests 
            
        [Theory(DisplayName = "Order CIT Allocation - Update 'delivery' transport order")]    // test case 7813 
        [InlineData(GenericStatus.Unconfirmed, "DELV", 0)]
        [InlineData(GenericStatus.Confirmed, "DELV", 0)]
        //[InlineData(GenericStatus.Registered, "DELV", 0)] TO DO - gap in SD regarding 'registered' status
        [InlineData(GenericStatus.Unconfirmed, "DELV", 1)]
        [InlineData(GenericStatus.Confirmed, "DELV", 1)]
        //[InlineData(GenericStatus.Registered, "DELV", 1)] TO DO - gap in SD regarding 'registered' status
        public void UpdateDeliveryTransportOrder(GenericStatus status, string serviceType, int leadTime)
        {
            try
            {
                HelperFacade.ContractHelper.SetLeadTimeSettings(orderCitAllocationTestFixture.ExternalLocation, serviceType, "EUR", leadTime);
                var newServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: false);           
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var newTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(newServiceOrder.ID);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(newTransportOrder, TransportOrderStatus.Planned);               
                HelperFacade.TransportHelper.UpdateServiceOrder(newServiceOrder, withProducts: true, withServices: false);                
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var updatedServiceOrder = DataFacade.Order.Take(o => o.WPOrderID == newServiceOrder.WPOrderID).Build();
                var updatedTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(updatedServiceOrder.ID);
                var updatedTransportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(updatedTransportOrder.ID);
                var logRecord = HelperFacade.TransportHelper.FindOrderCitAllocationLogRecord(updatedServiceOrder.ID);

                Assert.Equal(newTransportOrder.ID, updatedTransportOrder.ID);
                Assert.Equal(TransportOrderStatus.Planned, updatedTransportOrder.Status);
                Assert.Equal(DataFacade.ServiceType.Take(st => st.Code == serviceType).Build().ID, updatedTransportOrder.ServiceTypeID);
                Assert.Equal(updatedServiceOrder.ReferenceID, updatedTransportOrder.ReferenceCode);
                Assert.Equal(updatedServiceOrder.OrderType, updatedTransportOrder.OrderType);
                Assert.Equal(updatedServiceOrder.ServiceDate, updatedTransportOrder.TransportDate);
                Assert.Equal(updatedServiceOrder.ServiceDate, updatedTransportOrder.ServiceDate);
                Assert.Equal(updatedServiceOrder.BranchCode, DataFacade.Site.Take(s => s.ID == updatedTransportOrder.SiteID).Build().Branch_cd);
                Assert.Equal(updatedServiceOrder.LocationID, updatedTransportOrder.LocationID);
                Assert.False(updatedTransportOrder.IsWithException);
                Assert.False(updatedTransportOrder.IsPdaAdHoc);
                Assert.True(updatedTransportOrder.IsBillable);
                Assert.Equal(2, updatedTransportOrderProducts.Count());
                Assert.Equal(updatedServiceOrder.OrderedValue, updatedTransportOrderProducts.Sum(p => p.OrderedValue));
                Assert.Equal(OrderCitAllocationResult.Warning, logRecord.Result);
                Assert.Equal("Transport order content is updated.", logRecord.Message);
                Assert.Equal(updatedServiceOrder.ID, logRecord.ServiceOrderID);
                Assert.Equal(updatedTransportOrder.ID, logRecord.TransportOrderID);
            }
            catch 
            {
                throw;
            }
        }
        
        [Theory(DisplayName = "Order CIT Allocation - Update 'replenishment' transport order")]    // test case 7813 
        [InlineData(GenericStatus.Unconfirmed, "REPL")]
        [InlineData(GenericStatus.Confirmed, "REPL")]
        //[InlineData(GenericStatus.Registered, "REPL")] TO DO - gap in SD regarding 'registered' status
        public void UpdateReplenishmentTransportOrder(GenericStatus status, string serviceType)
        {
            try
            {
                var newServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var newTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(newServiceOrder.ID);
                var newTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(newTransportOrder.ID);
                HelperFacade.TransportHelper.UpdateServiceOrder(newServiceOrder, withProducts: true, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var updatedServiceOrder = DataFacade.Order.Take(o => o.WPOrderID == newServiceOrder.WPOrderID).Build();
                var updatedTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(updatedServiceOrder.ID);
                var updatedTransportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(updatedTransportOrder.ID);
                var updatedTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(updatedTransportOrder.ID);
                
                Assert.Equal(newTransportOrder.ID, updatedTransportOrder.ID);                
                Assert.True(updatedTransportOrder.IsBillable);
                Assert.Equal(2, updatedTransportOrderProducts.Count());
                Assert.Equal(updatedServiceOrder.OrderedValue, updatedTransportOrderProducts.Sum(p => p.OrderedValue));
                Assert.Equal(2, updatedTransportOrderServices.Count());
                Assert.True(newTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID != updatedTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID);
                Assert.True(newTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID != updatedTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID);
                Assert.True(updatedTransportOrderServices.All(s => s.IsPlanned));
                Assert.False(updatedTransportOrderServices.All(s => s.IsPerformed));               
            }
            catch 
            {
                throw;
            }
        }

        [Theory(DisplayName = "Order CIT Allocation - Update 'servicing' transport order")]    // test case 7813 
        [InlineData(GenericStatus.Unconfirmed, "SERV")]
        [InlineData(GenericStatus.Confirmed, "SERV")]
        //[InlineData(GenericStatus.Registered, "SERV")]
        public void UpdateServicingTransportOrder(GenericStatus status, string serviceType)
        {
            try
            {
                var newServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var newTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(newServiceOrder.ID);
                var newTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(newTransportOrder.ID);
                HelperFacade.TransportHelper.UpdateServiceOrder(newServiceOrder, withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var updatedServiceOrder = DataFacade.Order.Take(o => o.WPOrderID == newServiceOrder.WPOrderID).Build();
                var updatedTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(updatedServiceOrder.ID);            
                var updatedTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(updatedTransportOrder.ID);  
                
                Assert.Equal(newTransportOrder.ID, updatedTransportOrder.ID);               
                Assert.Equal(2, updatedTransportOrderServices.Count());
                Assert.True(newTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID != updatedTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID);
                Assert.True(newTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID != updatedTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID);
                Assert.True(updatedTransportOrderServices.All(s => s.IsPlanned));
                Assert.False(updatedTransportOrderServices.All(s => s.IsPerformed));                
            }
            catch
            {                
                throw;
            }
        }
        
        [Theory(DisplayName = "Order CIT Allocation - Update service order with service date in the past")]    // test case 7816-1
        [InlineData(GenericStatus.Unconfirmed, "REPL")]
        [InlineData(GenericStatus.Confirmed, "REPL")]
        //[InlineData(GenericStatus.Registered, "REPL")]        
        public void UpdateServiceOrderWithServiceDateInThePast(GenericStatus status, string serviceType)
        {
            try
            {
                var newServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var newTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(newServiceOrder.ID);
                var newTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(newTransportOrder.ID);
                HelperFacade.TransportHelper.UpdateServiceOrder(newServiceOrder, withProducts: true, withServices: true);
                HelperFacade.TransportHelper.ChangeServiceDate(newServiceOrder.ID, DateTime.Now.AddDays(-1));
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var updatedServiceOrder = DataFacade.Order.Take(o => o.WPOrderID == newServiceOrder.WPOrderID).Build();
                var updatedTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(updatedServiceOrder.ID);
                var updatedTransportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(updatedTransportOrder.ID);
                var updatedTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(updatedTransportOrder.ID);
                var logRecord = HelperFacade.TransportHelper.FindOrderCitAllocationLogRecord(updatedServiceOrder.ID);

                Assert.Equal(newTransportOrder.ID, updatedTransportOrder.ID);               
                // verify that products and services are not updated for service order with service date in the past                
                Assert.NotEqual(updatedTransportOrderProducts.Sum(p => p.OrderedValue), updatedServiceOrder.OrderedValue);
                Assert.Equal(newServiceOrder.OrderedValue, updatedTransportOrderProducts.Sum(p => p.OrderedValue));                
                Assert.Equal(2, updatedTransportOrderServices.Count());
                Assert.Equal(newTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID, updatedTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID);
                Assert.Equal(newTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID, updatedTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID);                
                Assert.Equal(OrderCitAllocationResult.Ok, logRecord.Result);
                Assert.True(logRecord.Message == null);
                Assert.Equal(updatedServiceOrder.ID, logRecord.ServiceOrderID);
                Assert.Equal(updatedTransportOrder.ID, logRecord.TransportOrderID);
            }
            catch
            {
                throw;
            }
        }
        
        [Theory(DisplayName = "Order CIT Allocation - Update 'fixed' service order")]    // test case 7816-2
        [InlineData(GenericStatus.Unconfirmed, "REPL")]
        [InlineData(GenericStatus.Confirmed, "REPL")]
        //[InlineData(GenericStatus.Registered, "REPL")]        
        public void UpdateFixedServiceOrder(GenericStatus status, string serviceType)
        {
            try
            {
                var newServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var newTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(newServiceOrder.ID);
                var newTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(newTransportOrder.ID);
                HelperFacade.TransportHelper.UpdateServiceOrder(newServiceOrder, withProducts: true, withServices: true);
                HelperFacade.TransportHelper.ChangeServiceOrderTypeToFixed(newServiceOrder.ID);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var updatedServiceOrder = DataFacade.Order.Take(o => o.WPOrderID == newServiceOrder.WPOrderID).Build();
                var updatedTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(updatedServiceOrder.ID);
                var updatedTransportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(updatedTransportOrder.ID);
                var updatedTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(updatedTransportOrder.ID);
                
                Assert.Equal(newTransportOrder.ID, updatedTransportOrder.ID);                               
                Assert.Equal(newServiceOrder.OrderType, updatedTransportOrder.OrderType);                        
                // verify that products and services are not updated for service order with 'fixed' order type                
                Assert.NotEqual(updatedTransportOrderProducts.Sum(p => p.OrderedValue), updatedServiceOrder.OrderedValue);
                Assert.Equal(newServiceOrder.OrderedValue, updatedTransportOrderProducts.Sum(p => p.OrderedValue));                
                Assert.Equal(newTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID, updatedTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID);
                Assert.Equal(newTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID, updatedTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID);                
            }
            catch
            {             
                throw;
            }
        }

        [Theory(DisplayName = "Order CIT Allocation - Update 'canceled' service order")]    // test case 7816-3
        [InlineData(GenericStatus.Unconfirmed, "DELV")]
        [InlineData(GenericStatus.Unconfirmed, "REPL")]
        public void UpdateCanceledServiceOrder(GenericStatus status, string serviceType)
        {
            try
            {
                var newServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var newTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(newServiceOrder.ID);
                var newTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(newTransportOrder.ID);
                HelperFacade.TransportHelper.UpdateServiceOrder(newServiceOrder, withProducts: true, withServices: true);
                var updatedServiceOrder = DataFacade.Order.Take(o => o.WPOrderID == newServiceOrder.WPOrderID).Build();
                HelperFacade.TransportHelper.ChangeGenericStatus(updatedServiceOrder, GenericStatus.Cancelled);
                HelperFacade.TransportHelper.RunCitAllocationJob();            
                var updatedTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(updatedServiceOrder.ID);
                var updatedTransportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(updatedTransportOrder.ID);
                var updatedTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(updatedTransportOrder.ID);

                Assert.Equal(newTransportOrder.ID, updatedTransportOrder.ID);                
                // verify that products and services are not updated for 'canceled' service order                
                Assert.NotEqual(updatedTransportOrderProducts.Sum(p => p.OrderedValue), updatedServiceOrder.OrderedValue);
                Assert.Equal(newServiceOrder.OrderedValue, updatedTransportOrderProducts.Sum(p => p.OrderedValue));                
                Assert.Equal(newTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID, updatedTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID);
                Assert.Equal(newTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID, updatedTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID);
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Order CIT Allocation - Update 'completed' service order")]    // test case 7816-4
        [InlineData(GenericStatus.Unconfirmed, "DELV")]
        [InlineData(GenericStatus.Unconfirmed, "REPL")]
        public void UpdateCompletedServiceOrder(GenericStatus status, string serviceType)
        {
            try
            {
                var newServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var newTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(newServiceOrder.ID);
                var newTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(newTransportOrder.ID);
                HelperFacade.TransportHelper.UpdateServiceOrder(newServiceOrder, withProducts: true, withServices: true);
                var updatedServiceOrder = DataFacade.Order.Take(o => o.WPOrderID == newServiceOrder.WPOrderID).Build();
                HelperFacade.TransportHelper.ChangeGenericStatus(updatedServiceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var updatedTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(updatedServiceOrder.ID);
                var updatedTransportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(updatedTransportOrder.ID);
                var updatedTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(updatedTransportOrder.ID);

                Assert.Equal(newTransportOrder.ID, updatedTransportOrder.ID);
                Assert.Equal(TransportOrderStatus.Registered, updatedTransportOrder.Status);              
                // verify that products and services are not updated for 'completed' service order                
                Assert.NotEqual(updatedTransportOrderProducts.Sum(p => p.OrderedValue), updatedServiceOrder.OrderedValue);
                Assert.Equal(newServiceOrder.OrderedValue, updatedTransportOrderProducts.Sum(p => p.OrderedValue));                
                Assert.Equal(newTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID, updatedTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID);
                Assert.Equal(newTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID, updatedTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID);
            }
            catch
            {                
                throw;
            }
        }

        [Theory(DisplayName = "Order CIT Allocation - Update service order with multiple transport orders")]    // test case 7816-5
        [InlineData(GenericStatus.Unconfirmed, "DELV")]
        [InlineData(GenericStatus.Unconfirmed, "REPL")]
        public void UpdateServiceOrderWithMultipleTransportOrders(GenericStatus status, string serviceType)
        {
            try
            {
                var newServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, status, serviceType, orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var newTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(newServiceOrder.ID);
                var newTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(newTransportOrder.ID);
                HelperFacade.TransportHelper.UpdateServiceOrder(newServiceOrder, withProducts: true, withServices: true);
                var updatedServiceOrder = DataFacade.Order.Take(o => o.WPOrderID == newServiceOrder.WPOrderID).Build();
                HelperFacade.TransportHelper.CreateSecondTransportOrder(updatedServiceOrder, orderCitAllocationTestFixture.ExternalLocation);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var updatedTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(updatedServiceOrder.ID);
                var updatedTransportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(updatedTransportOrder.ID);
                var updatedTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(updatedTransportOrder.ID);

                Assert.Equal(newTransportOrder.ID, updatedTransportOrder.ID);                
                // verify that products and services are not updated for service order with multiple transport orders                
                Assert.NotEqual(updatedTransportOrderProducts.Sum(p => p.OrderedValue), updatedServiceOrder.OrderedValue);
                Assert.Equal(newServiceOrder.OrderedValue, updatedTransportOrderProducts.Sum(p => p.OrderedValue));                
                Assert.Equal(newTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID, updatedTransportOrderServices.OrderBy(s => s.ID).FirstOrDefault().ID);
                Assert.Equal(newTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID, updatedTransportOrderServices.OrderByDescending(s => s.ID).FirstOrDefault().ID);
            }
            catch
            {                
                throw;
            }
        }

        //[Theory(DisplayName = "Order CIT Allocation - Replace service order products with barcoded products")]    // test case 7816-5
        //[InlineData(GenericStatus.Unconfirmed, "DELV")]
        //[InlineData(GenericStatus.Unconfirmed, "REPL")]
        //public void ReplaceServiceOrderProductsWithBarcodedProducts(GenericStatus status, string serviceType)
        //{
        [Fact(DisplayName = "Order CIT Allocation - Replace service order products with barcoded products")]    // test case 7816-5        
        public void ReplaceServiceOrderProductsWithBarcodedProducts()
        {
            try
            {
                var newServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Unconfirmed, "DELV", orderCitAllocationTestFixture.ExternalLocation, withProducts: true, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();            
                HelperFacade.TransportHelper.UpdateServiceOrder(newServiceOrder, withBarcodedProducts: true);
                var updatedServiceOrder = DataFacade.Order.Take(o => o.WPOrderID == newServiceOrder.WPOrderID).Build();            
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var updatedTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(updatedServiceOrder.ID);
                var updatedTransportOrderProducts = HelperFacade.TransportHelper.FindTransportOrderProducts(updatedTransportOrder.ID);
                var updatedTransportOrderServices = HelperFacade.TransportHelper.FindTransportOrderServices(updatedTransportOrder.ID);            
                Assert.Equal(1, updatedTransportOrderProducts.Count);
            }
            catch
            {               
                throw;
            }
        }       
        #endregion 
    }
}
