using Cwc.Contracts;
using Cwc.Transport;
using Cwc.Transport.Classes.UserSettings;
using Cwc.Transport.Enums;
using System.Linq;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Data;
using Xunit;
using Cwc.Transport.Helpers;
using Cwc.Ordering;
using Cwc.BaseData;
using Cwc.Common.Metadata;
using Cwc.Transport.Model;
using Cwc.Transport.Classes;
using CWC.AutoTests.ObjectBuilder.DailyDataBuilders;
using Cwc.BaseData.Classes;
using Cwc.Common.Extensions.Data;

namespace CWC.AutoTests.Tests.Transport.VisitMonitor
{
    [Collection("Transport Order Collection")]
    public class VisitMonitorFrameTests : IDisposable
    {
        DataModel.ModelContext _context;
        string routeCode;
        DateTime today = DateTime.Today;
        TimeSpan arrival;
        ServiceType serviceType;
        Location location;
        Site site;
        Cwc.Ordering.Order serviceOrder;
        Cwc.BaseData.Model.ReasonCode reason;
        public VisitMonitorFrameTests()
        {
            _context = new DataModel.ModelContext();
            routeCode = "SP1001";
            arrival = new TimeSpan(8, 0, 0);
            serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            location = DataFacade.Location.Take(l => l.Code == "SP02").Build();            
            site = DataFacade.Site.Take(s => s.ID == location.ServicingDepotID).Build();
            serviceOrder = DataFacade.Order.New(today, location, serviceType.Code).SaveToDb();
            reason = BaseDataFacade.ReasonCodeService.Load(_context.reasons.First().reason_cd.Value, new Cwc.Common.DataBaseParams());
        }

        [Fact(DisplayName = "Visit Monitor Frame - When Transport Order exists Then System shows it in the group")]
        public void VerifyThatTransportOrderWithoutDailyStopIsShownProprly()
        {
            var customer = DataFacade.Customer.Take(c => c.ID == location.CompanyID).Build();

            var transportOrer = DataFacade.TransportOrder.New()
                    .With_Location(location.ID)
                    .With_Site(location.ServicingDepotID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(today)
                    .With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Planned)
                    .With_ServiceOrder(serviceOrder.ID)
                    .With_ServiceType(serviceType.ID)
                    .With_MasterRouteCode("SP1001")
                    .With_MasterRouteDate(today)
                    .With_StopArrivalTime(new TimeSpan(8, 0, 0))
                    .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(serviceOrder.GenericStatus.In(GenericStatus.Unconfirmed, GenericStatus.Confirmed) ? 1 : 0, res[0][VisitMonitorGroupHelper.IsServiceOrderNotReleased]);
            Assert.Equal(0, res[0][VisitMonitorGroupHelper.IsFixedOrder]);
            Assert.Equal(1, res[0][VisitMonitorGroupHelper.IsAtRequestOrder]);
            Assert.Equal(0, res[0][VisitMonitorGroupHelper.IsAdHocOrder]);
            Assert.Equal(serviceType.OldType == Cwc.Constants.ServiceTypeConstants.Collect ? 1 : 0, res[0][VisitMonitorGroupHelper.IsCollect]);
            Assert.Equal(serviceType.OldType == Cwc.Constants.ServiceTypeConstants.Deliver ? 1 : 0, res[0][VisitMonitorGroupHelper.IsDelivery]);
            Assert.Equal(serviceType.OldType == Cwc.Constants.ServiceTypeConstants.Servicing ? 1 : 0, res[0][VisitMonitorGroupHelper.IsServicing]);
            Assert.Equal(serviceType.OldType == Cwc.Constants.ServiceTypeConstants.Replenishment ? 1 : 0, res[0][VisitMonitorGroupHelper.IsReplenishment]);
            Assert.Equal(location.ID, res[0][VisitMonitorGroupHelper.LocationID]);
            Assert.Equal(customer.DisplayCaption, res[0][VisitMonitorGroupHelper.CustomerDisplayCaption]);
            Assert.Equal(today, res[0][VisitMonitorGroupHelper.TransportDate]);
            Assert.Equal(location.ServicingDepotID, res[0][VisitMonitorGroupHelper.SiteID]);
            Assert.Equal(string.Empty, (res[0][VisitMonitorGroupHelper.DailyRouteCode].ToString()));
            Assert.Equal(string.Empty, (res[0][VisitMonitorGroupHelper.DailyRouteDate].ToString()));
            Assert.Equal(string.Empty, (res[0][VisitMonitorGroupHelper.ActualArrivalDisplayCaption].ToString()));
            Assert.Equal(string.Empty, (res[0][VisitMonitorGroupHelper.StopStatus].ToString()));
            Assert.Equal(string.Empty, (res[0][VisitMonitorGroupHelper.PdaReasonCode].ToString()));
            Assert.Equal(string.Empty, (res[0][VisitMonitorGroupHelper.FinalReasonCode].ToString()));
            Assert.Equal(string.Empty, (res[0][VisitMonitorGroupHelper.Remark].ToString()));
        }

        [Theory(DisplayName = "Visit Monitor Frame - When Transport Order is type Then System shows appropriate icon")]
        [InlineData("COLL", 1, 0, 0, 0)]
        [InlineData("DELV", 0, 1, 0, 0)]
        [InlineData("SERV", 0, 0, 1, 0)]
        [InlineData("REPL", 0, 0, 0, 1)]
        public void VerifyThatServiceTypeIsShownProperly(string serType, int collection, int deliver, int servicing, int replenish)
        {
            serviceType = DataFacade.ServiceType.Take(x => x.Code == serType).Build();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(location.ID)
                .With_Site(location.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(today)
                .With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder.ID)
                .With_ServiceType(serviceType.ID)
                .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, TransportOrderCode = transportOrer.Build().Code };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(collection, res[0][VisitMonitorGroupHelper.IsCollect]);
            Assert.Equal(deliver, res[0][VisitMonitorGroupHelper.IsDelivery]);
            Assert.Equal(servicing, res[0][VisitMonitorGroupHelper.IsServicing]);
            Assert.Equal(replenish, res[0][VisitMonitorGroupHelper.IsReplenishment]);
        }

        [Fact(DisplayName = "Visit Monitor Frame - When group contains all servrice types Then System shows all of them")]
        public void VerifyThatAllGroupServtypesAreShown()
        {
            var transportOrerColl = DataFacade.TransportOrder.New()
                    .With_Location(location.ID)
                    .With_Site(location.ServicingDepotID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(today)
                    .With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Planned)
                    .With_ServiceOrder(serviceOrder.ID)
                    .With_ServiceType(DataFacade.ServiceType.Take(x => x.Code == "COLL").Build().ID)
                    .SaveToDb();

            var transportOrerDelv = DataFacade.TransportOrder.New()
                    .With_Location(location.ID)
                    .With_Site(location.ServicingDepotID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(today)
                    .With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Planned)
                    .With_ServiceOrder(serviceOrder.ID)
                    .With_ServiceType(DataFacade.ServiceType.Take(x => x.Code == "DELV").Build().ID)
                    .SaveToDb();

            var transportOrerServ = DataFacade.TransportOrder.New()
                    .With_Location(location.ID)
                    .With_Site(location.ServicingDepotID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(today)
                    .With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Planned)
                    .With_ServiceOrder(serviceOrder.ID)
                    .With_ServiceType(DataFacade.ServiceType.Take(x => x.Code == "SERV").Build().ID)
                    .SaveToDb();

            var transportOrerRepl = DataFacade.TransportOrder.New()
                    .With_Location(location.ID)
                    .With_Site(location.ServicingDepotID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(today)
                    .With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Planned)
                    .With_ServiceOrder(serviceOrder.ID)
                    .With_ServiceType(DataFacade.ServiceType.Take(x => x.Code == "REPL").Build().ID)
                    .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(1, res[0][VisitMonitorGroupHelper.IsCollect]);
            Assert.Equal(1, res[0][VisitMonitorGroupHelper.IsDelivery]);
            Assert.Equal(1, res[0][VisitMonitorGroupHelper.IsServicing]);
            Assert.Equal(1, res[0][VisitMonitorGroupHelper.IsReplenishment]);
        }

        [Theory(DisplayName = "Visit Monitor Frame - When Transport Order have order type Then System shows appropriate icon")]
        [InlineData(OrderType.AdHoc, 1, 0, 0)]
        [InlineData(OrderType.AtRequest, 0, 1, 0)]
        [InlineData(OrderType.Fixed, 0, 0, 1)]
        public void VerifyThatOrdertypeIsShownProperly(OrderType orderType, int adhoc, int atreq, int fix)
        {         

            var transportOrer = DataFacade.TransportOrder.New()
                    .With_Location(location.ID)
                    .With_Site(location.ServicingDepotID)
                    .With_OrderType(orderType)
                    .With_TransportDate(today)
                    .With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Planned)
                    .With_ServiceOrder(serviceOrder.ID)
                    .With_ServiceType(serviceType.ID)
                    .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, TransportOrderCode = transportOrer.Build().Code };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(adhoc, res[0][VisitMonitorGroupHelper.IsAdHocOrder]);
            Assert.Equal(atreq, res[0][VisitMonitorGroupHelper.IsAtRequestOrder]);
            Assert.Equal(fix, res[0][VisitMonitorGroupHelper.IsFixedOrder]);
        }

        [Fact(DisplayName = "Visit Monitor Frame - When group contain all order types Then System shows it")]
        public void VerifyThatAllOrderTypesAreShown()
        {

            var transportOrerAd = DataFacade.TransportOrder.New()
                        .With_Location(location.ID)
                        .With_Site(location.ServicingDepotID)
                        .With_OrderType(OrderType.AdHoc)
                        .With_TransportDate(today)
                        .With_ServiceDate(today)
                        .With_Status(TransportOrderStatus.Planned)
                        .With_ServiceOrder(serviceOrder.ID)
                        .With_ServiceType(serviceType.ID)
                        .SaveToDb();


            var transportOrerAt = DataFacade.TransportOrder.New()
                        .With_Location(location.ID)
                        .With_Site(location.ServicingDepotID)
                        .With_OrderType(OrderType.AtRequest)
                        .With_TransportDate(today)
                        .With_ServiceDate(today)
                        .With_Status(TransportOrderStatus.Planned)
                        .With_ServiceOrder(serviceOrder.ID)
                        .With_ServiceType(serviceType.ID)
                        .SaveToDb();

            var transportOrerFix = DataFacade.TransportOrder.New()
                        .With_Location(location.ID)
                        .With_Site(location.ServicingDepotID)
                        .With_OrderType(OrderType.Fixed)
                        .With_TransportDate(today)
                        .With_ServiceDate(today)
                        .With_Status(TransportOrderStatus.Planned)
                        .With_ServiceOrder(serviceOrder.ID)
                        .With_ServiceType(serviceType.ID)
                        .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo};

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(1, res[0][VisitMonitorGroupHelper.IsAdHocOrder]);
            Assert.Equal(1, res[0][VisitMonitorGroupHelper.IsAtRequestOrder]);
            Assert.Equal(1, res[0][VisitMonitorGroupHelper.IsFixedOrder]);
        }

        [Theory(DisplayName = "Visit Monitor Frame - When Service Order is in {Unconfirmed, Confirmed} Then System shows warning Not Releasde message")]
        [InlineData(GenericStatus.Confirmed)]
        [InlineData(GenericStatus.Unconfirmed)]
        public void VerifyThatNotReleasedMessageIsShownProprly(GenericStatus status)
        {

            var transportOrer = DataFacade.TransportOrder.New()
                    .With_Location(location.ID)
                    .With_Site(location.ServicingDepotID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(today)
                    .With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Planned)
                    .With_ServiceOrder(serviceOrder.ID)
                    .With_ServiceType(serviceType.ID)
                    .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, TransportOrderCode = transportOrer.Build().Code };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(1, res[0][VisitMonitorGroupHelper.IsServiceOrderNotReleased]);
        }

        [Fact(DisplayName = "Visit Monitor Frame - When Daily Stop is found for Transport Order Then System shows Daily Stop data for group")]
        public void VerifyThatSystemMatchesDailyStopProprly()
        {
            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmmss"))
                .SaveToDb();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(location.ID)
                .With_Site(location.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(today)
                .With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder.ID)
                .With_ServiceType(serviceType.ID)
                .With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today)
                .With_StopArrivalTime(arrival)
                .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, TransportOrderCode = transportOrer.Build().Code };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(routeCode, res[0][VisitMonitorGroupHelper.DailyRouteCode]);
            Assert.Equal(today, res[0][VisitMonitorGroupHelper.DailyRouteDate]);
            Assert.Equal(arrival, res[0][VisitMonitorGroupHelper.PlannedArrival]);
            Assert.Equal(arrival.ToString(), res[0][VisitMonitorGroupHelper.ActualArrivalDisplayCaption].ToString());
            Assert.Equal($"{reason.DisplayCaption}", res[0][VisitMonitorGroupHelper.PdaReasonCode]);
        }

        [Fact(DisplayName = "Visit Monitor Frame - When Daily Stop - Date is not equal to TO - date Then System doesn't match this dailsy stop")]
        public void VerifyThatDailyStopWithNonEqualDateCannotBeMatched()
        {

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode)
                .With_DaiDate(today.AddDays(1))
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(location.ID)
                .With_Site(location.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(today)
                .With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder.ID)
                .With_ServiceType(serviceType.ID)
                .With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today)
                .With_StopArrivalTime(arrival)
                .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, TransportOrderCode = transportOrer.Build().Code };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(string.Empty, (res[0][VisitMonitorGroupHelper.DailyRouteCode].ToString()));
        }

        [Fact(DisplayName = "Visit Monitor Frame - When Daily Stop - time is not equal to TO - time Then System doesn't match this dailsy stop")]
        public void VerifyThatDailyStopWithNonEqualTimeCannotBeMatched()
        {

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.Add(new TimeSpan(0, 1, 0)).ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var transportOrer = DataFacade.TransportOrder.New()
                    .With_Location(location.ID)
                    .With_Site(location.ServicingDepotID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(today)
                    .With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Planned)
                    .With_ServiceOrder(serviceOrder.ID)
                    .With_ServiceType(serviceType.ID)
                    .With_MasterRouteCode(routeCode)
                    .With_MasterRouteDate(today)
                    .With_StopArrivalTime(arrival)
                    .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, TransportOrderCode = transportOrer.Build().Code };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(string.Empty, (res[0][VisitMonitorGroupHelper.DailyRouteCode].ToString()));
        }

        [Fact(DisplayName = "Visit Monitor Frame - When Daily Stop is not found Then Transport Status is empty")]
        public void VerifyThatWhenDailyStopIsNotFoundThenTransportStatusIsEmpty()
        {
            var customer = DataFacade.Customer.Take(c => c.ID == location.CompanyID).Build();

            var transportOrer = DataFacade.TransportOrder.New()
                    .With_Location(location.ID)
                    .With_Site(location.ServicingDepotID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(today)
                    .With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Planned)
                    .With_ServiceOrder(serviceOrder.ID)
                    .With_ServiceType(DataFacade.ServiceType.Take(x => x.Code == "COLL").Build().ID)
                    .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(string.Empty, res[0][VisitMonitorGroupHelper.StopStatus].ToString());
        }

        [Fact(DisplayName = "Visit Monitor Frame - When at least one trasnport order is planned Then Stop Status is planned")]
        public void VerifyThatWhenAtLeastTransportOrderIsPlannedThenStopStatusIsPlanned()
        {
            var serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();
            var serviceTypeServ = DataFacade.ServiceType.Take(x => x.Code == "SERV").Build();
            var serviceTypeRepl = DataFacade.ServiceType.Take(x => x.Code == "REPL").Build();

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var transportOrerPl = DataFacade.TransportOrder.New()
                .With_Location(location.ID) .With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeDelv.ID)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var transportOrerInTr = DataFacade.TransportOrder.New()
                .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeColl.ID)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.InTransit).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var transportOrerToClCom = DataFacade.TransportOrder.New()
                .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeServ.ID)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.ToClarifyCompletion).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var transportOrerTCompl = DataFacade.TransportOrder.New()
                .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeRepl.ID)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Completed).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal("Planned", res[0][VisitMonitorGroupHelper.StopStatus]);
        }

        [Fact(DisplayName = "Visit Monitor Frame - When at least one status is in transit then system shows group status = in transit")]
        public void VerifyWhenAtLEastOneOrderIsInTransThenStopStatusIsInTransot()
        {
            var serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();
            var serviceTypeServ = DataFacade.ServiceType.Take(x => x.Code == "SERV").Build();

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var transportOrerInTr = DataFacade.TransportOrder.New()
                    .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeDelv.ID)
                    .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.InTransit).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                    .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var transportOrerToClCan = DataFacade.TransportOrder.New()
                    .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeColl.ID)
                    .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.ToClarifyCancellation).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                    .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var transportOrerTCompl = DataFacade.TransportOrder.New()
                    .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeServ.ID)
                    .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Completed).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                    .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal("In transit", res[0][VisitMonitorGroupHelper.StopStatus]);
        }

        [Theory(DisplayName = "Visit Monitor Frame - When Transport Status is To Clarify .. Then System shows status To Clarify")]
        [InlineData(TransportOrderStatus.ToClarifyCancellation)]
        [InlineData(TransportOrderStatus.ToClarifyCompletion)]
        [InlineData(TransportOrderStatus.ToClarifyNewAdhocOrder)]
        public void VerifyThatWhenStatusToClarThenStopStatusIsToClarify(TransportOrderStatus status)
        {
            var serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var transportOrerToClCan = DataFacade.TransportOrder.New()
                        .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeDelv.ID)
                        .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                        .With_Status(status).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                        .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var transportOrerTCompl = DataFacade.TransportOrder.New()
                        .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeColl.ID)
                        .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                        .With_Status(TransportOrderStatus.Completed).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                        .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal("To clarify", res[0][VisitMonitorGroupHelper.StopStatus]);
        }

        [Fact(DisplayName = "Visit Monitor Frame - When at least transport order is Completed Then Stop Status is completed")]
        public void WhenStatusIsCompletedThenStopStatusIsCompleted()
        {
            var serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();

            var dailyLine = DailyDataFacade.DailyStop.New()
                 .With_MasterRoute(routeCode)
                 .With_DaiDate(today)
                 .With_ArrivalTime(arrival.ToString("hhmm"))
                 .With_Location(location).With_Site(site)
                 .With_Reason(reason.ID)
                 .With_ActualDaiDate(today)
                 .With_ActualArrivalTime(arrival.ToString("hhmm"))
                 .SaveToDb();

            var transportOrerToClCan = DataFacade.TransportOrder.New()
                        .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeDelv.ID)
                        .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                        .With_Status(TransportOrderStatus.Completed).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                        .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var transportOrerTCompl = DataFacade.TransportOrder.New()
                        .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeColl.ID)
                        .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                        .With_Status(TransportOrderStatus.Cancelled).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                        .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal("Completed", res[0][VisitMonitorGroupHelper.StopStatus]);
        }

        [Fact(DisplayName = "Visit Monitor Frame - When Transport Status is Cancelled Then Stop Status is Cancelled")]
        public void VerifWhenStatusIscancelledThenTransportStatusIsCancelled()
        {
            var serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var transportOrerToClCan = DataFacade.TransportOrder.New()
                .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeDelv.ID)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Cancelled).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var transportOrerTCompl = DataFacade.TransportOrder.New()
                .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeColl.ID)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Cancelled).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal("Cancelled", res[0][VisitMonitorGroupHelper.StopStatus]);
        }

        [Fact(DisplayName = "Visit Monitor Frame - When locations are diff (rest equal) Then System creates 2 groups")]
        public void VerifyThatSystemGroupsProperlyWithDiffLocations()
        {
            var serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();
            var firstLocation = DataFacade.Location.Take(l => l.Code == "SP02").Build();
            var secondLocation = DataFacade.Location.New().InitDefault().With_ServicingDepotID(firstLocation.BranchID).SaveToDb().Build();

            var firstTransportOrer = DataFacade.TransportOrder.New()
                                                        .With_Location(firstLocation.ID)
                                                        .With_Site(firstLocation.ServicingDepotID)
                                                        .With_OrderType(OrderType.AtRequest)
                                                        .With_TransportDate(today)
                                                        .With_ServiceDate(today)
                                                        .With_Status(TransportOrderStatus.Planned)
                                                        .With_ServiceOrder(serviceOrder.ID)
                                                        .With_ServiceType(serviceTypeDelv.ID)
                                                        .SaveToDb();

            var secondTransportOrer = DataFacade.TransportOrder.New()
                                                        .With_Location(secondLocation.ID)
                                                        .With_Site(secondLocation.ServicingDepotID)
                                                        .With_OrderType(OrderType.AtRequest)
                                                        .With_TransportDate(today)
                                                        .With_ServiceDate(today)
                                                        .With_Status(TransportOrderStatus.Planned)
                                                        .With_ServiceOrder(serviceOrder.ID)
                                                        .With_ServiceType(serviceTypeColl.ID)
                                                        .SaveToDb();

            var settings = new VisitMonitorsFilterSetting {StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, ServiceOrderNumber = serviceOrder.ID };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(2, res.Count);
        }

        [Fact(DisplayName = "Visit Monitor Frame - WHen Transport Order has diff transport day Then System groups them separately")]
        public void VerifyThatTransportOrderWithTheSameLocationsAndDiffTransDateGroupsSeparately()
        {
            var serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();
            var firstLocation = DataFacade.Location.Take(l => l.Code == "SP02").Build();

            var firstTransportOrer = DataFacade.TransportOrder.New()
                .With_Location(firstLocation.ID)
                .With_Site(firstLocation.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(today)
                .With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder.ID)
                .With_ServiceType(serviceTypeDelv.ID)
                .SaveToDb();

            var secondTransportOrer = DataFacade.TransportOrder.New()
                .With_Location(firstLocation.ID)
                .With_Site(firstLocation.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(today.AddDays(1))
                .With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder.ID)
                .With_ServiceType(serviceTypeColl.ID)
                .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { StartPeriod = today, EndPeriod = today.AddDays(1), OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, ServiceOrderNumber = serviceOrder.ID };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(2, res.Count);
        }

        [Fact(DisplayName = "Visit Monitor Frame - When Orders has diff master route Then system groups them separately")]
        public void VerifyThatOrdersWithDissMasterRouteGroupsSeparately()
        {
            var routeCode1 = "SP1001";
            var routeCode2 = "SP1002";
            var serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode1)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var transportOrerFirst = DataFacade.TransportOrder.New()
                    .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeDelv.ID)
                    .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode1)
                    .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var dailyLine2 = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode2)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var transportOrerTSecond = DataFacade.TransportOrder.New()
                    .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeColl.ID)
                    .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Completed).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode2)
                    .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(2, res.Count);
        }

        [Fact(DisplayName = "Visit Monitor Frame - When trasnport orders time is diff Then System groupes them separately")]
        public void VerifyThatTransportOrderWithDiffTimeAreGroupedSeparately()
        {
            var arrival1 = new TimeSpan(8, 0, 0);
            var arrival2 = new TimeSpan(8, 1, 0);
            var serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival1.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival1.ToString("hhmm"))
                .SaveToDb();

            var transportOrerFirst = DataFacade.TransportOrder.New()
                .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeDelv.ID)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival1).SaveToDb();

            var dailyLine2 = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival2.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival2.ToString("hhmm"))
                .SaveToDb();

            var transportOrerTSecond = DataFacade.TransportOrder.New()
                .With_Location(location.ID).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeColl.ID)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Completed).With_ServiceOrder(serviceOrder.ID).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival2).SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(2, res.Count);
        }

        [Theory(DisplayName = "Visit Monitor Frame - When for transport order exists cit processing history with exception Then System shows it's reason code")]
        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit)]
        [InlineData(ExceptionCaseName.StopCancellingDuringRoute)]
        public void VerifyThtFinalReasonCodeIsShownProperly(ExceptionCaseName name)
        {
            var exceptionCaseId = _context.Cwc_Transport_ExceptionCases.Where(e => e.Name == (int)name).First();

            var transportOrer = DataFacade.TransportOrder.New()
                            .With_Location(location.ID)
                            .With_Site(location.ServicingDepotID)
                            .With_OrderType(OrderType.AtRequest)
                            .With_TransportDate(today)
                            .With_ServiceDate(today)
                            .With_Status(TransportOrderStatus.Planned)
                            .With_ServiceOrder(serviceOrder.ID)
                            .With_ServiceType(serviceType.ID)
                            .SaveToDb();

            var citProcessingHistory = DataFacade.CitProcessingHistory.New()
                            .With_ProcessName(ProcessName.PerformDailyRoute)
                            .With_ProcessPhase(ProcessPhase.End)
                            .With_Status(1)
                            .With_IsWithException(true)
                            .With_DateCreated(today)
                            .With_ObjectID(transportOrer.Build().ID)
                            .With_ObjectClassID(MetadataHelper.GetClassID(typeof(TransportOrder)))
                            .With_AuthorID(_context.WP_Users.First().id)
                            .With_WorkstationID(_context.WP_BaseData_Workstations.First().id)
                            .SaveToDb();

            var citProcessHistoryException = DataFacade.CitProcessingHistoryException.New()
                            .With_Remark("remark")
                            .With_Action(ExceptionAction.ConfirmCancellation)
                            .With_Status(ProcessingHistoryExceptionStatus.Registered)
                            .With_DateResolved(today)
                            .With_Exception(exceptionCaseId.ID)
                            .With_CitProcessingHistoryID(citProcessingHistory.Build().ID)
                            .With_ReasonCodeID(reason.IdentityID)
                            .With_SiteID(_context.branches.First().branch_nr)
                            .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, TransportOrderCode = transportOrer.Build().Code };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal("remark", res[0][VisitMonitorGroupHelper.Remark]);
            Assert.True(res[0][VisitMonitorGroupHelper.FinalReasonCode].ToString().StartsWith($"{reason.ID} {reason.Description}"));
        }

        [Fact(DisplayName = "Visit Monitor Frame - When ExceptionCaseName is NotPlannedOrderIsPerformed Then System shows Final Reason Code but not remark")]
        public void VerifyThatSystemShowsFinalReasonCodeButNotRemark()
        {
            var exceptionCaseId = _context.Cwc_Transport_ExceptionCases.Where(e => e.Name == (int)ExceptionCaseName.NotPlannedOrderIsPerformed).First();

            var transportOrer = DataFacade.TransportOrder.New()
                    .With_Location(location.ID)
                    .With_Site(location.ServicingDepotID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(today)
                    .With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Planned)
                    .With_ServiceOrder(serviceOrder.ID)
                    .With_ServiceType(serviceType.ID)
                    .SaveToDb();

            var citProcessingHistory = DataFacade.CitProcessingHistory.New()
                    .With_ProcessName(ProcessName.PerformDailyRoute)
                    .With_ProcessPhase(ProcessPhase.End)
                    .With_Status(1)
                    .With_IsWithException(true)
                    .With_DateCreated(today)
                    .With_ObjectID(transportOrer.Build().ID)
                    .With_ObjectClassID(MetadataHelper.GetClassID(typeof(TransportOrder)))
                    .With_AuthorID(_context.WP_Users.First().id)
                    .With_WorkstationID(_context.WP_BaseData_Workstations.First().id)
                    .SaveToDb();

            var citProcessHistoryException = DataFacade.CitProcessingHistoryException.New()
                    .With_Remark("remark")
                    .With_Action(ExceptionAction.ConfirmCancellation)
                    .With_Status(ProcessingHistoryExceptionStatus.Registered)
                    .With_DateResolved(today)
                    .With_Exception(exceptionCaseId.ID)
                    .With_CitProcessingHistoryID(citProcessingHistory.Build().ID)
                    .With_ReasonCodeID(reason.IdentityID)
                    .With_SiteID(_context.branches.First().branch_nr)
                    .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, TransportOrderCode = transportOrer.Build().Code };

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(string.Empty, res[0][VisitMonitorGroupHelper.Remark].ToString());
            Assert.True(res[0][VisitMonitorGroupHelper.FinalReasonCode].ToString().StartsWith($"{reason.ID} {reason.Description}"));
        }

        [Fact(DisplayName = "Visit Monitor Frame - When multiple unique final reason codes are found Then Systemm shows all of them")]
        public void VerifyThatSystemShowsAllUniqueFinalReasonCodes()
        {
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var serviceTypeSecond = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();
            var exceptionCaseIdFirst = _context.Cwc_Transport_ExceptionCases.Where(e => e.Name == (int)ExceptionCaseName.NotExecutedServiceDuringVisit).First();
            var exceptionCaseIdSecond = _context.Cwc_Transport_ExceptionCases.Where(e => e.Name == (int)ExceptionCaseName.NotExecutedServiceDuringVisit).First();
            var reasonFirst = reason;
            var reasonSecond = BaseDataFacade.ReasonCodeService.Load(_context.reasons.First(x=>x.reason_cd != reasonFirst.ID).reason_cd.Value, new Cwc.Common.DataBaseParams());

            #region TransportOrders
            var transportOrerFirst = DataFacade.TransportOrder.New()
                     .With_Location(location.ID)
                     .With_Site(location.ServicingDepotID)
                     .With_OrderType(OrderType.AtRequest)
                     .With_TransportDate(today)
                     .With_ServiceDate(today)
                     .With_Status(TransportOrderStatus.Planned)
                     .With_ServiceOrder(serviceOrder.ID)
                     .With_ServiceType(serviceType.ID)
                     .SaveToDb();

            var transportOrerSecond = DataFacade.TransportOrder.New()
                 .With_Location(location.ID)
                 .With_Site(location.ServicingDepotID)
                 .With_OrderType(OrderType.AtRequest)
                 .With_TransportDate(today)
                 .With_ServiceDate(today)
                 .With_Status(TransportOrderStatus.Planned)
                 .With_ServiceOrder(serviceOrder.ID)
                 .With_ServiceType(serviceTypeSecond.ID)
                 .SaveToDb();
            #endregion

            #region CitProcessingHistory
            var citProcessingHistoryFirst = DataFacade.CitProcessingHistory.New()
                    .With_ProcessName(ProcessName.PerformDailyRoute)
                    .With_ProcessPhase(ProcessPhase.End)
                    .With_Status(1)
                    .With_IsWithException(true)
                    .With_DateCreated(today)
                    .With_ObjectID(transportOrerFirst.Build().ID)
                    .With_ObjectClassID(MetadataHelper.GetClassID(typeof(TransportOrder)))
                    .With_AuthorID(_context.WP_Users.First().id)
                    .With_WorkstationID(_context.WP_BaseData_Workstations.First().id)
                    .SaveToDb();

            var citProcessingHistorySecond = DataFacade.CitProcessingHistory.New()
                .With_ProcessName(ProcessName.PerformDailyRoute)
                .With_ProcessPhase(ProcessPhase.End)
                .With_Status(1)
                .With_IsWithException(true)
                .With_DateCreated(today)
                .With_ObjectID(transportOrerSecond.Build().ID)
                .With_ObjectClassID(MetadataHelper.GetClassID(typeof(TransportOrder)))
                .With_AuthorID(_context.WP_Users.First().id)
                .With_WorkstationID(_context.WP_BaseData_Workstations.First().id)
                .SaveToDb();
            #endregion

            #region CitProcessingHistoryException
            var citProcessHistoryExceptionFirst = DataFacade.CitProcessingHistoryException.New()
                    .With_Remark("remark0")
                    .With_Action(ExceptionAction.ConfirmCancellation)
                    .With_Status(ProcessingHistoryExceptionStatus.Registered)
                    .With_DateResolved(today)
                    .With_Exception(exceptionCaseIdFirst.ID)
                    .With_CitProcessingHistoryID(citProcessingHistoryFirst.Build().ID)
                    .With_ReasonCodeID(reasonFirst.IdentityID)
                    .With_SiteID(_context.branches.First().branch_nr)
                    .SaveToDb();

            var citProcessHistoryException = DataFacade.CitProcessingHistoryException.New()
                .With_Remark("remark1")
                .With_Action(ExceptionAction.ConfirmCancellation)
                .With_Status(ProcessingHistoryExceptionStatus.Registered)
                .With_DateResolved(today)
                .With_Exception(exceptionCaseIdSecond.ID)
                .With_CitProcessingHistoryID(citProcessingHistoryFirst.Build().ID)
                .With_ReasonCodeID(reasonSecond.IdentityID)
                .With_SiteID(_context.branches.First().branch_nr)
                .SaveToDb();

            var citProcessHistoryExceptionSecond = DataFacade.CitProcessingHistoryException.New()
                .With_Remark("remark2")
                .With_Action(ExceptionAction.ConfirmCancellation)
                .With_Status(ProcessingHistoryExceptionStatus.Registered)
                .With_DateResolved(today)
                .With_Exception(exceptionCaseIdSecond.ID)
                .With_CitProcessingHistoryID(citProcessingHistorySecond.Build().ID)
                .With_ReasonCodeID(reasonSecond.IdentityID)
                .With_SiteID(_context.branches.First().branch_nr)
                .SaveToDb(); 
            #endregion

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo};

            var query = TransportFacade.ScheduleMonitoringService.GetVisitMonitorQuery(settings);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(res[0][VisitMonitorGroupHelper.FinalReasonCode].ToString(), $"{reasonSecond.DisplayCaption}, {reasonFirst.DisplayCaption}");
        }

        [Fact(DisplayName = "Visit Monitor Frame - When group contains multiple transport orders Then System shows all of them in inline grid")]
        public void VerifyThatSystemShowsInlineTransportOrdersProperly()
        {

            var transportOrerFirst = DataFacade.TransportOrder.New()
                                                        .With_Location(location.ID)
                                                        .With_Site(location.ServicingDepotID)
                                                        .With_OrderType(OrderType.AtRequest)
                                                        .With_TransportDate(today)
                                                        .With_ServiceDate(today)
                                                        .With_Status(TransportOrderStatus.Planned)
                                                        .With_ServiceOrder(serviceOrder.ID)
                                                        .With_ServiceType(serviceType.ID)
                                                        .SaveToDb();

            var transportOrerSecond = DataFacade.TransportOrder.New()
                                                        .With_Location(location.ID)
                                                        .With_Site(location.ServicingDepotID)
                                                        .With_OrderType(OrderType.Fixed)
                                                        .With_TransportDate(today)
                                                        .With_ServiceDate(today)
                                                        .With_Status(TransportOrderStatus.Registered)
                                                        .With_ServiceOrder(serviceOrder.ID)
                                                        .With_ServiceType(serviceType.ID)
                                                        .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };
            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today };

            var query = TransportFacade.ScheduleMonitoringService.GetTransportOrdersQuery(settings, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(2, res.Count);
        }

        [Fact(DisplayName = "Visit Monitor Frame - When Transport Order is matched Then System shows it's data properly")]
        public void VerifyThatSystemShowsInlineDataProperly()
        {

            var transportOrerFirst = DataFacade.TransportOrder.New()
                                                        .With_Location(location.ID)
                                                        .With_Site(location.ServicingDepotID)
                                                        .With_OrderType(OrderType.AtRequest)
                                                        .With_TransportDate(today)
                                                        .With_ServiceDate(today)
                                                        .With_Status(TransportOrderStatus.Planned)
                                                        .With_ServiceOrder(serviceOrder.ID)
                                                        .With_ServiceType(serviceType.ID)
                                                        .With_IsWithException(true)
                                                        .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo };
            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today };

            var query = TransportFacade.ScheduleMonitoringService.GetTransportOrdersQuery(settings, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "No results are returned");
            Assert.Equal(transportOrerFirst.Build().Code, res[0][VisitMonitorTransportOrderHelper.Code]);
            Assert.Equal(today, res[0][VisitMonitorTransportOrderHelper.ServiceDate]);
            Assert.Equal(serviceType.DisplayCaption, res[0][VisitMonitorTransportOrderHelper.ServiceType]);
            Assert.Equal((int)OrderType.AtRequest, res[0][VisitMonitorTransportOrderHelper.OrderType]);
            Assert.Equal((int)TransportOrderStatus.Planned, res[0][VisitMonitorTransportOrderHelper.TransportOrderStatus]);
            Assert.Equal(true, res[0][VisitMonitorTransportOrderHelper.WithException]);
            Assert.Equal(serviceOrder.ID, res[0][VisitMonitorTransportOrderHelper.LinkedToServiceOrder]);
            Assert.Equal(serviceOrder.ReferenceID == null ? "" : serviceOrder.ReferenceID, res[0][VisitMonitorTransportOrderHelper.ReferenceID].ToString());
        }

        public void Dispose()
        {
            _context.Cwc_Transport_TransportOrderProducts.RemoveRange(_context.Cwc_Transport_TransportOrderProducts);
            _context.Cwc_Transport_TransportOrderServs.RemoveRange(_context.Cwc_Transport_TransportOrderServs);
            _context.SaveChanges();

            _context.Cwc_Transport_TransportOrders.RemoveRange(_context.Cwc_Transport_TransportOrders);
            _context.dai_lines.RemoveRange(_context.dai_lines.Where(d => d.mast_cd.StartsWith("SP")));
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
