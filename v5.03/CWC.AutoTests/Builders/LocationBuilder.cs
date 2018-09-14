using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.BaseData.Enums;
using Cwc.Common;
using Cwc.Security;
using Cwc.Sync;
using CWC.AutoTests.Model;
using CWC.AutoTests.Utils;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class LocationBuilder
    {
        DataBaseParams _dbParams;
        Location entity;
        public CustomerBuilder customerBuilder;

        public LocationBuilder()
        {
            ConfigurationKeySet.Load();
            SyncConfiguration.LoadExportMappings();
            _dbParams = new DataBaseParams();
        }

        public LocationBuilder With_ID(decimal value)
        {
            entity.ID = value;
            return this;
        }

        public LocationBuilder With_IdentityID(int value)
        {
            entity.IdentityID = value;
            return this;
        }

        public LocationBuilder With_DateCreated(DateTime value)
        {
            entity.SetDateCreated(value);
            return this;
        }

        public LocationBuilder With_Code(string value)
        {
            entity.Code = value;
            return this;
        }

        public LocationBuilder With_Name(string value)
        {
            entity.Name = value;
            return this;
        }

        public LocationBuilder With_Abbreviation(string value)
        {
            entity.Abbreviation = value;
            return this;
        }

        public LocationBuilder With_Enabled(bool enabled)
        {
            entity.Enabled = enabled;
            return this;
        }

        public LocationBuilder With_Address(BaseAddressBuilder address)
        {
            entity.BaseAddress = address;
            return this;
        }

        public LocationBuilder With_AddressDetails(string value)
        {
            entity.AddressDetails = value;
            return this;
        }

        public LocationBuilder With_CompanyID(decimal value)
        {
            entity.CompanyID = value;
            return this;
        }

        public LocationBuilder With_Company(Customer value)
        {
            entity.Company = value;
            return this;
        }

        public LocationBuilder With_City(string value)
        {
            entity.City = value;
            return this;
        }

        public LocationBuilder With_PostCode(string value)
        {
            entity.PostCode = value;
            return this;
        }

        public LocationBuilder With_Det_pc(string value)
        {
            entity.Det_pc = value;
            return this;
        }

        public LocationBuilder With_State(string value)
        {
            entity.State = value;
            return this;
        }

        public LocationBuilder With_Country(string value)
        {
            entity.Country = value;
            return this;
        }

        public LocationBuilder With_ContactPerson(string value)
        {
            entity.ContactPerson = value;
            return this;
        }

        public LocationBuilder With_TelephoneNumber(string value)
        {
            entity.TelephoneNumber = value;
            return this;
        }

        public LocationBuilder With_FaxNumber(string value)
        {
            entity.FaxNumber = value;
            return this;
        }

        public LocationBuilder With_Email(string value)
        {
            entity.Email = value;
            return this;
        }

        public LocationBuilder With_AddressStreet(string value)
        {
            entity.AddressStreet = value;
            return this;
        }

        public LocationBuilder With_HandlingType(string value)
        {
            entity.HandlingType = value;
            return this;
        }

        public LocationBuilder With_BranchID(int? value)
        {
            entity.BranchID = value;
            return this;
        }

        public LocationBuilder With_ServicingDepotID(int? value)
        {
            entity.ServicingDepotID = value;
            return this;
        }

        public LocationBuilder With_ServicingDepot(Site value)
        {
            entity.BranchID = value.ID;
            return this;
        }

        public LocationBuilder With_ServicingDepotCoinsID(int? value)
        {
            entity.ServicingDepotCoinsID = value;
            return this;
        }

        public LocationBuilder With_ServicingDepotCoins(Site value)
        {
            entity.ServicingDepotCoins = value;
            return this;
        }

        public LocationBuilder With_LtCode(string value)
        {
            try
            {
                using (var context = new AutomationBaseDataContext())
                {
                    if (!context.LocationTypes.Any(t => t.ltCode == value))
                    {
                        DataFacade.LocationType.New()
                            .With_ltCode(value)
                            .With_ltDesc(value)
                            .SaveToDb();
                    }
                    entity.LtCode = value;
                }
            }
            catch
            {
                throw;
            }
            return this;
        }

        public LocationBuilder With_LocationTypeID(string value)
        {
            entity.LocationTypeID = value;
            return this;
        }

        public LocationBuilder With_LocationType(LocationType value)
        {
            entity.LocationType = value;
            return this;
        }

        public LocationBuilder With_MainLocationID(decimal? value)
        {
            entity.MainLocationID = value;
            return this;
        }

        public LocationBuilder With_MainLocation(Location value)
        {
            entity.ParentLocation = value;
            return this;
        }

        public LocationBuilder With_CashPointNumber(string value)
        {
            entity.CashPointNumber = value;
            return this;
        }

        public LocationBuilder With_CashPointTypeID(int? value)
        {
            entity.CashPointTypeID = value;
            return this;
        }

        public LocationBuilder With_CashPointType(CashPointTypeBuilder value)
        {
            entity.CashPointType = value;
            return this;
        }

        public LocationBuilder With_RepackAllowed(bool? value)
        {
            entity.RepackAllowed = value;
            return this;
        }

        public LocationBuilder With_BagtypeDisabled(bool? value)
        {
            entity.BagtypeDisabled = value;
            return this;
        }

        public LocationBuilder With_IsScannerflag(string value)
        {
            entity.IsScannerflag = value;
            return this;
        }

        public LocationBuilder With_UseVerificationCollect(bool? value)
        {
            entity.UseVerificationCollect = value;
            return this;
        }

        public LocationBuilder With_UseVerificationDelivery(bool? value)
        {
            entity.UseVerificationDelivery = value;
            return this;
        }

        public LocationBuilder With_AlwaysOnRoute(bool? value)
        {
            entity.AlwaysOnRoute = value;
            return this;
        }

        public LocationBuilder With_PrintManifest(bool? value)
        {
            entity.PrintManifest = value;
            return this;
        }

        public LocationBuilder With_OMVSettingsID(int? value)
        {
            entity.OMVSettingsID = value;
            return this;
        }

        public LocationBuilder With_PackageType(string value)
        {
            entity.PackageType = value;
            return this;
        }

        public LocationBuilder With_DetailsInherited(bool value)
        {
            entity.DetailsInherited = value;
            return this;
        }

        public LocationBuilder With_Longitude(decimal? value)
        {
            entity.Longitude = value;
            return this;
        }

        public LocationBuilder With_Latitude(decimal? value)
        {
            entity.Latitude = value;
            return this;
        }

        public LocationBuilder With_InvoiceReference(string value)
        {
            entity.InvoiceReference = value;
            return this;
        }

        public LocationBuilder With_InboundReference(string value)
        {
            entity.InboundReference = value;
            return this;
        }

        public LocationBuilder With_OutboundReference(string value)
        {
            entity.OutboundReference = value;
            return this;
        }

        public LocationBuilder With_PreferredLanguage(int value)
        {

            entity.PreferredLanguage = value;
            return this;
        }

        public LocationBuilder With_BankAccount_Id(int? value)
        {
            entity.BankAccount_Id = value;
            return this;
        }

        public LocationBuilder With_BankAccount(BankAccountBuilder value)
        {
            entity.BankAccount = value;
            return this;
        }

        public LocationBuilder With_NotesSiteID(int? value)
        {
            entity.NotesSiteID = value;
            return this;
        }

        public LocationBuilder With_NotesSite(Site value)
        {
            entity.NotesSite = value;
            return this;
        }

        public LocationBuilder With_CoinsSiteID(int? value)
        {
            entity.CoinsSiteID = value;
            return this;
        }

        public LocationBuilder With_CoinsSite(Site value)
        {
            entity.CoinsSite = value;
            return this;
        }

        public LocationBuilder With_ForeignCurrencySiteID(int? value)
        {
            entity.ForeignCurrencySiteID = value;
            return this;
        }

        public LocationBuilder With_ForeignCurrencySite(Site value)
        {
            entity.ForeignCurrencySite = value;
            return this;
        }

        public LocationBuilder With_ElectronicSignature(bool? value)
        {
            entity.ElectronicSignature = value;
            return this;
        }

        public LocationBuilder With_TypedLocationIDAllowed(bool? value)
        {
            entity.TypedLocationIDAllowed = value;
            return this;
        }

        public LocationBuilder With_CancelVisitAllowed(bool? value)
        {
            entity.CancelVisitAllowed = value;

            return this;
        }

        public LocationBuilder With_PrintVisitReceipt(bool? value)
        {
            entity.PrintVisitReceipt = value;
            return this;
        }

        public LocationBuilder With_IsPrintFromLocationID(bool? value)
        {
            entity.IsPrintFromLocationID = value;
            return this;
        }

        public LocationBuilder With_IsPreAnnounceCollects(bool? value)
        {
            entity.IsPreAnnounceCollects = value;
            return this;
        }

        public LocationBuilder With_CallLocation(bool? value)
        {
            entity.CallLocation = value;
            return this;
        }

        public LocationBuilder With_PreCrediting(PreCreditingType? value)
        {
            entity.PreCrediting = value;
            return this;
        }

        public LocationBuilder With_ExternalCode(string value)
        {
            entity.ExternalCode = value;
            return this;
        }

        public LocationBuilder With_OrderingDepartmentID(int? value)
        {
            entity.OrderingDepartmentID = value;
            return this;
        }

        public LocationBuilder With_OrderingDepartment(Group value)
        {
            entity.OrderingDepartment = value;
            return this;
        }

        public LocationBuilder With_Level(LocationLevel value = LocationLevel.ServicePoint)
        {
            entity.SetLevel(value);
            return this;
        }

        public LocationBuilder With_IsInheritFromVisitAddress(bool value)
        {
            entity.IsInheritClosingPeriodFromAddress = value;
            return this;
        }

        public LocationBuilder With_VisitAddressID(int? value)
        {
            entity.VisitAddressID = value.Value;
            return this;
        }

        public LocationBuilder With_VisitAddress(Location value)
        {
            entity.VisitAddress = value;
            return this;
        }

        /// <summary>
        /// Used when locationid should be return for further assertion
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns>locationid as an out parameter</returns>
        public LocationBuilder New(out decimal locationId)
        {
            entity = new Location();
            entity.ID = BaseDataFacade.LocationService.GetNewLocationNumber(_dbParams);
            locationId = entity.ID;
            this.With_Level();
            return this;
        }

        /// <summary>
        /// Used when locationid should not be returned as an out parameter
        /// </summary>
        /// <returns></returns>
        public LocationBuilder New()
        {
            entity = new Location();
            entity.ID = BaseDataFacade.LocationService.GetNewLocationNumber(_dbParams);
            entity.SetDateCreated(DateTime.Now);
            entity.PreferredLanguage = 1;
            entity.SetLevel(LocationLevel.ServicePoint);
            entity.IsInheritClosingPeriodFromAddress = false;
            return this;
        }

        public static implicit operator Location(LocationBuilder ins)
        {
            return ins.Build();
        }

        public Location Build()
        {
            return entity;
        }

        public LocationBuilder SaveToDb()
        {
            var loginResult = SecurityFacade.LoginService.GetAdministratorLogin();
            UserParams userParams = new UserParams(loginResult);
            var result = BaseDataFacade.LocationService.Save(entity, userParams, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(string.Format("Location saving failed. Reason: {0}", result.GetMessage()));
            }

            return this;
        }

        public LocationBuilder Take(Expression<Func<Location, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                entity = context.Locations.Where(expression).FirstOrDefault();
                if (entity == null)
                {
                    throw new ArgumentNullException("location wasn't found by provided criteria");
                }
            }
            return this;
        }

        public void DeleteMany(Expression<Func<Location, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var locations = context.Locations.Where(expression).Select(x => x.ID).ToArray();
                if (locations.Length == 0)
                {
                    throw new ArgumentNullException("Locations with provided criteria weren't found");
                }

                foreach (var item in locations)
                {
                    var res = BaseDataFacade.CitProcessSettingLinkService.DeleteByLocation(item, _dbParams);
                    if (!res.IsSuccess)
                    {
                        throw new InvalidOperationException($"Cit Process Setting deletion failed. Reason: {res.GetMessage()}");
                    }
                }

                var result = BaseDataFacade.LocationService.Delete(locations);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Locations deletion failed. Reason: {result.GetMessage()}");
                }
            }
        }

        public void DeleteOne(Expression<Func<Location, bool>> expression)
        {
            using (var context = new BaseDataContext())
            {
                var location = context.Locations.Where(expression).Select(x => x.ID).First();
                if (location == 0)
                {
                    throw new ArgumentNullException("Location with provided criteria wasn't found");
                }

                var res = BaseDataFacade.CitProcessSettingLinkService.DeleteByLocation(location, _dbParams);
                if (!res.IsSuccess)
                {
                    throw new InvalidOperationException($"Cit Process Setting deletion failed. Reason: {res.GetMessage()}");
                }

                var result = BaseDataFacade.LocationService.Delete(location);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Location deletion failed. Reason: {result.GetMessage()}");
                }
            }
        }

        public LocationBuilder InitDefault()
        {
            customerBuilder = DataFacade.Customer.InitDefault().SaveToDb();
            this.New().With_LtCode(DataFacade.LocationType.Take(x => x.ltCode == "NOR").Build().ltCode)
                .With_Name(ValueGenerator.GenerateString("SP", 6))
                .With_Code(ValueGenerator.GenerateNumber())
                .With_Abbreviation(ValueGenerator.GenerateString("SP", 6))
                .With_CompanyID(customerBuilder.Build().ID)
                .With_HandlingType("NOR")
                .With_Level(LocationLevel.ServicePoint)
                .With_ServicingDepot(DataFacade.Site.Take(x => x.BranchType == BranchType.CITDepot));

            return this;
        }
    }
}

