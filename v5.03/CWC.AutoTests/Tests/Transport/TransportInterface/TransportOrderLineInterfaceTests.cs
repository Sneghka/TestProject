using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.Contracts;
using Cwc.Ordering;
using Cwc.Transport;
using Cwc.Transport.Enums;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.TransportOrderInterfaceBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CWC.AutoTests.Tests.Transport.TransportInterface
{
    public class TransportOrderLineInterfaceTests : IClassFixture<TransportOrderJobFixture>, IDisposable
    {
        string defaultDate, defaultOrderID, defaultFileName;
        Customer defaultCustomer;
        Location defaultLocation;
        ServiceType defaultServiceType;
        TransportOrderJobFixture fixture;
        DataModel.ModelContext context;
        DateTime today = DateTime.Today;

        public TransportOrderLineInterfaceTests(TransportOrderJobFixture fixt)
        {
            defaultDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            defaultOrderID = $"{DateTime.Now.ToString("yyyyMMddhhmmssfff")}";
            defaultFileName = $"SOline-{defaultOrderID}";
            defaultCustomer = DataFacade.Customer.Take(c => c.ReferenceNumber == "1101").Build();
            defaultLocation = DataFacade.Location.Take(x => x.Code == "SP01").Build();
            defaultServiceType = DataFacade.ServiceType.Take(x => x.Code == "DELV");
            fixture = fixt;
            context = new DataModel.ModelContext();
        }

        [Fact(DisplayName = "When transport order line - order id is not exists in the System Then System doesn't allow to import it")]
        public void VerifyThatSystemDoesntAllowToImportTransportOrderLineWithNonExistedTransportOrder()
        {
            var route = "PMR20000";
            var transportOrderLine = TransportOrderInterfaceFacade.TransportOrderLine.New(1)
                .WithOrderLineID($"{defaultOrderID}-1")
                .WithOrderID(defaultOrderID)
                .WithOrderLineStatus(TransportOrderStatus.Registered.ToString())
                .WithlocationID(defaultLocation.ID)
                .WithMasterRoute(route)
                .WithServiceType(defaultServiceType.Code)
                .WithOrderLineValue(1)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundMsg = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(x=>x.FileName.StartsWith(defaultFileName));
            var nonfoundLine = context.SOlines.FirstOrDefault(x=>x.OrderLine_ID == defaultOrderID+"-1");

            Assert.NotNull(foundMsg);
            Assert.Null(nonfoundLine);
            Assert.Equal($"Transport order with Code {defaultOrderID} does not exist.", foundMsg.Message.TrimEnd());
        }

        [Fact(DisplayName = "When transport order line - TransportOrder exists and action is create Then System doesn't allow to import current order line")]
        public void VerifyThatSystemDoesntAllowToCreateTransportOrderLineWithAlreadyExistedOrderID()
        {
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();
            var initialServiceDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(defaultLocation, defaultServiceType);
            var route = "RMR20000";

            var transportOrder = DataFacade.TransportOrder.New()
                .With_Location(defaultLocation.ID)
                .With_Site(defaultLocation.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(initialServiceDate)
                .With_ServiceDate(initialServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder.ID)
                .With_ServiceType(defaultServiceType.ID)
                .SaveToDb()
                .Build();

            var transportOrderLine = TransportOrderInterfaceFacade.TransportOrderLine.New(0)
                .WithOrderLineID($"{transportOrder.Code}-1")
                .WithOrderID($"{transportOrder.Code}")
                .WithOrderLineStatus(TransportOrderStatus.Registered.ToString())
                .WithlocationID(defaultLocation.ID)
                .WithMasterRoute(route)
                .WithServiceType(defaultServiceType.Code)
                .WithOrderLineValue(1)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundMsg = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(x => x.FileName.StartsWith(defaultFileName));
            var nonfoundLine = context.SOlines.FirstOrDefault(x => x.OrderLine_ID == defaultOrderID + "-1");

            Assert.NotNull(foundMsg);
            Assert.Null(nonfoundLine);
            Assert.Equal($"Transport order with Code {transportOrder.Code} already exists.", foundMsg.Message.TrimEnd());
        }

        [Fact(DisplayName = "When action is delete Then System doesn't allow to import current line")]
        public void VerifyThatSystemDoesntAllowoDeleteOrderLine()
        {
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();
            var initialServiceDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(defaultLocation, defaultServiceType);
            var route = "RMR20000";

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(defaultLocation.ID)
                .With_Site(defaultLocation.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(initialServiceDate)
                .With_ServiceDate(initialServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder.ID)
                .With_ServiceType(defaultServiceType.ID)
                .SaveToDb()
                .Build();

            var transportOrderLine = TransportOrderInterfaceFacade.TransportOrderLine.New(2)
                .WithOrderLineID($"{transportOrer.Code}-1")
                .WithOrderID($"{transportOrer.Code}")
                .WithOrderLineStatus(TransportOrderStatus.Registered.ToString())
                .WithlocationID(defaultLocation.ID)
                .WithMasterRoute(route)
                .WithServiceType(defaultServiceType.Code)
                .WithOrderLineValue(1).SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundMsg = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(x => x.FileName.StartsWith(defaultFileName));
            var foundOrder = context.Cwc_Transport_TransportOrders.FirstOrDefault(x=>x.id == transportOrer.ID);

            Assert.NotNull(foundMsg);
            Assert.NotNull(foundOrder);
            Assert.Equal($"It is not allowed to delete transport order.", foundMsg.Message.TrimEnd());
            
        }

        [Fact(DisplayName = "When Transport Order line is not Replenishment and action is update Then System updates given Transport Order Line")]
        public void VerifyThatWhenActionIsUpdateThenSystemUpdatesGinenTransportOrderLine()
        {
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();
            var arrivalTime = new TimeSpan(10, 0, 0);
            var initialServiceDate = DateTime.Now.Date;
            var route = "RMR20000";

            var transportOrder = DataFacade.TransportOrder.New()
                .With_Location(defaultLocation.ID)
                .With_Site(defaultLocation.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(initialServiceDate)
                .With_ServiceDate(initialServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder.ID)
                .With_ServiceType(defaultServiceType.ID)
                .SaveToDb()
                .Build();

            var transportOrderLine = TransportOrderInterfaceFacade.TransportOrderLine.New(1)
                .WithOrderLineID($"{transportOrder.Code}-1")
                .WithOrderID($"{transportOrder.Code}")
                .WithOrderLineStatus(TransportOrderStatus.Planned.ToString())
                .WithlocationID(defaultLocation.ID)
                .WithMasterRoute(route)
                .WithDaiDate(initialServiceDate.ToString("yyyy-MM-dd"))
                .WithArrivalTime($"{arrivalTime.ToString("hhmm")}")
                .WithDayNumber(1)
                .WithVisitSequence(1)
                .WithServiceType(defaultServiceType.Code)
                .WithOrderLineValue(1).
                SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundTransportOrder = DataFacade.TransportOrder.Take(x => x.ID == transportOrder.ID).Build();

            Assert.Equal(TransportOrderStatus.Planned, foundTransportOrder.Status);
            Assert.Equal(route, foundTransportOrder.MasterRouteCode);
            Assert.Equal(initialServiceDate.Date, foundTransportOrder.MasterRouteDate.Value.Date);
            Assert.Equal(arrivalTime, foundTransportOrder.StopArrivalTime.Value);
            Assert.Equal(1, foundTransportOrder.VisitSequence);
        }

        [Fact(DisplayName = "When action is create Then System creates adhoc transport order and service order line")]
        public void VerifyThatSYstemCreatesSolineAndServiceOrderWhenActionIssCreated()
        {
            var route = "RMR20000";
            var value = 2;
            var ariival = new TimeSpan(8, 0, 0);
            var serviceOrderCreated = DataFacade.Order.New(today, fixture.defaultLocation, fixture.deliveryServiceType.Code, out defaultOrderID).With_OrderType(OrderType.AdHoc).SaveToDb().Build();

            var transportOrderLine = TransportOrderInterfaceFacade.TransportOrderLine.New(0)
                .WithOrderLineID($"{defaultOrderID}-1")
                .WithOrderID($"{defaultOrderID}")
                .WithOrderLineStatus(TransportOrderStatus.Completed.ToString())
                .WithlocationID(defaultLocation.ID)
                .WithMasterRoute(route)
                .WithServiceType(defaultServiceType.Code)
                .WithOrderLineValue(value)
                .WithDayNumber(value)
                .WithDaiDate(DateTime.Now.ToString("yyyy-MM-dd"))
                .WithArrivalTime(ariival.ToString("hhmm"))
                .WithVisitSequence(value)
                .WithBranchNumber(defaultLocation.BranchID)
                .WithRevenue(value.ToString())
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundSoline = context.SOlines.FirstOrDefault(x => x.OrderLine_ID == defaultOrderID + "-1");
            var foundCreatedTransportOrder = DataFacade.TransportOrder.Take(x => x.ServiceOrderID == serviceOrderCreated.ID).Build();

            Assert.NotNull(foundSoline);
            Assert.NotNull(foundCreatedTransportOrder);

            Assert.Equal(true, foundCreatedTransportOrder.IsPdaAdHoc);
            Assert.Equal(OrderType.AdHoc, foundCreatedTransportOrder.OrderType);
            Assert.Equal(today, foundCreatedTransportOrder.ServiceDate.Value.Date);
            Assert.Equal(today, foundCreatedTransportOrder.TransportDate.Date);
            Assert.Equal(TransportOrderStatus.Completed, foundCreatedTransportOrder.Status);
            Assert.Equal(route, foundCreatedTransportOrder.MasterRouteCode);
            Assert.Equal(DateTime.Now.Date, foundCreatedTransportOrder.MasterRouteDate.Value.Date);
            Assert.Equal(ariival, foundCreatedTransportOrder.StopArrivalTime.Value);
            Assert.Equal(value, foundCreatedTransportOrder.VisitSequence);
        }

        [Fact(DisplayName = "When Service Order is created and Soline is send for creation Then System craetes Soline")]
        public void VerifyThatSystemCreatesSolineProperly()
        {
            var route = "RMR20000";
            var value = 2;
            var ariival = new TimeSpan(8, 0, 0);
            var serviceOrderCreated = DataFacade.Order.New(today, fixture.defaultLocation, fixture.deliveryServiceType.Code, out defaultOrderID).With_OrderType(OrderType.AdHoc).SaveToDb().Build();

            var transportOrderLine = TransportOrderInterfaceFacade.TransportOrderLine.New(0)
                .WithOrderLineID($"{defaultOrderID}-1")
                .WithOrderID($"{defaultOrderID}")
                .WithOrderLineStatus(TransportOrderStatus.Completed.ToString())
                .WithlocationID(defaultLocation.ID)
                .WithMasterRoute(route)
                .WithServiceType(defaultServiceType.Code)
                .WithOrderLineValue(value)
                .WithDayNumber(value)
                .WithDaiDate(DateTime.Now.ToString("yyyy-MM-dd"))
                .WithArrivalTime(ariival.ToString("hhmm"))
                .WithVisitSequence(value)
                .WithBranchNumber(defaultLocation.BranchID)
                .WithOrderLineTime1("0000-1200")
                .WithOrderLineTime2("1300-2300")
                .WithRevenue(value.ToString())
                .WithOrderLineValue(1000)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundSoline = context.SOlines.FirstOrDefault(x => x.OrderLine_ID == defaultOrderID + "-1");

            Assert.Equal(defaultOrderID, foundSoline.Order_ID);
            Assert.Equal("registered", foundSoline.Orderline_status);
            Assert.Equal(DateTime.Now.Date, foundSoline.Dai_date.Value.Date);
            Assert.Equal(ariival.ToString("hhmm"), foundSoline.a_time);
            Assert.Equal(value, foundSoline.Visit_Sequence);
            Assert.Equal(defaultLocation.BranchID, foundSoline.branch_nr);
            Assert.Equal("0000-1200", foundSoline.Orderline_timew1);
            Assert.Equal("1300-2300", foundSoline.Orderline_timew2);
            Assert.Equal(route, foundSoline.mast_cd);
            Assert.Equal(value, (int)foundSoline.Revenue);
            Assert.Equal(1000, (int)foundSoline.Orderline_value);
            Assert.Equal(defaultLocation.ID, foundSoline.Loc_nr);
            Assert.Equal(defaultServiceType.OldType, foundSoline.Serv_type);
        }

        [Fact(DisplayName = "When action is create and service order is not exists Then System sends SOline to replenishment que")]
        public void WhenSolineIsCreatedAndPropriateServiceOrderNotExistsThenSystemPutSolineinQue()
        {
            var route = "RMR20000";
            var value = 2;
            var ariival = new TimeSpan(8, 0, 0);

            var transportOrderLine = TransportOrderInterfaceFacade.TransportOrderLine.New(0)
                .WithOrderLineID($"{defaultOrderID}-1")
                .WithOrderID($"{defaultOrderID}")
                .WithOrderLineStatus(TransportOrderStatus.Completed.ToString())
                .WithlocationID(defaultLocation.ID)
                .WithMasterRoute(route)
                .WithServiceType(defaultServiceType.Code)
                .WithOrderLineValue(value)
                .WithDayNumber(value)
                .WithDaiDate(DateTime.Now.ToString("yyyy-MM-dd"))
                .WithArrivalTime(ariival.ToString("hhmm"))
                .WithVisitSequence(value)
                .WithBranchNumber(defaultLocation.BranchID)
                .WithOrderLineTime1("0000-1200")
                .WithOrderLineTime2("1300-2300")
                .WithRevenue(value.ToString())
                .WithOrderLineValue(1000)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundSoline = context.Cwc_Transport_TransportOrderIntegrationJobReplenishmentQueues.FirstOrDefault(x => x.FileName.StartsWith(defaultFileName) && x.Entity == (int)ReplenishmentQueueEntity.ServiceOrderLine);

            Assert.NotNull(foundSoline);
        }

        [Fact(DisplayName =  "When only one transport order is linked to service order Then System updates it SOline")]
        public void VerifyWhenOnlyOneServiceOrderExistsForTransportOrderThenSystemUpdateServiceOrderLine()
        {
            var route = "RMR20000";
            var value = 2;
            var ariival = new TimeSpan(8, 0, 0);
            var serviceOrderCreated = DataFacade.Order.New(today, fixture.defaultLocation, fixture.deliveryServiceType.Code, out defaultOrderID).With_OrderType(OrderType.AdHoc).SaveToDb().Build();

            var transportOrderCreated = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation).With_Site(fixture.defaultLocation.BranchID).With_ServiceType(fixture.deliveryServiceType)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(serviceOrderCreated.ServiceDate).With_ServiceDate(serviceOrderCreated.ServiceDate)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrderCreated).SaveToDb().Build();

            var transportOrderLine = TransportOrderInterfaceFacade.TransportOrderLine.New(1)
                .WithOrderLineID($"{transportOrderCreated.Code}-1")
                .WithOrderID($"{transportOrderCreated.Code}")
                .WithOrderLineStatus(TransportOrderStatus.Completed.ToString())
                .WithlocationID(defaultLocation.ID)
                .WithMasterRoute(route)
                .WithServiceType(defaultServiceType.Code)
                .WithOrderLineValue(value)
                .WithDayNumber(value)
                .WithDaiDate(DateTime.Now.ToString("yyyy-MM-dd"))
                .WithArrivalTime(ariival.ToString("hhmm"))
                .WithVisitSequence(value)
                .WithBranchNumber(defaultLocation.BranchID)
                .WithOrderLineTime1("0000-1200")
                .WithOrderLineTime2("1300-2300")
                .WithRevenue(value.ToString())
                .WithOrderLineValue(1000)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundSoline = context.SOlines.SingleOrDefault(x => x.Order_ID == defaultOrderID);

            Assert.NotNull(foundSoline);
            Assert.Equal(route, foundSoline.mast_cd);
            Assert.Equal(defaultLocation.BranchID, foundSoline.branch_nr);
            Assert.Equal(value, foundSoline.Day_nr);
            Assert.Equal(DateTime.Now.Date, foundSoline.Dai_date.Value.Date);
            Assert.True(foundSoline.a_time.StartsWith(ariival.ToString()));
            Assert.Equal(value, foundSoline.Visit_Sequence);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
