using Cwc.BaseData;
using Cwc.Routes;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.BasicExport
{
    public class BasicExportTest : IClassFixture<BasicExportCleanUpFixture>/*, IDisposable*/
    {
        #region DataInit
        const string customer = "customer";
        const string location = "location";
        const string material = "Material";
        const string product = "Product";
        const string prodContent = "ProdContent";
        const string serviceOrder = "ServiceOrder";
        const string orderLine = "SOline";
        const string orderProduct = "SOProduct";
        const string master = "master";
        const string masline = "mas_line";

        string refNumber, name;
        public BasicExportTest()
        {
            refNumber = $"1101{new Random().Next(1000, 99999).ToString()}";
            name = "AutoTestManagement";
        }
        #endregion

        [Fact(DisplayName = "Basic Export - Verify System exports RefernceNumber as a cus_nr attribute export file")]
        public void VerifyThatCustomerIsExported()
        {
            var abrev = "just abbrev";
            var action = 0;

            DataFacade.Customer.New()
                 .With_ReferenceNumber(refNumber)
                 .With_Name(name)
                 .With_Abbrev(abrev)
                 .SaveToDb();

            var customerExported = HelperFacade.BasicExportHelper.MapEntity(customer, refNumber, action);

            Assert.Equal(refNumber, customerExported["cus_nr"]);
            Assert.Equal(name, customerExported["name"]);
            Assert.Equal(abrev, customerExported["abbrev"]);
            Assert.Equal("true", customerExported["enabled"].ToLower());
            Assert.Equal("0", customerExported["WP_RecordType"]);
        }

        [Fact(DisplayName = "Basic Export - Verify that System exports proper Visit address")]
        public void VisitAddressShouldBeExported()
        {
            var action = 0;
            var abbrev = "new abbrev";
            var country = "Ukraine";
            var city = "Kyiv";
            var postCode = "01001";
            var street = "blvrd Perova, 10";
            var streetDet = "entrance keys";

            DataFacade.Customer.New()
                .With_ReferenceNumber(refNumber)
                .With_Name(name)
                .With_Abbrev(abbrev)
                .With_Enabled(false)
                .With_VisitAddress(DataFacade.Address.New()
                                                .With_Country(country)
                                                .With_City(city)
                                                .With_Street(street)
                                                .With_ExtraAddressInfo(streetDet)
                                                .With_PostalCode(postCode)
                                                .With_State("should not been exported")
                                                .With_Purpose(Cwc.BaseData.Purpose.Visit))
                .SaveToDb();

            var customerExported = HelperFacade.BasicExportHelper.MapEntity(customer, refNumber, action);

            Assert.Equal(country, customerExported["county"]);
            Assert.Equal(streetDet, customerExported["street_det"]);
            Assert.Equal(city, customerExported["town_st"]);
            Assert.Equal(postCode, customerExported["area_pc"]);
            Assert.Equal(street, customerExported["street"]);
            Assert.Equal("false", customerExported["enabled"].ToLower());
        }

        [Fact(DisplayName = "Basic Export - Verify that when Customer is edited Then System sends export file with Act = 1")]
        public void EditEventIsSentWhenCustomerIsUpdated()
        {
            DataFacade.Customer.New()
                .With_ReferenceNumber(refNumber)
                .With_Name(name)
                .With_Abbrev("abbrev")
                .With_Enabled(false)
                .SaveToDb();

            DataFacade.Customer.Take(c => c.ReferenceNumber == refNumber).With_Enabled(true).With_Name("edited").SaveToDb();

            var customerExported = HelperFacade.BasicExportHelper.MapEntity(customer, refNumber, 1);

            Assert.Equal("true", customerExported["enabled"].ToLower());
            Assert.Equal("edited", customerExported["name"]);
        }

        [Fact(DisplayName = "Basic Export - Verify that when Customer is deleted Then System sends export file with Act = 2")]
        public void DeleteEventIsSentWhenCustomerIsDeleted()
        {
            DataFacade.Customer.New()
                .With_ReferenceNumber(refNumber)
                .With_Name(name)
                .With_Abbrev("abbrev")
                .With_Enabled(false)
                .SaveToDb();

            DataFacade.Customer.Delete(c => c.ReferenceNumber == refNumber && c.RecordType == 0);

            var customerExported = HelperFacade.BasicExportHelper.MapEntity(customer, refNumber, 2);

            Assert.Equal(refNumber, customerExported["cus_nr"]);
        }

        [Fact(DisplayName = "Basic Export - Verify that when Material is created Then System exports file with Act = 0")]
        public void VerifyThatMaterialIsExportedOnCreation()
        {
            var type = "NOTE";
            var currency = "UAH";
            int number = 100;
            decimal denomination = new Random().Next(1, 1000);
            decimal weight = Convert.ToDecimal(new Random().NextDouble());

            DataFacade.Material.New()
                                    .With_MaterialID(refNumber)
                                    .With_Description(name)
                                    .With_Type(type)
                                    .With_MaterialNumber(number)
                                    .With_Currency(currency)
                                    .With_Denomination(denomination)
                                    .With_Weight(weight)
                                    .SaveToDb();

            var materialExported = HelperFacade.BasicExportHelper.MapEntity(material, refNumber, 0);

            Assert.Equal(refNumber, materialExported["materialID"]);
            Assert.Equal(name, materialExported["MaterialDesc"]);
            Assert.Equal(type, materialExported["matTypeCode"]);
            Assert.Equal(currency, materialExported["curCode"]);
            Assert.Equal(denomination, Convert.ToDecimal(materialExported["denomination"]));
            Assert.Equal(weight.ToString().Substring(0, 9), materialExported["weight"].Substring(0, 9));
        }

        [Fact(DisplayName = "Basic Export - Verify that when Material is updated Then System exports file with Act = 1")]
        public void VerifyThatMaterialIsExportedOnUpdating()
        {
            var type = "NOTE";
            var currency = "UAH";
            int number = 100;
            decimal denomination = new Random().Next(501, 1000);
            decimal denominationUpdated = new Random().Next(1, 500);
            decimal weight = Convert.ToDecimal(new Random().NextDouble());

            DataFacade.Material.New()
                                    .With_MaterialID(refNumber)
                                    .With_Description(name)
                                    .With_Type(type)
                                    .With_MaterialNumber(number)
                                    .With_Currency(currency)
                                    .With_Denomination(denomination)
                                    .With_Weight(weight)
                                    .SaveToDb();

            DataFacade.Material.Take(m => m.MaterialID == refNumber).With_Denomination(denominationUpdated).SaveToDb();

            var materialExported = HelperFacade.BasicExportHelper.MapEntity(material, refNumber, 1);

            Assert.Equal(denominationUpdated, Convert.ToDecimal(materialExported["denomination"]));
        }

        [Fact(DisplayName = "Basic Export - Verify that when Material is deleted Then System exports file with Act = 2")]
        public void VerifyThatMaterialIsExportedOnDeletion()
        {
            var type = "NOTE";
            var currency = "UAH";
            int number = 100;
            decimal denomination = new Random().Next(501, 1000);
            decimal weight = Convert.ToDecimal(new Random().NextDouble());

            DataFacade.Material.New()
                                    .With_MaterialID(refNumber)
                                    .With_Description(name)
                                    .With_Type(type)
                                    .With_MaterialNumber(number)
                                    .With_Currency(currency)
                                    .With_Denomination(denomination)
                                    .With_Weight(weight)
                                    .SaveToDb();

            DataFacade.Material.DeleteMany(m => m.MaterialID == refNumber);

            var materialExported = HelperFacade.BasicExportHelper.MapEntity(material, refNumber, 2);

            Assert.Equal(refNumber, materialExported["materialID"]);
        }

        [Fact(DisplayName = "Basic Export - Verify that when Product is created Then System exports file with Act = 0")]
        public void VerifyThatProductIsExportedOnCreation()
        {
            string type = "NOTE";
            decimal value = 100;
            decimal weight = 1;
            string currency = "USD";
            string unitName = "test unit name";
            string unitsname = "test units name";
            decimal wrapWeight = 2;
            string materialId = "314";
            int materialQuantity = 10;
            string articleCode = "article code";

            DataFacade.Product.New()
                                        .With_ProductCode(refNumber)
                                        .With_Description(name)
                                        .With_ArticleCode(articleCode)
                                        .With_Type(type)
                                        .With_Denomination(2m)
                                        .With_Weight(weight)
                                        .With_Value(value)
                                        .With_Currency(currency)
                                        .With_UnitName(unitName)
                                        .With_UnitsName(unitsname)
                                        .With_WrappingWeight(wrapWeight)
                                        .With_Materials(materialQuantity, DataFacade.Material.Take(x => x.MaterialID == materialId))
                                        .SaveToDb();

            var productExported = HelperFacade.BasicExportHelper.MapEntity(product, refNumber, 0);
            var prodContentExported = HelperFacade.BasicExportHelper.MapEntity(prodContent, refNumber, 0);

            //Product assertion
            Assert.Equal(refNumber, productExported["ProductCode"]);
            Assert.Equal(type, productExported["prodTypeCode"]);
            Assert.Equal(name, productExported["productDesc"]);
            Assert.Equal(articleCode, productExported["WP_ArticleCode"]);
            Assert.Equal(unitName, productExported["WP_UnitName"]);
            Assert.Equal(unitsname, productExported["WP_UnitsName"]);
            Assert.Equal(value, Convert.ToDecimal(productExported["WP_Value"]));
            Assert.Equal(currency, productExported["WP_CurrencyCode"]);

            //Product content assertion
            Assert.Equal(refNumber, prodContentExported["ProductCode"]);
            Assert.Equal(materialId, prodContentExported["materialID"]);
            Assert.Equal(materialQuantity.ToString(), prodContentExported["numItems"]);

        }

        [Fact(DisplayName = "Basic Export - Verify that when Product is updated Then System exports file with Act = 1")]
        public void VerifyThatProductIsExportedOnUpdating()
        {
            string type = "NOTE";
            decimal value = 100;
            decimal weight = 1;
            string currency = "USD";
            string unitName = "test unit name";
            string unitsname = "test units name";
            decimal wrapWeight = 2;
            string materialId = "314";
            int materialQuantity = 10;
            string articleCode = "article code";

            DataFacade.Product.New()
                                        .With_ProductCode(refNumber)
                                        .With_Description(name)
                                        .With_ArticleCode(articleCode)
                                        .With_Type(type)
                                        .With_Denomination(2m)
                                        .With_Weight(weight)
                                        .With_Value(value)
                                        .With_Currency(currency)
                                        .With_UnitName(unitName)
                                        .With_UnitsName(unitsname)
                                        .With_WrappingWeight(wrapWeight)
                                        .With_Materials(materialQuantity, DataFacade.Material.Take(x => x.MaterialID == materialId))
                                        .SaveToDb();

            DataFacade.Product.Take(refNumber).With_ArticleCode(refNumber).SaveToDb();

            var productExported = HelperFacade.BasicExportHelper.MapEntity(product, refNumber, 1);

            Assert.Equal(refNumber, productExported["WP_ArticleCode"]);
        }

        [Fact(DisplayName = "Basic Export - Verify that when Product is deleted Then System exports file with Act = 2")]
        public void VerifyThatProductIsExportedOnDeletion()
        {
            string type = "NOTE";
            decimal value = 100;
            decimal weight = 1;
            string currency = "USD";
            string unitName = "test unit name";
            string unitsname = "test units name";
            decimal wrapWeight = 2;
            string materialId = "314";
            int materialQuantity = 10;
            string articleCode = "article code";

            DataFacade.Product.New()
                              .With_ProductCode(refNumber)
                              .With_Description(name)
                              .With_ArticleCode(articleCode)
                              .With_Type(type)
                              .With_Denomination(2m)
                              .With_Weight(weight)
                              .With_Value(value)
                              .With_Currency(currency)
                              .With_UnitName(unitName)
                              .With_UnitsName(unitsname)
                              .With_WrappingWeight(wrapWeight)
                              .With_Materials(materialQuantity, DataFacade.Material.Take(x => x.MaterialID == materialId))
                              .SaveToDb();

            DataFacade.Product.Delete(p => p.ProductCode == refNumber);

            var productExported = HelperFacade.BasicExportHelper.MapEntity(product, refNumber, 2);
            var prodContentExported = HelperFacade.BasicExportHelper.MapEntity(prodContent, refNumber, 2);

            Assert.Equal(refNumber, productExported["ProductCode"]);
            Assert.Equal(refNumber, prodContentExported["ProductCode"]);
        }

        [Fact(DisplayName = "Basic Export - Verify that when MasterRoute is created Then System exports file with Act = 0")]
        public void VerifyThatMasterRouteIsExportedOnCreation()
        {
            try
            {
                var code = "1101";
                var location1 = DataFacade.Location.Take(l => l.Code == "SP01").Build();
                var dayName = Cwc.Common.Weekday.Wednesday;
                var dayNumber = Cwc.Routes.WeekdayNumber.Third;
                var planned = 10;
                var crew = 2;

                DataFacade.MasterRoute.New()
                                                .With_Code(code)
                                                .With_Number(refNumber)
                                                .With_Description(name)
                                                .With_CrewRequired(crew)
                                                .With_DistancePlanned(planned)
                                                .With_ReferenceNumber("referenceNumber")
                                                .With_WeekdayName(dayName)
                                                .With_WeekdayNumber(dayNumber)
                                                .With_DateCreated(DateTime.Now)
                                                .With_DateUpdated(DateTime.Now)
                                                .SaveToDb();

                DataFacade.MasterRouteStop.New()
                                                    .With_MasterRoute_id(DataFacade.MasterRoute.Take(x => x.Number == refNumber).Build().ID)
                                                    .With_Location_id(location1.ID)
                                                    .With_SequenceNumber(1)
                                                    .With_ArrivalTime(new TimeSpan(0, 0, 1))
                                                    .With_DepartureTime(new TimeSpan(0, 0, 1))
                                                    .With_DateCreated(DateTime.Now)
                                                    .With_DateUpdated(DateTime.Now).With_Author_id().With_Editor_id().With_OnSiteTime(new TimeSpan(0, 0, 1)).SaveToDb();


                var masterRouteExported = HelperFacade.BasicExportHelper.MapEntity(master, refNumber, 0);
                var masterRouteStopExported = HelperFacade.BasicExportHelper.MapEntity(masline, refNumber, 0);

                //master route assertion
                Assert.Equal(refNumber, masterRouteExported["mast_cd"]);
                Assert.Equal(((int)dayName + 1).ToString(), masterRouteExported["day_nr"]);
                Assert.Equal(crew.ToString(), masterRouteExported["one_mv"]);
                Assert.Equal(name, masterRouteExported["mas_rt_txt"]);
                Assert.Equal(planned.ToString(), masterRouteExported["p_km"]);

                //master route stop assertion
                Assert.Equal(((int)dayName + 1).ToString(), masterRouteStopExported["day_nr"]);
                Assert.Equal(location1.Code, masterRouteStopExported["ref_loc_nr"]);
                Assert.Equal(refNumber, masterRouteStopExported["mast_cd"]);
                Assert.Equal("1", masterRouteStopExported["seq_nr"]);
            }
            catch
            {
                throw;
            }
            finally
            {
                using (var context = new AutomationRoutesDataContext())
                {
                    context.MasterRoutes.RemoveRange(context.MasterRoutes.Where(mr => mr.Number == refNumber && mr.WeekdayNumber == WeekdayNumber.Third));
                    context.SaveChanges();
                }
            }
        }

        [Fact(DisplayName = "Basic Export - Verify that when MasterRoute is updated Then System exports file with Act = 1")]
        public void VerifyThatMasterRouteIsExportedOnUpdate()
        {
            try
            {
                var code = "1101";
                var location1 = DataFacade.Location.Take(l => l.Code == "SP01").Build();
                var dayName = Cwc.Common.Weekday.Wednesday;
                var dayNumber = Cwc.Routes.WeekdayNumber.Third;
                var planned = 10;
                var crew = 2;

                DataFacade.MasterRoute.New()
                                                .With_Code(code)
                                                .With_Number(refNumber)
                                                .With_Description(name)
                                                .With_CrewRequired(crew)
                                                .With_DistancePlanned(planned)
                                                .With_ReferenceNumber("referenceNumber")
                                                .With_WeekdayName(dayName)
                                                .With_WeekdayNumber(dayNumber)
                                                .With_DateCreated(DateTime.Now)
                                                .With_DateUpdated(DateTime.Now)
                                                .SaveToDb();

                var refNumberEdited = new Random().Next(1000, 99999).ToString();
                var plannedEdited = 3;
                var crewEdited = 3;

                DataFacade.MasterRoute.Take(r => r.Number == refNumber).With_CrewRequired(3).With_DistancePlanned(3).With_Number(refNumberEdited).SaveToDb();

                var masterRouteExported = HelperFacade.BasicExportHelper.MapEntity(master, refNumberEdited, 1);

                Assert.Equal(refNumberEdited, masterRouteExported["mast_cd"]);
                Assert.Equal(plannedEdited.ToString(), masterRouteExported["one_mv"]);
                Assert.Equal(crewEdited.ToString(), masterRouteExported["p_km"]);
            }
            catch
            {
                throw;
            }
            finally
            {
                using (var context = new AutomationRoutesDataContext())
                {
                    context.MasterRoutes.RemoveRange(context.MasterRoutes.Where(mr => mr.Number == refNumber && mr.WeekdayNumber == WeekdayNumber.Third));
                    context.SaveChanges();
                }
}
        }

        [Fact(DisplayName = "Basic Export - Verify that when MasterRoute is deleted Then System exports file with Act = 2")]
        public void VerifyThatMasterRouteIsExportedOnDelete()
        {
            try
            {
                var code = "1101";
                var location1 = DataFacade.Location.Take(l => l.Code == "SP01").Build();
                var dayName = Cwc.Common.Weekday.Wednesday;
                var dayNumber = Cwc.Routes.WeekdayNumber.Third;
                var planned = 10;
                var crew = 2;

                DataFacade.MasterRoute.New()
                                                .With_Code(code)
                                                .With_Number(refNumber)
                                                .With_Description(name)
                                                .With_CrewRequired(crew)
                                                .With_DistancePlanned(planned)
                                                .With_ReferenceNumber("referenceNumber")
                                                .With_WeekdayName(dayName)
                                                .With_WeekdayNumber(dayNumber)
                                                .With_DateCreated(DateTime.Now)
                                                .With_DateUpdated(DateTime.Now)
                                                .SaveToDb();

                DataFacade.MasterRoute.Delete(m => m.Number == refNumber);

                var masterRouteExported = HelperFacade.BasicExportHelper.MapEntity(master, refNumber, 2);

                Assert.Equal(refNumber, masterRouteExported["mast_cd"]);
            }
            catch
            {
                throw;
            }
            finally
            {
                using (var context = new AutomationRoutesDataContext())
                {
                    context.MasterRoutes.RemoveRange(context.MasterRoutes.Where(mr => mr.Number == refNumber && mr.WeekdayNumber == WeekdayNumber.Third));
                    context.SaveChanges();
                }
            }
        }

        [Fact(DisplayName = "Basic Export - Verify that when Location is created Then System exports file with Act = 0")]
        public void VerifyThatLocationIsExportedOnCreate()
        {
            var abbrev = "abbrev";
            decimal locationId;
            var customerCode = "1101";

            DataFacade.Location.New(out locationId)
                                             .With_Code(refNumber)
                                             .With_Name(name)
                                             .With_LtCode("533")
                                             .With_Abbreviation(abbrev)
                                             .With_HandlingType(BaseDataFacade.LocationNorType)
                                             .With_ServicingDepot(DataFacade.Site.Take(x => x.BranchType == Cwc.BaseData.BranchType.CITDepot))
                                             .With_Company(DataFacade.Customer.Take(c => c.ReferenceNumber == customerCode))
                                             .With_PreferredLanguage(1)
                                             .SaveToDb();

            var locationExported = HelperFacade.BasicExportHelper.MapEntity(location, refNumber, 0);

            Assert.Equal(locationId.ToString(), locationExported["loc_nr"]);
            Assert.Equal(refNumber, locationExported["ref_loc_nr"]);
            Assert.Equal(name, locationExported["name"]);
            Assert.Equal(abbrev, locationExported["abbrev"]);
            Assert.Equal(customerCode, locationExported["cus_nr"]);
            Assert.Equal(BaseDataFacade.LocationNorType, locationExported["lctyp_cd"]);
            Assert.Equal("533", locationExported["ltCode"]);
            Assert.Equal("true", locationExported["enabled"]);
        }

        [Fact(DisplayName = "Basic Export - Verify that when Location is edited Then System exports file with Act = 1")]
        public void VerifyThatLocationIsExportedOnUpdate()
        {
            var abbrev = "abbrev";
            decimal locationId;
            var customerCode = "1101";

            DataFacade.Location.New(out locationId)
                                             .With_Code(refNumber)
                                             .With_Name(name)
                                             .With_LtCode("NOR")
                                             .With_Abbreviation(abbrev)
                                             .With_HandlingType("NOR")
                                             .With_ServicingDepot(DataFacade.Site.Take(x => x.BranchType == Cwc.BaseData.BranchType.CITDepot))
                                             .With_Company(DataFacade.Customer.Take(c => c.ReferenceNumber == customerCode))
                                             .With_PreferredLanguage(1)
                                             .SaveToDb();

            abbrev = "edited";
            DataFacade.Location.Take(x => x.Code == refNumber).With_Abbreviation(abbrev).With_Enabled(false).SaveToDb();

            var locationExported = HelperFacade.BasicExportHelper.MapEntity(location, refNumber, 1);

            Assert.Equal(refNumber, locationExported["ref_loc_nr"]);
            Assert.Equal("false", locationExported["enabled"]);
            Assert.Equal(abbrev, locationExported["abbrev"]);
        }

        [Fact(DisplayName = "Basic Export - Verify that when Location is deleted Then System doesn't export file with Act = 2")]
        public void VerifyThatLocationIsExportedOnDelete()
        {
            var abbrev = "abbrev";
            decimal locationId;
            var customerCode = "1101";

            DataFacade.Location.New(out locationId)
                                             .With_Code(refNumber)
                                             .With_Name(name)
                                             .With_LtCode("533")
                                             .With_Abbreviation(abbrev)
                                             .With_HandlingType(BaseDataFacade.LocationNorType)
                                             .With_ServicingDepot(DataFacade.Site.Take(x => x.BranchType == Cwc.BaseData.BranchType.CITDepot))
                                             .With_Company(DataFacade.Customer.Take(c => c.ReferenceNumber == customerCode))
                                             .With_PreferredLanguage(1)
                                             .SaveToDb();

            DataFacade.Location.DeleteMany(l => l.Code == refNumber);

            Assert.Throws<ArgumentNullException>(() => HelperFacade.BasicExportHelper.MapEntity(location, refNumber, 2));
        }

        [Fact(DisplayName = "Basic Export - Verify that Systen exports location addresses properly")]
        public void VerifyThatLocationAddressIsExported()
        {
            var abbrev = "abbrev";
            decimal locationId;
            var customerCode = "1101";
            var country = "Ukraine";
            var city = "Kyiv";
            var postalCode = "1001";
            var street = "new street";
            var streeDet = "street details";



            DataFacade.Location.New(out locationId)
                                             .With_Code(refNumber)
                                             .With_Name(name)
                                             .With_LtCode("533")
                                             .With_Abbreviation(abbrev)
                                             .With_Address(DataFacade.Address.New()
                                                                            .With_Country(country)
                                                                            .With_City(city)
                                                                            .With_Street(street)
                                                                            .With_State("NotExported")
                                                                            .With_ExtraAddressInfo(streeDet)
                                                                            .With_PostalCode(postalCode)
                                                                            .With_Purpose(null))
                                             .With_HandlingType(BaseDataFacade.LocationNorType)
                                             .With_ServicingDepot(DataFacade.Site.Take(x => x.BranchType == Cwc.BaseData.BranchType.CITDepot))
                                             .With_Company(DataFacade.Customer.Take(c => c.ReferenceNumber == customerCode))
                                             .With_PreferredLanguage(1)
                                             .SaveToDb();

            var locationExported = HelperFacade.BasicExportHelper.MapEntity(location, refNumber, 0);

            Assert.Equal(refNumber, locationExported["ref_loc_nr"]);
            Assert.Equal(country, locationExported["county"]);
            Assert.Equal(city, locationExported["town_st"]);
            Assert.Equal(street, locationExported["street"]);
            Assert.Equal(streeDet, locationExported["street_det"]);
            Assert.Equal(postalCode, locationExported["area_pc"]);
        }
    }
}