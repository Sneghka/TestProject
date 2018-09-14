//using Cwc.BaseData;
//using Cwc.Common.Metadata;
//using Cwc.Contracts;
//using Cwc.Ordering;
//using Cwc.Security;
//using Cwc.Transport;
//using Cwc.Transport.Classes;
//using Cwc.Transport.Enums;
//using Cwc.Transport.Model;
//using CWC.AutoTests.ObjectBuilder;
//using CWC.AutoTests.ObjectBuilder.DailyDataBuilders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace CWC.AutoTests.Tests.Transport.ExceptionManagement
//{
//    [Collection("Transport Order Collection")]
//    public class ResolveCitProcessingHistoryExceptionTests : IDisposable
//    {
//        DataModel.ModelContext _context;
//        Location location;
//        DateTime date;
//        Cwc.Ordering.Order serviceOrder;
//        int transportOrderClassId;
//        Site site;
//        TimeSpan arrival;
//        string masterRoute, packNumber, trStatus;
//        public ResolveCitProcessingHistoryExceptionTests()
//        {
//            location = DataFacade.Location.Take(x => x.Code == "SP02").Build();
//            date = DataFacade.Order.DefineServicedate("DELV", location).Value;
//            serviceOrder = DataFacade.Order.New(date, location, "DELV").SaveToDb();
//            _context = new DataModel.ModelContext();
//            transportOrderClassId = MetadataHelper.GetClassID(typeof(TransportOrder)).Value;
//            site = DataFacade.Site.Take(x => x.ID == location.BranchID).Build();
//            arrival = new TimeSpan(8, 0, 0);
//            masterRoute = "SP20001";
//            packNumber = "PN200000";
//            trStatus = "TRK";
//        }

//        [Theory(DisplayName = "Resolve Cit Processing Exception - When Cit processing history exception is resoled Then System shows error message on attempt to resolve it again")]
//        [InlineData(ExceptionCaseName.StopCancellingDuringRoute)]
//        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit)]
//        [InlineData(ExceptionCaseName.NotPlannedOrderIsPerformed)]
//        public void VerifyThatSystemShowsErrorMessageOnAttemptToResolveAlreadyResolvedException(ExceptionCaseName caseName)
//        {
//            var exceptionCase = _context.Cwc_Transport_ExceptionCases.Where(x => x.Name == (int)caseName).First();
//            var citsetting = DataFacade.CitProcessingHistory.New()
//                .With_Status(1)
//                .With_DateCreated(date)
//                .With_ObjectID(int.MaxValue)
//                .With_ObjectClassID(transportOrderClassId)
//                .With_IsWithException(false)
//                .SaveToDb()
//                .Build();

//            var citSettingException = DataFacade.CitProcessingHistoryException.New()
//                .With_CitProcessingHistoryID(citsetting.ID)
//                .With_Status(ProcessingHistoryExceptionStatus.Resolved)
//                .With_Action(ExceptionAction.ConfirmCompletion)
//                .With_Exception(exceptionCase.ID)
//                .With_SiteID(site.ID)
//                .With_WorkstationID(48)
//                .SaveToDb();

//            var result = TransportFacade.CitProcessingHistoryExceptionService.ResolveCitProcessingHistoryException(citSettingException, null, ExceptionAction.ConfirmCompletion, null);

//            Assert.False(result.IsSuccess);
//            Assert.Equal("It is not allowed to resolve exception which is already resolved.", result.GetMessage());
//        }

//        [Theory(DisplayName = "Resolve Cit Processing Exception - When Cit processing history exception is not resoled Then System allows to resolve it")]
//        [InlineData(ExceptionCaseName.StopCancellingDuringRoute, ExceptionAction.Cancel)]
//        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ExceptionAction.ConfirmCancellation)]
//        [InlineData(ExceptionCaseName.NotPlannedOrderIsPerformed, ExceptionAction.ConfirmCompletionAndScheduleNew)]
//        public void VerifyThatSystemResolvesExceptionProperly(ExceptionCaseName caseName, ExceptionAction action)
//        {
//            var exceptionCase = _context.Cwc_Transport_ExceptionCases.Where(x => x.Name == (int)caseName).First();
//            var resaonCode = BaseDataFacade.ReasonCodeService.Load(_context.reasons.First().reason_cd.Value, new Cwc.Common.DataBaseParams());

//            var citsetting = DataFacade.CitProcessingHistory.New()
//                .With_Status(1)
//                .With_DateCreated(date)
//                .With_ObjectID(int.MaxValue)
//                .With_ObjectClassID(transportOrderClassId)
//                .With_IsWithException(false)
//                .SaveToDb()
//                .Build();

//            var citSettingException = DataFacade.CitProcessingHistoryException.New()
//                .With_CitProcessingHistoryID(citsetting.ID)
//                .With_Status(ProcessingHistoryExceptionStatus.InProgress)
//                .With_Action(action)
//                .With_Exception(exceptionCase.ID)
//                .With_SiteID(site.ID)
//                .SaveToDb()
//                .Build();

//            var result = TransportFacade.CitProcessingHistoryExceptionService.ResolveCitProcessingHistoryException(citSettingException, resaonCode, action, "sometext");

//            Assert.True(result.IsSuccess);
//            Assert.Equal(ProcessingHistoryExceptionStatus.Resolved, citSettingException.Status);
//            Assert.Equal(resaonCode.IdentityID, citSettingException.ReasonCodeID);
//            Assert.Equal("sometext", citSettingException.Remark);
//            Assert.Equal(action, citSettingException.Action);
//            Assert.Equal(DateTime.Now.Date.ToShortDateString(), citSettingException.DateResolved.Value.Date.ToShortDateString());
//        }

//        [Theory(DisplayName = "Resolve Cit Processing Exception - When Cit Processing History is resolving and ExceptionAction in Cancel Then System updates Transort Order -> Status = cancelled")]
//        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ExceptionAction.ConfirmCancellationAndScheduleNew, true)]
//        [InlineData(ExceptionCaseName.NotPlannedOrderIsPerformed, ExceptionAction.ConfirmCancellation, false)]
//        [InlineData(ExceptionCaseName.StopCancellingDuringRoute, ExceptionAction.CancelAndScheduleNew, false)]
//        [InlineData(ExceptionCaseName.StopCancellingDuringRoute, ExceptionAction.Cancel, true)]
//        public void VerifyThatCitProcessingHistoryExceptionCouldBeResolved(ExceptionCaseName name, ExceptionAction action, bool IsBillable)
//        {
//            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
//            var exceptionCaseId = _context.Cwc_Transport_ExceptionCases.Where(e => e.Name == (int)name).First();
//            var reasonCode = BaseDataFacade.ReasonCodeService.Load(_context.reasons.First().reason_cd.Value, new Cwc.Common.DataBaseParams());

//            var transportOrer = DataFacade.TransportOrder.New()
//                    .With_Location(location.ID)
//                    .With_Site(location.ServicingDepotID)
//                    .With_OrderType(OrderType.AtRequest)
//                    .With_TransportDate(date)
//                    .With_ServiceDate(date)
//                    .With_Status(TransportOrderStatus.ToClarifyCancellation)
//                    .With_MasterRouteCode(masterRoute)
//                    .With_ServiceOrder(serviceOrder.ID)
//                    .With_ServiceType(serviceType.ID)
//                    .SaveToDb().Build();

//            var hispack = DailyDataFacade.HisPack.New()
//              .With_Date(date)
//              .With_Time(arrival.ToString("hhmmss"))
//              .With_Status(trStatus)
//              .With_FrLocation(location)
//              .With_ToLocation(location)
//              .With_PackVal(1000)
//              .With_BagType(3301)
//              .With_PackNr(packNumber)
//              .With_MasterRoute(masterRoute)
//              .With_Site(site)
//              .With_OrderID(transportOrer.Code)
//              .SaveToDb();

//            var citProcessingHistory = DataFacade.CitProcessingHistory.New()
//                    .With_ProcessName(ProcessName.PerformDailyRoute)
//                    .With_ProcessPhase(ProcessPhase.End)
//                    .With_Status(1)
//                    .With_IsWithException(true)
//                    .With_DateCreated(date)
//                    .With_ObjectID(transportOrer.ID)
//                    .With_ObjectClassID(transportOrderClassId)
//                    .With_AuthorID(_context.WP_Users.First().id)
//                    .With_WorkstationID(_context.WP_BaseData_Workstations.First().id)
//                    .SaveToDb().Build();

//            var citProcessHistoryException = DataFacade.CitProcessingHistoryException.New()
//                    .With_Action(ExceptionAction.ConfirmCancellation)
//                    .With_Status(ProcessingHistoryExceptionStatus.Registered)
//                    .With_DateResolved(date)
//                    .With_Exception(exceptionCaseId.ID)
//                    .With_CitProcessingHistoryID(citProcessingHistory.ID)
//                    .With_ReasonCodeID(reasonCode.IdentityID)
//                    .With_SiteID(_context.branches.First().branch_nr)
//                    .SaveToDb().Build();

//            citProcessHistoryException.CitProcessingHistory = citProcessingHistory;

//            var exceptionItems = new List<ScheduleMonitoringExceptionItem> { new ScheduleMonitoringExceptionItem { Action = action, Exception = citProcessHistoryException, IsBillable = IsBillable, NewTransportDate = date.AddDays(1) } };

//            var result = TransportFacade.ScheduleMonitoringService.ResolveStopException(reasonCode, exceptionItems, "remarken");

//            var returnedOrder = DataFacade.TransportOrder.Take(x => x.Code == transportOrer.Code).Build();
//            Assert.Equal(TransportOrderStatus.Cancelled, returnedOrder.Status);
//            Assert.Equal(IsBillable, returnedOrder.IsBillable);
//        }

//        [Theory(DisplayName = "Resolve Cit Processing Exception - When ExceptionAction is in Confirm Then System sets Transport Order Status = Completed")]
//        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ExceptionAction.ConfirmCompletion, true)]
//        [InlineData(ExceptionCaseName.NotPlannedOrderIsPerformed, ExceptionAction.ConfirmCompletionAndScheduleNew, true)]
//        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ExceptionAction.ConfirmCompletion, false)]
//        [InlineData(ExceptionCaseName.NotPlannedOrderIsPerformed, ExceptionAction.ConfirmCompletionAndScheduleNew, false)]
//        public void VerifyThatSystemSetsCompletedStatusProperly(ExceptionCaseName name, ExceptionAction action, bool IsBillable)
//        {
//            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
//            var exceptionCaseId = _context.Cwc_Transport_ExceptionCases.Where(e => e.Name == (int)name).First();
//            var reasonCode = BaseDataFacade.ReasonCodeService.Load(_context.reasons.First().reason_cd.Value, new Cwc.Common.DataBaseParams());

//            var transportOrer = DataFacade.TransportOrder.New()
//                    .With_Location(location.ID)
//                    .With_Site(location.ServicingDepotID)
//                    .With_OrderType(OrderType.AtRequest)
//                    .With_TransportDate(date)
//                    .With_ServiceDate(date)
//                    .With_Status(TransportOrderStatus.Planned)
//                    .With_MasterRouteCode(masterRoute)
//                    .With_ServiceOrder(serviceOrder.ID)
//                    .With_ServiceType(serviceType.ID)
//                    .SaveToDb().Build();

//            var hispack = DailyDataFacade.HisPack.New()
//              .With_Date(date)
//              .With_Time(arrival.ToString("hhmmss"))
//              .With_Status(trStatus)
//              .With_FrLocation(location)
//              .With_ToLocation(location)
//              .With_PackVal(1000)
//              .With_BagType(3301)
//              .With_PackNr(packNumber)
//              .With_MasterRoute(masterRoute)
//              .With_Site(site)
//              .With_OrderID(transportOrer.Code)
//              .SaveToDb();

//            var citProcessingHistory = DataFacade.CitProcessingHistory.New()
//                    .With_ProcessName(ProcessName.PerformDailyRoute)
//                    .With_ProcessPhase(ProcessPhase.End)
//                    .With_Status(1)
//                    .With_IsWithException(true)
//                    .With_DateCreated(date)
//                    .With_ObjectID(transportOrer.ID)
//                    .With_ObjectClassID(transportOrderClassId)
//                    .With_AuthorID(_context.WP_Users.First().id)
//                    .With_WorkstationID(_context.WP_BaseData_Workstations.First().id)
//                    .SaveToDb().Build();

//            var citProcessHistoryException = DataFacade.CitProcessingHistoryException.New()
//                    .With_Action(ExceptionAction.ConfirmCancellation)
//                    .With_Status(ProcessingHistoryExceptionStatus.Registered)
//                    .With_DateResolved(date)
//                    .With_Exception(exceptionCaseId.ID)
//                    .With_CitProcessingHistoryID(citProcessingHistory.ID)
//                    .With_ReasonCodeID(reasonCode.IdentityID)
//                    .With_SiteID(_context.branches.First().branch_nr)
//                    .SaveToDb().Build();

//            citProcessHistoryException.CitProcessingHistory = citProcessingHistory;

//            var exceptionItems = new List<ScheduleMonitoringExceptionItem> { new ScheduleMonitoringExceptionItem { Action = action, Exception = citProcessHistoryException, IsBillable = IsBillable, NewTransportDate = date.AddDays(1) } };

//            var result = TransportFacade.ScheduleMonitoringService.ResolveStopException(reasonCode, exceptionItems, "remarken");

//            var returnedOrder = DataFacade.TransportOrder.Take(x => x.Code == transportOrer.Code).Build();
//            Assert.Equal(TransportOrderStatus.Completed, returnedOrder.Status);
//            Assert.Equal(IsBillable, returnedOrder.IsBillable);
//        }

//        [Theory(DisplayName = "Resolve Cit Processing Exception - When action is not in Schedule new Then New Transport Date can be empty")]
//        [InlineData(ExceptionAction.ConfirmCompletion, TransportOrderStatus.Completed)]
//        [InlineData(ExceptionAction.ConfirmCancellation, TransportOrderStatus.Cancelled)]
//        [InlineData(ExceptionAction.Cancel, TransportOrderStatus.Cancelled)]
//        public void VerifyThatNewTransportDateCanBeEmptyWhenisNotScheduleNew(ExceptionAction action, TransportOrderStatus expectedStatus )
//        {
//            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
//            var exceptionCaseId = _context.Cwc_Transport_ExceptionCases.Where(e => e.Name == (int)ExceptionCaseName.NotExecutedServiceDuringVisit).First();
//            var reasonCode = BaseDataFacade.ReasonCodeService.Load(_context.reasons.First().reason_cd.Value, new Cwc.Common.DataBaseParams());

//            var transportOrer = DataFacade.TransportOrder.New()
//                    .With_Location(location.ID)
//                    .With_Site(location.ServicingDepotID)
//                    .With_OrderType(OrderType.AtRequest)
//                    .With_TransportDate(date)
//                    .With_ServiceDate(date)
//                    .With_Status(TransportOrderStatus.Planned)
//                    .With_MasterRouteCode(masterRoute)
//                    .With_ServiceOrder(serviceOrder.ID)
//                    .With_ServiceType(serviceType.ID)
//                    .SaveToDb().Build();

//            var hispack = DailyDataFacade.HisPack.New()
//              .With_Date(date)
//              .With_Time(arrival.ToString("hhmmss"))
//              .With_Status(trStatus)
//              .With_FrLocation(location)
//              .With_ToLocation(location)
//              .With_PackVal(1000)
//              .With_BagType(3301)
//              .With_PackNr(packNumber)
//              .With_MasterRoute(masterRoute)
//              .With_Site(site)
//              .With_OrderID(transportOrer.Code)
//              .SaveToDb();

//            var citProcessingHistory = DataFacade.CitProcessingHistory.New()
//                    .With_ProcessName(ProcessName.PerformDailyRoute)
//                    .With_ProcessPhase(ProcessPhase.End)
//                    .With_Status(1)
//                    .With_IsWithException(true)
//                    .With_DateCreated(date)
//                    .With_ObjectID(transportOrer.ID)
//                    .With_ObjectClassID(transportOrderClassId)
//                    .With_AuthorID(_context.WP_Users.First().id)
//                    .With_WorkstationID(_context.WP_BaseData_Workstations.First().id)
//                    .SaveToDb().Build();

//            var citProcessHistoryException = DataFacade.CitProcessingHistoryException.New()
//                    .With_Action(ExceptionAction.ConfirmCancellation)
//                    .With_Status(ProcessingHistoryExceptionStatus.Registered)
//                    .With_DateResolved(date)
//                    .With_Exception(exceptionCaseId.ID)
//                    .With_CitProcessingHistoryID(citProcessingHistory.ID)
//                    .With_ReasonCodeID(reasonCode.IdentityID)
//                    .With_SiteID(_context.branches.First().branch_nr)
//                    .SaveToDb().Build();
//            citProcessHistoryException.CitProcessingHistory = citProcessingHistory;

//            var exceptionItems = new List<ScheduleMonitoringExceptionItem> { new ScheduleMonitoringExceptionItem { Action = action, Exception = citProcessHistoryException, IsBillable = true} };

//            var result = TransportFacade.ScheduleMonitoringService.ResolveStopException(reasonCode, exceptionItems, "remarken");

//            var returnedOrder = DataFacade.TransportOrder.Take(x => x.Code == transportOrer.Code).Build();

//            Assert.True(result.IsSuccess);
//            Assert.Equal(expectedStatus, returnedOrder.Status);

//        }

//        [Theory(DisplayName = "Resolve Cit Processing Exception - When action in Schedule New Then System creates copy of transport order")]
//        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ExceptionAction.CancelAndScheduleNew, true)]
//        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ExceptionAction.ConfirmCancellationAndScheduleNew, true)]
//        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ExceptionAction.ConfirmCompletionAndScheduleNew, false)]
//        public void VerifyThatSystemCopiesTransPOrtOrderOnResolving(ExceptionCaseName name, ExceptionAction action, bool IsBillable)
//        {
//            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
//            var exceptionCaseId = _context.Cwc_Transport_ExceptionCases.Where(e => e.Name == (int)name).First();
//            var reasonCode = BaseDataFacade.ReasonCodeService.Load(_context.reasons.First().reason_cd.Value, new Cwc.Common.DataBaseParams());

//            var servicedate = DataFacade.Order.DefineServicedate(serviceType.Code, location);

//            serviceOrder = DataFacade.Order.New(servicedate.Value, location, serviceType.Code)
//                    .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
//                    .With_ServiceTypeCode(serviceType.Code)
//                    .With_LocationCode(location.Code)
//                    .With_LocationID(location.ID)
//                    .With_GenericStatus(Cwc.Ordering.GenericStatus.Registered)
//                    .With_ServiceDate(servicedate)
//                    .With_CurrencyCode("EUR")
//                    .With_CustomerID(location.CompanyID)
//                    .SaveToDb()
//                    .Build();

//            var transportOrer = DataFacade.TransportOrder.New()
//                    .With_Location(location.ID)
//                    .With_Site(location.ServicingDepotID)
//                    .With_OrderType(OrderType.AtRequest)
//                    .With_TransportDate(date)
//                    .With_ServiceDate(date)
//                    .With_Status(TransportOrderStatus.Planned)
//                    .With_MasterRouteCode(masterRoute)
//                    .With_ServiceOrder(serviceOrder.ID)
//                    .With_ServiceType(serviceType.ID)
//                    .SaveToDb().Build();

//            var hispack = DailyDataFacade.HisPack.New()
//               .With_Date(date)
//               .With_Time(arrival.ToString("hhmmss"))
//               .With_Status(trStatus)
//               .With_FrLocation(location)
//               .With_ToLocation(location)
//               .With_PackVal(1000)
//               .With_BagType(3301)
//               .With_PackNr(packNumber)
//               .With_MasterRoute(masterRoute)
//               .With_Site(site)
//               .With_OrderID(transportOrer.Code)
//               .SaveToDb();

//            var citProcessingHistory = DataFacade.CitProcessingHistory.New()
//                    .With_ProcessName(ProcessName.PerformDailyRoute)
//                    .With_ProcessPhase(ProcessPhase.End)
//                    .With_Status(1)
//                    .With_IsWithException(true)
//                    .With_DateCreated(date)
//                    .With_ObjectID(transportOrer.ID)
//                    .With_ObjectClassID(transportOrderClassId)
//                    .With_AuthorID(_context.WP_Users.First().id)
//                    .With_WorkstationID(_context.WP_BaseData_Workstations.First().id)
//                    .SaveToDb().Build();

//            var citProcessHistoryException = DataFacade.CitProcessingHistoryException.New()
//                    .With_Action(ExceptionAction.ConfirmCancellation)
//                    .With_Status(ProcessingHistoryExceptionStatus.Registered)
//                    .With_DateResolved(date)
//                    .With_Exception(exceptionCaseId.ID)
//                    .With_CitProcessingHistoryID(citProcessingHistory.ID)
//                    .With_ReasonCodeID(reasonCode.IdentityID)
//                    .With_SiteID(_context.branches.First().branch_nr)
//                    .SaveToDb().Build();
//            citProcessHistoryException.CitProcessingHistory = citProcessingHistory;

//            var exceptionItems = new List<ScheduleMonitoringExceptionItem> { new ScheduleMonitoringExceptionItem { Action = action, Exception = citProcessHistoryException, IsBillable = IsBillable, NewTransportDate = date.AddDays(1) } };

//            var result = TransportFacade.ScheduleMonitoringService.ResolveStopException(reasonCode, exceptionItems, "remarken");

//            var returnedOrder = DataFacade.TransportOrder.Take(x => x.ServiceOrderID == serviceOrder.ID && x.Code != transportOrer.Code).Build();

//            Assert.True(result.IsSuccess, "returned result is not successfull");
//            Assert.Equal(transportOrer.OrderType, returnedOrder.OrderType);
//            Assert.Equal(transportOrer.ServiceDate.Value.ToShortTimeString(), returnedOrder.ServiceDate.Value.ToShortTimeString());
//            Assert.Equal(transportOrer.ServiceTypeID, returnedOrder.ServiceTypeID);
//            Assert.Equal(transportOrer.SiteID, returnedOrder.SiteID);
//            Assert.Equal(transportOrer.LocationID, returnedOrder.LocationID);
//            Assert.Equal(transportOrer.ServiceOrderID, returnedOrder.ServiceOrderID);
//        }

//        [Theory(DisplayName = "Resolve Cit Processing Exception - When Action is not Schedule New Then System doesn't create copy of transport order")]
//        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ExceptionAction.Cancel, true)]
//        [InlineData(ExceptionCaseName.NotPlannedOrderIsPerformed, ExceptionAction.ConfirmCancellation, true)]
//        [InlineData(ExceptionCaseName.NotExecutedServiceDuringVisit, ExceptionAction.ConfirmCompletion, false)]
//        public void VerifyThatSystenNotCreateTransportOrderCopyWhenActionNotInScheduleNew(ExceptionCaseName name, ExceptionAction action, bool IsBillable)
//        {
//            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
//            var exceptionCaseId = _context.Cwc_Transport_ExceptionCases.Where(e => e.Name == (int)name).First();
//            var reasonCode = BaseDataFacade.ReasonCodeService.Load(_context.reasons.First().reason_cd.Value, new Cwc.Common.DataBaseParams());

//            var servicedate = DataFacade.Order.DefineServicedate(serviceType.Code, location);

//            serviceOrder = DataFacade.Order.New(servicedate.Value, location, serviceType.Code)
//                    .With_OrderType(Cwc.Contracts.OrderType.AtRequest)
//                    .With_ServiceTypeCode(serviceType.Code)
//                    .With_LocationCode(location.Code)
//                    .With_LocationID(location.ID)
//                    .With_GenericStatus(Cwc.Ordering.GenericStatus.Registered)
//                    .With_ServiceDate(servicedate)
//                    .With_CurrencyCode("EUR")
//                    .With_CustomerID(location.CompanyID)
//                    .SaveToDb()
//                    .Build();

//            var transportOrer = DataFacade.TransportOrder.New()
//                    .With_Location(location.ID)
//                    .With_Site(location.ServicingDepotID)
//                    .With_OrderType(OrderType.AtRequest)
//                    .With_TransportDate(date)
//                    .With_ServiceDate(date)
//                    .With_Status(TransportOrderStatus.Planned)
//                    .With_MasterRouteCode(masterRoute)
//                    .With_ServiceOrder(serviceOrder.ID)
//                    .With_ServiceType(serviceType.ID)
//                    .SaveToDb().Build();

//            var hispack = DailyDataFacade.HisPack.New()
//              .With_Date(date)
//              .With_Time(arrival.ToString("hhmmss"))
//              .With_Status(trStatus)
//              .With_FrLocation(location)
//              .With_ToLocation(location)
//              .With_PackVal(1000)
//              .With_BagType(3301)
//              .With_PackNr(packNumber)
//              .With_MasterRoute(masterRoute)
//              .With_Site(site)
//              .With_OrderID(transportOrer.Code)
//              .SaveToDb();

//            var citProcessingHistory = DataFacade.CitProcessingHistory.New()
//                    .With_ProcessName(ProcessName.PerformDailyRoute)
//                    .With_ProcessPhase(ProcessPhase.End)
//                    .With_Status(1)
//                    .With_IsWithException(true)
//                    .With_DateCreated(date)
//                    .With_ObjectID(transportOrer.ID)
//                    .With_ObjectClassID(transportOrderClassId)
//                    .With_AuthorID(_context.WP_Users.First().id)
//                    .With_WorkstationID(_context.WP_BaseData_Workstations.First().id)
//                    .SaveToDb().Build();

//            var citProcessHistoryException = DataFacade.CitProcessingHistoryException.New()
//                    .With_Action(ExceptionAction.ConfirmCancellation)
//                    .With_Status(ProcessingHistoryExceptionStatus.Registered)
//                    .With_DateResolved(date)
//                    .With_Exception(exceptionCaseId.ID)
//                    .With_CitProcessingHistoryID(citProcessingHistory.ID)
//                    .With_ReasonCodeID(reasonCode.IdentityID)
//                    .With_SiteID(_context.branches.First().branch_nr)
//                    .SaveToDb().Build();

//            citProcessHistoryException.CitProcessingHistory = citProcessingHistory;
//            var exceptionItems = new List<ScheduleMonitoringExceptionItem> { new ScheduleMonitoringExceptionItem { Action = action, Exception = citProcessHistoryException, IsBillable = IsBillable, NewTransportDate = date.AddDays(1) } };

//            var result = TransportFacade.ScheduleMonitoringService.ResolveStopException(reasonCode, exceptionItems, "remarken");

//            Assert.True(result.IsSuccess, "Returned result is not successfull");
//            Assert.False(_context.Cwc_Transport_TransportOrders.Where(x => x.ServiceOrderID == serviceOrder.ID && x.Code != transportOrer.Code && x.ServiceTypeID == transportOrer.ServiceTypeID && x.OrderType == (int)transportOrer.OrderType).Any());
//        }

//        [Theory(DisplayName = @"Resolve Cit Processing Exception - When Action is in '.. and schedule new' Then System creates a copy of current transport order with Service Date = Exception -> New Transport Date")]
//        [InlineData(OrderType.AtRequest, TransportOrderStatus.Planned, "DELV", false, false, false)]
//        [InlineData(OrderType.AdHoc, TransportOrderStatus.Completed, "DELV", false, false, false)]
//        [InlineData(OrderType.Fixed, TransportOrderStatus.Registered, "DELV", false, false, false)]
//        //[InlineData(OrderType.AdHoc, TransportOrderStatus.Completed, "REPL")]
//        public void VerifyThatTransportOrderCopiesProperly(OrderType ordertype, TransportOrderStatus tos, string serType, bool isPerformCollection, bool isPerformDelivery, bool isPerformServicing)
//        {
//            var serviceType = DataFacade.ServiceType.Take(x => x.Code == serType).Build();

//            var newdate = date.AddDays(1);
//            var transportOrder = DataFacade.TransportOrder.New()
//                    .With_Location(location)
//                    .With_Site(location.ServicingDepotID)
//                    .With_OrderType(ordertype)
//                    .With_TransportDate(date)
//                    .With_ServiceDate(date)
//                    .With_Status(tos)
//                    .With_ServiceOrder(serviceOrder)
//                    .With_MasterRouteCode(masterRoute)
//                    .With_ServiceType(serviceType).SaveToDb()
//                    .Build();

//            var hispack = DailyDataFacade.HisPack.New()
//              .With_Date(date)
//              .With_Time(arrival.ToString("hhmmss"))
//              .With_Status(trStatus)
//              .With_FrLocation(location)
//              .With_ToLocation(location)
//              .With_PackVal(1000)
//              .With_BagType(3301)
//              .With_PackNr(packNumber)
//              .With_MasterRoute(masterRoute)
//              .With_Site(site)
//              .With_OrderID(transportOrder.Code)
//              .SaveToDb();

//            var result = TransportFacade.TransportOrderService.CopyTransportOrder(transportOrder, newdate, 
//                                                                                    isCopyProduct: false, 
//                                                                                    isCopyServicingCode: false, 
//                                                                                    isPerformCollect: isPerformCollection, 
//                                                                                    isPerformDelivery: isPerformDelivery, 
//                                                                                    isPerformServicing: isPerformServicing);

//            Assert.True(result.IsSuccess);
//            Assert.Equal(transportOrder.OrderType, result.Value.OrderType);
//            Assert.Equal(newdate, result.Value.TransportDate);
//            Assert.Equal(transportOrder.ServiceDate, result.Value.ServiceDate);
//            Assert.Equal(transportOrder.ServiceTypeID, result.Value.ServiceTypeID);
//            Assert.Equal(transportOrder.SiteID, result.Value.SiteID);
//            Assert.Equal(transportOrder.LocationID, result.Value.LocationID);
//            Assert.Equal(transportOrder.ServiceOrderID, result.Value.ServiceOrderID);
//            Assert.True(_context.Cwc_Transport_TransportOrders.Where(tr => tr.Code == result.Value.Code && tr.TransportDate == newdate).Any());
//        }

//        [Theory(DisplayName = @"Resolve Cit Processing Exception - When IsCopyProducts = true / IsCopyServices Then System copies transport order product")]
//        [InlineData(true, false)]
//        [InlineData(false, true)]
//        [InlineData(true, true)]
//        [InlineData(false, true)]
//        public void VerifyThatSystemCopiesTransportOrderProductProperly(bool isCopyProducts, bool isCopyServices)
//        {
//            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
//            var servCodeFirst = _context.ServicingCodes.First();
//            var author = SecurityFacade.LoginService.GetAdministratorLogin();
//            Cwc.BaseData.Product product = DataFacade.Product.Take(x => x.IsBarcodedProduct == false);
//            var quantity = 1;

//            var newdate = date.AddDays(1);
//            var transportOrder = DataFacade.TransportOrder.New()
//                                                        .With_Location(location)
//                                                        .With_Site(location.ServicingDepotID)
//                                                        .With_OrderType(OrderType.AtRequest)
//                                                        .With_TransportDate(date)
//                                                        .With_ServiceDate(date)
//                                                        .With_Status(TransportOrderStatus.Planned)
//                                                        .With_ServiceOrder(serviceOrder)
//                                                        .With_MasterRouteCode(masterRoute)
//                                                        .With_ServiceType(serviceType).SaveToDb()
//                                                        .Build();

//            var hispack = DailyDataFacade.HisPack.New()
//              .With_Date(date)
//              .With_Time(arrival.ToString("hhmmss"))
//              .With_Status(trStatus)
//              .With_FrLocation(location)
//              .With_ToLocation(location)
//              .With_PackVal(1000)
//              .With_BagType(3301)
//              .With_PackNr(packNumber)
//              .With_MasterRoute(masterRoute)
//              .With_Site(site)
//              .With_OrderID(transportOrder.Code)
//              .SaveToDb();

//            var transportOrderProduct = DataFacade.TransportOrderProduct.New()
//                .With_TransportOrder(transportOrder)
//                .With_Product(product)
//                .With_OrderedQuantity(quantity)
//                .With_OrderedValue(quantity * (int)product.Value)
//                .With_CurrencyID("EUR")
//                .With_DateCreated(date)
//                .With_DateUpdated(date)
//                .SaveToDb();

//            var transportOrderServiceFirst = DataFacade.TransportOrderServ.New()
//                .With_IsPerformed(false)
//                .With_IsPlanned(true)
//                .With_Service(servCodeFirst.servCode)
//                .With_TransportOrderID(transportOrder.ID)
//                .With_AuthorID(author.UserID)
//                .With_EditorID(author.UserID)
//                .SaveToDb();

//            var result = TransportFacade.TransportOrderService.CopyTransportOrder(transportOrder, newdate, isCopyProducts, isCopyServices, 
//                                                                                    isPerformCollect: false, 
//                                                                                    isPerformDelivery: false, 
//                                                                                    isPerformServicing: false);

//            Assert.True(result.IsSuccess);
//            Assert.Equal(isCopyProducts, _context.Cwc_Transport_TransportOrderProducts.Where(x => x.TransportOrderID == result.Value.ID && x.ProductID == product.ID).Any());
//            Assert.Equal(isCopyServices, _context.Cwc_Transport_TransportOrderServs.Where(x => x.TransportOrderID == result.Value.ID && x.ServiceID == servCodeFirst.servCode).Any());
//        }

//        [Fact(DisplayName = "Resolve Cit Processing Exception - When IsCopyProduct = true and exists several prdocts for Transport Order Then System Copies Them all")]
//        public void VerifyThatSeveralProductsAreMovedOnCopy()
//        {
//            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
//            Cwc.BaseData.Product product1 = DataFacade.Product.Take(x => x.IsBarcodedProduct == false);
//            var quantity1 = 1;
//            Cwc.BaseData.Product product2 = DataFacade.Product.Take(x => x.IsBarcodedProduct == false && x.ProductCode != product1.ProductCode);
//            var quantity2 = 2;

//            var newdate = date.AddDays(1);
//            var transportOrer = DataFacade.TransportOrder.New()
//                                                        .With_Location(location)
//                                                        .With_Site(location.ServicingDepotID)
//                                                        .With_OrderType(OrderType.AtRequest)
//                                                        .With_TransportDate(date)
//                                                        .With_ServiceDate(date)
//                                                        .With_Status(TransportOrderStatus.Planned)
//                                                        .With_ServiceOrder(serviceOrder)
//                                                        .With_MasterRouteCode(masterRoute)
//                                                        .With_ServiceType(serviceType).SaveToDb()
//                                                        .Build();

//            var hispack = DailyDataFacade.HisPack.New()
//              .With_Date(date)
//              .With_Time(arrival.ToString("hhmmss"))
//              .With_Status(trStatus)
//              .With_FrLocation(location)
//              .With_ToLocation(location)
//              .With_PackVal(1000)
//              .With_BagType(3301)
//              .With_PackNr(packNumber)
//              .With_MasterRoute(masterRoute)
//              .With_Site(site)
//              .With_OrderID(transportOrer.Code)
//              .SaveToDb();

//            var transportOrderProductFirst = DataFacade.TransportOrderProduct.New()
//                .With_TransportOrder(transportOrer)
//                .With_Product(product1)
//                .With_OrderedQuantity(quantity1)
//                .With_OrderedValue(quantity1 * (int)product1.Value)
//                .With_CurrencyID("EUR")
//                .With_DateCreated(date)
//                .With_DateUpdated(date)
//                .SaveToDb();

//            var transportOrderProductSecond = DataFacade.TransportOrderProduct.New()
//                .With_TransportOrder(transportOrer)
//                .With_Product(product2)
//                .With_OrderedQuantity(quantity2)
//                .With_OrderedValue(quantity2 * (int)product1.Value)
//                .With_CurrencyID("EUR")
//                .With_DateCreated(date)
//                .With_DateUpdated(date)
//                .SaveToDb();

//            var result = TransportFacade.TransportOrderService.CopyTransportOrder(transportOrer, newdate, isCopyProduct: true, isCopyServicingCode: false,
//                                                                                                                                isPerformCollect: false,
//                                                                                                                                isPerformDelivery: false,
//                                                                                                                                isPerformServicing: false);

//            Assert.True(result.IsSuccess);
//            Assert.Equal(2, _context.Cwc_Transport_TransportOrderProducts.Where(x => x.TransportOrderID == result.Value.ID).Count());
//        }

//        [Fact(DisplayName = "Resolve Cit Processing Exception - When IsCopyService = true and exists several prdocts for Transport Order Then System Copies Them all")]
//        public void VerifyThatSeveralServicesAreMovedOnCopy()
//        {
//            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
//            var servCodeFirst = _context.ServicingCodes.First();
//            var servCodeSecond = _context.ServicingCodes.Where(x => x.servCode != servCodeFirst.servCode).First();
//            var author = SecurityFacade.LoginService.GetAdministratorLogin();

//            var newdate = date.AddDays(1);
//            var transportOrer = DataFacade.TransportOrder.New()
//                                                        .With_Location(location)
//                                                        .With_Site(location.ServicingDepotID)
//                                                        .With_OrderType(OrderType.AtRequest)
//                                                        .With_TransportDate(date)
//                                                        .With_ServiceDate(date)
//                                                        .With_Status(TransportOrderStatus.Planned)
//                                                        .With_ServiceOrder(serviceOrder)
//                                                        .With_MasterRouteCode(masterRoute)
//                                                        .With_ServiceType(serviceType).SaveToDb()
//                                                        .Build();

//            var hispack = DailyDataFacade.HisPack.New()
//              .With_Date(date)
//              .With_Time(arrival.ToString("hhmmss"))
//              .With_Status(trStatus)
//              .With_FrLocation(location)
//              .With_ToLocation(location)
//              .With_PackVal(1000)
//              .With_BagType(3301)
//              .With_PackNr(packNumber)
//              .With_MasterRoute(masterRoute)
//              .With_Site(site)
//              .With_OrderID(transportOrer.Code)
//              .SaveToDb();

//            var transportOrderServiceFirst = DataFacade.TransportOrderServ.New()
//                .With_IsPerformed(false)
//                .With_IsPlanned(true)
//                .With_Service(servCodeFirst.servCode)
//                .With_TransportOrderID(transportOrer.ID)
//                .With_AuthorID(author.UserID)
//                .With_EditorID(author.UserID)
//                .SaveToDb();

//            var transportOrderServiceSecond = DataFacade.TransportOrderServ.New()
//                .With_IsPerformed(false)
//                .With_IsPlanned(true)
//                .With_Service(servCodeSecond.servCode)
//                .With_TransportOrderID(transportOrer.ID)
//                .With_AuthorID(author.UserID)
//                .With_EditorID(author.UserID)
//                .SaveToDb();

//            var result = TransportFacade.TransportOrderService.CopyTransportOrder(transportOrer, newdate, isCopyProduct: false, isCopyServicingCode: true,
//                                                                                                                                isPerformCollect: false,
//                                                                                                                                isPerformDelivery: true,
//                                                                                                                                isPerformServicing: false);

//            Assert.True(result.IsSuccess);
//            Assert.Equal(2, _context.Cwc_Transport_TransportOrderServs.Where(x => x.TransportOrderID == result.Value.ID).Count());
//        }

//        [Fact(DisplayName = "Resolve Cit Processing Exception - When multiple transport orders sets for resoling Then System resolves Them properly")]
//        public void VerifyThatSystemResolvesMultipleTransportOrdersProperly()
//        {
//            var serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
//            var serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();
//            var exceptionCaseId = _context.Cwc_Transport_ExceptionCases.Where(e => e.Name == (int)ExceptionCaseName.NotExecutedServiceDuringVisit).First();
//            var reasonCode = BaseDataFacade.ReasonCodeService.Load(_context.reasons.Where(r => r.reason_cd!=null).First().reason_cd.Value, new Cwc.Common.DataBaseParams());
//            date = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(location, serviceTypeColl);
//            var servicedate = DataFacade.Order.DefineServicedate(serviceTypeDelv.Code, location);

//            var transportOrerFirst = DataFacade.TransportOrder.New()
//                    .With_Location(location.ID)
//                    .With_Site(location.ServicingDepotID)
//                    .With_OrderType(OrderType.AtRequest)
//                    .With_TransportDate(date)
//                    .With_ServiceDate(date)
//                    .With_Status(TransportOrderStatus.InTransit)
//                    .With_MasterRouteCode(masterRoute)
//                    .With_ServiceOrder(serviceOrder.ID)
//                    .With_ServiceType(serviceTypeDelv.ID)
//                    .SaveToDb().Build();

//            var hispack = DailyDataFacade.HisPack.New()
//              .With_Date(date)
//              .With_Time(arrival.ToString("hhmmss"))
//              .With_Status(trStatus)
//              .With_FrLocation(location)
//              .With_ToLocation(location)
//              .With_PackVal(1000)
//              .With_BagType(3301)
//              .With_PackNr(packNumber)
//              .With_MasterRoute(masterRoute)
//              .With_Site(site)
//              .With_OrderID(transportOrerFirst.Code)
//              .SaveToDb();

//            var transportOrerSecond = DataFacade.TransportOrder.New()
//                    .With_Location(location.ID)
//                    .With_Site(location.ServicingDepotID)
//                    .With_OrderType(OrderType.AtRequest)
//                    .With_TransportDate(date)
//                    .With_ServiceDate(date)
//                    .With_Status(TransportOrderStatus.ToClarifyCompletion)
//                    .With_MasterRouteCode(masterRoute)
//                    .With_ServiceOrder(serviceOrder.ID)
//                    .With_ServiceType(serviceTypeColl.ID)
//                    .SaveToDb().Build();

//            var hispack2 = DailyDataFacade.HisPack.New()
//              .With_Date(date)
//              .With_Time(arrival.ToString("hhmmss"))
//              .With_Status(trStatus)
//              .With_FrLocation(location)
//              .With_ToLocation(location)
//              .With_PackVal(1000)
//              .With_BagType(3301)
//              .With_PackNr(packNumber)
//              .With_MasterRoute(masterRoute)
//              .With_Site(site)
//              .With_OrderID(transportOrerSecond.Code)
//              .SaveToDb();

//            var citProcessingHistoryFirst = DataFacade.CitProcessingHistory.New()
//                    .With_ProcessName(ProcessName.PerformDailyRoute)
//                    .With_ProcessPhase(ProcessPhase.End)
//                    .With_Status(1)
//                    .With_IsWithException(true)
//                    .With_DateCreated(date)
//                    .With_ObjectID(transportOrerFirst.ID)
//                    .With_ObjectClassID(transportOrderClassId)
//                    .With_AuthorID(_context.WP_Users.First().id)
//                    .With_WorkstationID(_context.WP_BaseData_Workstations.First().id)
//                    .SaveToDb().Build();

//            var citProcessingHistorySecond = DataFacade.CitProcessingHistory.New()
//                    .With_ProcessName(ProcessName.PerformDailyRoute)
//                    .With_ProcessPhase(ProcessPhase.End)
//                    .With_Status(1)
//                    .With_IsWithException(true)
//                    .With_DateCreated(date)
//                    .With_ObjectID(transportOrerSecond.ID)
//                    .With_ObjectClassID(transportOrderClassId)
//                    .With_AuthorID(_context.WP_Users.First().id)
//                    .With_WorkstationID(_context.WP_BaseData_Workstations.First().id)
//                    .SaveToDb().Build();

//            var citProcessHistoryExceptionFirst = DataFacade.CitProcessingHistoryException.New()
//                    .With_Action(ExceptionAction.ConfirmCancellation)
//                    .With_Status(ProcessingHistoryExceptionStatus.Registered)
//                    .With_DateResolved(date)
//                    .With_Exception(exceptionCaseId.ID)
//                    .With_CitProcessingHistoryID(citProcessingHistoryFirst.ID)
//                    .With_ReasonCodeID(reasonCode.IdentityID)
//                    .With_SiteID(_context.branches.First().branch_nr)
//                    .SaveToDb().Build();

//            var citProcessHistoryExceptionSecond = DataFacade.CitProcessingHistoryException.New()
//                    .With_Action(ExceptionAction.ConfirmCancellation)
//                    .With_Status(ProcessingHistoryExceptionStatus.Registered)
//                    .With_DateResolved(date)
//                    .With_Exception(exceptionCaseId.ID)
//                    .With_CitProcessingHistoryID(citProcessingHistorySecond.ID)
//                    .With_ReasonCodeID(reasonCode.IdentityID)
//                    .With_SiteID(_context.branches.First().branch_nr)
//                    .SaveToDb().Build();
//            citProcessHistoryExceptionFirst.CitProcessingHistory = citProcessingHistoryFirst;
//            citProcessHistoryExceptionSecond.CitProcessingHistory = citProcessingHistorySecond;

//            var exceptionItems = new List<ScheduleMonitoringExceptionItem>
//            {
//                new ScheduleMonitoringExceptionItem { Action = ExceptionAction.CancelAndScheduleNew, Exception = citProcessHistoryExceptionFirst, IsBillable = true, NewTransportDate = date.AddDays(30) },
//                new ScheduleMonitoringExceptionItem { Action = ExceptionAction.ConfirmCompletionAndScheduleNew, Exception = citProcessHistoryExceptionSecond, IsBillable = false, NewTransportDate = date.AddDays(30) }
//            };

//            var result = TransportFacade.ScheduleMonitoringService.ResolveStopException(reasonCode, exceptionItems, "remarken");

//            Assert.True(result.IsSuccess, "Returned result is not success");

//            var resurnedOrderFirst = DataFacade.TransportOrder.Take(x => x.Code == transportOrerFirst.Code).Build();
//            var resurnedOrderSecond = DataFacade.TransportOrder.Take(x => x.Code == transportOrerSecond.Code).Build();

//            var copiedTransportOrderFirst = DataFacade.TransportOrder.Take(x => x.ServiceOrderID == serviceOrder.ID && x.ServiceTypeID == transportOrerFirst.ServiceTypeID && x.Code != transportOrerFirst.Code);
//            var copiedTransportOrderSecond = DataFacade.TransportOrder.Take(x => x.ServiceOrderID == serviceOrder.ID && x.ServiceTypeID == transportOrerSecond.ServiceTypeID && x.Code != transportOrerSecond.Code);

//            Assert.Equal(TransportOrderStatus.Cancelled, resurnedOrderFirst.Status);
//            Assert.Equal(TransportOrderStatus.Completed, resurnedOrderSecond.Status);
//            Assert.Equal(true, resurnedOrderFirst.IsBillable);
//            Assert.Equal(false, resurnedOrderSecond.IsBillable);
//        }

//        [Theory(DisplayName = "Resolve Cit Processing Exception - When Transport Order with current date, service type, location aleady exists Then System shows error message and doesn't allow to create a copy")]
//        [InlineData(ExceptionAction.CancelAndScheduleNew)]
//        [InlineData(ExceptionAction.ConfirmCancellationAndScheduleNew)]
//        [InlineData(ExceptionAction.ConfirmCompletionAndScheduleNew)]
//        public void VerifyThatSystemDoesntAllowToCopyTransportOrderIfAnotherTransportOrderAlreadyExists(ExceptionAction action)
//        {
//            var serviceType = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();
//            var exceptionCaseId = _context.Cwc_Transport_ExceptionCases.Where(e => e.Name == (int)ExceptionCaseName.StopCancellingDuringRoute).First();
//            var reasonCode = BaseDataFacade.ReasonCodeService.Load(_context.reasons.First().reason_cd.Value, new Cwc.Common.DataBaseParams());
//            date = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(location, serviceType);
//            var newDate = date.AddDays(3);
//            var servicedate = DataFacade.Order.DefineServicedate(serviceType.Code, location);

//            var transportOrer = DataFacade.TransportOrder.New()
//                    .With_Location(location.ID)
//                    .With_Site(location.ServicingDepotID)
//                    .With_OrderType(OrderType.AtRequest)
//                    .With_TransportDate(date.Date)
//                    .With_ServiceDate(date.Date)
//                    .With_Status(TransportOrderStatus.Planned)
//                    .With_MasterRouteCode(masterRoute)
//                    .With_ServiceOrder(serviceOrder.ID)
//                    .With_ServiceType(serviceType.ID)
//                    .SaveToDb().Build();

//            var hispack = DailyDataFacade.HisPack.New()
//              .With_Date(date)
//              .With_Time(arrival.ToString("hhmmss"))
//              .With_Status(trStatus)
//              .With_FrLocation(location)
//              .With_ToLocation(location)
//              .With_PackVal(1000)
//              .With_BagType(3301)
//              .With_PackNr(packNumber)
//              .With_MasterRoute(masterRoute)
//              .With_Site(site)
//              .With_OrderID(transportOrer.Code)
//              .SaveToDb();

//            var transportOrerExisted = DataFacade.TransportOrder.New()
//                    .With_Location(location.ID)
//                    .With_Site(location.ServicingDepotID)
//                    .With_OrderType(OrderType.AtRequest)
//                    .With_TransportDate(newDate.Date)
//                    .With_ServiceDate(newDate.Date)
//                    .With_Status(TransportOrderStatus.Planned)
//                    .With_MasterRouteCode(masterRoute)
//                    .With_ServiceOrder(serviceOrder.ID)
//                    .With_ServiceType(serviceType.ID)
//                    .SaveToDb().Build();

//            var citProcessingHistory = DataFacade.CitProcessingHistory.New()
//                    .With_ProcessName(ProcessName.PerformDailyRoute)
//                    .With_ProcessPhase(ProcessPhase.End)
//                    .With_Status(1)
//                    .With_IsWithException(true)
//                    .With_DateCreated(date)
//                    .With_ObjectID(transportOrer.ID)
//                    .With_ObjectClassID(transportOrderClassId)
//                    .With_AuthorID(_context.WP_Users.First().id)
//                    .With_WorkstationID(_context.WP_BaseData_Workstations.First().id)
//                    .SaveToDb().Build();

//            var citProcessHistoryException = DataFacade.CitProcessingHistoryException.New()
//                    .With_Action(ExceptionAction.ConfirmCancellation)
//                    .With_Status(ProcessingHistoryExceptionStatus.Registered)
//                    .With_DateResolved(date)
//                    .With_Exception(exceptionCaseId.ID)
//                    .With_CitProcessingHistoryID(citProcessingHistory.ID)
//                    .With_ReasonCodeID(reasonCode.IdentityID)
//                    .With_SiteID(_context.branches.First().branch_nr)
//                    .SaveToDb().Build();
//            citProcessHistoryException.CitProcessingHistory = citProcessingHistory;

//            var exceptionItems = new List<ScheduleMonitoringExceptionItem> { new ScheduleMonitoringExceptionItem { Action = action, Exception = citProcessHistoryException, IsBillable = false, NewTransportDate = newDate.Date } };

//            var result = TransportFacade.ScheduleMonitoringService.ResolveStopException(reasonCode, exceptionItems, "remarken");

//            var expectedMsg = $"New transport order for location {location.DisplayCaption} cannot be created - the order with order type At-request and service type {serviceType.DisplayCaption} is already scheduled on {newDate.ToString("M/dd/yyyy")}.";

//            Assert.False(result.IsSuccess);
//            Assert.Equal(expectedMsg, result.GetMessage());
//        }

//        public void Dispose()
//        {
//            var idsTransportOrder = _context.Cwc_Transport_TransportOrders.Where(t => t.MasterRouteCode == masterRoute || (t.LocationID == location.ID && t.ServiceOrderID == serviceOrder.ID)).Select(x => x.id).ToArray();
//            var idsCitProcHist = _context.Cwc_Transport_CitProcessingHistories.Where(c => idsTransportOrder.Contains(c.ObjectID) || c.ObjectID == int.MaxValue).Select(x => x.ID).ToArray();

//            _context.Cwc_Transport_CitProcessingHistoryExceptions.RemoveRange(_context.Cwc_Transport_CitProcessingHistoryExceptions.Where(x => idsCitProcHist.Contains(x.CitProcessingHistoryID)));
//            _context.SaveChanges();

//            _context.Cwc_Transport_CitProcessingHistories.RemoveRange(_context.Cwc_Transport_CitProcessingHistories.Where(c => idsCitProcHist.Contains(c.ID)));
//            _context.SaveChanges();

//            _context.Cwc_Transport_TransportOrderProducts.RemoveRange(_context.Cwc_Transport_TransportOrderProducts.Where(x => idsTransportOrder.Contains(x.TransportOrderID)));
//            _context.Cwc_Transport_TransportOrderServs.RemoveRange(_context.Cwc_Transport_TransportOrderServs.Where(x => idsTransportOrder.Contains(x.TransportOrderID)));
//            _context.SaveChanges();

//            _context.Cwc_Transport_TransportOrders.RemoveRange(_context.Cwc_Transport_TransportOrders.Where(x => idsTransportOrder.Contains(x.id)));
//            _context.SaveChanges();
//            _context.Dispose();
//        }
//    }
//}
