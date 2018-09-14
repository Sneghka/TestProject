using Cwc.BaseData;
using CWC.AutoTests.Model;
using System;

namespace CWC.AutoTests.ObjectBuilder.DailyDataBuilders
{
    public class DailyServiceBuilder
    {
        DailyService dailyService;

        public DailyServiceBuilder()
        {
            dailyService = new DailyService();
            dailyService.DayNumber = 1;
        }

        public DailyServiceBuilder With_Date(DateTime value)
        {
            dailyService.DailyRouteDate = value;
            return this;
        }

        public DailyServiceBuilder With_Location(Location value)
        {
            dailyService.LocationId = value.ID;
            return this;
        }

        public DailyServiceBuilder With_LocationTo(Location value)
        {
            dailyService.LocationToID = value.ID;
            return this;
        }

        public DailyServiceBuilder With_BagType(int value)
        {
            dailyService.BagTypeID = value;
            return this;
        }

        public DailyServiceBuilder With_ServType(string value)
        {
            dailyService.ServiceType = value;
            return this;
        }

        public DailyServiceBuilder With_ArrivalTime(string value)
        {
            dailyService.ArrivalTime = value;
            return this;
        }

        public DailyServiceBuilder With_ArrivalDate(DateTime value)
        {
            dailyService.ArrivalDate = value;
            return this;
        }

        public DailyServiceBuilder With_Site(Site value)
        {
            dailyService.BranchCode = value.Branch_cd;
            return this;
        }

        public DailyServiceBuilder With_MasterRoute(string value)
        {
            dailyService.DailyRouteNumber = value;
            return this;
        }

        public DailyServiceBuilder With_ReasonCode(int value)
        {
            dailyService.ReasonCode = value;
            return this;
        }

        public DailyServiceBuilder SaveToDb()
        {
            using (var context = new AutomationBaseDataContext())
            {
                context.DailyServices.Add(dailyService);
                context.SaveChanges();
            }

            return this;
        }
    }
}
