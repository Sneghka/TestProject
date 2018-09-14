using Cwc.Transport;
using Cwc.Transport.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.DailyDataBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace CWC.AutoTests.Tests.Transport.PackageLifeCycle
{
    public class PlcForNewServiceOrderTests : IClassFixture<PlcFixture>, IDisposable
    {
        PlcFixture fixture;
        TransactionScope scope;
        PackageLifeCycleProcessingJobSettings setting;

        public PlcForNewServiceOrderTests(PlcFixture fixt)
        {
            fixture = fixt;
            scope = new TransactionScope();
            setting = new PackageLifeCycleProcessingJobSettings();
            setting.PreviousStarted = DateTime.Now.Date;
        }

        [Fact(DisplayName = "When his_pack->ToLocation is not delivery Transport Order-> Location Then System creates new Transport and Service Order")]
        public void Verify_SystemCreatesNewTransportandServiceOrderWhenLocationIsNotEqualDeliveryOrder()
        {
            var locationDiff = DataFacade.Location.InitDefault().With_Company(DataFacade.Customer.Take(x => x.ReferenceNumber == "1101")).With_HandlingType("NOR").SaveToDb().Build();
            var transportOrder = DataFacade.TransportOrder.InitDefault(fixture.orderDelv).With_StopArrivalTime(DateTime.Now.TimeOfDay).SaveToDb().Build();
            var hisPack = DailyDataFacade.HisPack.InitDefault(transportOrder).With_ToLocation(locationDiff).With_Status("RRT").SaveToDb();

            var result = TransportFacade.PackageLifeCycleProcessingJobService.ProcessPackageLifeCyclesData(setting);

            var createdTransportOrder = DataFacade.TransportOrder.TakeAll(x => x.LocationID == locationDiff.ID && x.OldTransportOrderCode == transportOrder.Code).FirstOrDefault();
            Assert.NotNull(createdTransportOrder);

            var createdServiceOrder = DataFacade.Order.Take(x => x.ID == createdTransportOrder.ServiceOrderID).Build();
            Assert.NotNull(createdServiceOrder);
        }

        [Theory(DisplayName = "When his_pack->ToLocation is not Transport Order-> Location and his_pack->ToLocation-> Handling Type in {Cas, Dep} Then System doesn't create new Transport and Service Order")]
        [InlineData("CAS")]
        [InlineData("DEP")]
        public void Verify_WhenLocationIsCasOrDepThenSystemDoesntProcessHisPack(string handling)
        {
            var locationDiff = DataFacade.Location.Take(x => x.HandlingType == handling).Build();
            var transportOrder = DataFacade.TransportOrder.InitDefault(fixture.orderDelv).With_StopArrivalTime(DateTime.Now.TimeOfDay).SaveToDb();
            var hisPack = DailyDataFacade.HisPack.InitDefault(transportOrder).With_ToLocation(locationDiff).With_Status("RRT").SaveToDb();
        }

        [Fact(DisplayName = "When his_pack->ToLocation is replenishment Transport Order-> Location Then System creates new Transport and Service Order")]
        public void Verify_SystemCreatesNewTransportandServiceOrderWhenLocationIsNotEqualReplenishmentOrder()
        {
            var locationDiff = DataFacade.Location.Take(x => x.Code == "JG02").Build();
            var transportOrder = DataFacade.TransportOrder.InitDefault(fixture.orderRepl).With_StopArrivalTime(DateTime.Now.TimeOfDay).SaveToDb();
            var hisPack = DailyDataFacade.HisPack.InitDefault(transportOrder).With_ToLocation(locationDiff).With_Status("RRT").SaveToDb();
            fixture.CreateLinks(transportOrder);


        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
