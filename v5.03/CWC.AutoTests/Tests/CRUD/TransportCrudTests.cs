using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.Common;
using Cwc.Common.Metadata;
using Cwc.Contracts;
using Cwc.Localization;
using Cwc.Security;
using Cwc.Transport;
using Cwc.Transport.Enums;
using Cwc.Transport.Model;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.Tests.Fixtures;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD
{
    [Xunit.Collection("Order")]
    public class TransportCrudTests : IClassFixture<OrderFixture>, IDisposable
    {
        AutomationTransportDataContext _context;
        DateTime date = DateTime.Today;
        int? site;
        string masterRoute, code;
        int transportOrderClassId;
        OrderFixture _fixture;
       
        public TransportCrudTests(OrderFixture fixture)
        {
            _fixture = fixture;
            ConfigurationKeySet.Load();
            _context = new AutomationTransportDataContext();     
            site = _fixture.Location.BranchID; /*!= null ? _fixture.Location.BranchID : _fixture.Location.NotesSiteID;*/
            transportOrderClassId = MetadataHelper.GetClassID(typeof(TransportOrder)).Value;
            masterRoute = "SP20001";
            code = $"1011{new Random().Next(4444,9999)}";
        }  
        
        [Theory(DisplayName = "Transport Order CRUD - When Transport Order -> Location is empty Then System doesn't allow to save it and shows error message", Skip = "Transport orders do not allow manual update")]
        [InlineData("COLL")]
        [InlineData("SERV")]
        [InlineData("DELV")]
        [InlineData("REPL")]
        public void VerifyThatSystemDoesntAllowToCreateTransportOrderWithoutLocation(string serType)
        {
            using (var context = DataContext.Create<TransportDataContext>())
            {
                var isCommited = false;
                try
                {
                    context.BeginTransaction();
                    var serviceOrder = DataFacade.Order.New(date, _fixture.Location, _fixture.ServiceType.Code).SaveToDb().Build();
                    var transportOrder = DataFacade.TransportOrder.New()
                                            .With_Code(code)
                                            .With_Site(_fixture.Location.ServicingDepotID)
                                            .With_OrderType(OrderType.AtRequest)
                                            .With_TransportDate(date)
                                            .With_ServiceDate(date)
                                            .With_Status(TransportOrderStatus.Planned)
                                            .With_ServiceOrder(serviceOrder.ID)
                                            .With_ServiceType(_fixture.ServiceType.ID)
                                            .With_MasterRouteCode(masterRoute)
                                            .Build();
                    var userLogin = SecurityFacade.LoginService.GetAdministratorLogin();
                    var userParams = new UserParams(userLogin);
                    var result = TransportFacade.TransportOrderService.Save(transportOrder, userParams, context);

                    Assert.False(result.IsSuccess);
                    Assert.Equal("Service order, service date and location must be defined for transport order.", result.Messages.First());                    
                }
                finally
                {
                    context.EndTransaction(isCommited);
                }
            }
        }
        
        [Theory(DisplayName = "Transport Order CRUD - When Transport Order -> Service Date is empty Then System doesn't allow to save it and shows error message", Skip = "Transport orders do not allow manual update")]
        [InlineData("COLL")]
        [InlineData("SERV")]
        [InlineData("DELV")]
        [InlineData("REPL")]
        public void VerifyThatSystemDoesntAllowToCreateTransportOrderWithoutServiceDate(string serType)
        {
            using (var context = DataContext.Create<TransportDataContext>())
            {
                var isCommited = false;
                try
                {
                    context.BeginTransaction();                   
                    var serviceOrder = DataFacade.Order.New(date, _fixture.Location, _fixture.ServiceType.Code).SaveToDb().Build();
                    // serviceType = DataFacade.ServiceType.Take(x => x.Code == serType);
                    var transportOrer = DataFacade.TransportOrder.New()
                                                        .With_Code(code)
                                                        .With_Site(_fixture.Location.ServicingDepotID)
                                                        .With_OrderType(OrderType.AtRequest)
                                                        .With_TransportDate(date)
                                                        .With_Location(_fixture.Location.ID)
                                                        .With_Status(TransportOrderStatus.Planned)
                                                        .With_ServiceOrder(serviceOrder.ID)
                                                        .With_ServiceType(_fixture.ServiceType.ID)
                                                        .With_MasterRouteCode(masterRoute)
                                                        .Build();
                    var userLogin = SecurityFacade.LoginService.GetAdministratorLogin();
                    var userParams = new UserParams(userLogin);
                    var result = TransportFacade.TransportOrderService.Save(transportOrer, userParams, context);

                    Assert.False(result.IsSuccess);
                    Assert.Equal("Service order, service date and location must be defined for transport order.", result.Messages.First());

                }
                finally
                {
                    context.EndTransaction(isCommited);
                }
            }
        }
        
        [Theory(DisplayName = "Transport Order CRUD - When Transport Order -> Service Order is empty Then System doesn't allow to save it and shows error message", Skip = "Transport orders do not allow manual update")]
        [InlineData("COLL")]
        [InlineData("SERV")]
        [InlineData("DELV")]
        [InlineData("REPL")]
        public void VerifyThatSystemDoesntAllowToCreateTransportOrderWithoutServiceOrder(string serType)
        {
            using (var context = DataContext.Create<TransportDataContext>())
            {
                var isCommited = false;
                try
                {
                    context.BeginTransaction();                    
                    var serviceOrder = DataFacade.Order.New(date, _fixture.Location, _fixture.ServiceType.Code).SaveToDb().Build();
                    var transportOrer = DataFacade.TransportOrder.New()
                                                        .With_Code(code)
                                                        .With_Site(_fixture.Location.ServicingDepotID)
                                                        .With_OrderType(OrderType.AtRequest)
                                                        .With_TransportDate(date)
                                                        .With_Location(_fixture.Location.ID)
                                                        .With_ServiceDate(date)
                                                        .With_Status(TransportOrderStatus.Planned)
                                                        .With_ServiceOrder((string)null)
                                                        .With_ServiceType(_fixture.ServiceType.ID)
                                                        .With_MasterRouteCode(masterRoute)
                                                        .Build();
                    var userLogin = SecurityFacade.LoginService.GetAdministratorLogin();
                    var userParams = new UserParams(userLogin);
                    var result = TransportFacade.TransportOrderService.Save(transportOrer, userParams, context);

                    Assert.False(result.IsSuccess);
                    Assert.Equal("Service order, service date and location must be defined for transport order.", result.Messages.First());
                }
                finally
                {
                    context.EndTransaction(isCommited);
                }
            }
        }
        
        [Fact(DisplayName = "Transport Order CRUD - When new transport order is creating with {Location, service type, order type, transport date} as in the existed Then System doesn't allow to create it", Skip = "Transport orders do not allow manual update")]
        public void VerifyThatSystemDoesntAllowTOCreateNewTransportOrderIfExistssTheSme()
        {
            var transportOrer = DataFacade.TransportOrder.New()
                                                        .With_Code("SP20001")
                                                        .With_Site(_fixture.Location.ServicingDepotID)
                                                        .With_OrderType(OrderType.AtRequest)
                                                        .With_TransportDate(date)
                                                        .With_Location(_fixture.Location.ID)
                                                        .With_ServiceDate(date.AddDays(2))
                                                        .With_Status(TransportOrderStatus.Planned)
                                                        .With_ServiceOrder(_fixture.ServiceOrder.ID)
                                                        .With_MasterRouteCode(masterRoute)
                                                        .With_ServiceType(_fixture.ServiceType.ID)
                                                        .SaveToDb();
            var duplicateTransportOrder = DataFacade.TransportOrder.New()
                                                        .With_Code(code)
                                                        .With_Site(_fixture.Location.ServicingDepotID)
                                                        .With_OrderType(OrderType.AtRequest)
                                                        .With_TransportDate(date)
                                                        .With_Location(_fixture.Location.ID)
                                                        .With_ServiceDate(date.AddDays(2))
                                                        .With_Status(TransportOrderStatus.Planned)
                                                        .With_ServiceOrder(_fixture.ServiceOrder.ID)
                                                        .With_MasterRouteCode(masterRoute)
                                                        .With_ServiceType(_fixture.ServiceType.ID);
            var result = TransportFacade.TransportOrderService.Save(duplicateTransportOrder);
            var assertMsg = $"New transport order for location {_fixture.Location.DisplayCaption} cannot be created - the order with order type {LocalizationFacade.UniqueInstance.ResourceServicesGetEnumValue(duplicateTransportOrder.Build().OrderType)} and service type {_fixture.ServiceType.DisplayCaption} is already scheduled on {duplicateTransportOrder.Build().TransportDate.ToShortDateString()}.";

            Assert.False(result.IsSuccess);
            Assert.Equal(assertMsg, result.Messages.First());
        }

        [Theory(DisplayName = "Transport Order CRUD - When new Transport order is created Then System creates Cit Processing history for it", Skip = "Transport orders do not allow manual update")]
        [InlineData(true)]
        [InlineData(false)]
        public void VerifyThatCitProcessingHistoryIsCreatedForNewOrder(bool isWithException)
        {
            var transportOrder = DataFacade.TransportOrder.New()
                                                        .With_Code("SP20001")
                                                        .With_Site(_fixture.Location.ServicingDepotID)
                                                        .With_OrderType(OrderType.AtRequest)
                                                        .With_TransportDate(date)
                                                        .With_Location(_fixture.Location.ID)
                                                        .With_ServiceDate(date.AddDays(2))
                                                        .With_Status(TransportOrderStatus.Planned)
                                                        .With_ServiceOrder(_fixture.ServiceOrder.ID)
                                                        .With_ServiceType(_fixture.ServiceType.ID)
                                                        .With_IsWithException(isWithException)
                                                        .With_MasterRouteCode(masterRoute)
                                                        .SaveToDb()
                                                        .Build();

            Assert.True(_context.CitProcessingHistories.Where(cph => cph.ObjectID == transportOrder.ID && cph.ObjectClassID == transportOrderClassId && cph.IsWithException == isWithException && cph.Status == (int)TransportOrderStatus.Planned).Any());
        }

        [Theory(DisplayName = "Transport Order CRUD - When new status is not equals to old status Then System creates processing history with proper data", Skip = "Transport orders do not allow manual update")]
        [InlineData(true)]
        [InlineData(false)]
        public void VerifyThatSystemCreatesProcessingHistoryOnSTatusUpdate(bool isWithException)
        {
            var transportOrder = DataFacade.TransportOrder.New()
                               .With_Code(code)
                               .With_Site(_fixture.Location.ServicingDepotID)
                               .With_OrderType(OrderType.AtRequest)
                               .With_TransportDate(date)
                               .With_Location(_fixture.Location.ID)
                               .With_ServiceDate(date.AddDays(2))
                               .With_Status(TransportOrderStatus.Planned)
                               .With_ServiceOrder(_fixture.ServiceOrder.ID)
                               .With_ServiceType(_fixture.ServiceType.ID)
                               .With_MasterRouteCode(masterRoute)
                               .SaveToDb();

            var updatedTO = transportOrder.With_Status(TransportOrderStatus.InTransit).With_IsWithException(isWithException).Build();

            var result = TransportFacade.TransportOrderService.Save(updatedTO);

            Assert.True(result.IsSuccess);
            Assert.True(_context.TransportOrders.Where(c => c.Code == updatedTO.Code).First().Status == TransportOrderStatus.InTransit);
            Assert.True(_context.CitProcessingHistories.Where(x => x.ObjectID == updatedTO.ID && x.ObjectClassID == transportOrderClassId && x.Status == (int)updatedTO.Status && x.IsWithException == isWithException).Any());
        }

        [Theory(DisplayName = "Transport Order CRUD - When new status in 'To claridy..' Then System creates Processing History Exception", Skip = "Transport orders do not allow manual update")]
        [InlineData(TransportOrderStatus.ToClarifyCancellation, ExceptionCaseName.StopCancellingDuringRoute)]
        [InlineData(TransportOrderStatus.ToClarifyCompletion, ExceptionCaseName.NotExecutedServiceDuringVisit)]
        [InlineData(TransportOrderStatus.ToClarifyNewAdhocOrder, ExceptionCaseName.NotPlannedOrderIsPerformed)]
        public void VerifyThatSystemCreatesProcessingHistoryExceptionProperly(TransportOrderStatus status, ExceptionCaseName caseName)
        {
            var transportOrder = DataFacade.TransportOrder.New()
                               .With_Code(code)
                               .With_Site(_fixture.Location.ServicingDepotID)
                               .With_OrderType(OrderType.AtRequest)
                               .With_TransportDate(date)
                               .With_Location(_fixture.Location.ID)
                               .With_ServiceDate(date.AddDays(2))
                               .With_Status(TransportOrderStatus.Planned)
                               .With_ServiceOrder(_fixture.ServiceOrder.ID)
                               .With_ServiceType(_fixture.ServiceType.ID)
                               .With_MasterRouteCode(masterRoute)
                               .SaveToDb();

            var updatedTO = transportOrder.With_Status(status).Build();
            var result = TransportFacade.TransportOrderService.Save(updatedTO);
            var foundCitProcessingHistory = _context.CitProcessingHistories.Where(x => x.ObjectID == updatedTO.ID && x.ObjectClassID == transportOrderClassId && x.Status == (int)status).First();
            var foudnExceptionCase = _context.ExceptionCases.Where(x => x.Name == caseName).First();

            Assert.True(result.IsSuccess);
            Assert.Equal(1, _context.CitProcessingHistoryExceptions.Where(x => x.CitProcessingHistoryID == foundCitProcessingHistory.ID && x.ExceptionCaseID == foudnExceptionCase.ID && x.SiteID == updatedTO.SiteID && x.Status == ProcessingHistoryExceptionStatus.InProgress).Count());
        }

        [Fact(DisplayName = "Transport Order CRUD - When User params contains userid and workstation Then System craetes Cit Processing Histry with this info", Skip = "Transport orders do not allow manual update")]
        public void VerifyThatCitProcessSettingsCreatedWithProperlyUserInfo()
        {
            var user = SecurityFacade.LoginService.GetAdministratorLogin();
            var userp = new UserParams { UserID = user.UserID, WorkstationID = 2 };
            var citsetting = DataFacade.CitProcessingHistory.New().
                With_Status(1).
                With_DateCreated(date).
                With_ObjectID(int.MaxValue).
                With_ObjectClassID(transportOrderClassId).
                With_IsWithException(false).
                With_UserParams(userp).
                SaveToDb();
            var foundSetting = _context.CitProcessingHistories.Where(c => c.ObjectID == int.MaxValue).First();
            Assert.True(foundSetting.AuthorID == user.UserID && foundSetting.WorkstationID == 2);
        }

        [Fact(DisplayName = "Transport Order CRUD - When Cit Processing History is deleted Then System removes all linked exceptions", Skip = "Transport orders do not allow manual update")]
        public void VerifyThatExceptionIsDeletedWhenCitProcessingHistoryIsDeleted()
        {
            var exceptionCase = _context.ExceptionCases.Where(x => x.Name == ExceptionCaseName.NotExecutedServiceDuringVisit).First();
            var citsProcHist = DataFacade.CitProcessingHistory.New()
                .With_Status(1)
                .With_DateCreated(date)
                .With_ObjectID(int.MaxValue)
                .With_ObjectClassID(transportOrderClassId)
                .With_IsWithException(false)
                .SaveToDb()
                .Build();
            var citSettingException1 = DataFacade.CitProcessingHistoryException.New()
                .With_CitProcessingHistoryID(citsProcHist.ID)
                .With_Status(ProcessingHistoryExceptionStatus.InProgress)
                .With_Action(ExceptionAction.ConfirmCompletion)
                .With_Exception(exceptionCase.ID)
                .With_SiteID(site.Value)
                .With_WorkstationID(48)
                .SaveToDb();
            var citSettingException2 = DataFacade.CitProcessingHistoryException.New()
                .With_CitProcessingHistoryID(citsProcHist.ID)
                .With_Action(ExceptionAction.CancelAndScheduleNew)
                .With_Status(ProcessingHistoryExceptionStatus.InProgress)
                .With_Exception(exceptionCase.ID)
                .With_SiteID(site.Value)
                .With_WorkstationID(48)
                .SaveToDb();
            var result = TransportFacade.CitProcessingHistoryService.Delete(citsProcHist);

            Assert.True(result.IsSuccess);
            Assert.False(_context.CitProcessingHistoryExceptions.Where(x => x.CitProcessingHistoryID == citsProcHist.ID).Any());
            Assert.False(_context.CitProcessingHistories.Where(x => x.ID == citsProcHist.ID).Any());
        }

        [Fact(DisplayName = "When Transport Order is updated Then System creates new Cit Processing History record", Skip = "Transport orders do not allow manual update")]
        public void VerifyThatSystemCreatesCitProcHistoryOnEveryOrderUpdate()
        {
            var transportOrderDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(_fixture.Location, _fixture.ServiceType, OrderType.AtRequest);
            var transportOrder = DataFacade.TransportOrder.New()
                .With_Site(_fixture.Location.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(transportOrderDate)
                .With_Location(_fixture.Location.ID)
                .With_ServiceDate(transportOrderDate)
                .With_Status(TransportOrderStatus.Registered)
                .With_ServiceOrder(_fixture.ServiceOrder.ID)
                .With_ServiceType(_fixture.ServiceType.ID)
                .With_MasterRouteCode(masterRoute)
                .SaveToDb();
            transportOrder.With_Status(TransportOrderStatus.Planned).SaveToDb();
            var foundCitProcHistoriees = _context.CitProcessingHistories.ToArray().Where(cph => cph.ObjectID == transportOrder.Build().ID);

            Assert.Equal(2, foundCitProcHistoriees.Count());
            Assert.True(foundCitProcHistoriees.Any(x => x.Status == (int)TransportOrderStatus.Registered), "Creation status is not found");
            Assert.True(foundCitProcHistoriees.Any(x => x.Status == (int)TransportOrderStatus.Planned), "Updation status is not found");
        }

        [Fact(DisplayName = "When new transport order status is equals to old Then System doesn't create Cit Proc History record", Skip = "Transport orders do not allow manual update")]
        public void VerifyThatSystemDoesntCreateCitProcHistoryWhenStatusIsEquals()
        {
            var transportOrderDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(_fixture.Location, _fixture.ServiceType, OrderType.AtRequest);
            var transportOrder = DataFacade.TransportOrder.New()
                .With_Site(_fixture.Location.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(transportOrderDate)
                .With_Location(_fixture.Location.ID)
                .With_ServiceDate(transportOrderDate)
                .With_Status(TransportOrderStatus.Registered)
                .With_ServiceOrder(_fixture.ServiceOrder.ID)
                .With_ServiceType(_fixture.ServiceType.ID)
                .With_MasterRouteCode(masterRoute)
                .SaveToDb();
            transportOrder.With_Status(TransportOrderStatus.Registered).SaveToDb();
            var foundCitProcHistoriees = _context.CitProcessingHistories.ToArray().Where(cph => cph.ObjectID == transportOrder.Build().ID);

            Assert.Equal(1, foundCitProcHistoriees.Count());
            Assert.True(foundCitProcHistoriees.Any(x => x.Status == (int)TransportOrderStatus.Registered), "Creation status is not found");
            Assert.False(foundCitProcHistoriees.Any(x => x.Status != (int)TransportOrderStatus.Registered), "Updation status is not found");
        }

        [Fact(DisplayName = "When new transport order status is equals to old status and IsCreateAndCompleteException = yes Then System creates new Cit Proc History", Skip = "Transport orders do not allow manual update")]
        public void VerifyThatSystemreatesNewCitProcHistoryWhenCreateExceptionFlagIsYes()
        {
            var transportOrderDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(_fixture.Location, _fixture.ServiceType, OrderType.AtRequest);

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Site(_fixture.Location.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(transportOrderDate)
                .With_Location(_fixture.Location.ID)
                .With_ServiceDate(transportOrderDate)
                .With_Status(TransportOrderStatus.Registered)
                .With_ServiceOrder(_fixture.ServiceOrder.ID)
                .With_ServiceType(_fixture.ServiceType.ID)
                .With_MasterRouteCode(masterRoute)
                .SaveToDb();

            transportOrer.With_Status(TransportOrderStatus.Registered).Update(processName: ProcessName.PerformDailyRoute, processPhase: ProcessPhase.Start, isCreateAndCompleteException: true);

            var foundCitProcHistoriees = _context.CitProcessingHistories.ToArray().Where(cph => cph.ObjectID == transportOrer.Build().ID);

            Assert.Equal(2, foundCitProcHistoriees.Count());
            Assert.True(foundCitProcHistoriees.Any(x => x.Status == (int)TransportOrderStatus.Registered && x.ProcessName == null), "Creation status is not found");
            Assert.True(foundCitProcHistoriees.Any(x => x.Status == (int)TransportOrderStatus.Registered && x.ProcessName == ProcessName.PerformDailyRoute), "Updation status is not found");
        }

        [Theory(DisplayName = "When new Transport Order status is cancelled Then System creates new Cit Processing History Exception", Skip = "Transport orders do not allow manual update")]
        [InlineData(true)]
        [InlineData(false)]
        public void VerifyThatSystemCreatesCitProcHistoryExceptionWhenStatusIsChangesToCancelled(bool isBillable)
        {
            var reasobCode = _context.ReasonCodes.FirstOrDefault(r=>r.IsCustomerResponsible == isBillable);
            var transportOrderDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(_fixture.Location, _fixture.ServiceType, OrderType.AtRequest);

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Site(_fixture.Location.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(transportOrderDate)
                .With_Location(_fixture.Location.ID)
                .With_ServiceDate(transportOrderDate)
                .With_Status(TransportOrderStatus.Registered)
                .With_ServiceOrder(_fixture.ServiceOrder.ID)
                .With_ServiceType(_fixture.ServiceType.ID)
                .With_MasterRouteCode(masterRoute)
                .SaveToDb();

            transportOrer.With_Status(TransportOrderStatus.Cancelled).With_PDAReasonCodeID(reasobCode.ID).SaveToDb();

            var foundCitProcHist = _context.CitProcessingHistories.ToArray().FirstOrDefault(x=>x.ObjectID == transportOrer.Build().ID && x.Status == (int)TransportOrderStatus.Cancelled);
            var foundCitProcHistException = _context.CitProcessingHistoryExceptions.FirstOrDefault(x=>x.CitProcessingHistoryID == foundCitProcHist.ID);

            Assert.NotNull(foundCitProcHistException);
            Assert.Equal((int)TransportOrderStatus.Cancelled, foundCitProcHist.Status);
            Assert.Equal(transportOrer.Build().IsWithException, foundCitProcHist.IsWithException);
            Assert.Equal(ProcessingHistoryExceptionStatus.Registered, foundCitProcHistException.Status);
            Assert.Equal(_context.ExceptionCases.FirstOrDefault(x=>x.Name == (int)ExceptionCaseName.StopCancellingDuringRoute).ID, foundCitProcHistException.ExceptionCaseID);
            Assert.Equal(transportOrer.Build().SiteID, foundCitProcHistException.SiteID);
            Assert.Equal(isBillable, transportOrer.Build().IsBillable);
        }
        
        public void Dispose()
        {
           // var idsTransportOrder = TransportOrders.Where(t => t.MasterRouteCode == masterRoute || (t.LocationID == _fixture.Location.ID && t.ServiceOrderID == _fixture.ServiceOrder.ID)).Select(x=>x.id).ToArray();
            var idsCitProcHist = _context.CitProcessingHistories.Where(c => c.ObjectID == int.MaxValue).Select(x => x.ID).ToArray();

            _context.CitProcessingHistoryExceptions.RemoveRange(_context.CitProcessingHistoryExceptions.Where(x => idsCitProcHist.Contains(x.CitProcessingHistoryID)));
            _context.CitProcessingHistories.RemoveRange(_context.CitProcessingHistories.Where(c => idsCitProcHist.Contains(c.ID)));
            //  _context.Cwc_Transport_TransportOrderProducts.RemoveRange(_context.Cwc_Transport_TransportOrderProducts.Where(x => idsTransportOrder.Contains(x.TransportOrderID)));
            //  TransportOrderservs.RemoveRange(TransportOrderservs.Where(x => idsTransportOrder.Contains(x.TransportOrderID)));
            //  TransportOrders.RemoveRange(TransportOrders.Where(x=>idsTransportOrder.Contains(x.id)));
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
