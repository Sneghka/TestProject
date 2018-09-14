using Cwc.CashCenter;
using CWC.AutoTests.Model;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.ObjectBuilder.FeedingBuilder;
using CWC.AutoTests.Tests.Fixtures;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.Feeding
{

    public class StockContainerListTests : IClassFixture<StockContainerFixture>
    {
        StockContainerFixture _fixture;
        StockContainer expectedContainer;
        StockPosition expectedPosition;
        DateTime date;

        public StockContainerListTests(StockContainerFixture fixture)
        {
            _fixture = fixture;
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;
            date = new DateTime(year, month, day);
            expectedContainer = _fixture.StockContainer.Build();
            expectedPosition = _fixture.StockPosition.Build();
        }

        [Fact(DisplayName = "Stock Container List Feeding - When stock container feeding is valid Then System creates new stock container")]
        public void StockContainerFeedingWhenValidImportsSuccessfully()
        {
            using (var context = new CashCenterDataContext())
            {
                var convertedEntity = FeedingBuilderFacade.StockContainerListFeeding.New().
                    With_StockContainer("NOTE").
                    With_Number(expectedContainer.Number).
                    With_LocationFromCode(_fixture.Location.Code).
                    With_DateCollected(date).
                    With_StockPositionList().
                        With_StockPosition().
                            With_Value(expectedPosition.Value).
                            With_CurrencyCode(expectedPosition.Currency_id).
                            With_Weight(expectedPosition.Weight).
                            With_IsTotal(true).
                    Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(convertedEntity.ToString());

                var actualContainer = context.StockContainers.Where(sc => sc.Number == expectedContainer.Number 
                    && sc.LocationFrom_id == expectedContainer.LocationFrom_id
                    && sc.Type == expectedContainer.Type
                    && sc.PreannouncementType == expectedContainer.PreannouncementType
                    && sc.IsMisscan == expectedContainer.IsMisscan
                    && sc.SiteCode == expectedContainer.SiteCode).FirstOrDefault();
                Assert.True(actualContainer != null, "StockContainer with provided criteria wasn't found");

                var actualPosition = context.StockPositions.Where(sp => sp.ID == expectedPosition.ID && sp.StockContainer_id == expectedContainer.ID).First();
                Assert.True(actualPosition != null, "StockPosition with provided criteria wasn't found");
                using (var _context = new AutomationFeedingDataContext())
                {
                    var message = _context.ValidatedFeedingLogs.OrderByDescending(f => f.DateCreated).FirstOrDefault(fm => fm.Message.Contains(expectedContainer.Number));
                    Assert.Equal($"Stock Container (Number '{expectedContainer.Number}', Location from = {expectedContainer.LocationFrom.Code}) has been successfully created.", message.Message);
                }
                
                Assert.True(expectedContainer.ID == actualContainer.ID, "Actual Stock Container doesn't match Expected Stock Container");
                Assert.True(expectedPosition.Value == actualPosition.Value, "Actual Stock Position doesn't match Expected Stock Position");
                Assert.True(expectedPosition.Currency_id == actualPosition.Currency_id, "Actual Stock Position doesn't match Expected Stock Position");
                Assert.True(expectedPosition.Weight == actualPosition.Weight, "Actual Stock Position doesn't match Expected Stock Position");
            }
        }

        //[Fact(DisplayName = "When stock container feeding is valid Then System sets Pre-announcement Type = 'customer electronic'")]
        //public void StockContainerFeedingValidPreAnnouncementTypeCorrect()
        //{
        //    using (var context = new CashCenterDataContext())
        //    {
        //        var year = DateTime.Now.Year;
        //        var month = DateTime.Now.Month;
        //        var day = DateTime.Now.Day;
        //        var date = new DateTime(year, month, day);
        //        var expectedContainer = _fixture.StockContainer.Build();
        //        var expectedPosition = _fixture.StockPosition.Build();

        //        var convertedEntity = FeedingBuilderFacade.StockContainerListFeeding.New().
        //            With_StockContainer().
        //            With_Number(expectedContainer.Number).
        //            With_LocationFromCode(/*_fixture.Location.Code*/"ELF01").
        //            With_DateCollected(date.ToString("YYYY-MM-DD")).
        //            With_StockPositionList().
        //                With_StockPosition(expectedPosition.Value,
        //                                    expectedPosition.Currency_id,
        //                                    expectedPosition.Weight, true).
        //                Build();

        //        var response = HelperFacade.FeedingHelper.SendFeeding(convertedEntity.ToString());

        //        var actualContainer = context.StockContainers.Where(sc => sc.Number == expectedContainer.Number && sc.LocationCode == "ELF01");
        //        Assert.True(actualContainer.Any(), "StockContainer with provided criteria wasn't found");

        //        var actualPosition = context.StockPositions.Where(sp => sp.ID == expectedPosition.ID && sp.StockContainer_id == expectedContainer.ID);
        //        Assert.True(actualPosition.Any(), "StockPosition with provided criteria wasn't found");

        //        var message = _context.Cwc_Feedings_ValidatedFeedingLogs.OrderByDescending(f => f.DateCreated).FirstOrDefault(fm => fm.Message.Contains(expectedContainer.Number));
        //        Assert.Equal($"Stock Container (Number ‘{expectedContainer.Number}’, Location from = ‘{expectedContainer.LocationFrom.Code}’) has been successfully created.", message.Message);

        //        Assert.True(expectedContainer == actualContainer.FirstOrDefault(), "Actual Stock Container doesn't match Expected Stock Container");
        //        Assert.True(expectedPosition == actualPosition.FirstOrDefault(), "Actual Stock Position doesn't match Expected Stock Position");
        //    }
        //}

        [Fact(DisplayName = "Stock Container List Feeding - When stock container feeding is valid Then System updates stock container")]
        public void StockContainerFeedingWhenValidImportUpdatedSuccessfully()
        {
            using (var context = new CashCenterDataContext())
            {
                //Send first feeding
                var convertedFirstEntity = FeedingBuilderFacade.StockContainerListFeeding.New().
                    With_StockContainer("NOTE").
                    With_Number(expectedContainer.Number).
                    With_LocationFromCode(_fixture.Location.Code).
                    With_DateCollected(date).
                    With_StockPositionList().
                        With_StockPosition().
                            With_Value(expectedPosition.Value).
                            With_CurrencyCode(expectedPosition.Currency_id).
                            With_Weight(expectedPosition.Weight).
                            With_IsTotal(true).
                    Build();

                var responseForFirstEntity = HelperFacade.FeedingHelper.SendFeeding(convertedFirstEntity.ToString());

                var createdContainer = context.StockContainers.Where(sc => sc.Number == expectedContainer.Number
                    && sc.LocationFrom_id == expectedContainer.LocationFrom_id
                    && sc.Type == expectedContainer.Type
                    && sc.PreannouncementType == expectedContainer.PreannouncementType
                    && sc.IsMisscan == expectedContainer.IsMisscan
                    && sc.SiteCode == expectedContainer.SiteCode).FirstOrDefault();

                Assert.True(createdContainer != null, "StockContainer with provided criteria wasn't found");

                //Send second feeding
                var convertedSecondEntity = FeedingBuilderFacade.StockContainerListFeeding.New().
                    With_StockContainer("NOTE").
                    With_Number(expectedContainer.Number).
                    With_LocationFromCode(_fixture.Location.Code).
                    With_DateCollected(date).
                    With_StockPositionList().
                        With_StockPosition().
                            With_Value(expectedPosition.Value).
                            With_CurrencyCode("GBP").
                            With_Weight(expectedPosition.Weight).
                            With_IsTotal(true).
                    Build();

                var responseForSecondEntity = HelperFacade.FeedingHelper.SendFeeding(convertedSecondEntity.ToString());

                var UpdatedContainer = context.StockContainers.Where(sc => sc.Number == expectedContainer.Number
                    && sc.LocationFrom_id == expectedContainer.LocationFrom_id
                    && sc.Type == expectedContainer.Type
                    && sc.PreannouncementType == expectedContainer.PreannouncementType
                    && sc.IsMisscan == expectedContainer.IsMisscan
                    && sc.SiteCode == expectedContainer.SiteCode).FirstOrDefault();
                Assert.True(UpdatedContainer != null, "StockContainer with provided criteria wasn't found");

                var actualPosition = context.StockPositions.Where(sp => sp.ID == expectedPosition.ID && sp.StockContainer_id == expectedContainer.ID).FirstOrDefault();
                Assert.True(actualPosition != null, "StockPosition with provided criteria wasn't found");

                using (var _context = new AutomationFeedingDataContext())
                {
                    var message = _context.ValidatedFeedingLogs.OrderByDescending(f => f.DateCreated).FirstOrDefault(fm => fm.Message.Contains(expectedContainer.Number));
                    Assert.Equal($"Stock Container (Number '{expectedContainer.Number}', Location from = {expectedContainer.LocationFrom.Code}) has been successfully updated.", message.Message);
                }
                Assert.True(expectedPosition.Currency_id == actualPosition.Currency_id, "Actual Stock Position doesn't match Expected Stock Position");
            }
        }
    }
}
