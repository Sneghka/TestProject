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
    public class TransportOrderUpdateInterfaceTests : IClassFixture<TransportOrderJobFixture>, IDisposable
    {
        string defaultDate, defaultOrderID, defaultFileName;
        TransportOrderJobFixture fixture;
        DataModel.ModelContext context;
        public TransportOrderUpdateInterfaceTests(TransportOrderJobFixture fixt)
        {
            defaultDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            defaultOrderID = $"{DateTime.Now.ToString("yyyyMMddhhmmssfff")}";
            defaultFileName = $"ServiceOrderUpdate-{defaultOrderID}";

            this.fixture = fixt;
            context = new DataModel.ModelContext();
        }

        [Theory(DisplayName = "When transport order update action is create or delete Then System doesn't allow to import them")]
        [InlineData(2)]
        [InlineData(0)]
        public void VerifyThatSystemDoesntAllowToCreateOrUpdateTransportOrderUpdate(int action)
        {

            var transportOrderUpdate = TransportOrderInterfaceFacade.TransportOrderUpdate.New(action)
                .WithOrderID(defaultOrderID)
                .WithOrderStatus(TransportOrderStatus.Registered.ToString())
                .WithNewServiceDate(defaultDate)
                .WithMasterRoute(fixture.routeCode)
                .WithException(Convert.ToInt32(false))
                .WithLocationID(fixture.defaultLocation.ID)
                .WithBranchCode(DataFacade.Site.Take(s => s.ID == fixture.defaultLocation.BranchID).Build().Branch_cd)
                .WithDateUpdated(defaultDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundmsg = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(m => m.FileName.StartsWith(defaultFileName));

            Assert.NotNull(foundmsg);
            Assert.Equal("Transport order can only be updated via transport order update file.", foundmsg.Message.TrimEnd());
        }

        [Theory(DisplayName = "When action is in Create or Delete Then System doesn't allow to import this file and doesn't applicate changes to transport order")]
        [InlineData(2)]
        [InlineData(0)]
        public void VerifyWhenActionIsCreateDeleteThenChangesAreNotApplicable(int action)
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

            var transportOrderUpdate = TransportOrderInterfaceFacade.TransportOrderUpdate.New(action)
                .WithOrderID(transportOrderCreated.Code)
                .WithOrderStatus(TransportOrderStatus.Registered.ToString())
                .WithNewServiceDate(transportOrderCreated.DateUpdated.ToString("yyyy-MM-dd hh:mm:ss"))
                .WithMasterRoute(fixture.routeCode)
                .WithException(Convert.ToInt32(false))
                .WithLocationID(fixture.defaultLocation.ID)
                .WithBranchCode(DataFacade.Site.Take(s => s.ID == fixture.defaultLocation.BranchID).Build().Branch_cd)
                .WithDateUpdated(defaultDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundTransportOrder = context.Cwc_Transport_TransportOrders.FirstOrDefault(tr=>tr.Code == transportOrderCreated.Code);

            Assert.NotNull(foundTransportOrder);

        }

        [Fact(DisplayName = "When transport order update - code is not existed Then System doesn't allow to import it")]
        public void VerifyThatSystemDoesntAllowToImportTransoportOrderUpdateWithNonExistedCode()
        {
            var definedServiceDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.deliveryServiceType, OrderType.AtRequest);

            var transportOrderUpdate = TransportOrderInterfaceFacade.TransportOrderUpdate.New(1)
                .WithOrderID(defaultOrderID)
                .WithOrderStatus(TransportOrderStatus.Registered.ToString())
                .WithNewServiceDate(definedServiceDate.ToString("yyyy-MM-dd hh:mm:ss"))
                .WithMasterRoute(fixture.routeCode)
                .WithException(Convert.ToInt32(false))
                .WithLocationID(fixture.defaultLocation.ID)
                .WithBranchCode(DataFacade.Site.Take(s => s.ID == fixture.defaultLocation.BranchID).Build().Branch_cd)
                .WithDateUpdated(defaultDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundMsg = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(m => m.FileName.StartsWith(defaultFileName));

            Assert.NotNull(foundMsg);
            Assert.Equal($"Transport order with Code {defaultOrderID} does not exist.", foundMsg.Message);
        }

        [Fact(DisplayName = "When transport order update - newdate is not equals to old Then System cancelles this transport order and create the new one")]
        public void VerifyThatSystemCancelesExistedTransportOrderIfNewDateIsnotEquals()
        {
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();
            var initialServiceDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.deliveryServiceType, OrderType.AtRequest);
            var nextDate = initialServiceDate.AddDays(1);

            var transportOrerCreated = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation.ID)
                .With_Site(fixture.defaultLocation.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(initialServiceDate)
                .With_ServiceDate(initialServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder.ID)
                .With_ServiceType(fixture.deliveryServiceType.ID)
                .SaveToDb()
                .Build();

            var transportOrderUpdate = TransportOrderInterfaceFacade.TransportOrderUpdate.New(1)
                .WithOrderID(transportOrerCreated.Code)
                .WithOrderStatus(TransportOrderStatus.Planned.ToString())
                .WithNewServiceDate(nextDate.ToString("yyyy-MM-dd"))
                .WithMasterRoute(fixture.routeCode)
                .WithException(Convert.ToInt32(true))
                .WithLocationID(fixture.defaultLocation.ID)
                .WithBranchCode(DataFacade.Site.Take(s => s.ID == fixture.defaultLocation.BranchID).Build().Branch_cd)
                .WithDateUpdated(defaultDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundCancelledOrder = context.Cwc_Transport_TransportOrders.FirstOrDefault(tr=>tr.Code == transportOrerCreated.Code);
            var foundProcessingHistory = context.Cwc_Transport_CitProcessingHistories.FirstOrDefault(ph => ph.ObjectID == transportOrerCreated.ID && ph.ProcessName == (int)ProcessName.PerformDailyRoute);
            var foudnCitProcessingHistoryException = context.Cwc_Transport_CitProcessingHistoryExceptions.FirstOrDefault(x=>x.CitProcessingHistoryID == foundProcessingHistory.ID);
            var foundCopiedTransportOrder = context.Cwc_Transport_TransportOrders.ToArray().FirstOrDefault(tr => tr.ServiceOrderID == transportOrerCreated.ServiceOrderID && tr.LocationID == fixture.defaultLocation.ID && tr.TransportDate.Date == nextDate.Date && tr.MasterRouteCode == fixture.routeCode && tr.Code != transportOrerCreated.Code);


            Assert.NotNull(foundCancelledOrder);
            Assert.Equal((int)TransportOrderStatus.Cancelled, foundCancelledOrder.Status);

            Assert.NotNull(foundProcessingHistory);
            Assert.True(foundProcessingHistory.ProcessPhase == (int)ProcessPhase.Start && foundProcessingHistory.IsWithException == true && foundProcessingHistory.Status == (int)TransportOrderStatus.Cancelled, "CitProcessingHistory saved incorrectly");

            Assert.NotNull(foudnCitProcessingHistoryException);
            Assert.True(foudnCitProcessingHistoryException.Action == (int)ExceptionAction.CancelAndScheduleNew && foudnCitProcessingHistoryException.Remark == "Auto re-planning order on next working day if service is not performed.", "CitProcessingHistoryException saved incorrectly");

            Assert.NotNull(foundCopiedTransportOrder);
        }

        [Fact(DisplayName = "When transport order update new status is OnRoute Then System changes it to InTransit")]
        public void VerifyThatSystemSubstitutsStatuses()
        {
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();
            var definedNewServiceDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.deliveryServiceType, OrderType.AtRequest);

            var transportOrerCreated = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation.ID)
                .With_Site(fixture.defaultLocation.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(definedNewServiceDate)
                .With_ServiceDate(definedNewServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder.ID)
                .With_ServiceType(fixture.deliveryServiceType.ID)
                .SaveToDb()
                .Build();

            var transportOrderUpdate = TransportOrderInterfaceFacade.TransportOrderUpdate.New(1)
                .WithOrderID(transportOrerCreated.Code)
                .WithOrderStatus("onroute")
                .WithNewServiceDate(definedNewServiceDate.ToString("yyyy-MM-dd"))
                .WithMasterRoute(fixture.routeCode)
                .WithException(Convert.ToInt32(false))
                .WithLocationID(fixture.defaultLocation.ID)
                .WithBranchCode(DataFacade.Site.Take(s => s.ID == fixture.defaultLocation.BranchID).Build().Branch_cd)
                .WithDateUpdated(defaultDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundOrder = context.Cwc_Transport_TransportOrders.FirstOrDefault(x => x.Code == transportOrerCreated.Code);

            Assert.NotNull(foundOrder);
            Assert.Equal((int)TransportOrderStatus.InTransit, foundOrder.Status);
        }

        [Fact(DisplayName = "When new transport date is equals in old and in updated transport order Then System updates it")]
        public void VerifyThatSystemUpdatesTransportOrderProperly()
        {            
            var serviceOrder = DataFacade.Order.New(DateTime.Today, fixture.defaultLocation, fixture.deliveryServiceType.Code).SaveToDb().Build();
            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation.ID)
                .With_Site(fixture.defaultLocation.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(serviceOrder.ServiceDate)
                .With_ServiceDate(serviceOrder.ServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder.ID)
                .With_ServiceType(fixture.deliveryServiceType.ID)
                .SaveToDb()
                .Build();
            var transportOrderUpdate = TransportOrderInterfaceFacade.TransportOrderUpdate.New(1)
                .WithOrderID(transportOrer.Code)
                .WithOrderStatus(TransportOrderStatus.Completed.ToString())
                .WithNewServiceDate(serviceOrder.ServiceDate.ToString("yyyy-MM-dd"))
                .WithMasterRoute(fixture.routeCode)
                .WithException(Convert.ToInt32(false))
                .WithLocationID(fixture.defaultLocation.ID)
                .WithBranchCode(DataFacade.Site.Take(s => s.ID == fixture.defaultLocation.BranchID).Build().Branch_cd)
                .WithDateUpdated(defaultDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);
            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);
            var foundOrder = context.Cwc_Transport_TransportOrders.Where(tr=>tr.Code == transportOrer.Code && tr.MasterRouteCode == fixture.routeCode);

            Assert.NotNull(foundOrder);
            Assert.Equal(1, foundOrder.Count());
        }

        [Fact(DisplayName = "When only one transport order is linked to Service Order Then System allows to update it's attributes")]
        public void VerifyThatSystemAllowsToUpdateServiceOrderIfOnlyOneTransportOrderIsLinked()
        {            
            var transportDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.deliveryServiceType, OrderType.AtRequest);
            var serviceOrder = DataFacade.Order.New(DateTime.Today, fixture.defaultLocation, fixture.deliveryServiceType.Code).SaveToDb().Build();
            var branch = DataFacade.Site.Take(s => s.ID != fixture.defaultLocation.ServicingDepotID).Build().Branch_cd;
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
            var transportOrderUpdate = TransportOrderInterfaceFacade.TransportOrderUpdate.New(1)
                .WithOrderID(transportOrderCreated.Code)
                .WithOrderStatus(TransportOrderStatus.Completed.ToString())
                .WithNewServiceDate(transportDate.ToString("yyyy-MM-dd"))
                .WithMasterRoute(fixture.routeCode)
                .WithException(Convert.ToInt32(false))
                .WithLocationID(fixture.defaultLocation.ID)
                .WithBranchCode(branch)
                .WithDateUpdated(defaultDate)
                .SaveToFolder(fixture.folderPath, defaultFileName);
            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);
            var foundOrder = context.ServiceOrders.First(or => or.Order_ID == serviceOrder.ID);

            Assert.Equal(fixture.routeCode, foundOrder.WP_mast_cd);
            Assert.Equal(transportDate.Date, foundOrder.WP_NewServiceDate.Value.Date);
            Assert.Equal(branch, foundOrder.WP_branch_cd);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
