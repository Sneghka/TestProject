using Cwc.BaseData;
using Cwc.Coin;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Data;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD.BaseDataCrudTests
{
    [Collection("MyCollection")]
    public class CashPointTypeCrudTests: IDisposable
    {
        string code;
        int number;
         

        public CashPointTypeCrudTests()
        {
            code = $"1314{ new Random().Next(4000, 9999)}";
            number = Int32.Parse(code);
        }

        public void Dispose()
        {
            using (var context = new AutomationCoinDataContext())
            {
                context.CashPointTypes.RemoveRange(context.CashPointTypes.Where(cpt => cpt.Name.StartsWith(code)));
                context.SaveChanges();
            }
        }

        [Fact(DisplayName = "CashPointType CRUD - CashPointType was Created successfully")]

        public void VerifyThatCashPointTypeWasCreatedSuccessfully()
        {
            var cashPointType = DataFacade.CashPointType.New().
                With_Number(number).
                With_Name(code).
                With_UseInOptimization(true).
                With_IsCollect(true).
                With_IsIssue(true).
                With_IsRecycle(true).
                With_HandlingType("ATM").
                SaveToDb().
                Build();

            var cashPointTypeCreated = DataFacade.CashPointType.Take(cpt => cpt.Name == code).Build();

            Assert.True(cashPointType.Number == cashPointTypeCreated.Number, "CashPointType wasn't created. Problem is with Number");
            Assert.True(cashPointType.IsUseInOptimization == cashPointTypeCreated.IsUseInOptimization, "CashPointType wasn't created. Problem is with IsUseInOptimization");
            Assert.True(cashPointType.IsCollect == cashPointTypeCreated.IsCollect, "CashPointType wasn't created. Problem is with IsCollect");
            Assert.True(cashPointType.IsIssue == cashPointTypeCreated.IsIssue, "CashPointType wasn't created. Problem is with IsIssue");
            Assert.True(cashPointType.IsRecycle == cashPointTypeCreated.IsRecycle, "CashPointType wasn't created. Problem is with IsRecycle");
            Assert.True(cashPointType.HandlingType == cashPointTypeCreated.HandlingType, "CashPointType wasn't created. Problem is with HandlingType");
        }

        [Fact(DisplayName = "CashPointType CRUD - CashPointType was Deleted successfully")]

        public void VerifyThatCashPointTypeWasDeletedSuccessfully()
        {
            var cashPointType = DataFacade.CashPointType.New().
                With_Number(number).
                With_Name(code).
                With_UseInOptimization(true).
                With_IsCollect(true).
                With_IsIssue(true).
                With_IsRecycle(true).
                With_HandlingType("ATM").
                SaveToDb();

            DataFacade.CashPointType.Delete(cpt => cpt.Name == code);

            using (var context = new CoinDataContext())
            {
                var result = context.CashPointTypes.FirstOrDefault(cpt => cpt.Name == code);
                Assert.True(result == null, "Cash Point Type wasn't deleted");
            }
        }

        [Fact(DisplayName = "CashPointType CRUD - CashPointType was Updated successfully")]

        public void VerifyThatCashPointTypeWasUpdatedSuccessfully()
        {
            var cashPointType = DataFacade.CashPointType.New().
                With_Number(number).
                With_Name(code).
                With_UseInOptimization(true).
                With_IsCollect(true).
                With_IsIssue(true).
                With_IsRecycle(true).
                With_HandlingType("ATM").
                SaveToDb().Build();

            var cashPointTypeCreated = DataFacade.CashPointType.Take(cpt => cpt.Name == code);
            cashPointTypeCreated.With_Number(number + 1).
                With_HandlingType("DEP").
                SaveToDb();

            var cashPointTypeUpdated = DataFacade.CashPointType.Take(cpt => cpt.Name == code).Build();

            Assert.False(cashPointType.Number == cashPointTypeUpdated.Number, "CashPointType wasn't Updated. Problem is with Number");
            Assert.False(cashPointType.HandlingType == cashPointTypeUpdated.HandlingType, "CashPointType wasn't updated. Problem is with HandlingType");
        }

        [Fact(DisplayName = "CashPointType CRUD - When CashPointType was created without Name Then system shows error message")]

        public void VerifyThatCashPointTypeCannotBeCreatedWithoutName()
        {
            var cashPointType = DataFacade.CashPointType.New().
                With_Number(number).
                With_UseInOptimization(true).
                With_IsCollect(true).
                With_IsIssue(true).
                With_IsRecycle(true).
                With_HandlingType("ATM");

            var result = BaseDataFacade.CashPointTypeService.Save(cashPointType);

            Assert.False(result.IsSuccess,"Cash point type saving should be unsuccessful");
            Assert.Equal("Value of property 'Name' is not specified.", result.Messages.First());
        }

        [Fact(DisplayName = "CashPointType CRUD - When CashPointType was created without Is Use In Optimization Then system shows error message")]

        public void VerifyThatCashPointTypeCannotBeCreatedWithoutIsUseInOptimization()
        {
            var cashPointType = DataFacade.CashPointType.New().
                With_Number(number).
                With_Name(code).
                With_IsIssue(true).
                With_IsRecycle(true).
                With_HandlingType("ATM");

            var result = BaseDataFacade.CashPointTypeService.Save(cashPointType);

            Assert.False(result.IsSuccess, "Cash point type saving should be unsuccessful");
            Assert.Equal("Value of property 'Is Use In Optimization' is not specified.", result.Messages.First());
        }

        [Fact(DisplayName = "CashPointType CRUD - When CashPointType was created without Is Issue Then system shows error message")]

        public void VerifyThatCashPointTypeCannotBeCreatedWithoutIsIssue()
        { 
            var cashPointType = DataFacade.CashPointType.New().
                With_Name(code).
                With_UseInOptimization(true).
                With_HandlingType("ATM");

            var result = BaseDataFacade.CashPointTypeService.Save(cashPointType);

            Assert.False(result.IsSuccess, "Cash point type saving should be unsuccessful");
            Assert.Equal("Please, specify at least one function.", result.Messages.First());
        }

        [Fact(DisplayName ="CAshPointType CRUD - When CashPointType was created without HandlingType Then system shows error message")]

        public void VerifyThatCashPointTypeCannotBeCreatedWithoutHandlingType()
        {
            var cashPointType = DataFacade.CashPointType.New().
                With_Name(code).
                With_UseInOptimization(true).
                With_IsIssue(true);

            var result = BaseDataFacade.CashPointTypeService.Save(cashPointType);

            Assert.False(result.IsSuccess, "Cash point type saving should be unsuccessful");
            Assert.Equal("Value of property 'Handling type' is not specified.", result.Messages.First());
        }


        [Fact(DisplayName = "CAshPointType CRUD - When CashPointType was created with existing name Then system shows error message")]

        public void VerifyThatCashPointTypeDublicatesCannotBeCreated()
        {
            var cashPointTypeFirst = DataFacade.CashPointType.New().
                With_Name(code).
                With_Number(number).
                With_UseInOptimization(true).
                With_IsIssue(true).
                With_HandlingType("ATM").
                SaveToDb();

            var cashPointTypeSecond = DataFacade.CashPointType.New().
                With_Name(code).
                With_Number(number).
                With_UseInOptimization(true).
                With_IsIssue(true).
                With_HandlingType("ATM");

            var result = BaseDataFacade.CashPointTypeService.Save(cashPointTypeSecond);

            Assert.False(result.IsSuccess, "Cash point type saving should be unsuccessful");
            Assert.Equal("Cash Point Type with the same value of property Name already exists.", result.Messages.First());
        }
        
        [Fact(DisplayName = "CashPointType CRUD - When CashPointType was deleted with reference Than system shows error message")]

        public void VerifyThatCashPointTypeCannotBedeletedWithReference()
        {
            var cashPointType = DataFacade.CashPointType.New().
                With_Name(code).
                With_Number(number).
                With_UseInOptimization(true).
                With_IsIssue(true).
                With_HandlingType("ATM").
                SaveToDb();

            var location = DataFacade.Location.InitDefault().
                With_CashPointTypeID(cashPointType.Build().ID).
                SaveToDb().
                Build();

            try
            {
                var result = BaseDataFacade.CashPointTypeService.Delete(cashPointType.Build().ID);

                Assert.False(result.IsSuccess, "CashPointType deletion should be unsuccessful");
                Assert.Equal($"Cash Point Type '{cashPointType.Build().ID}' cannot be deleted. There are other entities linked to it that may lose integrity.", result.Messages.First());
            }
            catch
            {
                throw;
            }

            finally
            {
                using (var context = new AutomationTransportDataContext())
                {
                    context.CitProcessSettingLinks.RemoveRange(context.CitProcessSettingLinks);
                    context.Locations.RemoveRange(context.Locations.Where(l => l.ID == location.CompanyID));
                    context.Customers.RemoveRange(context.Customers.Where(c => c.ID == location.CompanyID));
                    context.SaveChanges();
                }
            }
        }
    }
}
