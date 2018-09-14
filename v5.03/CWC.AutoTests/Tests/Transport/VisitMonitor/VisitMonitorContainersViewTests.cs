using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.Contracts;
using Cwc.Ordering;
using Cwc.Transport;
using Cwc.Transport.Classes;
using Cwc.Transport.Classes.UserSettings;
using Cwc.Transport.Enums;
using Cwc.Transport.Helpers;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.DailyDataBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CWC.AutoTests.Tests.Transport.VisitMonitor

{
    [Collection("Transport Order Collection")]
    public class VisitMonitorContainersViewTests : IDisposable
    {
        DataModel.ModelContext _context;
        string routeCode, packNumber;
        DateTime today = DateTime.Today;
        TimeSpan arrival;
        ServiceType serviceType;
        Location location;
        Site site;
        Cwc.Ordering.Order serviceOrder;
        DataModel.reason reason;
        VisitMonitorsFilterSetting filterSetting;

        public VisitMonitorContainersViewTests()
        {
            _context = new DataModel.ModelContext();
            routeCode = "SP1001";
            packNumber = "PN200000";
            arrival = new TimeSpan(8, 0, 0);
            serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            location = DataFacade.Location.Take(l => l.Code == "SP02").Build();
            site = DataFacade.Site.Take(s => s.ID == location.ServicingDepotID).Build();
            serviceOrder = DataFacade.Order.New(today, location, serviceType.Code).SaveToDb();
            reason = _context.reasons.First();
            filterSetting = new VisitMonitorsFilterSetting(1);
        }

        [Fact(DisplayName = "VM: Container dep view - When system matched planned to delivery containers Then System shows it")]
        public void VerifyThatSystemShowsMatchedPlannedContainersProprly()
        {

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var daiPack = DailyDataFacade.DaiPack
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmmss"))
                .With_FrLocation(location)
                .With_ToLocation(location)
                .With_Site(site)
                .With_PackNumber(packNumber)
                .with_MasterRoute(routeCode)
                .SaveToDb();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Code("SP2001").With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceType)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today, DailyRouteCode = routeCode, DailyRouteDate = today, PlannedArrivalTime = arrival, ActualArrivalTime = arrival };
            

            var query = TransportFacade.ScheduleMonitoringService.GetContainersQuery(filterSetting,selectedRow);
            

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.Equal(1, res.Count);
            Assert.Equal(packNumber, res[0][VisitMonitorContainerHelper.Number]);
            Assert.Equal(location.Code, res[0][VisitMonitorContainerHelper.LocationFrom]);
        }

        [Theory(DisplayName = "VM: Container dep view - When order is delivered or collected during daily stop Then System shows it")]
        [InlineData(null, 0)]
        [InlineData("TRK", 1)]
        [InlineData("LOC", 2)]
        [InlineData("TRKSPL", 3)]
        [InlineData("DEPSPL", 4)]
        [InlineData("DEP", 5)]
        [InlineData("CAN", 6)]
        [InlineData("TRK234234", 7)]
        public void VerifyThatSystemShowDoneOrdersProperly(string trStatus, int resStatus)
        {
            var dailyLine = DailyDataFacade.DailyStop.New()
               .With_MasterRoute(routeCode)
               .With_DaiDate(today)
               .With_ArrivalTime(arrival.ToString("hhmm"))
               .With_Location(location).With_Site(site)
               .With_Reason(reason.ID)
               .With_ActualDaiDate(today)
               .With_ActualArrivalTime(arrival.ToString("hhmm"))
               .SaveToDb();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceType)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var hispack = DailyDataFacade.HisPack.New()
                .With_Date(today)
                .With_Time(arrival.ToString("hhmmss"))
                .With_Status(trStatus)
                .With_FrLocation(location)
                .With_ToLocation(location)
                .With_PackVal(1000)
                .With_BagType(3301)
                .With_PackNr(packNumber)
                .With_MasterRoute(routeCode)
                .With_Site(site)
                .With_OrderID(transportOrer.Build().Code)
                .SaveToDb();

            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today, DailyRouteCode = routeCode, DailyRouteDate = today, PlannedArrivalTime = arrival, ActualArrivalTime = arrival };

            var query = TransportFacade.ScheduleMonitoringService.GetContainersQuery(filterSetting, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.Equal(1, res.Count);
            Assert.Equal(packNumber, res[0][VisitMonitorContainerHelper.Number]);
            Assert.Equal(location.Code, res[0][VisitMonitorContainerHelper.LocationFrom]);
            Assert.Equal(location.Code, res[0][VisitMonitorContainerHelper.LocationTo]);
            Assert.Equal(transportOrer.Build().Code, res[0][VisitMonitorContainerHelper.TransportOrder]);
            Assert.Equal(serviceOrder.ID, res[0][VisitMonitorContainerHelper.ServiceOrder]);
            Assert.Equal(1000m, res[0][VisitMonitorContainerHelper.Value]);
            Assert.Equal(resStatus, res[0][VisitMonitorContainerHelper.TransportStatus]);     
        }

        [Fact(DisplayName = "VM: Container dep view - When more that one his_packs are found Then System, takes latest by date + time -> Status")]
        public void VerifyThatSystemTakesOnlyLatestStatus()
        {
            var bagTypeFirst = _context.bag_types.First();
            var bagTypeSecond = _context.bag_types.First(b => b.bgtyp_nr != bagTypeFirst.bgtyp_nr);
            var actualArrival = arrival.Add(new TimeSpan(0, 2, 0));

            var dailyLine = DailyDataFacade.DailyStop.New()
               .With_MasterRoute(routeCode)
               .With_DaiDate(today)
               .With_ArrivalTime(arrival.ToString("hhmm"))
               .With_Location(location).With_Site(site)
               .With_Reason(reason.ID)
               .With_ActualDaiDate(today)
               .With_ActualArrivalTime(actualArrival.Add(new TimeSpan(0, 1, 0)).ToString("hhmm"))
               .SaveToDb();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceType)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var hispackEarlier = DailyDataFacade.HisPack.New()
                .With_Date(today)
                .With_Time(arrival.ToString("hhmmss"))
                .With_RealDate(today)
                .With_RealTime(actualArrival.ToString("hhmmss"))
                .With_Status("TRK")
                .With_FrLocation(location)
                .With_ToLocation(location)
                .With_PackVal(1000)
                .With_BagType(bagTypeFirst.bgtyp_nr)
                .With_PackNr(packNumber)
                .With_MasterRoute(routeCode)
                .With_Site(site)
                .With_OrderID(transportOrer.Build().Code)
                .SaveToDb();

            var hispackLater = DailyDataFacade.HisPack.New()
                .With_Date(today)
                .With_Time(actualArrival.Add(new TimeSpan(0,1,0)).ToString("hhmmss"))
                .With_RealDate(today)
                .With_RealTime(actualArrival.Add(new TimeSpan(0, 1, 0)).ToString("hhmmss"))
                .With_Status("LOC")
                .With_FrLocation(location)
                .With_ToLocation(location)
                .With_PackVal(2000)
                .With_BagType(bagTypeSecond.bgtyp_nr)
                .With_PackNr(packNumber)
                .With_MasterRoute(routeCode)
                .With_Site(site)
                .With_OrderID(transportOrer.Build().Code)
                .SaveToDb();

            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today, DailyRouteCode = routeCode, DailyRouteDate = today, PlannedArrivalTime = arrival, ActualArrivalTime = actualArrival.Add(new TimeSpan(0, 1, 0)) };

            var query = TransportFacade.ScheduleMonitoringService.GetContainersQuery(filterSetting, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.Equal(1, res.Count);
            Assert.Equal(2, res[0][VisitMonitorContainerHelper.TransportStatus]);
            Assert.Equal($"{bagTypeSecond.abbrev} {bagTypeSecond.descript}", res[0][VisitMonitorContainerHelper.ContainerType]);
            Assert.Equal((decimal)2000, res[0][VisitMonitorContainerHelper.Value]);
        }

        [Fact(DisplayName = "When more that one his packs are matched Then System uses latest by RealDate + RealTime")]
        public void VerifyThatSystemMatchesHosPackByLatestReadDatreAndTime()
        {
            var bagTypeFirst = _context.bag_types.First();
            var bagTypeSecond = _context.bag_types.First(b => b.bgtyp_nr != bagTypeFirst.bgtyp_nr);
            var actualArrival = arrival.Add(new TimeSpan(0, 2, 0));

            var dailyLine = DailyDataFacade.DailyStop.New()
               .With_MasterRoute(routeCode)
               .With_DaiDate(today)
               .With_ArrivalTime(arrival.ToString("hhmm"))
               .With_Location(location).With_SiteCode(site.Branch_cd)
               .With_Reason(reason.ID)
               .With_ActualDaiDate(today)
               .With_ActualArrivalTime(arrival.ToString("hhmm"))
               .SaveToDb();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceType)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var hispackEarlier = DailyDataFacade.HisPack.New()
                .With_Date(today)
                .With_Time(arrival.ToString("hhmmss"))
                .With_RealDate(today)
                .With_RealTime(actualArrival.ToString("hhmmss"))
                .With_Status("TRK")
                .With_FrLocation(location)
                .With_ToLocation(location)
                .With_PackVal(1000)
                .With_BagType(bagTypeFirst.bgtyp_nr)
                .With_PackNr(packNumber)
                .With_MasterRoute(routeCode)
                .With_Site(site)
                .With_OrderID(transportOrer.Build().Code)
                .SaveToDb();

            var hispackLater = DailyDataFacade.HisPack.New()
                .With_Date(today)
                .With_Time(arrival.ToString("hhmmss"))
                .With_RealDate(today)
                .With_RealTime(actualArrival.Add(new TimeSpan(0, 1, 0)).ToString("hhmmss"))
                .With_Status("LOC")
                .With_FrLocation(location)
                .With_ToLocation(location)
                .With_PackVal(2000)
                .With_BagType(bagTypeSecond.bgtyp_nr)
                .With_PackNr(packNumber)
                .With_MasterRoute(routeCode)
                .With_Site(site)
                .With_OrderID(transportOrer.Build().Code)
                .SaveToDb();

            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today, DailyRouteCode = routeCode, DailyRouteDate = today, PlannedArrivalTime = arrival, ActualArrivalTime = arrival };

            var query = TransportFacade.ScheduleMonitoringService.GetContainersQuery(filterSetting, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.Equal(1, res.Count);
            Assert.Equal(2, res[0][VisitMonitorContainerHelper.TransportStatus]);
            Assert.Equal($"{bagTypeSecond.abbrev} {bagTypeSecond.descript}", res[0][VisitMonitorContainerHelper.ContainerType]);
            Assert.Equal((decimal)2000, res[0][VisitMonitorContainerHelper.Value]);
        }

        [Theory(DisplayName = "VM: Container dep view - When his-pack status {‘INT’, ‘LOC’} The Deliverred field is selected")]
        [InlineData("LOC")]
        [InlineData("INT")]
        public void verifyThatSystemSetsDeliveredStatusProperly(string trStatus)
        {
            var dailyLine = DailyDataFacade.DailyStop.New()
               .With_MasterRoute(routeCode)
               .With_DaiDate(today)
               .With_ArrivalTime(arrival.ToString("hhmm"))
               .With_Location(location).With_Site(site)
               .With_Reason(reason.ID)
               .With_ActualDaiDate(today)
               .With_ActualArrivalTime(arrival.ToString("hhmm"))
               .SaveToDb();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceType)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var hispack = DailyDataFacade.HisPack.New()
                .With_Date(today)
                .With_Time(arrival.ToString("hhmmss"))
                .With_Status(trStatus)
                .With_FrLocation(location)
                .With_ToLocation(location)
                .With_PackVal(1000)
                .With_BagType(3301)
                .With_PackNr(packNumber)
                .With_MasterRoute(routeCode)
                .With_Site(site)
                .With_OrderID(transportOrer.Build().Code)
                .SaveToDb();

            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today, DailyRouteCode = routeCode, DailyRouteDate = today, PlannedArrivalTime = arrival, ActualArrivalTime = arrival };

            var query = TransportFacade.ScheduleMonitoringService.GetContainersQuery(filterSetting, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.Equal(1, res[0][VisitMonitorContainerHelper.Delivered]);
            Assert.Equal(0, res[0][VisitMonitorContainerHelper.Missing]);
        }

        [Fact(DisplayName = "VM: Container dep view - When his_pack status is 'FRZ RTRECEIV' Then System marks this container as missing")]
        public void VerifyThatSystemShowsMissedProperly()
        {
            var dailyLine = DailyDataFacade.DailyStop.New()
               .With_MasterRoute(routeCode)
               .With_DaiDate(today)
               .With_ArrivalTime(arrival.ToString("hhmm"))
               .With_Location(location).With_Site(site)
               .With_Reason(reason.ID)
               .With_ActualDaiDate(today)
               .With_ActualArrivalTime(arrival.ToString("hhmm"))
               .SaveToDb();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Code("SP2001").With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceType)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var hispack = DailyDataFacade.HisPack.New()
                .With_Date(today)
                .With_Time(arrival.ToString("hhmmss"))
                .With_Status("FRZ RTRECEIV")
                .With_FrLocation(location)
                .With_ToLocation(location)
                .With_PackVal(1000)
                .With_BagType(3301)
                .With_PackNr(packNumber)
                .With_MasterRoute(routeCode)
                .With_Site(site)
                .With_OrderID(transportOrer.Build().Code)
                .SaveToDb();

            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today, DailyRouteCode = routeCode, DailyRouteDate = today, PlannedArrivalTime = arrival, ActualArrivalTime = arrival };

            var query = TransportFacade.ScheduleMonitoringService.GetContainersQuery(filterSetting, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.Equal(0, res[0][VisitMonitorContainerHelper.Delivered]);
            Assert.Equal(1, res[0][VisitMonitorContainerHelper.Missing]);
        }

        public void Dispose()
        {
            _context.his_packs.RemoveRange(_context.his_packs.Where(p => p.pack_nr == packNumber));
            _context.Cwc_Transport_TransportOrders.RemoveRange(_context.Cwc_Transport_TransportOrders.Where(tr=>tr.MasterRouteCode == routeCode));
            _context.dai_lines.RemoveRange(_context.dai_lines.Where(d => d.mast_cd == routeCode));
            _context.dai_packs.RemoveRange(_context.dai_packs.Where(d => d.pack_nr == packNumber));
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
