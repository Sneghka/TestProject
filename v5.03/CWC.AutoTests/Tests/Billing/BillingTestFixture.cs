using Cwc.BaseData;
using Cwc.BaseData.Enums;
using Cwc.BaseData.Model;
using Cwc.Contracts;
using Cwc.Security;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace CWC.AutoTests.Tests.Billing
{
    [Xunit.CollectionDefinition("Billing")]
    public class BillingTestFixture
    {
        private const string billingCompanyLinkTableName = "dbo.Cwc_Billing_BillingJobSettingsCompanyLink";
        private const string locationGroupTableName = "dbo.WP_LocationGroup";
        private const string locationGroupLinkTableName = "dbo.WP_LocationGroupLocation";
        private const string containerType1Code =  "JG1LP";
        private const string containerType2Code =  "JG2LP";
        private const string productGroup1Code =   "3303N";  // code of notes product group
        private const string productGroup2Code =   "3303C";  // code of coins product group
        private const string externalLocationCode = "JG02";  // ref_loc_nr of location with external notes and coins sites
        private const string onwardLocation1Code =  "JG50";  // ref_loc_nr of the first onward location
        private const string onwardLocation2Code =  "JG51";  // ref_loc_nr of the second onward location
        private const string collectLocation1Code = "JG60";  // ref_loc_nr of the first collect location
        private const string collectLocation2Code = "JG61";  // ref_loc_nr of the second collect location
        private const string locationGroupCode="JG [3303]";
        private const string referenceNumber =      "3303";
        private const string localNotesSiteCode =     "JG";
        private const string localCoinsSiteCode ="JG Coin";
        private const string externalSiteCode =    "JG NC";
        private const string visitAddress1Code =  "JGVISIT";
        private const string visitAddress2Code ="JGVISIT2";
        private const string visitServicePoint1Code = "JGVSP01";
        private const string visitServicePoint2Code = "JGVSP02";        
        private const string visitServicePoint3Code = "JGVSP03";
        private const string currencyCode = "EUR";
        private const string noteType = "NOTE";
        private const string coinType = "COIN";
        private const string servicingCode1Code = "1";
        private const string servicingCode2Code = "2";
        public const string collectCode = "COLL";
        public const string deliverCode = "DELV";
        public const string replenishmentCode = "REPL";
        public const string servicingCode = "SERV";

        public int CollectServiceTypeID { get; private set; } = DataFacade.ServiceType.Take(s => s.Code == collectCode, asNoTracking: true).Build().ID;
        public int DeliverServiceTypeID { get; private set; } = DataFacade.ServiceType.Take(s => s.Code == deliverCode, asNoTracking: true).Build().ID;
        public int ReplenishmentServiceTypeID { get; private set; } = DataFacade.ServiceType.Take(s => s.Code == replenishmentCode, asNoTracking: true).Build().ID;
        public int ServicingServiceTypeID { get; private set; } = DataFacade.ServiceType.Take(s => s.Code == servicingCode, asNoTracking: true).Build().ID;
        public Contract CompanyContract { get; private set; }
        public Contract DefaultContract { get; private set; }
        public Customer Company { get; private set; } = DataFacade.Customer.Take(c => c.ReferenceNumber == referenceNumber, asNoTracking: true).Build();
        public Location ExternalLocation { get; private set; } // location used for most of tests
        public LocationGroup LocationGroup { get; private set; } // location group is required for transport cases testing
        public Location OnwardLocation1 { get; private set; } // first onward location for ordering cases testing
        public Location OnwardLocation2 { get; private set; } // second onward location for ordering cases testing
        public Location CollectLocation1 { get; private set; } // first collect location for ordering cases testing
        public Location CollectLocation2 { get; private set; } // second collect location for ordering cases testing
        public Location VisitServicePoint1 { get; private set; } // first visit location for transport cases testing
        public Location VisitServicePoint2 { get; private set; } // second visit location for transport cases testing
        public Location VisitServicePoint3 { get; private set; } // third visit location linked to second visit address for transport cases testing
        public Location VisitAddress1 { get; private set; } // visit address for transport cases testing
        public Location VisitAddress2 { get; private set; } // second visit address for transport cases testing        
        public BagType ContainerType1 { get; private set; }  // first container type for transport cases testing
        public BagType ContainerType2 { get; private set; } // second container type for transport cases testing
        public ProductGroup ProductGroup1 { get; private set; }
        public ProductGroup ProductGroup2 { get; private set; }
        public Product Note100EurProduct { get; private set; }
        public Product Note50EurProduct { get; private set; }
        public Product Coin1EurProduct { get; private set; }
        //public Product coin2EurProduct; // not used
        public ServicingCode ServicingCode1 { get; private set; }
        public ServicingCode ServicingCode2 { get; private set; }

        public BillingTestFixture()
        {
            // Disable "mandatory address" setting
            //var configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == ConfigurationKeyName.ServicePointVisitAddressIsMandatory);          
            //HelperFacade.ConfigurationKeysHelper.Update(configKey, "False");
            //configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == ConfigurationKeyName.LocationCashPointNumberAndTypeAreMandatory);            
            //HelperFacade.ConfigurationKeysHelper.Update(configKey, "False");
            var companyID = Company.ID;
            var today = DateTime.Now.Date;
            var servicingDepot = DataFacade.Site.Take(s => s.BranchType == BranchType.CITDepot && !s.WP_IsExternal);
            var orderingDepartmentID = DataFacade.Location.Take(l => l.Code == externalLocationCode).Build().OrderingDepartmentID;
            var localNotesSiteID = DataFacade.Site.Take(s => s.Branch_cd == localNotesSiteCode).Build().Branch_nr;
            var localCoinsSiteID = DataFacade.Site.Take(s => s.Branch_cd == localCoinsSiteCode).Build().Branch_nr;
            var externalSiteID = DataFacade.Site.Take(s => s.Branch_cd == externalSiteCode).Build().Branch_nr;
            var login = SecurityFacade.LoginService.GetAdministratorLogin();

            try
            {
                using (var context = new AutomationContractDataContext())
                {
                    // Find or create company contract                
                    CompanyContract = context.Contracts.AsNoTracking().FirstOrDefault(
                        c => c.CustomerID == companyID
                        && c.Currency_code == "EUR"
                        && c.IsLatestRevision
                        && c.Status == ContractStatus.Final);

                    if (CompanyContract == null)
                    {
                        CompanyContract = DataFacade.Contract.New()
                            .With_IsDefault(false)
                            .With_Number(referenceNumber)                            
                            .With_Currency_code("EUR")
                            .With_Customer_id(companyID)
                            .With_Date(DateTime.Now)
                            .With_EffectiveDate(DateTime.Now)
                            .With_StartDate(DateTime.Now)
                            .With_EndDate(DateTime.Now.AddDays(30))
                            .With_InterestRate(0)
                            .With_CustomerType(CustomerType.Direct)
                            .With_IsLatestRevision(true)
                            .SaveToDb();

                        ContractsFacade.ContractService.ActivateContract(CompanyContract, new UserParams(login));
                    }
                    else
                    {
                        CompanyContract.EndDate = DateTime.Now.AddDays(30);
                        context.SaveChanges();
                    }

                    HelperFacade.TransportHelper.ClearTestData();
                    HelperFacade.BillingHelper.ClearTestData();
                    HelperFacade.ContractHelper.ClearTestData();
                    // Re-configure ordering settings for main service types: COLL, DELV, SERV, REPL, P&P
                    // with clearing of all existing ordering settings of custom contract  
                    HelperFacade.ContractHelper.ReconfigureOrderingSettings(CompanyContract.ID);
                    // Re-configure schedule settings for main service types: COLL, DELV, SERV, REPL 
                    // with clearing of all existing schedule settings and schedule lines of custom contract                    
                    HelperFacade.ContractHelper.ReconfigureScheduleSettings(CompanyContract);
                    // Find default contract               
                    DefaultContract = context.Contracts.FirstOrDefault(c => c.IsDefault && c.IsLatestRevision && c.Status == ContractStatus.Final);
                    // Configure ordering settings for default contract if required
                    // ...
                    // currently, not required

                    var dbParams = context.GetDatabaseParams();

                    ExternalLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == externalLocationCode);
                    if (ExternalLocation == null)
                    {
                        ExternalLocation = new Location
                        {
                            ID = BaseDataFacade.LocationService.GetNewLocationNumber(dbParams),
                            Abbreviation = externalLocationCode,
                            Code = externalLocationCode,
                            Name = externalLocationCode,
                            LtCode = "NOR",
                            HandlingType = "NOR",
                            ServicingDepotID = servicingDepot.Build().ID,
                            CompanyID = companyID,
                            PreferredLanguage = 1,
                            OrderingDepartmentID = orderingDepartmentID,
                            IsInheritClosingPeriodFromAddress = false
                        };
                        ExternalLocation.SetLevel(LocationLevel.ServicePoint);
                        ExternalLocation.SetDateCreated(today);
                        context.Locations.Add(ExternalLocation);
                        context.SaveChanges();
                        HelperFacade.TransportHelper.SaveCitProcessSettingLink(ExternalLocation, dbParams);
                    }

                    // match or create first onward location                    
                    OnwardLocation1 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == onwardLocation1Code);
                    if (OnwardLocation1 == null)
                    {
                        OnwardLocation1 = new Location
                        {
                            ID = BaseDataFacade.LocationService.GetNewLocationNumber(dbParams),
                            Abbreviation = onwardLocation1Code,
                            Code = onwardLocation1Code,
                            Name = "Test Onward Location 1",
                            LtCode = "NOR",
                            HandlingType = "NOR",
                            ServicingDepotID = servicingDepot.Build().ID,
                            CompanyID = companyID,
                            PreferredLanguage = 1,
                            OrderingDepartmentID = orderingDepartmentID,
                            NotesSiteID = externalSiteID,
                            CoinsSiteID = externalSiteID,
                            IsInheritClosingPeriodFromAddress = false
                        };
                        OnwardLocation1.SetLevel(LocationLevel.ServicePoint);
                        OnwardLocation1.SetDateCreated(today);
                        context.Locations.Add(OnwardLocation1);
                        context.SaveChanges();
                        HelperFacade.TransportHelper.SaveCitProcessSettingLink(OnwardLocation1, dbParams);
                    }

                    // match or create second onward location
                    OnwardLocation2 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == onwardLocation2Code);
                    if (OnwardLocation2 == null)
                    {
                        OnwardLocation2 = new Location
                        {
                            ID = BaseDataFacade.LocationService.GetNewLocationNumber(dbParams),
                            Abbreviation = onwardLocation2Code,
                            Code = onwardLocation2Code,
                            Name = "Test Onward Location 2",
                            LtCode = "NOR",
                            HandlingType = "NOR",
                            ServicingDepotID = servicingDepot.Build().ID,
                            CompanyID = companyID,
                            PreferredLanguage = 1,
                            OrderingDepartmentID = orderingDepartmentID,
                            NotesSiteID = externalSiteID,
                            CoinsSiteID = externalSiteID,
                            IsInheritClosingPeriodFromAddress = false
                        };
                        OnwardLocation2.SetLevel(LocationLevel.ServicePoint);
                        OnwardLocation2.SetDateCreated(today);
                        context.Locations.Add(OnwardLocation2);
                        context.SaveChanges();
                        HelperFacade.TransportHelper.SaveCitProcessSettingLink(OnwardLocation2, dbParams);
                    }

                    // match or create first collect location
                    CollectLocation1 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == collectLocation1Code);
                    if (CollectLocation1 == null)
                    {
                        CollectLocation1 = new Location
                        {
                            ID = BaseDataFacade.LocationService.GetNewLocationNumber(dbParams),
                            Abbreviation = collectLocation1Code,
                            Code = collectLocation1Code,
                            Name = "Test Collect Location 1",
                            LtCode = "NOR",
                            HandlingType = "NOR",
                            ServicingDepotID = servicingDepot.Build().ID,
                            CompanyID = companyID,
                            PreferredLanguage = 1,
                            OrderingDepartmentID = orderingDepartmentID,
                            NotesSiteID = externalSiteID,
                            CoinsSiteID = externalSiteID,
                            IsInheritClosingPeriodFromAddress = false
                        };
                        CollectLocation1.SetLevel(LocationLevel.ServicePoint);
                        CollectLocation1.SetDateCreated(today);
                        context.Locations.Add(CollectLocation1);
                        context.SaveChanges();
                        HelperFacade.TransportHelper.SaveCitProcessSettingLink(CollectLocation1, dbParams);
                    }

                    // match or create second collect location
                    CollectLocation2 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == collectLocation2Code);
                    if (CollectLocation2 == null)
                    {
                        CollectLocation2 = new Location
                        {
                            ID = BaseDataFacade.LocationService.GetNewLocationNumber(dbParams),
                            Abbreviation = collectLocation2Code,
                            Code = collectLocation2Code,
                            Name = "Test Collect Location 2",
                            LtCode = "NOR",
                            HandlingType = "NOR",
                            ServicingDepotID = servicingDepot.Build().ID,
                            CompanyID = companyID,
                            PreferredLanguage = 1,
                            OrderingDepartmentID = orderingDepartmentID,
                            NotesSiteID = externalSiteID,
                            CoinsSiteID = externalSiteID,
                            IsInheritClosingPeriodFromAddress = false
                        };
                        CollectLocation2.SetLevel(LocationLevel.ServicePoint);
                        CollectLocation2.SetDateCreated(today);
                        context.Locations.Add(CollectLocation2);
                        context.SaveChanges();
                        HelperFacade.TransportHelper.SaveCitProcessSettingLink(CollectLocation2, dbParams);
                    }

                    // match or create main visit address
                    VisitAddress1 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == visitAddress1Code);
                    if (VisitAddress1 == null)
                    {
                        VisitAddress1 = new Location
                        {
                            ID = BaseDataFacade.LocationService.GetNewLocationNumber(dbParams),
                            Abbreviation = visitAddress1Code,
                            Code = visitAddress1Code,
                            Name = "JG Visit Address",
                            LtCode = "NOR",
                            HandlingType = "NOR",
                            ServicingDepotID = servicingDepot.Build().ID,
                            CompanyID = companyID,
                            PreferredLanguage = 1,
                            OrderingDepartmentID = orderingDepartmentID,
                            NotesSiteID = externalSiteID,
                            CoinsSiteID = externalSiteID,
                            IsInheritClosingPeriodFromAddress = false
                        };
                        VisitAddress1.SetDateCreated(today);
                        VisitAddress1.SetLevel(LocationLevel.VisitAddress);
                        context.Locations.Add(VisitAddress1);
                        context.SaveChanges();
                        HelperFacade.TransportHelper.SaveCitProcessSettingLink(VisitAddress1, dbParams);
                    }

                    // match or create second visit address
                    VisitAddress2 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == visitAddress2Code);
                    if (VisitAddress2 == null)
                    {
                        VisitAddress2 = new Location
                        {
                            ID = BaseDataFacade.LocationService.GetNewLocationNumber(dbParams),
                            Abbreviation = visitAddress2Code,
                            Code = visitAddress2Code,
                            Name = "JG Visit Address 2",
                            LtCode = "NOR",
                            HandlingType = "NOR",
                            ServicingDepotID = servicingDepot.Build().ID,
                            CompanyID = companyID,
                            PreferredLanguage = 1,
                            OrderingDepartmentID = orderingDepartmentID,
                            NotesSiteID = externalSiteID,
                            CoinsSiteID = externalSiteID,
                            IsInheritClosingPeriodFromAddress = false
                        };
                        VisitAddress2.SetDateCreated(today);
                        VisitAddress2.SetLevel(LocationLevel.VisitAddress);
                        context.Locations.Add(VisitAddress2);
                        context.SaveChanges();
                        HelperFacade.TransportHelper.SaveCitProcessSettingLink(VisitAddress2, dbParams);
                    }

                    // match or create first service point location (linked to the main visit address)
                    VisitServicePoint1 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == visitServicePoint1Code);
                    if (VisitServicePoint1 == null)
                    {
                        VisitServicePoint1 = new Location
                        {
                            ID = BaseDataFacade.LocationService.GetNewLocationNumber(dbParams),
                            Abbreviation = visitServicePoint1Code,
                            Code = visitServicePoint1Code,
                            Name = "Visit Service Point 1",
                            LtCode = "NOR",
                            HandlingType = "NOR",
                            ServicingDepotID = servicingDepot.Build().ID,
                            CompanyID = companyID,
                            PreferredLanguage = 1,
                            OrderingDepartmentID = orderingDepartmentID,
                            NotesSiteID = externalSiteID,
                            CoinsSiteID = externalSiteID,
                            VisitAddressID = VisitAddress1.IdentityID,
                            IsInheritClosingPeriodFromAddress = false
                        };
                        VisitServicePoint1.SetLevel(LocationLevel.ServicePoint);
                        VisitServicePoint1.SetDateCreated(today);
                        context.Locations.Add(VisitServicePoint1);
                        context.SaveChanges();
                        HelperFacade.TransportHelper.SaveCitProcessSettingLink(VisitServicePoint1, dbParams);
                    }

                    // match or create second service point location (linked to the main visit address)
                    VisitServicePoint2 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == visitServicePoint2Code);
                    if (VisitServicePoint2 == null)
                    {
                        VisitServicePoint2 = new Location
                        {
                            ID = BaseDataFacade.LocationService.GetNewLocationNumber(dbParams),
                            Abbreviation = visitServicePoint2Code,
                            Code = visitServicePoint2Code,
                            Name = "Visit Service Point 2",
                            LtCode = "NOR",
                            HandlingType = "NOR",
                            ServicingDepotID = servicingDepot.Build().ID,
                            CompanyID = companyID,
                            PreferredLanguage = 1,
                            OrderingDepartmentID = orderingDepartmentID,
                            NotesSiteID = externalSiteID,
                            CoinsSiteID = externalSiteID,
                            VisitAddressID = VisitAddress1.IdentityID,
                            IsInheritClosingPeriodFromAddress = false
                        };
                        VisitServicePoint2.SetLevel(LocationLevel.ServicePoint);
                        VisitServicePoint2.SetDateCreated(today);
                        context.Locations.Add(VisitServicePoint2);
                        context.SaveChanges();
                        HelperFacade.TransportHelper.SaveCitProcessSettingLink(VisitServicePoint2, dbParams);
                    }

                    // match or create third service point location (linked to the second visit address)
                    VisitServicePoint3 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == visitServicePoint3Code);
                    if (VisitServicePoint3 == null)
                    {
                        VisitServicePoint3 = new Location
                        {
                            ID = BaseDataFacade.LocationService.GetNewLocationNumber(dbParams),
                            Abbreviation = visitServicePoint3Code,
                            Code = visitServicePoint3Code,
                            Name = "Visit Service Point 3",
                            LtCode = "NOR",
                            HandlingType = "NOR",
                            ServicingDepotID = servicingDepot.Build().ID,
                            CompanyID = companyID,
                            PreferredLanguage = 1,
                            OrderingDepartmentID = orderingDepartmentID,
                            NotesSiteID = externalSiteID,
                            CoinsSiteID = externalSiteID,
                            VisitAddressID = VisitAddress2.IdentityID,
                            IsInheritClosingPeriodFromAddress = false
                        };
                        VisitServicePoint3.SetLevel(LocationLevel.ServicePoint);
                        VisitServicePoint3.SetDateCreated(today);
                        context.Locations.Add(VisitServicePoint3);
                        context.SaveChanges();
                        HelperFacade.TransportHelper.SaveCitProcessSettingLink(VisitServicePoint3, dbParams);
                    }
                    context.SaveChanges();
                }

                using (var context = new AutomationBaseDataContext())
                {
                    // Find or create container types
                    if (context.BagTypes.Any(b => b.Code == containerType1Code))
                    {
                        ContainerType1 = DataFacade.ContainerType.Take(b => b.Code == containerType1Code).Build();
                    }
                    else
                    {
                        ContainerType1 = DataFacade.ContainerType.New()
                                                                        .With_Number(3330)
                                                                        .With_Description(containerType1Code)
                                                                        .With_Code(containerType1Code)
                                                                        .With_MaxValue(100000)
                                                                        .With_MaxWeight(10)
                                                                        .With_RegularValue(0)
                                                                        .With_RegularWeight(0)
                                                                        .With_MaxProductQuantity(100000)
                                                                        .With_IsAllocationBySize(false)
                                                                        .With_IsAtmCassette(false)
                                                                        .SaveToDb();
                    }

                    if (context.BagTypes.Any(b => b.Code == containerType2Code))
                    {
                        ContainerType2 = DataFacade.ContainerType.Take(b => b.Code == containerType2Code).Build();
                    }
                    else
                    {
                        ContainerType2 = DataFacade.ContainerType.New()
                                                                        .With_Number(3331)
                                                                        .With_Description(containerType2Code)
                                                                        .With_Code(containerType2Code)
                                                                        .With_MaxValue(100000)
                                                                        .With_MaxWeight(10)
                                                                        .With_RegularValue(0)
                                                                        .With_RegularWeight(0)
                                                                        .With_MaxProductQuantity(100000)
                                                                        .With_IsAllocationBySize(false)
                                                                        .With_IsAtmCassette(false)
                                                                        .SaveToDb();
                    }

                    // Find or create product groups, and link it to products
                    if (context.ProductGroups.Any(g => g.Code == productGroup1Code))
                    {
                        ProductGroup1 = DataFacade.ProductGroup.Take(g => g.Code == productGroup1Code);
                    }
                    else
                    {
                        ProductGroup1 = DataFacade.ProductGroup.New()
                                                                    .With_Code(productGroup1Code)
                                                                    .With_Description(productGroup1Code)
                                                                    .SaveToDb();
                    }

                    if (context.ProductGroups.Any(g => g.Code == productGroup2Code))
                    {
                        ProductGroup2 = DataFacade.ProductGroup.Take(g => g.Code == productGroup2Code);
                    }
                    else
                    {
                        ProductGroup2 = DataFacade.ProductGroup.New()
                                                                    .With_Code(productGroup2Code)
                                                                    .With_Description(productGroup2Code)
                                                                    .SaveToDb();
                    }

                    // find products for product groups and link them to product groups
                    Note100EurProduct = DataFacade.Product.Take(p =>
                                                                    p.Value == 100
                                                                    && p.Currency == currencyCode
                                                                    && p.Type == noteType)
                                                                .With_ProductGroupID(ProductGroup1.ID)
                                                                .SaveToDb();

                    Note50EurProduct = DataFacade.Product.Take(p =>
                                                                    p.Value == 50
                                                                    && p.Currency == currencyCode
                                                                    && p.Type == noteType)
                                                                .With_ProductGroupID(ProductGroup1.ID)
                                                                .SaveToDb();

                    Coin1EurProduct = DataFacade.Product.Take(p =>
                                                                    p.Value == 25
                                                                    && p.Denomination == 1
                                                                    && p.Currency == currencyCode
                                                                    && p.Type == coinType)
                                                                .With_ProductGroupID(ProductGroup2.ID)
                                                                .SaveToDb();

                    //coin2EurProduct = DataFacade.Product.Take(p =>
                    //                                                p.Value == 50
                    //                                                && p.Denomination == 2
                    //                                                && p.Currency == currencyCode
                    //                                                && p.Type == coinType)
                    //                                            .With_ProductGroupID(productGroup2.ID)
                    //                                            .SaveToDb();                    

                    // Find or create servicing codes
                    if (context.ServicingCodes.Any(s => s.Code == servicingCode1Code))
                    {
                        ServicingCode1 = DataFacade.ServicingCode.Take(g => g.Code == servicingCode1Code);
                    }
                    else
                    {
                        ServicingCode1 = DataFacade.ServicingCode.New()
                                                                    .With_Code(servicingCode1Code)
                                                                    .With_Description("Primary service")
                                                                    .SaveToDb();
                    }

                    if (context.ServicingCodes.Any(s => s.Code == servicingCode2Code))
                    {
                        ServicingCode2 = DataFacade.ServicingCode.Take(g => g.Code == servicingCode2Code);
                    }
                    else
                    {
                        ServicingCode2 = DataFacade.ServicingCode.New()
                                                                    .With_Code(servicingCode2Code)
                                                                    .With_Description("Secondary service")
                                                                    .SaveToDb();
                    }

                    // Find or create location group and location group links to locations of configured customer
                    if (context.LocationGroups.Any(l => l.Code == locationGroupCode))
                    {
                        LocationGroup = BaseDataFacade.LocationGroupService.LoadLocationGroup(SecurityFacade.LoginService.GetAdministratorLogin(),
                            context.LocationGroups.Where(l => l.Code == locationGroupCode).FirstOrDefault().ID);

                        var locationGroupLocationsList = context.Locations.Where(l => l.CompanyID == companyID).ToList();
                        foreach (var location in locationGroupLocationsList)
                        {
                            if (!context.LocationGroupLocations.Any(l => l.LocationNumber == location.ID))
                            {
                                context.Database.ExecuteSqlCommand($"INSERT { locationGroupLinkTableName } (lc_nr, LocationGroupId) VALUES (@locationID, @locationGroupID);",
                                    new SqlParameter("locationID", location.ID),
                                    new SqlParameter("locationGroupID", LocationGroup.ID));

                                continue;
                            }
                            if (!context.LocationGroupLocations.Any(l => l.LocationNumber == location.ID && l.LocationGroupId == LocationGroup.ID))
                            {
                                context.Database.ExecuteSqlCommand($"UPDATE { locationGroupLinkTableName } SET LocationGroupId = @locationGroupID WHERE lc_nr = @locationID;",
                                    new SqlParameter("locationID", location.ID),
                                    new SqlParameter("locationGroupID", LocationGroup.ID));
                            }
                        }
                    }
                    else
                    {
                        context.Database.ExecuteSqlCommand($"INSERT { locationGroupTableName } (Code, Name) VALUES (@locationGroupCode, @locationGroupCode);",
                            new SqlParameter("locationGroupCode", locationGroupCode));
                        context.SaveChanges();
                        LocationGroup = BaseDataFacade.LocationGroupService.LoadLocationGroup(SecurityFacade.LoginService.GetAdministratorLogin(),
                            context.LocationGroups.Where(l => l.Code == locationGroupCode).FirstOrDefault().ID);

                        var locationGroupLocationsList = context.Locations.Where(l => l.CompanyID == companyID).ToList();
                        foreach (var location in locationGroupLocationsList)
                        {
                            if (!context.LocationGroupLocations.Any(l => l.LocationNumber == location.ID))
                            {
                                context.Database.ExecuteSqlCommand($"INSERT { locationGroupLinkTableName } (lc_nr, LocationGroupId) VALUES (@locationID, @locationGroupID);",
                                    new SqlParameter("locationID", location.ID),
                                    new SqlParameter("locationGroupID", LocationGroup.ID));

                                continue;
                            }
                            if (!context.LocationGroupLocations.Any(l => l.LocationNumber == location.ID && l.LocationGroupId == LocationGroup.ID))
                            {
                                context.Database.ExecuteSqlCommand($"UPDATE { locationGroupLinkTableName } SET LocationGroupId = @locationGroupID WHERE lc_nr = @locationID;",
                                    new SqlParameter("locationID", location.ID),
                                    new SqlParameter("locationGroupID", LocationGroup.ID));
                            }
                        }
                    }

                    // billing job settings -> company link configuration
                    if (context.Database.SqlQuery<int?>($"SELECT id FROM { billingCompanyLinkTableName } WHERE CompanyID = @CompanyID;",
                        new SqlParameter("CompanyID", companyID)).FirstOrDefault() == null)
                    {
                        context.Database.ExecuteSqlCommand($"INSERT { billingCompanyLinkTableName } (BillingJobSettingsID, CompanyID) VALUES (1, @CompanyID);",
                            new SqlParameter("CompanyID", companyID));
                    }
                    // delete all company links except test company link
                    context.Database.ExecuteSqlCommand($"DELETE { billingCompanyLinkTableName } WHERE CompanyID <> @CompanyID;",
                            new SqlParameter("CompanyID", companyID));

                    // delete all not default bank holiday settings
                    context.BankHolidays.RemoveRange(context.BankHolidays.Where(b => !context.BankHolidaySettings.Any(s => s.ID == b.BankingHolidaySettingId && s.IsDefault)));
                    //context.BankHolidaySettings.RemoveRange(context.BankHolidaySettings.Where(s => !s.IsDefault)); blocked by exception "Invalid column 'Location'"
                    context.SaveChanges();

                    HelperFacade.BillingHelper.SetZeroValueBillingFlag(true);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
