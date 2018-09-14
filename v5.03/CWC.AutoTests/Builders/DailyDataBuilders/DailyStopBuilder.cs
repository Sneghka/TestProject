using Cwc.BaseData;
using Cwc.Transport.Model;
using CWC.AutoTests.DataModel;
using CWC.AutoTests.Model;
using System;

namespace CWC.AutoTests.ObjectBuilder
{
    public class DailyStopBuilder
    {
        DailyStop dailyStop;

        public DailyStopBuilder()
        {            
        }               

        public DailyStopBuilder With_MasterRoute(string value)
        {
            dailyStop.RouteNumber = value;
            return this;
        }

        public DailyStopBuilder With_DaiDate(DateTime value)
        {            
            dailyStop.Date = value;
            dailyStop.DayNumber = (int)value.DayOfWeek;
            return this;
        }

        public DailyStopBuilder With_ActualDaiDate(DateTime value)
        {
            dailyStop.ActualDate = value;
            return this;
        }

        public DailyStopBuilder With_ArrivalTime(string value)
        {
            dailyStop.ArrivalTime = value;
            return this;
        }

        public DailyStopBuilder With_ActualArrivalTime(string value)
        {
            dailyStop.ActualArrivalTime = value;
            return this;
        }

        public DailyStopBuilder With_DepartureTime(string value)
        {
            dailyStop.DepartureTime = value;
            return this;
        }

        public DailyStopBuilder With_ActualDepartureTime(string value)
        {
            dailyStop.ActualDepartureTime = value;
            return this;
        }

        public DailyStopBuilder With_Site(Site value)
        {
            dailyStop.SiteId = value.Branch_cd;
            return this;
        }

        public DailyStopBuilder With_Site(int value)
        {
            var site = BaseDataFacade.SiteService.Load(value);

            if (site != null)
            {
                dailyStop.SiteId = site.Branch_cd;
            }

            return this;
        }

        public DailyStopBuilder With_Site(branch value)
        {
            dailyStop.SiteId = value.branch_cd;
            return this;
        }

        public DailyStopBuilder With_SiteCode(string value)
        {
            dailyStop.SiteId = value;
            return this;
        }

        public DailyStopBuilder With_Location(Location location)
        {
            dailyStop.LocationNumber = location.ID;
            //dailyStop.ref_loc_nr = location.Code;
            return this;
        }

        public DailyStopBuilder With_Reason(int reason)
        {
            dailyStop.ReasonCodeId = reason;
            return this;
        }

        public DailyStopBuilder New()
        {
            dailyStop = new DailyStop();            
            return this;
        }

        public DailyStop Build()
        {
            return this.dailyStop;
        }

        public DailyStopBuilder SaveToDb()
        {       
            try
            {
                using (var context = new AutomationTransportDataContext())
                {
                    context.DailyStops.Add(dailyStop);
                    context.SaveChanges();
                }
                
            }     
            catch
            {
                throw new Exception("Exception has occurred on creating new daily stop record! Please verify data model!");
            }
            return this;
        }
    }
}
