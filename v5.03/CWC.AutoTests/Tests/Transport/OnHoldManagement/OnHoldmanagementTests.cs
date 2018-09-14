using Cwc.BaseData;
using Cwc.Transport;
using Cwc.Transport.Model;
using Edsson.WebPortal.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace Edsson.WebPortal.AutoTests.Tests.Transport.OnHoldManagement
{
    public class OnHoldmanagementTests : IDisposable
    {
        Cwc.Ordering.Order defaultOrder;
        TransactionScope scope;
        CitProcessSettingBuilder settings;
        public OnHoldmanagementTests()
        {
            scope = new TransactionScope();
            var location = DataFacade.Location.Take(s => s.Code == "SP02").Build();
            var definedDate = DataFacade.Order.DefineServicedate("DELV", location).Value;
            defaultOrder = DataFacade.Order.New(definedDate, location, "DELV").SaveToDb().Build();
            settings = DataFacade.CitProcessSettings.MatchByLocation(location);
        }

        [Theory(DisplayName = "When IsOnHold == Transport Order -> IsOnHold Then System shows error message")]
        [InlineData(true, "Transport order on-hold status cannot be changed: it is already putted on-hold.")]
        [InlineData(false, "Transport order on-hold status cannot be changed: it is already not on-hold.")]
        public void Verify_ThatSystemDoesntAllowToPutHoldAlreadyHoldedOrder(bool isOnHold, string message)
        {
            var transportOrder = DataFacade.TransportOrder.InitDefault(defaultOrder).With_IsOnHold(isOnHold).Build();

            var result = TransportFacade.TransportOrderService.PutOnOffHoldTransportOrder(transportOrder, isOnHold, Cwc.BaseData.ProcessName.PutTransportOrderOnHold, Cwc.BaseData.ProcessPhase.Start);

            Assert.False(result.IsSuccess);
            Assert.Equal(message, result.GetMessage());
        }

        [Theory(DisplayName = "When IsOnHold != Transport Order -> IsOnHold Then System update Transport Order")]
        [InlineData(true)]
        [InlineData(false)]
        public void Verify_ThatSystemUpdatesTransportOrderProperly(bool isOnHold)
        {
            var transportOrder = DataFacade.TransportOrder.InitDefault(defaultOrder).With_IsOnHold(!isOnHold).SaveToDb().Build();

            var result = TransportFacade.TransportOrderService.PutOnOffHoldTransportOrder(transportOrder, isOnHold, Cwc.BaseData.ProcessName.PutTransportOrderOnHold, Cwc.BaseData.ProcessPhase.Start);

            var foundOrder = DataFacade.TransportOrder.Take(x => x.Code == transportOrder.Code).Build();
            Assert.True(result.IsSuccess);
            Assert.NotNull(foundOrder);
            Assert.Equal(isOnHold, foundOrder.IsOnHold);
            Assert.Equal(isOnHold, foundOrder.DatePuttedOnHold.HasValue);
        }

        [Theory(DisplayName = "When System sets Transport Order on hold Then System creates Cit Processing History for this event")]
        [InlineData(Cwc.BaseData.ProcessName.PutTransportOrderOnHold, Cwc.BaseData.ProcessPhase.Start, true)]
        [InlineData(Cwc.BaseData.ProcessName.Capturing, Cwc.BaseData.ProcessPhase.End, false)]
        public void Verify_ThatSystemCreatesCitProcessingHistoryProperly(Cwc.BaseData.ProcessName name, Cwc.BaseData.ProcessPhase phase, bool IsWithException)
        {
            var transportOrder = DataFacade.TransportOrder.InitDefault(defaultOrder).With_IsWithException(IsWithException).SaveToDb().Build();

            var result = TransportFacade.TransportOrderService.PutOnOffHoldTransportOrder(transportOrder, true, name, phase);

            var foundCitProcessingHistory = DataFacade.CitProcessingHistory.Take(x => x.ObjectID == transportOrder.ID && x.ProcessName == name).Build();

            Assert.Equal(foundCitProcessingHistory.Status, (int)transportOrder.Status);
            Assert.Equal(phase, foundCitProcessingHistory.ProcessPhase);
            Assert.Equal(IsWithException, foundCitProcessingHistory.IsWithException);

        }

        [Fact(DisplayName = "When transport order -> date putted on hold is expirred Then System sets order off hold")]
        public void Verify_ThatSystemPutOnHoldOffProperly()
        {
            var diff = 1;
            settings.With_OnHoldDaysNumber(diff).SaveToDb();
            var transportOrder = DataFacade.TransportOrder.InitDefault(defaultOrder).With_IsOnHold(true).With_PuttedOnHoldDate(DateTime.Now.AddDays(-(diff+1))).SaveToDb().Build();

            var result = TransportFacade.OnHoldTransportOrderProcessingJobService.ProcessOnHoldTransportOrders();

            var foundOrder = DataFacade.TransportOrder.Take(x => x.Code == transportOrder.Code).Build();
            Assert.True(result.IsSuccess);
            Assert.False(foundOrder.IsOnHold);
            Assert.Null(foundOrder.DatePuttedOnHold);
        }

        [Fact(DisplayName = "When Transport Order -> DatePuttedHold is not expired Then System doesn't takes it off hold")]
        public void Verify_WhenDatePuttedOnHoldIsNOtExpitedThenSystemDoesntPutOffHold()
        {
            var diff = 1;
            settings.With_OnHoldDaysNumber(diff).SaveToDb();
            var transportOrder = DataFacade.TransportOrder.InitDefault(defaultOrder).With_IsOnHold(true).With_PuttedOnHoldDate(DateTime.Now).SaveToDb().Build();

            var result = TransportFacade.OnHoldTransportOrderProcessingJobService.ProcessOnHoldTransportOrders();

            var foundOrder = DataFacade.TransportOrder.Take(x => x.Code == transportOrder.Code).Build();
            Assert.True(result.IsSuccess);
            Assert.True(foundOrder.IsOnHold);
            Assert.NotNull(foundOrder.DatePuttedOnHold);
        }

        [Fact(DisplayName = "When Expired Period is not set at settings Then System doesn't put transport order off hold")]
        public void Verify_WhenExpiredPeriodIsNotSetThenSystemDoesntPutOrderOffHold()
        {
            settings.With_OnHoldDaysNumber(null).SaveToDb();
            var transportOrder = DataFacade.TransportOrder.InitDefault(defaultOrder).With_IsOnHold(true).With_PuttedOnHoldDate(DateTime.Now.AddDays(-7)).SaveToDb().Build();

            var result = TransportFacade.OnHoldTransportOrderProcessingJobService.ProcessOnHoldTransportOrders();

            var foundOrder = DataFacade.TransportOrder.Take(x => x.Code == transportOrder.Code).Build();
            Assert.True(result.IsSuccess);
            Assert.True(foundOrder.IsOnHold);
            Assert.NotNull(foundOrder.DatePuttedOnHold);
        }

        [Fact(DisplayName = "When multiple transport orders are on hold and their date are expired Then System puts them off hold")]
        public void Verify_ThatMultipleordersCanBePutOffHold()
        {
            settings.With_OnHoldDaysNumber(1).SaveToDb();
            CreateTransportOrders(10);

            var result = TransportFacade.OnHoldTransportOrderProcessingJobService.ProcessOnHoldTransportOrders();

            var torders = DataFacade.TransportOrder.TakeAll(x => x.ServiceOrderID == defaultOrder.ID);

            Assert.True(result.IsSuccess);
            Assert.True(torders.All(x => !x.IsOnHold));
        }

        [Fact(DisplayName = "System should process 10'000 transport order in less than 15 secs")]
        public void Verify_ThatsystemProcessTransportOrderInExpectedperformance()
        {
            settings.With_OnHoldDaysNumber(1).SaveToDb();
            CreateTransportOrders(200);
            var watch = Stopwatch.StartNew();
            var result = TransportFacade.OnHoldTransportOrderProcessingJobService.ProcessOnHoldTransportOrders();
            watch.Stop();
            Assert.True(result.IsSuccess);
            Assert.True(watch.Elapsed.Seconds <= 1);
        }

        private void CreateTransportOrders(int amout)
        {
            for (int i = 0; i <= amout; i++)
            {
                DataFacade.TransportOrder.InitDefault(defaultOrder).With_IsOnHold(true).With_PuttedOnHoldDate(DateTime.Now.AddDays(-7)).SaveToDb().Build();
            }
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
