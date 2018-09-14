using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.BaseData.Enums;
using Cwc.BaseData.Model;
using Cwc.Contracts;
using Cwc.Security;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Linq;

namespace CWC.AutoTests.Tests.Fixtures
{
    public class OrderFixture : IDisposable
    {
        private const string serviceTypeCode = "COLL";
        private const string serviceTypeName = "AutoTestManagement";
        private const string serviceTypeOldType = "deliver";
        private const string customerName = "AutoTestManagement";
        private const string CashPointTypeName = "AutoTestManagement";
        private const string abbrev = "abbrev";
        private const string name = "AutoTestManagement";
        private const string type = "NOTE";
        private const string currency = "UAH";
        private const int number = 100;
        private const int materialQuantity = 10;

        public Customer Customer { get; set; }
        public ServiceType ServiceType { get; set; }
        public Location Location { get; set; }
        public Location LocationCIT { get; set; }
        public Location LocationATM { get; set; }
        public Site SiteCit { get; set; }        
        public LocationType LocationType { get; set; }
        public Contract CompanyContract { get; set; }
        public DateTime Date { get; set; }
        public Cwc.Ordering.Order ServiceOrder { get; set; }
        public CashPointType CashPointType { get; set; }
        public Material Material { get; set; }
        public Product ProductFirst { get; set; }
        public Product ProductSecond { get; set; }

        private decimal weight = new Random().Next(1, 1000);
        private decimal denomination = new Random().Next(1, 1000);
        private string defaultNumber = $"1101{new Random().Next(4000, 9999)}";
        private string locationCode = "SP021";
        private string customerCode = "3303";
        private string siteCode = $"1101{new Random().Next(33333, 99999)}";
        private string locationTypeCode = $"1101{new Random().Next(33333, 99999)}";
        private const string locationTypeName = "AutoTestManagement";
        private const string locationName = "AutoTestManagement";
        private const string siteName = "AutoTestManagement";
        private int cashPointTypeNumber = Int32.Parse($"1101{new Random().Next(33333, 99999)}");

        public OrderFixture()
        {
            var value = materialQuantity * denomination;
            var login = SecurityFacade.LoginService.GetAdministratorLogin();

            using (var _context = new AutomationBaseDataContext())
            {
                Customer = _context.Customers.Where(c => c.ReferenceNumber == customerCode).FirstOrDefault();
                if (Customer == null)
                {
                    Customer = DataFacade.Customer.New().
                        With_ReferenceNumber(customerCode).
                        With_Name(customerName).
                        With_Abbrev(abbrev).
                        SaveToDb();
                }

                LocationType = _context.LocationTypes.Where(lt => lt.ltCode == locationTypeCode).FirstOrDefault();
                if (LocationType == null)
                {
                    LocationType = DataFacade.LocationType.New().
                        With_ltCode(locationTypeCode).
                        With_ltDesc(locationTypeName).
                        With_Category(LocationTypeCategory.Retail).
                        SaveToDb();
                }

                ServiceType = _context.ServiceTypes.Where(st => st.Code == serviceTypeCode).FirstOrDefault();
                if (ServiceType == null)
                {
                    ServiceType = DataFacade.ServiceType.New().
                            With_Code(serviceTypeCode).
                            With_Name(serviceTypeName).
                            With_OldType(serviceTypeOldType).
                            SaveToDb(null).
                            Build();
                }

                LocationCIT = _context.Locations.Where(l => l.Code == (locationCode + "1")).FirstOrDefault();
                if (LocationCIT == null)
                {
                    LocationCIT = DataFacade.Location.New().
                        With_Code(locationCode + "1").
                        With_Name(locationName).
                        With_Abbreviation(abbrev).
                        With_Level(LocationLevel.ServicePoint).
                        With_CompanyID(Customer.ID).
                        With_HandlingType(BaseDataFacade.LocationDepType).
                        With_LocationTypeID(LocationType.ltCode).
                        SaveToDb().
                        Build();
                }

                SiteCit = _context.Sites.FirstOrDefault(s => s.Branch_cd == siteCode);
                if (SiteCit == null)
                {
                    SiteCit = DataFacade.Site.New().
                        With_Branch_cd(siteCode).
                        With_Description(siteName).
                        With_LocationID(LocationCIT.ID).
                        With_BranchType(BranchType.CITDepot).
                        With_WP_IsExternal(false).
                        SaveToDb().
                        Build();
                }

                CashPointType = _context.CashPointTypes.FirstOrDefault(c => c.Name == CashPointTypeName);
                if (CashPointType == null)
                {
                    CashPointType = DataFacade.CashPointType.New().
                        With_Number(cashPointTypeNumber).
                        With_Name(CashPointTypeName).
                        With_UseInOptimization(true).
                        With_IsCollect(true).
                        With_IsIssue(true).
                        With_IsRecycle(true).
                        With_HandlingType("ATM").
                        SaveToDb().
                        Build();
                }

                Location = _context.Locations.FirstOrDefault(l => l.Code == locationCode);
                if (Location == null)
                {
                    Location = DataFacade.Location.New().
                        With_Code(locationCode).
                        With_CompanyID(Customer.ID).
                        With_LocationTypeID(LocationType.ltCode).
                        With_Name(locationName).
                        With_Abbreviation(abbrev).
                        // With_BranchID(100001).
                        With_CashPointTypeID(CashPointType.ID).
                        With_HandlingType(BaseDataFacade.LocationNorType).
                        With_OrderingDepartmentID(20).
                        With_ServicingDepotID(SiteCit.ID).
                        With_Level(LocationLevel.ServicePoint).
                        SaveToDb().
                        Build();
                }

                LocationATM = _context.Locations.FirstOrDefault(l => l.Code == locationCode + "2");
                if (LocationATM == null)
                {
                    LocationATM = DataFacade.Location.New()
                        .With_Code(locationCode + "2")
                        .With_Name(locationName)
                        .With_Abbreviation(abbrev)
                        .With_Level(LocationLevel.ServicePoint)
                        .With_CompanyID(Customer.ID)
                        .With_HandlingType(BaseDataFacade.LocationAtmType)
                        .With_LocationTypeID(LocationType.ltCode)
                        .With_ServicingDepotID(SiteCit.ID)
                        .With_OrderingDepartmentID(20)
                        .With_CashPointTypeID(CashPointType.ID)
                        .SaveToDb();
                }                
            }

            using (var context = new AutomationContractDataContext())
            {
                CompanyContract = context.Contracts.AsNoTracking().FirstOrDefault(c => c.CustomerID == Customer.ID
                        && c.Currency_code == "EUR"
                        && c.IsLatestRevision
                        && c.Status == ContractStatus.Final);

                if (CompanyContract == null)
                {
                    CompanyContract = DataFacade.Contract.New()
                        .With_IsDefault(false)
                        .With_Number(customerCode)                        
                        .With_Currency_code("EUR")
                        .With_Customer_id(Customer.ID)
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

                HelperFacade.ContractHelper.ReconfigureOrderingSettings(CompanyContract.ID);
                HelperFacade.ContractHelper.ReconfigureScheduleSettings(CompanyContract);

                Material = DataFacade.Material.New().
                   With_MaterialID(defaultNumber).
                   With_Description(name).
                   With_Type(type).
                   With_MaterialNumber(number).
                   With_Currency(currency).
                   With_Denomination(denomination).
                   With_Weight(weight).
                   SaveToDb();

                ProductFirst = DataFacade.Product.New().
                   With_ProductCode(defaultNumber + "1").
                   With_Description(defaultNumber).
                   With_Denomination(denomination).
                   With_IsCustomerUnit(true).
                   With_Currency(currency).
                   With_Value(value).
                   With_Weight(weight).
                   With_Type(type).
                   With_WrappingWeight(weight).
                   With_Materials(materialQuantity, DataFacade.Material.Take(m => m.MaterialID == defaultNumber)).
                   SaveToDb();

                ProductSecond = DataFacade.Product.New().
                    With_ProductCode(defaultNumber).
                    With_Description(defaultNumber).
                    With_Denomination(denomination).
                    With_IsCustomerUnit(true).
                    With_Currency(currency).
                    With_Value(value * materialQuantity + value).
                    With_Weight(weight).
                    With_Type(type).
                    With_WrappingWeight(weight).
                    With_Materials(materialQuantity, Material).
                    With_Products(materialQuantity, ProductFirst).
                    SaveToDb().
                    Build();
            }
        }
        public void Dispose()
        {
            using (var _context = new AutomationCoinDataContext())
            {
                _context.Materials.RemoveRange(_context.Materials.Where(m => m.MaterialID.StartsWith(defaultNumber)));
                _context.ProductMaterialLinks
                    .RemoveRange(_context.ProductMaterialLinks
                        .Where(l => _context.Materials
                            .Where(m => m.MaterialID.StartsWith(defaultNumber))
                            .Select(x => x.ID)
                            .Contains(l.MaterialID)
                        )
                    );
                _context.ProdContents.RemoveRange(_context.ProdContents.Where(x => x.MaterialID.StartsWith(defaultNumber)));
                _context.ProdCompositions.RemoveRange(_context.ProdCompositions.Where(x => x.ProdCodeWhole.StartsWith(defaultNumber)));
                _context.Products.RemoveRange(_context.Products.Where(p => p.ProductCode.StartsWith(defaultNumber)));
                _context.SaveChanges();
                var allLocations = _context.Locations.Where(x => x.Code.StartsWith(locationCode)).Select(x => x.ID).ToArray();
                var createdMachines = _context.CashPoints.Where(x => x.Name == CashPointTypeName).Select(y => y.ID).ToArray();

                _context.Sites.RemoveRange(_context.Sites.Where(x => x.Branch_cd.StartsWith(siteCode)));
                _context.SaveChanges();
                using (var transportContext = new AutomationTransportDataContext())
                {
                    transportContext.CitProcessSettingLinks.RemoveRange(transportContext.CitProcessSettingLinks.Where(x => allLocations.Contains(x.LocationID)));
                    transportContext.SaveChanges();
                }

                _context.Locations.RemoveRange(_context.Locations.Where(x => allLocations.Contains(x.ID)));
                _context.SaveChanges();
                _context.StockPositions.RemoveRange(_context.StockPositions.Where(x => createdMachines.Contains(x.CoinMachineId.Value)));
                _context.SaveChanges();
                _context.CashPoints.RemoveRange(_context.CashPoints.Where(x => createdMachines.Contains(x.ID)));
                _context.SaveChanges();
                _context.CashPointTypes.RemoveRange(_context.CashPointTypes.Where(x => x.Name == CashPointTypeName));
                _context.SaveChanges();
                _context.LocationTypes.RemoveRange(_context.LocationTypes.Where(x => x.ltCode.StartsWith(locationTypeCode)));
                _context.SaveChanges();

                _context.MachineModels.RemoveRange(_context.MachineModels.Where(x => x.Name == CashPointTypeName));
                _context.SaveChanges();
            }            
        }                          
    }
}
