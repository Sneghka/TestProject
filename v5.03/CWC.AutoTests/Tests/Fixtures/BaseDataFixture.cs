using Cwc.BaseData;
using Cwc.BaseData.Enums;
using Cwc.Security;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Data;
using System.Linq;

namespace CWC.AutoTests.Tests.Fixtures
{
    /// <summary>
    /// Fixture for base data creation. It will create:
    /// 1. Company
    /// 2. Sites
    /// 3. Location Type
    /// 4. Locations
    /// 5. Create Order and Schedule settings (with lines) that are required for service order creation
    /// </summary>
    public class BaseDataFixture : IDisposable
    {
        private string name = "AutoTestManagement";
        private string code = $"1101{new Random().Next(3333, 9999)}";
        private string abbrev = "Abbrev";
        private int orderingDepartmentID = DataFacade.Group.Take(g => g.DepartmentType == DepartmentType.Ordering).Build().ID;

        public Location LocationCIT { get; set; }
        public Location LocationCC { get; set; }
        public Location LocationATM { get; set; }
        public Location LocationNOR { get; set; }
        public Location VisitAddress { get; set; }
        public Location MainLocation { get; set; }
        public Location CoinsLocation { get; set; }
        public Location NotesLocation { get; set; }
        public Location NotesAndCoinsLocation { get; set; }
        public Location Location { get; set; }

        public Customer Customer { get; set; }
        public Site SiteCC { get; set; }
        public Site SiteCIT { get; set; }
        public Site SiteCoins { get; set; }
        public Site SiteNotes { get; set; }
        public Site SiteNotesAndCoins { get; set; }
        public LocationType LocationType { get; set; }

        public BaseDataFixture()
        {
            Cwc.BaseData.Classes.ConfigurationKeySet.Load();
            var configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == ConfigurationKeyName.UseCwcCSMasterDataValidation);
            HelperFacade.ConfigurationKeysHelper.Update(configKey, "False");
                        
            using (var context = new AutomationBaseDataContext())
            {                
                Customer = context.Customers.FirstOrDefault(c => c.ReferenceNumber == code);
                if (Customer == null)
                {
                    Customer = DataFacade.Customer.New()
                        .With_ReferenceNumber(code)
                        .With_Name(name)
                        .With_Abbrev(abbrev)
                        .SaveToDb();
                }                

                LocationType = context.LocationTypes.FirstOrDefault(lt => lt.ltCode == code);
                if (LocationType == null)
                {
                    LocationType = DataFacade.LocationType.New()
                        .With_ltCode(code)
                        .With_ltDesc(name)
                        .With_Category(LocationTypeCategory.Retail)
                        .SaveToDb();
                }
               
                LocationCIT = context.Locations.FirstOrDefault(l => l.Code == code + "4");
                if (LocationCIT == null)
                {
                    LocationCIT = DataFacade.Location.New()
                        .With_Code(code + "4")
                        .With_Name(name)
                        .With_Abbreviation(abbrev)
                        .With_Level(LocationLevel.ServicePoint)
                        .With_CompanyID(Customer.ID)
                        .With_HandlingType(BaseDataFacade.LocationDepType)
                        .With_LocationTypeID(LocationType.ltCode)
                        .SaveToDb();
                }

                SiteCIT = context.Sites.FirstOrDefault(s => s.Branch_cd == code);
                if (SiteCIT == null)
                {
                    SiteCIT = DataFacade.Site.New()
                        .With_Branch_cd(code)
                        .With_Description(name)
                        .With_LocationID(LocationCIT.ID)
                        .With_BranchType(BranchType.CITDepot)
                        .With_WP_IsExternal(false)
                        .SaveToDb();
                }

                VisitAddress = context.Locations.FirstOrDefault(l => l.Code == code + "VA");
                if (VisitAddress == null)
                {
                    VisitAddress = DataFacade.Location.New()
                        .With_Code(code + "VA")
                        .With_Name(name)
                        .With_Enabled(true)
                        .With_Level(LocationLevel.VisitAddress)
                        .With_LocationTypeID(LocationType.ltCode)
                        .With_Abbreviation(abbrev)
                        .With_CompanyID(Customer.ID)
                        .With_ServicingDepotID(SiteCIT.ID)
                        .With_Address(DataFacade.Address.New()
                                        .With_Country(code)
                                        .With_City(code)
                                        .With_Street(code)
                                        .With_State(code))
                        .SaveToDb();
                }

                MainLocation = context.Locations.FirstOrDefault(l => l.Code == code + "MN");
                if (MainLocation == null)
                {
                    MainLocation = DataFacade.Location.New()
                        .With_Code(code + "MN")
                        .With_Name(name)
                        .With_Abbreviation(abbrev)
                        .With_Level(LocationLevel.ServicePoint)
                        .With_CompanyID(Customer.ID)
                        .With_HandlingType(BaseDataFacade.LocationCasType)
                        .With_LocationTypeID(LocationType.ltCode)
                        .With_Address(DataFacade.Address.New().With_Country(code).With_City(code).With_Street(code).With_State(code))
                        .SaveToDb();
                }

                CoinsLocation = context.Locations.FirstOrDefault(l => l.Code == code + "1");
                if (CoinsLocation == null)
                {
                    CoinsLocation = DataFacade.Location.New()
                        .With_Code(code + "1")
                        .With_Name(name)
                        .With_Abbreviation(abbrev)
                        .With_Level(LocationLevel.ServicePoint)
                        .With_CompanyID(Customer.ID)
                        .With_HandlingType(BaseDataFacade.LocationCasType)
                        .With_LocationTypeID(LocationType.ltCode)
                        .SaveToDb();
                }

                NotesLocation = context.Locations.FirstOrDefault(l => l.Code == code + "2");
                if (NotesLocation == null)
                {
                    NotesLocation = DataFacade.Location.New()
                        .With_Code(code + "2")
                        .With_Name(name)
                        .With_Abbreviation(abbrev)
                        .With_Level(LocationLevel.ServicePoint)
                        .With_CompanyID(Customer.ID)
                        .With_HandlingType(BaseDataFacade.LocationCasType)
                        .With_LocationTypeID(LocationType.ltCode)
                        .SaveToDb();
                }

                NotesAndCoinsLocation = context.Locations.FirstOrDefault(l => l.Code == code + "3");
                if (NotesAndCoinsLocation == null)
                {
                    NotesAndCoinsLocation = DataFacade.Location.New()
                        .With_Code(code + "3")
                        .With_Name(name)
                        .With_Abbreviation(abbrev)
                        .With_Level(LocationLevel.ServicePoint)
                        .With_CompanyID(Customer.ID)
                        .With_HandlingType(BaseDataFacade.LocationCasType)
                        .With_LocationTypeID(LocationType.ltCode)
                        .SaveToDb();
                }

                LocationCC = context.Locations.FirstOrDefault(l => l.Code == code + "5");
                if (LocationCC == null)
                {
                    LocationCC = DataFacade.Location.New()
                        .With_Code(code + "5")
                        .With_Name(name)
                        .With_Abbreviation(abbrev)
                        .With_Level(LocationLevel.ServicePoint)
                        .With_CompanyID(Customer.ID)
                        .With_HandlingType(BaseDataFacade.LocationCasType)
                        .With_LocationTypeID(LocationType.ltCode)
                        .SaveToDb();
                }

                SiteCC = context.Sites.FirstOrDefault(s => s.Branch_cd == code + "1");
                if (SiteCC == null)
                {
                    SiteCC = DataFacade.Site.New()
                        .With_Branch_cd(code + "1")
                        .With_Description(name)
                        .With_LocationID(LocationCC.ID)
                        .With_BranchType(BranchType.CashCenter)
                        .With_SubType(BranchSubType.NotesAndCoins)
                        .With_WP_IsExternal(false)
                        .SaveToDb();
                }

                LocationATM = context.Locations.FirstOrDefault(l => l.Code == code + "6");
                if (LocationATM == null)
                {
                    LocationATM = DataFacade.Location.New()
                        .With_Code(code + "6")
                        .With_Name(name)
                        .With_Abbreviation(abbrev)
                        .With_Level(LocationLevel.ServicePoint)
                        .With_CompanyID(Customer.ID)
                        .With_HandlingType(BaseDataFacade.LocationAtmType)
                        .With_LocationTypeID(LocationType.ltCode)
                        .With_ServicingDepotID(SiteCIT.ID)
                        .With_OrderingDepartmentID(orderingDepartmentID)
                        .With_CashPointNumber(code)
                        .With_CashPointTypeID(DataFacade.CashPointType.Take(x => x.Number == 1).Build().Number)
                        .SaveToDb();
                }

                LocationNOR = context.Locations.FirstOrDefault(l => l.Code == code + "7");
                if (LocationNOR == null)
                {
                    LocationNOR = DataFacade.Location.New()
                        .With_Code(code + "7")
                        .With_Name(name)
                        .With_Abbreviation(abbrev)
                        .With_Level(LocationLevel.ServicePoint)
                        .With_CompanyID(Customer.ID)
                        .With_HandlingType(BaseDataFacade.LocationNorType)
                        .With_OrderingDepartmentID(orderingDepartmentID)
                        .With_LocationTypeID(LocationType.ltCode)
                        .With_ServicingDepotID(SiteCIT.ID)
                        .SaveToDb();
                }

                SiteNotes = context.Sites.FirstOrDefault(s => s.Branch_cd == code + "2");
                if (SiteNotes == null)
                {
                    SiteNotes = DataFacade.Site.New()
                        .With_Branch_cd(code + "2")
                        .With_Description(name)
                        .With_LocationID(NotesLocation.ID)
                        .With_BranchType(BranchType.CashCenter)
                        .With_SubType(BranchSubType.Notes)
                        .With_WP_IsExternal(false)
                        .SaveToDb();
                }

                SiteCoins = context.Sites.FirstOrDefault(s => s.Branch_cd == code + "3");
                if (SiteCoins == null)
                {
                    SiteCoins = DataFacade.Site.New()
                        .With_Branch_cd(code + "3")
                        .With_Description(name)
                        .With_LocationID(CoinsLocation.ID)
                        .With_BranchType(BranchType.CashCenter)
                        .With_SubType(BranchSubType.Coins)
                        .With_WP_IsExternal(false)
                        .SaveToDb();
                }

                SiteNotesAndCoins = context.Sites.FirstOrDefault(s => s.Branch_cd == code + "4");
                if (SiteNotesAndCoins == null)
                {
                    SiteNotesAndCoins = DataFacade.Site.New()
                        .With_Branch_cd(code + "4")
                        .With_Description(name)
                        .With_LocationID(NotesAndCoinsLocation.ID)
                        .With_BranchType(BranchType.CashCenter)
                        .With_SubType(BranchSubType.NotesAndCoins)
                        .With_WP_IsExternal(false)
                        .SaveToDb();
                }                
            }            
        }

        public void Dispose()
        {
            using (var _context = new AutomationTransportDataContext())
            {
                _context.CitProcessSettingLinks.RemoveRange(_context.CitProcessSettingLinks);
                _context.SaveChanges();
                _context.BaseAddresses.RemoveRange(_context.BaseAddresses);
                _context.SaveChanges();
                _context.Locations.RemoveRange(_context.Locations.Where(l => l.Code.StartsWith(code)));
                _context.SaveChanges();

                _context.Sites.RemoveRange(_context.Sites.Where(b => b.Branch_cd.StartsWith(code)));
                _context.SaveChanges();
                _context.Customers.RemoveRange(_context.Customers.Where(c => c.ReferenceNumber.StartsWith(code)));
                _context.SaveChanges();
                _context.Customers.RemoveRange(_context.Customers.Where(x => x.Name == name));
                DataFacade.Site.DeleteMany(x => x.Branch_cd.StartsWith(code));
                _context.LocationTypes.RemoveRange(_context.LocationTypes.Where(x => x.ltCode.StartsWith(code)));
                _context.SaveChanges();
            }            
        }
    }
}
