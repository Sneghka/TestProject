using Cwc.BaseData;
using Cwc.Ordering;
using Cwc.Ordering.Classes.OrderExport;
using Cwc.Ordering.Model;
using CWC.AutoTests.Model;
using CWC.AutoTests.Tests.OrderExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace CWC.AutoTests.Helpers
{
    public class OrderExportHelper: BasicFileWorkHelper
    {
        AutomationOrderingDataContext context;

        public OrderExportHelper()
        {
            context = new AutomationOrderingDataContext();
        }

        public XElement GetDeliveryProduct(XElement serviceOrder, string productCode)
        {
            return GetThirdLevelContent(serviceOrder, "DeliveryProducts", "Product", "ProductCode", productCode);             
        }

        public XElement GetCollectectionMaterials(XElement serviceOrder, string materialID)
        {
            return GetThirdLevelContent(serviceOrder, "CollectectionMaterials", "Material", "MaterialID", materialID);
        }

        public XElement GetDeliveryOrderContentInMaterials(XElement serviceOrder, string materialID)
        {
            return GetThirdLevelContent(serviceOrder, "DeliveryOrderContentInMaterials", "Material", "MaterialID", materialID);
        }

        public string GetAttribute(XElement serviceOrder, string collectionName, string attributeName)
        {
            var collection = serviceOrder.Attributes().Where(x => x.Name == attributeName).ToList();
            foreach (var item in collection)
            {
                var itemValue = item.Value;
                return itemValue;
            }
            return null;
        }

        private XElement GetThirdLevelContent(XElement serviceOrder, string collectionName, string collectionItemName, string identificatorName, string identificatorValue)
        {
            var collection = GetTag(collectionName, serviceOrder);
            var collectionItem = collection.Elements().Where(x => x.Name == collectionItemName).ToList();
            foreach (var item in collectionItem)
            {
                var tag = GetTag(identificatorName, item);
                if (tag.Value == identificatorValue)
                {
                    return item;
                }
            }
            return null;
        }



        public void ChangePath(OrderExportJobSetting orderExportJobSetting, string path)
        {
            orderExportJobSetting.PutServiceOrdersFilesFolder = path;
        }

        public List<MaterialExportTempItem> DeliveryMaterials(string serviceOrderId, List<SOProduct> soProducts)
        {
            var materialList = (from serviceOrder in context.Orders
                              join soLine in context.OrderLines on serviceOrder.ID equals soLine.OrderID
                              join soProduct in context.SOProduct on soLine.ID equals soProduct.OrderLine_ID
                              join product in context.Products on soProduct.ProductCode equals product.ProductCode
                              join pml in context.ProductMaterialLinks on product.ID equals pml.ProductID
                              join material in context.Materials on pml.MaterialID equals material.ID
                              join currency in context.Currencies on material.Currency equals currency.NumericCode
                              where serviceOrderId.Contains(serviceOrder.ID)
                              group material by new { material.MaterialID,
                                    material.Weight,
                                    material.ID,
                                    material.Description,
                                    material.Type,
                                    material.Currency,
                                    material.Denomination,
                                    material.ReferenceCode,
                                    } into grp
                              select new MaterialExportTempItem()
                              {
                                  MaterialID = grp.Key.MaterialID,
                                  ID = grp.Key.ID,
                                  MaterialReferenceCode = grp.Key.ReferenceCode,
                                  MaterialDescription = grp.Key.Description,
                                  MaterialType = grp.Key.Type,
                                  CurrencyCode = grp.Key.Currency,
                                  Denomination = grp.Key.Denomination,
                                  Weight = grp.Key.Weight
                              }
                             ).ToList();

            var productList = (from soProduct in soProducts
                               join product in context.Products on soProduct.ProductCode equals product.ProductCode
                               join pml in context.ProductMaterialLinks on product.ID equals pml.ProductID
                               join ml in materialList on pml.MaterialID equals ml.ID

                               select new ProductExportTempItem()
                               {
                                   MaterialID = pml.MaterialID,
                                   Quantity = pml.NumberOfItems * soProduct.OrderProductNumber,
                               });


            var list = new List<MaterialExportTempItem>();

            foreach (var item in materialList)
            {
                var quantity = productList.Where(plList => plList.MaterialID == item.ID).Sum(f => f.Quantity);

                item.Quantity = quantity;
                item.Value = item.Denomination * quantity;
                item.Weight = item.Weight * quantity;
                list.Add(item);
            }


            return list;
        }

        public List<MaterialExportItemCollectionType> CollectedMaterials(string serviceOrderId)
        {
            var materialIDsTempList = (from serviceOrder in context.Orders
                                   join soLine in context.OrderLines on serviceOrder.ID equals soLine.OrderID
                                   join soProduct in context.SOProduct on soLine.ID equals soProduct.OrderLine_ID
                                   join material in context.Materials on soProduct.Material_id equals material.MaterialID
                                   where serviceOrderId.Contains(serviceOrder.ID) && soProduct.ProductCode == null && soProduct.Material_id != null 
                                   group material by new
                                   {
                                       material.MaterialID,
                                       material.Weight,
                                       soProduct.PreanQty,
                                       soProduct.PreanValue,
                                       material.Description,
                                       material.Type,
                                       material.Currency,
                                       material.Denomination,
                                       material.ReferenceCode,
                                   } into grp
                                   select new MaterialExportItemCollectionType()
                                   {
                                       MaterialID = grp.Key.MaterialID,
                                       MaterialReferenceCode = grp.Key.ReferenceCode,
                                       MaterialDescription = grp.Key.Description,
                                       MaterialType = grp.Key.Type,
                                       CurrencyCode = grp.Key.Currency,
                                       Denomination = grp.Key.Denomination,
                                       Value = grp.Key.PreanValue,
                                       Quantity = grp.Key.PreanQty,
                                       Weight = grp.Key.Weight * grp.Key.PreanQty,
                                   }
                             ).ToList();
           
            return materialIDsTempList;
        }

        public void DeliveryProductsCheck(Product product, List<SOProduct> serviceOrderProducts, XElement serviceOrderExported)
        {
            foreach(var serviceOrderProduct in serviceOrderProducts)
            {
                var serviceOrderDeliveryProductExported = HelperFacade.OrderExportHelper.GetDeliveryProduct(serviceOrderExported, product.ProductCode);
                Assert.Equal(product.ProductCode, GetTag("ProductCode", serviceOrderDeliveryProductExported).Value);
                Assert.Equal(serviceOrderProduct.OrderProductNumber.ToString(), GetTag("Quantity", serviceOrderDeliveryProductExported).Value);
            }            
        }

        public List<SOProduct> collectionMaterialsServiceOrderProducts (string serviceOrderId)
        {
            var list = (from serviceOrder in context.Orders
                        join soLine in context.OrderLines on serviceOrder.ID equals soLine.OrderID
                        join soProduct in context.SOProduct on soLine.ID equals soProduct.OrderLine_ID
                        where soProduct.OrderLine_ID == serviceOrderId && soProduct.ProductCode == null && soProduct.Material_id != null
                        select soProduct).ToList();
            
            return list;
        }

        public void DeliveryMaterialsCheck(List<MaterialExportTempItem> expectedMaterials, XElement serviceOrderExported)
        {
            foreach (var expectedMaterial in expectedMaterials)
            {                
                var serviceOrderDeliveryOrderContentInMaterialsExported = HelperFacade.OrderExportHelper.GetDeliveryOrderContentInMaterials(serviceOrderExported, expectedMaterial.MaterialID);
                MaterialsCheck(expectedMaterial, serviceOrderDeliveryOrderContentInMaterialsExported);
            }
        }

        public void CollectedMaterialsCheck(List<MaterialExportItemCollectionType> expectedMaterials, XElement serviceOrderExported)
        {
            foreach (var expectedMaterial in expectedMaterials)
            {
                var serviceOrderCollectectionMaterials = HelperFacade.OrderExportHelper.GetCollectectionMaterials(serviceOrderExported, expectedMaterial.MaterialID);
                MaterialsCheck(expectedMaterial, serviceOrderCollectectionMaterials);

            }
        }

        private void MaterialsCheck(MaterialExportItem expectedMaterial, XElement materials)
        {
            var expectedMaterialReferenceCodeTemp = expectedMaterial.MaterialReferenceCode != null ? expectedMaterial.MaterialReferenceCode : "";
            var expectedDenominationTemp = (expectedMaterial.Denomination.HasValue ? expectedMaterial.Denomination : 0).Value.ToString("G29");
            var expectedWeightTemp = (expectedMaterial.Weight.HasValue ? expectedMaterial.Weight : 0).Value.ToString("G29");
            var expectedValueTemp = (expectedMaterial.Value.HasValue ? expectedMaterial.Value : 0).Value.ToString("G29");
            var expectedValueRevaluatedTemp = BaseDataFacade.ExchangeRateService.GetValueByExchangeRate((expectedMaterial.Value.HasValue ? expectedMaterial.Value.Value : 0),
                                                                                                         expectedMaterial.CurrencyCode,
                                                                                                         null,
                                                                                                         null,
                                                                                                         context.GetDatabaseParams()).ToString("G29");
            var expectedQuantity = (expectedMaterial.Quantity.HasValue ? expectedMaterial.Quantity : 0).Value.ToString("G29");
            var expectedMaterialDescription = expectedMaterial.MaterialDescription != null ? expectedMaterial.MaterialDescription : "";
            var expectedMaterialType = expectedMaterial.MaterialType != null ? expectedMaterial.MaterialType : "";
            var expectedCurrencyCode = expectedMaterial.CurrencyCode != null ? expectedMaterial.CurrencyCode : "";
            
            Assert.Equal(expectedMaterial.MaterialID, GetTag("MaterialID", materials).Value);
            Assert.Equal(expectedMaterialReferenceCodeTemp, GetTag("MaterialReferenceCode", materials).Value);
            Assert.Equal(expectedMaterialDescription, GetTag("MaterialDescription", materials).Value);
            Assert.Equal(expectedMaterialType, GetTag("MaterialType", materials).Value);
            Assert.Equal(expectedCurrencyCode, GetTag("CurrencyCode", materials).Value);
            Assert.Equal(expectedDenominationTemp, GetTag("Denomination", materials).Value);
            Assert.Equal(expectedValueTemp, GetTag("Value", materials).Value);
            Assert.Equal(expectedValueRevaluatedTemp, GetTag("ValueRevaluated", materials).Value);
            Assert.Equal(expectedQuantity, GetTag("Quantity", materials).Value);
            Assert.Equal(expectedWeightTemp, GetTag("Weight", materials).Value);
        }


        public void ServiceOrderCheck(XElement serviceOrderExported, DateTime serviceDate, string number, GenericStatus genericStatus, string locationCode, string serviceType, string currencyCode, string orderType)
        {
            var numberExpected = number != null ? number : "";
            var serviceDateExpected = serviceDate.ToString("yyyy-MM-dd") != null ? serviceDate.ToString("yyyy-MM-dd") : "";
            var genericStatusExpected = genericStatus.ToString() != null ? genericStatus.ToString() : "";
            var locationCodeExpected = locationCode != null ? locationCode : "";
            var serviceTypeCodeExpected = serviceType != null ? serviceType : "";
            var currencyCodeExpected = currencyCode != null ? currencyCode : "";
            var orderTypeExpected = orderType != null ? orderType : "";

            Assert.Equal(serviceDateExpected, GetTag("ServiceDate", serviceOrderExported).Value);
            Assert.Equal(numberExpected, GetTag("Number", serviceOrderExported).Value);
            Assert.Equal(genericStatusExpected, GetTag("GenericStatus", serviceOrderExported).Value);
            Assert.Equal(locationCodeExpected, GetTag("LocationCode", serviceOrderExported).Value);
            Assert.Equal(serviceTypeCodeExpected, GetTag("ServiceTypeCode", serviceOrderExported).Value);
            Assert.Equal(currencyCodeExpected, GetTag("CurrencyCode", serviceOrderExported).Value);
            Assert.Equal(orderTypeExpected,GetTag("OrderType", serviceOrderExported).Value);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
