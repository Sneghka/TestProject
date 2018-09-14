using Cwc.BaseData;
using Cwc.BaseData.Enums;
using Cwc.Contracts;
using Cwc.Feedings;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.FeedingBuilder;
using CWC.AutoTests.Tests.Fixtures;
using System;
using System.Globalization;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.Feeding
{
    public class LocationServicingTimeWindowFeedingTest: IClassFixture<BaseDataFixture>, IDisposable
    {

        AutomationFeedingDataContext feedingContext;
        BaseDataFixture _fixture;
        private const string handlingType = "NOR";
        private const string name = "ServiceTimeWindowTest";
        private const string serviceTypeColl = "COLL";
        private const string orderTypeAtRequest = "At-request";
        private const int incorrectValue = 1;
        private static string timeFrom = DateTime.Now.ToString("t", CultureInfo.CreateSpecificCulture("hr-HR"));
        private static string timeTo = DateTime.Now.AddHours(1).ToString("t", CultureInfo.CreateSpecificCulture("hr-HR"));
        private static string refNumber = $"1101{new Random().Next(10000, 99999999)}";


        public LocationServicingTimeWindowFeedingTest(BaseDataFixture fixture)
        {
            feedingContext = new AutomationFeedingDataContext();
            _fixture = fixture;
        }

        [Fact(DisplayName = "Servicing Time Window - When Location is submitted with Servicing Time Window Then System creates it successfully")]
        public void VerifyThatServicingTimeWindowCanBeCreated()
        {
            var servicingTimeWindow = FeedingBuilderFacade.LocationServicingTimeWindowFeeding.New().
                With_Code(name).
                With_Name(name).
                With_IsEnabled(true).
                With_CompanyNumber(_fixture.Customer.ReferenceNumber).
                With_LocationTypeCode(_fixture.LocationType.ltCode).
                With_HandlingType(handlingType).
                With_DefaultCITSite(_fixture.SiteCIT.Branch_cd).
                With_LocationServiceTimeWindows().
                    With_Servicetype(serviceTypeColl).
                    With_Ordertype(orderTypeAtRequest).
                    With_TimeFrom(timeFrom).
                    With_TimeTo(timeTo).
                    With_Weekday(1).
                Build();

            var response = HelperFacade.FeedingHelper.SendFeeding(servicingTimeWindow.ToString());

            var message = $"Location with Code ‘{name}’ has been successfully created.";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message) && m.Action == ValidatedFeedingActionType.Create).Any());
        }

        [Fact(DisplayName = "Servicing Time Window - When Location is submitted with Servicing Time Window Then System Updated it successfully")]
        public void VerifyThatServicingTimeWindowCanBeUpdated()
        {
            var servicingTimeWindow = FeedingBuilderFacade.LocationServicingTimeWindowFeeding.New().
                With_Code(name).
                With_Name(name).
                With_IsEnabled(true).
                With_CompanyNumber(_fixture.Customer.ReferenceNumber).
                With_LocationTypeCode(_fixture.LocationType.ltCode).
                With_HandlingType(handlingType).
                With_DefaultCITSite(_fixture.SiteCIT.Branch_cd).
                With_LocationServiceTimeWindows().
                    With_Servicetype(serviceTypeColl).
                    With_Ordertype(orderTypeAtRequest).
                    With_TimeFrom(timeFrom).
                    With_TimeTo(timeTo).
                    With_Weekday(1).
                Build();
            
            var response = HelperFacade.FeedingHelper.SendFeeding(servicingTimeWindow.ToString());

            servicingTimeWindow = FeedingBuilderFacade.LocationServicingTimeWindowFeeding.New().
                With_Code(name).
                With_Name(name).
                With_IsEnabled(true).
                With_CompanyNumber(_fixture.Customer.ReferenceNumber).
                With_LocationTypeCode(_fixture.LocationType.ltCode).
                With_HandlingType(handlingType).
                With_DefaultCITSite(_fixture.SiteCIT.Branch_cd).
                With_LocationServiceTimeWindows().
                   With_Servicetype(serviceTypeColl).
                   With_Ordertype(orderTypeAtRequest).
                   With_TimeFrom(timeFrom).
                   With_TimeTo(timeTo).
                   With_Weekday(1).
                Build();

            response = HelperFacade.FeedingHelper.SendFeeding(servicingTimeWindow.ToString());
            var message = $"Location with Code ‘{name}’ has been successfully updated.";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message) && m.Action == ValidatedFeedingActionType.Update).Any());
        }


        [Fact(DisplayName = "Servicing Time Window - When Location is submitted with Servicing Time Window and TimeFrom mapped with error Then System shows an error")]
        public void VerifyThatServicingTimeWindowCanNotBeCreatedWithIncorrectTimeFrom()
        {
            var servicingTimeWindow = FeedingBuilderFacade.LocationServicingTimeWindowFeeding.New().
                With_Code(name).
                With_Name(name).
                With_IsEnabled(true).
                With_CompanyNumber(_fixture.Customer.ReferenceNumber).
                With_LocationTypeCode(_fixture.LocationType.ltCode).
                With_HandlingType(handlingType).
                With_DefaultCITSite(_fixture.SiteCIT.Branch_cd).
                With_LocationServiceTimeWindows().
                    With_Servicetype(serviceTypeColl).
                    With_Ordertype(orderTypeAtRequest).
                    With_TimeFrom(incorrectValue.ToString()).
                    With_TimeTo(timeTo).
                    With_Weekday(1).
                Build();

            var response = HelperFacade.FeedingHelper.SendFeeding(servicingTimeWindow.ToString());

            var message = $"Error for Location with Code = '{name}' (entity # 1): Attribute 'TimeFrom' format is broken, unable to parse value '{incorrectValue}'.";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message)).Any());
        }

        [Fact(DisplayName = "Servicing Time Window - When Location is submitted with Servicing Time Window and TimeTo mapped with error Then System shows an error")]
        public void VerifyThatServicingTimeWindowCanNotBeCreatedWithIncorrectTimeTo()
        {
            var servicingTimeWindow = FeedingBuilderFacade.LocationServicingTimeWindowFeeding.New().
                With_Code(name).
                With_Name(name).
                With_IsEnabled(true).
                With_CompanyNumber(_fixture.Customer.ReferenceNumber).
                With_LocationTypeCode(_fixture.LocationType.ltCode).
                With_HandlingType(handlingType).
                With_DefaultCITSite(_fixture.SiteCIT.Branch_cd).
                With_LocationServiceTimeWindows().
                    With_Servicetype(serviceTypeColl).
                    With_Ordertype(orderTypeAtRequest).
                    With_TimeFrom(timeFrom).
                    With_TimeTo(incorrectValue.ToString()).
                    With_Weekday(1).
                Build();

            var response = HelperFacade.FeedingHelper.SendFeeding(servicingTimeWindow.ToString());

            var message = $"Error for Location with Code = '{name}' (entity # 1): Attribute 'TimeTo' format is broken, unable to parse value '{incorrectValue}'.";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message)).Any());
        }

        [Fact(DisplayName = "Servicing Time Window - When Location is submitted with Servicing Time Window and TimeTo mapped with error Then System shows an error")]
        public void VerifyThatServicingTimeWindowCanNotBeCreatedWithTimeFromLessThanTimeTo()
        {
            var servicingTimeWindow = FeedingBuilderFacade.LocationServicingTimeWindowFeeding.New().
                With_Code(name).
                With_Name(name).
                With_IsEnabled(true).
                With_CompanyNumber(_fixture.Customer.ReferenceNumber).
                With_LocationTypeCode(_fixture.LocationType.ltCode).
                With_HandlingType(handlingType).
                With_DefaultCITSite(_fixture.SiteCIT.Branch_cd).
                With_LocationServiceTimeWindows().
                    With_Servicetype(serviceTypeColl).
                    With_Ordertype(orderTypeAtRequest).
                    With_TimeFrom(timeTo).
                    With_TimeTo(timeFrom).
                    With_Weekday(1).
                Build();

            var response = HelperFacade.FeedingHelper.SendFeeding(servicingTimeWindow.ToString());

            var message = $"Error for Location with Code ‘{name}’: Time To should be greater than Time From for weekday Tuesday, service type 1; order type AtRequest.";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message)).Any());
        }


        public void Dispose()
        {
            using (var _context = new AutomationTransportDataContext())
            {
                var locationList = _context.Locations.Where(l=>l.Name.StartsWith(name)).ToList();
                
                feedingContext.ValidatedFeedingLogs.RemoveRange(feedingContext.ValidatedFeedingLogs);
                feedingContext.ValidatedFeedings.RemoveRange(feedingContext.ValidatedFeedings);
                feedingContext.SaveChanges();
                _context.BaseAddresses.RemoveRange(_context.BaseAddresses);
                _context.CitProcessSettingLinks.RemoveRange(_context.CitProcessSettingLinks);
                foreach (var location in locationList)
                {
                    var citProcessSetting = _context.CitProcessSettings.Where(c => c.ServicePointID == location.ID).FirstOrDefault();
                    if (citProcessSetting != null)
                    {
                        _context.CitProcessSettingServicingTimeWindows.RemoveRange(_context.CitProcessSettingServicingTimeWindows.Where(st => st.CitProcessSettingID == citProcessSetting.ID));
                        _context.CitProcessSettingStopDurations.RemoveRange(_context.CitProcessSettingStopDurations.Where(s => s.CitProcessSettingID == citProcessSetting.ID));
                        _context.CitProcessSettingReplenishmentConfigurations.RemoveRange(_context.CitProcessSettingReplenishmentConfigurations.Where(r => r.CitProcessSettingID == citProcessSetting.ID));
                        _context.CitProcessSettings.RemoveRange(_context.CitProcessSettings.Where(l => l.ServicePointID == location.ID));
                    }
                    _context.Locations.RemoveRange(_context.Locations.Where(l => l.Name.StartsWith(name) && l.Level == LocationLevel.ServicePoint));
                    _context.SaveChanges();
                }
                feedingContext.Dispose();                
            }
        }
    }
}
