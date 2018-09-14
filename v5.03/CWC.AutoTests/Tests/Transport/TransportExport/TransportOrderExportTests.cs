using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.Transport.TransportExport
{
    public class TransportOrderExportTests : IClassFixture<TransportExportFixture>, IDisposable
    {
        string serviceOrderEntityName = "ServiceOrder";
        string serviceOrderProductEntityName = "SOProduct";
        string serviceOrderServiceEntityName = "SOService";
        string serviceOrderLineEntityName = "SOline";

        TransportExportFixture fixture;
        AutomationTransportDataContext context;
        Cwc.Ordering.Order serviceOrder;
        DateTime defaultDate;
        public TransportOrderExportTests(TransportExportFixture fixt)
        {
            fixture = fixt;
            context = new AutomationTransportDataContext();
            var location = DataFacade.Location.Take(x => x.Code == "SP02").Build();
            serviceOrder = DataFacade.Order.New(DateTime.Today, location, "DELV").SaveToDb();
            
        }

        [Fact(DisplayName = "Transport Order Exprot - When transport order is created and IsExport = yes Then System export transport order")]
        public void VerifyThatSystemExportsTransportOrderOnCreation()
        {
            defaultDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.defaultServiceType, Cwc.Contracts.OrderType.AtRequest);

            var transportOrder = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_ServiceType(fixture.defaultServiceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(Cwc.Transport.Enums.TransportOrderStatus.Registered)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(defaultDate)
                .With_ServiceDate(defaultDate)
                .With_Site(fixture.defaultLocation.ServicingDepotID)
                .SaveToDb()
                .Build();

            var foundExportedOrderExported = HelperFacade.TransportExportHelper.MapEntity(serviceOrderEntityName, transportOrder.Code, 0);
            var foundExportedOrderLine = HelperFacade.TransportExportHelper.MapEntity(serviceOrderLineEntityName, transportOrder.Code, 0);

            Assert.NotNull(foundExportedOrderExported);
            Assert.NotNull(foundExportedOrderLine);
        }

        [Fact(DisplayName = "Transport Order Exprot - When Transport Order Product is created Then System exports Transport Order Product")]
        public void VerifyThattransportOrderProductIsExportedOnCreation()
        {
            defaultDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.defaultServiceType, Cwc.Contracts.OrderType.AtRequest);

            var transportOrder = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_ServiceType(fixture.defaultServiceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(Cwc.Transport.Enums.TransportOrderStatus.Registered)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(defaultDate)
                .With_ServiceDate(defaultDate)
                .With_Site(fixture.defaultLocation.ServicingDepotID)
                .SaveToDb()
                .Build();

            var defaultProduct = DataFacade.Product.Take(p => p.Value > 0).Build();

            var transportOrderProduct = DataFacade.TransportOrderProduct.New()
                .With_TransportOrder(transportOrder)
                .With_Product(defaultProduct)
                .With_CurrencyID("EUR")
                .With_OrderedQuantity(1)
                .With_OrderedValue((int)defaultProduct.Value * 1)
                .SaveToDb()
                .Build();

            var foundExportedTransportOrderProductExported = HelperFacade.TransportExportHelper.MapEntity(serviceOrderProductEntityName, transportOrder.Code, 0);

            Assert.NotNull(foundExportedTransportOrderProductExported);
        }

        [Fact(DisplayName = "Transport Order Exprot - When new Transport Order Service is created Then System exports it")]
        public void VerifyThatOrderServiceIsExportedOnCreation()
        {
            defaultDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.defaultServiceType, Cwc.Contracts.OrderType.AtRequest);

            var transportOrder = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_ServiceType(fixture.defaultServiceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(Cwc.Transport.Enums.TransportOrderStatus.Registered)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(defaultDate)
                .With_ServiceDate(defaultDate)
                .With_Site(fixture.defaultLocation.ServicingDepotID)
                .SaveToDb()
                .Build();

            var defaultServCode = context.ServicingCodes.First();

            var transportOrderService = DataFacade.TransportOrderServ.New()
                .With_TransportOrderID(transportOrder)
                .With_Service(defaultServCode.Code)
                .With_IsPerformed(false)
                .With_IsPlanned(true)
                .SaveToDb();

            var foundTransportOrderServiceExported = HelperFacade.TransportExportHelper.MapEntity(serviceOrderServiceEntityName, transportOrder.Code, 0);

            Assert.NotNull(foundTransportOrderServiceExported);
        }

        [Fact(DisplayName = "Transport Order Exprot - When Transport Order is updated and IsExport = yes The System exports transport order")]
        public void VerifyThatTransportOrderIsExportedOnUpdation()
        {
            var transportOrderUpdated = DataFacade.TransportOrder.Take(x => x.Status == Cwc.Transport.Enums.TransportOrderStatus.Registered).Update(null, null, null, null, false, false, false, null, null, true).Build();

            var foundTransportOrderExported = HelperFacade.TransportExportHelper.MapEntity(serviceOrderEntityName, transportOrderUpdated.Code, 1);
            var foundExportedOrderLine = HelperFacade.TransportExportHelper.MapEntity(serviceOrderLineEntityName, transportOrderUpdated.Code, 1);

            Assert.NotNull(foundTransportOrderExported);
        }

        [Fact(DisplayName = "Transport Order Exprot - When Transport order product is updated Then System export it")]
        public void VerifyThatTransportOrderProductIsExportedOnUpdation()
        {
            defaultDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.defaultServiceType, Cwc.Contracts.OrderType.AtRequest);
            var defaultProduct = DataFacade.Product.Take(p => p.Value > 0).Build();

            var transportOrder = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_ServiceType(fixture.defaultServiceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(Cwc.Transport.Enums.TransportOrderStatus.Registered)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(defaultDate)
                .With_ServiceDate(defaultDate)
                .With_Site(fixture.defaultLocation.ServicingDepotID)
                .SaveToDb()
                .Build();

            var transportOrderProduct = DataFacade.TransportOrderProduct.New()
                .With_TransportOrder(transportOrder)
                .With_Product(defaultProduct)
                .With_CurrencyID("EUR")
                .With_OrderedQuantity(1)
                .With_OrderedValue((int)defaultProduct.Value * 1)
                .SaveToDb();

            transportOrderProduct.With_OrderedQuantity(2).SaveToDb().Build();
            
            var foundTransportOrderProductExported = HelperFacade.TransportExportHelper.MapEntity(serviceOrderProductEntityName, transportOrder.Code, 1);

            Assert.NotNull(foundTransportOrderProductExported);
        }

        [Fact(DisplayName = "Transport Order Exprot - When Transport Order service is updated then transport order service is exported")]
        public void VerifyThatTransportOrderServiceIsExportedOnUpdation()
        {
            var newDdate = DateTime.Now;
            var transportOrderService = DataFacade.TransportOrderServ.Take(x => x.ServiceID != string.Empty).With_DateUpdated(newDdate).SaveToDb().Build();
            var linkedTransportOrder = DataFacade.TransportOrder.Take(tr => tr.ID == transportOrderService.TransportOrderID).Build();

            var foundTransportOrderServiceExported = HelperFacade.TransportExportHelper.MapEntity(serviceOrderServiceEntityName, linkedTransportOrder.Code, 1);

            Assert.NotNull(foundTransportOrderServiceExported);
        }

        [Fact(DisplayName = "Transport Order Exprot - When transport order product is deleted Then System exports it")]
        public void VerifyThatTransportOrderProductIsExportedOnDeletion()

        {
            defaultDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.defaultServiceType, Cwc.Contracts.OrderType.AtRequest);
            var defaultProduct = DataFacade.Product.Take(p => p.Value > 0).Build();

            var transportOrder = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_ServiceType(fixture.defaultServiceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(Cwc.Transport.Enums.TransportOrderStatus.Registered)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(defaultDate)
                .With_ServiceDate(defaultDate)
                .With_Site(fixture.defaultLocation.ServicingDepotID)
                .SaveToDb()
                .Build();

            var transportOrderProduct = DataFacade.TransportOrderProduct.New()
                .With_TransportOrder(transportOrder)
                .With_Product(defaultProduct)
                .With_CurrencyID("EUR")
                .With_OrderedQuantity(1)
                .With_OrderedValue((int)defaultProduct.Value * 1)
                .SaveToDb();

            DataFacade.TransportOrderProduct.Delete(transportOrderProduct);

            var foundTransportOrderProductExported = HelperFacade.TransportExportHelper.MapEntity(serviceOrderProductEntityName, transportOrder.Code, 2);

            Assert.NotNull(foundTransportOrderProductExported);
        }

        [Fact(DisplayName = "Transport Order Exprot - When transport order service is deleted Then System exports it")]
        public void VerifyThatTransportOrderServiceIsExportedOnDeletion()
        {
            defaultDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.defaultServiceType, Cwc.Contracts.OrderType.AtRequest);
            var defaultService = DataFacade.ServicingCode.Take(s => s.Code != null).Build();

            var transportOrder = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_ServiceType(fixture.defaultServiceType)
                .With_ServiceOrder(serviceOrder)
                .With_Status(Cwc.Transport.Enums.TransportOrderStatus.Registered)
                .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
                .With_TransportDate(defaultDate)
                .With_ServiceDate(defaultDate)
                .With_Site(fixture.defaultLocation.ServicingDepotID)
                .SaveToDb()
                .Build();

            var transportOrderServ = DataFacade.TransportOrderServ.New()
                .With_TransportOrderID(transportOrder.ID)
                .With_Service(defaultService)
                .With_IsPerformed(false)
                .With_IsPlanned(true)
                .SaveToDb().
                Build();

            DataFacade.TransportOrderServ.Delete(transportOrderServ);

            var foundTransportOrderServiceExported = HelperFacade.TransportExportHelper.MapEntity(serviceOrderServiceEntityName, transportOrder.Code, 2);

            Assert.NotNull(foundTransportOrderServiceExported);
        }

        public void Dispose()
        {
            context.TransportOrderExportItems.RemoveRange(context.TransportOrderExportItems);
            context.SaveChanges();
            context.Dispose();
        }
    }
}
