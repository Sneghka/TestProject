using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.Common.Metadata;
using Cwc.Contracts;
using Cwc.Ordering;
using Cwc.Transport;
using Cwc.Transport.Classes;
using Cwc.Transport.Classes.UserSettings;
using Cwc.Transport.Enums;
using Cwc.Transport.Model;
using Cwc.Transport.Web.Actions;
using Cwc.Transport.Web.Forms.UserControls;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.DailyDataBuilders;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.Transport.VisitMonitor
{
    [Collection("Transport Order Collection")]
    public class EditVisitMonitorFormTests : IDisposable
    {
        AutomationTransportDataContext transportContext;
        AutomationBaseDataContext baseDataContext;
        DateTime today = DateTime.Today;
        ServiceType serviceTypeDelv;
        Location location;
        Order serviceOrder;
        Site site;
        string masterRoute;
        int transportOrderClassId;

        public EditVisitMonitorFormTests()
        {
            transportContext = new AutomationTransportDataContext();
            baseDataContext = new AutomationBaseDataContext();                        
            serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            location = DataFacade.Location.Take(l => l.Code == "SP02").Build();            
            serviceOrder = DataFacade.Order.New(today, location, serviceTypeDelv.Code).SaveToDb().Build();
            masterRoute = "SP20001";
            transportOrderClassId = MetadataHelper.GetClassID(typeof(TransportOrder)).Value;
            site = DataFacade.Site.Take(s => s.ID == location.BranchID).Build();
        }

        [Theory(DisplayName = "Edit Visit Form - When for transport order group exists exceptions Then System shows all of them on Edit Visit form")]
        [InlineData(ExceptionCaseName.StopCancellingDuringRoute, ProcessingHistoryExceptionStatus.InProgress, 1)]
        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ProcessingHistoryExceptionStatus.InProgress, 1)]
        [InlineData(ExceptionCaseName.NotPlannedOrderIsPerformed, ProcessingHistoryExceptionStatus.InProgress, 1)]
        [InlineData(ExceptionCaseName.StopCancellingDuringRoute, ProcessingHistoryExceptionStatus.Registered, 0)]
        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ProcessingHistoryExceptionStatus.Registered, 0)]
        [InlineData(ExceptionCaseName.NotPlannedOrderIsPerformed, ProcessingHistoryExceptionStatus.Registered, 0)]
        [InlineData(ExceptionCaseName.StopCancellingDuringRoute, ProcessingHistoryExceptionStatus.Resolved, 0)]
        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ProcessingHistoryExceptionStatus.Resolved, 0)]
        [InlineData(ExceptionCaseName.NotPlannedOrderIsPerformed, ProcessingHistoryExceptionStatus.Resolved, 0)]
        public void VerifyThatSystemCollectsExceptionsPorprly(ExceptionCaseName name, ProcessingHistoryExceptionStatus status, int shouldbe)
        { 
            var exceptionCaseId = transportContext.ExceptionCases.Where(e => e.Name == name).First();
            var reason = transportContext.ReasonCodes.First();

            var transportOrer = DataFacade.TransportOrder.New()
                                .With_Location(location.ID)
                                .With_Site(location.ServicingDepotID)
                                .With_OrderType(OrderType.AtRequest)
                                .With_TransportDate(today)
                                .With_MasterRouteCode(masterRoute)
                                .With_ServiceDate(today)
                                .With_Status(TransportOrderStatus.Planned)
                                .With_ServiceOrder(serviceOrder.ID)
                                .With_ServiceType(serviceTypeDelv.ID)
                                .SaveToDb();

            var citProcessingHistory = DataFacade.CitProcessingHistory.New()
                                      .With_ProcessName(ProcessName.PerformDailyRoute)
                                      .With_ProcessPhase(ProcessPhase.End)
                                      .With_Status(1)
                                      .With_IsWithException(true)
                                      .With_DateCreated(today)
                                      .With_ObjectID(transportOrer.Build().ID)
                                      .With_ObjectClassID(MetadataHelper.GetClassID(typeof(TransportOrder)))
                                      .With_AuthorID(baseDataContext.UserAccounts.First().ID)
                                      .With_WorkstationID(transportContext.Workstations.First().ID)
                                      .SaveToDb();

            var citProcessHistoryException = DataFacade.CitProcessingHistoryException.New()
                                             .With_Remark("remark")
                                             .With_Action(ExceptionAction.ConfirmCancellation)
                                             .With_Status(status)
                                             .With_DateResolved(today)
                                             .With_Exception(exceptionCaseId.ID)
                                             .With_CitProcessingHistoryID(citProcessingHistory.Build().ID)
                                             .With_ReasonCodeID(reason.ID)
                                             .With_SiteID(transportContext.Sites.First().Branch_nr)
                                             .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, TransportOrderCode = transportOrer.Build().Code };
            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today };

            var result = TransportFacade.ScheduleMonitoringService.GetExceptionsToResolve(settings, selectedRow);

            Assert.Equal(shouldbe, result.Count);
        }

        [Theory(DisplayName = "Edit Visit Form - When edit form is opened on specific transport order Then System shows exceptions for it")]
        [InlineData(ExceptionCaseName.StopCancellingDuringRoute, ProcessingHistoryExceptionStatus.InProgress, 1)]
        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ProcessingHistoryExceptionStatus.InProgress, 1)]
        [InlineData(ExceptionCaseName.NotPlannedOrderIsPerformed, ProcessingHistoryExceptionStatus.InProgress, 1)]
        [InlineData(ExceptionCaseName.StopCancellingDuringRoute, ProcessingHistoryExceptionStatus.Registered, 0)]
        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ProcessingHistoryExceptionStatus.Registered, 0)]
        [InlineData(ExceptionCaseName.NotPlannedOrderIsPerformed, ProcessingHistoryExceptionStatus.Registered, 0)]
        [InlineData(ExceptionCaseName.StopCancellingDuringRoute, ProcessingHistoryExceptionStatus.Resolved, 0)]
        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ProcessingHistoryExceptionStatus.Resolved, 0)]
        [InlineData(ExceptionCaseName.NotPlannedOrderIsPerformed, ProcessingHistoryExceptionStatus.Resolved, 0)]       
        public void VerifyThatSystemGetsExceptionForSpecificOrder(ExceptionCaseName name, ProcessingHistoryExceptionStatus status, int shouldbe)
        {
            var exceptionCaseId = transportContext.ExceptionCases.Where(e => e.Name == name).First();
            var reason = transportContext.ReasonCodes.First();

            var transportOrer = DataFacade.TransportOrder.New()
                                .With_Location(location.ID)
                                .With_Site(location.ServicingDepotID)
                                .With_OrderType(OrderType.AtRequest)
                                .With_TransportDate(today)
                                .With_MasterRouteCode(masterRoute)
                                .With_ServiceDate(today)
                                .With_Status(TransportOrderStatus.Planned)
                                .With_ServiceOrder(serviceOrder.ID)
                                .With_ServiceType(serviceTypeDelv.ID)
                                .SaveToDb();

            var citProcessingHistory = DataFacade.CitProcessingHistory.New()
                                      .With_ProcessName(ProcessName.PerformDailyRoute)
                                      .With_ProcessPhase(ProcessPhase.End)
                                      .With_Status(1)
                                      .With_IsWithException(true)
                                      .With_DateCreated(today)
                                      .With_ObjectID(transportOrer.Build().ID)
                                      .With_ObjectClassID(MetadataHelper.GetClassID(typeof(TransportOrder)))
                                      .With_AuthorID(baseDataContext.UserAccounts.First().ID)
                                      .With_WorkstationID(transportContext.Workstations.First().ID)
                                      .SaveToDb();

            var citProcessHistoryException = DataFacade.CitProcessingHistoryException.New()
                                             .With_Remark("remark")
                                             .With_Action(ExceptionAction.ConfirmCancellation)
                                             .With_Status(status)
                                             .With_DateResolved(today)
                                             .With_Exception(exceptionCaseId.ID)
                                             .With_CitProcessingHistoryID(citProcessingHistory.Build().ID)
                                             .With_ReasonCodeID(reason.ID)
                                             .With_SiteID(transportContext.Sites.First().Branch_nr)
                                             .SaveToDb();

            var result = TransportFacade.ScheduleMonitoringService.GetExceptionsToResolve(transportOrer.Build().ID);

            Assert.Equal(shouldbe, result.Count);
        }

        [Fact(DisplayName = "Edit Visit Form - When multiple exceptions for multiple orders from group exists Then System shows all of them")]
        public void VerifyWhenMultipleExceptionFromMultipleOrdersExistsThenSystemShowsAllOfThem()
        {
            var serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();
            var exceptionCaseFirst = transportContext.ExceptionCases.Where(e => e.Name == ExceptionCaseName.StopCancellingDuringRoute).First();
            var exceptionCaseSecond = transportContext.ExceptionCases.Where(e => e.Name == ExceptionCaseName.NotExecutedServiceDuringVisit).First();
            var reason = transportContext.ReasonCodes.First();

            #region TransportOrders
            var transportOrerDelv = DataFacade.TransportOrder.New()
                                    .With_Location(location.ID)
                                    .With_Site(location.ServicingDepotID)
                                    .With_OrderType(OrderType.AtRequest)
                                    .With_TransportDate(today)
                                    .With_MasterRouteCode(masterRoute)
                                    .With_ServiceDate(today)
                                    .With_Status(TransportOrderStatus.Planned)
                                    .With_ServiceOrder(serviceOrder.ID)
                                    .With_ServiceType(serviceTypeDelv.ID)
                                    .SaveToDb();

            var transportOrerColl = DataFacade.TransportOrder.New()
                                .With_Location(location.ID)
                                .With_Site(location.ServicingDepotID)
                                .With_OrderType(OrderType.AtRequest)
                                .With_TransportDate(today)
                                .With_MasterRouteCode(masterRoute)
                                .With_ServiceDate(today)
                                .With_Status(TransportOrderStatus.Planned)
                                .With_ServiceOrder(serviceOrder.ID)
                                .With_ServiceType(serviceTypeColl.ID)
                                .SaveToDb();
            #endregion

            #region CitProcessingHistory
            var citProcessingHistoryDelv = DataFacade.CitProcessingHistory.New()
                                         .With_ProcessName(ProcessName.PerformDailyRoute)
                                         .With_ProcessPhase(ProcessPhase.End)
                                         .With_Status(1)
                                         .With_IsWithException(true)
                                         .With_DateCreated(today)
                                         .With_ObjectID(transportOrerDelv.Build().ID)
                                         .With_ObjectClassID(MetadataHelper.GetClassID(typeof(TransportOrder)))
                                         .With_AuthorID(baseDataContext.UserAccounts.First().ID)
                                         .With_WorkstationID(transportContext.Workstations.First().ID)
                                         .SaveToDb();

            var citProcessingHistoryColl = DataFacade.CitProcessingHistory.New()
                                      .With_ProcessName(ProcessName.PerformDailyRoute)
                                      .With_ProcessPhase(ProcessPhase.End)
                                      .With_Status(1)
                                      .With_IsWithException(true)
                                      .With_DateCreated(today)
                                      .With_ObjectID(transportOrerColl.Build().ID)
                                      .With_ObjectClassID(MetadataHelper.GetClassID(typeof(TransportOrder)))
                                      .With_AuthorID(baseDataContext.UserAccounts.First().ID)
                                      .With_WorkstationID(transportContext.Workstations.First().ID)
                                      .SaveToDb();
            #endregion

            #region CitProcessingHistoryException
            var citProcessHistoryExceptionDelv = DataFacade.CitProcessingHistoryException.New()
                                                 .With_Remark("remark")
                                                 .With_Action(ExceptionAction.ConfirmCancellation)
                                                 .With_Status(ProcessingHistoryExceptionStatus.InProgress)
                                                 .With_DateResolved(today)
                                                 .With_Exception(exceptionCaseFirst.ID)
                                                 .With_CitProcessingHistoryID(citProcessingHistoryDelv.Build().ID)
                                                 .With_ReasonCodeID(reason.ID)
                                                 .With_SiteID(transportContext.Sites.First().Branch_nr)
                                                 .SaveToDb();

            var citProcessHistoryExceptionColl = DataFacade.CitProcessingHistoryException.New()
                                             .With_Remark("remark")
                                             .With_Action(ExceptionAction.ConfirmCancellation)
                                             .With_Status(ProcessingHistoryExceptionStatus.InProgress)
                                             .With_DateResolved(today)
                                             .With_Exception(exceptionCaseSecond.ID)
                                             .With_CitProcessingHistoryID(citProcessingHistoryColl.Build().ID)
                                             .With_ReasonCodeID(reason.ID)
                                             .With_SiteID(transportContext.Sites.First().Branch_nr)
                                             .SaveToDb(); 
            #endregion

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo};
            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today };

            var result = TransportFacade.ScheduleMonitoringService.GetExceptionsToResolve(settings, selectedRow);

            Assert.Equal(2, result.Count);
        }

        [Fact(DisplayName = "Edit Visit Form - When no exceptions are found for transport order Then System shows error message on attempt to open form")]
        public void VerifyThatSystemShowsErrorMessageWhenNoExceptionAreFoundForTransportOrder()
        {
            var action = new ResolveExceptionAction();

            var result = action.CheckActionExecutable(null, null, new Cwc.Common.UI.FormParameters { ObjectID = int.MaxValue });

            Assert.False(result.IsSuccess);
            Assert.Equal("There is no exceptions to resolve for selected transport order.", result.Messages.First());
        }

        [Fact(DisplayName = "Edit Visit Form - When Daily Stop is not defined on Exception form loading Then System returns null")]
        public void VerifyThatSystemReturnsNullOnExceptionFormLoadingIdDailyStopIsNotDefined()
        {
            var transportOrer = DataFacade.TransportOrder.New()
                    .With_Location(location.ID)
                    .With_Site(location.ServicingDepotID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(today)
                    .With_ServiceDate(today)
                    .With_Status(TransportOrderStatus.Planned)
                    .With_MasterRouteCode(masterRoute)
                    .With_ServiceOrder(serviceOrder.ID)
                    .With_ServiceType(serviceTypeDelv.ID)
                    .SaveToDb().Build();

            var citProcessingHistory = DataFacade.CitProcessingHistory.New()
                    .With_ProcessName(ProcessName.PerformDailyRoute)
                    .With_ProcessPhase(ProcessPhase.End)
                    .With_Status(1)
                    .With_IsWithException(true)
                    .With_DateCreated(today)
                    .With_ObjectID(transportOrer.ID)
                    .With_ObjectClassID(transportOrderClassId)
                    .With_AuthorID(baseDataContext.UserAccounts.First().ID)
                    .With_WorkstationID(transportContext.Workstations.First().ID)
                    .SaveToDb().Build();

            var result = TransportFacade.ScheduleMonitoringService.GetExceptionStopCategoryItem(citProcessingHistory);

            Assert.Null(result);
        }

        [Fact(DisplayName = "Edit Visit Form - When Daily Stop exists for Transport Order with linked exceptions Then System defined ExceptionStopCategories On Form Loading")]
        public void VerifyThatSystenDefinedExceptionStopCategoryProperly()
        {
            var arrival = new TimeSpan(10, 0, 0);
            var reason = transportContext.ReasonCodes.First();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(location.ID)
                .With_Site(location.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(today)
                .With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned)
                .With_MasterRouteCode(masterRoute)
                .With_MasterRouteDate(today)
                .With_StopArrivalTime(arrival)
                .With_ServiceOrder(serviceOrder.ID)
                .With_ServiceType(serviceTypeDelv.ID)
                .SaveToDb().Build();

            var citProcessingHistory = DataFacade.CitProcessingHistory.New()
                .With_ProcessName(ProcessName.PerformDailyRoute)
                .With_ProcessPhase(ProcessPhase.End)
                .With_Status(1)
                .With_IsWithException(true)
                .With_DateCreated(today)
                .With_ObjectID(transportOrer.ID)
                .With_ObjectClassID(transportOrderClassId)
                .With_AuthorID(baseDataContext.UserAccounts.First().ID)
                .With_WorkstationID(transportContext.Workstations.First().ID)
                .SaveToDb().Build();

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(masterRoute)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var result = TransportFacade.ScheduleMonitoringService.GetExceptionStopCategoryItem(citProcessingHistory);

            Assert.Equal(masterRoute, result.DailyRouteCode);
            Assert.Equal(today, result.DailyRouteDate);
            Assert.Equal(location.ID, result.LocationID);
            Assert.Equal(site.ID, result.StartSiteID);
            Assert.Equal(reason.ID, result.ReasonCodeID);
            Assert.Equal(reason.IsCustomerResponsible.Value? "Customer" : "CIT", result.ResponsibleDisplayCaption);

        }

        [Theory(DisplayName = "Edit Visit Form - When transport order status is ToClarify and Intransit Then System defines List of allowed actions")]
        [InlineData(TransportOrderStatus.ToClarifyCancellation, new[] { ExceptionAction.ConfirmCancellation, ExceptionAction.ConfirmCancellationAndScheduleNew, ExceptionAction.PutOnHold })]
        [InlineData(TransportOrderStatus.ToClarifyCompletion, new[] { ExceptionAction.ConfirmCompletion, ExceptionAction.ConfirmCompletionAndScheduleNew, ExceptionAction.PutOnHold })]
        [InlineData(TransportOrderStatus.ToClarifyNewAdhocOrder, new[] { ExceptionAction.ConfirmCompletion })]
        [InlineData(TransportOrderStatus.InTransit, new[] { ExceptionAction.Cancel, ExceptionAction.CancelAndScheduleNew})]
        public void VerifyThatSystemSelectsProperActionDefinedFromTransportOrderStatus(TransportOrderStatus status, ExceptionAction[] allowed )
        {
            var result = TransportFacade.ScheduleMonitoringService.GetExceptionActionComboboxItems(status, true, "Deliver", ProcessingHistoryExceptionStatus.Registered);

            Assert.Equal(result.Count, allowed.Count());
            Assert.False(result.Except(allowed.ToList()).Any());
        }

        [Fact(DisplayName = "Edit Visit Form - When dai_serv are matched for dailyStop Then System shows them in Cancellation Codes grid")]
        public void VerifyThatSystemSelectsCancellationCodesOnEditFormProperly()
        {
            var arrival = new TimeSpan(10, 0, 0);
            var reasonFirst = transportContext.ReasonCodes.First();
            var reasonSecond = transportContext.ReasonCodes.First(r => r.ID != reasonFirst.ID);
            var bagtypeFirst = transportContext.BagTypes.First();
            var bagtypeSecond = transportContext.BagTypes.First(x => x.ID != bagtypeFirst.ID);

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(masterRoute)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var daiServDeliver = DailyDataFacade.DailyService
                .With_ArrivalDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_BagType(bagtypeFirst.ID)
                .With_Date(today).With_Location(location)
                .With_Site(site)
                .With_MasterRoute(masterRoute)
                .With_ReasonCode(reasonFirst.ID)
                .With_ServType("Deliver").SaveToDb();

            var daiServCollect = DailyDataFacade.DailyService
                .With_ArrivalDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_BagType(bagtypeSecond.ID)
                .With_Date(today).With_Location(location)
                .With_Site(site)
                .With_MasterRoute(masterRoute)
                .With_ReasonCode(reasonSecond.ID)
                .With_ServType("Collect").SaveToDb();

            var daiServEmptyReasonCode = DailyDataFacade.DailyService
                .With_ArrivalDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_BagType(bagtypeFirst.ID)
                .With_Date(today).With_Location(location)
                .With_Site(site)
                .With_MasterRoute(masterRoute)
                .With_ServType("Service").SaveToDb();

            var result = TransportFacade.ScheduleMonitoringService.GetExceptionDailyRouteServiceItems(new ExceptionStopCategoryItem { DailyRouteDate = today, DailyRouteCode = masterRoute, LocationID = location.ID, StartSiteID = site.ID, DailyRouteTime = arrival.ToString("hhmm") });

            Assert.Equal(2, result.Count);
            Assert.True(result.Any(x => x.Service == "Collect" && x.PdaReasonCodeID == reasonSecond.ID && x.ContainerTypeID == bagtypeSecond.ID), "Dai_serv for Collect is not matched");
            Assert.True(result.Any(x => x.Service == "Deliver" && x.PdaReasonCodeID == reasonFirst.ID && x.ContainerTypeID == bagtypeFirst.ID), "Dai_serv for Deliver is not matched");
        }

        [Fact(DisplayName = "Edit Visit Form - When DailServ {Time} is not equals to Daily Stop - time Then System shows it on Cancellation Codes grid")]
        public void VerifyThatDaiServMatchedProperlyByTime()
        {
            var arrival = new TimeSpan(10, 0, 0);
            var reasonFirst = transportContext.ReasonCodes.First();
            var bagtypeFirst = transportContext.BagTypes.First();

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(masterRoute)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var daiServDeliver = DailyDataFacade.DailyService
                .With_ArrivalDate(today)
                .With_ArrivalTime(arrival.Add(new TimeSpan(0, 10, 0)).ToString("hhmm"))
                .With_BagType(bagtypeFirst.ID)
                .With_Date(today).With_Location(location)
                .With_Site(site)
                .With_MasterRoute(masterRoute)
                .With_ReasonCode(reasonFirst.ID)
                .With_ServType("Deliver").SaveToDb();

            var daiServCollect = DailyDataFacade.DailyService
                .With_ArrivalDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_BagType(bagtypeFirst.ID)
                .With_Date(today).With_Location(location)
                .With_Site(site)
                .With_MasterRoute(masterRoute)
                .With_ReasonCode(reasonFirst.ID)
                .With_ServType("Collect").SaveToDb();

            var result = TransportFacade.ScheduleMonitoringService.GetExceptionDailyRouteServiceItems(new ExceptionStopCategoryItem { DailyRouteDate = today, DailyRouteCode = masterRoute, LocationID = location.ID, StartSiteID = site.ID, DailyRouteTime = arrival.ToString("hhmm") });

            Assert.Equal(1, result.Count);
        }

        [Fact(DisplayName = "Edit Visit Form - When DailServ {Date} is not equals to DailyStop - date Then System shows it on Cancellation Codes grid")]
        public void VerifyThatDaiServMatchedProperlyByDate()
        {
            var arrival = new TimeSpan(10, 0, 0);
            var reasonFirst = transportContext.ReasonCodes.First();
            var bagtypeFirst = transportContext.BagTypes.First();

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(masterRoute)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var daiServDeliver = DailyDataFacade.DailyService
                .With_ArrivalDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_BagType(bagtypeFirst.ID)
                .With_Date(today.AddDays(1)).With_Location(location)
                .With_Site(site)
                .With_MasterRoute(masterRoute)
                .With_ReasonCode(reasonFirst.ID)
                .With_ServType("Deliver").SaveToDb();

            var result = TransportFacade.ScheduleMonitoringService.GetExceptionDailyRouteServiceItems(new ExceptionStopCategoryItem { DailyRouteDate = today, DailyRouteCode = masterRoute, LocationID = location.ID, StartSiteID = site.ID });

            Assert.Equal(0, result.Count);
        }

        [Fact(DisplayName = "Edit Visit Form - When DailServ {Location} is not equals to DailyStop - location Then System shows it on Cancellation Codes grid")]
        public void VerifyThatDaiServMatchedProperlyByLocation()
        {
            var arrival = new TimeSpan(10, 0, 0);
            var reasonFirst = transportContext.ReasonCodes.First();
            var bagtypeFirst = transportContext.BagTypes.First();
            var locationDiff = DataFacade.Location.Take(l => l.ID != location.ID);

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(masterRoute)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var daiServDeliver = DailyDataFacade.DailyService
                .With_ArrivalDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_BagType(bagtypeFirst.ID)
                .With_Date(today).With_Location(locationDiff)
                .With_Site(site)
                .With_MasterRoute(masterRoute)
                .With_ReasonCode(reasonFirst.ID)
                .With_ServType("Deliver").SaveToDb();

            var result = TransportFacade.ScheduleMonitoringService.GetExceptionDailyRouteServiceItems(new ExceptionStopCategoryItem { DailyRouteDate = today, DailyRouteCode = masterRoute, LocationID = location.ID, StartSiteID = site.ID });

            Assert.Equal(0, result.Count);
        }

        [Fact(DisplayName = "Edit Visit Form - When DailServ {Site} is not equals to DailyStop - site Then System shows it on Cancellation Codes grid")]
        public void VerifyThatDaiServMatchedProperlyBySite()
        {
            var arrival = new TimeSpan(10, 0, 0);
            var reasonFirst = transportContext.ReasonCodes.First();
            var bagtypeFirst = transportContext.BagTypes.First();
            var siteDiff = DataFacade.Site.Take(l => l.ID != site.ID);

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(masterRoute)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var daiServDeliver = DailyDataFacade.DailyService
                .With_ArrivalDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_BagType(bagtypeFirst.ID)
                .With_Date(today).With_Location(location)
                .With_Site(siteDiff)
                .With_MasterRoute(masterRoute)
                .With_ReasonCode(reasonFirst.ID)
                .With_ServType("Deliver").SaveToDb();

            var result = TransportFacade.ScheduleMonitoringService.GetExceptionDailyRouteServiceItems(new ExceptionStopCategoryItem { DailyRouteDate = today, DailyRouteCode = masterRoute, LocationID = location.ID, StartSiteID = site.ID });

            Assert.Equal(0, result.Count);
        }

        class Temp : ExceptionDetailsStopCategoryCtrl
        {
            public void PageLoad()
            {
                base.Page_Load(null, null);
            }
        }
        public void Dispose()
        {
            var idsTransportOrder = transportContext.TransportOrders.Where(t => t.MasterRouteCode == masterRoute || (t.LocationID == location.ID && t.ServiceOrderID == serviceOrder.ID)).Select(x => x.ID).ToArray();
            var idsCitProcHist = transportContext.CitProcessingHistories.Where(c => idsTransportOrder.Contains(c.ObjectID) || c.ObjectID == int.MaxValue).Select(x => x.ID).ToArray();

            transportContext.DailyStops.RemoveRange(transportContext.DailyStops.Where(d => d.RouteNumber == masterRoute));
            transportContext.SaveChanges();

            transportContext.DailyServices.RemoveRange(transportContext.DailyServices.Where(x=>x.DailyRouteNumber == masterRoute));
            transportContext.SaveChanges();

            transportContext.CitProcessingHistoryExceptions.RemoveRange(transportContext.CitProcessingHistoryExceptions.Where(x => idsCitProcHist.Contains(x.CitProcessingHistoryID)));
            transportContext.SaveChanges();

            transportContext.CitProcessingHistories.RemoveRange(transportContext.CitProcessingHistories.Where(c => idsCitProcHist.Contains(c.ID)));
            transportContext.SaveChanges();

            transportContext.TransportOrderProducts.RemoveRange(transportContext.TransportOrderProducts.Where(x => idsTransportOrder.Contains(x.TransportOrderID)));
            transportContext.SaveChanges();

            transportContext.TransportOrderServs.RemoveRange(transportContext.TransportOrderServs.Where(x => idsTransportOrder.Contains(x.TransportOrderID)));
            transportContext.SaveChanges();

            transportContext.TransportOrders.RemoveRange(transportContext.TransportOrders.Where(x => idsTransportOrder.Contains(x.ID)));
            transportContext.SaveChanges();
            transportContext.Dispose();
            baseDataContext.Dispose();
        }
    }
}
