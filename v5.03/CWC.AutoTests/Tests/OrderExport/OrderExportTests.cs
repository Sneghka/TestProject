using Cwc.BaseData;
using Cwc.Ordering;
using Cwc.Ordering.Model;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.FeedingBuilder;
using CWC.AutoTests.Tests.Fixtures;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.OrderExport
{
    public class OrderExportTests : IClassFixture<OrderFixture>, IDisposable
    {
        string filePath;
        string filePathTemp;
        string serviceTypeDelv = "DELV";
        string serviceTypeColl = "COLL";
        string serviceTypeRepl = "REPL";
        string refNum;
        string currencyCode = "EUR";
        string orderTypeAtRequest = "AtRequest";
        string orderTypeAdHoc = "AdHoc";
        string lastExportedDate;
        OrderFixture fixture;
        AutomationOrderingDataContext _context;
        AutomationFeedingDataContext context;
        DateTime serviceDate;
        Product product;
        OrderExportJobSetting orderExportJobSetting;
        Material material;

        public OrderExportTests(OrderFixture _fixture)
        {
            fixture = _fixture;
            _context = new AutomationOrderingDataContext();
            context = new AutomationFeedingDataContext();
            refNum = $"1101{new Random(DateTime.Now.Ticks.GetHashCode()).Next(1212, 12121211)}";
            serviceDate = DateTime.Today;
            product = DataFacade.Product.Take(p => p.Description.Contains("Bundel") && p.Value < 20000).Build();
            orderExportJobSetting = OrderingFacade.OrderExportJobSettingService.Load();
            material = DataFacade.Material.Take(x => x.Type == "NOTE").Build();
            filePathTemp = _context.OrderExportSetting.Where(s => s.ID == 1).FirstOrDefault().PutServiceOrdersFilesFolder;
            var folder = System.IO.Path.Combine("Exchange", "OrderExport", "");
            var basePath = System.IO.Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            filePath = System.IO.Path.Combine(basePath, folder);
            HelperFacade.OrderExportHelper.ChangePath(orderExportJobSetting, filePath);
            lastExportedDate = orderExportJobSetting.LastExportTime.Value.Date.ToString("yyyyMMdd");
        }

        [Fact(DisplayName = "Order Export - Verify that Delivery Service Order can be exported")]
        public void VerifyThatDeliveryServiceOrderIsExported()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate.ToString("yyyy-MM-dd hh:mm:ss"))
                    .With_LocationCode(fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeDelv)
                    .With_GenericStatus(GenericStatus.Registered)
                    .With_DeliveryProducts()
                        .With_Product()
                            .With_ProductCode(product.ProductCode)
                            .With_ProductQuantity(3)
                    .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                var serviceOrder = _context.Orders.Where(so => so.ID == refNum).FirstOrDefault();
                var serviceOrderLine = _context.OrderLines.Where(so => so.OrderID == refNum).ToArray();
                var serviceOrderDeliveryProduct = _context.SOProduct.Where(so => so.OrderLine_ID == refNum + "-1").ToList().Where(a => a.ProductCode != null).ToList();

                OrderingFacade.OrderExportJobService.Run(orderExportJobSetting, null);

                var expectedMaterials = HelperFacade.OrderExportHelper.DeliveryMaterials(serviceOrder.ID, serviceOrderDeliveryProduct);
                var serviceOrderExported = HelperFacade.OrderExportHelper.GetEntity("Service Order", lastExportedDate, filePath);

                Assert.Equal("0",HelperFacade.OrderExportHelper.GetAttribute(serviceOrderExported, "Service Order","act"));
                HelperFacade.OrderExportHelper.ServiceOrderCheck(serviceOrderExported, serviceDate, refNum, GenericStatus.Registered, fixture.Location.Code, serviceTypeDelv, currencyCode, orderTypeAtRequest);
                HelperFacade.OrderExportHelper.DeliveryProductsCheck(product, serviceOrderDeliveryProduct, serviceOrderExported);
                HelperFacade.OrderExportHelper.DeliveryMaterialsCheck(expectedMaterials, serviceOrderExported);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Order Export - Verify that Collected Service Order can be exported")]
        public void VerifyThatCollectedServiceOrderIsExported()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate.ToString("yyyy-MM-dd hh:mm:ss"))
                    .With_LocationCode(fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeColl)
                    .With_GenericStatus(GenericStatus.Registered)
                    .With_CollectedPackages()
                        .With_Package()
                            .With_PackageNumber(refNum)
                        .With_Materials()
                            .With_Material()
                                .With_MaterialID(material.MaterialID)
                    .With_MaterialQuantity(10)
                    .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                var serviceOrder = _context.Orders.Where(so => so.ID == refNum).FirstOrDefault();
                var serviceOrderLine = _context.OrderLines.Where(so => so.OrderID == refNum).ToArray();
                var serviceOrderCollectedProduct = _context.SOProduct.Where(so => so.OrderLine_ID == refNum + "-1").ToList().Where(a => a.ProductCode == null).ToList();

                OrderingFacade.OrderExportJobService.Run(orderExportJobSetting, null);

                var expectedMaterials = HelperFacade.OrderExportHelper.CollectedMaterials(serviceOrder.ID);
                var serviceOrderExported = HelperFacade.OrderExportHelper.GetEntity("Service Order", lastExportedDate, filePath);

                Assert.Equal("0", HelperFacade.OrderExportHelper.GetAttribute(serviceOrderExported, "Service Order", "act"));
                HelperFacade.OrderExportHelper.ServiceOrderCheck(serviceOrderExported, serviceDate, refNum, GenericStatus.Registered, fixture.Location.Code, serviceTypeColl, currencyCode, orderTypeAtRequest);
                HelperFacade.OrderExportHelper.CollectedMaterialsCheck(expectedMaterials, serviceOrderExported);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Order Export - Verify that Replanishment Service Order can be exported")]
        public void VerifyThatReplanishmentServiceOrderIsExported()
        {
            try
            {
                var serviceOrderList = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
                    .With_Number(refNum)
                    .With_ServiceDate(serviceDate.ToString("yyyy-MM-dd hh:mm:ss"))
                    .With_LocationCode(fixture.Location.Code)
                    .With_ServiceTypeCode(serviceTypeDelv)
                    .With_GenericStatus(GenericStatus.Registered)
                    .With_DeliveryProducts()
                        .With_Product()
                            .With_ProductCode(product.ProductCode)
                            .With_ProductQuantity(1)
                        .With_Product()
                            .With_ProductCode("29")
                            .With_ProductQuantity(1)
                    .With_CollectedPackages()
                        .With_Package()
                            .With_PackageNumber(refNum)
                        .With_Materials()
                            .With_Material()
                                .With_MaterialID(material.MaterialID)
                                .With_MaterialQuantity(10)
                    .Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderList.ToString());

                var serviceOrder = _context.Orders.Where(so => so.ID == refNum).FirstOrDefault();
                var serviceOrderLine = _context.OrderLines.Where(so => so.OrderID == refNum).ToArray();
                var serviceOrderProduct = _context.SOProduct.Where(so => so.OrderLine_ID == refNum + "-1").ToList();
                var serviceOrderDeliveryProduct = serviceOrderProduct.Where(a => a.ProductCode != null).ToList();
                var serviceOrderCollectedProduct = serviceOrderProduct.Where(a => a.ProductCode == null).ToList();

                OrderingFacade.OrderExportJobService.Run(orderExportJobSetting, null);

                var expectedDelvMaterials = HelperFacade.OrderExportHelper.DeliveryMaterials(serviceOrder.ID, serviceOrderProduct);
                var expectedCollMaterials = HelperFacade.OrderExportHelper.CollectedMaterials(serviceOrder.ID);
                var serviceOrderExported = HelperFacade.OrderExportHelper.GetEntity("Service Order", lastExportedDate, filePath);

                Assert.Equal("0", HelperFacade.OrderExportHelper.GetAttribute(serviceOrderExported, "Service Order", "act"));
                HelperFacade.OrderExportHelper.ServiceOrderCheck(serviceOrderExported, serviceDate, refNum, GenericStatus.Registered, fixture.Location.Code, serviceTypeDelv, currencyCode, orderTypeAtRequest);
                HelperFacade.OrderExportHelper.DeliveryProductsCheck(product, serviceOrderDeliveryProduct, serviceOrderExported);
                HelperFacade.OrderExportHelper.DeliveryMaterialsCheck(expectedDelvMaterials, serviceOrderExported);
                HelperFacade.OrderExportHelper.CollectedMaterialsCheck(expectedCollMaterials, serviceOrderExported);
            }
            catch
            {
                throw;
            }
        }

        //[Fact(DisplayName = "Order Export - Verify that Service Order can be updated")]
        //public void VerifyThatReplanishmentServiceOrderIsUpdated()
        //{
        //    try
        //    {
        //        var serviceOrderListfirst = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
        //            .With_Number(refNum)
        //            .With_ServiceDate(serviceDate.ToString("yyyy-MM-dd hh:mm:ss"))
        //            .With_LocationCode(fixture.Location.Code)
        //            .With_ServiceTypeCode(serviceTypeDelv)
        //            .With_GenericStatus(GenericStatus.Registered)
        //            .With_DeliveryProducts()
        //                .With_Product()
        //                    .With_ProductCode(product.ProductCode)
        //                    .With_ProductQuantity(1)
        //                .With_Product()
        //                    .With_ProductCode("29")
        //                    .With_ProductQuantity(1)
        //            .With_CollectedPackages()
        //                .With_Package()
        //                    .With_PackageNumber(refNum)
        //                .With_Materials()
        //                    .With_Material()
        //                        .With_MaterialID(material.MaterialID)
        //                        .With_MaterialQuantity(10)
        //            .Build();

        //        var response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListfirst.ToString());

        //        var serviceOrderListSecond = FeedingBuilderFacade.ServiceOrderListFeeding.New().With_ServiceOrder()
        //            .With_Number(refNum)
        //            .With_ServiceDate(serviceDate.ToString("yyyy-MM-dd hh:mm:ss"))
        //            .With_LocationCode(fixture.Location.Code)
        //            .With_ServiceTypeCode(serviceTypeDelv)
        //            .With_GenericStatus(GenericStatus.Registered)
        //            .With_DeliveryProducts()
        //                .With_Product()
        //                    .With_ProductCode(product.ProductCode)
        //                    .With_ProductQuantity(4)
        //            .With_CollectedPackages()
        //                .With_Package()
        //                    .With_PackageNumber(refNum)
        //                .With_Materials()
        //                    .With_Material()
        //                        .With_MaterialID(material.MaterialID)
        //                        .With_MaterialQuantity(10)
        //            .Build();

        //        response = HelperFacade.FeedingHelper.SendFeeding(serviceOrderListfirst.ToString());

        //        var serviceOrder = _context.Orders.Where(so => so.ID == refNum).FirstOrDefault();
        //        var serviceOrderLine = _context.OrderLines.Where(so => so.OrderID == refNum).ToArray();
        //        var serviceOrderProduct = _context.SOProduct.Where(so => so.OrderLine_ID == refNum + "-1").ToList();
        //        var serviceOrderDeliveryProduct = serviceOrderProduct.Where(a => a.ProductCode != null).ToList();
        //        var serviceOrderCollectedProduct = serviceOrderProduct.Where(a => a.ProductCode == null).ToList();

        //        OrderingFacade.OrderExportJobService.Run(orderExportJobSetting, null);

        //        var expectedDelvMaterials = HelperFacade.OrderExportHelper.DeliveryMaterials(serviceOrder.ID, serviceOrderProduct);
        //        var expectedCollMaterials = HelperFacade.OrderExportHelper.CollectedMaterials(serviceOrder.ID);
        //        var serviceOrderExported = HelperFacade.OrderExportHelper.GetServiceOrder("Service Order", orderExportJobSetting.LastExportTime.Value, filePath);

        //        Assert.Equal("1", HelperFacade.OrderExportHelper.GetAttribute(serviceOrderExported, "Service Order", "act"));
        //        HelperFacade.OrderExportHelper.ServiceOrderCheck(serviceOrderExported, serviceDate, refNum, GenericStatus.Registered, fixture.Location.Code, serviceTypeDelv, currencyCode, orderTypeAtRequest);
        //        HelperFacade.OrderExportHelper.DeliveryProductsCheck(product, serviceOrderDeliveryProduct, serviceOrderExported);
        //        HelperFacade.OrderExportHelper.DeliveryMaterialsCheck(expectedDelvMaterials, serviceOrderExported);
        //        HelperFacade.OrderExportHelper.CollectedMaterialsCheck(expectedCollMaterials, serviceOrderExported);

        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        public void Dispose()
        {
            HelperFacade.OrderExportHelper.RemoveExportedFile("Service Order", lastExportedDate, filePath);
            HelperFacade.OrderExportHelper.ChangePath(orderExportJobSetting, filePathTemp);
            _context.SaveChanges();
            _context.SOProduct.RemoveRange(_context.SOProduct.Where(sop => sop.OrderLine_ID.Contains(refNum)));
            _context.OrderLines.RemoveRange(_context.OrderLines.Where(o => o.OrderID.Contains(refNum)));
            _context.Orders.RemoveRange(_context.Orders.Where(o => o.ID.Contains(refNum)));
            context.SaveChanges();
            _context.Dispose();
            context.Dispose();
        }
    }
}
