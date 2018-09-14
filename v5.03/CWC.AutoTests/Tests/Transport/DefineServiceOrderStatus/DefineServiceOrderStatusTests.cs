using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.Constants;
using Cwc.Ordering;
using Cwc.Transport;
using Cwc.Transport.Enums;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CWC.AutoTests.Tests.Transport.DefineServiceOrderStatus
{
    public class DefineServiceOrderStatusTests : IDisposable
    {
        string code, name;
        ServiceType serviceType;
        DataModel.ModelContext context;
        DateTime serviceDate = DateTime.Today;
        Site externalSite;
        Customer customer;
        LocationType locationtype;
        int? orderingDep;
        public DefineServiceOrderStatusTests()
        {
            ConfigurationKeySet.Load();
            code = $"Code{new Random().Next(11111, 99999)}";
            name = $"Name{new Random().Next(11111, 99999)}";
            serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV");            
            context = new DataModel.ModelContext();
            externalSite = DataFacade.Site.Take(x => x.WP_IsExternal && x.BranchType == Cwc.BaseData.BranchType.CITDepot);
            customer = DataFacade.Customer.Take(x => x.ReferenceNumber == "1101");
            locationtype = DataFacade.LocationType.Take(x => x.ltCode == "NOR");
            orderingDep = context.locations.First(x => x.ref_loc_nr == "SP02").OrderingDepartmentID;
        }

        [Fact(DisplayName = "When Service Order - Location - Servicing Cit Site isExternal Then System defines local status = empty, withException = empty")]
        public void VerifyWhenSiteIsExternalThenLocalStatusAndExcepotionAreEmpty()
        {

            var location = DataFacade.Location.New().
                With_Code(code).
                With_Company(customer).
                With_LocationTypeID(locationtype.ltCode).
                With_Name(name).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepot(externalSite).
                With_Level(Cwc.BaseData.Enums.LocationLevel.ServicePoint)
                .With_OrderingDepartmentID(orderingDep)
                .SaveToDb();
            var serviceOrder = DataFacade.Order.New(serviceDate, location, serviceType.Code).SaveToDb();
            var transportOrder = DataFacade.TransportOrder.New()
                .With_Location(location)
                .With_ServiceType(serviceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(TransportOrderStatus.Registered)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(serviceDate)
                .With_ServiceDate(serviceDate)
                .With_Site(location.Build().ServicingDepotID)
                .SaveToDb()
                .Build();
            var definedStatus = TransportFacade.TransportOrderService.DefineServiceOrderTransportStatus(serviceOrder);

            Assert.Null(definedStatus.Status);
            Assert.Null(definedStatus.IsWithException);
        }


        [Fact(DisplayName = "When all linked Transport Orders are cancelled Then System defimes Order Status as cancelled")]
        public void VerifyWhenAllTransportOrderStatusAreCancelledThenSystemDefinesOrderStatusAsCancelled()
        {
            var location = DataFacade.Location.Take(x => x.Code == "SP02");
            var serviceOrder = DataFacade.Order.New(serviceDate, location, serviceType.Code).SaveToDb();
            var transportOrderFirst = DataFacade.TransportOrder.New()
                .With_Location(location)
                .With_ServiceType(serviceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(TransportOrderStatus.Cancelled)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(serviceDate)
                .With_ServiceDate(serviceDate)
                .With_Site(location.Build().ServicingDepotID)
                .With_IsWithException(false)
                .SaveToDb()
                .Build();
            var transportOrderSecond= DataFacade.TransportOrder.New()
                .With_Location(location)
                .With_ServiceType(serviceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(TransportOrderStatus.Cancelled)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(serviceDate)
                .With_ServiceDate(serviceDate)
                .With_Site(location.Build().ServicingDepotID)
                .With_IsWithException(true)
                .SaveToDb()
                .Build();
            var definedStatus = TransportFacade.TransportOrderService.DefineServiceOrderTransportStatus(serviceOrder);

            Assert.Equal(GenericStatus.Cancelled, definedStatus.Status);
            Assert.True(definedStatus.IsWithException);
        }


        [Theory(DisplayName = "When all linked transport orders are Cancelled and Completed Then System defines Service Order status as Completed")]
        [InlineData(TransportOrderStatus.Cancelled, TransportOrderStatus.Completed)]
        [InlineData(TransportOrderStatus.Completed, TransportOrderStatus.Completed)]
        public void VerifyWhenAllLinkedTransportOrderAreCancelledAndCompltedThenSystemDefinesServiceOrderStatusISCompleted(TransportOrderStatus status1, TransportOrderStatus status2)
        {
            var location = DataFacade.Location.Take(x => x.Code == "SP02");
            var serviceOrder = DataFacade.Order.New(serviceDate, location, serviceType.Code).SaveToDb();
            var transportOrderFirst = DataFacade.TransportOrder.New()
                .With_Location(location)
                .With_ServiceType(serviceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(status1)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(serviceDate)
                .With_ServiceDate(serviceDate)
                .With_Site(location.Build().ServicingDepotID)
                .With_IsWithException(false)
                .SaveToDb()
                .Build();
            var transportOrderSecond = DataFacade.TransportOrder.New()
                .With_Location(location)
                .With_ServiceType(serviceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(status2)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(serviceDate)
                .With_ServiceDate(serviceDate)
                .With_Site(location.Build().ServicingDepotID)
                .With_IsWithException(false)
                .SaveToDb()
                .Build();
            var definedStatus = TransportFacade.TransportOrderService.DefineServiceOrderTransportStatus(serviceOrder);

            Assert.Equal(GenericStatus.Completed, definedStatus.Status);
            Assert.False(definedStatus.IsWithException);
        }

        [Theory(DisplayName = "When at least one linked Transport Order status is corresponds to InProgress Then System defines Service Order status as InProgress")]
        [InlineData(TransportOrderStatus.Planned)]
        [InlineData(TransportOrderStatus.InTransit)]
        [InlineData(TransportOrderStatus.ToClarifyCancellation)]
        [InlineData(TransportOrderStatus.ToClarifyCompletion)]
        [InlineData(TransportOrderStatus.ToClarifyNewAdhocOrder)]
        public void VerifyWhenAtLeastOneLinkedTransportOrderStatusIsEqualsToInProgressThenSystemDefinesServiceOrderStatusAsInProgress(TransportOrderStatus status)
        {
            var location = DataFacade.Location.Take(x => x.Code == "SP02");
            var serviceOrder = DataFacade.Order.New(serviceDate, location, serviceType.Code).SaveToDb();
            var transportOrderFirst = DataFacade.TransportOrder.New()
                .With_Location(location)
                .With_ServiceType(serviceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(TransportOrderStatus.Cancelled)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(serviceDate)
                .With_ServiceDate(serviceDate)
                .With_Site(location.Build().ServicingDepotID)
                .With_IsWithException(false)
                .SaveToDb()
                .Build();
            var transportOrderSecond = DataFacade.TransportOrder.New()
                .With_Location(location)
                .With_ServiceType(serviceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(TransportOrderStatus.Completed)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(serviceDate)
                .With_ServiceDate(serviceDate)
                .With_Site(location.Build().ServicingDepotID)
                .With_IsWithException(false)
                .SaveToDb()
                .Build();
            var transportOrderThird = DataFacade.TransportOrder.New()
                .With_Location(location)
                .With_ServiceType(serviceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(status)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(serviceDate)
                .With_ServiceDate(serviceDate)
                .With_Site(location.Build().ServicingDepotID)
                .With_IsWithException(true)
                .SaveToDb()
                .Build();
            var transportOrderFourth = DataFacade.TransportOrder.New()
                .With_Location(location)
                .With_ServiceType(serviceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(TransportOrderStatus.Registered)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(serviceDate)
                .With_ServiceDate(serviceDate)
                .With_Site(location.Build().ServicingDepotID)
                .With_IsWithException(false)
                .SaveToDb()
                .Build();
            var definedStatus = TransportFacade.TransportOrderService.DefineServiceOrderTransportStatus(serviceOrder);

            Assert.Equal(GenericStatus.InProgress, definedStatus.Status);
            Assert.Null(definedStatus.IsWithException);
        }

        [Theory(DisplayName = "When at least one linked Transport orders is Registered and there is not InProgress linked transport orders Then System defines Service Order Status as registered")]
        [InlineData(TransportOrderStatus.Cancelled, TransportOrderStatus.Completed, TransportOrderStatus.Registered)]
        [InlineData(TransportOrderStatus.Cancelled, TransportOrderStatus.Completed, TransportOrderStatus.Collected)]
        [InlineData(TransportOrderStatus.Cancelled, TransportOrderStatus.Cancelled, TransportOrderStatus.Registered)]
        [InlineData(TransportOrderStatus.Completed, TransportOrderStatus.Completed, TransportOrderStatus.Collected)]
        public void VerifyWhenAllTransportOrderStatusISRegistedredThenSystemDefinesLocalStatusRegistered(TransportOrderStatus status1, TransportOrderStatus status2, TransportOrderStatus status3)
        {
            var location = DataFacade.Location.Take(x => x.Code == "SP02");
            var serviceOrder = DataFacade.Order.New(serviceDate, location, serviceType.Code).SaveToDb();
            var transportOrderFirst = DataFacade.TransportOrder.New()
                .With_Location(location)
                .With_ServiceType(serviceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(status1)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(serviceDate)
                .With_ServiceDate(serviceDate)
                .With_Site(location.Build().ServicingDepotID)
                .With_IsWithException(false)
                .SaveToDb()
                .Build();
            var transportOrderSecond = DataFacade.TransportOrder.New()
                .With_Location(location)
                .With_ServiceType(serviceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(status2)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(serviceDate)
                .With_ServiceDate(serviceDate)
                .With_Site(location.Build().ServicingDepotID)
                .With_IsWithException(false)
                .SaveToDb()
                .Build();
            var transportOrderThird = DataFacade.TransportOrder.New()
                .With_Location(location)
                .With_ServiceType(serviceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(status3)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(serviceDate)
                .With_ServiceDate(serviceDate)
                .With_Site(location.Build().ServicingDepotID)
                .With_IsWithException(false)
                .SaveToDb()
                .Build();
            var definedStatus = TransportFacade.TransportOrderService.DefineServiceOrderTransportStatus(serviceOrder);

            Assert.Equal(GenericStatus.Registered, definedStatus.Status);
            Assert.Null(definedStatus.IsWithException);
        }

        [Fact(DisplayName = "When service type is not in {Coll, Delv, Serv, Repl} Then System doesn't process them")]
        public void VerifyThatOnlyCollDelvServReplTypesAreProcessed()
        {
            var pickandPack = DataFacade.ServiceType.Take(s => s.OldType == ServiceTypeConstants.PickAndPack).Build();
            var serviceOrder = DataFacade.Order.Take(x => x.ID != null);
            serviceOrder.Build().ServiceTypeCode = pickandPack.Code;

           Assert.Throws(typeof(Exception), ()=>  TransportFacade.TransportOrderService.DefineServiceOrderTransportStatus(serviceOrder));
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
