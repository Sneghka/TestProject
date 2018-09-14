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
    public class TransportOrderProductInterfaceTests : IClassFixture<TransportOrderJobFixture>, IDisposable
    {
        string defaultDate, defaultOrderID, defaultFileName;
        Product defaultProduct;
        TransportOrderJobFixture fixture;
        DataModel.ModelContext context;
        DateTime defaultTransportOrderDate;
        public TransportOrderProductInterfaceTests(TransportOrderJobFixture fixt)
        {
            defaultDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            defaultOrderID = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            defaultFileName = $"SOProduct-{defaultOrderID}";
            defaultProduct = DataFacade.Product.Take(p => !p.IsBarcodedProduct);
            fixture = fixt;
            defaultTransportOrderDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.deliveryServiceType, OrderType.AtRequest);
            context = new DataModel.ModelContext();
        }

        [Theory(DisplayName = "When transport order product with orderlineid and productcode is not exists Then System doesn't allow to update or delete it")]
        [InlineData(1)]
        [InlineData(2)]
        public void VerifyThatSystemDoesntAllowToUpdateOrDeleteNonExistedTransportOrderProduct(int action)
        {
            var value = 2;
            var transportOrderProduct = TransportOrderInterfaceFacade.TransportOrderProduct.New(action)
                .WithOrderLineID(defaultOrderID)
                .WithProductCode(defaultProduct.ProductCode)
                .WithOrderProductNumber(value)
                .WithOrderProductValue(defaultProduct.Value * value)
                .WithTotalOnly(Convert.ToInt32(false))
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundMessage = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(m=>m.FileName.StartsWith(defaultFileName));

            Assert.NotNull(foundMessage);
            Assert.Equal($"Product {defaultProduct.ProductCode} does not found for transport order with Code {defaultOrderID.Substring(0, defaultOrderID.Length - 2)}.", foundMessage.Message);
        }

        [Fact(DisplayName = "When transport order product is already exists Then System doesn't allow to import the same")]
        public void VerifyThatSystemDoesnyAllowToCreateNonUniqueTransportOrderProduct()
        {
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();
            var qty = 2;

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_Site(fixture.defaultLocation.BranchID)
                .With_ServiceType(fixture.deliveryServiceType)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(defaultTransportOrderDate)
                .With_ServiceDate(serviceOrder.ServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder)
                .SaveToDb().Build();

            var transortOrderProduct = DataFacade.TransportOrderProduct.New()
                .With_TransportOrder(transportOrer)
                .With_Product(defaultProduct)
                .With_OrderedQuantity(qty)
                .With_OrderedValue(qty * (int)defaultProduct.Value)
                .With_CurrencyID("EUR")
                .With_DateCreated(DateTime.Now)
                .With_DateUpdated(DateTime.Now)
                .SaveToDb()
                .Build();

            var transortOrderProductNonUnique = TransportOrderInterfaceFacade.TransportOrderProduct.New(0)
                .WithOrderLineID($"{transportOrer.Code}-1")
                .WithProductCode(defaultProduct.ProductCode)
                .WithOrderProductNumber(qty)
                .WithOrderProductValue(defaultProduct.Value * qty)
                .WithTotalOnly(Convert.ToInt32(false)).SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundMsg = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(m => m.FileName.StartsWith(defaultFileName));
            var foundTransportProducts = context.Cwc_Transport_TransportOrderProducts.Where(tr => tr.TransportOrderID == transportOrer.ID);

            Assert.NotNull(foundMsg);
            Assert.Equal($"Product {defaultProduct.ProductCode} already linked to transport order with Code {transportOrer.Code}.", foundMsg.Message);
            Assert.Equal(1, foundTransportProducts.Count());
        }

        [Fact(DisplayName = "When transport order product - product code is not exists in the System Then System doesn't allow to create it")]
        public void VerifyThatSystemDoesntAllowToImportTransportOrderProductWithNonExistedProduct()
        {
            var qty = 2;
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_Site(fixture.defaultLocation.BranchID)
                .With_ServiceType(fixture.deliveryServiceType)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(defaultTransportOrderDate)
                .With_ServiceDate(serviceOrder.ServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder)
                .SaveToDb().Build();

            var transortOrderProductNonUnique = TransportOrderInterfaceFacade.TransportOrderProduct.New(0)
                .WithOrderLineID($"{transportOrer.Code}-1")
                .WithProductCode("122445QQ")
                .WithOrderProductNumber(qty)
                .WithOrderProductValue(defaultProduct.Value * qty)
                .WithTotalOnly(Convert.ToInt32(false)).SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundMsg = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(m => m.FileName.StartsWith(defaultFileName));
            var foundTransportProducts = context.Cwc_Transport_TransportOrderProducts.FirstOrDefault(tr => tr.TransportOrderID == transportOrer.ID);

            Assert.NotNull(foundMsg);
            Assert.Equal($"There is no product 122445QQ.", foundMsg.Message);
        }

        [Fact(DisplayName = "When transport order product is valid Then System creates it")]
        public void VerifyThatSystemCreatesTransportOrderProductProperly()
        {
            var qty = 2;
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_Site(fixture.defaultLocation.BranchID)
                .With_ServiceType(fixture.deliveryServiceType)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(defaultTransportOrderDate)
                .With_ServiceDate(serviceOrder.ServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder)
                .SaveToDb().Build();

            var transortOrderProductNonUnique = TransportOrderInterfaceFacade.TransportOrderProduct.New(0)
                .WithOrderLineID($"{transportOrer.Code}-1")
                .WithProductCode(defaultProduct.ProductCode)
                .WithOrderProductNumber(qty)
                .WithOrderProductValue(defaultProduct.Value * qty)
                .WithTotalOnly(Convert.ToInt32(false))
                .WithCurrency("EUR").SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundTransportOrderProduct = context.Cwc_Transport_TransportOrderProducts.FirstOrDefault(x=>x.TransportOrderID == transportOrer.ID);

            Assert.NotNull(foundTransportOrderProduct);
            Assert.Equal(defaultProduct.ID, foundTransportOrderProduct.ProductID);
            Assert.Equal(qty, foundTransportOrderProduct.OrderedQuantity);
            Assert.Equal(defaultProduct.Value * qty, foundTransportOrderProduct.OrderedValue);
            Assert.Equal("EUR", foundTransportOrderProduct.CurrencyID);
        }

        [Fact(DisplayName = "When transport order product is valid Then System updates it")]
        public void VerifyThatSystemUpdatesTransportOrderProductProperly()
        {
            var qtyOrigin = 2;
            var qtyUpdated = 1;
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_Site(fixture.defaultLocation.BranchID)
                .With_ServiceType(fixture.deliveryServiceType)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(defaultTransportOrderDate)
                .With_ServiceDate(serviceOrder.ServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder).SaveToDb().Build();

            var transortOrderProduct = DataFacade.TransportOrderProduct.New()
                .With_TransportOrder(transportOrer)
                .With_Product(defaultProduct)
                .With_OrderedQuantity(qtyOrigin)
                .With_OrderedValue(qtyOrigin * (int)defaultProduct.Value)
                .With_CurrencyID("EUR")
                .With_DateCreated(DateTime.Now)
                .With_DateUpdated(DateTime.Now)
                .SaveToDb()
                .Build();

            var transortOrderProductUpdated = TransportOrderInterfaceFacade.TransportOrderProduct.New(1)
                .WithOrderLineID($"{transportOrer.Code}-1")
                .WithProductCode(defaultProduct.ProductCode)
                .WithOrderProductNumber(qtyUpdated)
                .WithOrderProductValue(defaultProduct.Value * qtyUpdated)
                .WithTotalOnly(Convert.ToInt32(false))
                .WithActualNumber(qtyUpdated)
                .WithActualValue(defaultProduct.Value * qtyUpdated)
                .WithCurrency("EUR")
                .WithPreanQty(qtyUpdated)
                .WithPreanValue(defaultProduct.Value * qtyUpdated)
                .WithPackNumber(defaultOrderID)
                .WithReject(Convert.ToInt32(false))
                .WithLocationNumber(fixture.defaultLocation.Code).SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundTransportOrderProduct = context.Cwc_Transport_TransportOrderProducts.FirstOrDefault(x=>x.TransportOrderID == transportOrer.ID);

            Assert.NotNull(foundTransportOrderProduct);
            Assert.Equal(qtyUpdated, foundTransportOrderProduct.OrderedQuantity);
            Assert.Equal(qtyUpdated * defaultProduct.Value, foundTransportOrderProduct.OrderedValue);
        }

        [Fact(DisplayName = "When transport order product is valid Then System deletes it")]
        public void VerifyThatSystemDeletesTransportOrderProductProperly()
        {
            var qty = 2;
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_Site(fixture.defaultLocation.BranchID)
                .With_ServiceType(fixture.deliveryServiceType)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(defaultTransportOrderDate)
                .With_ServiceDate(serviceOrder.ServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder)
                .SaveToDb().Build();

            var transortOrderProduct = DataFacade.TransportOrderProduct.New()
                .With_TransportOrder(transportOrer)
                .With_Product(defaultProduct)
                .With_OrderedQuantity(qty)
                .With_OrderedValue(qty * (int)defaultProduct.Value)
                .With_CurrencyID("EUR")
                .With_DateCreated(DateTime.Now)
                .With_DateUpdated(DateTime.Now)
                .SaveToDb()
                .Build();

            var transortOrderProductNonUnique = TransportOrderInterfaceFacade.TransportOrderProduct.New(2)
                .WithOrderLineID($"{transportOrer.Code}-1")
                .WithProductCode(defaultProduct.ProductCode)
                .WithOrderProductNumber(qty)
                .WithOrderProductValue(defaultProduct.Value * qty)
                .WithTotalOnly(Convert.ToInt32(false)).SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundTransportOrderProduct = context.Cwc_Transport_TransportOrderProducts.FirstOrDefault(x=>x.TransportOrderID == transportOrer.ID);

            Assert.Null(foundTransportOrderProduct);
        }

        [Theory(DisplayName = "When Actual number / value is passed and only one Service Order Product exisst Then System updates it")]
        [InlineData("10", null)]
        [InlineData(null, "10")]
        [InlineData("10", "10")]
        public void VerifyThatSystemAllowToUpdateServiceOrderProduct(string actualNumber, string actualValue)
        {

        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
