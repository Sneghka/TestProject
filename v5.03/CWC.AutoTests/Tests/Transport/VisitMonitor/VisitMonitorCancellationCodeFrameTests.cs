//using Cwc.BaseData;
//using Cwc.Contracts;
//using Cwc.Ordering;
//using Cwc.Transport;
//using Cwc.Transport.Classes;
//using Cwc.Transport.Enums;
//using System.Linq;
//using Cwc.Transport.Helpers;
//using CWC.AutoTests.Helpers;
//using CWC.AutoTests.ObjectBuilder;
//using CWC.AutoTests.ObjectBuilder.DailyDataBuilders;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;
//using Cwc.BaseData.Classes;

//namespace CWC.AutoTests.Tests.Transport.VisitMonitor
//{
//    [Collection("Transport Order Collection")]
//    public class VisitMonitorCancellationCodeFrameTests : IDisposable
//    {
//        DataModel.ModelContext _context;
//        string routeCode;
//        DateTime date;
//        TimeSpan arrival;
//        ServiceType serviceType;
//        Location location;
//        Site site;
//        Cwc.Ordering.Order serviceOrder;
//        DataModel.reason reason;
//        public VisitMonitorCancellationCodeFrameTests()
//        {
//            _context = new DataModel.ModelContext();
//            routeCode = "SP1001";
//            arrival = new TimeSpan(8, 0, 0);
//            serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV");
//            location = DataFacade.Location.Take(l => l.Code == "SP02").Build();
//            date = DataFacade.Order.DefineServicedate(serviceType.Code, location).Value;
//            site = DataFacade.Site.Take(s => s.ID == location.ServicingDepotID).Build();
//            serviceOrder = DataFacade.Order.New(date, location, serviceType.Code).SaveToDb();
//            reason = _context.reasons.First();
//        }

//        [Fact(DisplayName = "Cancellation dependent view - When dai_serv is matched Then System shows cancellation codes properly")]
//        public void VerifyThatSystemShowsCancellationCodesProperly()
//        {
//            var containerType = _context.bag_types.Where(b => b.bgtyp_nr == 3301).First();
//            var secondReasonCode = _context.reasons.Where(r => r.reason_cd != reason.reason_cd).First();

//            var dailyLine = DailyDataFacade.DailyStop.New()
//               .With_MasterRoute(routeCode)
//               .With_DaiDate(date)
//               .With_ArrivalTime(arrival.ToString("hhmm"))
//               .With_Location(location).With_Site(site)
//               .With_Reason(reason.ID)
//               .With_ActualDaiDate(date)
//               .With_ActualArrivalTime(arrival.ToString("hhmm"))
//               .SaveToDb();

//            var transportOrer = DataFacade.TransportOrder.New()
//                .With_Code("SP2001").With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceType)
//                .With_OrderType(OrderType.AtRequest).With_TransportDate(date).With_ServiceDate(date)
//                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
//                .With_MasterRouteDate(date).With_StopArrivalTime(arrival).SaveToDb();

//            var daiservFirst = DailyDataFacade.DaiServ
//                .With_MasterRoute(routeCode)
//                .With_Date(date)
//                .With_ArrivalDate(date)
//                .With_ArrivalTime(arrival.ToString("hhmm"))
//                .With_Location(location)
//                .With_LocationTo(location)
//                .With_Site(site)
//                .With_BagType(containerType.bgtyp_nr)
//                .With_ServType("Collect")
//                .With_ReasonCode(reason.reason_cd.Value)
//                .SaveToDb();

//            var daiservSecond = DailyDataFacade.DaiServ
//                .With_MasterRoute(routeCode)
//                .With_Date(date)
//                .With_ArrivalDate(date)
//                .With_ArrivalTime(arrival.ToString("hhmm"))
//                .With_Location(location)
//                .With_LocationTo(location)
//                .With_Site(site)
//                .With_BagType(containerType.bgtyp_nr)
//                .With_ServType("Deliver")
//                .With_ReasonCode(secondReasonCode.reason_cd.Value)
//                .SaveToDb(); ;

//            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = date, DailyRouteCode = routeCode, DailyRouteDate = date, PlannedArrivalTime = arrival };
                        
//            var query = TransportFacade.ScheduleMonitoringService.GetCancellationCodesQuery(selectedRow); 

//            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

//            Assert.Equal(2, res.Count);
//            Assert.True(res.Any(r => r[VisitMonitorCancellationCodeHelper.Service].ToString() == "Collect" && r[VisitMonitorCancellationCodeHelper.ContainerType].ToString() ==  $"{containerType.abbrev} {containerType.descript}"
//                && r[VisitMonitorCancellationCodeHelper.PdaReasonCode].ToString().StartsWith($"{reason.reason_cd} {reason.descript}")));
//            Assert.True(res.Any(r => r[VisitMonitorCancellationCodeHelper.Service].ToString() == "Deliver" && r[VisitMonitorCancellationCodeHelper.ContainerType].ToString() == $"{containerType.abbrev} {containerType.descript}"
//                && r[VisitMonitorCancellationCodeHelper.PdaReasonCode].ToString().StartsWith($"{secondReasonCode.reason_cd} {secondReasonCode.descript}")));
//        }

//        [Fact(DisplayName = "Cancellation dependent view - When found dai_serv bag type or reason code is not found Then System shows the record also")]
//        public void VerifyThatWhenBagTypeOrReasonCodeIsNotFoundThenSystemShowsRecord()
//        {
//            var dailyLine = DailyDataFacade.DailyStop.New()
//               .With_MasterRoute(routeCode)
//               .With_DaiDate(date)
//               .With_ArrivalTime(arrival.ToString("hhmm"))
//               .With_Location(location).With_Site(site)
//               .With_Reason(reason.ID)
//               .With_ActualDaiDate(date)
//               .With_ActualArrivalTime(arrival.ToString("hhmm"))
//               .SaveToDb();

//            var transportOrer = DataFacade.TransportOrder.New()
//                .With_Code("SP2001").With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceType)
//                .With_OrderType(OrderType.AtRequest).With_TransportDate(date).With_ServiceDate(date)
//                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
//                .With_MasterRouteDate(date).With_StopArrivalTime(arrival).SaveToDb();

//            var daiservFirst = DailyDataFacade.DaiServ
//                .With_MasterRoute(routeCode)
//                .With_Date(date)
//                .With_ArrivalDate(date)
//                .With_ArrivalTime(arrival.ToString("hhmm"))
//                .With_Location(location)
//                .With_LocationTo(location)
//                .With_Site(site)
//                .With_BagType(int.MaxValue)
//                .With_ServType("Collect")
//                .With_ReasonCode(int.MaxValue)
//                .SaveToDb();

//            var daiservSecond = DailyDataFacade.DaiServ
//                .With_MasterRoute(routeCode)
//                .With_Date(date)
//                .With_ArrivalDate(date)
//                .With_ArrivalTime(arrival.ToString("hhmm"))
//                .With_Location(location)
//                .With_LocationTo(location)
//                .With_Site(site)
//                .With_BagType(int.MaxValue)
//                .With_ServType("Deliver")
//                .With_ReasonCode(int.MaxValue)
//                .SaveToDb(); ;

//            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = date, DailyRouteCode = routeCode, DailyRouteDate = date, PlannedArrivalTime = arrival };

//            var query = TransportFacade.ScheduleMonitoringService.GetCancellationCodesQuery(selectedRow);

//            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

//            Assert.Equal(2, res.Count);
//            Assert.True(res.Any(r => r[VisitMonitorCancellationCodeHelper.Service].ToString() == "Collect" && r[VisitMonitorCancellationCodeHelper.ContainerType].ToString() == string.Empty
//                && r[VisitMonitorCancellationCodeHelper.PdaReasonCode].ToString() == string.Empty));
//            Assert.True(res.Any(r => r[VisitMonitorCancellationCodeHelper.Service].ToString() == "Deliver" && r[VisitMonitorCancellationCodeHelper.ContainerType].ToString() == string.Empty
//                && r[VisitMonitorCancellationCodeHelper.PdaReasonCode].ToString() == string.Empty));
//        }

//        [Fact(DisplayName = "Cancellation dependent view - When multiple transport orders contains matched dai_serv Then System shows it proprly")]
//        public void VerifyThatMultipleTransportOrderDataIsShownProperly()
//        {
//            var containerType = _context.bag_types.Where(b => b.bgtyp_nr == 3301).First();
//            var secondReasonCode = _context.reasons.Where(r => r.reason_cd != reason.reason_cd).First();
//            var servTypeSecond = DataFacade.ServiceType.Take(x => x.Code == "COLL");

//            var dailyLineFirst = DailyDataFacade.DailyStop.New()
//               .With_MasterRoute(routeCode)
//               .With_DaiDate(date)
//               .With_ArrivalTime(arrival.ToString("hhmm"))
//               .With_Location(location).With_Site(site)
//               .With_Reason(reason.ID)
//               .With_ActualDaiDate(date)
//               .With_ActualArrivalTime(arrival.ToString("hhmm"))
//               .SaveToDb();

//            var transportOrerFirst = DataFacade.TransportOrder.New()
//                .With_Code("SP2001").With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceType)
//                .With_OrderType(OrderType.AtRequest).With_TransportDate(date).With_ServiceDate(date)
//                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
//                .With_MasterRouteDate(date).With_StopArrivalTime(arrival).SaveToDb();

//            var transportOrerSecond = DataFacade.TransportOrder.New()
//                .With_Code("SP2002").With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(servTypeSecond)
//                .With_OrderType(OrderType.AtRequest).With_TransportDate(date).With_ServiceDate(date)
//                .With_Status(TransportOrderStatus.Registered).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
//                .With_MasterRouteDate(date).With_StopArrivalTime(arrival).SaveToDb();

//            var daiservFirst = DailyDataFacade.DaiServ
//                .With_MasterRoute(routeCode)
//                .With_Date(date)
//                .With_ArrivalDate(date)
//                .With_ArrivalTime(arrival.ToString("hhmm"))
//                .With_Location(location)
//                .With_LocationTo(location)
//                .With_Site(site)
//                .With_BagType(containerType.bgtyp_nr)
//                .With_ServType("Collect")
//                .With_ReasonCode(reason.reason_cd.Value)
//                .SaveToDb();

//            var daiservSecond = DailyDataFacade.DaiServ
//                .With_MasterRoute(routeCode)
//                .With_Date(date)
//                .With_ArrivalDate(date)
//                .With_ArrivalTime(arrival.ToString("hhmm"))
//                .With_Location(location)
//                .With_LocationTo(location)
//                .With_Site(site)
//                .With_BagType(containerType.bgtyp_nr)
//                .With_ServType("Deliver")
//                .With_ReasonCode(secondReasonCode.reason_cd.Value)
//                .SaveToDb(); ;

//            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = date, DailyRouteCode = routeCode, DailyRouteDate = date, PlannedArrivalTime = arrival };

//            var query = TransportFacade.ScheduleMonitoringService.GetCancellationCodesQuery(selectedRow);

//            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

//            Assert.Equal(2, res.Count);
//        }

//        public void Dispose()
//        {
//            _context.dai_lines.RemoveRange(_context.dai_lines.Where(d=>d.mast_cd == routeCode));
//            _context.dai_servs.RemoveRange(_context.dai_servs.Where(d=>d.mast_cd == routeCode));
//            _context.Cwc_Transport_TransportOrders.RemoveRange(_context.Cwc_Transport_TransportOrders);
//            _context.SaveChanges();
//            _context.Dispose();
//        }
//    }
//}
