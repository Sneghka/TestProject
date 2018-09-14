using Cwc.BaseData;
using Cwc.BaseData.Model;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Data;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ProductBuilder
    {        
        Product entity;
        DataTable materialsTable, productsTable;
        AutomationBaseDataContext context;

        public ProductBuilder()
        {
            context = new AutomationBaseDataContext();
        }

        public ProductBuilder With_WP_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_ProductCode(String value)
        {
            if (entity != null)
            {
                entity.ProductCode = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_Description(String value)
        {
            if (entity != null)
            {
                entity.Description = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_Type(String value)
        {
            if (entity != null)
            {
                entity.Type = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_ArticleCode(String value)
        {
            if (entity != null)
            {
                entity.ArticleCode = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_UnitName(String value)
        {
            if (entity != null)
            {
                entity.UnitName = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_UnitsName(String value)
        {
            if (entity != null)
            {
                entity.UnitsName = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_Currency(String value)
        {
            if (entity != null)
            {
                entity.Currency = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_AvailableFrom(DateTime? value)
        {
            if (entity != null)
            {
                entity.AvailableFrom = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_AvailableTo(DateTime? value)
        {
            if (entity != null)
            {
                entity.AvailableTo = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_MaximumQty(Int32? value)
        {
            if (entity != null)
            {
                entity.MaximumQty = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_Value(Decimal value)
        {
            if (entity != null)
            {
                entity.Value = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_Weight(Decimal value)
        {
            if (entity != null)
            {
                entity.Weight = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_WrappingWeight(Decimal value)
        {
            if (entity != null)
            {
                entity.WrappingWeight = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_Denomination(Decimal value)
        {
            if (entity != null)
            {
                entity.Denomination = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_IsStockUnit(Boolean value)
        {
            if (entity != null)
            {
                entity.IsStockUnit = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_IsCustomerUnit(Boolean value)
        {
            if (entity != null)
            {
                entity.IsCustomerUnit = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_IsInternalUnit(Boolean value)
        {
            if (entity != null)
            {
                entity.IsInternalUnit = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_IsSupplyUnit(Boolean value)
        {
            if (entity != null)
            {
                entity.IsSupplyUnit = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_IsBarcodedProduct(Boolean value)
        {
            if (entity != null)
            {
                entity.IsBarcodedProduct = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_BagTypeID(Int32? value)
        {
            if (entity != null)
            {
                entity.BagTypeID = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_ProductGroup(ProductGroup value)
        {
            if (entity != null)
            {
                entity.ProductGroup = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_ProductGroupID(int? value)
        {
            if (entity != null)
            {
                entity.ProductGroupId = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductBuilder With_Materials(int quantity, Material material)
        {            
            materialsTable = new DataTable();
            materialsTable.Columns.Add("materialID", typeof(string));           
            materialsTable.Columns.Add("Quantity", typeof(int));
            materialsTable.Rows.Add(material.MaterialID, quantity);
            return this;
        }

        public ProductBuilder With_Products(int quantity, Product product)
        {            
            productsTable = new DataTable();
            productsTable.Columns.Add("productID", typeof(string));
            productsTable.Columns.Add("Quantity", typeof(int));
            productsTable.Rows.Add(product.ProductCode, quantity);
            return this;
        }

        public ProductBuilder New()
        {
            entity = new Product();
            return this;
        }

        public static implicit operator Product(ProductBuilder ins)
        {
            return ins.Build();
        }

        public Product Build()
        {
            return entity;
        }

        public ProductBuilder SaveToDb()
        {
            Result result;

            if (materialsTable != null && productsTable == null)
            {
                result = BaseDataFacade.ProductService.Save(entity, materialsTable, new DataTable(), null);
            }
            else if (materialsTable == null && productsTable != null)
            {
                result = BaseDataFacade.ProductService.Save(entity, new DataTable(), productsTable, null);
            }
            else if (materialsTable != null && productsTable != null)
            {
                result = BaseDataFacade.ProductService.Save(entity, materialsTable, productsTable, null);
            }
            else
            {
                result = BaseDataFacade.ProductService.Save(entity, null);
            }

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Product saving failed. Reson: {result.GetMessage()}");
            }

            return this;
        }

        public void Delete(Func<Product, bool> expression)
        {
            var product = context.Products.FirstOrDefault(expression);
            if (product == null)
            {
                throw new ArgumentNullException("Product with provided criteria wasn't found");
            }

            var result = BaseDataFacade.ProductService.Delete(new[] { product.ID }, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Product deletion failed. Reason: {result.GetMessage()}");
            }

        }

        public ProductBuilder Take(string prodCode)
        {
            entity = context.Products.FirstOrDefault(x => x.ProductCode == prodCode); 
            if (entity == null)
            {
                throw new ArgumentNullException($"Product with code {prodCode} doesn't exist");
            }
            return this;
        }

        public ProductBuilder Take(Func<Product, bool> expression)
        {
            entity = context.Products.FirstOrDefault(expression);
            if (entity == null)
            {
                throw new ArgumentNullException("Product with given criteria doesn't exist");
            }
            return this;
        }        
    }
}