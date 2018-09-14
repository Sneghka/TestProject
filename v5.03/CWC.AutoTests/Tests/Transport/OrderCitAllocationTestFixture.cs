using Cwc.BaseData;
using Cwc.Contracts;
using Cwc.Security;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Linq;

namespace CWC.AutoTests.Tests.Transport
{
    public class OrderCitAllocationTestFixture
    {
        public const string externalLocationCode = "JG02";  // ref_loc_nr of location with external notes and coins sites
        public const string externalDepotLocCode = "JG99";  // ref_loc_nr of location with external CIT depot
        public const string locationCase1Code = "TOCase1";  // next 8 location codes are required for testing 7818 test case
        public const string locationCase2Code = "TOCase2";
        public const string locationCase3Code = "TOCase3";
        public const string locationCase4Code = "TOCase4";
        public const string locationCase5Code = "TOCase5";
        public const string locationCase6Code = "TOCase6";
        public const string locationCase7Code = "TOCase7";
        public const string locationCase8Code = "TOCase8";
        public const string referenceNumber      = "3303";
        public const string localNotesSite         = "JG";
        public const string localCoinsSite    = "JG Coin";
        public const string ExternalSite        = "JG NC";
        public Location ExternalDepotLocation { get; private set; }
        public Location ExternalLocation { get; private set; }
        public Location LocationCase1 { get; private set; }
        public Location LocationCase2 { get; private set; }
        public Location LocationCase3 { get; private set; }
        public Location LocationCase4 { get; private set; }
        public Location LocationCase5 { get; private set; }
        public Location LocationCase6 { get; private set; }
        public Location LocationCase7 { get; private set; }
        public Location LocationCase8 { get; private set; }        
        public Contract CompanyContract { get; private set; }
        private Site externalDepot;

        public OrderCitAllocationTestFixture()
        {
            var servicingDepot = DataFacade.Site.Take(s => s.BranchType == BranchType.CITDepot && !s.WP_IsExternal);
            var companyID = DataFacade.Customer.Take(c => c.ReferenceNumber == referenceNumber).Build().ID;
            var orderingDepartmentID = DataFacade.Location.Take(l => l.Code == externalLocationCode).Build().OrderingDepartmentID;
            var localNotesSiteID = DataFacade.Site.Take(s => s.Branch_cd == localNotesSite).Build().Branch_nr;
            var localCoinsSiteID = DataFacade.Site.Take(s => s.Branch_cd == localCoinsSite).Build().Branch_nr;
            var externalSiteID = DataFacade.Site.Take(s => s.Branch_cd == ExternalSite).Build().Branch_nr;
            var bankLocationTypeCode = DataFacade.LocationType.Take(lt => lt.ltDesc == "Bank").Build().ltCode;
            var login = SecurityFacade.LoginService.GetAdministratorLogin();

            using (var context = new AutomationContractDataContext())
            {
                // Find or create company contract                
                CompanyContract = context.Contracts.FirstOrDefault(c => c.CustomerID == companyID
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

                // Re-configure ordering settings for main service types: COLL, DELV, SERV, REPL
                // with clearing of all existing ordering settings of custom contract  
                HelperFacade.ContractHelper.ReconfigureOrderingSettings(CompanyContract.ID);

                // Re-configure schedule settings for main service types: COLL, DELV, SERV, REPL 
                // with clearing of all existing schedule settings and schedule lines of custom contract                    
                HelperFacade.ContractHelper.ReconfigureScheduleSettings(CompanyContract);                
            }

            using (var context = new AutomationBaseDataContext())
            {
                try
                {                    
                    ExternalLocation = context.Locations.FirstOrDefault(l => l.Code == externalLocationCode);
                    if (ExternalLocation == null)
                    {
                        throw new Exception("JG02 location is not found. Please check your database.");
                    }
                                        
                    externalDepot = context.Sites.FirstOrDefault(s => s.BranchType == BranchType.CITDepot && s.WP_IsExternal);
                    if (externalDepot == null)
                    {
                        externalDepot = DataFacade.Site.New()
                            .With_BranchType(BranchType.CITDepot)
                            .With_Branch_cd("EX01")
                            .With_Description("External CIT depot")
                            .With_Location(DataFacade.Location.Take(l => l.ID > 1))
                            .With_WP_IsExternal(true)
                            .SaveToDb();
                    }
                                         
                    ExternalDepotLocation = context.Locations.FirstOrDefault(l => l.CompanyID == companyID && context.Sites.Any(b => b.BranchType == BranchType.CITDepot && b.WP_IsExternal && b.ID == l.BranchID));
                    if (ExternalDepotLocation == null)
                    {
                        ExternalDepotLocation = DataFacade.Location.New()
                            .With_Abbreviation(externalDepotLocCode)
                            .With_Code(externalDepotLocCode)
                            .With_Name("Transport Order TC7815-5")
                            .With_LtCode("NOR")
                            .With_HandlingType("NOR")
                            .With_ServicingDepot(externalDepot)
                            .With_CompanyID(companyID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(orderingDepartmentID)
                            .SaveToDb();                            
                    }

                    /* Location -> Notes Site is empty.
                       Location -> Coins Site is empty.
                       Location -> Location type -> Category != 'Bank'. */
                    if (context.Locations.Any(l => l.Code == locationCase1Code))
                    {
                        LocationCase1 = DataFacade.Location.Take(l => l.Code == locationCase1Code).Build();
                    }
                    else
                    {
                        LocationCase1 = DataFacade.Location.New()
                            .With_Abbreviation(locationCase1Code)
                            .With_Code(locationCase1Code)
                            .With_Name("Transport Order TC7818-1")
                            .With_LtCode("NOR")
                            .With_HandlingType("NOR")
                            .With_ServicingDepot(servicingDepot)
                            .With_CompanyID(companyID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(orderingDepartmentID)
                            .SaveToDb();
                    }

                    /* Location -> Notes Site -> Is External = "no".
                       Location -> Coins Site -> Is External = "no".
                       Location -> Location type -> Category != 'Bank'. */
                    if (context.Locations.Any(l => l.Code == locationCase2Code))
                    {
                        LocationCase2 = DataFacade.Location.Take(l => l.Code == locationCase2Code).Build();
                    }
                    else
                    {
                        LocationCase2 = DataFacade.Location.New()
                            .With_Abbreviation(locationCase2Code)
                            .With_Code(locationCase2Code)
                            .With_Name("Transport Order TC7818-2")
                            .With_LtCode("NOR")
                            .With_HandlingType("NOR")
                            .With_ServicingDepot(servicingDepot)
                            .With_CompanyID(companyID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(orderingDepartmentID)
                            .With_NotesSiteID(localNotesSiteID)
                            .With_CoinsSiteID(localCoinsSiteID)
                            .SaveToDb();
                    }

                    /* Location -> Notes Site -> Is External = "no".
                       Location -> Coins Site is empty.
                       Location -> Location type -> Category != 'Bank'. */
                    if (context.Locations.Any(l => l.Code == locationCase3Code))
                    {
                        LocationCase3 = DataFacade.Location.Take(l => l.Code == locationCase3Code).Build();
                    }
                    else
                    {
                        LocationCase3 = DataFacade.Location.New()
                            .With_Abbreviation(locationCase3Code)
                            .With_Code(locationCase3Code)
                            .With_Name("Transport Order TC7818-3")
                            .With_LtCode("NOR")
                            .With_HandlingType("NOR")
                            .With_ServicingDepot(servicingDepot)
                            .With_CompanyID(companyID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(orderingDepartmentID)
                            .With_NotesSiteID(localNotesSiteID)                                               
                            .SaveToDb();
                    }

                    /* Location -> Notes Site is empty.
                       Location -> Coins Site -> Is External = "no".
                       Location -> Location type -> Category != 'Bank'. */
                    if (context.Locations.Any(l => l.Code == locationCase4Code))
                    {
                        LocationCase4 = DataFacade.Location.Take(l => l.Code == locationCase4Code).Build();
                    }
                    else
                    {
                        LocationCase4 = DataFacade.Location.New()
                            .With_Abbreviation(locationCase4Code)
                            .With_Code(locationCase4Code)
                            .With_Name("Transport Order TC7818-4")
                            .With_LtCode("NOR")
                            .With_HandlingType("NOR")
                            .With_ServicingDepot(servicingDepot)
                            .With_CompanyID(companyID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(orderingDepartmentID)
                            .With_CoinsSiteID(localCoinsSiteID)
                            .SaveToDb();
                    }

                    /* Location -> Notes Site -> Is External = "no".
                       Location -> Coins Site -> Is External = "no".
                       Location -> Location type -> Category = 'Bank'. */
                    if (context.Locations.Any(l => l.Code == locationCase5Code))
                    {
                        LocationCase5 = DataFacade.Location.Take(l => l.Code == locationCase5Code).Build();
                    }
                    else
                    {
                        LocationCase5 = DataFacade.Location.New()
                            .With_Abbreviation(locationCase5Code)
                            .With_Code(locationCase5Code)
                            .With_Name("Transport Order TC7818-5")
                            .With_LtCode(bankLocationTypeCode)
                            .With_HandlingType("NOR")
                            .With_ServicingDepot(servicingDepot)
                            .With_CompanyID(companyID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(orderingDepartmentID)
                            .With_NotesSiteID(localNotesSiteID)
                            .With_CoinsSiteID(localCoinsSiteID)
                            .SaveToDb();
                    }

                    /* Location -> Notes Site is empty.
                       Location -> Coins Site is empty.
                       Location -> Location type -> Category = 'Bank'. */
                    if (context.Locations.Any(l => l.Code == locationCase6Code))
                    {
                        LocationCase6 = DataFacade.Location.Take(l => l.Code == locationCase6Code).Build();
                    }
                    else
                    {
                        LocationCase6 = DataFacade.Location.New()
                            .With_Abbreviation(locationCase6Code)
                            .With_Code(locationCase6Code)
                            .With_Name("Transport Order TC7818-6")
                            .With_LtCode(bankLocationTypeCode)
                            .With_HandlingType("NOR")
                            .With_ServicingDepot(servicingDepot)
                            .With_CompanyID(companyID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(orderingDepartmentID)                                               
                            .SaveToDb();
                    }

                    /* Location -> Notes Site -> Is External = "yes".
                       Location -> Coins Site -> Is External = "yes".
                       Location -> Location type -> Category = 'Bank'. */
                    if (context.Locations.Any(l => l.Code == locationCase7Code))
                    {
                        LocationCase7 = DataFacade.Location.Take(l => l.Code == locationCase7Code).Build();
                    }
                    else
                    {
                        LocationCase7 = DataFacade.Location.New()
                            .With_Abbreviation(locationCase7Code)
                            .With_Code(locationCase7Code)
                            .With_Name("Transport Order TC7818-7")
                            .With_LtCode(bankLocationTypeCode)
                            .With_HandlingType("NOR")
                            .With_ServicingDepot(servicingDepot)
                            .With_CompanyID(companyID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(orderingDepartmentID)
                            .With_NotesSiteID(externalSiteID)
                            .With_CoinsSiteID(externalSiteID)
                            .SaveToDb();
                    }

                    /* Location -> Notes Site -> Is External = "yes".
                       Location -> Coins Site -> Is External = "yes".
                       Location -> Location type -> Category != 'Bank'. */
                    if (context.Locations.Any(l => l.Code == locationCase8Code))
                    {
                        LocationCase8 = DataFacade.Location.Take(l => l.Code == locationCase8Code).Build();
                    }
                    else
                    {
                        LocationCase8 = DataFacade.Location.New()
                            .With_Abbreviation(locationCase8Code)
                            .With_Code(locationCase8Code)
                            .With_Name("Transport Order TC7818-8")
                            .With_LtCode("NOR")
                            .With_HandlingType("NOR")
                            .With_ServicingDepot(servicingDepot)
                            .With_CompanyID(companyID)
                            .With_PreferredLanguage(1)
                            .With_OrderingDepartmentID(orderingDepartmentID)
                            .With_NotesSiteID(externalSiteID)
                            .With_CoinsSiteID(externalSiteID)
                            .SaveToDb();
                    }

                    context.SaveChanges();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    HelperFacade.TransportHelper.ClearTestData();
                }
            }
        }            
    }
}
