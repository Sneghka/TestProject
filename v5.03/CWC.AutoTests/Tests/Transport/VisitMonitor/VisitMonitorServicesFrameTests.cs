using Cwc.BaseData;
using Cwc.Contracts;
using Cwc.Ordering;
using Cwc.Transport.Enums;
using CWC.AutoTests.ObjectBuilder;
using System;
using Xunit;
using System.Linq;
using Cwc.Security;
using Cwc.Transport.Classes.UserSettings;
using Cwc.Transport.Classes;
using Cwc.Transport;
using CWC.AutoTests.Helpers;
using Cwc.Transport.Helpers;
using CWC.AutoTests.ObjectBuilder.DailyDataBuilders;
using Cwc.BaseData.Classes;

namespace CWC.AutoTests.Tests.Transport.VisitMonitor
{
    [Collection("Transport Order Collection")]
    public class VisitMonitorServicesFrameTests : IDisposable
    {
        DataModel.ModelContext _context;
        DateTime today = DateTime.Today;
        TimeSpan arrival, actualArrival;
        ServiceType serviceTypeColl;
        ServiceType serviceTypeDelv;
        Location location;
        Site site;
        Cwc.Ordering.Order serviceOrder;
        string masterRout = "SP2001";

        public VisitMonitorServicesFrameTests()
        {
            _context = new DataModel.ModelContext();
            arrival = new TimeSpan(8, 0, 0);
            actualArrival = new TimeSpan(8, 1, 0);
            serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();
            serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            location = DataFacade.Location.Take(l => l.Code == "SP02").Build();            
            site = DataFacade.Site.Take(s => s.ID == location.ServicingDepotID).Build();
            serviceOrder = DataFacade.Order.New(today, location, serviceTypeDelv.Code).SaveToDb();
        }

        [Fact(DisplayName = "VM: Services dep view - When Transport Orders contains transport order services Then System shows all of hem")]
        public void VerifThatSystemShowsServiceProperly()
        {
            var servCodeFirst = _context.ServicingCodes.First();
            var servCodeSecond = _context.ServicingCodes.Where(s => s.servCode != servCodeFirst.servCode).First();
            var author = SecurityFacade.LoginService.GetAdministratorLogin();

            var transportOrerFirst = DataFacade.TransportOrder.New()
                .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeColl.ID)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder.ID)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).With_MasterRouteCode("SP2001").SaveToDb();

            var transportOrerSecond = DataFacade.TransportOrder.New()
                .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeDelv.ID)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder.ID)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).With_MasterRouteCode("SP2001").SaveToDb();

            var transportOrderServiceFirst = DataFacade.TransportOrderServ.New()
                .With_IsPerformed(false)
                .With_IsPlanned(true)
                .With_Service(servCodeFirst.servCode)
                .With_TransportOrderID(transportOrerFirst.Build().ID)
                .With_AuthorID(author.UserID)
                .With_EditorID(author.UserID)
                .SaveToDb();

            var transportOrderServiceSecond = DataFacade.TransportOrderServ.New()
                .With_IsPerformed(true)
                .With_IsPlanned(false)
                .With_Service(servCodeSecond.servCode)
                .With_TransportOrderID(transportOrerSecond.Build().ID)
                .With_AuthorID(author.UserID)
                .With_EditorID(author.UserID)
                .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };
            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today };

            var query = TransportFacade.ScheduleMonitoringService.GetJobsQuery(settings, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.Equal(2, res.Count);

            Assert.True(res.Any(x => x[VisitMonitorJobHelper.Code].ToString() == servCodeFirst.servCode
                                && x[VisitMonitorJobHelper.Description].ToString() == servCodeFirst.servDesc
                                && (int)x[VisitMonitorJobHelper.Executed] == Convert.ToInt32(false)
                                && (int)x[VisitMonitorJobHelper.Planned] == Convert.ToInt32(true)
                                && x[VisitMonitorJobHelper.TransportOrder].ToString() == transportOrerFirst.Build().Code
                                && x[VisitMonitorJobHelper.ServiceOrder].ToString() == serviceOrder.ID));

            Assert.True(res.Any(x => x[VisitMonitorJobHelper.Code].ToString() == servCodeSecond.servCode
                                && x[VisitMonitorJobHelper.Description].ToString() == servCodeSecond.servDesc
                                && (int)x[VisitMonitorJobHelper.Executed] == Convert.ToInt32(true)
                                && (int)x[VisitMonitorJobHelper.Planned] == Convert.ToInt32(false)
                                && x[VisitMonitorJobHelper.TransportOrder].ToString() == transportOrerSecond.Build().Code
                                && x[VisitMonitorJobHelper.ServiceOrder].ToString() == serviceOrder.ID));
        }

        [Fact(DisplayName = "VM: Services dep view - When dai_servicing records exists Then System matches them")]
        public void VerifyThatSystemMatchesDaiServicingProprly()
        {
            var servCodeFirst = _context.ServicingCodes.First();
            var author = SecurityFacade.LoginService.GetAdministratorLogin();

            var transportOrerFirst = DataFacade.TransportOrder.New()
                .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeColl.ID)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder.ID)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).With_MasterRouteCode(masterRout).SaveToDb();

            var dailyLine = DailyDataFacade.DailyStop.New()
               .With_MasterRoute(masterRout)
               .With_DaiDate(today)
               .With_ArrivalTime(arrival.ToString("hhmm"))
               .With_Location(location).With_Site(site)
               .With_ActualDaiDate(today)
               .With_ActualArrivalTime(actualArrival.ToString("hhmm"))
               .SaveToDb();

            var daiServicing = DailyDataFacade.DaiServicindCode.New()
                .With_MasterRoute(masterRout)
                .With_Site(site)
                .With_DaiDate(today)                
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_ServCode(servCodeFirst.servCode)
                .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };
            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today, PlannedArrivalTime = arrival, DailyRouteCode = masterRout, DailyRouteDate = today };

            var query = TransportFacade.ScheduleMonitoringService.GetJobsQuery(settings, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(servCodeFirst.servCode, res[0][VisitMonitorJobHelper.Code]);
            Assert.Equal(servCodeFirst.servDesc, res[0][VisitMonitorJobHelper.Description]);
            Assert.Equal(0, res[0][VisitMonitorJobHelper.Planned]);
            Assert.Equal(1, res[0][VisitMonitorJobHelper.Executed]);
            Assert.Equal(string.Empty, res[0][VisitMonitorJobHelper.TransportOrder].ToString());
            Assert.Equal(string.Empty, res[0][VisitMonitorJobHelper.ServiceOrder].ToString());
        }

        [Fact(DisplayName = "VM: Services dep view - When system find both performed (dai_servicing) and non performed (Transport Servicing) jobs Then System shows both")]
        public void VerifyThatSystemShowsPerformedAndNotPerformedJobs()
        {
            var servCodeFirst = _context.ServicingCodes.First();
            var servCodeSecond = _context.ServicingCodes.Where(s => s.servCode != servCodeFirst.servCode).First();
            var author = SecurityFacade.LoginService.GetAdministratorLogin();

            var transportOrerFirst = DataFacade.TransportOrder.New()
                .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeColl.ID)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder.ID)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).With_MasterRouteCode("SP2001").SaveToDb();

            var transportOrderServiceFirst = DataFacade.TransportOrderServ.New()
                .With_IsPerformed(false)
                .With_IsPlanned(true)
                .With_Service(servCodeFirst.servCode)
                .With_TransportOrderID(transportOrerFirst.Build().ID)
                .With_AuthorID(author.UserID)
                .With_EditorID(author.UserID)
                .SaveToDb();

            var dailyLine = DailyDataFacade.DailyStop.New()
               .With_MasterRoute(masterRout)
               .With_DaiDate(today)
               .With_ArrivalTime(arrival.ToString("hhmm"))
               .With_Location(location).With_Site(site)
               .With_ActualDaiDate(today)
               .With_ActualArrivalTime(actualArrival.ToString("hhmm"))
               .SaveToDb();

            var daiServicing = DailyDataFacade.DaiServicindCode.New()
                .With_MasterRoute(masterRout)
                .With_Site(site)
                .With_DaiDate(today)                
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_ServCode(servCodeSecond.servCode)
                .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };
            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today, DailyRouteCode = masterRout, DailyRouteDate = today, PlannedArrivalTime = arrival };

            var query = TransportFacade.ScheduleMonitoringService.GetJobsQuery(settings, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.Equal(2, res.Count);

            Assert.True(res.Any(x => x[VisitMonitorJobHelper.Code].ToString() == servCodeFirst.servCode
                               && x[VisitMonitorJobHelper.Description].ToString() == servCodeFirst.servDesc
                               && (int)x[VisitMonitorJobHelper.Executed] == Convert.ToInt32(false)
                               && (int)x[VisitMonitorJobHelper.Planned] == Convert.ToInt32(true)
                               && x[VisitMonitorJobHelper.TransportOrder].ToString() == transportOrerFirst.Build().Code
                               && x[VisitMonitorJobHelper.ServiceOrder].ToString() == serviceOrder.ID));

            Assert.True(res.Any(x => x[VisitMonitorJobHelper.Code].ToString() == servCodeSecond.servCode
                               && x[VisitMonitorJobHelper.Description].ToString() == servCodeSecond.servDesc
                               && (int)x[VisitMonitorJobHelper.Executed] == Convert.ToInt32(true)
                               && (int)x[VisitMonitorJobHelper.Planned] == Convert.ToInt32(false)
                               && x[VisitMonitorJobHelper.TransportOrder].ToString() == string.Empty
                               && x[VisitMonitorJobHelper.ServiceOrder].ToString() == string.Empty));
        }

        public void Dispose()
        {
            var ids = _context.Cwc_Transport_TransportOrders.Where(tr => tr.MasterRouteCode.StartsWith("SP")).Select(tr=>tr.id).ToArray();

            _context.dai_lines.RemoveRange(_context.dai_lines.Where(d => d.mast_cd == masterRout));
            _context.dai_servicingcodes.RemoveRange(_context.dai_servicingcodes.Where(d => d.mast_cd == masterRout));
            _context.Cwc_Transport_TransportOrderServs.RemoveRange(_context.Cwc_Transport_TransportOrderServs.Where(ts => ids.Contains(ts.TransportOrderID)));
            _context.Cwc_Transport_TransportOrders.RemoveRange(_context.Cwc_Transport_TransportOrders.Where(tr => ids.Contains(tr.id)));
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
