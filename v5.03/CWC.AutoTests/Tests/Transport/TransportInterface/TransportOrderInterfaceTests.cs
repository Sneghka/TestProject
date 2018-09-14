using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.Contracts;
using Cwc.Ordering;
using Cwc.Transport;
using Cwc.Transport.Enums;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.TransportOrderInterfaceBuilder;
using CWC.AutoTests.ObjectBuilder.TransportOrderInterfaceBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CWC.AutoTests.Tests.Transport.TransportInterface
{
    public class TransportOrderInterfaceTests : IClassFixture<TransportOrderJobFixture>, IDisposable
    {
        string defaultOrderID, defaultFileName;     
        TransportOrderJobFixture fixture;
        DataModel.ModelContext context;
        string serviceDate = DateTime.Today.ToString("yyyy-MM-dd hh:mm:ss");

        public TransportOrderInterfaceTests(TransportOrderJobFixture fixt)
        {            
            defaultOrderID = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            defaultFileName = $"ServiceOrder-{defaultOrderID}";            
            this.fixture = fixt;
            context = new DataModel.ModelContext();
        }

        [Fact(DisplayName = "When action is update and transport order is not exist Then System shows error message")]
        public void VerifyThatSystemDoesntAllowToUpdateNonExistedOrder()
        {
            var serviceCode = DataFacade.ServiceType.Take(s => s.Code == "DELV").Build();
            var transportOrder = TransportOrderInterfaceFacade.TransportOrder.New(1)
                .WithOrderID(defaultOrderID)
                .WithCusnr(fixture.defaultCustomer.ReferenceNumber)
                .WithServiceDate(serviceDate)
                .WithOrder_Status(TransportOrderStatus.Registered.ToString())
                .WithOrder_Type((int)OrderType.AtRequest)
                .WithOrder_Level((int)OrderLevel.Location)
                .WithServiceType_Code(serviceCode.Code)
                .WithDateCreated(serviceDate)
                .WithDateUpdated(serviceDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);
            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);
            var message = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(m=>m.FileName.StartsWith(defaultFileName));

            Assert.Null(context.Cwc_Transport_TransportOrders.FirstOrDefault(to => to.Code == defaultOrderID));
            Assert.NotNull(message);
            Assert.True(message.Message == $"Transport order with Code {defaultOrderID} does not exist." && message.Result == 1 && message.Action == 1, "Error message is not found in log");
        }

        [Fact(DisplayName = "When transport order code is non unique Then System doesn't allow to import it")]
        public void VerifyThatSystemDoesntAllowToCreateTransportOrderWithNonUniqueCode()
        {
            var serviceCode = DataFacade.ServiceType.Take(s => s.Code == "DELV").Build();            
            var existedCode = DataFacade.TransportOrder.Take(x => x.Code != null).Build();
            var transportOrder = TransportOrderInterfaceFacade.TransportOrder.New(0)
                .WithOrderID(existedCode.Code)
                .WithCusnr(fixture.defaultCustomer.ReferenceNumber)
                .WithServiceDate(serviceDate)
                .WithOrder_Status(TransportOrderStatus.Registered.ToString())
                .WithOrder_Type((int)OrderType.AtRequest)
                .WithOrder_Level((int)OrderLevel.Location)
                .WithServiceType_Code(serviceCode.Code)
                .WithDateCreated(serviceDate)
                .WithDateUpdated(serviceDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);
            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);
            var message = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(m => m.FileName.StartsWith(defaultFileName));

            Assert.Equal(1, context.Cwc_Transport_TransportOrders.Count(to => to.Code == existedCode.Code));
            Assert.NotNull(message);
            Assert.True(message.Message == $"Transport order with Code {existedCode.Code} already exists." && message.Action == 0 && message.Result == 1);
        }

        [Fact(DisplayName = "When action is delete Then System shows error message")]
        public void VerifyThatSystemDoesntAllowToDeleteTransportOrder()
        {
            var transportOrder = TransportOrderInterfaceFacade.TransportOrder.New(2)
                .WithOrderID(defaultOrderID)
                .WithCusnr(fixture.defaultCustomer.ReferenceNumber)
                .WithOrder_Status(TransportOrderStatus.Registered.ToString())
                .WithOrder_Type((int)OrderType.AtRequest)
                .WithOrder_Level((int)OrderLevel.Location)
                .WithDateCreated(serviceDate)
                .WithDateUpdated(serviceDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var message = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(m => m.FileName.StartsWith(defaultFileName));
            Assert.NotNull(message);
            Assert.True(message.Message == $"It is not allowed to delete transport order." && message.Result == 1);

        }

        [Theory(DisplayName = "When action is create and service type is not replenishment Then System creates new adhoc order")]
        [InlineData("COLL")]
        [InlineData("DELV")]
        [InlineData("SERV")]
        public void VerifyThatAddhocTransportOrderIsCreatedProperly(string servCode)
        {
            var serviceCode = DataFacade.ServiceType.Take(s => s.Code == servCode).Build();
            var inputtedValue = 10;
            var email = "abc@edsson.com";
            var routeCode = "PMR2000000";

            var transportOrder = TransportOrderInterfaceFacade.TransportOrder.New(0)
                .WithOrderID(defaultOrderID)
                .WithCusnr(fixture.defaultCustomer.ReferenceNumber)
                .WithServiceDate(serviceDate)
                .WithOrder_Status(TransportOrderStatus.Registered.ToString())
                .WithOrder_Type((int)OrderType.AdHoc)
                .WithOrder_Level((int)OrderLevel.Location)
                .WithServiceType_Code(serviceCode.Code)
                .WithLocationCode(fixture.defaultLocation.Code)
                .WithLocationID(fixture.defaultLocation.ID)
                .WithReference_ID(defaultOrderID)
                .WithOrderedValue(inputtedValue)
                .WithOrderedWeight(inputtedValue)
                .WithPreannouncedValue(inputtedValue)
                .WithComments(defaultOrderID)
                .WithSpecialCoinsValue(inputtedValue)
                .WithPickUpLocation(fixture.defaultLocation.Code)
                .WithEmail(email)
                .WithSiteCode(DataFacade.Site.Take(x=>x.ID == fixture.defaultLocation.BranchID).Build().Branch_cd)
                .WithMasterRote(routeCode)
                .WithCurrency("EUR")
                .WithDateCreated(serviceDate)
                .WithDateUpdated(serviceDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundOrder = context.ServiceOrders.FirstOrDefault(to=>to.Order_ID == defaultOrderID);
            var message = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(m => m.FileName.StartsWith(defaultFileName));

            Assert.NotNull(foundOrder);
            Assert.True(foundOrder.Order_Type == (int)OrderType.AdHoc, "Created Transport Order should be AdHoc");

            Assert.NotNull(message);
            Assert.True(message.Action == 0 && message.Result == 0);
        }

        [Fact(DisplayName = "When transport order file is valid Then System updates existed transport order attributes")]
        public void VerifyThatSystemUpdatesTransportOrderProperly()
        {
            var transportDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.deliveryServiceType, OrderType.AtRequest);
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();
            var transportOrderCreated = DataFacade.TransportOrder.New()
                .With_OrderType(OrderType.AtRequest)
                .With_ServiceOrder(serviceOrder)
                .With_Location(fixture.defaultLocation)
                .With_ServiceDate(transportDate)
                .With_TransportDate(transportDate)
                .With_ServiceType(fixture.deliveryServiceType)
                .With_Status(TransportOrderStatus.Registered)
                .With_Site(fixture.defaultLocation.ServicingDepotID)
                .SaveToDb()
                .Build();
            var transportOrderImported = TransportOrderInterfaceFacade.TransportOrder.New(1)
                .WithOrderID(transportOrderCreated.Code)
                .WithCusnr(fixture.defaultCustomer.ReferenceNumber)
                .WithServiceDate(transportDate.ToString("yyyy-MM-dd hh:mm:ss"))
                .WithOrder_Type((int)OrderType.AtRequest)
                .WithOrder_Level((int)serviceOrder.OrderLevel)
                .WithOrder_Status("ONROUTE")
                .WithMasterRote(fixture.routeCode)
                .WithMasterRote(serviceDate)
                .WithDateUpdated(serviceDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);
            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);
            var foundOrder = context.Cwc_Transport_TransportOrders.FirstOrDefault(tr => tr.Code == transportOrderCreated.Code);

            Assert.NotNull(foundOrder);
            Assert.Equal((int)TransportOrderStatus.InTransit, foundOrder.Status);
            Assert.Equal(fixture.routeCode, foundOrder.MasterRouteCode);
        }

        [Fact(DisplayName = "When only one transport order is linked to Service Order Then System allows to update it's attributes")]
        public void VerifyThatSystemAllowsToUpdateServiceOrderIfOnlyOneTransportOrderIsLinked()
        {
            var transportDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.deliveryServiceType, OrderType.AtRequest);
            var serviceOrder = DataFacade.Order.New(DateTime.Today, fixture.defaultLocation, fixture.deliveryServiceType.Code).SaveToDb().Build();
            var branch = DataFacade.Site.Take(s => s.ID != fixture.defaultLocation.ServicingDepotID).Build().Branch_cd;
            var email = "abc@edsson.com";
            var transportOrderCreated = DataFacade.TransportOrder.New()
                .With_OrderType(OrderType.AtRequest)
                .With_ServiceOrder(serviceOrder)
                .With_Location(fixture.defaultLocation)
                .With_ServiceDate(transportDate)
                .With_TransportDate(transportDate)
                .With_ServiceType(fixture.deliveryServiceType)
                .With_Status(TransportOrderStatus.Registered)
                .With_Site(fixture.defaultLocation.ServicingDepotID)
                .SaveToDb()
                .Build();
            var transportOrderImported = TransportOrderInterfaceFacade.TransportOrder.New(1)
                .WithOrderID(transportOrderCreated.Code)
                .WithCusnr(fixture.defaultCustomer.ReferenceNumber)
                .WithServiceDate(transportDate.ToString("yyyy-MM-dd hh:mm:ss"))
                .WithOrder_Type((int)OrderType.AtRequest)
                .WithOrder_Level((int)serviceOrder.OrderLevel)
                .WithOrder_Status(TransportOrderStatus.Planned.ToString())
                .WithSiteCode(branch)
                .WithMasterRote(fixture.routeCode)
                .WithComments("12345")
                .WithMasterRote(serviceDate)
                .WithEmail(email)
                .WithDateUpdated(serviceDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundUpdatedServiceOrder = context.ServiceOrders.FirstOrDefault(or => or.Order_ID == serviceOrder.ID);

            Assert.NotNull(foundUpdatedServiceOrder);
            Assert.Equal("12345", foundUpdatedServiceOrder.WP_Comments);
            Assert.Equal(fixture.routeCode, foundUpdatedServiceOrder.WP_mast_cd);
            Assert.Equal(branch, foundUpdatedServiceOrder.WP_branch_cd);
            Assert.Equal(email, foundUpdatedServiceOrder.WP_Email);
            Assert.Equal(serviceDate, foundUpdatedServiceOrder.WP_DateUpdated?.ToString("yyyy-MM-dd hh:mm:ss"));
        }

        [Fact(DisplayName = "When Serice Type == Replnishment Then System adds this order in que")]
        public void VerifyThatSystemAddsReplenishmentOrderIntoQue()
        {
            var serviceCodeRepl = DataFacade.ServiceType.Take(s => s.Code == "REPL").Build();
            var collectionServiceType = context.WP_ServiceTypes.First(x => x.Code == "COLL");
            var deliveryServiceType = context.WP_ServiceTypes.First(x => x.Code == "DELV");           
            var inputtedValue = 10;
            var email = "abc@edsson.com";
            var routeCode = "PMR2000000";
            var replenishmentConfig = DataFacade.CitProcessSettings.MatchByLocation(fixture.defaultLocation).
                With_ReplenishentConfiguration(serviceCodeRepl.ID, addedCollectionType: collectionServiceType.id, addedDeliveryType: deliveryServiceType.id);
            var transportOrder = TransportOrderInterfaceFacade.TransportOrder.New(0)
                .WithOrderID(defaultOrderID)
                .WithCusnr(fixture.defaultCustomer.ReferenceNumber)
                .WithServiceDate(serviceDate)
                .WithOrder_Status(TransportOrderStatus.Registered.ToString())
                .WithOrder_Type((int)OrderType.AdHoc)
                .WithOrder_Level((int)OrderLevel.Location)
                .WithServiceType_Code(deliveryServiceType.Code)
                .WithLocationCode(fixture.defaultLocation.Code)
                .WithLocationID(fixture.defaultLocation.ID)
                .WithReference_ID(defaultOrderID)
                .WithOrderedValue(inputtedValue)
                .WithOrderedWeight(inputtedValue)
                .WithPreannouncedValue(inputtedValue)
                .WithComments(defaultOrderID)
                .WithSpecialCoinsValue(inputtedValue)
                .WithPickUpLocation(fixture.defaultLocation.Code)
                .WithEmail(email)
                .WithSiteCode(DataFacade.Site.Take(x => x.ID == fixture.defaultLocation.BranchID).Build().Branch_cd)
                .WithMasterRote(routeCode)
                .WithCurrency("EUR")
                .WithDateCreated(serviceDate)
                .WithDateUpdated(serviceDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);
            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);
            var foundQue = context.Cwc_Transport_TransportOrderIntegrationJobReplenishmentQueues.FirstOrDefault(x=>x.FileName.StartsWith(defaultFileName));

            Assert.NotNull(foundQue);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
