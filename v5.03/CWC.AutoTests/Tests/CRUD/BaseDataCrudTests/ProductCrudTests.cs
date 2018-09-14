using Cwc.BaseData;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Data;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD.BaseDataCrudTests
{
    public class ProductCrudTests : IDisposable
    {
        string defaultNumber, name, type, currency;
        int materialQuantity, number;
        decimal denomination, weight, value;
        DataTable materialsTable, productsTable;
        Cwc.Common.Result result;

        public ProductCrudTests()
        {
            defaultNumber = $"1101{new Random().Next(4000, 9999)}";
            materialQuantity = 10;
            number = 100;
            denomination = new Random().Next(1, 1000);
            value = materialQuantity * denomination;
            weight = new Random().Next(1, 1000);
            name = "AutoTestManagement";
            type = "NOTE";
            currency = "UAH";

            materialsTable = new DataTable();
            materialsTable.Columns.Add("materialID", typeof(string));
            materialsTable.Columns.Add("Description", typeof(string));
            materialsTable.Columns.Add("Denomination", typeof(string));
            materialsTable.Columns.Add("Type", typeof(string));
            materialsTable.Columns.Add("TypeDescription", typeof(string));
            materialsTable.Columns.Add("Quantity", typeof(string));
            materialsTable.Columns.Add("WP_ID", typeof(string));
            materialsTable.Columns.Add("Weight", typeof(string));            

            productsTable = new DataTable();
            productsTable.Columns.Add("productID", typeof(string));
            productsTable.Columns.Add("Description", typeof(string));
            productsTable.Columns.Add("Value", typeof(string));
            productsTable.Columns.Add("Type", typeof(string));
            productsTable.Columns.Add("TypeDescription", typeof(string));
            productsTable.Columns.Add("Quantity", typeof(string));
            productsTable.Columns.Add("WP_ID", typeof(string));
            productsTable.Columns.Add("Weight", typeof(string));
           

        }
        public void Dispose()
        {
            using (var context = new AutomationBaseDataContext())
            {
                context.Materials.RemoveRange(context.Materials.Where(m => m.MaterialID.StartsWith(defaultNumber)));
                context.ProductMaterialLinks
                    .RemoveRange(context.ProductMaterialLinks
                        .Where(l => context.Materials
                            .Where(m => m.MaterialID.StartsWith(defaultNumber))
                            .Select(x => x.ID)
                            .Contains(l.MaterialID)
                        )
                    );
                context.ProdContents.RemoveRange(context.ProdContents.Where(x => x.MaterialID.StartsWith(defaultNumber)));
                context.ProdCompositions.RemoveRange(context.ProdCompositions.Where(x => x.ProdCodeWhole.StartsWith(defaultNumber)));
                context.Products.RemoveRange(context.Products.Where(p => p.ProductCode.StartsWith(defaultNumber)));
                context.SaveChanges();
            }
        }

        [Fact(DisplayName = "Product CRUD - Product with Material and Product was created successfully")]

        public void VerifyThatProductWithMaterialAndProductWasCreatedSuccessfully()
        {

            var material = DataFacade.Material.New().
                With_MaterialID(defaultNumber).
                With_Description(name).
                With_Type(type).
                With_MaterialNumber(number).
                With_Currency(currency).
                With_Denomination(denomination).
                With_Weight(weight).
                SaveToDb();

            var productFirst = DataFacade.Product.New().
                With_ProductCode(defaultNumber + "1").
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(value).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Materials(materialQuantity, DataFacade.Material.Take(m => m.MaterialID == defaultNumber)).
                SaveToDb();

            var productSecond = DataFacade.Product.New().
                With_ProductCode(defaultNumber).
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(value * materialQuantity + value).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Materials(materialQuantity, material).
                With_Products(materialQuantity, productFirst).                    
                SaveToDb().
                Build();

            var productCreated = DataFacade.Product.Take(p => p.ProductCode == defaultNumber).Build();

            Assert.True(productSecond.Description == productCreated.Description, "Product with Material and Product wasn't created. Problem is with Description");
            Assert.True(productSecond.Denomination == productCreated.Denomination, "Product with Material and Product wasn't created. Problem is with Denomination");
            Assert.True(productSecond.Value == productCreated.Value, "Product with Material and Product wasn't created. Problem is with Value");
            Assert.True(productSecond.Weight == productCreated.Weight, "Product with Material and Product wasn't created. Problem is with Weight");
            Assert.True(productSecond.Type == productCreated.Type, "Product with Material and Product wasn't created. Problem is with Type");
            Assert.True(productSecond.WrappingWeight == productCreated.WrappingWeight, "Product with Material and Product wasn't created. Problem is with WrappingWeight");
        }

        [Fact(DisplayName = "Product CRUD - Product with Material was created successfully")]

        public void VerifyThatProductWithMaterialWasCreatedSuccessfully()
        {
            var material = DataFacade.Material.New().
                With_MaterialID(defaultNumber).
                With_Description(name).
                With_Type(type).
                With_MaterialNumber(number).
                With_Currency(currency).
                With_Denomination(denomination).
                With_Weight(weight).
                SaveToDb();

            var product = DataFacade.Product.New().
                With_ProductCode(defaultNumber).
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(value).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Materials(materialQuantity, material).
                SaveToDb().
                Build();

            var productCreated = DataFacade.Product.Take(p => p.ProductCode == defaultNumber).Build();

            Assert.True(product.Description == productCreated.Description, "Product with Material and Product wasn't created. Problem is with Description");
            Assert.True(product.Denomination == productCreated.Denomination, "Product with Material and Product wasn't created. Problem is with Denomination");
            Assert.True(product.Value == productCreated.Value, "Product with Material and Product wasn't created. Problem is with Value");
            Assert.True(product.Weight == productCreated.Weight, "Product with Material and Product wasn't created. Problem is with Weight");
            Assert.True(product.Type == productCreated.Type, "Product with Material and Product wasn't created. Problem is with Type");
            Assert.True(product.WrappingWeight == productCreated.WrappingWeight, "Product with Material and Product wasn't created. Problem is with WrappingWeight");
        }

        [Fact(DisplayName = "Product CRUD - Product with Product was created successfully")]

        public void VerifyThatProductWithProductWasCreatedSuccessfully()
        {
            var material = DataFacade.Material.New().
                With_MaterialID(defaultNumber).
                With_Description(name).
                With_Type(type).
                With_MaterialNumber(number).
                With_Currency(currency).
                With_Denomination(denomination).
                With_Weight(weight).
                SaveToDb();

            var productFirst = DataFacade.Product.New().
                With_ProductCode(defaultNumber + "1").
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(value).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Materials(materialQuantity, DataFacade.Material.Take(m => m.MaterialID == defaultNumber)).
                SaveToDb();

            var productSecond = DataFacade.Product.New().
                With_ProductCode(defaultNumber).
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(value * materialQuantity).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Products(materialQuantity, productFirst).
                SaveToDb().
                Build();

            var productCreated = DataFacade.Product.Take(p => p.ProductCode == defaultNumber).Build();

            Assert.True(productSecond.Description == productCreated.Description, "Product with Material and Product wasn't created. Problem is with Description");
            Assert.True(productSecond.Denomination == productCreated.Denomination, "Product with Material and Product wasn't created. Problem is with Denomination");
            Assert.True(productSecond.Value == productCreated.Value, "Product with Material and Product wasn't created. Problem is with Value");
            Assert.True(productSecond.Weight == productCreated.Weight, "Product with Material and Product wasn't created. Problem is with Weight");
            Assert.True(productSecond.Type == productCreated.Type, "Product with Material and Product wasn't created. Problem is with Type");
            Assert.True(productSecond.WrappingWeight == productCreated.WrappingWeight, "Product with Material and Product wasn't created. Problem is with WrappingWeight");
        }


        [Fact(DisplayName = "Product CRUD - Product was deleted successfully")]

        public void VerifyThatProductWasDeletedSuccessfully()
        { 
            var material = DataFacade.Material.New().
                 With_MaterialID(defaultNumber).
                 With_Description(name).
                 With_Type(type).
                 With_MaterialNumber(number).
                 With_Currency(currency).
                 With_Denomination(denomination).
                 With_Weight(weight).
                 SaveToDb();

            var product = DataFacade.Product.New().
                With_ProductCode(defaultNumber).
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(value).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Materials(materialQuantity, material).
                SaveToDb();

            product.Delete(p => p.ProductCode == defaultNumber);

            using (var context = new AutomationBaseDataContext())
            {
                var result = context.Products.FirstOrDefault(p => p.ProductCode == defaultNumber);
                Assert.True(result == null, "Product wasn't deleted");
            }
        }

        [Fact(DisplayName = "Product CRUD - Product was updated successfully")]

        public void VerifyThatProductWasUpdatedSuccessfully()
        {
            var material = DataFacade.Material.New().
                With_MaterialID(defaultNumber).
                With_Description(name).
                With_Type(type).
                With_MaterialNumber(number).
                With_Currency(currency).
                With_Denomination(denomination).
                With_Weight(weight).
                SaveToDb();

            var product = DataFacade.Product.New().
                With_ProductCode(defaultNumber).
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(value).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Materials(materialQuantity, material).                
                SaveToDb().
                Build();

            var productCreated = DataFacade.Product.Take(p => p.ProductCode == defaultNumber);
            productCreated.With_Weight(1m).
                With_Type("COIN").
                SaveToDb();
            
            var productUpdated = DataFacade.Product.Take(p => p.ProductCode == defaultNumber).Build();

            Assert.False(product.Weight == productUpdated.Weight,"Weight wasn't updated");
            Assert.False(product.Type == productUpdated.Type, "Type wasn't updated");
        }

        [Fact(DisplayName = "Product CRUD - When Product was created with Available From > Available To Then System shows error message")]
        
        public void VerifyThatProductWithIncorrectAvailabilityCannotBeCreated()
        {
            var availableFrom = new DateTime(2017, 1, 1, 10, 0, 0);
            var availableTo = new DateTime(2017, 1, 1, 9, 0, 0);

            var material = DataFacade.Material.New().
                With_MaterialID(defaultNumber).
                With_Description(name).
                With_Type(type).
                With_MaterialNumber(number).
                With_Currency(currency).
                With_Denomination(denomination).
                With_Weight(weight).
                SaveToDb();

            var product = DataFacade.Product.New().
                With_ProductCode(defaultNumber).
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(weight).
                With_Weight(value).
                With_AvailableFrom(availableFrom).
                With_AvailableTo(availableTo).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Materials(materialQuantity, material);

            materialsTable.Rows.Add(material.Build().MaterialID,
            material.Build().Description,
                material.Build().Denomination,
                material.Build().Type,
                material.Build().TypeDescription,
                materialQuantity.ToString(),
                material.Build().ID,
                material.Build().Weight);
            
            if (materialsTable != null && productsTable != null)
            {
                result = BaseDataFacade.ProductService.Save(product, materialsTable, productsTable, null);
            }
            else
            {
                result = BaseDataFacade.ProductService.Save(product, null);
            }

            Assert.False(result.IsSuccess, "Saving should be unsuccessful");
            Assert.Equal("Date Available From should be less or equal to date Available To.", result.Messages.First());
        }

        [Fact(DisplayName = "Product CRUD - When Product was created with empty content Then System shows error message")]

        public void VerifyThatProductWithEmptyContentCannotBeCreated()
        {
            var product = DataFacade.Product.New().
                With_ProductCode(defaultNumber).
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(value).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight);

            if (materialsTable != null && productsTable != null)
            {
                result = BaseDataFacade.ProductService.Save(product, materialsTable, productsTable, null);
            }
            else
            {
                result = BaseDataFacade.ProductService.Save(product, null);
            }

            Assert.False(result.IsSuccess, "Saving should be unsuccessful");
            Assert.Equal("Products with an empty content are not allowed.", result.Messages.First());
        }

        [Fact(DisplayName = "Product CRUD - When Product was created exists Product Code Then System shows error message")]
        public void VerifyThatProductWithGivenProductCodeIsAlreadyExists()
        {
            var material = DataFacade.Material.New().
                With_MaterialID(defaultNumber).
                With_Description(name).
                With_Type(type).
                With_MaterialNumber(number).
                With_Currency(currency).
                With_Denomination(denomination).
                With_Weight(weight).
                SaveToDb();

            var productFirst = DataFacade.Product.New().
                With_ProductCode(defaultNumber).
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(value).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Materials(materialQuantity, DataFacade.Material.Take(m => m.MaterialID == defaultNumber)).
                SaveToDb();

            var productSecond = DataFacade.Product.New().
                With_ProductCode(defaultNumber).
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(value).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Products(materialQuantity, productFirst);

            materialsTable.Rows.Add(material.Build().MaterialID,
                material.Build().Description,
                material.Build().Denomination,
                material.Build().Type,
                material.Build().TypeDescription,
                materialQuantity.ToString(),
                material.Build().ID,
                material.Build().Weight);

            if (materialsTable != null && productsTable != null)
            {
                result = BaseDataFacade.ProductService.Save(productSecond, materialsTable, productsTable, null);
            }
            else
            {
                result = BaseDataFacade.ProductService.Save(productSecond, null);
            }

            Assert.False(result.IsSuccess, "Saving should be unsuccessful");
            Assert.Equal("Product with given Product Code already exists.", result.Messages.First());
        }

        [Fact(DisplayName = "Product CRUD - When Product was deleted with reference Then System shows error message")]
        public void VerifyThatProductWithReferenceCannotBeDeleted()
        {
            var material = DataFacade.Material.New().
                With_MaterialID(defaultNumber).
                With_Description(name).
                With_Type(type).
                With_MaterialNumber(number).
                With_Currency(currency).
                With_Denomination(denomination).
                With_Weight(weight).
                SaveToDb();

            var productFirst = DataFacade.Product.New().
                With_ProductCode(defaultNumber + "1").
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(value).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Materials(materialQuantity, DataFacade.Material.Take(m => m.MaterialID == defaultNumber)).
                SaveToDb();

            var productSecond = DataFacade.Product.New().
                With_ProductCode(defaultNumber).
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(value * materialQuantity).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Products(materialQuantity, productFirst).
                SaveToDb().
                Build();

            materialsTable.Rows.Add(material.Build().MaterialID,
                material.Build().Description,
                material.Build().Denomination,
                material.Build().Type,
                material.Build().TypeDescription,
                materialQuantity.ToString(),
                material.Build().ID,
                material.Build().Weight);

            using (var context = new AutomationBaseDataContext())
            {
                var result = BaseDataFacade.ProductService.Delete(new[] { productFirst.Build().ID }, null); ;
                Assert.False(result.IsSuccess, "Deleting should be unsuccessful");
                Assert.Equal("One or more objects 'Product' cannot be deleted. There are other entities linked to it that may lose integrity.", result.Messages.First());
            }

        }
    }
}
