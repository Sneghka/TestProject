using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.BaseData.Enums;
using Cwc.Feedings;
using Cwc.Security;
using Cwc.Sync;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.Tests.Fixtures;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.AutoTests
{
    [Collection("MyCollection")]
    public class LocationListFeedingTest : IClassFixture<BaseDataFixture>, IDisposable
    {
        AutomationFeedingDataContext feedingContext;
        AutomationTransportDataContext _context;
        string refNumber, name;
        BaseDataFixture _fixture;

        public LocationListFeedingTest(BaseDataFixture fixture)
        {
            ConfigurationKeySet.Load();
            SyncConfiguration.LoadExportMappings();
            feedingContext = new AutomationFeedingDataContext();
            _context = new AutomationTransportDataContext();
            refNumber = $"1101{new Random().Next(10000, 99999999)}";
            name = "AutoTestManagement";
            _fixture = fixture;

        }

        [Fact(DisplayName = "Location list - When Location is submitted without Code Then System doesn't allow to import this location")]
        public void VerifyThatLocationCannotBeCreatedWithoutSpecifiedCode()
        {
            var location = DataFacade.Location.New().
                With_Code(String.Empty).
                With_Name(name).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Mandatory attribute 'Code' is not submitted") && m.Result == ValidatedFeedingLogResult.Failed).Any());
        }

        [Fact(DisplayName = "Location list - When Location is submitted without Name Then System doesn't allow to import this location")]
        public void VerifyThatLocationCannotBeCreatedWithoutSpecifiedName()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(string.Empty).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Mandatory attribute 'Name' is not submitted") && m.Message.Contains(refNumber) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Location is submitted without Customer Then System doesn't allow to import this location")]
        public void VerifyThatLocationCannotBeCreatedWithoutSpecifiedCustomer()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(DataFacade.Customer.New().With_ReferenceNumber("")).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Mandatory attribute 'CompanyNumber' is not submitted") && m.Message.Contains(refNumber) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Location is submitted without LocationTyoe Then System doesn't allow to import this location")]
        public void VerifyThatLocationCannotBeCreatedWithoutSpecifiedLocationTyoe()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(_fixture.Customer).
                With_LocationTypeID(DataFacade.LocationType.New().With_ltCode("").Build().ltCode).
                With_HandlingType("NOR").
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Mandatory attribute 'LocationTypeCode' is not submitted") && m.Message.Contains(refNumber) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Service Type is submitted without HandlingTyoe Then System doesn't allow to import this location")]
        public void VerifyThatLocationCannotBeCreatedWithoutSpecifiedHandlingTyoe()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("").
                With_Level(LocationLevel.ServicePoint).
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Value of property 'Handling Type' is not specified.") && m.Message.Contains(refNumber) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When submitted Customer is not exists Then System doesn't allow to import this location")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithNonExistedCustomer()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(DataFacade.Customer.New().With_ReferenceNumber(refNumber + "1")).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Customer with provided Reference Number ‘{refNumber + "1"}’ does not exist";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When submitted LocationType is not exists Then System doesn't allow to import this location")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithNonExistedLocationType()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(_fixture.Customer).
                With_LocationTypeID(DataFacade.LocationType.New().With_ltCode(refNumber).Build().ltCode).
                With_HandlingType("NOR").
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Location Type with provided Code ‘{refNumber}’ does not exist.";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When submitted HandlingType is not exists Then System doesn't allow to import this location")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithNonExistedHandlingType()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType(refNumber).
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Handling Type with provided lctyp_cd ‘{refNumber}’ does not exist";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When submitted MainLocation is not exists Then System doesn't allow to import this location")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithNonExistedMainLocation()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_ServicingDepot(_fixture.SiteCIT).
                With_MainLocation(DataFacade.Location.New().With_Code(refNumber + "1"));

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Location with provided Code ‘{refNumber + "1"}’ does not exist";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When submitted Default Cit Site is not exists Then System doesn't allow to import this location")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithNonExistedDefCitSite()
        {
            var citSiteTemp = DataFacade.Site.InitDefault().With_Location(_fixture.LocationCIT).With_BranchType(BranchType.CITDepot).SaveToDb().Build();

            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_ServicingDepot(citSiteTemp);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            DataFacade.Site.DeleteMany(x => x.ID == citSiteTemp.ID);

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Site with provided Code ‘{citSiteTemp.Branch_cd}’ does not exist.";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When submitted Default Cit Site is not exists Then System doesn't allow to import this location")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithNonExistedNoteSite()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_NotesSite(DataFacade.Site.New().With_Branch_cd(refNumber)).
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Site with provided Code ‘{refNumber}’ does not exist.";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When submitted Default Cit Site is not exists Then System doesn't allow to import this location")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithNonExistedCoinSite()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_CoinsSite(DataFacade.Site.New().With_Branch_cd(refNumber)).
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Site with provided Code ‘{refNumber}’ does not exist.";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When submitted Default Coin Cit Site is not exists Then System doesn't allow to import this location")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithNonExistedCitCoinSite()
        {
            var site = DataFacade.Site.New().InitDefault().With_BranchType(BranchType.CITDepot).With_Branch_cd(refNumber).With_Location(_fixture.LocationCIT).Build();
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_ServicingDepotCoins(site).
                With_ServicingDepot(_fixture.SiteCIT);           

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Site with provided Code ‘{refNumber}’ does not exist.";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When submitted Bank Account is not exists Then System doesn't allow to import this location")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithNonExistedBankAcc()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_BankAccount(DataFacade.BankAccount.New(refNumber)).
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Bank Account with provided Number ‘{refNumber}’ does not exist";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When submitted Ordering Department is not exists Then System doesn't allow to import this location")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithNonExistedOrderingDep()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_OrderingDepartment(DataFacade.Group.New().With_Name(refNumber)).
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Group with provided Name ‘{refNumber}’ does not exist";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When location is submitted with empty abbrev Then System replaces it with Code value")]
        public void VerifyThatEmptyAbbrevIsReplacedWithCode()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(_context.Locations.Where(l => l.Code == refNumber && l.Abbreviation == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Handling Type <> {‘CAS’, ‘DEP’} and Default Cit Site is not submitted Then System doesn't allow to submit this location")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithoutDefCitSite()
        {
            var handlingType = "NOR";
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Company(_fixture.Customer).
                With_HandlingType(handlingType);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Default servicing CIT site must be specified for location with handling type '{handlingType}'";

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Handling Type is ‘DEP’and Default Cit Site is not submitted Then System allows to submit this location")]
        public void VerifyThatSystemAllowToImportLocationWithoutDefCitSiteForDep()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Company(_fixture.Customer).
                With_HandlingType("DEP");

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("successfully created")).Any());
            Assert.True(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Handling Type is ‘CAS’and Default Cit Site is not submitted Then System allows to submit this location")]
        public void VerifyThatSystemAllowToImportLocationWithoutDefCitSiteForCas()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Company(_fixture.Customer).
                With_HandlingType("CAS");

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("successfully created")).Any());
            Assert.True(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Main location code is equals to imported location type Then System doesn't allow to import current location")]
        public void verifyThatSystemDoesntAllowToSetlocationAsMainLocationForItself()
        {
            var location = DataFacade.Location.New().
                With_Code(refNumber).
                With_Abbreviation("abbrev").
                With_Name(name).
                With_Enabled(true).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_CompanyID(_fixture.Customer.ID).
                With_HandlingType("NOR").
                With_ServicingDepot(_fixture.SiteCIT).
                SaveToDb();

            location.With_MainLocation(DataFacade.Location.New().With_Code(refNumber)).With_Company(_fixture.Customer).With_Enabled(false);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMSG = $"Main location ‘{location.Build().DisplayCaption}’ cannot be used, since it will cause circular references between locations";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMSG) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber && !l.Enabled).Any());
        }

        [Fact(DisplayName = "Location list - When imported location is caused circular reference Then System doesn't allow to import current location", Skip = "CWC8655 should be fixed")]
        public void VerifyThatSystemShouldntAllowCircularReferences()
        {
            var location1 = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_CompanyID(_fixture.Customer.IdentityID)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .SaveToDb();
            var location2 = DataFacade.Location.New()
                                            .With_Code(refNumber + "1")
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_CompanyID(_fixture.Customer.IdentityID)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_MainLocation(location1)
                                            .SaveToDb();

            location1.With_MainLocation(location2).With_Enabled(false);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location1.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMSG = $"Main location ‘{location2.Build().DisplayCaption}’ cannot be used, since it will cause circular references between locations";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMSG) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber && l.Enabled).Any());
        }

        [Fact(DisplayName = "Location list - When imported Notes site is Cit Site Then System doesn't allow to save this location")]
        public void VerifyThatNotesSiteCannotBeCitSite()
        {
            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_NotesSite(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Notes Site must be configured as cash center with sub-type ‘notes’ or ‘notes and coins’") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When imported Notes site is Coin Site Then System doesn't allow to save this location")]
        public void VerifyThatNotesSiteCannotBeCoinSite()
        {

            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_NotesSite(_fixture.SiteCoins);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Notes Site must be configured as cash center with sub-type ‘notes’ or ‘notes and coins’") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When imported Coin site is Cit Site Then System doesn't allow to save this location")]
        public void VerifyThatCoinsSiteCannotBeCitSite()
        {
            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_CoinsSite(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Coins Site must be configured as cash center with sub-type ‘coins’ or ‘notes and coins’") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When imported Coin site is Notes Site Then System doesn't allow to save this location")]
        public void VerifyThatCoinsSiteCannotBeNotesSite()
        {

            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_CoinsSite(_fixture.SiteNotes);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Coins Site must be configured as cash center with sub-type ‘coins’ or ‘notes and coins’") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Notes site is set as a Notes and Coins Then System saves this location")]
        public void VerifyThatNotesSiteCouldBeNotesAndCoind()
        {

            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_NotesSite(_fixture.SiteNotesAndCoins);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(_context.Locations.Any(l => l.Code == refNumber && l.NotesSiteID == _fixture.SiteNotesAndCoins.Branch_nr));
        }

        [Fact(DisplayName = "Location list - When Coins site is set as a Notes and Coins Then System saves this location")]
        public void VerifyThatCoinsSiteCouldBeNotesAndCoind()
        {

            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_CoinsSite(_fixture.SiteNotesAndCoins);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(_context.Locations.Any(l => l.Code == refNumber && l.CoinsSiteID == _fixture.SiteNotesAndCoins.Branch_nr));
        }

        [Fact(DisplayName = "Location list - When Servicing depot is not Cit Site Then System doesn't allow to import current location")]
        public void VerifyThatSystemDoesntAllowToSetServicingDepoWithNotCitSite()
        {

            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteNotes);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Default servicing CIT site must be configured as CIT depot") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Servicing Coin depot is not Cit Site Then System doesn't allow to import current location")]
        public void VerifyThatSystemDoesntAllowToSetServicingCoinDepoWithNotCitSite()
        {

            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_ServicingDepotCoinsID(_fixture.SiteNotes.ID);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Coins servicing CIT site must be configured as CIT depot") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Configuration keys are configured Then Atm and Atm type are mandatory")]
        public void VerifyWhenCongigKeysreSetThenAtmIsMandatory()
        {
            var configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == Cwc.BaseData.Enums.ConfigurationKeyName.LocationCashPointNumberAndTypeAreMandatory);
            HelperFacade.ConfigurationKeysHelper.Update(configKey, "True");

            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("ATM")
                                            .With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Cash point number and Cash point type are mandatory for locations with handling type ATM.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Configuration keys are configured and ATM Type is empty Then System doesn't allow to import this location")]
        public void VerifyWhenCongigKeysreSetAndAtmTypeIsEmptyThenAtmIsMandatory()
        {
            var configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == Cwc.BaseData.Enums.ConfigurationKeyName.LocationCashPointNumberAndTypeAreMandatory);
            HelperFacade.ConfigurationKeysHelper.Update(configKey, "True");

            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("ATM")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_CashPointNumber(refNumber);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Cash point number and Cash point type are mandatory for locations with handling type ATM.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Configuration keys are configured and ATMID is empty Then System doesn't allow to import this location")]
        public void VerifyWhenCongigKeysreSetAndAtmIdIsEmptyThenAtmIsMandatory()
        {
            var configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == Cwc.BaseData.Enums.ConfigurationKeyName.LocationCashPointNumberAndTypeAreMandatory);
            HelperFacade.ConfigurationKeysHelper.Update(configKey, "True");

            var cashPointType = DataFacade.CashPointType.Take(x => x.Description != null)?.Build();

            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("ATM")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_CashPointTypeID(cashPointType?.ID);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Cash point number and Cash point type are mandatory for locations with handling type ATM.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Configuration keys are configured and ATMID and ATM Type are configured Then System import this location")]
        public void VerifyWhenCongigKeysreSetMandatoryAreSetThenAtmIsMandatory()
        {
            var configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == Cwc.BaseData.Enums.ConfigurationKeyName.LocationCashPointNumberAndTypeAreMandatory);
            HelperFacade.ConfigurationKeysHelper.Update(configKey, "True");

            var atmType = DataFacade.CashPointType.Take(x => x.Name != null).Build(); // change Name to Number

            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("ATM")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_CashPointTypeID(atmType.ID)
                                            .With_CashPointNumber(refNumber);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("successfully created") && m.Result == ValidatedFeedingLogResult.Ok).Any());
            Assert.True(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Cash Point number is specified and it's not unique among enable locations Then System doesn't allow to save this location")]
        public void VerifyWhenCashPointNumberIsNotUniqueAmongOtherEnabledLocationsThenSystemShowsErrorMessage()
        {

            var location = DataFacade.Location.New()
                                            .With_Code(refNumber + "1")
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_CompanyID(_fixture.Customer.IdentityID)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_CashPointNumber(refNumber)
                                            .SaveToDb();

            var locationWithNonUnique = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_CashPointNumber(refNumber);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { locationWithNonUnique.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Another enabled location with same Cash Point Number already exists.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When address is not valid Then location is not saved")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithInvalidAddress()
        {
            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_Address(DataFacade.Address.New().With_Country("Ukraine"));

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("City and Street are mandatory upon specifying address data") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When City is not specified Then location is not saved")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithputCity()
        {
            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_Address(DataFacade.Address.New().With_Street("street"));

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("City and Street are mandatory upon specifying address data") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When Street is not specified Then location is not saved")]
        public void VerifyThatSystemDoesntAllowToImportLocationWithputStreet()
        {
            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_Address(DataFacade.Address.New().With_City("city"));

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("City and Street are mandatory upon specifying address data") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When address is valid Then location is saved")]
        public void VerifyThatLocationWithValidAddressIsSaved()
        {
            var location = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_Address(DataFacade.Address.New().With_Country(refNumber).With_City(refNumber).With_Street(refNumber).With_State(refNumber));

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var respomse = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var locationId = DataFacade.Location.Take(l => l.Code == refNumber).Build().ID;

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("successfully created") && m.Result == ValidatedFeedingLogResult.Ok).Any());
            Assert.True(_context.BaseAddresses.Where(a => a.City == refNumber && a.Country == refNumber && a.Street == refNumber && a.ObjectID == locationId).Any());
            Assert.True(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When location list contains both valid and invalid locations Then System saves all valid and rejected all invalid")]
        public void VerifyThatSystemImportsAllValidAndRejectsAllInvalidLocationsFromList()
        {
            var validLocation = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT);

            var invalidLocation = DataFacade.Location.New()
                                            .With_Code(refNumber + "1")
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR");

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { validLocation.Build(), invalidLocation.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(_context.Locations.Where(l => l.Code == refNumber).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber + "1").Any());
        }

        [Fact(DisplayName = "Location list - When location list contains both valid and invalid locations Then System logs all valid and all invalid locations")]
        public void VerifyThatSystemLogsAllValidAndRejectsAllInvalidLocationsFromList()
        {
            var validLocation = DataFacade.Location.New()
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT);

            var invalidLocation = DataFacade.Location.New()
                                            .With_Code(refNumber + "1")
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR");

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { validLocation.Build(), invalidLocation.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("successfully created") && m.Result == ValidatedFeedingLogResult.Ok).Any());
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber + "1") && m.Message.Contains("Default servicing CIT site must be specified for location with handling type") && m.Result == ValidatedFeedingLogResult.Failed).Any());
        }

        [Fact(DisplayName = "Location list - When service point customer is not equals to visit address customer Then System doesn't allow t import such location")]
        public void VerifyThatVisitAddressCustomerSHouldBeEqualsToServicePointCustomer()
        {
            var visitAddressCustomer = DataFacade.Customer.Take(c => c.ID != _fixture.Customer.ID && c.Enabled);
            var visitAdress = DataFacade.Location.New()
                .With_Level(LocationLevel.VisitAddress)
                .With_Code(refNumber + "1")
                .With_Abbreviation("abbrev")
                .With_Name(name).With_Enabled(true)
                .With_CompanyID(visitAddressCustomer.Build().ID )
                .With_LocationTypeID(_fixture.LocationType.ltCode)
                .With_ServicingDepot(_fixture.SiteCIT)
                .SaveToDb()
                .Build();

            var servicepoint = DataFacade.Location.New()
                                            .With_Level(LocationLevel.ServicePoint)
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType(BaseDataFacade.LocationNorType)
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_VisitAddressID(visitAdress.IdentityID);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { servicepoint.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Visit address company {visitAddressCustomer.Build().DisplayCaption} is not the company of current service point ({_fixture.Customer.DisplayCaption}).";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(assertMsg) && m.Message.Contains(refNumber) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - When new Visit Address customer is not equals to linked Servie Point customer Then System changes service point customer to new one", Skip = "Haven't found use case for this check")]
        public void VerifyThatVisitAddressCustomerShouldBeEqualsToEveryLinkedLocationCustomer()
        {
            var visitAddressCustomer = DataFacade.Customer.Take(c => c.ID != _fixture.Customer.ID && c.Enabled && c.RecordType == (int)CustomerRecordType.Company).Build();
            var visitAddress = DataFacade.Location.New()
                .With_Level(LocationLevel.VisitAddress)
                .With_Code(refNumber + "1")
                .With_Abbreviation("abbrev")
                .With_Name(name)
                .With_Enabled(true)
                .With_CompanyID(_fixture.Customer.IdentityID)
                .With_LocationTypeID(_fixture.LocationType.ltCode)
                .With_ServicingDepot(_fixture.SiteCIT)
                .SaveToDb()
                .Build();

            var servicePoint = DataFacade.Location.New()
                .With_Level(LocationLevel.ServicePoint)
                .With_Code(refNumber)
                .With_Abbreviation("abbrev")
                .With_Name(name)
                .With_Enabled(true)
                .With_LocationTypeID(_fixture.LocationType.ltCode)
                .With_CompanyID(_fixture.Customer.IdentityID)
                .With_HandlingType(BaseDataFacade.LocationNorType)
                .With_ServicingDepot(_fixture.SiteCIT)
                .With_VisitAddress(visitAddress)
                .SaveToDb();

            var visitAdressFromDB = DataFacade.Location.Take(l => l.Level == LocationLevel.VisitAddress && l.Code == (visitAddress.Code));
            visitAdressFromDB.With_CompanyID(visitAddressCustomer.IdentityID);
            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { visitAddress });
            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());
            var foundUpdatedServicePoint = _context.Locations.FirstOrDefault(x => x.Code == refNumber);
            var foundUpdatedVisitAddress = _context.Locations.FirstOrDefault(x => x.Code == refNumber + "1");

            Assert.Equal(visitAddressCustomer.IdentityID, foundUpdatedServicePoint.ID);
            Assert.Equal(visitAddressCustomer.IdentityID, foundUpdatedVisitAddress.ID);
        }

        [Fact(DisplayName = "Location list - When visit address is mandatory Then System doesn't allow to import service point without")]
        public void VerifyWhenVisiTAddressIsManDatoryThenSystemDoesntAllowToSaveServicePOintWithout()
        {
            var configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == ConfigurationKeyName.ServicePointVisitAddressIsMandatory);
            HelperFacade.ConfigurationKeysHelper.Update(configKey, "True");

            var servicepoint = DataFacade.Location.New()
                                            .With_Level(LocationLevel.ServicePoint)
                                            .With_Code(refNumber)
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_HandlingType(BaseDataFacade.LocationNorType);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { servicepoint.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Address is mandatory for service point.") && m.Message.Contains(refNumber) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Locations.Where(l => l.Code == refNumber).Any());
        }

        [Fact(DisplayName = "Location list - Location list - When several locations are valid Then System imports them all")]
        public void VerifyThatSystemIsAbleToSaveSeveralLocations()
        {
            var companyId = _fixture.Customer.ID;
            var firstLocation = DataFacade.Location.New()
                                           .With_Code(refNumber)
                                           .With_Abbreviation("abbrev")
                                           .With_Name(name)
                                           .With_Enabled(true)
                                           .With_LocationTypeID(_fixture.LocationType.ltCode)
                                           .With_Company(_fixture.Customer)
                                           .With_HandlingType("NOR")
                                           .With_ServicingDepot(_fixture.SiteCIT);

            var secondLocation = DataFacade.Location.New()
                                            .With_Code(refNumber + "1")
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name)
                                            .With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_Company(_fixture.Customer)
                                            .With_HandlingType("NOR")
                                            .With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { firstLocation.Build(), secondLocation.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(_context.Locations.Where(l => l.Code == refNumber).Any());
            Assert.True(_context.Locations.Where(l => l.Code == refNumber + "1").Any());
        }

        [Fact(DisplayName = "Location list - When handling type is empty and Level = Visit Address Then System allows to import this location")]
        public void VerifyThatVisitAddressCouldBeSavedWithoutHandlingType()
        {
            var visitAdress = DataFacade.Location.New()
                .With_Level(LocationLevel.VisitAddress)
                .With_Code(refNumber)
                .With_LocationTypeID(_fixture.LocationType.ltCode)
                .With_Name(name).With_Enabled(true)
                .With_Company(_fixture.Customer)
                .With_ServicingDepot(_fixture.SiteCIT);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { visitAdress.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            Assert.True(_context.Locations.Where(l => l.Code == refNumber && l.Level == (int)LocationLevel.VisitAddress).Any());
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(refNumber) && l.Result == ValidatedFeedingLogResult.Ok).Any());
        }


        [Fact(DisplayName = "Location list - When main location is empty and visit address is not empty Then System sets address from visit address", Skip = "Have not found use case in specification")]
        public void VerifyThatAddressCouldBeSetFromVisitAddress()
        {
            var visitAdress = DataFacade.Location.New()
                .With_Level(LocationLevel.VisitAddress)
                .With_Code(refNumber)
                .With_Abbreviation("abbrev")
                .With_HandlingType("NOR")
                .With_LocationTypeID(_fixture.LocationType.ltCode)
                .With_Name(name).With_Enabled(true)
                .With_CompanyID(_fixture.Customer.ID)
                .With_ServicingDepot(_fixture.SiteCIT)
                .With_Address(DataFacade.Address.New().With_Country(refNumber).With_City(refNumber).With_Street(refNumber).With_State(refNumber))
                .SaveToDb();

            var servicepoint = DataFacade.Location.New()
                                            .With_Level(LocationLevel.ServicePoint)
                                            .With_Code(refNumber + "1")
                                            .With_Abbreviation("abbrev")
                                            .With_Name(name).With_Enabled(true)
                                            .With_LocationTypeID(_fixture.LocationType.ltCode)
                                            .With_CompanyID(_fixture.Customer.ID)
                                            .With_HandlingType(BaseDataFacade.LocationNorType)
                                            .With_ServicingDepot(_fixture.SiteCIT)
                                            .With_VisitAddress(visitAdress.Build())
                                            .SaveToDb();

            var location = DataFacade.Location.Take(x => x.Code == refNumber + "1").Build();
            Assert.True(_context.BaseAddresses.Where(x => x.ObjectID == location.ID && x.City == refNumber && x.Street == refNumber).Any());
        }

        [Theory(DisplayName ="LocationList - Location with InvoiceReference, InboundReference, OutboundReference up to 50 symbols can be created")]
        [InlineData(LocationLevel.ServicePoint)]
        [InlineData(LocationLevel.VisitAddress)]
        public void VerifyThatLocationCanBeCreatedWithReferenceFields(LocationLevel level)
        {
            var reference = "12345123451234512345123451234512345123451234512345";
            var location = DataFacade.Location.New().
                With_Level(level).
                With_Code(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_Abbreviation("abrev").
                With_ServicingDepot(_fixture.SiteCIT).
                With_InvoiceReference(reference).
                With_InboundReference(reference).
                With_OutboundReference(reference);

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var locationTemp = DataFacade.Location.Take(x => x.Code == refNumber).Build();
            Assert.True(locationTemp != null, "location was not created");
        }

        [Theory(DisplayName = "LocationList - Location with InvoiceReference with more then 50 symbols cannot be created")]
        [InlineData(LocationLevel.ServicePoint)]
        [InlineData(LocationLevel.VisitAddress)]
        public void VerifyThatLocationWithInvoiceReferenceMoreThenLimitCannotBeCreated(LocationLevel level)
        {
            var reference = "12345123451234512345123451234512345123451234512345";
            var location = DataFacade.Location.New().
                With_Level(level).
                With_Code(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_Abbreviation("abrev").
                With_ServicingDepot(_fixture.SiteCIT).
                With_InvoiceReference(reference + "1");

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());

            var assertMsg = $"Error for Location with Code ‘{refNumber}’: Value of property 'Invoice Reference' is too long: '{reference +"1"}'";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(assertMsg)).Any(), "Message is incorrect");

        }

        [Theory(DisplayName = "LocationList - Location with InboundReference with more then 50 symbols cannot be created")]
        [InlineData(LocationLevel.ServicePoint)]
        [InlineData(LocationLevel.VisitAddress)]
        public void VerifyThatLocationWithInboundReferenceMoreThenLimitCannotBeCreated(LocationLevel level)
        {
            var reference = "12345123451234512345123451234512345123451234512345";
            var location = DataFacade.Location.New().
                With_Level(level).
                With_Code(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_Abbreviation("abrev").
                With_ServicingDepot(_fixture.SiteCIT).
                With_InboundReference(reference + "1");

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());


            var assertMsg = $"Error for Location with Code ‘{refNumber}’: Value of property 'Inbound Reference' is too long: '{reference + "1"}'";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(assertMsg)).Any(), "Message is incorrect");
        }

        [Theory(DisplayName = "LocationList - Location with OutboundReference with more then 50 symbols cannot be created")]
        [InlineData(LocationLevel.ServicePoint)]
        [InlineData(LocationLevel.VisitAddress)]
        public void VerifyThatLocationWithOutboundReferenceMoreThenLimitCannotBeCreated(LocationLevel level)
        {
            var reference = "12345123451234512345123451234512345123451234512345";
            var location = DataFacade.Location.New().
                With_Level(level).
                With_Code(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_Company(_fixture.Customer).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_HandlingType("NOR").
                With_Abbreviation("abrev").
                With_ServicingDepot(_fixture.SiteCIT).
                With_OutboundReference(reference + "1");

            var converted = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { location.Build() });

            var response = HelperFacade.FeedingHelper.SendFeeding(converted.ToString());


            var assertMsg = $"Error for Location with Code ‘{refNumber}’: Value of property 'Outbound Reference' is too long: '{reference + "1"}'";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(assertMsg)).Any(), "Message is incorrect");
        }


        public void Dispose()
        {
            var configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == ConfigurationKeyName.LocationCashPointNumberAndTypeAreMandatory);
            HelperFacade.ConfigurationKeysHelper.Update(configKey, "False");
            configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == ConfigurationKeyName.ServicePointVisitAddressIsMandatory);
            HelperFacade.ConfigurationKeysHelper.Update(configKey, "False");
            feedingContext.ValidatedFeedingLogs.RemoveRange(feedingContext.ValidatedFeedingLogs);
            feedingContext.ValidatedFeedings.RemoveRange(feedingContext.ValidatedFeedings);
            feedingContext.SaveChanges();
            _context.BaseAddresses.RemoveRange(_context.BaseAddresses);
            _context.CitProcessSettingLinks.RemoveRange(_context.CitProcessSettingLinks);
            _context.Locations.RemoveRange(_context.Locations.Where(l => l.Code.StartsWith(refNumber) && l.Level == LocationLevel.ServicePoint));
            _context.Locations.RemoveRange(_context.Locations.Where(l => l.Code.StartsWith(refNumber) && l.Level == LocationLevel.VisitAddress));
            _context.SaveChanges();
            feedingContext.Dispose();
            _context.Dispose();
        }
    }
}
