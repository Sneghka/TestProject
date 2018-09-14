using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.Security;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.Tests.Fixtures;
using System;
using System.Data;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD.BaseDataCrudTests
{

    [Collection("MyCollection")]
    public class CashCenterProcessSettingsCrudTests:IClassFixture<BaseDataFixture>, IDisposable
    {
        LoginResult login;
        string code;
        int number;
        DateTime dateCreated, dateUpdated;
        BaseDataFixture _fixture;


        public CashCenterProcessSettingsCrudTests(BaseDataFixture fixture)
        {
            _fixture = fixture;
            login = SecurityFacade.LoginService.GetAdministratorLogin();
            code = $"1314{ new Random().Next(4000, 9999)}";
            number = Int32.Parse(code);
            dateCreated = new DateTime(2017, 08, 30, 2, 5, 30);
            dateUpdated = new DateTime(2017, 08, 30, 2, 10, 30);
        }
        public void Dispose()
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                context.Customers.RemoveRange(context.Customers.Where(c => c.ReferenceNumber.StartsWith(code)));
                context.CashCenterProcessSettings.RemoveRange(context.CashCenterProcessSettings.Where(c => c.LocationId == _fixture.LocationCC.ID && c.CustomerId == _fixture.Customer.ID && c.LocationTypeCode == "ATM"));
                context.SaveChanges();
            }
        }

        [Fact(DisplayName = "CashCenterProcessSettings CRUD - CashCenterProcessSettings was created successfully")]

        public void VerifyThatCashCenterProcessSettingsWasCreatedSuccessfully()
        {
            var cashCenterProcessSettings = DataFacade.CashCenterProcessSetting.New().
                With_AuthorId(login.UserID).
                With_EditorId(login.UserID).
                With_LocationTypeCode("ATM").
                With_LocationId(_fixture.LocationCC.ID).
                With_CustomerId(_fixture.Customer.ID).
                SaveToDb().
                Build();

            try
            {
                var cashCenterProcessSettingsCreated = DataFacade.CashCenterProcessSetting.Take(ccps => ccps.ID == cashCenterProcessSettings.ID).Build();

                Assert.True(cashCenterProcessSettings.AuthorId == cashCenterProcessSettingsCreated.AuthorId, "Cash Center Process Settings wasn't. Created problem is with AutorId");
                Assert.True(cashCenterProcessSettings.EditorId == cashCenterProcessSettingsCreated.EditorId, "Cash Center Process Settings wasn't. Created problem is with EditorId");
                Assert.True(cashCenterProcessSettings.CustomerId == cashCenterProcessSettingsCreated.CustomerId, "Cash Center Process Settings wasn't. Created problem is with CustomerId");
            }
            catch
            {
                throw;
            }
            finally
            {
                using (var context = new AutomationCashCenterDataContext())
                {
                    context.CashCenterProcessSettingProductLinks.RemoveRange(context.CashCenterProcessSettingProductLinks.Where(c => c.CashCenterProcessSettingId == cashCenterProcessSettings.ID));
                }
            }
        }

        [Fact(DisplayName = "CashCenterProcessSettings CRUD - CashCenterProcessSettings was updated successfully")]

        public void VerifyThatCashCenterProcessSettingsWasUpdatedSuccessfully()
        {
            var customerFirst = DataFacade.Customer.New().
                With_ReferenceNumber(code + "1").
                With_Name("Name").
                With_RecordType(CustomerRecordType.Company).
                With_IBANBankIdentifier("AAAA").
                SaveToDb().
                Build();

            var cashCenterProcessSettings = DataFacade.CashCenterProcessSetting.New().
                With_AuthorId(login.UserID).
                With_EditorId(login.UserID).
                With_LocationTypeCode("ATM").
                With_LocationId(_fixture.LocationCC.ID).
                With_CustomerId(_fixture.Customer.ID).
                SaveToDb().
                Build();

            try
            {
                var cashCenterProcessSettingsCreated = DataFacade.CashCenterProcessSetting.Take(ccps => ccps.ID == cashCenterProcessSettings.ID);
                cashCenterProcessSettingsCreated.With_CustomerId(customerFirst.ID).SaveToDb();

                var cashCenterProcessSettingsUpdated = DataFacade.CashCenterProcessSetting.Take(ccps => ccps.ID == cashCenterProcessSettings.ID).Build();
                Assert.False(cashCenterProcessSettings.CustomerId == cashCenterProcessSettingsUpdated.CustomerId, "Cash Center Process Settings wasn't Updated.");
            }
            catch
            {
                throw;
            }
            finally
            {
                using (var context = new AutomationCashCenterDataContext())
                {
                    context.Customers.RemoveRange(context.Customers.Where(c => c.ReferenceNumber.StartsWith(code)));
                    context.CashCenterProcessSettingProductLinks.RemoveRange(context.CashCenterProcessSettingProductLinks.Where(c => c.CashCenterProcessSettingId == cashCenterProcessSettings.ID));
                    context.CashCenterProcessSettings.RemoveRange(context.CashCenterProcessSettings.Where(c => c.LocationId == _fixture.LocationCC.ID && c.CustomerId == customerFirst.ID && c.LocationTypeCode == "ATM"));
                }
            }
        }

        [Fact(DisplayName = "CashCenterProcessSettings CRUD - CashCenterProcessSettings was deleted successfully")]

        public void VerifyThatCashCenterProcessSettingsWasDeletedSuccessfully()
        {
            var cashCenterProcessSettings = DataFacade.CashCenterProcessSetting.New().
                With_AuthorId(login.UserID).
                With_EditorId(login.UserID).
                With_LocationTypeCode("ATM").
                With_LocationId(_fixture.LocationCC.ID).
                With_CustomerId(_fixture.Customer.ID).
                SaveToDb().
                Build();

            try
            {
                DataFacade.CashCenterProcessSetting.Delete(ccps => ccps.ID == cashCenterProcessSettings.ID);
                using (var context = new AutomationCashCenterDataContext())
                {
                    var cashCenterProcessSettingsUpdated = context.CashCenterProcessSettings.FirstOrDefault(ccps => ccps.ID == cashCenterProcessSettings.ID);

                    Assert.True(cashCenterProcessSettingsUpdated == null, "Cash Center Process Settings wasn't Deleted.");
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                using (var context = new AutomationCashCenterDataContext())
                {
                    context.CashCenterProcessSettingProductLinks.RemoveRange(context.CashCenterProcessSettingProductLinks.Where(c => c.CashCenterProcessSettingId == cashCenterProcessSettings.ID));
                }
            }
        }

        [Fact(DisplayName = "CashCenterProcessSettings CRUD - When CashCenterProcessSettings was created with existing combination of Company, Location anf LocationType Then system shows error message")]
        public void VerifyThatCashCenterProcessSettingsDublicateCannotBeCreated()
        {

            var cashCenterProcessSettingsFirst = DataFacade.CashCenterProcessSetting.New().
                With_AuthorId(login.UserID).
                With_EditorId(login.UserID).
                With_LocationTypeCode("ATM").
                With_CustomerId(_fixture.Customer.ID).
                With_CashPointTypeId(_fixture.LocationCIT.IdentityID).
                SaveToDb().
                Build();
            
            try
            {
                var cashCenterProcessSettingsSecond = DataFacade.CashCenterProcessSetting.New().
                    With_AuthorId(login.UserID).
                    With_EditorId(login.UserID).
                    With_LocationTypeCode("ATM").
                    With_CustomerId(_fixture.Customer.ID).
                    With_CashPointTypeId(_fixture.LocationCIT.IdentityID);

                var lr = SecurityFacade.LoginService.GetAdministratorLogin();
                var userParams = new UserParams(lr);
                var result = CashCenterFacade.ProcessSettingService.Save(cashCenterProcessSettingsSecond, userParams, null);

                Assert.True(!result.IsSuccess, "Cash Center Process Settings Dublicate with existing combination of Company, Location anf LocationType was Created.");
                Assert.Equal("Cash center process setting with selected configuration already exists.", result.Messages.FirstOrDefault());
            }
            catch
            {
                throw;
            }
            finally
            {
                using (var context = new AutomationCashCenterDataContext())
                {
                    context.CashCenterProcessSettingProductLinks.RemoveRange(context.CashCenterProcessSettingProductLinks.Where(c => c.CashCenterProcessSettingId == cashCenterProcessSettingsFirst.ID));
                    context.SaveChanges();
                }
            }
        }

        [Fact(DisplayName = "CashCenterProcessSettings CRUD - When CashCenterProcessSettings was created with existing Location Then system shows error message")]
        public void VerifyThatCashCenterProcessSettingsCannotBeCreatedWithExistingLocation()
        {

            var cashCenterProcessSettingsFirst = DataFacade.CashCenterProcessSetting.New().
                With_AuthorId(login.UserID).
                With_EditorId(login.UserID).
                With_LocationTypeCode("ATM").
                With_LocationId(_fixture.LocationCC.ID).
                With_CustomerId(_fixture.Customer.ID).
               
                SaveToDb().
                Build();

            var cashCenterProcessSettingsSecond = DataFacade.CashCenterProcessSetting.New().
                With_AuthorId(login.UserID).
                With_EditorId(login.UserID).
                With_LocationTypeCode("ATM").
                With_LocationId(_fixture.LocationCC.ID).
                With_CustomerId(_fixture.Customer.ID);
            try
            {
                var lr = SecurityFacade.LoginService.GetAdministratorLogin();
                var userParams = new UserParams(lr);
                var result = CashCenterFacade.ProcessSettingService.Save(cashCenterProcessSettingsSecond, userParams, null);

                Assert.True(!result.IsSuccess, "Cash Center Process Settings Dublicate with existing combination of Company, Location anf LocationType was Created.");
                Assert.Equal("Setting with the same Service Point already exists.", result.Messages.FirstOrDefault());
            }
            catch
            {
                throw;
            }
            finally
            {
                using (var context = new AutomationCashCenterDataContext())
                {
                    context.CashCenterProcessSettingProductLinks.RemoveRange(context.CashCenterProcessSettingProductLinks.Where(c => c.CashCenterProcessSettingId == cashCenterProcessSettingsFirst.ID));
                }
            }
        }

        [Fact(DisplayName = "CashCenterProcessSettings CRUD - When Default CashCenterProcessSettings was deleted Than systen shows error message")]

        public void VerifyThatDefaultCashCenterProcessSettingsCannotBeDeleted()
        {
            var cashCenterProcessSettings = DataFacade.CashCenterProcessSetting.New().
                With_Default(true).
                With_AuthorId(login.UserID).
                With_EditorId(login.UserID).
                With_LocationTypeCode("ATM").
                With_LocationId(_fixture.LocationCC.ID).
                With_CustomerId(_fixture.Customer.ID).
                SaveToDb().
                Build();

            try
            {
                using (var context = new AutomationCashCenterDataContext())
                {
                    var result = CashCenterFacade.ProcessSettingService.Delete(cashCenterProcessSettings, null);

                    Assert.True(!result.IsSuccess, "Cash Center Process Settings was Deleted.");
                    Assert.Equal("Default level of record cannot be deleted", result.Messages.FirstOrDefault());
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                using (var context = new AutomationCashCenterDataContext())
                {
                    context.CashCenterProcessSettingProductLinks.RemoveRange(context.CashCenterProcessSettingProductLinks.Where(c => c.CashCenterProcessSettingId == cashCenterProcessSettings.ID));
                }
            }
        }
    }
}
