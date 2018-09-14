using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.FeedingBuilder;
using CWC.AutoTests.Tests.Fixtures;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.AutoTests
{
    [Collection("MyCollection")]
    public class ServiceOrderFeedingTests : IDisposable, IClassFixture<OrderFixture>//, IClassFixture<CleanUpFxture>, IClassFixture<ServiceOrderCleanUpFixture> 
    {
        string refNum;
        AutomationOrderingDataContext _context;
        static OrderFixture _fixture;
        string serviceDate = DateTime.Today.ToString("yyyy-MM-dd hh:mm:ss");

        public ServiceOrderFeedingTests(OrderFixture fixture)
        {
            _context = new AutomationOrderingDataContext();
            refNum = $"1101{new Random(DateTime.Now.Ticks.GetHashCode()).Next(1212, 12121211)}";
            _fixture = fixture;
        }

        [Fact(DisplayName = "Service Order Feeding import - When ProductFeeding doesn't contain any {productCode, material, IsTotal = yes} Then Sysytem doesn't allow to import this service Order")]
        public void VerifyThatProductFeedingShouldContainAnyOFMandatoryFields()
        {
            var serviceType = "DELV";            
            var product = DataFacade.Product.Take(p => p.UnitName.Contains("Bundel 5")).Build();
            var productNumber = 2;
            var prodValue = product.Value * productNumber;
            var serviceOrder = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_CurrencyCode(_fixture.Customer.ReferenceNumber)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType)
                .With_CurrencyCode("EUR")
                .With_Location(_fixture.Location.Code)
                .With_OrderLine(_fixture.Location.Code)
                .With_OrderLineProduct("", "0", "", "", "", "")
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrder.ToString());

            Assert.Equal(response.Body.ErrorMessage, "Product Feeding does not contain neither Product Code nor Material ID nor pre-announced Total Value");
            Assert.False(_context.Orders.Where(so => so.ReferenceID == refNum).Any());
        }

        [Fact(DisplayName = "Service Order Feeding import - When Customer is not set Then System uses Location -> Customer")]
        public void VerifyWhenCustomerIsNotSetThenSystemUsesLocationsCustomer()
        {
            var serviceType = "DELV";            
            var locationCode = _fixture.Location.Code;
            var customerID = _fixture.Location.CompanyID;
            var product = DataFacade.Product.Take(p => p.UnitName.Contains("Bundel 5")).Build();
            var productNumber = 2;
            var prodValue = product.Value * productNumber;
            var serviceOrder = FeedingBuilderFacade.ServiceOrderFeeding.New().
                With_ReferenceID(refNum).
                With_ServiceDate(serviceDate).
                With_ServiceCode(serviceType).
                With_Location(locationCode).
                With_CurrencyCode("EUR").
                With_OrderLine(locationCode).
                With_OrderLineProduct(product.ProductCode, productNumber.ToString(), prodValue.ToString()).
                Build();
            var respomse = HelperFacade.FeedingHelper.SendFeeding(serviceOrder.ToString());

            Assert.True(_context.Orders.Where(so => so.ReferenceID == refNum && so.CustomerID == customerID).Any());
        }

        [Fact(DisplayName = "Service Order Feeding import - When Service Order is cretated Then System created SOline")]
        public void VerifyThatSystemCreatesSoline()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();            
            var locationCode = _fixture.Location.Code;
            var customerID = _fixture.Location.CompanyID;
            var product = DataFacade.Product.Take(p => p.UnitName.Contains("Bundel 5")).Build();
            var productNumber = 2;
            var prodValue = product.Value * productNumber;
            var serviceOrder = FeedingBuilderFacade.ServiceOrderFeeding.New().
                With_ReferenceID(refNum).
                With_ServiceDate(serviceDate).
                With_ServiceCode(serviceType.Code).
                With_Location(locationCode).
                With_CurrencyCode("EUR").
                With_OrderLine(_fixture.Location.Code).
                With_OrderLineProduct(product.ProductCode, productNumber.ToString(), prodValue.ToString()).
                Build();
            var respomse = HelperFacade.FeedingHelper.SendFeeding(serviceOrder.ToString());
            var orderid = _context.Orders.Where(so => so.ReferenceID == refNum).Select(x => x.WPOrderID).First().ToString();
            var orderLine = _context.OrderLines.Where(soline => soline.OrderID == orderid && soline.ID == orderid + "-1");

            Assert.True(orderLine.Any(), "OrderLine i not found");
            Assert.Equal(orderLine.Select(x => x.LocationID).First(), _fixture.Location.ID);
            Assert.Equal(orderLine.Select(x => x.ServiceType).First(), serviceType.OldType);
            Assert.Equal(orderLine.Select(x => x.OrderLineValue).First(), prodValue);
        }

        [Fact(DisplayName = "Service Order Feeding import - When Order line contains multiple products Then Order Line value is a sum of all linked products", Skip = "CWC8657 should be fixed")]
        public void VerifyThatSystemCalculatesOrderLineValueProperly()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var product1 = DataFacade.Product.Take(p => p.UnitName.Contains("Bundel 5")).Build();
            var productNumber1 = 2;
            var prodValue1 = product1.Value * productNumber1;
            var product2 = DataFacade.Product.Take(p => p.UnitName.Contains("Bundel 10")).Build();
            var productNumber2 = 2;
            var prodValue2 = product2.Value * productNumber2;
            var serviceOrder = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                    .With_OrderLineProduct(product1.ProductCode, productNumber1.ToString(), prodValue1.ToString())
                    .With_OrderLineProduct(product2.ToString(), productNumber2.ToString(), prodValue2.ToString()).Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrder.ToString());
            var orderId = _context.Orders.Where(x => x.ReferenceID == refNum).Select(x => x.WPOrderID).First();

            Assert.Equal(_context.OrderLines.Where(x => x.ID == orderId + "-1").Select(x => x.OrderLineValue).First(), (prodValue1 + prodValue2));
        }

        [Fact(DisplayName = "Service Order Feeding import - When order feeding is send with product feeding Then System creates SOProduct record")]
        public void VerifyThatSystemCreatesProductProperly()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();            
            var product = DataFacade.Product.Take(p => p.UnitName.Contains("Bundel 5")).Build();
            var productNumber = 2;
            var prodValue = product.Value * productNumber;
            var serviceOrder = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .With_OrderLineProduct(product.ProductCode, productNumber.ToString(), prodValue.ToString())
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrder.ToString());
            var orderId = _context.Orders.Where(x => x.ReferenceID == refNum).Select(x => x.WPOrderID).First();
            var soproducts = _context.SOProduct.Where(p => p.OrderLine_ID == orderId + "-1");

            Assert.True(soproducts.Any(), "ServiceProduct is not found");
            Assert.Equal(soproducts.Select(x => x.ProductCode).First(), product.ProductCode);
            Assert.Equal(soproducts.Select(x => x.OrderProductNumber).First(), productNumber);
            Assert.Equal(soproducts.Select(x => x.OrderProductValue).First(), prodValue);
            Assert.Equal(soproducts.Select(x => x.Currency).First(), "EUR");
        }

        [Fact(DisplayName = "Service Order Feeding import - When order feeding contains collection product with IsTotal is no Then Sysem creates proper order products")]
        public void VerifyThatNonTotalCollectOrderCreatedProperly()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var material = DataFacade.Material.Take(x => x.Type == "NOTE" && x.Description.Contains("Eur"));
            var preanQty = 10;
            var preanVal = material.Build().Denomination * preanQty;
            var serviceOrder = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .With_OrderLineProduct("PN" + refNum, "0", "0", preanQty.ToString(), preanVal.ToString(), material.Build().MaterialID).
                Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrder.ToString());
            var orderId = _context.Orders.Where(x => x.ReferenceID == refNum).Select(x => x.WPOrderID).First();
            var soproducts = _context.SOProduct.Where(p => p.OrderLine_ID == orderId + "-1");

            Assert.True(soproducts.Any(), "ServiceProduct is not found");
            Assert.Equal("PN" + refNum, soproducts.First().PackNr);
            Assert.Equal(soproducts.First().Material_id, material.Build().MaterialID);
            Assert.Equal(soproducts.First().PreanQty, preanQty);
            Assert.Equal(soproducts.First().PreanValue, preanVal);
            Assert.Equal(soproducts.First().TotalOnly, false);
            Assert.Equal(soproducts.First().Reject, false);
        }

        [Fact(DisplayName = "Service Order Feeding import - When order feeding contains collection product with IsTotal is yes Then Sysem creates proper order products", Skip = "CWC8658 should be fixed")]
        public void VerifyThatTotalCollectOrderCreatedProperly()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();            
            var preanVal = 200;
            var serviceOrder = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .With_OrderLineProduct("PN" + refNum, "1", "EUR", preanVal.ToString(), "1")
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrder.ToString());
            var orderId = _context.Orders.Where(x => x.ReferenceID == refNum).Select(x => x.WPOrderID).First();
            var soproducts = _context.SOProduct.Where(p => p.OrderLine_ID == orderId + "-1");

            Assert.True(soproducts.Any(), "ServiceProduct is not found");
            Assert.Equal("PN" + refNum, soproducts.First().PackNr);
            Assert.Null(soproducts.First().Material_id);
            Assert.Equal(0, soproducts.First().PreanQty);
            Assert.Equal(preanVal, soproducts.First().PreanValue);
            Assert.Equal(true, soproducts.First().TotalOnly);
            Assert.Equal(true, soproducts.First().Reject);
            Assert.Equal("EUR", soproducts.First().Currency);
        }

        [Fact(DisplayName = "When package number is non unique Then System doesn't allow to import this order", Skip = "CWC8670")]
        public void VerifyThatSystemDoesntAllowToImportNonUniquePckageNumber()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();            
            var preanVal = 200;
            var serviceOrder = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .With_OrderLineProduct("PN" + refNum, "1", "EUR", preanVal.ToString(), "1")
                .With_OrderLineProduct("PN" + refNum, "2", "EUR", preanVal.ToString(), "1").Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrder.ToString());
            var assertMsg = $"Package number(s) PN{refNum} already used. Please, enter other number(s)";

            Assert.Equal(0, _context.Orders.Where(x => x.ReferenceID == refNum).Count());
            Assert.True(response.Body.ErrorMessage.Contains(assertMsg));
        }

        [Fact(DisplayName = "When package number is non unique in diff orders Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportNonUniquePckageNumberInDiffOrders()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();            
            var preanVal = 200;
            var serviceOrder = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .With_OrderLineProduct("PN" + refNum, "1", "EUR", preanVal.ToString(), "1")
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrder.ToString());

            serviceOrder = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum + "1")
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .With_OrderLineProduct("PN" + refNum, "1", "EUR", preanVal.ToString(), "1")
                .Build();
            response = HelperFacade.FeedingHelper.SendFeeding(serviceOrder.ToString());

            var assertMsg = $"Package number(s) PN{refNum} already used. Please, enter other number(s)";

            Assert.Equal(0, _context.Orders.Where(x => x.ReferenceID == refNum + "1").Count());
            Assert.True(response.Body.ErrorMessage.Contains(assertMsg));
        }

        [Fact(DisplayName = "Service Order Feeding import - When Servicing code is submitted Then System created Servicing order")]
        public void VerifyThatSOServiceCreatedProperly()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();            
            var firstService = _context.ServicingCodes.First().Code;
            var serviceOrder = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .With_OrderLineService(firstService)
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrder.ToString());
            var orderId = _context.Orders.Where(x => x.ReferenceID == refNum).Select(x => x.WPOrderID).First();
            var serviceFirst = _context.OrderLineServices.Where(p => p.OrderLineID == orderId + "-1");

            Assert.True(serviceFirst.Any(), "SOService wasm't found");
            Assert.Equal(firstService, serviceFirst.First().ServiceCode);
            Assert.Equal(false, serviceFirst.First().IsServicePerformed);
            Assert.Equal(true, serviceFirst.First().IsServicePlanned);
        }

        [Fact(DisplayName = "Service Order Feeding import - When sevaral Servicing codes is submitted Then System created Servicing order")]
        public void VerifyThatSeveralSOServicesCreatedProperly()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();           
            var firstService = _context.ServicingCodes.First().Code;
            var secondService = _context.ServicingCodes.Where(x => x.Code != firstService).First().Code;
            var serviceOrder = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.LocationATM.Code)
                .With_Customer(_fixture.Customer.Cus_nr.ToString())
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                    .With_OrderLineService(firstService)
                    .With_OrderLineService(secondService)
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrder.ToString());
            var orderId = _context.Orders.Where(x => x.ReferenceID == refNum).Select(x => x.WPOrderID).First();

            Assert.True(_context.OrderLineServices.Where(x => x.OrderLineID == orderId + "-1" && x.ServiceCode == firstService).Any());
            Assert.True(_context.OrderLineServices.Where(x => x.OrderLineID == orderId + "-1" && x.ServiceCode == secondService).Any());
        }

        [Fact(DisplayName = "Service Order Feeding import - When reference id is not submitted Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportOrderWithoutRefrenceID()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var order = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID("")
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(order.ToString());

            Assert.Equal(response.Body.ErrorMessage, "Mandatory attribute 'ReferenceID' is not submitted.");
        }

        [Fact(DisplayName = "Service Order Feeding import - When service date is not submitted Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportOrderWithoutServiceDare()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var order = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate("")
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(order.ToString());

            Assert.Equal(response.Body.ErrorMessage, "Mandatory attribute 'Service_Date' is not submitted.");
            Assert.False(_context.Orders.Where(s => s.WPOrderID.ToString() == refNum).Any());
        }

        [Fact(DisplayName = "Service Order Feeding import - When service date is non order day Then System doesn't allow to import this order", Skip = "Create method to define non order day")]
        public void VerifyThatOrderCannotBePlacedInNonOrderDay()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();            
            var serviceDate = DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd hh:mm:ss");
            var order = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(order.ToString());

            Assert.Equal(response.Body.ErrorMessage, "Placing order on non-order day is prohibited by applied contract ordering settings.");
            Assert.False(_context.Orders.Where(s => s.WPOrderID.ToString() == refNum).Any());
        }

        [Fact(DisplayName = "Service Order Feeding import - When service type is not submitted Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportOrderWithoutServiceType()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();            
            var order = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode("")
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(order.ToString());

            Assert.Equal(response.Body.ErrorMessage, "Mandatory attribute 'WP_ServiceType_Code' is not submitted.");
        }

        [Fact(DisplayName = "Service Order Feeding import - When location is not submitted Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportOrderWithoutLocation()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var order = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location("")
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(order.ToString());

            Assert.Equal(response.Body.ErrorMessage, "Mandatory attribute 'WP_ref_loc_nr' is not submitted.");
        }

        [Fact(DisplayName = "Service Order Feeding import - When feeding contains non existed location Then System doesn't allow to import it")]
        public void VerifyThatSystemNotAllowsToImportServiceOrderWithNonExistedLocation()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var order = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location("dvervvve")
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(order.ToString());

            Assert.Equal(response.Body.ErrorMessage, "Location with provided Code 'dvervvve' does not exist.");
        }

        [Fact(DisplayName = "Service Order Feeding import - When feeding contains non existed customer Then System doesn't allow to import it", Skip = "CWC8663 should be fixed")]
        public void VerifyThatSystemNotAllowsToImportServiceOrderWithNonExistedCustomer()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var order = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(refNum)
                .With_Customer("qaqaqaqaqa")
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(order.ToString());

            Assert.Equal(response.Body.ErrorMessage, "Customer 'qaqaqaqaqa' does not exist in data base.");
        }

        [Fact(DisplayName = "Service Order Feeding import - When feeding contains non existed service type Then System doesn't allow to import it")]
        public void VerifyThatSystemNotAllowsToImportServiceOrderWithNonExistedServiceType()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var order = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode("qaqaqaqa")
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_OrderLine(_fixture.Location.Code)
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(order.ToString());

            Assert.Equal(response.Body.ErrorMessage, "Service Type with provided Code ‘qaqaqaqa’ does not exist.");
        }

        [Fact(DisplayName = "Service Order Feeding import - When feeding is valid Then System creates Service Order with proper attributes")]
        public void VerifyThatServiceOrderIsCreatedWithProperAttributes()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();            
            var product = DataFacade.Product.Take(p => p.UnitName.Contains("Bundel 5")).Build();
            var productNumber = 2;
            var prodValue = product.Value * productNumber;

            var order = FeedingBuilderFacade.ServiceOrderFeeding.New()
                .With_ReferenceID(refNum)
                .With_BankReferenceID(refNum)
                .With_CiteferenceID(refNum)
                .With_ServiceDate(serviceDate)
                .With_ServiceCode(serviceType.Code)
                .With_Location(_fixture.Location.Code)
                .With_CurrencyCode("EUR")
                .With_Customer(_fixture.Customer.ReferenceNumber)
                .With_Email("example@mm.com")
                .With_OrderLine(_fixture.Location.Code)
                .With_OrderLineProduct(product.ProductCode, productNumber.ToString(), prodValue.ToString())
                .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(order.ToString());
            var foundOrder = _context.Orders.Where(s => s.ReferenceID == refNum);

            Assert.True(foundOrder.Any(), "Service order is not created");
            Assert.Equal(refNum, foundOrder.First().ReferenceID);
            Assert.Equal(refNum, foundOrder.First().BankReference);
            Assert.Equal(refNum, foundOrder.First().CITReference);
            Assert.Equal("EUR", foundOrder.First().CurrencyCode);
            Assert.Equal(_fixture.Location.Code, foundOrder.First().LocationCode);
            Assert.Equal(_fixture.Location.ID, foundOrder.First().LocationID);
            Assert.Equal(serviceType.Code, foundOrder.First().ServiceTypeCode);
            Assert.Equal(Cwc.Contracts.OrderType.AtRequest, foundOrder.First().OrderType);
            Assert.Equal(prodValue, foundOrder.First().OrderedValue);
            Assert.Equal(foundOrder.First().ServiceDate, foundOrder.First().NewServiceDate);
            Assert.Equal(DateTime.Parse(serviceDate), foundOrder.First().ServiceDate);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
