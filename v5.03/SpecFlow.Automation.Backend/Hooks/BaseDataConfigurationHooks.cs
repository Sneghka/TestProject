using Cwc.BaseData;
using Cwc.BaseData.Classes;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.ObjectBuilder;
using System.Linq;
using TechTalk.SpecFlow;
using Cwc.BaseData.Enums;
using Cwc.Security;
using System;
using System.Collections.Generic;
using CWC.AutoTests.Model;
using Cwc.CallManagement;

namespace Specflow.Automation.Backend.Hooks
{
    [Binding, Scope(Tag = "basedata-generation-required")]
    public class BaseDataConfigurationHooks
    {
        #region Configuration codes
        // Companies
        private const string NoteCompanyNumber = "5505";
        private const string CoinCompanyNumber = "5506";
        private const string ForeignCompanyNumber = "5507";
        private const string CashCenterOwnerCompanyNumber = "88088";
        private const string CitOwnerCompanyNumber = "99099";
        private const string ForeignStockOwnerCompanyNumber = "997";
        private const string NoteStockOwnerCompanyNumber = "998";
        private const string CoinStockOwnerCompanyNumber = "999";
        // Cash center and CIT locations
        private const string NoteCCLocationCode = "88088NCC";
        private const string CoinCCLocationCode = "88088CCC";
        private const string ForeignCCLocationCode = "88088FCC";
        private const string ExternalCCLocationCode = "88088ECC";
        private const string NoteCitDepotLocationCode = "99099NCIT";
        private const string CoinCitDepotLocationCode = "99099CCIT";
        // Cash center and CIT sites
        private const string NoteCashCenterCode = "NCC";
        private const string CoinCashCenterCode = "CCC";
        private const string ForeignCashCenterCode = "FCC";
        private const string ExternalCashCenterCode = "ECC";
        private const string NoteCitDepotCode = "NCIT";
        private const string CoinCitDepotCode = "CCIT";
        // Ordering department
        private const string OrderingDepartmentName = "Vault 101";
        // Client service points (locations)
        private const string NoteLocation1Code = "5505ATM01";  // first onward note location
        private const string NoteLocation2Code = "5505ATM02";  // second onward note location
        private const string CoinLocation1Code = "5506ATM01";  // first onward coin location
        private const string CoinLocation2Code = "5506ATM02";  // second onward coin location
        private const string ForeignLocationCode = "5507ATM01"; // location with configured foreign currency site only
        private const string NoteCoinLocationCode = "5507ATM02"; // location with configured notes and coins sites
        private const string NoteForeignLocationCode = "5507ATM03"; // location with configured notes and foreign currency sites
        private const string CoinForeignLocationCode = "5507ATM04"; // location with configured coins and foreign currency sites
        private const string NoteCoinForeignLocationCode = "5507ATM05"; // location configured with all sites 
        private const string OutOfServiceLocationCode = "5507ATM99"; // location with all sites not configured
        private const string ExternalLocationCode = "88088ATM01";  // location with external notes, coins and foreign currency sites
        // Materials
        private const string Eur1MaterialCode = "3303001"; // coin default
        private const string Eur2MaterialCode = "3303002"; // coin default        
        private const string Eur10MaterialCode= "3303010"; // note default
        private const string Eur20MaterialCode = "3303020"; // note default
        private const string Eur100MaterialCode = "3303100"; // note default
        private const string Usd1MaterialCode = "3304001"; // coin foreign
        private const string Usd10MaterialCode= "3304010"; // note foreign
        private const string Usd20MaterialCode = "3304020"; // note foreign
        private const string Usd100MaterialCode = "3304100"; // note foreign
        private const string Gbp10MaterialCode= "3305010"; // note domestic
        private const string Gbp100MaterialCode = "3305100"; // note domestic
        // Products
        private const string Eur1ProductCode = "3303001"; // simple coin product
        private const string Eur1ProductDesc = "1 EUR Roll";
        private const string Eur2ProductCode = "3303002"; // simple coin product
        private const string Eur2ProductDesc = "2 EUR Roll";
        private const string Eur1PackProductCode = "33030011"; // complex coin product
        private const string Eur1PackProductDesc = "1 EUR Plak";
        private const string Eur2PackProductCode = "33030021"; // complex coin product
        private const string Eur2PackProductDesc = "2 EUR Plak";
        private const string Eur10ProductCode = "3303010"; // simple note product
        private const string Eur10ProductDesc = "10 EUR Bundle";
        private const string Eur100ProductCode = "3303100"; // simple note product
        private const string Eur100ProductDesc = "100 EUR Bundle";
        private const string Usd1ProductCode = "3304001"; // simple foreign coin product
        private const string Usd1ProductDesc = "1 USD Roll";
        private const string Usd10ProductCode = "3304010"; // simple foreign note product
        private const string Usd10ProductDesc = "10 USD Bundle";
        private const string Usd100ProductCode = "3304100"; // simple foreign note product
        private const string Usd100ProductDesc = "100 USD Bundle";
        private const string Gbp10ProductCode = "3305010"; // simple domestic note product
        private const string Gbp10ProductDesc = "10 GBP Bundle";
        private const string Gbp100ProductCode = "3305100"; // simple domestic note product
        private const string Gbp100ProductDesc = "100 GBP Bundle";
        private const string Eur1BarcodedProductCode = "33030012"; // barcoded coin product
        private const string Eur1BarcodedProductDesc = "1 EUR Barcoded Bundle";
        private const string Eur10BarcodedProductCode = "33030102"; // barcoded note product
        private const string Eur10BarcodedProductDesc = "10 EUR Barcoded Bundle";
        private const string Eur100BarcodedProductCode = "33031002"; // barcoded note product
        private const string Eur100BarcodedProductDesc = "100 EUR Barcoded Bundle";
        private const string Usd1BarcodedProductCode = "33040012"; // barcoded foreign coin product
        private const string Usd1BarcodedProductDesc = "1 USD Barcoded Bundle";
        private const string Usd10BarcodedProductCode = "33040102"; // barcoded foreign note product
        private const string Usd10BarcodedProductDesc = "10 USD Barcoded Bundle";
        private const string Eur1LooseProductCode = "33030013"; // loose coin product
        private const string Eur1LooseProductDesc = "1 EUR Loose Product";
        private const string Eur20LooseProductCode = "33030203"; // loose note product
        private const string Eur20LooseProductDesc = "20 EUR Loose Product";
        private const string Usd1LooseProductCode = "33040013"; // loose foreign coin product
        private const string Usd1LooseProductDesc = "1 USD Loose Product";
        private const string Usd20LooseProductCode = "33040203"; // loose foreign note product
        private const string Usd20LooseProductDesc = "20 USD Loose Product";
        // Bag types
        private const string SimpleNotesProductsBagTypeCode = "SNPB01"; // bag type for notes products consisting of materials only
        private const int SimpleNotesProductsBagTypeNumber = 330301;
        private const string ComplexNotesProductsBagTypeCode = "CNPB01"; // bag type for notes products consisting of products only
        private const int ComplexNotesProductsBagTypeNumber = 330302;
        private const string SimpleCoinsProductsBagTypeCode = "SCPB01"; // bag type for coins products consisting of materials only
        private const int SimpleCoinsProductsBagTypeNumber = 330303;
        private const string ComplexCoinsProductsBagTypeCode = "CCPB01"; // bag type for coins products consisting of products only
        private const int ComplexCoinsProductsBagTypeNumber = 330304;
        private const string BarcodedNotesProductsBagTypeCode = "BPB01"; // bag type for barcoded notes products
        private const int BarcodedNotesProductsBagTypeNumber = 330305;
        private const string BarcodedCoinsProductsBagTypeCode = "BPB02"; // bag type for barcoded coins products
        private const int BarcodedCoinsProductsBagTypeNumber = 330306;
        private const string ForeignCurrencyNotesProductsBagTypeCode = "FCPB01"; // bag type for foreign currency notes products 
        private const int ForeignCurrencyNotesProductsBagTypeNumber = 330307;
        private const string ForeignCurrencyCoinsProductsBagTypeCode = "FCPB02"; // bag type for foreign currency coins products 
        private const int ForeignCurrencyCoinsProductsBagTypeNumber = 330308;
        private const string LooseProductsBagTypeCode = "LPB01"; // bag type for loose notes products
        private const int LooseProductsBagTypeNumber = 330309;        
        private const string SizeAllocationBagTypeCode = "SAB01"; // bag type used for the allocation of products by size
        private const int SizeAllocationBagTypeNumber = 330310;
        private const string AtmCassetteBagTypeCode = "ACB01"; // bag type used for allocation of ATM cassettes
        private const int AtmCassetteBagTypeNumber = 330311;
        // Service types
        public const string DeliverCode = "DELV";
        public const string CollectCode = "COLL";
        public const string ReplenishmentCode = "REPL";
        public const string ServicingCode = "SERV";
        // Requestor
        public const string RequestorName = "kyrychek";
        // may be of use later
        //private const string visitAddressCode = "JGVISIT";
        //private const string visitAddress2Code = "JGVISIT2";
        //private const string visitServicePoint1Code = "JGSP01";
        //private const string visitServicePoint2Code = "JGSP02";
        //private const string visitServicePoint3Code = "JGSP03";
        //private const string containerType1Code = "JG1LP";
        //private const string containerType2Code = "JG2LP";
        //private const string productGroup1Code = "5505N";  // code of notes product group
        //private const string productGroup2Code = "5506C";  // code of coins product group
        //private const string locationGroupCode = "JG [3303]";
        //private const string currencyCode = "EUR";
        //private const string noteType = "NOTE";
        //private const string coinType = "COIN";
        //private const string servicingCode1Code = "1";
        //private const string servicingCode2Code = "2";        
        #endregion

        #region Configuration entities
        private static DateTime today = DateTime.Today;
        private static LoginResult login = SecurityFacade.LoginService.GetAdministratorLogin();             
        private static Customer citOwnerCompany;        
        private static Location noteCCLocation;
        private static Location coinCCLocation;
        private static Location foreignCCLocation;
        private static Location externalCCLocation;
        private static Location noteCitDepotLocation;
        private static Location coinCitDepotLocation;        

        public static bool IsBaseDataConfigured { get; private set; }
        public static Group OrderingDepartment { get; private set; }
        public static BagType SimpleNotesProductsBagType { get; private set; }
        public static BagType ComplexNotesProductsBagType { get; private set; }
        public static BagType SimpleCoinsProductsBagType { get; private set; }
        public static BagType ComplexCoinsProductsBagType { get; private set; }
        public static BagType BarcodedNotesProductsBagType { get; private set; }
        public static BagType BarcodedCoinsProductsBagType { get; private set; }
        public static BagType ForeignCurrencyNotesProductsBagType { get; private set; }
        public static BagType ForeignCurrencyCoinsProductsBagType { get; private set; }
        public static BagType LooseProductsBagType { get; private set; }        
        public static BagType SizeAllocationBagType { get; private set; }
        public static BagType AtmCassetteBagType { get; private set; }
        public static Customer NoteCompany { get; private set; }
        public static Customer CoinCompany { get; private set; }
        public static Customer ForeignCompany { get; private set; }        
        public static Customer CashCenterOwnerCompany { get; private set; }
        public static Customer NoteStockOwnerCompany { get; private set; }
        public static Customer CoinStockOwnerCompany { get; private set; }
        public static Customer ForeignStockOwnerCompany { get; private set; }
        public static Location NoteLocation1 { get; private set; }
        public static Location NoteLocation2 { get; private set; }
        public static Location CoinLocation1 { get; private set; }
        public static Location CoinLocation2 { get; private set; }
        public static Location ForeignLocation { get; private set; }
        public static Location NoteCoinLocation { get; private set; }
        public static Location NoteForeignLocation { get; private set; }
        public static Location CoinForeignLocation { get; private set; }
        public static Location NoteCoinForeignLocation { get; private set; }
        public static Location OutOfServiceLocation { get; private set; }
        public static Location ExternalLocation { get; private set; }        
        public static Material Eur1Material  { get; private set; }
        public static Material Eur2Material { get; private set; }
        public static Material Eur10Material { get; private set; }
        public static Material Eur20Material { get; private set; }
        public static Material Eur100Material { get; private set; }
        public static Material Usd1Material { get; private set; }
        public static Material Usd10Material { get; private set; }
        public static Material Usd20Material { get; private set; }
        public static Material Usd100Material { get; private set; }
        public static Material Gbp10Material { get; private set; }
        public static Material Gbp100Material { get; private set; }
        public static Product Eur1Product     { get; private set; }
        public static Product Eur2Product     { get; private set; }
        public static Product Eur1PackProduct { get; private set; }
        public static Product Eur2PackProduct { get; private set; }
        public static Product Eur10Product    { get; private set; }        
        public static Product Eur100Product   { get; private set; }
        public static Product Usd1Product { get; private set; }
        public static Product Usd10Product    { get; private set; }        
        public static Product Usd100Product   { get; private set; }
        public static Product Gbp10Product    { get; private set; }
        public static Product Gbp100Product { get; private set; }
        public static Product Eur1BarcodedProduct { get; private set; }
        public static Product Eur10BarcodedProduct { get; private set; }
        public static Product Eur100BarcodedProduct { get; private set; }
        public static Product Usd1BarcodedProduct { get; private set; }
        public static Product Usd10BarcodedProduct { get; private set; }
        public static Product Eur1LooseProduct { get; private set; }
        public static Product Eur20LooseProduct { get; private set; }
        public static Product Usd1LooseProduct { get; private set; }
        public static Product Usd20LooseProduct { get; private set; }
        public static Site NoteCashCenter { get; private set; }
        public static Site CoinCashCenter { get; private set; }
        public static Site ForeignCashCenter { get; private set; }
        public static Site ExternalCashCenter { get; private set; }
        public static Site NoteCitDepot { get; private set; }
        public static Site CoinCitDepot { get; private set; }
        public static Requestor CallRequestor { get; private set; }

        public static Dictionary<string, Customer> CustomerDict { get; private set; } = new Dictionary<string, Customer>(); // dictionary to select customer without db query
        public static Dictionary<string, Location> LocationDict { get; private set; } = new Dictionary<string, Location>(); // dictionary to select location without db query
        public static Dictionary<string, Site> SiteDict { get; private set; } = new Dictionary<string, Site>(); // dictionary to select site without db query
        public static Dictionary<string, Product> ProductDict { get; private set; } = new Dictionary<string, Product>(); // dictionary to select product without db query
        public static Dictionary<string, Requestor> RequestorDict { get; private set; } = new Dictionary<string, Requestor>(); // dictionary to select requestor without db query
        #endregion

        [BeforeFeature(Order = 1)]
        public static void Init()
        {
            ConfigurationKeySet.Load();
            Cwc.Sync.SyncConfiguration.LoadExportMappings();            
        }

        [BeforeFeature(Order = 2)]
        public static void ConfigureCompanies()
        {
            using (var context = new AutomationBaseDataContext())
            {
                try
                {
                    NoteCompany = context.Customers.AsNoTracking().FirstOrDefault(c => c.ReferenceNumber == NoteCompanyNumber);
                    if (NoteCompany == null)
                    {
                        NoteCompany = DataFacade.Customer.New()
                            .With_Abbrev(NoteCompanyNumber)
                            .With_Code(NoteCompanyNumber)
                            .With_Enabled(true)
                            .With_Name(NoteCompanyNumber)
                            .With_PreferredLanguage(1)
                            .With_RecordType(CustomerRecordType.Company)
                            .With_ReferenceNumber(NoteCompanyNumber)
                            .SaveToDb();
                    }                    

                    CoinCompany = context.Customers.AsNoTracking().FirstOrDefault(c => c.ReferenceNumber == CoinCompanyNumber);
                    if (CoinCompany == null)
                    {
                        CoinCompany = DataFacade.Customer.New()
                            .With_Abbrev(CoinCompanyNumber)
                            .With_Code(CoinCompanyNumber)
                            .With_Enabled(true)
                            .With_Name(CoinCompanyNumber)
                            .With_PreferredLanguage(1)
                            .With_RecordType(CustomerRecordType.Company)
                            .With_ReferenceNumber(CoinCompanyNumber)
                            .SaveToDb();
                    }                    

                    ForeignCompany = context.Customers.AsNoTracking().FirstOrDefault(c => c.ReferenceNumber == ForeignCompanyNumber);
                    if (ForeignCompany == null)
                    {
                        ForeignCompany = DataFacade.Customer.New()
                            .With_Abbrev(ForeignCompanyNumber)
                            .With_Code(ForeignCompanyNumber)
                            .With_Enabled(true)
                            .With_Name(ForeignCompanyNumber)
                            .With_PreferredLanguage(1)
                            .With_RecordType(CustomerRecordType.Company)
                            .With_ReferenceNumber(ForeignCompanyNumber)
                            .SaveToDb();
                    }                    

                    CashCenterOwnerCompany = context.Customers.AsNoTracking().FirstOrDefault(c => c.ReferenceNumber == CashCenterOwnerCompanyNumber);
                    if (CashCenterOwnerCompany == null)
                    {
                        CashCenterOwnerCompany = DataFacade.Customer.New()
                            .With_Abbrev(CashCenterOwnerCompanyNumber)
                            .With_Code(CashCenterOwnerCompanyNumber)
                            .With_Enabled(true)
                            .With_Name(CashCenterOwnerCompanyNumber)
                            .With_PreferredLanguage(1)
                            .With_RecordType(CustomerRecordType.Company)
                            .With_ReferenceNumber(CashCenterOwnerCompanyNumber)
                            .SaveToDb();
                    }                    

                    citOwnerCompany = context.Customers.AsNoTracking().FirstOrDefault(c => c.ReferenceNumber == CitOwnerCompanyNumber);
                    if (citOwnerCompany == null)
                    {
                        citOwnerCompany = DataFacade.Customer.New()
                            .With_Abbrev(CitOwnerCompanyNumber)
                            .With_Code(CitOwnerCompanyNumber)
                            .With_Enabled(true)
                            .With_Name(CitOwnerCompanyNumber)
                            .With_PreferredLanguage(1)
                            .With_RecordType(CustomerRecordType.Company)
                            .With_ReferenceNumber(CitOwnerCompanyNumber)
                            .SaveToDb();
                    }
                    // currently, the property is static
                    // CompanyDict.Add(CitOwnerCompanyNumber, citOwnerCompany);

                    NoteStockOwnerCompany = context.Customers.AsNoTracking().FirstOrDefault(c => c.ReferenceNumber == NoteStockOwnerCompanyNumber);
                    if (NoteStockOwnerCompany == null)
                    {
                        NoteStockOwnerCompany = DataFacade.Customer.New()
                            .With_Abbrev(NoteStockOwnerCompanyNumber)
                            .With_Code(NoteStockOwnerCompanyNumber)
                            .With_Enabled(true)
                            .With_Name(NoteStockOwnerCompanyNumber)
                            .With_PreferredLanguage(1)
                            .With_RecordType(CustomerRecordType.Company)
                            .With_ReferenceNumber(NoteStockOwnerCompanyNumber)
                            .SaveToDb();
                    }
                    
                    CoinStockOwnerCompany = context.Customers.AsNoTracking().FirstOrDefault(c => c.ReferenceNumber == CoinStockOwnerCompanyNumber);
                    if (CoinStockOwnerCompany == null)
                    {
                        CoinStockOwnerCompany = DataFacade.Customer.New()
                            .With_Abbrev(CoinStockOwnerCompanyNumber)
                            .With_Code(CoinStockOwnerCompanyNumber)
                            .With_Enabled(true)
                            .With_Name(CoinStockOwnerCompanyNumber)
                            .With_PreferredLanguage(1)
                            .With_RecordType(CustomerRecordType.Company)
                            .With_ReferenceNumber(CoinStockOwnerCompanyNumber)
                            .SaveToDb();
                    }                    

                    ForeignStockOwnerCompany = context.Customers.AsNoTracking().FirstOrDefault(c => c.ReferenceNumber == ForeignStockOwnerCompanyNumber);
                    if (ForeignStockOwnerCompany == null)
                    {
                        ForeignStockOwnerCompany = DataFacade.Customer.New()
                            .With_Abbrev(ForeignStockOwnerCompanyNumber)
                            .With_Code(ForeignStockOwnerCompanyNumber)
                            .With_Enabled(true)
                            .With_Name(ForeignStockOwnerCompanyNumber)
                            .With_PreferredLanguage(1)
                            .With_RecordType(CustomerRecordType.Company)
                            .With_ReferenceNumber(ForeignStockOwnerCompanyNumber)
                            .SaveToDb();
                    }

                    CustomerDict.Add(NoteCompanyNumber, NoteCompany);
                    CustomerDict.Add(CoinCompanyNumber, CoinCompany);
                    CustomerDict.Add(ForeignCompanyNumber, ForeignCompany);
                    CustomerDict.Add(CashCenterOwnerCompanyNumber, CashCenterOwnerCompany);
                    CustomerDict.Add(NoteStockOwnerCompanyNumber, NoteStockOwnerCompany);
                    CustomerDict.Add(CoinStockOwnerCompanyNumber, CoinStockOwnerCompany);
                    CustomerDict.Add(ForeignStockOwnerCompanyNumber, ForeignStockOwnerCompany);
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 3)]
        public static void ConfigureCashCenterAndCitDepotLocations()
        {
            using (var context = new AutomationBaseDataContext())
            {
                try
                {
                    noteCCLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == NoteCCLocationCode);
                    if (noteCCLocation == null)
                    {
                        noteCCLocation = DataFacade.Location.New()
                            .With_Abbreviation(NoteCCLocationCode)
                            .With_Code(NoteCCLocationCode)
                            .With_Name(NoteCCLocationCode)
                            .With_LtCode("CAS")
                            .With_HandlingType("CAS")
                            .With_CompanyID(CashCenterOwnerCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }

                    coinCCLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == CoinCCLocationCode);
                    if (coinCCLocation == null)
                    {
                        coinCCLocation = DataFacade.Location.New()
                            .With_Abbreviation(CoinCCLocationCode)
                            .With_Code(CoinCCLocationCode)
                            .With_Name(CoinCCLocationCode)
                            .With_LtCode("CAS")
                            .With_HandlingType("CAS")
                            .With_CompanyID(CashCenterOwnerCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }

                    foreignCCLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == ForeignCCLocationCode);
                    if (foreignCCLocation == null)
                    {
                        foreignCCLocation = DataFacade.Location.New()
                            .With_Abbreviation(ForeignCCLocationCode)
                            .With_Code(ForeignCCLocationCode)
                            .With_Name(ForeignCCLocationCode)
                            .With_LtCode("CAS")
                            .With_HandlingType("CAS")
                            .With_CompanyID(CashCenterOwnerCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }

                    externalCCLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == ExternalCCLocationCode);
                    if (externalCCLocation == null)
                    {
                        externalCCLocation = DataFacade.Location.New()
                            .With_Abbreviation(ExternalCCLocationCode)
                            .With_Code(ExternalCCLocationCode)
                            .With_Name(ExternalCCLocationCode)
                            .With_LtCode("CAS")
                            .With_HandlingType("CAS")
                            .With_CompanyID(CashCenterOwnerCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }

                    noteCitDepotLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == NoteCitDepotLocationCode);
                    if (noteCitDepotLocation == null)
                    {
                        noteCitDepotLocation = DataFacade.Location.New()
                            .With_Abbreviation(NoteCitDepotLocationCode)
                            .With_Code(NoteCitDepotLocationCode)
                            .With_Name(NoteCitDepotLocationCode)
                            .With_LtCode("DEP")
                            .With_HandlingType("DEP")
                            .With_CompanyID(citOwnerCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }

                    coinCitDepotLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == CoinCitDepotLocationCode);
                    if (coinCitDepotLocation == null)
                    {
                        coinCitDepotLocation = DataFacade.Location.New()
                            .With_Abbreviation(CoinCitDepotLocationCode)
                            .With_Code(CoinCitDepotLocationCode)
                            .With_Name(CoinCitDepotLocationCode)
                            .With_LtCode("DEP")
                            .With_HandlingType("DEP")
                            .With_CompanyID(citOwnerCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }
                }
                catch
                {
                    throw;
                }
            }            
        }

        [BeforeFeature(Order = 4)]
        public static void ConfigureSites()
        {
            using (var context = new AutomationBaseDataContext())
            {
                try
                {
                    NoteCashCenter = context.Sites.AsNoTracking().FirstOrDefault(s => s.Branch_cd == NoteCashCenterCode);
                    if (NoteCashCenter == null)
                    {
                        NoteCashCenter = DataFacade.Site.New()
                            .With_BranchType(BranchType.CashCenter)
                            .With_Branch_cd(NoteCashCenterCode)
                            .With_Description(NoteCashCenterCode)
                            .With_Location(noteCCLocation)
                            .With_SubType(BranchSubType.Notes)
                            .With_WP_IsExternal(false)
                            .SaveToDb();
                    }

                    CoinCashCenter = context.Sites.AsNoTracking().FirstOrDefault(s => s.Branch_cd == CoinCashCenterCode);
                    if (CoinCashCenter == null)
                    {
                        CoinCashCenter = DataFacade.Site.New()
                            .With_BranchType(BranchType.CashCenter)
                            .With_Branch_cd(CoinCashCenterCode)
                            .With_Description(CoinCashCenterCode)
                            .With_Location(coinCCLocation)
                            .With_SubType(BranchSubType.Coins)
                            .With_WP_IsExternal(false)
                            .SaveToDb();
                    }

                    ForeignCashCenter = context.Sites.AsNoTracking().FirstOrDefault(s => s.Branch_cd == ForeignCashCenterCode);
                    if (ForeignCashCenter == null)
                    {
                        ForeignCashCenter = DataFacade.Site.New()
                            .With_BranchType(BranchType.CashCenter)
                            .With_Branch_cd(ForeignCashCenterCode)
                            .With_Description(ForeignCashCenterCode)
                            .With_Location(foreignCCLocation)
                            .With_SubType(BranchSubType.NotesAndCoins)
                            .With_WP_IsExternal(false)
                            .SaveToDb();
                    }

                    ExternalCashCenter = context.Sites.AsNoTracking().FirstOrDefault(s => s.Branch_cd == ExternalCashCenterCode);
                    if (ExternalCashCenter == null)
                    {
                        ExternalCashCenter = DataFacade.Site.New()
                            .With_BranchType(BranchType.CashCenter)
                            .With_Branch_cd(ExternalCashCenterCode)
                            .With_Description(ExternalCashCenterCode)
                            .With_Location(externalCCLocation)
                            .With_SubType(BranchSubType.NotesAndCoins)
                            .With_WP_IsExternal(true)
                            .SaveToDb();
                    }

                    NoteCitDepot = context.Sites.AsNoTracking().FirstOrDefault(s => s.Branch_cd == NoteCitDepotCode);
                    if (NoteCitDepot == null)
                    {
                        NoteCitDepot = DataFacade.Site.New()
                            .With_BranchType(BranchType.CITDepot)
                            .With_Branch_cd(NoteCitDepotCode)
                            .With_Description(NoteCitDepotCode)
                            .With_Location(noteCitDepotLocation)
                            .With_SubType(BranchSubType.Notes)
                            .With_WP_IsExternal(false)
                            .SaveToDb();
                    }

                    CoinCitDepot = context.Sites.AsNoTracking().FirstOrDefault(s => s.Branch_cd == CoinCitDepotCode);
                    if (CoinCitDepot == null)
                    {
                        CoinCitDepot = DataFacade.Site.New()
                            .With_BranchType(BranchType.CITDepot)
                            .With_Branch_cd(CoinCitDepotCode)
                            .With_Description(CoinCitDepotCode)
                            .With_Location(coinCitDepotLocation)
                            .With_SubType(BranchSubType.Coins)
                            .With_WP_IsExternal(false)
                            .SaveToDb();
                    }

                    SiteDict.Add(NoteCashCenterCode, NoteCashCenter);
                    SiteDict.Add(CoinCashCenterCode, CoinCashCenter);
                    SiteDict.Add(ForeignCashCenterCode, ForeignCashCenter);
                    SiteDict.Add(ExternalCashCenterCode, ExternalCashCenter);
                    SiteDict.Add(NoteCitDepotCode, NoteCitDepot);
                    SiteDict.Add(CoinCitDepotCode, CoinCitDepot);
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 5)]
        public static void ConfigureOrderingDepartment()
        {
            using (var context = new AutomationSecurityDataContext())
            {
                try
                {
                    OrderingDepartment = context.Groups.AsNoTracking().FirstOrDefault(g => g.Name == OrderingDepartmentName);
                    if (OrderingDepartment == null)
                    {
                        OrderingDepartment = DataFacade.Group.New()
                            .With_Name(OrderingDepartmentName)
                            .With_Type(GroupType.Department)
                            .With_DepartmentType(DepartmentType.Ordering)
                            .With_Status(GroupStatus.Active)
                            .SaveToDb();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 6)]       
        public static void ConfigureLocations()
        {
            // Disable "mandatory address" setting
            var configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == ConfigurationKeyName.ServicePointVisitAddressIsMandatory);
            HelperFacade.ConfigurationKeysHelper.Update(configKey, "False");
            configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == ConfigurationKeyName.LocationCashPointNumberAndTypeAreMandatory);
            HelperFacade.ConfigurationKeysHelper.Update(configKey, "False");

            using (var context = new AutomationBaseDataContext())
            {
                try
                {
                    NoteLocation1 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == NoteLocation1Code);
                    if (NoteLocation1 == null)
                    {
                        NoteLocation1 = DataFacade.Location.New()
                            .With_DateCreated(today)
                            .With_Abbreviation(NoteLocation1Code)
                            .With_Code(NoteLocation1Code)
                            .With_Name(NoteLocation1Code)
                            .With_LtCode("ATM")
                            .With_HandlingType("ATM")
                            .With_ServicingDepotID(NoteCitDepot.ID)
                            .With_CompanyID(NoteCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(OrderingDepartment.ID)
                            .With_NotesSiteID(NoteCashCenter.ID)                            
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }                    

                    NoteLocation2 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == NoteLocation2Code);
                    if (NoteLocation2 == null)
                    {
                        NoteLocation2 = DataFacade.Location.New()
                            .With_DateCreated(today)
                            .With_Abbreviation(NoteLocation2Code)
                            .With_Code(NoteLocation2Code)
                            .With_Name(NoteLocation2Code)
                            .With_LtCode("ATM")
                            .With_HandlingType("ATM")
                            .With_ServicingDepotID(NoteCitDepot.ID)
                            .With_CompanyID(NoteCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(OrderingDepartment.ID)
                            .With_NotesSiteID(NoteCashCenter.ID)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }
                    
                    CoinLocation1 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == CoinLocation1Code);
                    if (CoinLocation1 == null)
                    {
                        CoinLocation1 = DataFacade.Location.New()
                            .With_DateCreated(today)
                            .With_Abbreviation(CoinLocation1Code)
                            .With_Code(CoinLocation1Code)
                            .With_Name(CoinLocation1Code)
                            .With_LtCode("ATM")
                            .With_HandlingType("ATM")
                            .With_ServicingDepotID(CoinCitDepot.ID)
                            .With_ServicingDepotCoinsID(CoinCitDepot.ID)
                            .With_CompanyID(CoinCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(OrderingDepartment.ID)                            
                            .With_CoinsSiteID(CoinCashCenter.ID)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }
                    
                    CoinLocation2 = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == CoinLocation2Code);
                    if (CoinLocation2 == null)
                    {
                        CoinLocation2 = DataFacade.Location.New()
                            .With_DateCreated(today)
                            .With_Abbreviation(CoinLocation2Code)
                            .With_Code(CoinLocation2Code)
                            .With_Name(CoinLocation2Code)
                            .With_LtCode("ATM")
                            .With_HandlingType("ATM")
                            .With_ServicingDepotID(CoinCitDepot.ID)
                            .With_ServicingDepotCoinsID(CoinCitDepot.ID)
                            .With_CompanyID(CoinCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(OrderingDepartment.ID)
                            .With_CoinsSiteID(CoinCashCenter.ID)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }

                    ForeignLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == ForeignLocationCode);
                    if (ForeignLocation == null)
                    {
                        ForeignLocation = DataFacade.Location.New()
                            .With_DateCreated(today)
                            .With_Abbreviation(ForeignLocationCode)
                            .With_Code(ForeignLocationCode)
                            .With_Name(ForeignLocationCode)
                            .With_LtCode("ATM")
                            .With_HandlingType("ATM")
                            .With_ServicingDepotID(NoteCitDepot.ID)                            
                            .With_CompanyID(ForeignCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(OrderingDepartment.ID)                            
                            .With_ForeignCurrencySiteID(ForeignCashCenter.IdentityID)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }
                    
                    NoteCoinLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == NoteCoinLocationCode);
                    if (NoteCoinLocation == null)
                    {
                        NoteCoinLocation = DataFacade.Location.New()
                            .With_DateCreated(today)
                            .With_Abbreviation(NoteCoinLocationCode)
                            .With_Code(NoteCoinLocationCode)
                            .With_Name(NoteCoinLocationCode)
                            .With_LtCode("ATM")
                            .With_HandlingType("ATM")
                            .With_ServicingDepotID(NoteCitDepot.ID)
                            .With_ServicingDepotCoinsID(CoinCitDepot.ID)
                            .With_CompanyID(ForeignCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(OrderingDepartment.ID)
                            .With_NotesSiteID(NoteCashCenter.ID)
                            .With_CoinsSiteID(CoinCashCenter.ID)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }
                    
                    NoteForeignLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == NoteForeignLocationCode);
                    if (NoteForeignLocation == null)
                    {
                        NoteForeignLocation = DataFacade.Location.New()
                            .With_DateCreated(today)
                            .With_Abbreviation(NoteForeignLocationCode)
                            .With_Code(NoteForeignLocationCode)
                            .With_Name(NoteForeignLocationCode)
                            .With_LtCode("ATM")
                            .With_HandlingType("ATM")
                            .With_ServicingDepotID(NoteCitDepot.ID)
                            .With_ServicingDepotCoinsID(CoinCitDepot.ID)
                            .With_CompanyID(ForeignCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(OrderingDepartment.ID)
                            .With_NotesSiteID(NoteCashCenter.ID)
                            .With_ForeignCurrencySiteID(ForeignCashCenter.IdentityID)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }                    

                    CoinForeignLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == CoinForeignLocationCode);
                    if (CoinForeignLocation == null)
                    {
                        CoinForeignLocation = DataFacade.Location.New()
                            .With_DateCreated(today)
                            .With_Abbreviation(CoinForeignLocationCode)
                            .With_Code(CoinForeignLocationCode)
                            .With_Name(CoinForeignLocationCode)
                            .With_LtCode("ATM")
                            .With_HandlingType("ATM")
                            .With_ServicingDepotID(NoteCitDepot.ID)
                            .With_ServicingDepotCoinsID(CoinCitDepot.ID)
                            .With_CompanyID(ForeignCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(OrderingDepartment.ID)                            
                            .With_CoinsSiteID(CoinCashCenter.ID)
                            .With_ForeignCurrencySiteID(ForeignCashCenter.IdentityID)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }                    

                    NoteCoinForeignLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == NoteCoinForeignLocationCode);
                    if (NoteCoinForeignLocation == null)
                    {
                        NoteCoinForeignLocation = DataFacade.Location.New()
                            .With_DateCreated(today)
                            .With_Abbreviation(NoteCoinForeignLocationCode)
                            .With_Code(NoteCoinForeignLocationCode)
                            .With_Name(NoteCoinForeignLocationCode)
                            .With_LtCode("ATM")
                            .With_HandlingType("ATM")
                            .With_ServicingDepotID(NoteCitDepot.ID)
                            .With_ServicingDepotCoinsID(CoinCitDepot.ID)
                            .With_CompanyID(ForeignCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(OrderingDepartment.ID)
                            .With_NotesSiteID(NoteCashCenter.ID)
                            .With_CoinsSiteID(CoinCashCenter.ID)
                            .With_ForeignCurrencySiteID(ForeignCashCenter.IdentityID)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }
                    
                    OutOfServiceLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == OutOfServiceLocationCode);
                    if (OutOfServiceLocation == null)
                    {
                        OutOfServiceLocation = DataFacade.Location.New()
                            .With_DateCreated(today)
                            .With_Abbreviation(OutOfServiceLocationCode)
                            .With_Code(OutOfServiceLocationCode)
                            .With_Name(OutOfServiceLocationCode)
                            .With_LtCode("ATM")
                            .With_HandlingType("ATM")
                            .With_ServicingDepotID(NoteCitDepot.ID)
                            .With_ServicingDepotCoinsID(CoinCitDepot.ID)
                            .With_CompanyID(ForeignCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(OrderingDepartment.ID)                            
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }
                    
                    ExternalLocation = context.Locations.AsNoTracking().FirstOrDefault(l => l.Code == ExternalLocationCode);
                    if (ExternalLocation == null)
                    {
                        ExternalLocation = DataFacade.Location.New()
                            .With_DateCreated(today)
                            .With_Abbreviation(ExternalLocationCode)
                            .With_Code(ExternalLocationCode)
                            .With_Name(ExternalLocationCode)
                            .With_LtCode("ATM")
                            .With_HandlingType("ATM")
                            .With_ServicingDepotID(NoteCitDepot.ID)
                            .With_ServicingDepotCoinsID(CoinCitDepot.ID)
                            .With_CompanyID(ForeignCompany.ID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(OrderingDepartment.ID)
                            .With_NotesSiteID(ExternalCashCenter.ID)
                            .With_CoinsSiteID(ExternalCashCenter.ID)
                            .With_ForeignCurrencySiteID(ExternalCashCenter.IdentityID)
                            .With_IsInheritFromVisitAddress(false)
                            .SaveToDb();
                    }

                    LocationDict.Add(NoteLocation1Code, NoteLocation1);
                    LocationDict.Add(NoteLocation2Code, NoteLocation2);
                    LocationDict.Add(CoinLocation1Code, CoinLocation1);
                    LocationDict.Add(CoinLocation2Code, CoinLocation2);
                    LocationDict.Add(ForeignLocationCode, ForeignLocation);
                    LocationDict.Add(NoteCoinLocationCode, NoteCoinLocation);
                    LocationDict.Add(NoteForeignLocationCode, NoteForeignLocation);
                    LocationDict.Add(CoinForeignLocationCode, CoinForeignLocation);
                    LocationDict.Add(NoteCoinForeignLocationCode, NoteCoinForeignLocation);
                    LocationDict.Add(OutOfServiceLocationCode, OutOfServiceLocation);
                    LocationDict.Add(ExternalLocationCode, ExternalLocation);
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 7)]
        public static void ConfigureMaterials()
        {
            using (var context = new AutomationBaseDataContext())
            {
                try
                {
                    Eur1Material = context.Materials.AsNoTracking().FirstOrDefault(m => m.Currency == "EUR" && m.Type == "COIN" && m.Denomination == 1m); // cannot use materialCode due to { currency, type, denomination } constraint
                    if (Eur1Material == null)
                    {
                        Eur1Material = DataFacade.Material.New()
                            .With_Currency("EUR")
                            .With_Denomination(1m)
                            .With_Description("1 EUR Coin")
                            .With_MaterialID(Eur1MaterialCode)
                            .With_Type("COIN")
                            .With_Weight(0.0075m)
                            .SaveToDb();
                    }                    

                    Eur2Material = context.Materials.AsNoTracking().FirstOrDefault(m => m.Currency == "EUR" && m.Type == "COIN" && m.Denomination == 2m);
                    if (Eur2Material == null)
                    {
                        Eur2Material = DataFacade.Material.New()
                            .With_Currency("EUR")
                            .With_Denomination(2m)
                            .With_Description("2 EUR Coin")
                            .With_MaterialID(Eur2MaterialCode)
                            .With_Type("COIN")
                            .With_Weight(0.0085m)
                            .SaveToDb();
                    }

                    Eur10Material = context.Materials.AsNoTracking().FirstOrDefault(m => m.Currency == "EUR" && m.Type == "NOTE" && m.Denomination == 10m);
                    if (Eur10Material == null)
                    {
                        Eur10Material = DataFacade.Material.New()
                            .With_Currency("EUR")
                            .With_Denomination(10m)
                            .With_Description("10 EUR Note")
                            .With_MaterialID(Eur10MaterialCode)
                            .With_Type("NOTE")
                            .With_Weight(0.001m)
                            .SaveToDb();
                    }

                    Eur20Material = context.Materials.AsNoTracking().FirstOrDefault(m => m.Currency == "EUR" && m.Type == "NOTE" && m.Denomination == 20m);
                    if (Eur20Material == null)
                    {
                        Eur10Material = DataFacade.Material.New()
                            .With_Currency("EUR")
                            .With_Denomination(20m)
                            .With_Description("20 EUR Note")
                            .With_MaterialID(Eur20MaterialCode)
                            .With_Type("NOTE")
                            .With_Weight(0.001m)
                            .SaveToDb();
                    }

                    Eur100Material = context.Materials.AsNoTracking().FirstOrDefault(m => m.Currency == "EUR" && m.Type == "NOTE" && m.Denomination == 100m);
                    if (Eur100Material == null)
                    {
                        Eur100Material = DataFacade.Material.New()
                            .With_Currency("EUR")
                            .With_Denomination(100m)
                            .With_Description("100 EUR Note")
                            .With_MaterialID(Eur100MaterialCode)
                            .With_Type("NOTE")
                            .With_Weight(0.001m)
                            .SaveToDb();
                    }

                    Usd1Material = context.Materials.AsNoTracking().FirstOrDefault(m => m.Currency == "USD" && m.Type == "COIN" && m.Denomination == 1m);
                    if (Usd1Material == null)
                    {
                        Usd1Material = DataFacade.Material.New()
                            .With_Currency("USD")
                            .With_Denomination(1m)
                            .With_Description("1 USD Coin")
                            .With_MaterialID(Usd1MaterialCode)
                            .With_Type("COIN")
                            .With_Weight(0.0075m)
                            .SaveToDb();
                    }

                    Usd10Material = context.Materials.AsNoTracking().FirstOrDefault(m => m.Currency == "USD" && m.Type == "NOTE" && m.Denomination == 10m);
                    if (Usd10Material == null)
                    {
                        Usd10Material = DataFacade.Material.New()
                            .With_Currency("USD")
                            .With_Denomination(10m)
                            .With_Description("10 USD Note")
                            .With_MaterialID(Usd10MaterialCode)
                            .With_Type("NOTE")
                            .With_Weight(0.001m)
                            .SaveToDb();
                    }

                    Usd20Material = context.Materials.AsNoTracking().FirstOrDefault(m => m.Currency == "USD" && m.Type == "NOTE" && m.Denomination == 20m);
                    if (Usd20Material == null)
                    {
                        Usd10Material = DataFacade.Material.New()
                            .With_Currency("USD")
                            .With_Denomination(20m)
                            .With_Description("20 USD Note")
                            .With_MaterialID(Usd20MaterialCode)
                            .With_Type("NOTE")
                            .With_Weight(0.001m)
                            .SaveToDb();
                    }

                    Usd100Material = context.Materials.AsNoTracking().FirstOrDefault(m => m.Currency == "USD" && m.Type == "NOTE" && m.Denomination == 100m);
                    if (Usd100Material == null)
                    {
                        Usd100Material = DataFacade.Material.New()
                            .With_Currency("USD")
                            .With_Denomination(100m)
                            .With_Description("100 USD Note")
                            .With_MaterialID(Usd100MaterialCode)
                            .With_Type("NOTE")
                            .With_Weight(0.001m)
                            .SaveToDb();
                    }

                    Gbp10Material = context.Materials.AsNoTracking().FirstOrDefault(m => m.Currency == "GBP" && m.Type == "NOTE" && m.Denomination == 10m);
                    if (Gbp10Material == null)
                    {
                        Gbp10Material = DataFacade.Material.New()
                            .With_Currency("GBP")
                            .With_Denomination(10m)
                            .With_Description("10 GBP Note")
                            .With_MaterialID(Gbp10MaterialCode)
                            .With_Type("NOTE")
                            .With_Weight(0.001m)
                            .SaveToDb();
                    }

                    Gbp100Material = context.Materials.AsNoTracking().FirstOrDefault(m => m.Currency == "GBP" && m.Type == "NOTE" && m.Denomination == 100m);
                    if (Gbp100Material == null)
                    {
                        Gbp100Material = DataFacade.Material.New()
                            .With_Currency("GBP")
                            .With_Denomination(100m)
                            .With_Description("100 GBP Note")
                            .With_MaterialID(Gbp100MaterialCode)
                            .With_Type("NOTE")
                            .With_Weight(0.001m)
                            .SaveToDb();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 8)]
        public static void ConfigureProducts()
        {
            using (var context = new AutomationBaseDataContext())
            {
                try
                {
                    Eur1Product = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Eur1ProductCode);
                    if (Eur1Product == null)
                    {
                        Eur1Product = DataFacade.Product.New()
                            .With_Currency("EUR")
                            .With_Denomination(1m)
                            .With_Description(Eur1ProductDesc)
                            .With_ProductCode(Eur1ProductCode)
                            .With_Type("COIN")
                            .With_Value(25)
                            .With_Weight(0.1875m)
                            .With_WrappingWeight(0)
                            .With_Materials(25, Eur1Material)
                            .SaveToDb();
                    }
                    
                    Eur2Product = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Eur2ProductCode);
                    if (Eur2Product == null)
                    {
                        Eur2Product = DataFacade.Product.New()
                            .With_Currency("EUR")
                            .With_Denomination(2m)
                            .With_Description(Eur2ProductDesc)
                            .With_ProductCode(Eur2ProductCode)
                            .With_Type("COIN")
                            .With_Value(50)
                            .With_Weight(0.2125m)
                            .With_WrappingWeight(0)
                            .With_Materials(25, Eur2Material)
                            .SaveToDb();
                    }

                    Eur1PackProduct = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Eur1PackProductCode);
                    if (Eur1PackProduct == null)
                    {
                        Eur1PackProduct = DataFacade.Product.New()
                            .With_Currency("EUR")
                            .With_Denomination(1m)
                            .With_Description(Eur1PackProductDesc)
                            .With_ProductCode(Eur1PackProductCode)
                            .With_Type("COIN")
                            .With_Value(250)
                            .With_Weight(1.875m)
                            .With_WrappingWeight(0)
                            .With_Products(10, Eur1Product)
                            .SaveToDb();
                    }
                    
                    Eur2PackProduct = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Eur2PackProductCode);
                    if (Eur2PackProduct == null)
                    {
                        Eur2PackProduct = DataFacade.Product.New()
                            .With_Currency("EUR")
                            .With_Denomination(2m)
                            .With_Description(Eur2PackProductDesc)
                            .With_ProductCode(Eur2PackProductCode)
                            .With_Type("COIN")
                            .With_Value(500)
                            .With_Weight(2.125m)
                            .With_WrappingWeight(0)
                            .With_Products(10, Eur2Product)
                            .SaveToDb();
                    }

                    Eur10Product = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Eur10ProductCode);
                    if (Eur10Product == null)
                    {
                        Eur10Product = DataFacade.Product.New()
                            .With_Currency("EUR")
                            .With_Denomination(10m)
                            .With_Description(Eur10ProductDesc)
                            .With_ProductCode(Eur10ProductCode)
                            .With_Type("NOTE")
                            .With_Value(1000)
                            .With_Weight(0.1m)
                            .With_Materials(100, Eur10Material)
                            .With_WrappingWeight(0)
                            .SaveToDb();
                    }

                    Eur100Product = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Eur100ProductCode);
                    if (Eur100Product == null)
                    {
                        Eur100Product = DataFacade.Product.New()
                            .With_Currency("EUR")
                            .With_Denomination(100m)
                            .With_Description(Eur100ProductDesc)
                            .With_ProductCode(Eur100ProductCode)
                            .With_Type("NOTE")
                            .With_Value(10000)
                            .With_Weight(0.1m)
                            .With_Materials(100, Eur100Material)
                            .With_WrappingWeight(0)
                            .SaveToDb();
                    }

                    Usd1Product = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Usd1ProductCode);
                    if (Usd1Product == null)
                    {
                        Usd1Product = DataFacade.Product.New()
                            .With_Currency("USD")
                            .With_Denomination(1m)
                            .With_Description(Usd1ProductDesc)
                            .With_ProductCode(Usd1ProductCode)
                            .With_Type("COIN")
                            .With_Value(25)
                            .With_Weight(0.1875m)
                            .With_Materials(25, Usd1Material)
                            .With_WrappingWeight(0)
                            .SaveToDb();
                    }

                    Usd10Product = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Usd10ProductCode);
                    if (Usd10Product == null)
                    {
                        Usd10Product = DataFacade.Product.New()
                            .With_Currency("USD")
                            .With_Denomination(10m)
                            .With_Description(Usd10ProductDesc)
                            .With_ProductCode(Usd10ProductCode)
                            .With_Type("NOTE")
                            .With_Value(1000)
                            .With_Weight(0.1m)
                            .With_Materials(100, Usd10Material)
                            .With_WrappingWeight(0)
                            .SaveToDb();
                    }

                    Usd100Product = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Usd100ProductCode);
                    if (Usd100Product == null)
                    {
                        Usd100Product = DataFacade.Product.New()
                            .With_Currency("USD")
                            .With_Denomination(100m)
                            .With_Description(Usd100ProductDesc)
                            .With_ProductCode(Usd100ProductCode)
                            .With_Type("NOTE")
                            .With_Value(10000)
                            .With_Weight(0.1m)
                            .With_Materials(100, Usd100Material)
                            .With_WrappingWeight(0)
                            .SaveToDb();
                    }                    

                    Gbp10Product = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Gbp10ProductCode);
                    if (Gbp10Product == null)
                    {
                        Gbp10Product = DataFacade.Product.New()
                            .With_Currency("GBP")
                            .With_Denomination(10m)
                            .With_Description(Gbp10ProductDesc)
                            .With_ProductCode(Gbp10ProductCode)
                            .With_Type("NOTE")
                            .With_Value(1000)
                            .With_Weight(0.1m)
                            .With_Materials(100, Gbp10Material)
                            .With_WrappingWeight(0)
                            .SaveToDb();
                    }
                    
                    Gbp100Product = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Gbp100ProductCode);
                    if (Gbp100Product == null)
                    {
                        Gbp100Product = DataFacade.Product.New()
                            .With_Currency("GBP")
                            .With_Denomination(100m)
                            .With_Description(Gbp100ProductDesc)
                            .With_ProductCode(Gbp100ProductCode)
                            .With_Type("NOTE")
                            .With_Value(10000)
                            .With_Weight(0.1m)
                            .With_Materials(100, Gbp100Material)
                            .With_WrappingWeight(0)
                            .SaveToDb();
                    }

                    Eur1BarcodedProduct = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Eur1BarcodedProductCode);
                    if (Eur1BarcodedProduct == null)
                    {
                        Eur1BarcodedProduct = DataFacade.Product.New()
                            .With_Currency("EUR")
                            .With_Denomination(1m)
                            .With_Description(Eur1BarcodedProductDesc)
                            .With_ProductCode(Eur1BarcodedProductCode)
                            .With_Type("COIN")
                            .With_Value(10)
                            .With_Weight(0.075m)
                            .With_Materials(10, Eur1Material)
                            .With_WrappingWeight(0)
                            .With_IsBarcodedProduct(true)
                            .SaveToDb();
                    }

                    Eur10BarcodedProduct = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Eur10BarcodedProductCode);
                    if (Eur10BarcodedProduct == null)
                    {
                        Eur10BarcodedProduct = DataFacade.Product.New()
                            .With_Currency("EUR")
                            .With_Denomination(10m)
                            .With_Description(Eur10BarcodedProductDesc)
                            .With_ProductCode(Eur10BarcodedProductCode)
                            .With_Type("NOTE")
                            .With_Value(100)
                            .With_Weight(0.01m)
                            .With_Materials(10, Eur10Material)
                            .With_WrappingWeight(0)
                            .With_IsBarcodedProduct(true)
                            .SaveToDb();
                    }

                    Eur100BarcodedProduct = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Eur100BarcodedProductCode);
                    if (Eur100BarcodedProduct == null)
                    {
                        Eur100BarcodedProduct = DataFacade.Product.New()
                            .With_Currency("EUR")
                            .With_Denomination(100m)
                            .With_Description(Eur100BarcodedProductDesc)
                            .With_ProductCode(Eur100BarcodedProductCode)
                            .With_Type("NOTE")
                            .With_Value(1000)
                            .With_Weight(0.01m)
                            .With_Materials(10, Eur100Material)
                            .With_WrappingWeight(0)
                            .With_IsBarcodedProduct(true)
                            .SaveToDb();
                    }

                    Usd1BarcodedProduct = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Usd1BarcodedProductCode);
                    if (Usd1BarcodedProduct == null)
                    {
                        Usd1BarcodedProduct = DataFacade.Product.New()
                            .With_Currency("USD")
                            .With_Denomination(1m)
                            .With_Description(Usd1BarcodedProductDesc)
                            .With_ProductCode(Usd1BarcodedProductCode)
                            .With_Type("COIN")
                            .With_Value(10)
                            .With_Weight(0.075m)
                            .With_Materials(10, Usd1Material)
                            .With_WrappingWeight(0)
                            .With_IsBarcodedProduct(true)
                            .SaveToDb();
                    }

                    Usd10BarcodedProduct = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Usd10BarcodedProductCode);
                    if (Usd10BarcodedProduct == null)
                    {
                        Usd10BarcodedProduct = DataFacade.Product.New()
                            .With_Currency("USD")
                            .With_Denomination(10m)
                            .With_Description(Usd10BarcodedProductDesc)
                            .With_ProductCode(Usd10BarcodedProductCode)
                            .With_Type("NOTE")
                            .With_Value(100)
                            .With_Weight(0.01m)
                            .With_Materials(10, Usd10Material)
                            .With_WrappingWeight(0)
                            .With_IsBarcodedProduct(true)
                            .SaveToDb();
                    }

                    Eur1LooseProduct = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Eur1LooseProductCode);
                    if (Eur1LooseProduct == null)
                    {
                        Eur1LooseProduct = DataFacade.Product.New()
                            .With_Currency("EUR")
                            .With_Denomination(1m)
                            .With_Description(Eur1LooseProductDesc)
                            .With_ProductCode(Eur1LooseProductCode)
                            .With_Type("COIN")
                            .With_Value(100)
                            .With_Weight(0.75m)
                            .With_Materials(100, Eur1Material)
                            .With_WrappingWeight(0)
                            .SaveToDb();
                    }

                    Eur20LooseProduct = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Eur20LooseProductCode);
                    if (Eur20LooseProduct == null)
                    {
                        Eur20LooseProduct = DataFacade.Product.New()
                            .With_Currency("EUR")
                            .With_Denomination(20m)
                            .With_Description(Eur20LooseProductDesc)
                            .With_ProductCode(Eur20LooseProductCode)
                            .With_Type("NOTE")
                            .With_Value(2000)
                            .With_Weight(0.1m)
                            .With_Materials(100, Eur20Material)
                            .With_WrappingWeight(0)
                            .SaveToDb();
                    }

                    Usd1LooseProduct = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Usd1LooseProductCode);
                    if (Usd1LooseProduct == null)
                    {
                        Usd1LooseProduct = DataFacade.Product.New()
                            .With_Currency("USD")
                            .With_Denomination(1m)
                            .With_Description(Usd1LooseProductDesc)
                            .With_ProductCode(Usd1LooseProductCode)
                            .With_Type("COIN")
                            .With_Value(100)
                            .With_Weight(0.75m)
                            .With_Materials(100, Usd1Material)
                            .With_WrappingWeight(0)
                            .SaveToDb();
                    }

                    Usd20LooseProduct = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductCode == Usd20LooseProductCode);
                    if (Usd20LooseProduct == null)
                    {
                        Usd20LooseProduct = DataFacade.Product.New()
                            .With_Currency("USD")
                            .With_Denomination(20m)
                            .With_Description(Usd20LooseProductDesc)
                            .With_ProductCode(Usd20LooseProductCode)
                            .With_Type("NOTE")
                            .With_Value(2000)
                            .With_Weight(0.1m)
                            .With_Materials(100, Usd20Material)
                            .With_WrappingWeight(0)
                            .SaveToDb();
                    }

                    ProductDict.Add(Eur1Product.Description, Eur1Product);
                    ProductDict.Add(Eur2Product.Description, Eur2Product);
                    ProductDict.Add(Eur1PackProduct.Description, Eur1PackProduct);
                    ProductDict.Add(Eur2PackProduct.Description, Eur2PackProduct);
                    ProductDict.Add(Eur10Product.Description, Eur10Product);                    
                    ProductDict.Add(Eur100Product.Description, Eur100Product);
                    ProductDict.Add(Usd1Product.Description, Usd1Product);
                    ProductDict.Add(Usd10Product.Description, Usd10Product);                    
                    ProductDict.Add(Usd100Product.Description, Usd100Product);
                    ProductDict.Add(Gbp10Product.Description, Gbp10Product);
                    ProductDict.Add(Gbp100Product.Description, Gbp100Product);
                    ProductDict.Add(Eur1BarcodedProduct.Description, Eur1BarcodedProduct);
                    ProductDict.Add(Eur10BarcodedProduct.Description, Eur10BarcodedProduct);
                    ProductDict.Add(Eur100BarcodedProduct.Description, Eur100BarcodedProduct);
                    ProductDict.Add(Usd1BarcodedProduct.Description, Usd1BarcodedProduct);
                    ProductDict.Add(Usd10BarcodedProduct.Description, Usd10BarcodedProduct);
                    ProductDict.Add(Eur1LooseProduct.Description, Eur1LooseProduct);
                    ProductDict.Add(Eur20LooseProduct.Description, Eur20LooseProduct);
                    ProductDict.Add(Usd1LooseProduct.Description, Usd1LooseProduct);
                    ProductDict.Add(Usd20LooseProduct.Description, Usd20LooseProduct);
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 9)]
        public static void ConfigureBagTypes()
        {
            using (var context = new AutomationBaseDataContext())
            {
                try
                {
                    SimpleNotesProductsBagType = context.BagTypes.AsNoTracking().FirstOrDefault(b => b.Code == SimpleNotesProductsBagTypeCode);
                    if (SimpleNotesProductsBagType == null)
                    {
                        SimpleNotesProductsBagType = DataFacade.ContainerType.New()
                            .With_Number(SimpleNotesProductsBagTypeNumber)
                            .With_Code(SimpleNotesProductsBagTypeCode)
                            .With_Description(SimpleNotesProductsBagTypeCode)
                            .With_IsAllocationBySize(false)
                            .With_IsAtmCassette(false)
                            .With_MaxValue(200000m) // 4 x 50000 (500 EUR bundle)
                            .With_MaxWeight(0.5m)
                            .With_MaxProductQuantity(4) // 4 x 100 notes                            
                            .SaveToDb();
                    }

                    /* not used currently - DO NOT REMOVE */
                    //ComplexNotesProductsBagType = context.BagTypes.AsNoTracking().FirstOrDefault(b => b.Code == ComplexNotesProductsBagTypeCode);
                    //if (ComplexNotesProductsBagType == null)
                    //{
                    //    ComplexNotesProductsBagType = DataFacade.ContainerType.New()
                    //        .With_Number(ComplexNotesProductsBagTypeNumber)
                    //        .With_Code(ComplexNotesProductsBagTypeCode)
                    //        .With_Description(ComplexNotesProductsBagTypeCode)
                    //        .With_IsAllocationBySize(false)
                    //        .With_IsAtmCassette(false)
                    //        .With_MaxValue(500000m)
                    //        .With_MaxWeight(2m)
                    //        .With_MaxProductQuantity(1000) 
                    //        .SaveToDb();
                    //}

                    SimpleCoinsProductsBagType = context.BagTypes.AsNoTracking().FirstOrDefault(b => b.Code == SimpleCoinsProductsBagTypeCode);
                    if (SimpleCoinsProductsBagType == null)
                    {
                        SimpleCoinsProductsBagType = DataFacade.ContainerType.New()
                            .With_Number(SimpleCoinsProductsBagTypeNumber)
                            .With_Code(SimpleCoinsProductsBagTypeCode)
                            .With_Description(SimpleCoinsProductsBagTypeCode)
                            .With_IsAllocationBySize(false)
                            .With_IsAtmCassette(false)
                            .With_MaxValue(200m) // 4 x 50 (2 EUR roll)
                            .With_MaxWeight(1m)
                            .With_MaxProductQuantity(4) // 4 rolls
                            .SaveToDb();
                    }

                    ComplexCoinsProductsBagType = context.BagTypes.AsNoTracking().FirstOrDefault(b => b.Code == ComplexCoinsProductsBagTypeCode);
                    if (ComplexCoinsProductsBagType == null)
                    {
                        ComplexCoinsProductsBagType = DataFacade.ContainerType.New()
                            .With_Number(ComplexCoinsProductsBagTypeNumber)
                            .With_Code(ComplexCoinsProductsBagTypeCode)
                            .With_Description(ComplexCoinsProductsBagTypeCode)
                            .With_IsAllocationBySize(false)
                            .With_IsAtmCassette(false)
                            .With_MaxValue(2000m) // 4 x 500 (2 EUR pack of rolls)
                            .With_MaxWeight(15m)
                            .With_MaxProductQuantity(4) // 4 packs
                            .SaveToDb();
                    }

                    BarcodedNotesProductsBagType = context.BagTypes.AsNoTracking().FirstOrDefault(b => b.Code == BarcodedNotesProductsBagTypeCode);
                    if (BarcodedNotesProductsBagType == null)
                    {
                        BarcodedNotesProductsBagType = DataFacade.ContainerType.New()
                            .With_Number(BarcodedNotesProductsBagTypeNumber)
                            .With_Code(BarcodedNotesProductsBagTypeCode)
                            .With_Description(BarcodedNotesProductsBagTypeCode)
                            .With_IsAllocationBySize(false)
                            .With_IsAtmCassette(false)
                            .With_MaxValue(200000m)
                            .With_MaxWeight(0.5m)
                            .With_MaxProductQuantity(4)
                            .SaveToDb();
                    }

                    BarcodedCoinsProductsBagType = context.BagTypes.AsNoTracking().FirstOrDefault(b => b.Code == BarcodedCoinsProductsBagTypeCode);
                    if (BarcodedCoinsProductsBagType == null)
                    {
                        BarcodedCoinsProductsBagType = DataFacade.ContainerType.New()
                            .With_Number(BarcodedCoinsProductsBagTypeNumber)
                            .With_Code(BarcodedCoinsProductsBagTypeCode)
                            .With_Description(BarcodedCoinsProductsBagTypeCode)
                            .With_IsAllocationBySize(false)
                            .With_IsAtmCassette(false)
                            .With_MaxValue(40m)
                            .With_MaxWeight(0.5m)
                            .With_MaxProductQuantity(4)
                            .SaveToDb();
                    }

                    ForeignCurrencyNotesProductsBagType = context.BagTypes.AsNoTracking().FirstOrDefault(b => b.Code == ForeignCurrencyNotesProductsBagTypeCode);
                    if (ForeignCurrencyNotesProductsBagType == null)
                    {
                        ForeignCurrencyNotesProductsBagType = DataFacade.ContainerType.New()
                            .With_Number(ForeignCurrencyNotesProductsBagTypeNumber)
                            .With_Code(ForeignCurrencyNotesProductsBagTypeCode)
                            .With_Description(ForeignCurrencyNotesProductsBagTypeCode)
                            .With_IsAllocationBySize(false)
                            .With_IsAtmCassette(false)
                            .With_MaxValue(40000m) // 4 x 10000 (100 USD bundle)
                            .With_MaxWeight(0.5m)
                            .With_MaxProductQuantity(4) // 4 x 100 notes
                            .SaveToDb();
                    }

                    ForeignCurrencyCoinsProductsBagType = context.BagTypes.AsNoTracking().FirstOrDefault(b => b.Code == ForeignCurrencyCoinsProductsBagTypeCode);
                    if (ForeignCurrencyCoinsProductsBagType == null)
                    {
                        ForeignCurrencyCoinsProductsBagType = DataFacade.ContainerType.New()
                            .With_Number(ForeignCurrencyCoinsProductsBagTypeNumber)
                            .With_Code(ForeignCurrencyCoinsProductsBagTypeCode)
                            .With_Description(ForeignCurrencyCoinsProductsBagTypeCode)
                            .With_IsAllocationBySize(false)
                            .With_IsAtmCassette(false)
                            .With_MaxValue(100m) // 4 x 25 (1 USD roll)
                            .With_MaxWeight(1m)
                            .With_MaxProductQuantity(4) // 4 rolls
                            .SaveToDb();
                    }

                    LooseProductsBagType = context.BagTypes.AsNoTracking().FirstOrDefault(b => b.Code == LooseProductsBagTypeCode);
                    if (LooseProductsBagType == null)
                    {
                        LooseProductsBagType = DataFacade.ContainerType.New()
                            .With_Number(LooseProductsBagTypeNumber)
                            .With_Code(LooseProductsBagTypeCode)
                            .With_Description(LooseProductsBagTypeCode)
                            .With_IsAllocationBySize(false)
                            .With_IsAtmCassette(false)
                            .With_MaxValue(400000m) // 8 x 50000 (500 EUR bundle)
                            .With_MaxWeight(1m)
                            .With_MaxProductQuantity(8) // 8 x note bundles or coin rolls
                            .SaveToDb();
                    }                    

                    SizeAllocationBagType = context.BagTypes.AsNoTracking().FirstOrDefault(b => b.Code == SizeAllocationBagTypeCode);
                    if (SizeAllocationBagType == null)
                    {
                        SizeAllocationBagType = DataFacade.ContainerType.New()
                            .With_Number(SizeAllocationBagTypeNumber)
                            .With_Code(SizeAllocationBagTypeCode)
                            .With_Description(SizeAllocationBagTypeCode)
                            .With_IsAllocationBySize(true)
                            .With_IsAtmCassette(false)
                            .With_MaxValue(200000m) // 4 x 50000 (500 EUR bundle)
                            .With_MaxWeight(0.5m)
                            .With_MaxProductQuantity(4) // 4 x 100 notes
                            .SaveToDb();
                    }

                    AtmCassetteBagType = context.BagTypes.AsNoTracking().FirstOrDefault(b => b.Code == AtmCassetteBagTypeCode);
                    if (AtmCassetteBagType == null)
                    {
                        AtmCassetteBagType = DataFacade.ContainerType.New()
                            .With_Number(AtmCassetteBagTypeNumber)
                            .With_Code(AtmCassetteBagTypeCode)
                            .With_Description(AtmCassetteBagTypeCode)
                            .With_IsAllocationBySize(false)
                            .With_IsAtmCassette(true)
                            .With_MaxValue(200000m) // 4 x 50000 (500 EUR bundle)
                            .With_MaxWeight(0.5m)
                            .With_MaxProductQuantity(4) // 4 x 100 notes
                            .SaveToDb();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 10)]
        public static void ConfigureBagTypeLinks()
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                try
                {
                    var simpleNotesProductsBagTypeId = SimpleNotesProductsBagType.ID;
                    var simpleCoinsProductsBagTypeId = SimpleCoinsProductsBagType.ID;
                    var complexCoinsProductsBagTypeId = ComplexCoinsProductsBagType.ID;
                    var barcodedNotesProductsBagTypeId = BarcodedNotesProductsBagType.ID;
                    var barcodedCoinsProductsBagTypeId = BarcodedCoinsProductsBagType.ID;
                    var foreignCurrencyNotesProductsBagTypeId = ForeignCurrencyNotesProductsBagType.ID;
                    var foreignCurrencyCoinsProductsBagTypeId = ForeignCurrencyCoinsProductsBagType.ID;
                    var looseProductsBagTypeId = LooseProductsBagType.ID;                    

                    if (!context.BagTypeContentSettings.Any(l => l.BagType_id == SimpleNotesProductsBagType.ID))
                    {
                        foreach(var productId in new string[] { Eur10ProductCode, Eur100ProductCode, Gbp10ProductCode, Gbp100ProductCode })
                        {
                            AddBagTypeContentSettingLink(simpleNotesProductsBagTypeId, productId);
                        }                        
                    }

                    if (!context.BagTypeContentSettings.Any(l => l.BagType_id == SimpleCoinsProductsBagType.ID))
                    {
                        foreach (var productId in new string[] { Eur1ProductCode, Eur2ProductCode })
                        {
                            AddBagTypeContentSettingLink(simpleCoinsProductsBagTypeId, productId);
                        }
                    }

                    if (!context.BagTypeContentSettings.Any(l => l.BagType_id == ComplexCoinsProductsBagType.ID))
                    {
                        foreach (var productId in new string[] { Eur1PackProductCode, Eur2PackProductCode })
                        {
                            AddBagTypeContentSettingLink(complexCoinsProductsBagTypeId, productId);
                        }
                    }

                    if (!context.BagTypeContentSettings.Any(l => l.BagType_id == BarcodedNotesProductsBagType.ID))
                    {
                        foreach (var productId in new string[] { Eur10BarcodedProductCode, Eur100BarcodedProductCode, Usd10BarcodedProductCode })
                        {
                            AddBagTypeContentSettingLink(barcodedNotesProductsBagTypeId, productId);
                        }
                    }

                    if (!context.BagTypeContentSettings.Any(l => l.BagType_id == BarcodedCoinsProductsBagType.ID))
                    {
                        foreach (var productId in new string[] { Eur1BarcodedProductCode, Usd1BarcodedProductCode })
                        {
                            AddBagTypeContentSettingLink(barcodedCoinsProductsBagTypeId, productId);
                        }
                    }

                    if (!context.BagTypeContentSettings.Any(l => l.BagType_id == ForeignCurrencyNotesProductsBagType.ID))
                    {
                        foreach (var productId in new string[] { Usd10ProductCode, Usd100ProductCode })
                        {
                            AddBagTypeContentSettingLink(foreignCurrencyNotesProductsBagTypeId, productId);
                        }
                    }

                    if (!context.BagTypeContentSettings.Any(l => l.BagType_id == ForeignCurrencyCoinsProductsBagType.ID))
                    {
                        foreach (var productId in new string[] { Usd1ProductCode })
                        {
                            AddBagTypeContentSettingLink(foreignCurrencyCoinsProductsBagTypeId, productId);
                        }
                    }

                    if (!context.BagTypeContentSettings.Any(l => l.BagType_id == LooseProductsBagType.ID))
                    {
                        foreach (var productId in new string[] { Eur1LooseProductCode, Usd1LooseProductCode, Eur20LooseProductCode,  Usd20LooseProductCode })
                        {
                            AddBagTypeContentSettingLink(looseProductsBagTypeId, productId);
                        }
                    }

                    if (!context.BagTypeMaterialTypeLinks.Any(l => l.BagTypeID == SizeAllocationBagType.ID))
                    {
                        foreach (var type in new string[] { "NOTE", "COIN" })
                        {
                            var link = new BagTypeMaterialTypeLink
                            {
                                BagTypeID = SizeAllocationBagType.ID,
                                MaterialTypeID = type,
                                MinimumQuantity = 1,
                                MaximumQuantity = 400
                            };
                            context.BagTypeMaterialTypeLinks.Add(link);
                        }
                    }

                    /// local function to add content setting link
                    void AddBagTypeContentSettingLink(int bagTypeId, string productId)
                    {
                        var link = new BagTypeContentSetting
                        {
                            BagType_id = bagTypeId,
                            Product_id = productId,
                            MaxQuantity = 4
                        };
                        context.BagTypeContentSettings.Add(link);
                    }

                    context.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 11)]
        public static void ConfigureServiceTypes()
        {
            using (var context = new AutomationBaseDataContext())
            {
                if (!context.ServiceTypes.Any(s => s.Code == DeliverCode))
                {
                    AddServiceType(DeliverCode, TypeOfServiceType.Deliver);                    
                }
                if (!context.ServiceTypes.Any(s => s.Code == CollectCode))
                {
                    AddServiceType(CollectCode, TypeOfServiceType.Collect);
                }
                if (!context.ServiceTypes.Any(s => s.Code == ServicingCode))
                {
                    AddServiceType(ServicingCode, TypeOfServiceType.Servicing);
                }
                if (!context.ServiceTypes.Any(s => s.Code == ReplenishmentCode))
                {
                    AddServiceType(ReplenishmentCode, TypeOfServiceType.Replenishment);
                }

                /// local function to add a service type
                void AddServiceType(string code, TypeOfServiceType type)
                {
                    var serviceType = new ServiceType {Code = code, Name = code, OldType = code, Type = type};
                    context.ServiceTypes.Add(serviceType);
                }

                context.SaveChanges();
            }
        }       

        [BeforeFeature(Order = 12)]
        public static void SetIsBaseDataConfigured()
        {
            IsBaseDataConfigured = true;
        }

        [BeforeFeature(Order = 13)]
        public static void ConfigureRequestor()
        {
            using (var context = new AutomationCallManagementDataContext())
            {
                try
                {
                    CallRequestor = context.Requestors.AsNoTracking().FirstOrDefault(r => r.Name == RequestorName);
                    RequestorDict.Add(RequestorName, CallRequestor);
                }
                catch
                {
                    throw;
                }
            }
        }

        public static void ConfigureBaseData()
        {
            Init();
            ConfigureCompanies();
            ConfigureCashCenterAndCitDepotLocations();
            ConfigureSites();
            ConfigureOrderingDepartment();
            ConfigureLocations();
            ConfigureMaterials();
            ConfigureProducts();
            ConfigureBagTypes();
            ConfigureBagTypeLinks();
            ConfigureServiceTypes();
            ConfigureRequestor();
            SetIsBaseDataConfigured();
        }
    }
}
