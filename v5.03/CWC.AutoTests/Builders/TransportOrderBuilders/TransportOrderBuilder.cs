using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.Common;
using Cwc.Contracts;
using Cwc.Ordering;
using Cwc.Transport;
using Cwc.Transport.Enums;
using Cwc.Transport.Model;
using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class TransportOrderBuilder : IDisposable
    {
        DataBaseParams _dbParams;
        TransportOrder entity;
        AutomationTransportDataContext context;
        public LocationBuilder location;
        public OrderBuilder order;

        public TransportOrderBuilder()
        {
            ConfigurationKeySet.Load();
            _dbParams = new DataBaseParams();
            context = new AutomationTransportDataContext();
        }

        public TransportOrderBuilder With_Code(String value)
        {
            if (entity != null)
            {
                entity.SetCode(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_ReferenceCode(String value)
        {
            if (entity != null)
            {
                entity.SetReferenceCode(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_OrderType(OrderType value)
        {
            if (entity != null)
            {
                entity.SetOrderType(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_TransportDate(DateTime value)
        {
            if (entity != null)
            {
                entity.SetTransportDate(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_ServiceDate(DateTime? value)
        {
            if (entity != null)
            {
                entity.SetServiceDate(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_Status(TransportOrderStatus value)
        {
            if (entity != null)
            {
                entity.SetStatus(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_IsWithException(Boolean value)
        {
            if (entity != null)
            {
                entity.SetIsWithException(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_IsPdaAdHoc(Boolean value)
        {
            if (entity != null)
            {
                entity.SetIsPdaAdHoc(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_MasterRouteCode(String value)
        {
            if (entity != null)
            {
                entity.MasterRouteCode = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_MasterRouteDate(DateTime? value)
        {
            if (entity != null)
            {
                entity.MasterRouteDate = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_StopArrivalTime(TimeSpan? value)
        {
            if (entity != null)
            {
                entity.StopArrivalTime = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_VisitSequence(Int32? value)
        {
            if (entity != null)
            {
                entity.VisitSequence = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_IsBillable(Boolean value)
        {
            if (entity != null)
            {
                entity.SetIsBillable(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_ServiceType(Int32 value)
        {
            if (entity != null)
            {
                entity.SetServiceTypeID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_ServiceType(ServiceType value)
        {
            if (entity != null)
            {

                entity.SetServiceTypeID(value.ID);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_Site(Int32? value)
        {
            if (entity != null)
            {
                entity.SetSiteID(value.Value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_Site(Site value)
        {
            if (entity != null)
            {

                entity.SetSiteID(value.ID);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_Location(Decimal? value)
        {
            if (entity != null)
            {
                entity.SetLocationID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_Location(Location value)
        {
            if (entity != null)
            {

                entity.SetLocationID(value.ID);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_LocationPickUpCode(String value)
        {
            if (entity != null)
            {
                entity.SetLocationPickUpCode(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_ServiceOrder(String value)
        {
            if (entity != null)
            {
                entity.SetServiceOrderID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_ServiceOrder(Order value)
        {
            if (entity != null)
            {

                entity.SetServiceOrderID(value.ID);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_PDAReasonCodeID(Int32? value)
        {
            if (entity != null)
            {
                entity.PDAReasonCodeID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_DateCreated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateCreated(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_DateUpdated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateUpdated(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_IsOnHold(bool value)
        {
            entity.SetIsOnHold(value);
            return this;
        }

        public TransportOrderBuilder With_PuttedOnHoldDate(DateTime value)
        {
            entity.DatePuttedOnHold = value;
            return this;
        }

        public TransportOrderBuilder With_AuthorID(Int32? value)
        {
            if (entity != null)
            {
                entity.SetAuthorID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_EditorID(Int32? value)
        {
            if (entity != null)
            {
                entity.SetEditorID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderBuilder With_DeliveryType(int? servitypeId = null)
        {
            if (servitypeId.HasValue)
            {
                entity.DeliveryServiceTypeId = servitypeId.Value;
            }
            else
            {
                entity.DeliveryServiceTypeId = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build().ID;
            }

            return this;
        }

        public TransportOrderBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public DateTime DefineApplicableTransportOrderDate(Location location, ServiceType serviceType, OrderType ordertype = OrderType.AtRequest)
        {
            var maxDate = (from tro in context.TransportOrders
                          where tro.LocationID == location.ID && tro.ServiceTypeID == serviceType.ID && tro.OrderType == ordertype
                          orderby tro.ServiceDate descending
                          select tro.ServiceDate).FirstOrDefault();

            return maxDate != null ?  maxDate.Value.AddDays(1) : DateTime.Now;
        }

        public TransportOrderBuilder New()
        {
            entity = new TransportOrder();
            entity.SetDateCreated(DateTime.Now);
            entity.SetDateUpdated(DateTime.Now);
            
            return this;
        }

        public static implicit operator TransportOrder(TransportOrderBuilder ins)
        {
            return ins.Build();
        }

        public TransportOrder Build()
        {
            return entity;
        }

        public TransportOrderBuilder SaveToDb()
        {
            var result = TransportFacade.TransportOrderService.Save(entity);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Transport Order saving failed. Reason: {result.GetMessage()}");
            }

            return this;
        }

        public TransportOrderBuilder Update(UserParams userParams = null, AutomationTransportDataContext transportDataContext = null, 
            ProcessName? processName = null, ProcessPhase? processPhase = null, bool isDefineServiceOrderStatus = true, 
            bool isCreateAndCompleteException = false, bool isReferenceOnly = false, ExceptionAction? action = null, string remark = null, bool isExport = false)
        {
            var result = TransportFacade.TransportOrderService.UpdateTransportOrder(entity, 
                                                                                    userParams: userParams, 
                                                                                    context: transportDataContext, 
                                                                                    processName: processName, 
                                                                                    processPhase: processPhase, 
                                                                                    isDefineServiceOrderStatus: isDefineServiceOrderStatus, 
                                                                                    isCreateAndCompleteException: isCreateAndCompleteException, 
                                                                                    action: action, 
                                                                                    remark: remark, 
                                                                                    isExport: isExport);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Transport order update failed. Reason: {result.GetMessage()}");
            }

            return this;
        }

        public TransportOrderBuilder Take(Expression<Func<TransportOrder, bool>> expression)
        {
            entity = context.TransportOrders.FirstOrDefault(expression);

            if (entity == null)
            {
                throw new ArgumentNullException("Transport Order with given criteria is not found");
            }

            return this;
        }

        public IEnumerable<TransportOrder> TakeAll(Expression<Func<TransportOrder, bool>> expression)
        {
            return this.context.TransportOrders.Where(expression);
        }

        public TransportOrderBuilder InitDefault(Order orderFrom)
        {
            if(orderFrom == null)
            {
                throw new InvalidOperationException("Transport Order cannot be created from an empty Service Order");
            }

            var location = DataFacade.Location.Take(x => x.ID == orderFrom.LocationID).Build();
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == orderFrom.ServiceTypeCode).Build();

            entity = DataFacade.TransportOrder.New()
                 .With_Location(location.ID)
                 .With_ServiceOrder(orderFrom.ID)
                 .With_ServiceDate(orderFrom.ServiceDate)
                 .With_TransportDate(orderFrom.ServiceDate)
                 .With_ServiceType(serviceType.ID)
                 .With_OrderType(orderFrom.OrderType)
                 .With_Site(location.BranchID);

            return this;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}