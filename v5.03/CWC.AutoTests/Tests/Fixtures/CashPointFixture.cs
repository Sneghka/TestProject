using Cwc.BaseData;
using Cwc.BaseData.Enums;
using Cwc.BaseData.Model;
using Cwc.Coin;
using Cwc.Security;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Linq;

namespace CWC.AutoTests.Tests.Fixtures
{
    public class CashPointFixture : IDisposable
    {
        public Group groupOrdering, groupGeneral, groupManagement, groupSecurity;
        public MachineModel CashPointModel { get; set; }
        public CashPointType CashPointType { get; set; }
        public CoinMachine CashPoint { get; set; }
        public Customer Customer { get; set; }
        public StockPositionBuilder BaseStockPosition { get; set; }
        public Site SiteCIT { get; set; }
        public Location Location { get; set; }
        public Location LocationCIT { get; set; }
        public LocationType LocationType { get; set; }
        public Material Material { get; set; }

        string code = $"1314{new Random().Next(1000, 2000)}";
        const string name = "AutoTestManagement";
        const string abbrev = "Abbreviation";
        decimal denomination = new Random().Next(1, 1000);
        decimal weight = new Random().Next(1, 1000);


        public CashPointFixture()
        {
            int number = Int32.Parse(code);
            //ConfigurationNamesKey
            Cwc.BaseData.Classes.ConfigurationKeySet.Load();
            Cwc.Sync.SyncConfiguration.LoadExportMappings();
            var locker = new object();
            lock(locker)
            {
                using (var context = new AutomationCoinDataContext())
                {
                    groupOrdering = DataFacade.Group.Take(x => x.DepartmentType == DepartmentType.Ordering);
                    groupGeneral = DataFacade.Group.Take(x => x.DepartmentType == DepartmentType.General);
                    groupManagement = DataFacade.Group.Take(x => x.DepartmentType == DepartmentType.Management);
                    groupSecurity = DataFacade.Group.Take(x => x.DepartmentType == DepartmentType.Security);

                    Customer = context.Customers.FirstOrDefault(c => c.ReferenceNumber == code);
                    if (Customer == null)
                    {
                        Customer = DataFacade.Customer.New()
                            .With_ReferenceNumber(code)
                            .With_Name(name)
                            .With_Abbrev(abbrev)
                            .SaveToDb();
                    }

                    CashPointModel = DataFacade.CashPointModel.Take(cpm => cpm.Description == "Recycle ");

                    Material = context.Materials.FirstOrDefault(m => m.Description == "10Euro");
                    if (Material == null)
                    {
                        Material = DataFacade.Material.New().
                            With_MaterialID(code).
                            With_Description(name).
                            With_Type("NOTE").
                            With_MaterialNumber(number).
                            With_Currency("UAH").
                            With_Denomination(denomination).
                            With_Weight(weight).
                            SaveToDb();
                    }

                    CashPointType = context.CashPointTypes.FirstOrDefault(cpt => cpt.Name == name);
                    if (CashPointType == null)
                    {
                        CashPointType = DataFacade.CashPointType.New().
                            With_Number(number).
                            With_Name(name).
                            With_UseInOptimization(true).
                            With_IsCollect(true).
                            With_IsIssue(true).
                            With_IsRecycle(true).
                            With_HandlingType("ATM").
                            SaveToDb();
                    }

                    LocationType = context.LocationTypes.FirstOrDefault(lt => lt.ltCode == "533");
                    if (LocationType == null)
                    {
                        LocationType = DataFacade.LocationType.New().
                            With_ltCode(code).
                            With_ltDesc(name).
                            With_Category(LocationTypeCategory.Retail).
                            SaveToDb();
                    }

                    //CashPointModel = DataFacade.CashPointModel.New()
                    //    .With_Description(name)
                    //    .With_Name(name)
                    //    .SaveToDb();

                    LocationCIT = DataFacade.Location.New()
                        .With_Code(code + "4")
                        .With_Name(name)
                        .With_Abbreviation(abbrev)
                        .With_Level(LocationLevel.ServicePoint)
                        .With_CompanyID(Customer.ID)
                        .With_HandlingType(BaseDataFacade.LocationDepType)
                        .With_LocationTypeID(LocationType.ltCode)
                        //.With_ServicingDepot(SiteCIT)
                        .SaveToDb();

                    SiteCIT = DataFacade.Site.New()
                        .With_Branch_cd(code)
                        .With_Description(name)
                        .With_LocationID(LocationCIT.ID)
                        .With_BranchType(BranchType.CITDepot)
                        .With_WP_IsExternal(false)
                        .SaveToDb();

                    Location = DataFacade.Location.New()
                        .With_Code(code)
                        .With_Name(name)
                        .With_CompanyID(Customer.ID)
                        .With_Abbreviation(abbrev)
                        .With_LocationTypeID(LocationType.ltCode)
                        .With_HandlingType("ATM")
                        .With_ServicingDepotID(SiteCIT.ID)
                        .SaveToDb();

                    CashPoint = DataFacade.CashPoint.New()
                        .WithNumber(code)
                        .WithName(name)
                        .WithLocation(Location)
                        .WithIndividualStock(true)
                        .WithType(CashPointType)
                        .WithModel(CashPointModel)
                        .WithReplenishment(Cwc.Coin.ConstantNames.Enums.CoinMachine.ReplenishmentMethod.AddCash)
                        .WithCollectAndDeliveryServType(DataFacade.ServiceType.Take(x => x.Code == "DELV").Build())
                        .WithCollectOnlyServType(DataFacade.ServiceType.Take(x => x.Code == "COLL").Build())
                        .SaveToDb();

                    BaseStockPosition = DataFacade.StockPosition.New()
                        .WithMachineType(CashPointType)
                        .WithMachineModel(CashPointModel)
                        .WithQuantity(0)
                        .WithValue(0)
                        .WithWeight(0)
                        .WithCoinMachine(CashPoint)
                        .WithDateCreated(DateTime.Now.Date)
                        .WithDateUpdated(DateTime.Now.Date)
                        .WithIndicator(Cwc.Coin.ConstantNames.Enums.StockPosition.Indicator.Quantity)
                        .WithCollectGreenValue(600)
                        .WithCollectOrangeValue(900)
                        .WithDirection(Direction.Recycle)
                        .WithTotals(Cwc.Coin.ConstantNames.Enums.StockPosition.Totals.None)
                        .WithCapacity(1000)
                        .WithPriority(Priority.Normal)
                        .WithIsGrandTotal(false)
                        .WithCurrency("EUR")
                        .WithResidualCashPercentage(10)
                        .WithIssueGreenValue(400)
                        .WithIssueOrangeValue(100)
                        .WithIsOptimized(true)
                        .WithCounterfeits(false);

                    var actualMixedStockPosition = BaseStockPosition
                        .WithIsNew(true)
                        .WithType(Cwc.Coin.ConstantNames.Enums.StockPosition.Type.Actual)
                        .WithCassetteNumber(1)
                        .WithIsMixed(true)
                        .WithMaterialType("NOTE")
                        .SaveToDb();

                    var configMixedStockPosition = BaseStockPosition
                        .WithIsNew(true)
                        .WithType(Cwc.Coin.ConstantNames.Enums.StockPosition.Type.Configuration)
                        .WithCassetteNumber(1)
                        .WithIsMixed(true)
                        .WithMaterialType("NOTE")
                        .SaveToDb();

                    var actualNonMixedStockPosition = BaseStockPosition
                        .WithIsNew(true)
                        .WithType(Cwc.Coin.ConstantNames.Enums.StockPosition.Type.Actual)
                        .WithCassetteNumber(1)
                        .WithMaterial(Material.MaterialID)
                        .WithIsMixed(false)
                        .WithMaterialType("NOTE")
                        .SaveToDb();

                    var configNonMixedStockPosition = BaseStockPosition
                        .WithIsNew(true)
                        .WithType(Cwc.Coin.ConstantNames.Enums.StockPosition.Type.Configuration)
                        .WithCassetteNumber(1)
                        .WithMaterial(Material.MaterialID)
                        .WithIsMixed(false)
                        .WithMaterialType("NOTE")
                        .SaveToDb();
                }
            }
        }

        public void Dispose()
        {
            var locker = new object();
            lock (locker)
            {
                using (var _context = new AutomationCoinDataContext())
                {
                    var allLocations = _context.Locations.Where(x => x.Code.StartsWith(code)).Select(x => x.ID).ToArray();
                    var createdMachinesList = _context.CashPoints.Where(x => x.Name == name).Select(y => y.ID).ToList();

                    _context.Materials.RemoveRange(_context.Materials.Where(m => m.MaterialID.StartsWith(code)));
                    _context.SaveChanges();
                    _context.StockPositions.RemoveRange(_context.StockPositions.Where(x => createdMachinesList.Contains(x.CoinMachineId.Value)));
                    _context.SaveChanges();
                    _context.CashPoints.RemoveRange(_context.CashPoints.Where(x => createdMachinesList.Contains(x.ID)));
                    _context.SaveChanges();

                    _context.Sites.RemoveRange(_context.Sites.Where(x => x.Branch_cd.StartsWith(code)));
                    _context.SaveChanges();

                    using (var transportContext = new AutomationTransportDataContext())
                    {
                        transportContext.CitProcessSettingLinks.RemoveRange(transportContext.CitProcessSettingLinks.Where(x => allLocations.Contains(x.LocationID)));
                        transportContext.SaveChanges();
                    }

                    _context.Locations.RemoveRange(_context.Locations.Where(x => allLocations.Contains(x.ID)));
                    _context.SaveChanges();

                    _context.Customers.RemoveRange(_context.Customers.Where(x => x.ReferenceNumber.StartsWith(code)));
                    _context.SaveChanges();

                    _context.LocationTypes.RemoveRange(_context.LocationTypes.Where(x => x.ltCode.StartsWith(code)));
                    _context.SaveChanges();

                    _context.CashPointTypes.RemoveRange(_context.CashPointTypes.Where(x => x.Name == name));
                    _context.MachineModels.RemoveRange(_context.MachineModels.Where(x => x.Name == name));
                    _context.SaveChanges();
                }
            }
        }

    }
}
