using Cwc.BaseData;
using Cwc.Feedings;
using Cwc.Ordering;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.FeedingBuilder;
using CWC.AutoTests.Tests.Fixtures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.AutoTests
{
    [Xunit.Collection("MyCollection")]
    public class ServiceOrderListTests : IClassFixture<OrderFixture>, IDisposable
    {
        AutomationOrderingDataContext orderingContext;
        AutomationFeedingDataContext feedingContext;
        OrderFixture _fixture;
        Material material, secondMaterial;
        Product product;
        string today = DateTime.Now.Date.ToString("yyyy-MM-dd");
        string serviceDate;
        string serviceTypeDelv = "DELV";
        string serviceTypeColl = "COLL";
        string nonexistedCode;
        string refNum;


        public ServiceOrderListTests(OrderFixture fixture)
        {
            _fixture = fixture;
            orderingContext = new AutomationOrderingDataContext();
            feedingContext = new AutomationFeedingDataContext();
            refNum = $"1101{new Random(DateTime.Now.Ticks.GetHashCode()).Next(1212, 12121211)}";
            serviceDate = DateTime.Today.ToString("yyyy-MM-dd hh:mm:ss");
            Assert.True(serviceDate != null, "ServiceDate is not specified");
            product = DataFacade.Product.Take(p => p.Description.Contains("Bundel") && p.Value < 20000).Build();
            nonexistedCode = "qaqaqaqa";
            material = DataFacade.Material.Take(x => x.Type == "NOTE").Build();
            secondMaterial = DataFacade.Material.Take(x => x.Type == "NOTE" && x.MaterialID != material.MaterialID).Build();
        }

        public void Dispose()
        {
            orderingContext.SOProduct.RemoveRange(orderingContext.SOProduct.Where(sop => sop.OrderLine_ID.Contains(refNum)));
            orderingContext.OrderLines.RemoveRange(orderingContext.OrderLines.Where(o => o.OrderID.Contains(refNum)));
            orderingContext.Orders.RemoveRange(orderingContext.Orders.Where(o => o.ID.Contains(refNum)));
            orderingContext.SaveChanges();
            feedingContext.Dispose();
            orderingContext.Dispose();
        }

        [Fact(DisplayName = "Service Order List Feeding - When Service Date is empty Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportWithoutServiceDate()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(null).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("Mandatory attribute 'ServiceDate' is not submitted") && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Service Order List Feeding - When Location Code is empty Then System doesn't allow to import this order")]
        [InlineData("")]
        [InlineData(null)]
        public void VerifyThatSystemDoesntAllowToImportWithoutLocationCode(string locationCodePar)
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(locationCodePar).
                    With_ServiceTypeCode(serviceTypeDelv).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("Mandatory attribute 'LocationCode' is not submitted") && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Service Order List Feeding - When Service Type i empty Then System doesn't allow to import this order")]
        [InlineData("")]
        [InlineData(null)]
        public void VerifyThatSystemDoesntAllowToImportWithoutServiceType(string serviceTypePar)
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypePar).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("Mandatory attribute 'ServiceTypeCode' is not submitted") && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Service Order List Feeding - When Product is submitted Then Product Code should be specified")]
        [InlineData("")]
        [InlineData(null)]
        public void VerifyThatDeliveryProductShouldContainProductCode(string productCode)
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_DeliveryProducts().
                        With_Product().
                            With_ProductCode(productCode).
                            With_ProductQuantity(5).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("Mandatory attribute 'ProductCode' is not submitted") && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Product is submitted Then quantity or value should be specified")]
        public void VerifyThatDeliveryProductShouldContainQuantity()
        {
            try
            {
                var product = DataFacade.Product.Take(p => p.ProductCode == "27").Build();

                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_DeliveryProducts().
                        With_Product().
                            With_ProductCode(product.ProductCode).
                            With_ProductQuantity(null).
                            With_ProductValue(null).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("for delivery no quantity and value are passed") && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }


        [Theory(DisplayName = "Service Order List Feeding - When Collect Package is submitted without package number Then System doesn't allow to import current order")]
        [InlineData("")]
        [InlineData(null)]
        public void VerifyThatUponImportOfCollectPacckageNumberIsMandatory(string packageNumber)
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_CollectedPackages().
                        With_Package().
                            With_PackageNumber(packageNumber).
                        With_Materials().
                            With_Material().
                                With_MaterialID(material.MaterialID).
                                With_MaterialQuantity(10).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("Mandatory attribute 'PackageNumber' is not submitted") && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Service Order List Feeding - When Collect Package Material is submitted without MaterialID Then System doesn't allow to import current order")]
        [InlineData("")]
        [InlineData(null)]
        public void VerifyThatUponImportOfCollectPacckageMaterialCodeIsMandatory(string materialCode)
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_CollectedPackages().
                        With_Package().
                            With_PackageNumber(refNum).
                        With_Materials().
                            With_Material().
                                With_MaterialID(materialCode).
                                With_MaterialQuantity(10).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("Mandatory attribute 'MaterialID' is not submitted") && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Collect Package Material is submitted without Material quantity Then System doesn't allow to import current order")]
        public void VerifyThatUponImportOfCollectPacckageMaterialQuantityIsMandatory()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_CollectedPackages().
                        With_Package().
                            With_PackageNumber(refNum).
                        With_Materials().
                            With_Material().
                                With_MaterialID(material.MaterialID).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("Mandatory attribute 'Quantity' is not submitted") && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Collect Package Total is submitted without Value Then System doesn't allow to import current order")]
        public void VerifyThatUponImportOfCollectPacckageTotalValueIsMandatory()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_CollectedPackages().
                        With_Package().
                            With_PackageNumber(refNum).
                        With_Total().
                            With_TotalCurrency("EUR").
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("Mandatory attribute 'Value' is not submitted") && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Service Order List Feeding - When Collect Package Total is submitted without Currency Then System doesn't allow to import current order")]
        [InlineData("")]
        [InlineData(null)]
        public void VerifyThatUponImportOfCollectPacckageTotalCurrencyIsMandatory(string currency)
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_CollectedPackages().
                        With_Package().
                            With_PackageNumber(refNum).
                        With_Total().
                            With_TotalValue(10).
                            With_TotalCurrency(currency).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("Mandatory attribute 'CurrencyCode' is not submitted") && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Services is submitted without Servicing Code Then System doesn't allow to import current order")]
        public void VerifyThatUponImportOServicesServiceCodeMandatory()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_Services().
                        With_Service().
                        With_ServicingCode("").
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("Mandatory attribute 'ServiceCode' is not submitted") && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order is submitted with non existed Location Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportOrderWithNonExistedLocation()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(nonexistedCode).
                    With_ServiceTypeCode(serviceTypeDelv).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());
                var assertMsg = $"Location with provided Code ‘{nonexistedCode}’ does not exist.";

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains(assertMsg) && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order is submitted with non existed Pick up Location Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportOrderWithNonExistedPickLocation()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_PickLocationCode(nonexistedCode).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());
                var assertMsg = $"Location with provided Code ‘{nonexistedCode}’ does not exist.";

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains(assertMsg) && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order is submitted with non existed Service Type Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportOrderWithNonExistedServiceType()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(nonexistedCode).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());
                var assertMsg = $"Service Type with provided Code ‘{nonexistedCode}’ does not exist.";

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains(assertMsg) && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order is submitted with non existed Currency Code Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportOrderWithNonExistedCurrencyCode()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_CurrencyCode(nonexistedCode).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());
                var assertMsg = $"Currency with provided Code ‘{nonexistedCode}’ does not exist.";

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains(assertMsg) && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order is submitted with non existed Cancel Reason Code Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportOrderWithNonExistedCancelReasonCode()
        {
            try
            {
                var nonexistedCode = 4124;

                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_CancelReasonCode(nonexistedCode).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());
                var assertMsg = $"Reason Code with provided Code ‘{nonexistedCode}’ does not exist.";

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains(assertMsg) && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order is submitted with non existed Product Code Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportOrderWithNonExistedProduct()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_DeliveryProducts().
                        With_Product().
                            With_ProductCode(nonexistedCode).
                            With_ProductQuantity(10).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());
                var assertMsg = $"Product with provided Code ‘{nonexistedCode}’ does not exist.";

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains(assertMsg) && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order is submitted with non existed MaterialID Then System doesn't allow to import this order")]
        public void VerifyThatSystemDoesntAllowToImportOrderWithNonExistedMaterial()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_CollectedPackages().
                        With_Package().
                            With_PackageNumber(refNum).
                        With_Materials().
                            With_Material().
                                With_MaterialID(nonexistedCode).
                                With_MaterialQuantity(10).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());
                var assertMsg = $"Material with provided Material ID ‘{nonexistedCode}’ does not exist.";

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains(assertMsg) && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Services is submitted without Servicing Code Then System doesn't allow to import current order")]
        public void VerifyThatSystemDoesntAllowToImportOrderWithNonExistedServiceCode()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeDelv).
                    With_Services().
                        With_Service().
                        With_ServicingCode(nonexistedCode).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());
                var assertMsg = $"Service Code with provided Code ‘{nonexistedCode}’ does not exist.";

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains(assertMsg) && l.Result == ValidatedFeedingLogResult.Failed).Any());
                Assert.False(orderingContext.Orders.Where(s => s.ID == refNum).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order is submitted with valid Products Then System creates both ServiceOrder, -Line, -Products")]
        public void VerifyThatSystemCreatesServiceOrderWithDeliveryProductsProperly()
        {
            try
            {
                var firstProductAmount = 10;

                var secondProduct = DataFacade.Product.Take(p => p.Description.Contains("Bundel") && p.ProductCode != product.ProductCode && p.Value < 20000).Build();
                var secondProductAmount = 5;

                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeDelv)
                    .With_DeliveryProducts()
                        .With_Product()
                            .With_ProductCode(product.ProductCode)
                            .With_ProductQuantity(firstProductAmount)
                        .With_Product()
                            .With_ProductCode(secondProduct.ProductCode)
                            .With_ProductQuantity(secondProductAmount)
                    .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("successfully created") && l.Result == 0).Any());
                Assert.True(orderingContext.Orders.Where(s => s.ID == refNum).Any());
                Assert.True(orderingContext.OrderLines.Where(s => s.OrderID == refNum && s.ID == refNum + "-1").Any());
                Assert.Equal(2, orderingContext.SOProduct.Where(s => s.OrderLine_ID == refNum + "-1").Count());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order Products are exists and new feeding is sent wihtout DeliveryProducts tag Then System doesn't perform any actions on Order Products")]
        public void VerifyThatSystemDoesntRemovesProductsWhenDeliveryProducttagIsNotSent()
        {
            try
            {
                var firstProductAmount = 10;

                var secondProduct = DataFacade.Product.Take(p => p.Description.Contains("Bundel") && p.ProductCode != product.ProductCode && p.Value < 20000).Build();
                var secondProductAmount = 5;

                //Create Order
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeDelv)
                    .With_DeliveryProducts()
                        .With_Product()
                            .With_ProductCode(product.ProductCode)
                            .With_ProductQuantity(firstProductAmount)
                        .With_Product()
                            .With_ProductCode(secondProduct.ProductCode)
                            .With_ProductQuantity(secondProductAmount)
                    .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                //Update Order
                serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeDelv).Build();

                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("successfully updated") && l.Result == 0).Any());
                Assert.Equal(2, orderingContext.SOProduct.Where(s => s.OrderLine_ID == refNum + "-1").Count());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When DeliveryProducts tag is not sent for new Order Then this Order doesn't contain any products")]
        public void VerifyThatNoDeliveryProductsAreCreatedWhenDeliveryProductsTagIsNotSent()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeDelv).Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("successfully created") && l.Result == 0).Any());
                Assert.True(orderingContext.Orders.Where(s => s.ID == refNum).Any());
                Assert.True(orderingContext.OrderLines.Where(s => s.OrderID == refNum && s.ID == refNum + "-1").Any());
                Assert.Equal(0, orderingContext.SOProduct.Where(s => s.OrderLine_ID == refNum + "-1").Count());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order Products are exists and new feeding is sent wiht empty DeliveryProducts tag Then System deletes all linked Order Products")]
        public void VerifyThatSystemRemovesProductsWhenDeliveryProducttagIsSentEmptyt()
        {
            try
            {

                var firstProductAmount = 1;

                var secondProduct = DataFacade.Product.Take(p => p.Description.Contains("Bundel") && p.ProductCode != product.ProductCode && p.Value < 20000).Build();
                var secondProductAmount = 1;

                //Create Order
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeDelv)
                    .With_DeliveryProducts()
                        .With_Product()
                            .With_ProductCode(product.ProductCode)
                            .With_ProductQuantity(firstProductAmount)
                        .With_Product()
                            .With_ProductCode(secondProduct.ProductCode)
                            .With_ProductQuantity(secondProductAmount)
                    .Build();


                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                //Update Order
                serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeDelv).With_DeliveryProducts().Build();

                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("successfully updated") && l.Result == 0).Any());
                Assert.Equal(0, orderingContext.SOProduct.Where(s => s.OrderLine_ID == refNum + "-1").Count());
            }
            catch
            {
                throw;
            }
        }


        [Fact(DisplayName = "Service Order List Feeding - When existed service order is submitted with new products Then System replaces old products with the new one")]
        public void verifyWhenServiceOrderIsUpdatedThenSystemChangesExistedProductsToTheNewOne()
        {
            try
            {
                var firstProductAmount = 1;

                var secondProduct = DataFacade.Product.Take(p =>
                                                                p.Description.Contains("Bundel")
                                                                && p.ProductCode != product.ProductCode
                                                                && p.Value < 20000)
                                                                .Build();
                var secondProductAmount = 1;

                var thirdProduct = DataFacade.Product.Take(p =>
                                                                p.Description.Contains("Bundel")
                                                                && p.ProductCode != product.ProductCode
                                                                && p.ProductCode != secondProduct.ProductCode
                                                                && p.Value < 20000)
                                                                .Build();
                var thirdProductAmount = 1;

                //Create Order
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl)
                    .With_DeliveryProducts()
                        .With_Product()
                            .With_ProductCode(product.ProductCode)
                            .With_ProductQuantity(firstProductAmount)
                        .With_Product()
                            .With_ProductCode(secondProduct.ProductCode)
                            .With_ProductQuantity(secondProductAmount)
                    .Build();


                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                //Update Order
                serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl)
                    .With_DeliveryProducts()
                        .With_Product()
                            .With_ProductCode(thirdProduct.ProductCode)
                            .With_ProductQuantity(thirdProductAmount)
                    .Build();

                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("successfully updated") && l.Result == 0).Any());
                Assert.Equal(1, orderingContext.SOProduct.Where(s => s.OrderLine_ID == refNum + "-1").Count());
                Assert.True(orderingContext.SOProduct.Where(s => s.OrderLine_ID == refNum + "-1" && s.ProductCode == thirdProduct.ProductCode).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order is submitted with valid Materials Then System creates both ServiceOrder, -Line, -Products")]
        public void VerifyThatSystemCreatesServiceOrderWithCollectedPackageMaterialProperly()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl)
                    .With_CollectedPackages()
                        .With_Package()
                            .With_PackageNumber(refNum)
                        .With_Materials()
                            .With_Material()
                                .With_MaterialID(material.MaterialID)
                                .With_MaterialQuantity(10)
                            .With_Material()
                                .With_MaterialID(secondMaterial.MaterialID)
                                .With_MaterialQuantity(5)
                        .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("successfully created") && l.Result == 0).Any());
                Assert.Equal(1, orderingContext.OrderLines.Where(sl => sl.OrderID == refNum).Count());
                Assert.Equal(2, orderingContext.SOProduct.Where(sp => sp.OrderLine_ID == refNum + "-1").Count());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Products are existed for Order and CollectedPackage is not sent Then this products are not changed")]
        public void VerifyThatWhenCollectedPackageIsNotSubmittedInExistedOrderSystemNotRemovesExistedProducts()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl)
                    .With_CollectedPackages()
                        .With_Package()
                            .With_PackageNumber(refNum)
                        .With_Materials()
                            .With_Material()
                                .With_MaterialID(material.MaterialID)
                                .With_MaterialQuantity(10)
                            .With_Material()
                                .With_MaterialID(secondMaterial.MaterialID)
                                .With_MaterialQuantity(5)
                        .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeColl).
                    Build();

                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("successfully updated") && l.Result == 0).Any());
                Assert.Equal(2, orderingContext.SOProduct.Where(sp => sp.OrderLine_ID == refNum + "-1").Count());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Products are existed for Order and CollectedPackage tag is sent Then this products should be removed")]
        public void VerifyThatWhenCollectedPackageIsSubmittedInExistedOrderSystemRemovesExistedProducts()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl)
                    .With_CollectedPackages()
                        .With_Package()
                            .With_PackageNumber(refNum)
                        .With_Materials()
                            .With_Material()
                                .With_MaterialID(material.MaterialID)
                                .With_MaterialQuantity(10)
                            .With_Material()
                                .With_MaterialID(secondMaterial.MaterialID)
                                .With_MaterialQuantity(5)
                        .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder().
                    With_Number(refNum).
                    With_ServiceDate(serviceDate).
                    With_LocationCode(_fixture.Location.Code).
                    With_ServiceTypeCode(serviceTypeColl).
                    With_CollectedPackages().
                    Build();

                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("successfully updated") && l.Result == 0).Any());
                Assert.Equal(1, orderingContext.Orders.Where(s => s.ID == refNum).Count());
                Assert.Equal(1, orderingContext.OrderLines.Where(sp => sp.OrderID == refNum && sp.ID == refNum + "-1").Count());
                Assert.Equal(0, orderingContext.SOProduct.Where(sp => sp.OrderLine_ID == refNum + "-1").Count());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order is submitted with valid Totald Then System creates both ServiceOrder, -Line, -Products", Skip = "Waiting for change #TTCWC0066")]
        public void VerifyThatSystemCreatesServiceOrderWithCollectedPackageTotalProperly()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl)
                    .With_CollectedPackages()
                        .With_Package()
                            .With_PackageNumber(refNum)
                        .With_Total()
                            .With_TotalValue(10)
                            .With_TotalCurrency("EUR")
                        .With_Total()
                            .With_TotalValue(20)
                            .With_TotalCurrency("USD")
                    .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("successfully created") && l.Result == 0).Any());
                Assert.Equal(1, orderingContext.OrderLines.Where(sl => sl.OrderID == refNum).Count());
                Assert.Equal(2, orderingContext.SOProduct.Where(sp => sp.OrderLine_ID == refNum + "-1").Count());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Total Products are existed for Order and CollectedPackage is not sent Then this products are not changed", Skip = "Waiting for change #TTCWC0066")]
        public void VerifyThatSystemDoesntPerformAnyActionsOnProductsWhenCollectedPackagesTagIsNotSet()
        {
            try
            {
                //create order
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl)
                    .With_CollectedPackages()
                        .With_Package()
                            .With_PackageNumber(refNum)
                        .With_Total()
                            .With_TotalValue(10)
                            .With_TotalCurrency("EUR")
                        .With_Total()
                            .With_TotalValue(20)
                            .With_TotalCurrency("USD")
                    .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                //update order
                serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl).Build();

                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("successfully updated") && l.Result == 0).Any());
                Assert.Equal(1, orderingContext.OrderLines.Where(sl => sl.OrderID == refNum).Count());
                Assert.Equal(2, orderingContext.SOProduct.Where(sp => sp.OrderLine_ID == refNum + "-1").Count());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Total Products are existed for Order and CollectedPackage tag is sent empty, existing packages should be removed. ", Skip = "Waiting for change #TTCWC0066")]
        public void VerifyThatSystemRemovesProductsWhenCollectedPackagesTagIsSetEmpty()
        {
            try
            {
                //create order
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl)
                    .With_CollectedPackages()
                        .With_Package()
                            .With_PackageNumber(refNum)
                        .With_Total()
                            .With_TotalValue(10)
                            .With_TotalCurrency("EUR")
                        .With_Total()
                            .With_TotalValue(20)
                            .With_TotalCurrency("USD")
                    .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                //update order
                serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl)
                    .With_CollectedPackages()
                    .Build();

                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("successfully updated") && l.Result == 0).Any());
                Assert.Equal(1, orderingContext.OrderLines.Where(sl => sl.OrderID == refNum).Count());
                Assert.Equal(0, orderingContext.SOProduct.Where(sp => sp.OrderLine_ID == refNum + "-1").Count());
            }
            catch
            {
                throw;
            }
        }


        [Fact(DisplayName = "Service Order List Feeding - When new products are submitted for existed order Then System replaces old products with the new one", Skip = "Waiting for change #TTCWC0066")]
        public void VerifyThatNewSubmittedProductReplacesTheOldOne()
        {
            try
            {
                //create order
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl)
                    .With_CollectedPackages()
                        .With_Package()
                            .With_PackageNumber(refNum)
                        .With_Total()
                            .With_TotalValue(10)
                            .With_TotalCurrency("EUR")
                        .With_Total()
                            .With_TotalValue(20)
                            .With_TotalCurrency("USD")
                    .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                //update order
                serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate)
                    .With_LocationCode(_fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl)
                    .With_CollectedPackages()
                        .With_Package()
                            .With_PackageNumber(refNum)
                        .With_Total()
                            .With_TotalValue(10)
                            .With_TotalCurrency("UAH")
                    .Build();

                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains("successfully updated") && l.Result == 0).Any());
                Assert.Equal(1, orderingContext.OrderLines.Where(sl => sl.OrderID == refNum).Count());
                Assert.Equal(1, orderingContext.SOProduct.Where(sp => sp.OrderLine_ID == refNum + "-1").Count());
                Assert.True(orderingContext.SOProduct.Where(sp => sp.OrderLine_ID == refNum + "-1" && sp.Currency == "UAH").Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When multiple valid orders are submitted Then System creates this orders")]
        public void VerifyThatSystemAllowsToImportSeveralOrders()
        {
            try
            {
                var serviceDateSecond = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss");
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New()
                                                                                    .With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_DeliveryProducts()
                                                                                            .With_Product()
                                                                                                .With_ProductCode(product.ProductCode)
                                                                                                .With_ProductQuantity(1)
                                                                                   .With_ServiceOrder()
                                                                                        .With_Number(refNum + "1")
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeColl)
                                                                                        .With_CollectedPackages()
                                                                                            .With_Package()
                                                                                                .With_PackageNumber(refNum)
                                                                                            .With_Materials()
                                                                                                .With_Material()
                                                                                                    .With_MaterialID(material.MaterialID)
                                                                                                    .With_MaterialQuantity(10)
                                                                                    .With_ServiceOrder()
                                                                                        .With_Number(refNum + "2")
                                                                                        .With_ServiceDate(serviceDateSecond)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeColl)
                                                                                        .With_CollectedPackages()
                                                                                            .With_Package()
                                                                                                .With_PackageNumber(refNum + "1")
                                                                                            .With_Total()
                                                                                                .With_TotalValue(10)
                                                                                                .With_TotalCurrency("USD")
                                                                                        .Build();
                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.Equal(3, orderingContext.Orders.Where(s => s.ID.StartsWith(refNum)).Count());
                Assert.Equal(3, orderingContext.OrderLines.Where(x => x.OrderID.StartsWith(refNum)).Count());
                Assert.Equal(1, orderingContext.SOProduct.Where(x => x.OrderLine_ID == refNum + "-1" && x.ProductCode == product.ProductCode).Count());
                Assert.Equal(1, orderingContext.SOProduct.Where(x => x.OrderLine_ID == refNum + "1-1" && x.Material_id == material.MaterialID).Count());
                Assert.Equal(1, orderingContext.SOProduct.Where(x => x.OrderLine_ID == refNum + "2-1" && x.Currency == "USD").Count());
            }

            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When among service order list contains both valid and invalid orders Then System imports all valid and rejects all invalid orders")]
        public void VerifyThatSystemImportsAllValidOrdersAndRejectsInvalid()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New()
                                                                                    .With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_DeliveryProducts()
                                                                                            .With_Product()
                                                                                                .With_ProductCode(product.ProductCode)
                                                                                                .With_ProductQuantity(1)
                                                                                   .With_ServiceOrder()
                                                                                        .With_Number(refNum + "1")
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode("")
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_CollectedPackages()
                                                                                            .With_Package()
                                                                                                .With_PackageNumber(refNum + "1")
                                                                                                .With_Materials()
                                                                                                    .With_Material()
                                                                                                        .With_MaterialID(material.MaterialID)
                                                                                                        .With_MaterialQuantity(10)
                                                                                    .With_ServiceOrder()
                                                                                        .With_Number(refNum + "2")
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeColl)
                                                                                        .With_CollectedPackages()
                                                                                            .With_Package()
                                                                                                .With_PackageNumber(refNum + "1")
                                                                                                .With_Total()
                                                                                                    .With_TotalValue(10)
                                                                                                    .With_TotalCurrency("USD")
                                                                                        .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                Assert.Equal(2, orderingContext.Orders.Where(s => s.ID.StartsWith(refNum)).Count());
                Assert.Equal(0, orderingContext.Orders.Where(s => s.ID == refNum + "1").Count());
                Assert.Equal(2, orderingContext.OrderLines.Where(x => x.OrderID.StartsWith(refNum)).Count());
                Assert.Equal(1, orderingContext.SOProduct.Where(x => x.OrderLine_ID == refNum + "-1" && x.ProductCode == product.ProductCode).Count());
                Assert.Equal(0, orderingContext.SOProduct.Where(x => x.OrderLine_ID == refNum + "1-1" && x.Material_id == material.MaterialID).Count());
                Assert.Equal(1, orderingContext.SOProduct.Where(x => x.OrderLine_ID == refNum + "2-1" && x.Currency == "USD").Count());
            }
            catch
            {
                throw;
            }
        }


        [Fact(DisplayName = "Service Order List Feeding - When Pack_nr is not unique Then System doesn't allow to import current order")]
        public void VerifyThatPackageNumberShouldBeuniqueAmongAllOrders()
        {
            try
            {
                var serviceDateSecond = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss");
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New()
                                                                                    .With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeColl)
                                                                                        .With_CollectedPackages()
                                                                                            .With_Package()
                                                                                                .With_PackageNumber(refNum)
                                                                                            .With_Materials()
                                                                                                .With_Material()
                                                                                                    .With_MaterialID(material.MaterialID)
                                                                                                    .With_MaterialQuantity(10)
                                                                                    .With_ServiceOrder()
                                                                                        .With_Number(refNum + "1")
                                                                                        .With_ServiceDate(serviceDateSecond)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeColl)
                                                                                        .With_CollectedPackages()
                                                                                            .With_Package()
                                                                                                .With_PackageNumber(refNum)
                                                                                            .With_Materials()
                                                                                                .With_Material()
                                                                                                    .With_MaterialID(material.MaterialID)
                                                                                                    .With_MaterialQuantity(10)
                                                                                    .Build();
                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());
                var assertMsg = $"Please, enter other number(s)";

                Assert.Equal(1, orderingContext.Orders.Where(s => s.ID == refNum).Count());
                Assert.Equal(0, orderingContext.Orders.Where(s => s.ID == refNum + "1").Count());
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum + "1") && l.Message.Contains(assertMsg)).Any(), $"Message is incorrect");
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Service Order List Feeding - When Generic Status is new and not Cancelled and not in Registered, Unconfirmed, Confirmed Then System shows error message")]
        [InlineData(GenericStatus.Completed)]
        [InlineData(GenericStatus.InProgress)]
        public void VerifyThatServiceOrderWithGenericStutusNotInRegisteredUnconfirmedConfirmedCouldNotBeCreated(GenericStatus genericStatus)
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_GenericStatus(genericStatus)
                                                                                        .With_DeliveryProducts()
                                                                                            .With_Product()
                                                                                                .With_ProductCode(product.ProductCode)
                                                                                                .With_ProductQuantity(1)
                                                                                   .Build();
                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());
                var assertMsg = $"Passed generic status is invalid.";

                Assert.Equal(0, orderingContext.Orders.Where(x => x.ID == refNum).Count());
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains(assertMsg)).Any(), $"Message for Generic Status - '{genericStatus}' is incorrect");
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Service Order List Feeding - When updating Service Order with generic Status Unconfirmed to Generic status not in Unconfirmed, Registered, Confirmed Then System shows error message")]
        [InlineData(GenericStatus.Completed)]
        [InlineData(GenericStatus.InProgress)]
        public void VerifyThatServiceOrderWithGenericStutusUnconfirmedToNotInRegisteredUnconfirmedConfirmedCouldNotBeUpdated(GenericStatus genericStatus)
        {
            try
            {
                var serviceOrderListFirst = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_GenericStatus(GenericStatus.Unconfirmed)
                                                                                        .With_DeliveryProducts()
                                                                                            .With_Product()
                                                                                                .With_ProductCode(product.ProductCode)
                                                                                                .With_ProductQuantity(1)
                                                                                        .Build();
                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListFirst.ToString());
                var serviceOrderListSecond = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_GenericStatus(genericStatus)
                                                                                        .With_DeliveryProducts()
                                                                                            .With_Product()
                                                                                                .With_ProductCode(product.ProductCode)
                                                                                                .With_ProductQuantity(1)
                                                                                        .Build();
                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListSecond.ToString());
                var assertMsg = $"Passed generic status is invalid.";

                Assert.Equal(1, orderingContext.Orders.Where(x => x.ID == refNum && x.GenericStatus == GenericStatus.Unconfirmed).Count());
                Assert.Equal(0, orderingContext.Orders.Where(x => x.ID == refNum && x.GenericStatus == genericStatus).Count());
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains(assertMsg)).Any(), $"Message for Generic Status - '{genericStatus}' is incorrect");
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Service Order List Feeding - When updating Service Order with generic Status Confirmed to Generic status not in Registered, Confirmed Then System shows error message")]
        [InlineData(GenericStatus.Completed)]
        [InlineData(GenericStatus.InProgress)]
        [InlineData(GenericStatus.Unconfirmed)]
        public void VerifyThatServiceOrderWithGenericStutusConfirmedToNotInRegisteredConfirmedCouldNotBeUpdated(GenericStatus genericStatus)
        {
            try
            {
                var serviceOrderListFirst = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_GenericStatus(GenericStatus.Confirmed)
                                                                                        .With_DeliveryProducts()
                                                                                            .With_Product()
                                                                                                .With_ProductCode(product.ProductCode)
                                                                                                .With_ProductQuantity(1)
                                                                                        .Build();
                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListFirst.ToString());
                var serviceOrderListSecond = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_GenericStatus(genericStatus)
                                                                                        .With_DeliveryProducts()
                                                                                            .With_Product()
                                                                                                .With_ProductCode(product.ProductCode)
                                                                                                .With_ProductQuantity(1)
                                                                                        .Build();
                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListSecond.ToString());
                var assertMsg = $"Passed generic status is invalid.";

                Assert.Equal(1, orderingContext.Orders.Where(x => x.ID == refNum && x.GenericStatus == GenericStatus.Confirmed).Count());
                Assert.Equal(0, orderingContext.Orders.Where(x => x.ID == refNum && x.GenericStatus == genericStatus).Count());
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains(assertMsg)).Any(), $"Message for Generic Status - '{genericStatus}' is incorrect");
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Service Order List Feeding - When updating Service Order with generic Status Registered to Generic status not in Registered Then System shows error message")]
        [InlineData(GenericStatus.Completed)]
        [InlineData(GenericStatus.InProgress)]
        [InlineData(GenericStatus.Unconfirmed)]
        [InlineData(GenericStatus.Confirmed)]
        public void VerifyThatServiceOrderWithGenericStutusRegisteredToNotInRegisteredCouldNotBeUpdated(GenericStatus genericStatus)
        {
            try
            {
                var serviceOrderListFirst = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_GenericStatus(GenericStatus.Registered)
                                                                                        .With_DeliveryProducts()
                                                                                            .With_Product()
                                                                                                .With_ProductCode(product.ProductCode)
                                                                                                .With_ProductQuantity(1)
                                                                                        .Build();
                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListFirst.ToString());
                var serviceOrderListSecond = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_GenericStatus(genericStatus)
                                                                                        .With_DeliveryProducts()
                                                                                            .With_Product()
                                                                                                .With_ProductCode(product.ProductCode)
                                                                                                .With_ProductQuantity(1)
                                                                                        .Build();
                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListSecond.ToString());
                var assertMsg = $"Passed generic status is invalid.";

                Assert.Equal(1, orderingContext.Orders.Where(x => x.ID == refNum && x.GenericStatus == GenericStatus.Registered).Count());
                Assert.Equal(0, orderingContext.Orders.Where(x => x.ID == refNum && x.GenericStatus == genericStatus).Count());
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNum) && l.Message.Contains(assertMsg)).Any(), $"Message for Generic Status - '{genericStatus}' is incorrect");
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Service Order List Feeding - When updating Service Order with generic Status Cancelled to Generic status not in Canceled Then System shows error message")]
        [InlineData(GenericStatus.Completed)]
        [InlineData(GenericStatus.InProgress)]
        [InlineData(GenericStatus.Unconfirmed)]
        [InlineData(GenericStatus.Confirmed)]
        [InlineData(GenericStatus.Registered)]
        public void VerifyThatServiceOrderWithGenericStutusCancelledToNotInCancelledCouldNotBeUpdated(GenericStatus genericStatus)
        {
            try
            {
                var serviceOrderListFirst = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_GenericStatus(GenericStatus.Confirmed)
                                                                                        .With_DeliveryProducts()
                                                                                            .With_Product()
                                                                                                .With_ProductCode(product.ProductCode)
                                                                                                .With_ProductQuantity(1)
                                                                                        .Build();
                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListFirst.ToString());
                var serviceOrderListSecond = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_GenericStatus(GenericStatus.Cancelled)
                                                                                        .With_CancelReasonCode(1001)
                                                                                        .Build();
                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListSecond.ToString());
                var serviceOrderListThird = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_GenericStatus(genericStatus)
                                                                                        .With_DeliveryProducts()
                                                                                            .With_Product()
                                                                                                .With_ProductCode(product.ProductCode)
                                                                                                .With_ProductQuantity(1)
                                                                                        .Build();
                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListThird.ToString());
                var assertMsg = $" Order '{refNum}' is cancelled and cannot be edited anymore";

                Assert.Equal(1, orderingContext.Orders.Where(x => x.ID == refNum && x.GenericStatus == GenericStatus.Cancelled).Count());
                Assert.Equal(0, orderingContext.Orders.Where(x => x.ID == refNum && x.GenericStatus == genericStatus).Count());
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(assertMsg)).Any(), $"Message for Generic Status - '{genericStatus}' is incorrect");
            }
            catch
            {
                throw;
            }
        }

        [Theory(DisplayName = "Service Order List Feeding - Verify that Service Order can be Cancelled")]
        [InlineData(GenericStatus.Unconfirmed)]
        [InlineData(GenericStatus.Confirmed)]
        [InlineData(GenericStatus.Registered)]
        public void VerifyThatServiceOrderCanBeCancelled(GenericStatus genericStatus)
        {
            try
            {
                var serviceOrderListFirst = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_GenericStatus(genericStatus)
                                                                                        .With_DeliveryProducts()
                                                                                            .With_Product()
                                                                                                .With_ProductCode(product.ProductCode)
                                                                                                .With_ProductQuantity(1)
                                                                                        .Build();
                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListFirst.ToString());
                var assertMsg = $"Order with Code ‘{refNum}’ has been successfully created.";

                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(assertMsg)).Any(), $"Message for Generic Status - '{genericStatus}' is incorrect");

                var serviceOrderListSecond = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                        .With_Number(refNum)
                                                                                        .With_ServiceDate(serviceDate)
                                                                                        .With_LocationCode(_fixture.Location.Code)
                                                                                        .With_ServiceTypeCode(serviceTypeDelv)
                                                                                        .With_GenericStatus(GenericStatus.Cancelled)
                                                                                        .With_CancelReasonCode(1001)
                                                                                        .Build();
                response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListSecond.ToString());
                assertMsg = $"Order with Code '{refNum}' has been successfully cancelled.";

                Assert.Equal(1, orderingContext.Orders.Where(x => x.ID == refNum && x.GenericStatus == GenericStatus.Cancelled).Count());
                Assert.Equal(0, orderingContext.Orders.Where(x => x.ID == refNum && x.GenericStatus == genericStatus).Count());
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(assertMsg)).Any(), $"Message for Generic Status - '{genericStatus}' is incorrect");
            }
            catch
            {
                throw;
            }
        }
        
        [Theory(DisplayName = "Service Order List Feeding - When Order is submitted with Quantity/Value pair Then System processes feeding correctly")]
        [ClassData(typeof(QuantityValuePairs))]
        public void VerifyThatServiceOrderCanBeCreatedWithCorrectQuantityValuePairWhenMapperIsNotSpecified(Product product, int? quantity, decimal? value, string message, int messageNumber)
        {
            if (DateTime.Today.DayOfWeek.ToString() == "Sunday" || DateTime.Today.DayOfWeek.ToString() == "Saturday")
                serviceDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd hh:mm:ss");

            var serviceOrderListFirst = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                                                                                       .With_Number(refNum)
                                                                                       .With_ServiceDate(serviceDate)
                                                                                       .With_LocationCode(_fixture.Location.Code)
                                                                                       .With_ServiceTypeCode(serviceTypeDelv)
                                                                                       .With_DeliveryProducts()
                                                                                           .With_Product(product.ProductCode, quantity, value)
                                                                                       .Build();
            var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListFirst.ToString());
            var assertMsg = string.Empty;
            if (messageNumber == 1) assertMsg = string.Format(message, refNum);
            if (messageNumber == 2) assertMsg = string.Format(message, refNum, product.DisplayCaption);
            if (messageNumber == 3) assertMsg = string.Format(message, refNum, quantity, value, product.DisplayCaption);
            if (messageNumber == 4) assertMsg = string.Format(message, refNum, value, product.DisplayCaption, decimal.Round(product.Value,2));          

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(assertMsg)).Any(), $"Message for Service Order with Number - '{refNum}' is incorrect");

            if (messageNumber == 1 || messageNumber == 2)
            {
                if (quantity != null) Assert.Equal(quantity, orderingContext.SOProduct.Where(x => x.OrderLine_ID == refNum + "-1").FirstOrDefault().OrderProductNumber);
                if (value != null) Assert.Equal(value, orderingContext.SOProduct.Where(x => x.OrderLine_ID == refNum + "-1").FirstOrDefault().OrderProductValue);
            }
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order is submitted with invalid Mapper name the system rejects feeding")]
        public void VerifyThatServiceOrderCanNotBeCreatedWhenMapperNameIsInvalid()
        {
            if (DateTime.Today.DayOfWeek.ToString() == "Sunday" || DateTime.Today.DayOfWeek.ToString() == "Saturday")
                serviceDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd hh:mm:ss");
            var mapperName = "AutotestInvalid";

            var serviceOrderListFirst = FeedingBuilderFacade.ServiceOrderListFeeding.New(mapperName).With_ServiceOrder()
                                                                                       .With_Number(refNum)
                                                                                       .With_ServiceDate(serviceDate)
                                                                                       .With_LocationCode(_fixture.Location.Code)
                                                                                       .With_ServiceTypeCode(serviceTypeDelv)
                                                                                       .With_DeliveryProducts()
                                                                                           .With_Product(product.ProductCode, 1, product.Value)
                                                                                       .Build();

            var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListFirst.ToString());           
            var assertMsg = $"Mapper with such name ‘{mapperName}’ cannot be found";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(assertMsg)).Any(), $"Message for Service Order with Mapper - '{mapperName}' is incorrect");
        }

        [Fact(DisplayName = "Service Order List Feeding - When Order is submitted with valid Mapper name the system create Service Order according to the mapper settings")]
        public void VerifyThatServiceOrderCanBeCreatedWithValidMapper()
        {
            if (DateTime.Today.DayOfWeek.ToString() == "Sunday" || DateTime.Today.DayOfWeek.ToString() == "Saturday")
                serviceDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd hh:mm:ss");

            var mapperName = "Autotest";
            var externalProductCodeFirst = "£10";
            var externalProductCodeSecond = "£20";

            var serviceOrderListFirst = FeedingBuilderFacade.ServiceOrderListFeeding.New(mapperName).With_ServiceOrder()
                                                                                       .With_Number(refNum)
                                                                                       .With_ServiceDate(serviceDate)
                                                                                       .With_LocationCode(_fixture.Location.Code)
                                                                                       .With_ServiceTypeCode(serviceTypeDelv)
                                                                                       .With_DeliveryProducts()
                                                                                           .With_Product(externalProductCodeFirst, 2, null)
                                                                                           .With_Product(externalProductCodeSecond, 3, null)
                                                                                       .Build();

            var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListFirst.ToString());
            var assertMsg = $"Order with Code ‘{refNum}’ has been successfully created.";
            var product1 = orderingContext.SOProduct.Where(x => x.OrderLine_ID == refNum + "-1" && x.ProductCode == "26").FirstOrDefault();

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(assertMsg)).Any(), $"Message for Service Order with Number - '{refNum}' is incorrect");
            Assert.True(orderingContext.SOProduct.Where(x => x.OrderLine_ID == refNum + "-1" && x.ProductCode == "26").Any(), $"The product with external code {externalProductCodeFirst} cannot be converted according mapper settings");
            Assert.True(orderingContext.SOProduct.Where(x => x.OrderLine_ID == refNum + "-1" && x.ProductCode == "25").Any(), $"The product with external code {externalProductCodeSecond} cannot be converted according mapper settings");
        }

        public class QuantityValuePairs : IEnumerable<object[]>
        {

            Product product = DataFacade.Product.Take(p => p.Description.Contains("Bundel") && p.Value < 2000).Build();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { product, 2, product.Value * 2, "Order with Code ‘{0}’ has been successfully created.", 1 };
                yield return new object[] { product, 3, null, "Order with Code ‘{0}’ has been successfully created.", 1 };
                yield return new object[] { product, null, product.Value * 3, "Order with Code ‘{0}’ has been successfully created.", 1 };
                yield return new object[] { product, null, null, "Error for Order with Number '{0}': 'For product {1} for delivery no quantity and value are passed.'", 2 };
                yield return new object[] { product, 2, product.Value + 1, "Error for Order with Number '{0}': 'Quantity {1} does not match to value {2} for product {3} for delivery.'", 3 };
                yield return new object[] { product, null, product.Value + 1, "Error for Order with Number '{0}': 'Incorrect value {1} for product {2} for delivery: value must be a multiple of product value {3}.'", 4 };

            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}



