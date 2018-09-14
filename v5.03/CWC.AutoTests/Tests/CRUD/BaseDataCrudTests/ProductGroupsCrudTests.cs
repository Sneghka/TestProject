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
    public class ProductGroupsCrudTests : IDisposable
    {
        string defaultNumber, name;

        public ProductGroupsCrudTests()
        {
            defaultNumber = $"1101{new Random().Next(4000, 9999)}";
            name = "AutoTestManagement";
        }

        public void Dispose()
        {
            using (var context = new AutomationBaseDataContext())
            {
                context.ProductGroups.RemoveRange(context.ProductGroups.Where(pg => pg.Code.StartsWith(defaultNumber)));
                context.SaveChanges();
            }
        }

        #region Positive Tests
        /// <summary>
        /// Positive CRUD tests for Product group
        /// </summary>
        [Fact(DisplayName = "ProductGroup CRUD - ProductGroup was created successfully")]
        public void VerifyThatProductGroupWasCreatedSuccessfully()
        {
            var productGroup = DataFacade.ProductGroup.New().
                With_Code(defaultNumber).
                With_Description(name).
                SaveToDb().
                Build();

            var productGroupCreated = DataFacade.ProductGroup.Take(pg => pg.Code == defaultNumber).Build();

            Assert.True(productGroup.Description == productGroupCreated.Description,"ProductGroup wasn't created. Problem is with Description");
        }

        [Fact(DisplayName = "ProductGroup CRUD - ProductGroup was deleted successfully")]
        public void VerifyThatProductGroupWasDeletedSuccessfully()
        {
            var productGroup = DataFacade.ProductGroup.New().
                With_Code(defaultNumber).
                With_Description(name).
                SaveToDb();

            DataFacade.ProductGroup.Delete(pg => pg.Code == defaultNumber);

            using (var context = new AutomationBaseDataContext())
            {
                var result = context.ProductGroups.Where(pg => pg.Code == defaultNumber).FirstOrDefault();
                Assert.True(result == null,"Product Group wasn't deleted");
            }
        }

        [Fact(DisplayName = "ProductGroup CRUD - ProductGroup was updated successfully")]
        public void VerifyThatProductGroupWasUpdatedSuccessfully()
        {
            var productGroup = DataFacade.ProductGroup.New().
                With_Code(defaultNumber).
                With_Description(name).
                SaveToDb().
                Build();

            var productGroupCreated = DataFacade.ProductGroup.Take(pg => pg.Code == defaultNumber);
            productGroupCreated.With_Description(name + "1").SaveToDb();

            var productGroupUpdated = DataFacade.ProductGroup.Take(pg => pg.Code == defaultNumber).Build();

            Assert.False(productGroup.Description == productGroupUpdated.Description, "ProductGroup wasn't created. Problem is with Description");
        }
        #endregion

        #region Negtive Tests
        /// <summary>
        /// Negative CRUD tests for Product group
        /// </summary>
        [Fact(DisplayName = "ProductGroup CRUD - When Prduct Group is saved without Code Then system shows error message")]
        public void VerifyThatProductCannotBeCreatedWithoutCode()
        {
            var productGroup = DataFacade.ProductGroup.New().
                With_Code(string.Empty).
                With_Description(name);

            var result = BaseDataFacade.ProductGroupService.Save(productGroup, null);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Value of property 'Code' is not specified.", result.Messages.First());
        }

        [Fact(DisplayName = "ProductGroup CRUD - When Prduct Group is saved without Description Then system shows error message")]
        public void VerifyThatProductCannotBeCreatedWithoutDesctiption()
        {
            var productGroup = DataFacade.ProductGroup.New().
                With_Code(defaultNumber).
                With_Description(string.Empty);

            var result = BaseDataFacade.ProductGroupService.Save(productGroup, null);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Value of property 'Cwc.BaseData.Model.ProductGroup.Description' is not specified.", result.Messages.First());
        }
        #endregion
    }
}
