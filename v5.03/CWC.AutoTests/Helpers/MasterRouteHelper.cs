using Cwc.Routes;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.Helpers
{
    public class MasterRouteHelper
    {
        public MasterRouteHelper()
        {
            
        }

        public void ConvertEntityToXmlAndSendFeeding(string emptyTag, params MasterRouteBuilder[] expectedRoutes) {
            
                var convertedEntity = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(expectedRoutes, emptyTag);
                var response = HelperFacade.FeedingHelper.SendFeeding(convertedEntity.ToString());                     
        }
        public string GetValidatedFeedingLogMessage(string code)
        {

            var foundMessage = string.Empty;
            try
            {
                using (var feedingContext = new AutomationFeedingDataContext())
                {
                    foundMessage = feedingContext.ValidatedFeedingLogs.OrderByDescending(f => f.DateCreated).FirstOrDefault(fm => fm.Message.Contains(code)).Message.Trim();
                }
            }
            catch
            {
                throw new Exception($"Validated Feeding log for route with code '{code}' is not found");
            }

            return foundMessage.Replace(Environment.NewLine, "");
        }

        public int CountStops(Expression<Func<MasterRouteStop, bool>> expression)
        {
            using (var routeContext = new AutomationRoutesDataContext())
            {
                var found = routeContext.MasterRouteStops.Where(expression).ToList();

                if (found.Count == 0)
                {
                    throw new ArgumentNullException("Master Rote doesn't contain any Stops");
                }

                return found.Count;
            }
        }

        public  List<TimeSpan> CalculateTimes(List<TimeSpan> timesList)
        {
            var arr1 = timesList[0];
            var onSite1 = timesList[1];
            var dept1 = timesList[2];
            var arr2 = timesList[3];
            var onSite2 = timesList[4];
            var dept2 = timesList[5];

            if (arr1 == default(TimeSpan) && dept1 == default(TimeSpan) && onSite1 == default(TimeSpan))
            {
                arr1 = new TimeSpan(9, 00, 00);
                onSite1 = new TimeSpan(0, 05, 00);
                dept1 = new TimeSpan(9, 05, 00);
            }
            if (arr1 == default(TimeSpan) && onSite1 == default(TimeSpan) && dept1 != default(TimeSpan))
            {
                onSite1 = new TimeSpan(0, 05, 00);
                arr1 = dept1 - onSite1;
            }
            if (arr1 == default(TimeSpan) && dept1 != default(TimeSpan) && onSite1 != default(TimeSpan))
            {
                arr1 = dept1 - onSite1;
            }
            if (arr1 != default(TimeSpan) && dept1 == default(TimeSpan) && onSite1 != default(TimeSpan))
            {
                dept1 = arr1 + onSite1;
            }
            if (arr1 == default(TimeSpan) && dept1 == default(TimeSpan) && onSite1 != default(TimeSpan))
            {
                arr1 = new TimeSpan(9, 00, 00);
                dept1 = arr1 + onSite1;
            }
            if (arr1 != default(TimeSpan) && dept1 == default(TimeSpan) && onSite1 == default(TimeSpan))
            {
                onSite1 = new TimeSpan(0, 05, 00);
                dept1 = arr1 + onSite1;
            }
            if (arr1 != default(TimeSpan) && dept1 != default(TimeSpan) && onSite1 == default(TimeSpan))
            {
                onSite1 = dept1 - arr1;
            }


            if (arr2 == default(TimeSpan) && dept2 == default(TimeSpan) && onSite2 == default(TimeSpan))
            {
                arr2 = dept1 + new TimeSpan(0, 05, 00);
                onSite2 = new TimeSpan(0, 05, 00);
                dept2 = arr2 + onSite2;
            }
            if (arr2 == default(TimeSpan) && dept2 != default(TimeSpan) && onSite2 == default(TimeSpan))
            {
                onSite2 = new TimeSpan(0, 05, 00);
                arr2 = dept2 - onSite2;
            }
            if (arr2 == default(TimeSpan) && dept2 != default(TimeSpan) && onSite2 != default(TimeSpan))
            {
                arr2 = dept2 - onSite2;
            }
            if (arr2 != default(TimeSpan) && dept2 == default(TimeSpan) && onSite2 != default(TimeSpan))
            {
                dept2 = arr2 + onSite2;
            }
            if (arr2 == default(TimeSpan) && dept2 == default(TimeSpan) && onSite2 != default(TimeSpan))
            {
                arr2 = dept1 + new TimeSpan(0, 05, 00);
                dept2 = arr2 + onSite2;
            }
            if (arr2 != default(TimeSpan) && dept2 == default(TimeSpan) && onSite2 == default(TimeSpan))
            {
                onSite2 = new TimeSpan(0, 05, 00);
                dept2 = arr2 + onSite2;
            }
            if (arr2 != default(TimeSpan) && dept2 != default(TimeSpan) && onSite2 == default(TimeSpan))
            {
                onSite2 = dept2 - arr2;
            }

            return new List<TimeSpan>() { arr1, onSite1, dept1, arr2, onSite2, dept2 };
        }

        public bool IsTimeCorrect(List<TimeSpan> timesList, MasterRouteStop actualRouteStop1, MasterRouteStop actualRouteStop2)
        {
            var result = true;
            var exeptionStr = new List<string>();

            var newTimes = HelperFacade.MasterRouteHelper.CalculateTimes(timesList);

            var arr1 = newTimes[0];
            var onSite1 = newTimes[1];
            var dept1 = newTimes[2];
            var arr2 = newTimes[3];
            var onSite2 = newTimes[4];
            var dept2 = newTimes[5];

            if (actualRouteStop1.ArrivalTime != arr1)
            {
                exeptionStr.Add($"MasterRouteStop ArrivalTime isn't match. Expected {arr1}, Actual {actualRouteStop1.ArrivalTime}\n");
                result = false;
            };
            if (actualRouteStop1.OnSiteTime != onSite1)
            {
                exeptionStr.Add($"MasterRouteStop OnSiteTime isn't match. Expected {onSite1}, Actual {actualRouteStop1.OnSiteTime}\n");
                result = false;
            }
            if (actualRouteStop1.DepartureTime != dept1)
            {
                exeptionStr.Add($"MasterRouteStop DepatureTime isn't match.  Expected {dept1}, Actual {actualRouteStop1.DepartureTime}\n");
                result = false;
            }
            if (actualRouteStop2.ArrivalTime != arr2)
            {
                exeptionStr.Add($"MasterRouteStop ArrivalTime isn't match. Expected {arr2}, Actual {actualRouteStop2.ArrivalTime}\n");
                result = false;
            }
            if (actualRouteStop2.OnSiteTime != onSite2)
            {
                exeptionStr.Add($"MasterRouteStop OnSiteTime isn't match. Expected {onSite2}, Actual {actualRouteStop2.OnSiteTime}\n");
                result = false;
            }
            if (actualRouteStop2.DepartureTime != dept2)
            {
                exeptionStr.Add($"MasterRouteStop DepatureTime isn't match.  Expected {dept2}, Actual {actualRouteStop2.DepartureTime}\n");
                result = false;
            }

            if (result == false)
            {
                throw new Exception(string.Concat(exeptionStr));
            }

            return result;
        }
    }
}
