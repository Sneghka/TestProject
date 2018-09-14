using Cwc.BaseData;
using Cwc.Transport.Model;
using CWC.AutoTests.Model;
using System;

namespace CWC.AutoTests.ObjectBuilder.DailyDataBuilders
{
    public class DaiServicingCodeBuilder
    {
        DailyStopJob daiServicingCode;
        public DaiServicingCodeBuilder()
        {            
        }

        public DaiServicingCodeBuilder With_DaiDate(DateTime value)
        {
            daiServicingCode.Date = value;            
            daiServicingCode.DayNumber = (int)value.DayOfWeek;
            return this;
        }

        public DaiServicingCodeBuilder With_ActualDate(DateTime value)
        {
            daiServicingCode.ActualDate = value;
            return this;
        }

        public DaiServicingCodeBuilder With_ServCode(string value)
        {
            daiServicingCode.ServicingCode = value;
            return this;
        }

        public DaiServicingCodeBuilder With_ArrivalTime(string value)
        {
            daiServicingCode.ArrivalTime = value;
            return this;
        }

        public DaiServicingCodeBuilder With_StartTime(string value)
        {
            daiServicingCode.StartTime = value;
            return this;
        }

        public DaiServicingCodeBuilder With_EndTime(string value)
        {
            daiServicingCode.EndTime = value;
            return this;
        }

        public DaiServicingCodeBuilder With_Site(Site value)
        {
            daiServicingCode.SiteCode = value.Branch_cd;
            return this;
        }

        public DaiServicingCodeBuilder With_Site(string value)
        {
            daiServicingCode.SiteCode = value;
            return this;
        }

        public DaiServicingCodeBuilder With_MasterRoute(string value)
        {
            daiServicingCode.RouteNumber = value;
            return this;
        }

        public DaiServicingCodeBuilder New()
        {
            daiServicingCode = new DailyStopJob();            
            return this;
        }

        public DailyStopJob Build()
        {
            return this.daiServicingCode;
        }

        public DaiServicingCodeBuilder SaveToDb()
        {
            using (var context = new AutomationTransportDataContext())
            {
                context.DailyStopJobs.Add(daiServicingCode);
                context.SaveChanges();
            }
            return this;
        }
    }
}
