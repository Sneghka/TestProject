using Cwc.BaseData;
using Cwc.BaseData.Enums;
using Cwc.CashCenter;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CWC.AutoTests.Tests.Fixtures
{
    public class StockContainerFixture: IDisposable
    {
        public StockContainerBuilder StockContainer { get; set; }
        public StockPositionCashCenterBuilder StockPosition { get; set; }
        public Customer Customer { get; set; }
        public Location Location { get; set; }
        public Location NoteLocation { get; set; }
        public Site NoteSite { get; set; }
        public LocationType LocationType { get; set; }
        public List<StockPosition> StockPositionList;
        DateTime date = DateTime.Today;
        int defaultNumber = new Random().Next(40000, 99999);
        string code = $"{new Random().Next(40000, 99999)}";
        const string CustomerCode = "3306";
        const string CustomerName = "FaryWorld";
        const string abbrev = "";
        string LocationTypeCode = $"1314{new Random().Next(1000, 2000)}";
        const string LocationTypeName = "AutoTestManagement";
        const string siteCode = "100060";
        const string siteName = "Fary";
        const int quantity = 10;
        const string currencyId = "EUR";


        public StockContainerFixture()
        {
            using (var context = new AutomationBaseDataContext())
            {
                Customer = context.Customers.FirstOrDefault(c => c.ReferenceNumber == CustomerCode);
                if (Customer == null)
                {
                    Customer = DataFacade.Customer.New().
                        With_ReferenceNumber(CustomerCode).
                        With_Name(CustomerName).
                        With_Abbrev(abbrev).
                        SaveToDb().
                        Build();
                }

                LocationType = context.LocationTypes.FirstOrDefault();
                if (LocationType == null)
                {
                    LocationType = DataFacade.LocationType.New().
                        With_ltCode(LocationTypeCode).
                        With_ltDesc(LocationTypeName).
                        With_Category(LocationTypeCategory.Retail).
                        SaveToDb();
                }

                NoteLocation = context.Locations.Where(l => l.Code == (code + "1")).FirstOrDefault();
                if (NoteLocation == null)
                {
                    NoteLocation = DataFacade.Location.New().
                        With_Code(code + "1").
                        With_Name(LocationTypeName).
                        With_Abbreviation(code + "1").
                        With_Level(LocationLevel.ServicePoint).
                        With_CompanyID(Customer.ID).
                        With_HandlingType(BaseDataFacade.LocationDepType).
                        With_LocationTypeID(LocationType.ltCode).
                        SaveToDb();
                }

                NoteSite = context.Sites.FirstOrDefault(b => b.Branch_cd == siteCode);
                if (NoteSite == null)
                {
                    NoteSite = DataFacade.Site.New().
                        With_Branch_cd(siteCode).
                        With_Description(siteName).
                        With_LocationID(NoteLocation.ID).
                        With_BranchType(BranchType.CITDepot).
                        With_WP_IsExternal(false).
                        SaveToDb();
                }

                Location = context.Locations.FirstOrDefault(l => l.NotesSiteID != null);
                if (Location == null)
                {
                    Location = DataFacade.Location.New().
                        With_Code(code).
                        With_CompanyID(Customer.ID).
                        With_LocationTypeID(LocationType.ltCode).
                        With_Name(LocationTypeName).
                        With_Abbreviation(code).
                        With_HandlingType("NOR").
                        With_ServicingDepotID(NoteSite.ID).
                        With_Level(LocationLevel.ServicePoint).
                        SaveToDb();
                }

                StockPosition = DataFacade.StockPositionCashCenter.New().
                    With_QualificationType(QualificationType.Fit).
                    With_Quantity(quantity).
                    With_Value(1m).
                    With_Weight(1m).
                    With_Currency_id(currencyId).
                    With_StockOwner_id(Customer.IdentityID);
                StockPosition.Build().SetIsTotal(true);

                StockPositionList = new List<StockPosition>();
                StockPositionList.Add(StockPosition);

                StockContainer = DataFacade.StockContainer.New().
                    WithNumber(code).
                    WithLocationFrom(Location.ID).
                    WithDateCollected(date).
                    With_StockPositions(StockPositionList).
                    SaveToDb();

            }
        }

        public void Dispose()
        {
            using (var _context = new AutomationCashCenterDataContext())
            {
                _context.StockPositions.RemoveRange(_context.StockPositions.Where(sp=>sp.Quantity == quantity && sp.Currency_id == currencyId && sp.IsTotal == true && sp.StockOwner_id == Customer.IdentityID));
                _context.SaveChanges();
                _context.StockContainers.RemoveRange(_context.StockContainers.Where(sc=>sc.Number == code));
                _context.SaveChanges();
            }
        }
    }
}