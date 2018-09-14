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
    public class BagTypeCrudTests : IDisposable
    {
        string defaultNumber;
        int code;

        public BagTypeCrudTests()
        {
            defaultNumber = $"1101{new Random().Next(4000, 9999)}";
            code = Int32.Parse(defaultNumber);
        }

        public void Dispose()
        {
            using (var context = new AutomationBaseDataContext())
            {
                context.BagTypes.RemoveRange(context.BagTypes.Where(bt=>bt.ID == code));
                context.SaveChanges();
            }            
        }

        [Fact(DisplayName = "BagType CRUD - BagType was created successfully")]

        public void VerifyThatBagTypeWasCreatedSuccessfully()
        {
            var bagType = DataFacade.ContainerType.New().
                With_Number(code).
                With_Code(defaultNumber).
                With_Description(defaultNumber).
                With_BarcdIdent(defaultNumber).
                SaveToDb().
                Build();

            var bagTypeCreated = DataFacade.ContainerType.Take(ba => ba.ID == code).Build();

            Assert.True(bagType.Code == bagTypeCreated.Code, "BagType wasn't created. Problem is with Code");
            Assert.True(bagType.Description == bagTypeCreated.Description, "BagType wasn't created. Problem is with Description");
            Assert.True(bagType.BarcdIdent == bagTypeCreated.BarcdIdent, "BagType wasn't created. Pr;oblem is eith BarcdIdent");
        }

        [Fact(DisplayName = "BagType CRUD - BagType was Updated successfully")]

        public void VerifyThatBagTypeWasUpdatedSuccessfully()
        {
            var bagType = DataFacade.ContainerType.New().
                With_Number(code).
                With_Code(defaultNumber).
                With_Description(defaultNumber).
                With_BarcdIdent(defaultNumber).
                SaveToDb().
                Build();

            var bagTypeCreated = DataFacade.ContainerType.Take(ba => ba.ID == code);
            bagTypeCreated.With_Description(defaultNumber + "1").SaveToDb();

            var bagTypeUpdated = DataFacade.ContainerType.Take(bt => bt.ID == code).Build();

            Assert.False(bagType.Description == bagTypeUpdated.Description, "BagType wasn't Updated. Problem is with Code");
        }

        [Fact(DisplayName = "BagType CRUD - BagType was Deleted successfully")]

        public void VerifyThatBagTypeWasDeletedSuccessfully()
        {
            var bagType = DataFacade.ContainerType.New().
                With_Number(code).
                With_Code(defaultNumber).
                With_Description(defaultNumber).
                With_BarcdIdent(defaultNumber).
                SaveToDb().
                Build();

            DataFacade.ContainerType.DeleteMany(bt => bt.ID == code);

            var result = BaseDataFacade.BagTypeService.Load(bagType.ID);   
                     
            Assert.True(result == null, "BagType wasn't deleted");
        }

        [Fact(DisplayName = "BagType CRUD - When BagType was created without ID Then system shows error message")]

        public void VerifyThatBagTypeCannotCreateWithoutId()
        {
            var bagType = DataFacade.ContainerType.New().
                With_Code(defaultNumber).
                With_Description(defaultNumber).
                With_BarcdIdent(defaultNumber);

            var result = BaseDataFacade.BagTypeService.Save(bagType);

            Assert.False(result.IsSuccess, "BagType was Saved without ID.");
            Assert.Equal("Value of property 'Cwc.BaseData.BagType.ID' is not specified.", result.Messages.FirstOrDefault());
        }

        [Fact(DisplayName = "BagType CRUD - When BagType was created without Code Then system shows error message")]

        public void VerifyThatBagTypeWaseCannotCreateWithoutCode()
        {
            var bagType = DataFacade.ContainerType.New().
                With_Number(code).
                With_Description(defaultNumber);

            var result = BaseDataFacade.BagTypeService.Save(bagType);

            Assert.False(result.IsSuccess, "BagType was Saved without Code.");
            Assert.Equal("Value of property 'Code' is not specified.", result.Messages.FirstOrDefault());
        }

        [Fact(DisplayName = "BagType CRUD - When BagType was created without Description Then system shows error message")]

        public void VerifyThatBagTypeWaseCannotCreateWithoutDescription()
        {
            var bagType = DataFacade.ContainerType.New().
                With_Number(code).
                With_Code(defaultNumber);

            var result = BaseDataFacade.BagTypeService.Save(bagType);

            Assert.False(result.IsSuccess, "BagType was Saved without Description.");
            Assert.Equal("Value of property 'Description' is not specified.", result.Messages.FirstOrDefault());
        }
    }
}
