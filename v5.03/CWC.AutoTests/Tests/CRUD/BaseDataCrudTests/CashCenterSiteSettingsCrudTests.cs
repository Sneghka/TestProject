using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.CashCenter.Enums;
using Cwc.Security;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD.BaseDataCrudTests
{
    public class CashCenterSiteSettingsCrudTests : IDisposable
    {
        string code;
        int number;
        DateTime dateUpdated, dateCreated;

        public CashCenterSiteSettingsCrudTests()
        {
            code = $"1314{ new Random().Next(4000, 9999)}";
            number = Int32.Parse(code);
            dateCreated = new DateTime(2017, 08, 30, 2, 5, 30);
            dateUpdated = new DateTime(2017, 08, 30, 2, 10, 30);
        }

        public void Dispose()
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                context.CashCenterSiteSettings.RemoveRange(context.CashCenterSiteSettings.Where(ccss=>ccss.DateCreated == dateCreated && ccss.DateUpdated == dateUpdated));
                context.SaveChanges();
            }
        }

        [Fact(DisplayName = "CashCenterSiteSettings CRUD - CashCenterSiteSettings was created successfully")]

        public void VerifyThatCashCenterSiteSettingsWasCreatedSucessfully()
        {
            var cashCenterSiteSettings = DataFacade.CashCenterSiteSetting.New().
                With_DateCreated(dateCreated).
                With_DateUpdated(dateUpdated).
                With_AtmPickList(AtmPickList.GeneratePerATM).
                With_IsPickListShowWeight(false).
                SaveToDb().
                Build();

            var cashCenterSiteSettingsCreated = DataFacade.CashCenterSiteSetting.Take(ccss => ccss.DateCreated == dateCreated && ccss.DateUpdated == dateUpdated).Build();

            Assert.True(cashCenterSiteSettings.AtmPickList == cashCenterSiteSettingsCreated.AtmPickList, "CashCenterSiteSettings wasn't created. Problem is with AtmPickList");
        }

        [Fact(DisplayName = "CashCenterSiteSettings CRUD - CashCenterSiteSettings was updated successfully")]

        public void VerifyThatCashCenterSiteSettingsWasUpdatedSucessfully()
        {
            var cashCenterSiteSettings = DataFacade.CashCenterSiteSetting.New().
                With_SiteId(1).
                With_DateCreated(dateCreated).
                With_DateUpdated(dateUpdated).
                With_AtmPickList(AtmPickList.GeneratePerATM).
                With_IsPickListShowWeight(false).
                SaveToDb().
                Build();

            var cashCenterSiteSettingsCreated = DataFacade.CashCenterSiteSetting.Take(ccss => ccss.DateCreated == dateCreated && ccss.DateUpdated == dateUpdated);
            cashCenterSiteSettingsCreated.With_AtmPickList(AtmPickList.GeneratePerPackage).SaveToDb();

            var cashCenterSiteSettingsUpdated = DataFacade.CashCenterSiteSetting.Take(ccss => ccss.DateCreated == dateCreated && ccss.DateUpdated == dateUpdated).Build();

            Assert.False(cashCenterSiteSettings.AtmPickList == cashCenterSiteSettingsUpdated.AtmPickList, "CashCenterSiteSettings wasn't updated. Problem is with AtmPickList");
        }

        [Fact(DisplayName = "CashCenterSiteSettings CRUD - CashCenterSiteSettings was deleted successfully")]

        public void VerifyThatCashCenterSiteSettingsWasDeletedSucessfully()
        {
            var cashCenterSiteSettings = DataFacade.CashCenterSiteSetting.New().
                With_DateCreated(dateCreated).
                With_DateUpdated(dateUpdated).
                With_AtmPickList(AtmPickList.GeneratePerATM).
                With_IsPickListShowWeight(false).
                SaveToDb().
                Build();

            DataFacade.CashCenterSiteSetting.Delete(ccss => ccss.DateCreated == dateCreated && ccss.DateUpdated == dateUpdated);

            using (var context = new CashCenterDataContext())
            {
                var cashCenterSiteSettingsCreated = context.CashCenterSiteSettings.FirstOrDefault(ccss => ccss.DateCreated == dateCreated && ccss.DateUpdated == dateUpdated);
                Assert.True(cashCenterSiteSettingsCreated == null, "CashCenterSiteSettings wasn't deleted");
            }
        }

        [Fact(DisplayName = "CashCenterSiteSettings CRUD - When CashCenterSiteSettings child was created with Parent's Level = Site Then system shows error message", Skip = "CashCenter Site Setting child was created with Parent's Level = Site(Doc CWC-CC-Site 3.2.1 1a)")]

        public void VerifyThatCashCenterSiteSettingsCannotBeCreatedWhenParentLevelSite()
        {
            var cashCenterSiteSettings = DataFacade.CashCenterSiteSetting.New().
                With_SiteSubType(1).
                With_SiteId(1).
                With_DateCreated(dateCreated).
                With_DateUpdated(dateUpdated).
                With_AtmPickList(AtmPickList.GeneratePerATM).
                SaveToDb().
                Build();

            var cashCenterSiteSettingsChild = DataFacade.CashCenterSiteSetting.CreateChild(ccss => ccss.DateCreated == dateCreated && ccss.DateUpdated == dateUpdated);
            cashCenterSiteSettingsChild.SetSiteId(2);

            var userID = SecurityFacade.LoginService.GetAdministratorLogin();
            UserParams userParams = new UserParams(userID);
            var result = CashCenterFacade.SiteSettingService.Save(cashCenterSiteSettingsChild, userParams, mode: null, dbParams: null);

            Assert.True(!result.IsSuccess, $"CashCenter Site Setting child was created with Parent's Level = '{cashCenterSiteSettings.Level}'");
            Assert.Equal("Selected level is the lowest. New settings cannot be created.", result.Messages.First());
        }

        [Fact(DisplayName = "CashCenterSiteSettings CRUD - When CashCenterSiteSettings child was created when Parent's Level = Site Then system shows error message")]

        public void VerifyThatCashCenterSiteSettingsDublicateCannotBeCreated()
        {
            var cashCenterSiteSettings = DataFacade.CashCenterSiteSetting.New().
                With_SiteSubType(1).
                With_SiteId(1).
                With_DateCreated(dateCreated).
                With_DateUpdated(dateUpdated).
                With_AtmPickList(AtmPickList.GeneratePerATM).
                With_IsPickListShowWeight(false).
                SaveToDb().
                Build();

            var cashCenterSiteSettingsFirst = DataFacade.CashCenterSiteSetting.New().
                With_SiteSubType(1).
                With_SiteId(1).
                With_DateCreated(dateCreated).
                With_DateUpdated(dateUpdated).
                With_IsPickListShowWeight(false).
                With_AtmPickList(AtmPickList.GeneratePerATM);

            var userID = SecurityFacade.LoginService.GetAdministratorLogin();
            UserParams userParams = new UserParams(userID);
            var result = CashCenterFacade.SiteSettingService.Save(cashCenterSiteSettingsFirst, userParams, mode: null, dbParams: null);

            Assert.True(!result.IsSuccess, "CashCenter Site Setting was created ");
            Assert.Equal("Cash Center Site setting with selected configuration already exists.", result.Messages.First());
        }
    }
}
