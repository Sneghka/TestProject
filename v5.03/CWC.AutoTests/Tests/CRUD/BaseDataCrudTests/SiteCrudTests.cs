using Cwc.BaseData;
using Cwc.BaseData.Enums;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.Tests.Fixtures;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD.BaseDataCrudTests
{
    [Collection("MyCollection")]
    public class SiteCrudTests : IClassFixture<BaseDataFixture>, IDisposable
    {
        string defaultNumber, name;
        BaseDataFixture _fixture;

        public SiteCrudTests(BaseDataFixture fixture)
        {
            defaultNumber = $"1101{new Random().Next(4000, 9999)}";
            name = "AutoTestManagement";
            _fixture = fixture;
        }
        public void Dispose()
        {
            using (var context = new AutomationTransportDataContext())
            {
                context.BaseAddresses.RemoveRange(context.BaseAddresses);
                context.Sites.RemoveRange(context.Sites.Where(b => b.Branch_cd.StartsWith(defaultNumber)));
                context.CitProcessSettingLinks.RemoveRange(context.CitProcessSettingLinks);
                context.Locations.RemoveRange(context.Locations.Where(l => l.Code.StartsWith(defaultNumber)));
                context.SaveChanges();
            }
        }

        #region Positive Tests
        /// <summary>
        /// Positive CRUD tests for Site
        /// </summary>
        [Theory(DisplayName = "Site CRUD - When Site was created successfully")]
        #region TestData
        [InlineData(BranchType.CashCenter, BranchSubType.Coins, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.Notes, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.NotesAndCoins, "CAS")]
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "DEP")]
        #endregion

        public void VerifyThatSiteWasCreatedSuccessfully(BranchType branchType, BranchSubType branchSubType, string handlingType)
        {
            var location = DataFacade.Location.New().
                With_Code(defaultNumber).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(defaultNumber).
                With_HandlingType(handlingType).
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(LocationLevel.ServicePoint).
                SaveToDb().
                Build();

            var site = DataFacade.Site.New().
                With_Branch_cd(defaultNumber).
                With_Description(defaultNumber).
                With_BranchType(branchType).
                With_SubType(branchSubType).
                With_WP_IsExternal(false).
                With_LocationID(location.ID).
                SaveToDb().Build();

            var siteCreated = DataFacade.Site.Take(s => s.Branch_cd == defaultNumber).Build();

            Assert.True(site.Description == siteCreated.Description,"Site wasn't crated. Problem is with Description");
            Assert.True(site.BranchType == siteCreated.BranchType, "Site wasn't crated. Problem is with BranchType");
            Assert.True(site.SubType == siteCreated.SubType, "Site wasn't crated. Problem is with SubType");
            Assert.True(site.WP_IsExternal == siteCreated.WP_IsExternal, "Site wasn't crated. Problem is with IsExternal field");
            Assert.True(site.Location == siteCreated.Location, "Site wasn't crated. Problem is with Description");
        }

        [Theory(DisplayName = "Site CRUD - Site was Updated successfully")]
        #region TestData
        [InlineData(BranchType.CashCenter, BranchSubType.Coins, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.Notes, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.NotesAndCoins, "CAS")]
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "DEP")]
        #endregion

        public void VerifyThatSiteWasUpdatedSuccessfully(BranchType branchType, BranchSubType branchSubType, string handlingType)
        {
            var location = DataFacade.Location.New().
                With_Code(defaultNumber).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(defaultNumber).
                With_HandlingType(handlingType).
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(LocationLevel.ServicePoint).
                SaveToDb().
                Build();

            var site = DataFacade.Site.New().
                With_Branch_cd(defaultNumber).
                With_Description(defaultNumber).
                With_BranchType(branchType).
                With_SubType(branchSubType).
                With_WP_IsExternal(false).
                With_LocationID(location.ID).
                SaveToDb().
                Build();

            var siteCreated = DataFacade.Site.Take(s => s.Branch_cd == defaultNumber);
            siteCreated.With_Description(defaultNumber + "1").
                With_WP_IsExternal(true).
                SaveToDb();

            var siteUpdated = DataFacade.Site.Take(s => s.Branch_cd == defaultNumber).Build();
            Assert.False(siteUpdated.Description == site.Description, "Site wasn't updated. Problem is with Description");
            Assert.False(siteUpdated.WP_IsExternal == site.WP_IsExternal, "Site wasn't updated. The problem with IsExternal");
        }

        [Theory(DisplayName = "Site CRUD - Site was Deleted successfully")]
        #region TestData
        [InlineData(BranchType.CashCenter, BranchSubType.Coins, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.Notes, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.NotesAndCoins, "CAS")]
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "DEP")]
        #endregion

        public void VerifyThatSiteWasDeletedSuccessfully(BranchType branchType, BranchSubType branchSubType, string handlingType)
        {
            var location = DataFacade.Location.New().
                With_Code(defaultNumber).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(defaultNumber).
                With_HandlingType(handlingType).
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(LocationLevel.ServicePoint).
                SaveToDb().
                Build();

            var site = DataFacade.Site.New().
                With_Branch_cd(defaultNumber).
                With_Description(defaultNumber).
                With_BranchType(branchType).
                With_SubType(branchSubType).
                With_WP_IsExternal(false).
                With_LocationID(location.ID).
                SaveToDb().
                Build();

            DataFacade.Site.DeleteOne(s => s.Branch_cd == defaultNumber);

            using (var context = new AutomationBaseDataContext())
            {
                var result = context.Sites.FirstOrDefault(s => s.Branch_cd == defaultNumber);
                Assert.True(result == null,"Site wasn't deleted");
            }
        }
        #endregion

        #region Negtive Tests
        /// <summary>
        /// Negative CRUD tests for Site
        /// </summary>
        [Theory(DisplayName = "Site CRUD - Verify that Site with BranchType = CashCenter cannot be created with wrong handling type")]

        #region TestData
        [InlineData(BranchType.CashCenter, BranchSubType.Coins, "TRK")]
        [InlineData(BranchType.CashCenter, BranchSubType.Notes, "TRK")]
        [InlineData(BranchType.CashCenter, BranchSubType.NotesAndCoins, "TRK")]
        [InlineData(BranchType.CashCenter, BranchSubType.Coins, "ATM")]
        [InlineData(BranchType.CashCenter, BranchSubType.Notes, "ATM")]
        [InlineData(BranchType.CashCenter, BranchSubType.NotesAndCoins, "ATM")]
        [InlineData(BranchType.CashCenter, BranchSubType.Coins, "NOR")]
        [InlineData(BranchType.CashCenter, BranchSubType.Notes, "NOR")]
        [InlineData(BranchType.CashCenter, BranchSubType.NotesAndCoins, "NOR")]
        [InlineData(BranchType.CashCenter, BranchSubType.Coins, "SRV")]
        [InlineData(BranchType.CashCenter, BranchSubType.Notes, "SRV")]
        [InlineData(BranchType.CashCenter, BranchSubType.NotesAndCoins, "SRV")]
        [InlineData(BranchType.CashCenter, BranchSubType.Coins, "DEP")]
        [InlineData(BranchType.CashCenter, BranchSubType.Notes, "DEP")]
        [InlineData(BranchType.CashCenter, BranchSubType.NotesAndCoins, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "TRK")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "TRK")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "TRK")]
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "ATM")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "ATM")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "ATM")]
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "NOR")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "NOR")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "NOR")]
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "SRV")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "SRV")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "SRV")]
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "CAS")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "CAS")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "CAS")]
        #endregion

        public void VerifyThatSiteCachCenterCannotBeCreatesWithWrongHandlingType(BranchType branchType, BranchSubType branchSubType, string handlingType)
        {
            var location = DataFacade.Location.New().
                With_Code(defaultNumber).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(defaultNumber).
                With_HandlingType(handlingType).
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(LocationLevel.ServicePoint).
                SaveToDb().
                Build();

            var site = DataFacade.Site.New().
                With_Branch_cd(defaultNumber).
                With_Description(defaultNumber).
                With_BranchType(branchType).
                With_SubType(branchSubType).
                With_WP_IsExternal(false).
                With_LocationID(location.ID);

            var result = BaseDataFacade.SiteService.Save(site);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            if (branchType == BranchType.CashCenter)
            {
                Assert.Equal("Branches with Type == ‘Cash Center’ can be linked to Locations with Handling Type == ‘CAS’ only.", result.Messages.First());
            }
            else
            {
                Assert.Equal("Branches with Type == ‘CIT Depot’ can be linked to Locations with Handling Type == ‘DEP’ only.", result.Messages.First());
            }
        }

        [Theory(DisplayName = "Site CRUD - Verify that Site cannot be created without Branch Code")]
        #region TestData
        [InlineData(BranchType.CashCenter, BranchSubType.Coins, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.Notes, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.NotesAndCoins, "CAS")]
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "DEP")]
        #endregion

        public void VerifyThatSiteCannotBeCreatesWithoutBranchCode(BranchType branchType, BranchSubType branchSubType, string handlingType)
        {
            var location = DataFacade.Location.New().
                With_Code(defaultNumber).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(defaultNumber).
                With_HandlingType(handlingType).
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(LocationLevel.ServicePoint).
                SaveToDb().
                Build();

            var site = DataFacade.Site.New().
                With_Branch_cd(string.Empty).
                With_Description(defaultNumber).
                With_BranchType(branchType).
                With_SubType(branchSubType).
                With_WP_IsExternal(false).
                With_LocationID(location.ID);

            var result = BaseDataFacade.SiteService.Save(site);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Please, specify Branch code", result.Messages.First());
        }

        [Theory(DisplayName = "Site CRUD - Verify that Site cannot be created without Description")]
        #region TestData
        [InlineData(BranchType.CashCenter, BranchSubType.Coins, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.Notes, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.NotesAndCoins, "CAS")]
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "DEP")]
        #endregion

        public void VerifyThatSiteCannotBeCreatesWithoutDescription(BranchType branchType, BranchSubType branchSubType, string handlingType)
        {
            var location = DataFacade.Location.New().
                With_Code(defaultNumber).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(defaultNumber).
                With_HandlingType(handlingType).
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(LocationLevel.ServicePoint).
                SaveToDb().
                Build();

            var site = DataFacade.Site.New().
                With_Branch_cd(defaultNumber).
                With_Description(string.Empty).
                With_BranchType(branchType).
                With_SubType(branchSubType).
                With_WP_IsExternal(false).
                With_LocationID(location.ID);

            var result = BaseDataFacade.SiteService.Save(site);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Please, specify Description", result.Messages.First());
        }

        [Theory(DisplayName = "Site CRUD - Verify that Site cannot be created without Location")]
        #region TestData
        [InlineData(BranchType.CashCenter, BranchSubType.Coins, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.Notes, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.NotesAndCoins, "CAS")]
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "DEP")]
        #endregion

        public void VerifyThatSiteCannotBeCreatesWithoutLocation(BranchType branchType, BranchSubType branchSubType, string handlingType)
        {
            var site = DataFacade.Site.New().
                With_Branch_cd(defaultNumber).
                With_Description(defaultNumber).
                With_BranchType(branchType).
                With_SubType(branchSubType).
                With_WP_IsExternal(false);

            var result = BaseDataFacade.SiteService.Save(site);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Please, specify Location code", result.Messages.First());
        }


        [Theory(DisplayName = "Site CRUD - When Site with not unique Branch code Then System doesn't allow to save it")]
        #region TestData
        [InlineData(BranchType.CashCenter, BranchSubType.Coins, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.Notes, "CAS")]
        [InlineData(BranchType.CashCenter, BranchSubType.NotesAndCoins, "CAS")]
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "DEP")]
        #endregion

        public void VerifyThatSiteCannotBeCreatesWith(BranchType branchType, BranchSubType branchSubType, string handlingType)
        {
            var location = DataFacade.Location.New().
                With_Code(defaultNumber).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(defaultNumber).
                With_HandlingType(handlingType).
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(LocationLevel.ServicePoint).
                SaveToDb().
                Build();

            var siteFirst = DataFacade.Site.New().
                With_Branch_cd(defaultNumber).
                With_Description(defaultNumber).
                With_BranchType(branchType).
                With_SubType(branchSubType).
                With_WP_IsExternal(false).
                With_LocationID(location.ID).SaveToDb();

            var siteSecond = DataFacade.Site.New().
                With_Branch_cd(defaultNumber).
                With_Description(defaultNumber).
                With_BranchType(branchType).
                With_SubType(branchSubType).
                With_WP_IsExternal(false).
                With_LocationID(location.ID);

            var result = BaseDataFacade.SiteService.Save(siteSecond);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal($"Site with given Code '{defaultNumber}' already exists.", result.Messages.First());
        }

        [Theory(DisplayName = "Site CRUD - When Site was deleted with reference Then system shows error message")]
        #region TestData
        [InlineData(BranchType.CITDepot, BranchSubType.Coins, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.Notes, "DEP")]
        [InlineData(BranchType.CITDepot, BranchSubType.NotesAndCoins, "DEP")]
        #endregion
        public void VerifyThatSiteCannotBeDeletedWithReference(BranchType branchType, BranchSubType branchSubType, string handlingType)
        {
            var locationFirst = DataFacade.Location.New().
                With_Code(defaultNumber).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(defaultNumber).
                With_HandlingType(handlingType).
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(LocationLevel.ServicePoint).
                SaveToDb().
                Build();

            var site = DataFacade.Site.New().
                With_Branch_cd(defaultNumber).
                With_Description(defaultNumber).
                With_BranchType(branchType).
                //With_SubType(branchSubType).
                With_WP_IsExternal(false).
                With_LocationID(locationFirst.ID).
                SaveToDb().
                Build();

            var locationSecond = DataFacade.Location.New().
                With_Code(defaultNumber + "1").
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(defaultNumber).
                With_HandlingType(handlingType).
                With_ServicingDepotID(site.ID).
                With_Level(LocationLevel.ServicePoint).
                SaveToDb().
                Build();

            var result = BaseDataFacade.SiteService.Delete(site);

            Assert.False(result.IsSuccess , "Site was deleted");
            Assert.Equal("One or more objects 'Site' cannot be deleted. There are other entities linked to it that may lose integrity", result.Messages.First());
        }
        #endregion
    }
}
