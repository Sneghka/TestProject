using Cwc.Routes;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace CWC.AutoTests.Tests.Feeding
{
    public class RouteListFeedingTests : IClassFixture<RouteListFixture>, IDisposable
    {
        RouteListFixture _fixture;
        MasterRouteBuilder routeBuilder;
        static MasterRoute expectedRoute;
        List<MasterRouteStopBuilder> masterRouteStopsList = new List<MasterRouteStopBuilder>();
        MasterRouteStopBuilder expectedRouteStop1;
        MasterRouteStopBuilder expectedRouteStop2;
        string code, description, referenceNumber;
        int crewRequired;
        decimal distancePlanned;
        string successCreateMessage = "Master Route with Code '{0} for branch SP' has been successfully created";
        string successUpdateMessage = "Master Route with Code '{0} for branch SP' has been successfully updated";
        string errorAbsenceMandatoryAttributeCodeMessage = "Error for Master Route with Code = '{0}', CITSiteCode = '{1}' (entity # 1): Mandatory attribute 'Code' is not submitted.";
        string errorAbsenceMandatoryAttributeCitSiteCodeMessage = "Error for Master Route with Code = '{0}', CITSiteCode = '' (entity # 1): Mandatory attribute 'CITSiteCode' is not submitted.";
        string errorInvalidCitSiteCodeMessage = "Error for Master Route with Code = '{0}', CITSiteCode = '{1}' (entity # 1): Location with provided CITSiteCode ‘{1}’ does not exist.";
        string errorAbsenceMandatoryAttributeWeekdayMessage = "Error for Master Route with Code = '{0}', CITSiteCode = '{1}' (entity # 1): Mandatory attribute 'Weekday' is not submitted.";
        string errorAbsenceMandatoryAttributeLocationMessage = "Error for Master Route with Code = '{0}', CITSiteCode = '{1}' (entity # 1): Mandatory attribute 'LocationCode' is not submitted.";
        string errorAbsenceMandatoryAttributreSequenceNumber = "Error for Master Route with Code = '{0}', CITSiteCode = '{1}' (entity # 1): Mandatory attribute 'SequenceNumber' is not submitted.";
        string errorAbsenceStopWithSequenceNumber1Message = "Error for Master Route with Code '{0}', Branch '{1}': List of stops does not contain consistent list of Sequence Number starting from 1.";
        string errorLocationLevelIsVisitAddressMessage = "Error for Master Route with Code '{0}', Branch '{1}': It is not allowed to create stop at address.";
        string errorNotSynchronizedStopTimesMessage = "Error for Master Route with Code '{0}', Branch '{1}': Departure time does not correspond to arrival time and on site time for stop(s): {2}";
        static int routeId;
        static decimal locationIdOne;
        static decimal locationIdTwo;
        Expression<Func<MasterRouteStop, bool>> actualRouteStopOneExpression = x => x.MasterRoute_id == routeId && x.Location_id == locationIdOne;
        Expression<Func<MasterRouteStop, bool>> actualRouteStopTwoExpression = x => x.MasterRoute_id == routeId && x.Location_id == locationIdTwo;
        Expression<Func<MasterRoute, bool>> actualMasterRouteExpression = x => x.Code == expectedRoute.Code && x.Branch_id == expectedRoute.Branch_id;

        public RouteListFeedingTests(RouteListFixture fixture)
        {
            _fixture = fixture;
            crewRequired = new Random().Next(1, 4);
            distancePlanned = new Random().Next(10, 40);
            code = Utils.ValueGenerator.GenerateString("PMR", 7);
            description = Utils.ValueGenerator.GenerateString("ATM", 7);
            referenceNumber = Utils.ValueGenerator.GenerateString("ATM", 7);

            expectedRouteStop1 = DataFacade.MasterRouteStop.New() // create first Master Route Stop
                                .With_Location_id(_fixture.Location1.ID)
                                .With_SequenceNumber(1)
                                .With_ArrivalTime(_fixture.StopArrivalTime)
                                .With_OnSiteTime(_fixture.StopOnSIteTime)
                                .With_DepartureTime(_fixture.DepartureTime);

            expectedRouteStop2 = DataFacade.MasterRouteStop.New() // create second Master Route Stop
                               .With_Location_id(_fixture.Location2.ID)
                               .With_SequenceNumber(2)
                               .With_ArrivalTime(_fixture.DepartureTime.Add(_fixture.TravelTime))
                               .With_OnSiteTime(_fixture.StopOnSIteTime)
                               .With_DepartureTime(_fixture.DepartureTime.Add(_fixture.TravelTime).Add(_fixture.StopOnSIteTime));

            routeBuilder = DataFacade.MasterRoute.New() // create Master Route without Stops. Add in test if needed. 
                .With_Code(code)
                .With_Branch(_fixture.Site)
                .With_Branch_id(_fixture.Site)
                .With_CrewRequired(crewRequired)
                .With_DistancePlanned(distancePlanned)
                .With_Description(description)
                .With_ReferenceNumber(referenceNumber);

            locationIdOne = _fixture.Location1.ID;
            locationIdTwo = _fixture.Location2.ID;

        }

        [Fact(DisplayName = "Master Route Feeding - When doesn't exists another master route with equals {Code,Branch,Weekday} and Master Route is valid Then System creates it")]
        public void VerifyThatMasterRouteCanBeCreatedWhenAnotherIsNotExists()
        {
            routeBuilder.With_MasterRouteStops(expectedRouteStop1, expectedRouteStop2);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            Assert.Equal(String.Format(successCreateMessage, code), HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code));

            var actualRoute = DataFacade.MasterRoute.Take(x => x.Code == code).Build();
            Assert.Equal(routeBuilder.Build(), actualRoute, new MasterRouteComparer());

            routeId = actualRoute.ID;
            var actualRouteStop1 = DataFacade.MasterRouteStop.Take(actualRouteStopOneExpression).Build();
            Assert.Equal(expectedRouteStop1.Build(), actualRouteStop1, new MasterRouteStopComparer());

            var actualRouteStop2 = DataFacade.MasterRouteStop.Take(actualRouteStopTwoExpression).Build();
            Assert.Equal(expectedRouteStop2.Build(), actualRouteStop2, new MasterRouteStopComparer());
        }

        [Fact(DisplayName = "Master Route Feeding - When exists another master route with equal {Code, Branch, WeekdayNumber} Then System updates this master route")]
        public void VerifyThatMasterRouteCanBeUpdatedWhenExistMasterRouteWithTheSameCodeBranchWeekday()
        {
            routeBuilder.With_MasterRouteStops(expectedRouteStop1, expectedRouteStop2);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            var actualRoute = DataFacade.MasterRoute.Take(x => x.Code == code).Build();
            Assert.Equal(routeBuilder.Build(), actualRoute, new MasterRouteComparer());

            var expectedRouteNew = routeBuilder
                .With_CrewRequired(crewRequired + 1)
                .With_Description(description + " NEW")
                .With_DistancePlanned(distancePlanned + 15)
                .With_ReferenceNumber(referenceNumber + " NEW");
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", expectedRouteNew);
            Assert.Equal(String.Format(successUpdateMessage, code), HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code));

            var actualRouteNew = DataFacade.MasterRoute.Take(x => x.Code == code).Build();
            Assert.Equal(expectedRouteNew.Build(), actualRouteNew, new MasterRouteComparer());
        }
        [Fact(DisplayName = "Master Route Feeding - When exists another master route with equal {Code, Branch, WeekdayNumber} Then System updates this master route")]
        public void VerifyThatSystemDoesNotUpdateMasterRouteBranchForExistingMasterRouteButCreatesNewMasterRoute()
        {
            routeBuilder.With_MasterRouteStops(expectedRouteStop1, expectedRouteStop2);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            var actualRoute = DataFacade.MasterRoute.Take(x => x.Code == code).Build();
            Assert.Equal(routeBuilder.Build(), actualRoute, new MasterRouteComparer());

            var newBranch = DataFacade.Site.Take(x => x.Branch_cd == "878");
            string newSuccessCreateMessage = "Master Route with Code '{0} for branch 878' has been successfully created";

            var expectedRouteNew = routeBuilder
                .With_Branch(newBranch)
                .With_Branch_id(newBranch);

            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", expectedRouteNew);
            Assert.Equal(String.Format(newSuccessCreateMessage, code), HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code));

            var actualRouteNew = DataFacade.MasterRoute.Take(x => x.Code == code).Build();
            Assert.Equal(expectedRouteNew.Build(), actualRouteNew, new MasterRouteComparer());
        }


        [Fact(DisplayName = "Master Route Feeding - When master route is updated Then System deletes all it's stops")]
        public void VerifyWhenMasterRouteIsUpdatedThenSystemDeletesAllPrevStops()
        {
            routeBuilder.With_MasterRouteStops(expectedRouteStop1, expectedRouteStop2);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            var actualRoute = DataFacade.MasterRoute.Take(x => x.Code == code).Build();
            Assert.Equal(routeBuilder.Build(), actualRoute, new MasterRouteComparer());
            routeId = actualRoute.ID;

            var actualRouteStop1 = DataFacade.MasterRouteStop.Take(actualRouteStopOneExpression).Build();
            Assert.Equal(expectedRouteStop1.Build(), actualRouteStop1, new MasterRouteStopComparer());

            var actualRouteStop2 = DataFacade.MasterRouteStop.Take(actualRouteStopTwoExpression).Build();
            Assert.Equal(expectedRouteStop2.Build(), actualRouteStop2, new MasterRouteStopComparer());

            var newLocation = DataFacade.Location.Take(x => x.Code == "JG02").Build();
            var expectedRouteStopNew = DataFacade.MasterRouteStop.New() // create new first Master Route Stop
                                   .With_Location_id(newLocation.ID)
                                   .With_SequenceNumber(1)
                                   .With_ArrivalTime(_fixture.StopArrivalTime)
                                   .With_OnSiteTime(_fixture.StopOnSIteTime)
                                   .With_DepartureTime(_fixture.DepartureTime);
            var expectedRouteNew = routeBuilder.With_MasterRouteStops(expectedRouteStopNew);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", expectedRouteNew);
            Assert.Equal(String.Format(successUpdateMessage, code), HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code));

            var actualRouteNew = DataFacade.MasterRoute.Take(x => x.Code == code).Build();
            Assert.Equal(expectedRouteNew.Build(), actualRouteNew, new MasterRouteComparer());

            routeId = actualRouteNew.ID;
            locationIdOne = newLocation.ID;
            var actualRouteStopNew = DataFacade.MasterRouteStop.Take(actualRouteStopOneExpression).Build();
            Assert.Equal(expectedRouteStopNew.Build(), actualRouteStopNew, new MasterRouteStopComparer());
            Assert.True(HelperFacade.MasterRouteHelper.CountStops(x => x.MasterRoute_id == routeId) == 1);
        }

        [Fact(DisplayName = "Master Route Feeding - When Code is not specified Then System doesn't allow to create Master Route")]
        public void VerifyMasterRouteCannotBecreatedWithoutMandCodeField()
        {
            routeBuilder.With_MasterRouteStops(expectedRouteStop1);
            code = "";
            routeBuilder.With_Code(code);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("Code", routeBuilder);
            expectedRoute = routeBuilder.Build();
            var expectedMessage = String.Format(errorAbsenceMandatoryAttributeCodeMessage, code, _fixture.Site.Branch_cd);
            Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code), true, true, true);
            Assert.False(DataFacade.MasterRoute.IsMasterRouteExist(actualMasterRouteExpression), "System should not allow to import feeding wihtout mandatory Code field");
        }

        [Fact(DisplayName = "Master Route Feeding - When Cit Site Code is not specified Then System doesn't allow to create Master Route")]
        public void VerifyMasterRouteCannotBecreatedWithoutMandCitSitCodeField()
        {
            routeBuilder.With_MasterRouteStops(expectedRouteStop1);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("CITSiteCode", routeBuilder);
            expectedRoute = routeBuilder.Build();
            var expectedMessage = String.Format(errorAbsenceMandatoryAttributeCitSiteCodeMessage, code);
            Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code), true, true, true);
            Assert.False(DataFacade.MasterRoute.IsMasterRouteExist(actualMasterRouteExpression), "System should not allow to import feeding wihtout mandatory SiteCode field");
        }

        [Fact(DisplayName = "Master Route Feeding - When WeekdayNumber is not specified Then System doesn't allow to create Master Route")]
        public void VerifyMasterRouteCannotBecreatedWithoutMandWeekdayNumber()
        {
            routeBuilder.With_MasterRouteStops(expectedRouteStop1);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("Weekday", routeBuilder);
            expectedRoute = routeBuilder.Build();
            var expectedMessage = String.Format(errorAbsenceMandatoryAttributeWeekdayMessage, code, _fixture.Site.Branch_cd);
            Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code), true, true, true);
            Assert.False(DataFacade.MasterRoute.IsMasterRouteExist(actualMasterRouteExpression), "System should not allow to import feeding wihtout mandatory SiteCode field");
        }

        [Fact(DisplayName = "Master Route Feeding - When Number contains more then 10 characters Then System doesn't allow to create Master Route")]
        public void VerifyMasterRouteCannotBecreatedWhenNumberContainsMoreThen10Characters()
        {
            routeBuilder.With_MasterRouteStops(expectedRouteStop1);
            var codeNew = "ABCDEFGHIJK";
            routeBuilder.With_Code(codeNew);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            expectedRoute = routeBuilder.Build();
            var branchCodeLength = _fixture.Site.Branch_cd.Length;
            var expectedMessage = $"Error for Master Route with Code '{codeNew}', Branch '{_fixture.Site.Branch_cd}': Branch code is <{branchCodeLength}> characters, so Route Number cannot be longer than <{10 - branchCodeLength}> characters";
            Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(codeNew), true, true, true);
            Assert.False(DataFacade.MasterRoute.IsMasterRouteExist(actualMasterRouteExpression), "System should not allow to import feeding wiht Branch + Code > 10");
        }

        [Fact(DisplayName = "Master Route Feeding - When doesn't exist master route stop with sequence number 1 Then System doesn't allow create Master Route and Stops")]
        public void VerifyThatMasterRouteCanBeCreatedWhenStopWithSequenceNumberOneIsAbsent()
        {
            expectedRouteStop1.With_SequenceNumber(2);
            expectedRouteStop2.With_SequenceNumber(3);
            routeBuilder.With_MasterRouteStops(expectedRouteStop1, expectedRouteStop2);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            var expectedMessage = String.Format(errorAbsenceStopWithSequenceNumber1Message, code, _fixture.Site.Branch_cd);
            Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code), true, true, true);

            expectedRoute = routeBuilder.Build();
            Assert.False(DataFacade.MasterRoute.IsMasterRouteExist(actualMasterRouteExpression), "System should not allow to import feeding wihtout Stop with sequence number 1.");
        }

        [Fact(DisplayName = "Master Route Feeding - When at least one LocaitonCode is not specified Then System doesn't allow to create Master Route Stops and master Route")]
        public void VerifyMasterRouteStopCannotBeCreatedWithoutMandLocationFields()
        {
            expectedRouteStop1.Without_Location();
            routeBuilder.With_MasterRouteStops(expectedRouteStop1);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            expectedRoute = routeBuilder.Build();
            var expectedMessage = String.Format(errorAbsenceMandatoryAttributeLocationMessage, code, _fixture.Site.Branch_cd);
            Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code), true, true, true);
            Assert.False(DataFacade.MasterRoute.IsMasterRouteExist(actualMasterRouteExpression), "System should not allow to import feeding wihtout Location.");
        }

        [Fact(DisplayName = "Master Route Feeding - When at least one SequenceNummber is not specified Then System doesn't allow to create Master Route Stops and master Route")]
        public void VerifyMasterRouteStopCannotBecreatedWithoutMandSequenceNumberFields()
        {
            expectedRouteStop1.Without_SequenceNumber();
            routeBuilder.With_MasterRouteStops(expectedRouteStop1, expectedRouteStop2);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            expectedRoute = routeBuilder.Build();
            var expectedMessage = String.Format(errorAbsenceMandatoryAttributreSequenceNumber, code, _fixture.Site.Branch_cd);
            Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code), true, true, true);
            Assert.False(DataFacade.MasterRoute.IsMasterRouteExist(actualMasterRouteExpression), "System should not allow to import feeding wihtout mandatory SequenceNumber field");
        }

        [Fact(DisplayName = "Master Route Feeding - When stops are not in the correct order Then System allows to create Master Route Stops and master Route")]
        public void VerifyMasterRouteStopsAreNotInTheCorrectOrder()
        {
            routeBuilder.With_MasterRouteStops(expectedRouteStop2, expectedRouteStop1);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            var expectedMessage = String.Format(successCreateMessage, code);
            Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code), true, true, true);

            var expectedRoute = routeBuilder.Build();
            var actualRoute = DataFacade.MasterRoute.Take(x => x.Code == code).Build();
            Assert.Equal(expectedRoute, actualRoute, new MasterRouteComparer());
        }

        [Fact(DisplayName = "Master Route Feeding - When some stops sequence number is omitted Then System doesn't allow to create Master Route Stops and master Route")]
        public void VerifyMasterRouteStopSequenceNumberIsOmitted()
        {
            expectedRouteStop2.With_SequenceNumber(3);
            routeBuilder.With_MasterRouteStops(expectedRouteStop1, expectedRouteStop2);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            expectedRoute = routeBuilder.Build();
            var expectedMessage = String.Format(errorAbsenceStopWithSequenceNumber1Message, code, _fixture.Site.Branch_cd);
            Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code), true, true, true);
            Assert.False(DataFacade.MasterRoute.IsMasterRouteExist(actualMasterRouteExpression), "System should not allow to import feeding when stops are not in the correct order");
        }

        [Fact(DisplayName = "Master Route Feeding - When at least one stop has not synchronized time settings Then System doesn't allow to import route and logs error message")]
        public void VerifyThatTimeSetingsShouldBeSynchronized()
        {
            expectedRouteStop2.With_DepartureTime(new TimeSpan(10, 30, 00));
            routeBuilder.With_MasterRouteStops(expectedRouteStop1, expectedRouteStop2);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            //expectedRoute = routeBuilder.Build();
            var expectedMessage = String.Format(errorNotSynchronizedStopTimesMessage, code, _fixture.Site.Branch_cd, expectedRouteStop2.Build().SequenceNumber);
            Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code), true, true, true);
        }

        [Fact(DisplayName = "Master Route Feeding - When CIT Code is invalid then System doesn't create Master Route")]
        public void VerifyThatMasterRouteCanNotBeCreatedWhitInvalidCitCode()
        {
            try
            {
                _fixture.Site.Branch_cd = "X";
                routeBuilder.With_MasterRouteStops(expectedRouteStop1, expectedRouteStop2);
                HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
                expectedRoute = routeBuilder.Build();
                var expectedMessage = String.Format(errorInvalidCitSiteCodeMessage, code, _fixture.Site.Branch_cd);
                Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code), true, true, true);
                Assert.False(DataFacade.MasterRoute.IsMasterRouteExist(actualMasterRouteExpression), "System should not allow to import feeding wihtout synchronized time settings");
            }
            finally
            {
                _fixture.Site.Branch_cd = "SP";
            }
        }

        [Fact(DisplayName = "Master Route Feeding - When stop location level is VisitAdress Then System doesn't allow to create Master Route Stops and master Route")]
        public void VerifyMasterRouteCanotBeCreatedIfStopLocationLevelIsVisitAddress()
        {
            expectedRouteStop2.With_Location(_fixture.VisitAddressLocation);
            routeBuilder.With_MasterRouteStops(expectedRouteStop1, expectedRouteStop2);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            expectedRoute = routeBuilder.Build();
            var expectedMessage = String.Format(errorLocationLevelIsVisitAddressMessage, code, _fixture.Site.Branch_cd);
            Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code), true, true, true);
            Assert.False(DataFacade.MasterRoute.IsMasterRouteExist(actualMasterRouteExpression), "System should not allow to import feeding when stops are not in the correct order");
        }

        [Theory(DisplayName = "Master Route Feeding - When any time attribute is empty for a Stop then System recalculates it and creates Master Route")]
        [MemberData("GetTimes", MemberType = typeof(StopsTimeValues))]
        public void VerifyThatSystemSetsCorrectTimeValuesOnMasterRouteCreation(TimeSpan arr1, TimeSpan onsite1, TimeSpan depart1, TimeSpan arr2, TimeSpan onsite2, TimeSpan depart2)
        {
            var timesList = new List<TimeSpan> { arr1, onsite1, depart1, arr2, onsite2, depart2 };

            expectedRouteStop1.With_ArrivalTime(arr1);
            expectedRouteStop1.With_OnSiteTime(onsite1);
            expectedRouteStop1.With_DepartureTime(depart1);

            expectedRouteStop2.With_ArrivalTime(arr2);
            expectedRouteStop2.With_OnSiteTime(onsite2);
            expectedRouteStop2.With_DepartureTime(depart2);

            routeBuilder.With_MasterRouteStops(expectedRouteStop1, expectedRouteStop2);
            HelperFacade.MasterRouteHelper.ConvertEntityToXmlAndSendFeeding("", routeBuilder);
            var expectedMessage = String.Format(successCreateMessage, code);
            Assert.Equal(expectedMessage, HelperFacade.MasterRouteHelper.GetValidatedFeedingLogMessage(code), true, true, true);

            var actualRoute = DataFacade.MasterRoute.Take(x => x.Code == code).Build();
            routeId = actualRoute.ID;
            var actualRouteStop1 = DataFacade.MasterRouteStop.Take(actualRouteStopOneExpression).Build();
            var actualRouteStop2 = DataFacade.MasterRouteStop.Take(actualRouteStopTwoExpression).Build();
            Assert.True(HelperFacade.MasterRouteHelper.IsTimeCorrect(timesList, actualRouteStop1, actualRouteStop2));
        }

        public void Dispose()
        {
            if (DataFacade.MasterRoute.IsMasterRouteExist(x => x.Code == code))
                DataFacade.MasterRoute.Delete(mr => mr.Code.StartsWith(code) && mr.Description.StartsWith(description) && mr.ReferenceNumber.StartsWith(referenceNumber));
        }

        public class MasterRouteComparer : IEqualityComparer<MasterRoute>
        {
            public bool Equals(MasterRoute expectedRoute, MasterRoute actualRoute)
            {
                var result = true;
                var exeptionStr = new List<string>();

                if (expectedRoute.Code != actualRoute.Code)
                {
                    exeptionStr.Add("MasterRoute Code isn't match.\n");
                    result = false;
                }
                if (expectedRoute.Description != actualRoute.Description)
                {
                    exeptionStr.Add("MasterRoute Description isn't match.\n");
                    result = false;
                }
                if (expectedRoute.ReferenceNumber != actualRoute.ReferenceNumber)
                {
                    exeptionStr.Add("MasterRoute ReferenceNumber isn't match.\n");
                    result = false;
                }
                if (expectedRoute.ReferenceNumber != actualRoute.ReferenceNumber)
                {
                    exeptionStr.Add("MasterRoute ReferenceNumber isn't match.\n");
                    result = false;
                }
                if (expectedRoute.WeekdayNumber != actualRoute.WeekdayNumber)
                {
                    exeptionStr.Add("MasterRoute WeekdayNumber isn't match.\n");
                    result = false;
                }
                if (expectedRoute.CrewRequired != actualRoute.CrewRequired)
                {
                    exeptionStr.Add("MasterRoute CrewRequired isn't match.\n");
                    result = false;
                }
                if (expectedRoute.DistancePlanned != actualRoute.DistancePlanned)
                {
                    exeptionStr.Add("MasterRoute DistancePlanned isn't match.\n");
                    result = false;
                }

                if (result == false)
                {
                    throw new Exception(string.Concat(exeptionStr));
                }

                return result;
            }

            public int GetHashCode(MasterRoute route)
            {
                return 0;
            }
        }

        public class MasterRouteStopComparer : IEqualityComparer<MasterRouteStop>
        {
            public bool Equals(MasterRouteStop expectedRouteStop, MasterRouteStop actualRouteStop)
            {
                var result = true;
                var exeptionStr = new List<string>();

                if (expectedRouteStop.Location_id != actualRouteStop.Location_id)
                {
                    exeptionStr.Add($"MasterRouteStop Location isn't match: expected Location - {expectedRouteStop.Location.Code}, actual Location - {actualRouteStop.Location.Code}\n");
                    result = false;
                }
                if (expectedRouteStop.SequenceNumber != actualRouteStop.SequenceNumber)
                {
                    exeptionStr.Add("MasterRouteStop SequenceNumber isn't match.\n");
                    result = false;
                }
                if (expectedRouteStop.ArrivalTime != actualRouteStop.ArrivalTime)
                {
                    exeptionStr.Add("MasterRouteStop ArrivalTime isn't match.\n");
                    result = false;
                }
                if (expectedRouteStop.OnSiteTime != actualRouteStop.OnSiteTime)
                {
                    exeptionStr.Add("MasterRouteStop OnSiteTime isn't match.\n");
                    result = false;
                }
                if (expectedRouteStop.DepartureTime != actualRouteStop.DepartureTime)
                {
                    exeptionStr.Add("MasterRouteStop DepatureTime isn't match.\n");
                    result = false;
                }

                if (result == false)
                {
                    throw new Exception(string.Concat(exeptionStr));
                }

                return result;
            }

            public int GetHashCode(MasterRouteStop route)
            {
                return 0;
            }
        }

        public class StopsTimeValues
        {
            public static readonly List<object[]> Times = new List<object[]>()
            {
                new object[] { null , new TimeSpan( 0, 15, 0), new TimeSpan(9, 15, 0), new TimeSpan(10,0,0), new TimeSpan( 0, 15, 0), new TimeSpan(10, 15, 0)},
                new object[] { null, null, new TimeSpan(9, 15, 0), new TimeSpan(10,0,0), new TimeSpan( 0, 15, 0), new TimeSpan(10, 15, 0)},
                new object[] { null, null, null, new TimeSpan(10,0,0), new TimeSpan( 0, 15, 0), new TimeSpan(10, 15, 0)},
                new object[] { new TimeSpan(9, 15, 0), new TimeSpan( 0, 15, 0), null, new TimeSpan(10,0,0), new TimeSpan( 0, 15, 0), new TimeSpan(10, 15, 0)},
                new object[] { new TimeSpan(9, 15, 0), null, null, new TimeSpan(10,0,0), new TimeSpan( 0, 15, 0), new TimeSpan(10, 15, 0)},
                new object[] { new TimeSpan(9, 15, 0), null, new TimeSpan(9, 30, 0), new TimeSpan(10,0,0), new TimeSpan( 0, 15, 0), new TimeSpan(10, 15, 0)},
                new object[] { null, new TimeSpan(0, 15, 0), null, new TimeSpan(10,0,0), new TimeSpan( 0, 15, 0), new TimeSpan(10, 15, 0)},
                new object[] { new TimeSpan(9,0,0), new TimeSpan( 0, 15, 0), new TimeSpan(9, 15, 0), null, new TimeSpan( 0, 15, 0), new TimeSpan(10, 15, 0)},
                new object[] { new TimeSpan(9,0,0), new TimeSpan( 0, 15, 0), new TimeSpan(9, 15, 0), new TimeSpan(10, 00, 0), null, new TimeSpan(10, 15, 0)},
                new object[] { new TimeSpan(9,0,0), new TimeSpan( 0, 15, 0), new TimeSpan(9, 15, 0), null, new TimeSpan( 0, 15, 0), null},
                new object[] { new TimeSpan(9,0,0), new TimeSpan( 0, 15, 0), new TimeSpan(9, 15, 0), null, null, new TimeSpan(10, 15, 0)},
                new object[] { new TimeSpan(9,0,0), new TimeSpan( 0, 15, 0), new TimeSpan(9, 15, 0), null, null, null},
                new object[] { null, null, null, null, null,null},
                new object[] { default(TimeSpan), null, null, null, null,null},
            };
            public static IEnumerable<object[]> GetTimes
            {
                get
                {
                    return Times;
                }
            }
        }

    }
}
