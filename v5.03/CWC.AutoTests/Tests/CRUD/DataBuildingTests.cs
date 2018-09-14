//using Cwc.BaseData;
//using CWC.AutoTests.ObjectBuilder;
//using Xunit;
//using System.Transactions;

//namespace CWC.AutoTests.Tests.AutoTests
//{
//    public class DataBuildingTests
//    {
//        [Fact(DisplayName = "Create new site with new location and existed LocationType, with new customer")]
//        public void CreateSite_WithNewLocation_WithNewCustomer()
//        {
//            DataFacade.Site.New()
//                .With_Branch_cd("13012")
//                .With_Description("Podles created")
//                .With_BranchType(Cwc.BaseData.BranchType.CashCenter)
//                .With_SubType(Cwc.BaseData.BranchSubType.Notes)
//                .With_WP_IsExternal(true)
//                .With_Location(DataFacade.Location.New()
//                                    .With_Code("1300312")
//                                    .With_Name("podles loc")
//                                    .With_LocationTypeID(DataFacade.LocationType.Take(x => x.ltCode == "733"))
//                                    .With_HandlingType("DEP")
//                                    .With_Abbreviation("abbrev")
//                                    .With_PreferredLanguage(1)
//                                    .With_Company(DataFacade.Customer.New()
//                                                        .With_ReferenceNumber("1300312")
//                                                        .With_Code("1300312")
//                                                        .With_Name("podles company")
//                                                        .With_RecordType(Cwc.Security.CustomerRecordType.Company)
//                                                        .With_Abbrev("podles comp")
//                                                        .With_Enabled(false).SaveToDb())
//                                   .SaveToDb())
//                .SaveToDb();

           
//        }

//        [Fact(DisplayName = "Create new site with existed location")]
//        public void CreateSite_WithExistedLocation()
//        {
//            Site site = DataFacade.Site.New()
//               .With_Branch_cd("PBrah01")
//               .With_Description("Podles created")
//               .With_BranchType(Cwc.BaseData.BranchType.CashCenter)
//               .With_SubType(Cwc.BaseData.BranchSubType.Notes)
//               .With_WP_IsExternal(false)
//               .With_Location(DataFacade.Location.Take(x => x.Code == "320032"));
//        }


//        [Fact(DisplayName = "Modified builder test for base data")]
//        public void UpdBuilderBaseData()
//        {
//            using (new TransactionScope())
//            {
//                var site = DataFacade.Site.InitDefault().SaveToDb();

//                var location = site.locationBuilder.With_HandlingType("NOR").SaveToDb().Build();
//                var customer = site.locationBuilder.customerBuilder.With_Abbrev("some mew value").SaveToDb().Build();

//                var locationFromDb = DataFacade.Location.Take(x => x.Code == location.Code).Build();
//                var customerFromDb = DataFacade.Customer.Take(x => x.ReferenceNumber == customer.ReferenceNumber).Build();

//                Assert.Equal("some new value", site.locationBuilder.customerBuilder.Build().Abbrev);
//                Assert.Equal("some new value", customerFromDb.Abbrev);
//                Assert.Equal("NOR", site.locationBuilder.Build().HandlingType);
//                Assert.Equal("NOR", locationFromDb.HandlingType);
//            }           
//        }
//    }
//}
