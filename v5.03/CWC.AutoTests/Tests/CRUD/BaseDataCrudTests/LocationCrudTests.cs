using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.BaseData.Enums;
using Cwc.Security;
using Cwc.Sync;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.Tests.Fixtures;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD.BaseDataCrudTests
{
    [Collection("MyCollection")]
    public class LocationCrudTests : IClassFixture<BaseDataFixture>, IDisposable
    {
        LoginResult login;
        string code, name;
        BaseDataFixture _fixture;

        public LocationCrudTests(BaseDataFixture fixture)
        {
            ConfigurationKeySet.Load();
            SyncConfiguration.LoadExportMappings();
            login = SecurityFacade.LoginService.GetAdministratorLogin();
            code = $"1314{new Random().Next(4000, 9999)}";
            name = "AutoTestManagement";
            _fixture = fixture;
        }

        public void Dispose()
        {
            using (var context = new AutomationTransportDataContext())
            {
                var configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == ConfigurationKeyName.LocationCashPointNumberAndTypeAreMandatory);
                HelperFacade.ConfigurationKeysHelper.Update(configKey, "False");
                configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == ConfigurationKeyName.ServicePointVisitAddressIsMandatory);
                HelperFacade.ConfigurationKeysHelper.Update(configKey, "False");
                context.BaseAddresses.RemoveRange(context.BaseAddresses);
                context.CitProcessSettingLinks.RemoveRange(context.CitProcessSettingLinks);
                context.Locations.RemoveRange(context.Locations.Where(l => l.Code.StartsWith(code) && l.Level == LocationLevel.ServicePoint));
                context.Locations.RemoveRange(context.Locations.Where(l => l.Code.StartsWith(code) && l.Level == LocationLevel.VisitAddress));
                context.Customers.RemoveRange(context.Customers.Where(c => c.ReferenceNumber.StartsWith(code)));
                context.SaveChanges();
            }
        }
        
        [Theory(DisplayName = "Location CRUD - When Location is created successfully")]
        [InlineData(LocationLevel.VisitAddress)]
        [InlineData(LocationLevel.ServicePoint)]
        public void VerifyThatLocationIsCreatedSuccessfully(LocationLevel level)
        {
            var location = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(level).
                SaveToDb().
                Build();

            var locationCreated = DataFacade.Location.Take(l => l.Code == code).Build();

            Assert.True(locationCreated.CompanyID == location.CompanyID, "Location wasn't created");           
            Assert.True(locationCreated.LocationType == location.LocationType, "Location wasn't created");
            Assert.True(locationCreated.Name == location.Name, "Location wasn't created");
            Assert.True(locationCreated.Abbreviation == location.Abbreviation, "Location wasn't created");
            Assert.True(locationCreated.HandlingType == location.HandlingType, "Location wasn't created");
            Assert.True(locationCreated.ServicingDepotID == location.ServicingDepotID, "Location wasn't created");
            Assert.True(locationCreated.Level == location.Level, "Location wasn't created");
        }
        
        [Theory(DisplayName = "Location CRUD - When Location is updated successfully")]
        [InlineData(LocationLevel.ServicePoint)]
        [InlineData(LocationLevel.VisitAddress)]
        public void VerifyThatLocationIsUpdatedSuccessfully(LocationLevel level)
        {
            var location = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(level).
                SaveToDb().
                Build();

            var locationCreated = DataFacade.Location.Take(l => l.Code == code);
                locationCreated.With_CompanyID(_fixture.Customer.ID + 1m).
                    With_Name(name + "1").
                    With_Abbreviation(code + "1").
                    With_HandlingType("ATM").
                    SaveToDb();

            var locationUpdated = DataFacade.Location.Take(l => l.Code == code).Build();
            Assert.False(locationUpdated.CompanyID == location.CompanyID, "Location wasn't updated");
            Assert.False(locationUpdated.Name == location.Name, "Location wasn't updated");
            Assert.False(locationUpdated.Abbreviation == location.Abbreviation, "Location wasn't updated");
            Assert.False(locationUpdated.HandlingType == location.HandlingType, "Location wasn't updated");
        }
        
        [Theory(DisplayName = "Location CRUD - When Location is deleted successfully")]
        [InlineData(LocationLevel.VisitAddress)]
        [InlineData(LocationLevel.ServicePoint)]
        public void VerifyThatLocationIsDeletedSuccessfully(LocationLevel level)
        {
            var location = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Enabled(false).
                With_Level(level).
                SaveToDb();

            DataFacade.Location.DeleteOne(l => l.Code == code);
            using (var context = new BaseDataContext())
            {
                var result = context.Locations.Where(l => l.Code == code).FirstOrDefault();
                Assert.True( result == null, "Location wasn't deleted");
            }
        }
                
        [Theory(DisplayName = "Location CRUD - When Code is not specified Then system shows error message")]
        [InlineData(LocationLevel.VisitAddress)]
        [InlineData(LocationLevel.ServicePoint)]
        public void VerifyThatLocationWithEmptyNumberCannotBeCreated(LocationLevel level)
        {
            var location = DataFacade.Location.New().
                With_Code(String.Empty).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(level);

            var result = BaseDataFacade.LocationService.Save(location);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Value of property 'Code' is not specified.", result.Messages.First());
        }
        
        [Theory(DisplayName = "Location CRUD - When Name is not specified Then system shows error message")]
        [InlineData(LocationLevel.VisitAddress)]
        [InlineData(LocationLevel.ServicePoint)]
        public void VerifyThatLocationWithEmptyNameCannotBeCreated(LocationLevel level)
        {
            var location = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(String.Empty).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(level);

            var result = BaseDataFacade.LocationService.Save(location);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Value of property 'Name' is not specified.", result.Messages.First());
        }

        [Fact(DisplayName = "Location CRUD - When Level = Service Point and CIT Depot is not specified Then system shows error message")]
        public void VerifyThatServicePointWithEmptyCITDepotCannotBeCreated()
        {
            var location = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(code).
                With_HandlingType("NOR");

            var result = BaseDataFacade.LocationService.Save(location);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Default servicing CIT site must be specified for location with handling type 'NOR'.", result.Messages.First());
        }
        
        [Theory(DisplayName = "Location CRUD - When Code is not unique Then system shows error message")]
        [InlineData(LocationLevel.VisitAddress)]
        [InlineData(LocationLevel.ServicePoint)]
        public void VerifyThatLocationWithNonUniqueNumberCannotBeCreated(LocationLevel level)
        {
            var locationFirst = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(level).SaveToDb();

            var locationSecond = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(level);

            var result = BaseDataFacade.LocationService.Save(locationSecond);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Location with given Code already exists", result.Messages.First());
        }

        [Fact(DisplayName = "Location CRUD - When circular references used Then system shows error message")]
        public void VerifyThatLocationWithCircularReferencesCannotBeCreated()
        {
            var locationFirst = DataFacade.Location.New().
                With_Code(code + "1").
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                SaveToDb();

            var locationSecond = DataFacade.Location.New().
                With_Code(code + "2").
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_MainLocationID(locationFirst.Build().ID).
                SaveToDb().
                Build();

            locationFirst.With_MainLocationID(locationSecond.ID);

            var result = BaseDataFacade.LocationService.Save(locationFirst);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal($"Main location ‘{locationSecond.DisplayCaption}’ cannot be used, since it will cause circular references between locations.", result.Messages.First());
            using (var context = new AutomationBaseDataContext())
            {
                Assert.False(context.Customers.Where(c => c.ReferenceNumber == code + "1").Any(), "Location shouldn't be saved after error");
            }
        }
        
        [Fact(DisplayName = "Location CRUD - When Notes Site != Cash Center OR Sub-Type = Coins Then system shows error message")]
        public void VerifyThatServicePointWithNotesSiteCannotBeCreatedWhenNotCashCenterTypeORCoinsSubType()
        {
            var location = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_NotesSiteID(_fixture.SiteCoins.ID);

            var result = BaseDataFacade.LocationService.Save(location);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Notes Site must be configured as cash center with sub-type ‘notes’ or ‘notes and coins’.", result.Messages.First());
        }

        [Fact(DisplayName = "Location CRUD - When Coins Site != Cash Center OR Sub-Type = Notes Then system shows error message")]
        public void VerifyThatServicePointWithCoinsSiteCannotBeCreatedWhenNotCashCenterTypeORNotesSubType()
        {
            var location = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_CoinsSiteID(_fixture.SiteNotes.ID);

            var result = BaseDataFacade.LocationService.Save(location);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Coins Site must be configured as cash center with sub-type ‘coins’ or ‘notes and coins’.", result.Messages.First());
        }

        [Fact(DisplayName = "Location CRUD - When Type of Servicing Depot != CIT Depot Then system shows error message")]
        public void VerifyThatServicePointWithDefaultServicingDepotWhenTypeNotCITDepot()
        {
            var location = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCC.ID);

            var result = BaseDataFacade.LocationService.Save(location);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Default servicing CIT site must be configured as CIT depot.", result.Messages.First());
        }

        [Fact(DisplayName = "Location CRUD - When Type of Coins Servicing Depot != CIT Depot Then system shows error message")]
        public void VerifyThatServicePointWithCoinServicingDepotWhenTypeNotCITDepot()
        {
            var location = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_ServicingDepotCoinsID(_fixture.SiteCC.ID);

            var result = BaseDataFacade.LocationService.Save(location);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Coins servicing CIT site must be configured as CIT depot.", result.Messages.First());
        }
        
        [Fact(DisplayName = "Location CRUD - When Visit Address belongs to different company Then system shows error message")]
        public void VerifyThatServicePointCannotBeSavedWhenVisitAddressBelongsToAnotherCompany()
        {
            var newCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(code + "3").
                With_Name(name).
                With_Abbrev(code + "3").
                SaveToDb().Build();

            var servicePoint = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(newCustomer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Level(LocationLevel.ServicePoint).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_VisitAddressID(_fixture.VisitAddress.IdentityID);

            var result = BaseDataFacade.LocationService.Save(servicePoint);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal($"Visit address company {_fixture.Customer.DisplayCaption} is not the company of current service point ({newCustomer.DisplayCaption}).", result.Messages.First());
        }

        [Fact(DisplayName = "Location CRUD - When Location linked to Visit Address belongs to different company Then system changes the service point company")]
        public void VerifyThatVisitAddressCannotBeSavedWhenLinkedServicePointBelongsToAnotherCompany()
        {
            var newCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(code + "33").
                With_Name(name).
                With_Abbrev(code + "VA").
                SaveToDb().
                Build();

            var visitAddress = DataFacade.Location.New().
                With_Code(code + "VA").
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Level(LocationLevel.VisitAddress).
                With_ServicingDepot(_fixture.SiteCIT).
                With_Abbreviation(code).
                With_Address(DataFacade.Address.New().
                    With_City("Kyiv").
                    With_Street("street")).
                SaveToDb();

            var servicePoint = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Level(LocationLevel.ServicePoint).
                With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_VisitAddressID(visitAddress.Build().IdentityID).
                SaveToDb().
                Build();

            visitAddress.With_CompanyID(newCustomer.ID).SaveToDb();
            var foundLocation = DataFacade.Location.Take(x => x.ID == servicePoint.ID).Build();

            Assert.Equal(foundLocation.CompanyID, newCustomer.ID);            
        }
        
        [Fact(DisplayName = "Location CRUD - When Handling Type is empty and Level = Service Point Then system shows error message")]
        public void VerifyThatServicePointCannotBeSavedWhenHandlingTypeIsEmpty()
        {
            var servicePoint = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).
                With_Level(LocationLevel.ServicePoint).
                With_Abbreviation(code).
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_VisitAddress(_fixture.VisitAddress);

            var result = BaseDataFacade.LocationService.Save(servicePoint);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Value of property 'Handling Type' is not specified.", result.Messages.First());
        }

        [Theory(DisplayName = "Location CRUD - When Location is not new and old Code <> new Code Then system shows error message")]
        [InlineData(LocationLevel.VisitAddress)]
        [InlineData(LocationLevel.ServicePoint)]
        public void VerifyThatLocationWithChangedCodeCannotBeSaved(LocationLevel level)
        {
            var location = DataFacade.Location.New().
                With_Code(code).
                With_CompanyID(_fixture.Customer.ID).
                With_LocationTypeID(_fixture.LocationType.ltCode).
                With_Name(name).With_Abbreviation(code).
                With_HandlingType("NOR").
                With_ServicingDepotID(_fixture.SiteCIT.ID).
                With_Level(level).SaveToDb();

            location.With_Code(code + "1");                

            var result = BaseDataFacade.LocationService.Save(location);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("'Code' cannot be changed for Location, which already exists.", result.Messages.First());
        }
    }
}
