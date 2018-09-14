using Cwc.Transport;
using Cwc.Transport.Enums;
using Cwc.Transport.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.DailyDataBuilders;
using System;
using System.Linq;
using System.Transactions;
using Xunit;

namespace CWC.AutoTests.Tests.Transport.PackageLifeCycle
{
    public class PlcForNewTransportOrderTests : IClassFixture<PlcFixture>, IDisposable
    {
        PlcFixture fixture;
        DataModel.ModelContext context;        
        PackageLifeCycleProcessingJobSettings setting;

        public PlcForNewTransportOrderTests(PlcFixture fixt)
        {            
            fixture = fixt;
            context = new DataModel.ModelContext();
            setting = new PackageLifeCycleProcessingJobSettings
            {
                PreviousStarted = DateTime.Now.Date
            };
        }

        [Fact(DisplayName = "When non replenishment package is recollected Then System creates new Transport Order for it")]
        public void Verify_SystemCreatesNewTransportOrderForNonReplenishmentPackage()
        {
            var newDelvDate = fixture.orderDelv.ServiceDate.AddDays(1);
            var transportOrder = DataFacade.TransportOrder.InitDefault(fixture.orderDelv).With_StopArrivalTime(DateTime.Now.TimeOfDay).SaveToDb().Build();
            var hisPack = DailyDataFacade.HisPack.InitDefault(transportOrder).With_NewDeliveryDate(newDelvDate).With_Status("REROUTED").SaveToDb().Build();

            var result = TransportFacade.PackageLifeCycleProcessingJobService.ProcessPackageLifeCyclesData(setting);

            var foundOrder = DataFacade.TransportOrder.Take(x => x.OldTransportOrderCode == transportOrder.Code).Build();
            var foundLog = context.Cwc_Transport_PackageLifeCycleProcessingJobLogs.FirstOrDefault(x => x.TransportOrderOldId == transportOrder.ID);

            Assert.True(result.IsSuccess);
            Assert.Equal(hisPack.NewDeliveryDate.Value.Date, foundOrder.TransportDate.Date);
            Assert.NotNull(foundLog);
            Assert.Equal(foundLog.Result, (int)ResultType.Ok);
            Assert.Equal((int)PackageLifeCycleProcessingAction.RerouteNewDeliveryDate, foundLog.Action);
            Assert.Equal(hisPack.ID, foundLog.PackageLifeCycleId);
            Assert.Equal(foundOrder.ID, foundLog.TransportOrderNewId);
            Assert.Equal(foundOrder.ServiceOrderID, transportOrder.ServiceOrderID);
        }

        [Fact(DisplayName = "When replenishment package is recollected Then System creates new Transport Order for it")]
        public void Verify_SystemCreatesNewTransportOrderForReplenishmentPackage()
        {
            var newDelvDate = fixture.orderRepl.ServiceDate.AddDays(1);
            var transportOrder = DataFacade.TransportOrder.InitDefault(fixture.orderRepl).With_StopArrivalTime(DateTime.Now.TimeOfDay).SaveToDb().Build();
            var hisPack = DailyDataFacade.HisPack.InitDefault(transportOrder).With_NewDeliveryDate(newDelvDate).With_Status("REROUTED").SaveToDb().Build();
            fixture.CreateLinks(transportOrder);

            var result = TransportFacade.PackageLifeCycleProcessingJobService.ProcessPackageLifeCyclesData(setting);

            var foundOrder = DataFacade.TransportOrder.Take(x => x.OldTransportOrderCode == transportOrder.Code).Build();
            var foundLog = context.Cwc_Transport_PackageLifeCycleProcessingJobLogs.FirstOrDefault(x => x.TransportOrderOldId == transportOrder.ID);

            Assert.True(result.IsSuccess);
            Assert.Equal(hisPack.NewDeliveryDate.Value.Date, foundOrder.TransportDate.Date);
            Assert.NotNull(foundLog);
            Assert.Equal(foundLog.Result, (int)ResultType.Ok);
            Assert.Equal((int)PackageLifeCycleProcessingAction.RerouteNewDeliveryDate, foundLog.Action);
            Assert.Equal(hisPack.ID, foundLog.PackageLifeCycleId);
            Assert.Equal(foundOrder.ID, foundLog.TransportOrderNewId);
            Assert.Equal(foundOrder.ServiceOrderID, transportOrder.ServiceOrderID);
        }

        [Theory(DisplayName = "When suitable transport order exists for package Then System doesn't create a new one and logs error message")]
        [InlineData(TransportOrderStatus.Planned)]
        [InlineData(TransportOrderStatus.Registered)]
        public void Verify_SystemDoesntCreateNewTransportOrderWhenSuitableIsExistNonRepl(TransportOrderStatus status)
        {
            var newDelvDate = fixture.orderDelv.ServiceDate.AddDays(1);
            var transportOrderOrigin = DataFacade.TransportOrder.InitDefault(fixture.orderDelv).With_StopArrivalTime(DateTime.Now.TimeOfDay).SaveToDb().Build();
            var hisPack = DailyDataFacade.HisPack.InitDefault(transportOrderOrigin).With_NewDeliveryDate(newDelvDate).With_Status("REROUTED").SaveToDb().Build();
            var transportOrderSuitable = DataFacade.TransportOrder.InitDefault(fixture.orderDelv).With_TransportDate(newDelvDate).With_Status(status).SaveToDb().Build();

            var result = TransportFacade.PackageLifeCycleProcessingJobService.ProcessPackageLifeCyclesData(setting);

            var foundOrders = context.Cwc_Transport_TransportOrders.Count(x => x.ServiceOrderID == fixture.orderDelv.ID);
            var foundLog = context.Cwc_Transport_PackageLifeCycleProcessingJobLogs.FirstOrDefault(x => x.TransportOrderOldId == transportOrderOrigin.ID);

            Assert.Equal(2, foundOrders);
            Assert.NotNull(foundLog);
            Assert.Equal(transportOrderSuitable.ID, foundLog.TransportOrderNewId);
            Assert.Equal((int)ResultType.Warning, foundLog.Result);
            Assert.Equal(hisPack.ID, foundLog.PackageLifeCycleId);
            Assert.Equal($"Transport order for new date already exists. Container must be relinked in Transport Module.", foundLog.Message);
        }

        [Theory(DisplayName = "When suitable Transport Order is already in use Then System creates new Transport Order")]
        [InlineData(TransportOrderStatus.Completed)]
        [InlineData(TransportOrderStatus.InTransit)]
        public void Verify_SystemLogsReturnedFromCopyErrorMessageInLog(TransportOrderStatus status)

        {
            var newDelvDate = fixture.orderDelv.ServiceDate.AddDays(1);
            var transportOrderOrigin = DataFacade.TransportOrder.InitDefault(fixture.orderDelv).With_StopArrivalTime(DateTime.Now.TimeOfDay).SaveToDb().Build();
            var hisPack = DailyDataFacade.HisPack.InitDefault(transportOrderOrigin).With_NewDeliveryDate(newDelvDate).With_Status("REROUTED").SaveToDb().Build();
            var transportOrderSuitable = DataFacade.TransportOrder.InitDefault(fixture.orderDelv).With_TransportDate(newDelvDate).With_Status(status).SaveToDb().Build();

            var result = TransportFacade.PackageLifeCycleProcessingJobService.ProcessPackageLifeCyclesData(setting);

            var foundOrder = DataFacade.TransportOrder.Take(x => x.OldTransportOrderCode == transportOrderOrigin.Code).Build();
            var foundLog = context.Cwc_Transport_PackageLifeCycleProcessingJobLogs.FirstOrDefault(x => x.TransportOrderOldId == transportOrderOrigin.ID);

            Assert.True(result.IsSuccess);
            Assert.Equal(hisPack.NewDeliveryDate.Value.Date, foundOrder.TransportDate.Date);
            Assert.NotNull(foundLog);
            Assert.Equal(foundLog.Result, (int)ResultType.Ok);
            Assert.Equal((int)PackageLifeCycleProcessingAction.RerouteNewDeliveryDate, foundLog.Action);
            Assert.Equal(hisPack.ID, foundLog.PackageLifeCycleId);
            Assert.Equal(foundOrder.ID, foundLog.TransportOrderNewId);
        }

        [Fact(DisplayName = "When CopyTransportOrder returns an error Then System logs this error message")]
        public void Verify_CopiedTransportOrderDoesntContainProductAndServicesFromOriginal()
        {
            var newDelvDate = fixture.orderDelv.ServiceDate.AddDays(1);
            var transportOrder = DataFacade.TransportOrder.InitDefault(fixture.orderDelv).With_StopArrivalTime(DateTime.Now.TimeOfDay).SaveToDb().Build();
            var transportOrderProduct = DataFacade.TransportOrderProduct.InitDefault(transportOrder.ID).SaveToDb();
            var transportOrderService = DataFacade.TransportOrderServ.InitDefault(transportOrder.ID).SaveToDb();
            var hisPack = DailyDataFacade.HisPack.InitDefault(transportOrder).With_NewDeliveryDate(newDelvDate).With_Status("REROUTED").SaveToDb().Build();

            var result = TransportFacade.PackageLifeCycleProcessingJobService.ProcessPackageLifeCyclesData(setting);

            var foundCreatedOrder = DataFacade.TransportOrder.Take(x => x.OldTransportOrderCode == transportOrder.Code).Build();
            var foundLinkedProducts = context.Cwc_Transport_TransportOrderProducts.FirstOrDefault(x => x.TransportOrderID == foundCreatedOrder.ID);
            var foundLinkedServices = context.Cwc_Transport_TransportOrderServs.FirstOrDefault(x => x.TransportOrderID == foundCreatedOrder.ID);

            Assert.True(result.IsSuccess);
            Assert.Equal(hisPack.NewDeliveryDate.Value.Date, foundCreatedOrder.TransportDate.Date);
            Assert.Null(foundLinkedProducts);
            Assert.Null(foundLinkedServices);
        }

        public void Dispose()
        {            
            context.Dispose();
        }
    }
}
