using Cwc.BaseData;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Data;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD.BaseDataCrudTests
{
    [Collection("MyCollection")]
    public class MaterialCrudTests : IDisposable
    {
        string defaultNumber, name, type, currency;
        int number, materialQuantity;
        decimal denomination, weight;
        DataTable materialsTable, productsTable;

        public MaterialCrudTests()
        {           
            defaultNumber = $"1101{new Random().Next(4000, 9999)}";
            number = 100;
            denomination = new Random().Next(1, 1000);
            weight = new Random().Next(1, 1000);
            name = "AutoTestManagement";
            type = "NOTE";
            currency = "UAH";
            materialQuantity = 10;

            #region MaterialsTable
            materialsTable = new DataTable();
            materialsTable.Columns.Add("materialID", typeof(string));
            materialsTable.Columns.Add("Description", typeof(string));
            materialsTable.Columns.Add("Denomination", typeof(string));
            materialsTable.Columns.Add("Type", typeof(string));
            materialsTable.Columns.Add("TypeDescription", typeof(string));
            materialsTable.Columns.Add("Quantity", typeof(string));
            materialsTable.Columns.Add("WP_ID", typeof(string));
            materialsTable.Columns.Add("Weight", typeof(string));
            #endregion

            #region ProductsTable
            productsTable = new DataTable();
            productsTable.Columns.Add("productID", typeof(string));
            productsTable.Columns.Add("Description", typeof(string));
            productsTable.Columns.Add("Value", typeof(string));
            productsTable.Columns.Add("Type", typeof(string));
            productsTable.Columns.Add("TypeDescription", typeof(string));
            productsTable.Columns.Add("Quantity", typeof(string));
            productsTable.Columns.Add("WP_ID", typeof(string));
            productsTable.Columns.Add("Weight", typeof(string));
            #endregion
        }

        public void Dispose()
        {
            using (var context = new AutomationBaseDataContext())
            {
                context.Materials.RemoveRange(context.Materials.Where(m => m.MaterialID.StartsWith(defaultNumber)));
                context.SaveChanges();
            }
        }

        [Fact(DisplayName = "Material CRUD - Material was created successfully")]
        public void VerifyThatMaterialWasCreatedSuccessfully()
        {
            var material = DataFacade.Material.New().
                With_MaterialID(defaultNumber).
                With_Description(name).
                With_Type(type).
                With_MaterialNumber(number).
                With_Currency(currency).
                With_Denomination(denomination).
                With_Weight(weight).
                SaveToDb().
                Build();

            var materialCreated = DataFacade.Material.Take(m => m.MaterialID == defaultNumber).Build();

            Assert.True(material.Description == materialCreated.Description, "Material wasn't created. Problem is with Description");
            Assert.True(material.Type == materialCreated.Type, "Material wasn't created.  Problem is with Type");
            Assert.True(material.MaterialNumber == materialCreated.MaterialNumber, "Material wasn't created. Problem is with MaterialNumber");
            Assert.True(material.Currency == materialCreated.Currency, "Material wasn't created. Problem is with Currency");
            Assert.True(material.Denomination == materialCreated.Denomination, "Material wasn't created. Problem is with Denomination");
            Assert.True(material.Weight == materialCreated.Weight, "Material wasn't created. Problem is with Weight");
        }

        [Fact(DisplayName = "Material CRUD - Material was deleted successfully")]
        public void VerifyThatMaterialWasDeletedSuccessfully()
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

            DataFacade.Material.DeleteMany(m => m.MaterialID == defaultNumber);

            using (var context = new AutomationBaseDataContext())
            {
                var result = context.Materials.FirstOrDefault(m => m.MaterialID == defaultNumber);
                Assert.True(result == null, "Material wasn't deleted");
            }
        }

        [Fact(DisplayName = "Material CRUD - Material was updated successfully")]
        public void VerifyThatMaterialWasUpdatedSuccessfully()
        {
            var material = DataFacade.Material.New().
                With_MaterialID(defaultNumber).
                With_Description(name).
                With_Type(type).
                With_MaterialNumber(number).
                With_Currency(currency).
                With_Denomination(denomination).
                With_Weight(weight).
                SaveToDb().
                Build();

            var materialCreated = DataFacade.Material.Take(m => m.MaterialID == defaultNumber);
            materialCreated.With_Description(name + "1").
                With_Type("COIN").
                With_Description(name + "1").
                With_MaterialNumber(number + 1).
                With_Currency("USD").
                With_Denomination(denomination + 1m).
                With_Weight(weight + 1m).
                SaveToDb();

            var materialUpdated = DataFacade.Material.Take(m => m.MaterialID == defaultNumber).Build();

            Assert.False(material.Type == materialUpdated.Type, "Material wasn't updated. Problem is with Type");
            Assert.False(material.Denomination == materialUpdated.Denomination, "Material wasn't updated. Problem is with Denomination");
            Assert.False(material.MaterialNumber == materialUpdated.MaterialNumber, "Material wasn't updated. Problem is with Materialnumber");
            Assert.False(material.Currency == materialUpdated.Currency, "Material wasn't updated. Problem is with Currency");
            Assert.False(material.Description == materialUpdated.Description, "Material wasn't updated. Problem is with Description");
            Assert.False(material.Weight == materialUpdated.Weight, "Material wasn't updated. Problem is with Weight");
        }

        [Fact(DisplayName = "Material CRUD - When Combination of {Denomination, Currency, Material Type} is not unique Then system shows error message")]
        public void VerifyThatMaterialDublicatesCannotBeCreated()
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

            var materialSecond = DataFacade.Material.New().
                With_MaterialID(defaultNumber + "1").
                With_Description(name).
                With_Type(type).
                With_MaterialNumber(number).
                With_Currency(currency).
                With_Denomination(denomination).
                With_Weight(weight);

            var result = BaseDataFacade.MaterialService.Save(materialSecond, null);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Material with specified Material Type, Currency and Denomination already exists.", result.Messages.First());
        }

        [Fact(DisplayName = "Material CRUD - WhenMaterial is created with empty MaterialID field Then system shows error message")]
        public void VerifyThatMaterialCannotBeCreatedWithEmptyMateialID()
        {
            var material = DataFacade.Material.New().
                With_MaterialID(string.Empty).
                With_Description(name).
                With_Type(type).
                With_MaterialNumber(number).
                With_Currency(currency).
                With_Denomination(denomination).
                With_Weight(weight);

            var result = BaseDataFacade.MaterialService.Save(material, null);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Value of property 'Material ID' is not specified.", result.Messages.First());
        }

        [Fact(DisplayName = "Material CRUD - WhenMaterial is created with empty Type Then system shows error message")]
        public void VerifyThatMaterialCannotBeCreatedWithEmptyType()
        {
            var material = DataFacade.Material.New().
                With_MaterialID(defaultNumber).
                With_Description(name).
                With_Type(string.Empty).
                With_MaterialNumber(number).
                With_Currency(currency).
                With_Denomination(denomination).
                With_Weight(weight);

            var result = BaseDataFacade.MaterialService.Save(material, null);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Value of property 'Type' is not specified.", result.Messages.First());
        }

        [Fact(DisplayName = "Material CRUD - WhenMaterial is created without Currency Then system shows error message")]
        public void VerifyThatMaterialCannotBeCreatedWithoutCurrency()
        {
            var material = DataFacade.Material.New().
                With_MaterialID(defaultNumber).
                With_Description(name).
                With_Type(type).
                With_Denomination(denomination).
                With_Weight(weight);

            var result = BaseDataFacade.MaterialService.Save(material, null);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Value of property 'Currency' is not specified.", result.Messages.First());
        }

        //[Fact(DisplayName = "Material CRUD - WhenMaterial is created without Denomination Then system shows error message")]
        //public void VerifyThatMaterialCannotBeCreatedWithoutDenomination()
        //{
        //    var material = DataFacade.Material.New().
        //        With_MaterialID(defaultNumber).
        //        With_Description(name).
        //        With_Type(type).
        //        With_Currency(currency).
        //        With_Weight(weight);

        //    var result = BaseDataFacade.MaterialService.Save(material, null);

        //    Assert.False(result.IsSuccess, "Result should be unsuccessful");
        //    Assert.Equal("Value of property 'Denomination' is not specified.", result.Messages.First());
        //}

        [Fact(DisplayName = "Material CRUD - When Material is deleted with reference Then System shows error message")]
        public void VerifyThatMaterialCannotBeDeletedWithReference()
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

            material = DataFacade.Material.Take(m => m.MaterialID == defaultNumber);
            var materialBuilder = material.Build();

            var productFirst = DataFacade.Product.New().
                With_ProductCode(defaultNumber).
                With_Description(defaultNumber).
                With_Denomination(denomination).
                With_Value(denomination * materialQuantity).
                With_Weight(weight).
                With_Type(type).
                With_WrappingWeight(weight).
                With_Materials(materialQuantity, DataFacade.Material.Take(m => m.MaterialID == defaultNumber)).
                SaveToDb();
            
            materialsTable.Rows.Add(materialBuilder.MaterialID,
                materialBuilder.Description,
                materialBuilder.Denomination,
                materialBuilder.Type,
                materialBuilder.TypeDescription,
                materialQuantity.ToString(),
                materialBuilder.ID,
                materialBuilder.Weight);
            try
            {
                using (var context = new AutomationBaseDataContext())
                {
                    var result = BaseDataFacade.MaterialService.Delete(new[] { materialBuilder.ID }, null);
                    Assert.False(result.IsSuccess, "Material was deleted");
                    Assert.Equal("One or more objects 'Material' cannot be deleted. There are other entities linked to it that may lose integrity.", result.Messages.First());
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                using (var context = new AutomationBaseDataContext())
                {
                    context.ProductMaterialLinks.RemoveRange(context.ProductMaterialLinks.Where(pml => pml.MaterialID == (materialBuilder.ID)));
                    context.SaveChanges();
                }
            }
        }
    }
}