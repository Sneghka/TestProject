using Cwc.BaseData;
using Cwc.Common;
using Cwc.Contracts;
using Cwc.Ordering;
using Cwc.Ordering.Classes;
using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class OrderBuilder
    {        
        Order entity;
        List<SOProduct> productList = new List<SOProduct>();

        public OrderBuilder()
        {                        
        }

        public OrderBuilder With_OrderID(string value)
        {
            if (entity != null)
            {
                entity.SetID(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_WPOrderID(long value)
        {
            if (entity != null)
            {
                entity.SetWPID(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_DateCreated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateCreated(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_DateUpdated(DateTime? value)
        {
            if (entity != null)
            {
                entity.DateUpdated = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_Comments(string value)
        {
            if (entity != null)
            {
                entity.Comments = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_AuthorID(Int32 value)
        {
            if (entity != null)
            {
                entity.SetAuthorID(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_EditorID(Int32? value)
        {
            if (entity != null)
            {
                entity.EditorID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_OrderType(OrderType value)
        {
            if (entity != null)
            {
                entity.OrderType = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_OrderLevel(OrderLevel value)
        {
            if (entity != null)
            {
                entity.OrderLevel = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_ServiceDate(DateTime? value)
        {
            if (entity != null)
            {
                    
                entity.ServiceDate = value.Value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_NewServiceDate(DateTime value)
        {
            if (entity != null)
            {
                entity.SetNewServiceDate(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_CancelRemark(string value)
        {
            if (entity != null)
            {
                entity.CancelRemark = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_CustomerID(decimal? value)
        {
            if (entity != null)
            {
                entity.CustomerID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_LocationID(decimal value)
        {
            if (entity != null)
            {
                entity.LocationID = value;
                entity.OrderLine.LocationID = value;                
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_LocationCode(string value)
        {
            if (entity != null)
            {
                entity.SetLocationCode(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_LocationCodePickUp(string value)
        {
            if (entity != null)
            {
                entity.SetLocationCodePickUp(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_ReferenceID(string value)
        {
            if (entity != null)
            {
                entity.ReferenceID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_BankReference(string value)
        {
            if (entity != null)
            {
                entity.BankReference = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_CITReference(string value)
        {
            if (entity != null)
            {
                entity.CITReference = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_ServiceTypeCode(string value)
        {
            if (entity != null)
            {
                entity.ServiceTypeCode = value;
                entity.OrderLine.ServiceType = value == "DELV" ? "Deliver" : value == "COLL" ? "Collect" : null;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_OrderedValue(decimal? value)
        {
            if (entity != null)
            {
                var orderLineValue = entity.SpecialCoinsValue == null ? value : (value + entity.SpecialCoinsValue);

                entity.SetOrderedValue(value);                
                entity.OrderLine.SetOrderLineValue(orderLineValue);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_OrderedValue(List<DeliveryProductSpecification> deliveryContent)
        {
            if (entity != null && entity.ServiceDate != null)
            {
                var value = deliveryContent.Sum(x => x.Product.Currency == entity.CurrencyCode 
                    ? x.TotalValue 
                    : BaseDataFacade.ExchangeRateService.GetValueByExchangeRate(x.TotalValue, x.Product.Currency, entity.CurrencyCode, entity.ServiceDate));
                var orderLineValue = entity.SpecialCoinsValue == null ? value : (value + entity.SpecialCoinsValue);

                entity.SetOrderedValue(value);
                entity.OrderLine.SetOrderLineValue(orderLineValue);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first, and ServiceDate property is set.");
        }        

        public OrderBuilder With_PreannouncedValue(decimal? value)
        {
            if (entity != null)
            {
                entity.SetPreannouncedValue(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_SpecialCoinsValue(decimal? value)
        {
            if (entity != null)
            {
                var orderLineValue = entity.OrderedValue == null ? value : (value + entity.OrderedValue);

                entity.SetSpecialCoinsValue(value);
                entity.OrderLine.SetOrderLineValue(orderLineValue);                
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_MasterRoute(int value)
        {
            if (entity != null)
            {
                entity.MasterRouteID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_RouteCode(string value)
        {
            if (entity != null)
            {
                entity.SetRouteCode(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_BranchCode(string value)
        {
            if (entity != null)
            {
                entity.SetBranchCode(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_Branch(Site value)
        {
            if (entity != null)
            {
                entity.SetBranchCode(value.Branch_cd);                
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_CurrencyCode(string value)
        {
            if (entity != null)
            {
                entity.CurrencyCode = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_GenericStatus(GenericStatus? value)
        {
            if (entity != null)
            {
                entity.GenericStatus = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_EMail(string value)
        {
            if (entity != null)
            {
                entity.EMail = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_Optimized(bool value)
        {
            if (entity != null)
            {
                entity.Optimized = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_Exception(bool value)
        {
            if (entity != null)
            {
                entity.IsWithException = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_ReleaseDeadline(DateTime? value)
        {
            if (entity != null)
            {
                entity.ReleaseDeadline = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_TransOptimizerOrderID(decimal? value)
        {
            if (entity != null)
            {
                entity.TransOptimizerOrderID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_ReasonCodeID(Int32? value)
        {
            if (entity != null)
            {
                entity.ReasonCodeID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_CollectPrecreditingDate(DateTime? value)
        {
            if (entity != null)
            {
                entity.CollectPrecreditingDate = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderBuilder With_DateCompleted(DateTime? value)
        {
            if (entity != null)
            {
                entity.SetDateCompleted(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public OrderBuilder With_Product(int quantity, ProductBuilder product, ProductBuilder product2 = null)
        {
            var productAdd = product.Build();
            entity.OrderLine.AddNote(productAdd.ProductCode, quantity, productAdd.Value, productAdd.Currency);

            if (product2 != null)
            {
                var product2Add = product2.Build();
                entity.OrderLine.AddNote(product2Add.ProductCode, quantity, product2Add.Value, product2Add.Currency);
            }            
            return this;
        }
        
        public OrderBuilder WithOrderLineLocation(Location location)
        {
            entity.OrderLine.LocationID = location.ID;
            return this;
        }

        public OrderBuilder With_Product(string productType, Product product, int quantity)
        {            
            entity.OrderLine.AddNoteOrCoin(productType, product.ProductCode, quantity, product.Value, product.Currency);
            return this;
        }

        public OrderBuilder With_Products(Dictionary<Product, int> productCollection)
        {            
            foreach(var item in productCollection)
            {
                switch (item.Key.Type)
                {
                    case "NOTE":
                        entity.OrderLine.AddNote(item.Key.ProductCode, item.Value, item.Key.Value, item.Key.Currency);
                        break;
                    case "COIN":
                        entity.OrderLine.AddCoin(item.Key.ProductCode, item.Value, item.Key.Value, item.Key.Currency);
                        break;
                    case "CONS":
                        entity.OrderLine.AddConsumable(item.Key.ProductCode, item.Value, item.Key.Currency);
                        break;
                    case "SPEC":
                        entity.OrderLine.AddSpecialCoins(item.Key.ProductCode, item.Value, item.Key.Value, item.Key.Currency);
                        break;
                    default:
                        throw new NotImplementedException("Provided product type is not supported!");
                }
            }            
            return this;
        }

        public OrderBuilder With_Products(List<DeliveryProductSpecification> deliveryContent)
        {
            if (entity.OrderLine is null)
            {
                throw new InvalidOperationException($"With_Products method cannot be processed because OrderLine is not defined for order {entity.ID}");
            }

            foreach (var item in deliveryContent)
            {
                var product = new SOProduct
                {
                    OrderLine_ID = entity.OrderLine.ID,
                    ProductCode = item.Product.ProductCode,
                    OrderProductNumber = item.Quantity,
                    OrderProductValue = item.TotalValue,
                    Currency = item.Product.Currency,
                    IsLoose = item.IsLoose
                };

                productList.Add(product);
            }

            return this;
        }

        public OrderBuilder With_Service(string servCode, string servCode2 = null)
        {
            entity.OrderLine.AddService(servCode, true, false);
            if (servCode2 != null)
            {
                entity.OrderLine.AddService(servCode2, true, false);
            }
            return this;
        }

        public OrderBuilder New(OrderType type = OrderType.AtRequest, OrderLevel level = OrderLevel.Service, string currency = "EUR")
        {
            var orderId = OrderingFacade.OrderService.GetNewOrderNumber().Value;
            var id = OrderingFacade.OrderService.GetOrderID(orderId);
            var date = DateTime.Now;

            entity = new Order();
            this.With_OrderID(id);
            this.With_WPOrderID(orderId);
            this.With_OrderType(type);
            this.With_OrderLevel(level);
            this.With_CurrencyCode(currency);
            this.With_DateCreated(date);
            this.With_DateUpdated(date);            
            this.With_Optimized(false);
            this.With_Exception(false);
            entity.CreateNewOrderLine();            
            return this;
        }

        public OrderBuilder New(DateTime serviceDate, Location location, string serviceTypeCode, out string orderNumber, string currency = "EUR")
        {
            var serviceType = BaseDataFacade.ServiceTypeService.LoadByCode(serviceTypeCode, null);
            var settings = ContractsFacade.ContractOrderingSettingsService.MatchContractOrderingSettings(location.CompanyID, serviceType.ID, location.LtCode, location.ID, currency, null, null, null, null);
            var _dbParams = new DataBaseParams(new SqlConnection(DataBaseHelper.GetConnectionString()));

            if (_dbParams.Connection.State == System.Data.ConnectionState.Closed)
            {
                _dbParams.Connection.Open();
            }

            var result = OrderingFacade.OrderService.CreateNewOrder(serviceDate, 
                new Cwc.Common.Data.DateTimePeriod { Begin = DateTime.Now.Date, End = DateTime.Now.Date.AddDays(settings.Value.Period-1) }, 
                location,
                serviceType,
                settings.Value, 
                currency, 
                new List<DateTime> { DateTime.Now.AddDays(-1) }, 
                _dbParams);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Order creation failed. Reason: {result.GetMessage()}");
            }

            orderNumber = result.Value.Order.ID;
            entity = result.Value.Order;
            this.With_WPOrderID(entity.WPOrderID);
            entity.CurrencyCode = currency;
            return this;
        }

        public OrderBuilder New(DateTime serviceDate, Location location, string serviceTypeCode, string currency = "EUR")
        {
            var serviceType = BaseDataFacade.ServiceTypeService.LoadByCode(serviceTypeCode, null);
            var settings = ContractsFacade.ContractOrderingSettingsService.MatchContractOrderingSettings(location.CompanyID, serviceType.ID, location.LtCode, location.ID, currency, null, null, null,null);
            var _dbParams = new DataBaseParams(new SqlConnection(DataBaseHelper.GetConnectionString()));

            if (_dbParams.Connection.State == System.Data.ConnectionState.Closed)
            {
                _dbParams.Connection.Open();
            }

            var result = OrderingFacade.OrderService.CreateNewOrder(serviceDate,
                                                                            new Cwc.Common.Data.DateTimePeriod { Begin = DateTime.Now.Date, End = DateTime.Now.Date.AddDays(settings.Value.Period - 1) },
                                                                            location,
                                                                            serviceType,
                                                                            settings.Value, 
                                                                            currency,
                                                                            new List<DateTime> { DateTime.Now.AddDays(-1) },
                                                                            _dbParams);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Order creation failed. Reason: {result.GetMessage()}");
            }
                        
            entity = result.Value.Order;
            this.With_WPOrderID(entity.WPOrderID);
            entity.CurrencyCode = currency;
            return this;
        }

        public static implicit operator Order(OrderBuilder ins)
        {
            return ins.Build();
        }

        public Order Build()
        {
            return entity;
        }

        public OrderBuilder SaveToDbLite()
        {
            using (var context = new AutomationOrderingDataContext())
            {
                context.Orders.Add(entity);
                context.OrderLines.Add(entity.OrderLine);
                context.SOProduct.AddRange(productList);
                context.SaveChanges();
            }
            return this;
        }

        public OrderBuilder SaveToDb()
        {                       
            var res = OrderingFacade.OrderService.SaveOrder(entity, dbParams: null, productSaveOptions: new ServiceOrderProductSaveOptions { IsSaveNotesAndCoins = true, IsSaveServicingCodes = true }, isModifyContent: true);

            if (!res.IsSuccess)
            {
                throw new InvalidOperationException($"Order saving failed. Reason: {res.GetMessage()}");
            }
            return this;
        }

        /// NOT USED. DO NOT REMOVE
        //public DateTime? DefineServicedate(string servicetypeCode, Location location)
        //{
        //    var serviceType = BaseDataFacade.ServiceTypeService.LoadByCode(servicetypeCode, new DataBaseParams());
        //    var setting = ContractsFacade.ContractOrderingSettingsService.MatchContractOrderingSettings(location.CompanyID, serviceType.ID, location.LtCode, location.ID, "EUR", null, null, null, _dbParams);
        //    var startDate = setting != null ? ContractsFacade.ContractOrderingSettingsService.CalculateOrderingPeriodStartDate(setting.Value, location, null, null).Value : DateTime.Now;
        //    var endDate = setting != null ? startDate.AddDays(setting.Value.Period).Date : startDate.AddDays(30);
                        
        //    for (var date = startDate; date < endDate; date = date.AddDays(1))
        //    {
        //        var res = ContractsFacade.ContractOrderingSettingsService.CheckOrderDay(date,
        //            location.CompanyID,
        //            serviceType.ID,
        //            location.ID, "EUR",
        //            null,
        //            new Cwc.Common.Data.DateTimePeriod { Begin = startDate, End = endDate.AddDays(-1) },
        //            null,
        //            null);

        //        if (res.IsAtRequesAllowed)
        //        {                        
        //            return res.CheckedDay.Date;                        
        //        }
        //    }            
        //    return null;
        //}

        public OrderBuilder Take(Expression<Func<Order, bool>> expression, OrderingDataContext dataContext = null)
        {
            using (var newContext = new OrderingDataContext())
            {
                var context = dataContext ?? newContext;
                var contextOrder = context.Orders.FirstOrDefault(expression);
                if (contextOrder == null)
                {
                    throw new ArgumentNullException("Service Order with provided criteria was not found.");
                }

                entity = OrderingFacade.OrderService.LoadOrder(contextOrder.ID);
            }
            return this;
        }

        public void DeleteMany(IEnumerable<Order> ordertoRemove)
        {
            using (var context = new AutomationOrderingDataContext())
            {
                foreach (var item in ordertoRemove)
                {
                    context.Orders.Attach(item);
                }
                context.Orders.RemoveRange(ordertoRemove);
                context.SaveChanges();
            }
        }

        public void DeleteOne(int[] ordertoRemoveIds)
        {
            using (var context = new AutomationOrderingDataContext())
            {
                foreach (var orderId in ordertoRemoveIds)
                {
                    var order = context.Orders.FirstOrDefault(x => x.IdentityID == orderId);
                    context.Orders.Attach(order);
                    context.Orders.Remove(order);
                }
                context.SaveChanges();
            }
            
        }
    }
}