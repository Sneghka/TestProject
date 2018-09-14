using Cwc.BaseData;
using Cwc.BaseData.Model;
using Cwc.Common;
using Cwc.Contracts;
using Cwc.Ordering;
using Cwc.Security;
using Cwc.Transport;
using Cwc.Transport.Enums;
using Cwc.Transport.Model;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.DailyDataBuilders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CWC.AutoTests.Helpers.Transport
{
    public class TransportHelper
    {        
        private string orderNumber;        

        public TransportHelper()
        {            
        }

        /// <summary>
        /// Run "Order CIT Allocation" job
        /// </summary>
        public void RunCitAllocationJob()
        {
            var dbExecutor = new DataBaseExecutor();
            dbExecutor.OpenConnection();
            try
            {
                TransportFacade.OrderCitAllocationManagementService.AllocateServiceOrdersToTransportOrders(dbExecutor.DataBaseParams);
            }
            catch
            {
                throw;
            }
            finally
            {
                dbExecutor.CloseConnection();
            }
        }

        public void RunAutomatedLocationServicesCreationJob()
        {
            using (var context = new AutomationCashPointDataContext())
            {
                var dbExecutor = new DataBaseExecutor();
                dbExecutor.OpenConnection();
                try
                {
                    var settingsID = context.AutomatedOrderCreationSettings.First().ID;
                    var settings = ContractsFacade.LocationServiceCreationJobSettingsService.Load(settingsID.Value, dbExecutor.DataBaseParams);
                    var result = ContractsFacade.LocationServiceCreationJobSettingsService.AutomatedLocationServiceCreation(settings);
                    if (!result.IsSuccess)
                    {
                        throw new InvalidOperationException($"Location service creation failed. Reason: {result.GetMessage()}");
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    dbExecutor.CloseConnection();
                }
            }
        }

        public Order CreateServiceOrderWithContentLite(string serviceType, DateTime serviceDate, GenericStatus status, Location location, List<DeliveryProductSpecification> deliveryContent)
        {
            var branch = DataFacade.Site.Take(x => x.ID == location.BranchID);

            return DataFacade.Order.New()
                .With_ServiceTypeCode(serviceType)
                .With_ServiceDate(serviceDate)
                .With_GenericStatus(status)
                .With_CustomerID(location.CompanyID)
                .With_OrderedValue(deliveryContent)
                .With_LocationCode(location.Code)
                .With_LocationID(location.ID)
                .With_Branch(branch)
                .With_Products(deliveryContent)
                .SaveToDbLite();
        }

        /// <summary>
        /// Create service order for CIT allocation
        /// </summary>
        /// <param name="status">Generic status of service order</param>
        /// <param name="serviceType">Service type of service order</param>
        /// <param name="location">Location of service order</param>
        /// <param name="withProducts">Flag to create SOProducts</param>
        /// <param name="withServices">Flag to create SOServices</param>
        /// <returns>Created service order entity</returns>
        public Order CreateServiceOrder(DateTime serviceDate, GenericStatus status, string serviceType, Location location, bool withProducts = true, bool withServices = true, 
            bool withBarcodedProducts = false)
        {
            var currencyCode = "EUR";
            OrderBuilder order;
            var quantity = 1;

            using (var context = new AutomationBaseDataContext())
            {
                if (withBarcodedProducts)
                {                    
                    var barcodedProductCode = context.Products.Where(p => p.Currency == "EUR" && p.Type == "NOTE" && p.IsBarcodedProduct).First().ProductCode;
                    order = DataFacade.Order
                        .New(serviceDate, location, serviceType, out orderNumber)
                        .With_OrderType(OrderType.AtRequest)
                        .With_ServiceTypeCode(serviceType)
                        .With_LocationCode(location.Code)
                        .With_LocationID(location.ID)
                        .With_GenericStatus(status)
                        .With_ServiceDate(serviceDate)
                        .With_CurrencyCode(currencyCode)
                        .With_CustomerID(location.CompanyID)
                        .With_Product(quantity, DataFacade.Product.Take(barcodedProductCode))
                        .SaveToDb();
                }

                // ATTENTION!!! On editing product codes and service codes below don't forget to edit product codes and service codes
                // in "UpdateServiceOrder" method because that method operates with "old" and "new" definitions of products and services
                var productCode = context.Products.Where(p => p.Currency == "EUR" && p.Value == 100 && p.Type == "NOTE").First().ProductCode;
                var productCode2 = context.Products.Where(p => p.Currency == "EUR" && p.Value == 50 && p.Type == "NOTE").First().ProductCode;                

                if (withProducts && withServices)
                {
                    order = DataFacade.Order
                        .New(serviceDate, location, serviceType, out orderNumber)
                        .With_OrderType(OrderType.AtRequest)
                        .With_ServiceTypeCode(serviceType)
                        .With_LocationCode(location.Code)
                        .With_LocationID(location.ID)
                        .With_GenericStatus(status)
                        .With_ServiceDate(serviceDate)
                        .With_CurrencyCode(currencyCode)
                        .With_CustomerID(location.CompanyID)
                        .With_Product(quantity, DataFacade.Product.Take(productCode), DataFacade.Product.Take(productCode2))
                        .With_Service("1", "2")
                        .SaveToDb();
                }
                else if (withProducts && !withServices)
                {
                    order = DataFacade.Order
                        .New(serviceDate, location, serviceType, out orderNumber)
                        .With_OrderType(OrderType.AtRequest)
                        .With_ServiceTypeCode(serviceType)
                        .With_LocationCode(location.Code)
                        .With_LocationID(location.ID)
                        .With_GenericStatus(status)
                        .With_ServiceDate(serviceDate)
                        .With_CurrencyCode(currencyCode)
                        .With_CustomerID(location.CompanyID)
                        .With_Product(quantity, DataFacade.Product.Take(productCode), DataFacade.Product.Take(productCode2))
                        .SaveToDb();
                }
                else if (!withProducts && withServices)
                {
                    // ATTENTION!!! On editing product codes and service codes below don't forget to edit product codes and service codes
                    // in "UpdateServiceOrder" method because that method operates with "old" and "new" definitions of products and services
                    order = DataFacade.Order
                        .New(serviceDate, location, serviceType, out orderNumber)
                        .With_OrderType(OrderType.AtRequest)
                        .With_ServiceTypeCode(serviceType)
                        .With_LocationCode(location.Code)
                        .With_LocationID(location.ID)
                        .With_GenericStatus(status)
                        .With_ServiceDate(serviceDate)
                        .With_CurrencyCode(currencyCode)
                        .With_CustomerID(location.CompanyID)
                        .With_Service("1", "2")
                        .SaveToDb();
                }
                else
                {
                    order = DataFacade.Order
                        .New(serviceDate, location, serviceType, out orderNumber)
                        .With_OrderType(OrderType.AtRequest)
                        .With_ServiceTypeCode(serviceType)
                        .With_LocationCode(location.Code)
                        .With_LocationID(location.ID)
                        .With_GenericStatus(status)
                        .With_ServiceDate(serviceDate)
                        .With_CurrencyCode(currencyCode)
                        .With_CustomerID(location.CompanyID)
                        .SaveToDb();
                }

                return order;
            }
        }

        public Order CreateServiceOrder(DateTime serviceDate, string serviceType, GenericStatus status, Location location, Dictionary<Product, int> productDict)
        {
            return DataFacade.Order.New(serviceDate, location, serviceType)
                .With_OrderType(OrderType.AtRequest)
                .With_ServiceTypeCode(serviceType)
                .With_LocationCode(location.Code)
                .With_LocationID(location.ID)
                .With_GenericStatus(status)
                .With_ServiceDate(serviceDate)                
                .With_CustomerID(location.CompanyID)
                .With_Products(productDict)
                .SaveToDb();
        }

        /// <summary>
        /// Create 'deliver' service order with a single product position
        /// </summary>
        /// <param name="status">Generic status</param>
        /// <param name="location">Order location entity</param>
        /// <param name="product">Product entity</param>
        /// <param name="quantity">Product quantity</param>
        /// <param name="modelContext">EF context</param>
        /// <returns>Service order entity</returns>
        public Order CreateDeliverServiceOrder(DateTime serviceDate, GenericStatus status, Location location, Product product, int quantity)
        {
            var serviceType = "DELV";                                   
            return DataFacade.Order
                .New(serviceDate, location, serviceType)
                .With_OrderType(OrderType.AtRequest)
                .With_ServiceTypeCode(serviceType)
                .With_LocationCode(location.Code)
                .With_LocationID(location.ID)
                .With_GenericStatus(status)
                .With_ServiceDate(serviceDate)
                .With_CurrencyCode(product.Currency)
                .With_CustomerID(location.CompanyID)
                .With_Product(product.Type, product, quantity)
                .SaveToDb();
        }

        /// <summary>
        /// Create 'deliver' service order with multiple product positions
        /// </summary>
        /// <param name="status">Generic status</param>
        /// <param name="location">Order location entity</param>
        /// <param name="productCollection">Collection of products and quantities</param>
        /// <param name="orderCurrency">Order currency</param>
        /// <param name="modelContext">EF context</param>
        /// <returns>Service order entity</returns>
        public Order CreateDeliverServiceOrder(DateTime serviceDate, GenericStatus status, Location location, Dictionary<Product, int> productCollection, 
            string orderCurrency = "EUR")
        {
            var serviceType = "DELV";                        
            var order = DataFacade.Order
                .New(serviceDate, location, serviceType)
                .With_OrderType(OrderType.AtRequest)
                .With_ServiceTypeCode(serviceType)
                .With_LocationCode(location.Code)
                .With_LocationID(location.ID)
                .With_GenericStatus(status)
                .With_ServiceDate(serviceDate)
                .With_CurrencyCode(orderCurrency)
                .With_CustomerID(location.CompanyID)
                .With_Products(productCollection)
                .SaveToDb();

            return order;            
        }

        /// <summary>
        /// Create service product without products and services, and without returned value
        /// </summary>
        /// <param name="status"> Generic status of service order</param>
        /// <param name="serviceType"> Service type of service order</param>
        /// <param name="location"> Location of service order</param>
        public void SaveEmptyServiceOrder(DateTime serviceDate, GenericStatus status, string serviceType, Location location)
        {            
            var currencyCode = "EUR";            
            var order = DataFacade.Order
                .New(serviceDate, location, serviceType, out orderNumber)
                .With_OrderType(OrderType.AtRequest)
                .With_ServiceTypeCode(serviceType)
                .With_LocationCode(location.Code)
                .With_LocationID(location.ID)
                .With_GenericStatus(status)
                .With_ServiceDate(serviceDate)
                .With_CurrencyCode(currencyCode)
                .With_CustomerID(location.CompanyID)
                .SaveToDb();
        }

        /// <summary>
        /// Update existing service order
        /// </summary>
        /// <param name="serviceOrder"> Service order entity</param>
        /// <param name="withProducts"> Flag to update SOProducts</param>
        /// <param name="withServices"> Flag to update SOServices</param>
        public void UpdateServiceOrder(Order serviceOrder, bool withProducts = false, bool withServices = false, bool withBarcodedProducts = false)
        {            
            var quantity = 1;

            using (var context = new AutomationBaseDataContext())
            {
                if (withBarcodedProducts)
                {
                    var barcodedProductCode = context.Products.Where(p => p.Currency == "EUR" && p.Type == "NOTE" && p.IsBarcodedProduct).First().ProductCode;
                    var order = DataFacade.Order
                        .Take(o => o.WPOrderID == serviceOrder.WPOrderID)
                        .With_Product(quantity, DataFacade.Product.Take(barcodedProductCode))
                        .SaveToDb();
                    return;
                }

                // ATTENTION!!! On editing product codes below refer to "CreateServiceOrder" method 
                // to properly set "old" and "new" products         
                var productCode = context.Products.Where(p => p.Currency == "EUR" && p.Value == 10 && p.Type == "NOTE").First().ProductCode;
                var productCode2 = context.Products.Where(p => p.Currency == "EUR" && p.Value == 20 && p.Type == "NOTE").First().ProductCode;

                if (withProducts && !withServices)
                {                    
                    var order = DataFacade.Order
                        .Take(o => o.WPOrderID == serviceOrder.WPOrderID)
                        .With_Product(quantity, DataFacade.Product.Take(productCode), DataFacade.Product.Take(productCode2))
                        .SaveToDb();
                    return;
                }

                if (!withProducts && withServices)
                {                    
                    var order = DataFacade.Order
                        .Take(o => o.WPOrderID == serviceOrder.WPOrderID)
                        .With_Service("3", "4")
                        .SaveToDb();
                    return;
                }

                if (withProducts && withServices)
                {                    
                    var order = DataFacade.Order
                        .Take(o => o.WPOrderID == serviceOrder.WPOrderID)
                        .With_Product(quantity, DataFacade.Product.Take(productCode), DataFacade.Product.Take(productCode2))
                        .With_Service("3", "4")
                        .SaveToDb();
                    return;
                }
                else
                {
                    OrderBuilder order = DataFacade.Order.Take(o => o.WPOrderID == serviceOrder.WPOrderID).SaveToDb();
                    return;
                }
            }
        }

        /// <summary>
        /// Create new his_pack record and return it
        /// </summary>
        /// <param name="transportOrder">Transport order entity</param>
        /// <param name="arrivalTime">Arrival time attribute</param>
        /// <param name="status">His_pack status</param>
        /// <param name="onwardLocation">Onward location entity</param>
        /// <param name="collectLocation">Collect location entity</param>
        public PackageLifeCycle CreateHisPack(TransportOrder transportOrder, string arrivalTime, string status, int containerTypeID, int value = 0,
            Location onwardLocation = null, Location collectLocation = null, DateTime? date = null, string routeCode = null)
        {
            var hisPack = DailyDataFacade.HisPack.New()
                .With_Date(date ?? transportOrder.TransportDate)
                .With_Time(arrivalTime)
                .With_Status(status)
                .With_FrLocation(collectLocation)
                .With_ToLocation(onwardLocation)
                .With_BagType(containerTypeID)
                .With_PackNr($"{ transportOrder.Code }-{ DateTime.Now.ToString("ddMMyyyyhhmmssffff") }")
                .With_PackVal(value)
                .With_MasterRoute(routeCode ?? GetMasterRouteCode(transportOrder.TransportDate))
                .With_Site(transportOrder.SiteID)
                .With_OrderID(transportOrder.Code)
                .SaveToDb();

            return hisPack.Build();
        }

        /// <summary>
        /// Create new his_pack record and return it
        /// </summary>
        /// <param name="date">Date of visit</param>
        /// <param name="arrivalTime">Arrival time</param>
        /// <param name="status">His_pack status</param>
        /// <param name="onwardLocation">Onward location entity</param>
        /// <param name="collectLocation">Collect location entity</param>
        public PackageLifeCycle CreateHisPack(DateTime date, string arrivalTime, string status, int containerTypeID, int value = 0, 
            Location onwardLocation = null, Location collectLocation = null, string routeCode = null)
        {
            var hisPack = DailyDataFacade.HisPack.New()
                .With_Date(date)
                .With_Time(arrivalTime)
                .With_Status(status)
                .With_FrLocation(collectLocation)
                .With_ToLocation(onwardLocation)
                .With_BagType(containerTypeID)
                .With_PackNr($"3303-{ DateTime.Now.ToString("ddMMyyyyhhmmssffff") }")
                .With_PackVal(value)                                                
                .With_MasterRoute(routeCode ?? GetMasterRouteCode(date))
                .With_Site(DataFacade.Site.Take(s => s.BranchType == BranchType.CITDepot))
                //.With_OrderID(transportOrder.Code)
                .SaveToDb();

            return hisPack.Build();
        }

        /// <summary>
        /// Create new his_pack record without returning it
        /// </summary>
        /// <param name="transportOrder">Transport order entity</param>
        /// <param name="arrivalTime">Arrival time attribute</param>
        /// <param name="status">His_pack status</param>
        /// <param name="onwardLocation">Onward location entity</param>
        /// <param name="collectLocation">Collect location entity</param>
        public void SaveHisPack(TransportOrder transportOrder, string arrivalTime, string status, Location onwardLocation = null, Location collectLocation = null, string packageNumber = null)
        {
            var hisPack = DailyDataFacade.HisPack.New()
                .With_Date(transportOrder.TransportDate)
                .With_Time(arrivalTime)
                .With_Status(status)
                .With_FrLocation(collectLocation)
                .With_ToLocation(onwardLocation)                                                
                .With_BagType(3301)
                .With_PackNr(String.IsNullOrEmpty(packageNumber) ? $"{ transportOrder.Code }-{ new Random().Next(1, 9999) }" : packageNumber)
                .With_MasterRoute(GetMasterRouteCode(transportOrder.TransportDate))
                .With_Site(transportOrder.SiteID)
                .With_OrderID(transportOrder.Code)
                .SaveToDb();
        }

        /// <summary>
        /// Create new his_pack record without returning it
        /// </summary>
        /// <param name="date">Date of visit</param>
        /// <param name="arrivalTime">Arrival time</param>
        /// <param name="status">His_pack status</param>
        /// <param name="onwardLocation">Onward location entity</param>
        /// <param name="collectLocation">Collect location entity</param>
        public void SaveHisPack(DateTime date, string arrivalTime, string status, Location onwardLocation = null, Location collectLocation = null)
        {
            var hispack = DailyDataFacade.HisPack.New()
                .With_Date(date)
                .With_Time(arrivalTime)
                .With_Status(status)
                .With_FrLocation(collectLocation)
                .With_ToLocation(onwardLocation)
                .With_BagType(3301)
                .With_PackNr($"3303-{ date.ToString("ddMMyyyy") }-{ new Random().Next(1, 9999) }")
                .With_MasterRoute(GetMasterRouteCode(date))
                .With_Site(DataFacade.Site.Take(s => s.BranchType == BranchType.CITDepot))
                //.With_OrderID(transportOrder.Code)
                .SaveToDb();
        }
                
        /// <summary>
        /// Create new dailyStop record and return it
        /// </summary>
        /// <param name="transportOrder">Transport order</param>
        /// <param name="arrivalTime">Arrival time</param>
        /// <param name="location">Location</param>
        public DailyStop CreateDaiLine(TransportOrder transportOrder, Location location, string arrivalTime = null, string dTime = null)
        {
            if (transportOrder.MasterRouteDate != null)
            {
                var dailyStop = DailyDataFacade.DailyStop.New()
                    .With_MasterRoute(transportOrder.MasterRouteCode)
                    .With_DaiDate((DateTime)transportOrder.MasterRouteDate)
                    .With_ActualDaiDate(transportOrder.TransportDate)
                    .With_ArrivalTime(transportOrder.StopArrivalTime != null 
                        ? transportOrder.StopArrivalTime.ToString().Replace(":","").Substring(0,4) 
                        : arrivalTime.Substring(0, 4))
                    .With_ActualArrivalTime(arrivalTime)
                    .With_DepartureTime(!String.IsNullOrEmpty(dTime) ? dTime.Substring(0, 4) : null)
                    .With_ActualDepartureTime(dTime)
                    .With_Location(location)
                    .With_Site(transportOrder.SiteID)
                    .SaveToDb();

                return dailyStop.Build();
            }
            else
            {
                throw new NullReferenceException("Transport order -> MasterRouteDate should have a valid date! Dai_line record cannot be created with empty dai_date!");
            }
        }

        /// <summary>
        /// Create new dailyStop record and return it
        /// </summary>
        /// <param name="location">Location</param>
        /// <param name="date">Date</param>
        /// <param name="arrivalTime">Arrival time</param>
        public DailyStop CreateDaiLine(Location location, DateTime date, string arrivalTime, string dTime = null)
        {
            var dailyStop = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(GetMasterRouteCode(date))
                .With_DaiDate(date)
                .With_ActualDaiDate(date)
                .With_ArrivalTime(arrivalTime.Substring(0, 4))
                .With_ActualArrivalTime(arrivalTime)
                .With_DepartureTime(!String.IsNullOrEmpty(dTime) ? dTime.Substring(0, 4) : null)
                .With_ActualDepartureTime(dTime)
                .With_Location(location)
                .With_Site(DataFacade.Site.Take(s => s.ID == location.ServicingDepotID))
                .SaveToDb();

            return dailyStop.Build();
        }

        /// <summary>
        /// Save new dailyStop record without returning it
        /// </summary>
        /// <param name="transportOrder">Transport order</param>
        /// <param name="arrivalTime">Arrival time</param>
        /// <param name="location">Location</param>
        public void SaveDaiLine(TransportOrder transportOrder, Location location, string arrivalTime, string dTime = null)
        {
            if (transportOrder.MasterRouteDate != null)
            {
                var dailine = DailyDataFacade.DailyStop.New()
                    .With_MasterRoute(GetMasterRouteCode(transportOrder.TransportDate))
                    .With_DaiDate((DateTime)transportOrder.MasterRouteDate)
                    .With_ActualDaiDate(transportOrder.TransportDate)
                    .With_ArrivalTime(transportOrder.StopArrivalTime != null
                        ? transportOrder.StopArrivalTime.ToString().Replace(":", "").Substring(0, 4)
                        : arrivalTime.Substring(0, 4))
                    .With_ActualArrivalTime(arrivalTime)
                    .With_DepartureTime(!String.IsNullOrEmpty(dTime) ? dTime.Substring(0, 4) : null)
                    .With_ActualDepartureTime(dTime)
                    .With_Location(location)
                    .With_Site(transportOrder.SiteID)
                    .SaveToDb();
            }
            else
            {
                throw new NullReferenceException("Transport order -> MasterRouteDate should have a valid date! Dai_line record cannot be created with empty dai_date!");
            }
        }

        /// <summary>
        /// Save new dailyStop record without returning it
        /// </summary>
        /// <param name="location">Location</param>
        /// <param name="date">Date</param>
        /// <param name="arrivalTime">Arrival time</param>
        public void SaveDaiLine(Location location, DateTime date, string arrivalTime, string dTime = null)
        {
            var dailine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(GetMasterRouteCode(date))
                .With_DaiDate(date)
                .With_ActualDaiDate(date)
                .With_ArrivalTime(arrivalTime.Substring(0,4))
                .With_ActualArrivalTime(arrivalTime)
                .With_DepartureTime(!String.IsNullOrEmpty(dTime) ? dTime.Substring(0, 4) : null)
                .With_ActualDepartureTime(dTime)
                .With_Location(location)
                .With_Site(DataFacade.Site.Take(s => s.ID == location.ServicingDepotID))                                                    
                .SaveToDb();            
        }                

        public DailyStopProduct CreateDaiCoin(DailyStop stop, Cwc.BaseData.Product product, int deliverQuantity = 0, int collectQuantity = 0, string routeCode = null,
            decimal? locationID = null, DateTime? date = null, string arrivalTime = null)
        {
            if (stop.LocationNumber == null)
            {
                throw new KeyNotFoundException("Dai_line should have valid loc_nr attribute!");
            }                       
            
            var daiCoin = DailyDataFacade.DaiCoin.New()
                .With_MasterRoute(routeCode ?? stop.RouteNumber)
                .With_DaiDate(date ?? stop.Date)
                .With_Time(arrivalTime ?? stop.ActualArrivalTime ?? stop.ArrivalTime)
                .With_Location(locationID ?? (decimal)stop.LocationNumber)
                .With_Product(product)
                .With_AmountDel(deliverQuantity)
                .With_AmountCol(collectQuantity)
                .SaveToDb();

            return daiCoin.Build();
        }

        public DailyStopJob CreateDaiServicingCode(DailyStop stop, string code, string routeCode = null, DateTime? date = null, 
            string arrivalTime = null, string startTime = null, string endTime = null)
        {
            var daiServicingCode = DailyDataFacade.DaiServicindCode.New()
                .With_MasterRoute(routeCode ?? stop.RouteNumber)
                .With_DaiDate(date ?? stop.Date)
                .With_ActualDate(date ?? stop.Date)
                .With_ArrivalTime(arrivalTime ?? stop.ActualArrivalTime ?? stop.ArrivalTime)
                .With_StartTime(startTime)
                .With_EndTime(endTime)
                .With_ServCode(code)
                .With_Site(stop.SiteId)
                .SaveToDb();

            return daiServicingCode.Build();
        }               

        /// <summary>
        /// Find transport order with specified ServiceOrderID
        /// </summary>
        /// <param name="orderNumber"> Transport order → ServiceOrderID</param>
        /// <returns> Transport order</returns>
        public TransportOrder FindTransportOrder(string orderNumber)
        {
            using (var context = new AutomationTransportDataContext())
            {
                var transportOrder = context.TransportOrders.FirstOrDefault(t => t.ServiceOrderID == orderNumber);
                return transportOrder;
            }
        }

        /// <summary>
        /// Find transport order products linked to specified transport order
        /// </summary>
        /// <param name="transportOrderID"> id of transport order</param>
        /// <returns> List of transport order products</returns>
        public List<TransportOrderProduct> FindTransportOrderProducts(int transportOrderID)
        {
            using (var context = new AutomationTransportDataContext())
            {
                var transportOrderProducts = context.TransportOrderProducts.Where(p => p.TransportOrderID == transportOrderID).ToList();
                return transportOrderProducts;
            }
        }

        /// <summary>
        /// Find transport order services linked to specified transport order
        /// </summary>
        /// <param name="transportOrderID"></param>
        /// <returns> List of transport order services</returns>
        public List<TransportOrderServ> FindTransportOrderServices(int transportOrderID)
        {
            using (var context = new AutomationTransportDataContext())
            {
                var transportOrderServices = context.TransportOrderServs.Where(s => s.TransportOrderID == transportOrderID).ToList();
                return transportOrderServices;
            }
        }

        /// <summary>
        /// Find order CIT allocation log record for particular service order
        /// </summary>
        /// <param name="orderNumber"> Service order -> Order_ID attribute</param>
        /// <returns></returns>
        public OrderCitAllocationLog FindOrderCitAllocationLogRecord(string orderNumber)
        {
            using (var context = new AutomationTransportDataContext())
            {
                var logRecord = context.OrderCITAllocationLogs.Where(l => l.ServiceOrderID == orderNumber).OrderByDescending(l => l.DateCreated).FirstOrDefault();
                return logRecord;
            }
        }

        /// <summary>
        /// Edit service date for particular service order
        /// </summary>
        /// <param name="orderID"> Order_ID of service order</param>
        /// <param name="updatedServiceDate"> New service date</param>
        public void ChangeServiceDate(string orderID, DateTime updatedServiceDate)
        {
            using (var context = new AutomationOrderingDataContext())
            {
                var order = context.Orders.FirstOrDefault(o => o.ID == orderID);
                if (order != null)
                {
                    order.ServiceDate = updatedServiceDate;
                    context.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException($"Service order with ID = {orderID} is not found.");
                }
            }               
        }

        /// <summary>
        /// Edit order type for particular service order
        /// </summary>
        /// <param name="orderID"> Order_ID of service order</param>
        public void ChangeServiceOrderTypeToFixed(string orderID)
        {
            using (var context = new AutomationOrderingDataContext())
            {
                var order = context.Orders.FirstOrDefault(o => o.ID == orderID);
                if (order != null)
                {
                    order.OrderType = OrderType.Fixed;
                    context.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException($"Service order with ID = {orderID} is not found.");
                }
            }
        }

        public void ChangeTransportOrderTypeToFixed(int transportOrderId)
        {
            DataFacade.TransportOrder.Take(t => t.ID == transportOrderId).With_OrderType(OrderType.Fixed).SaveToDb();            
        }

        /// <summary>
        /// Edit generic status of particular service order
        /// </summary>
        /// <param name="serviceOrder"> Service order to edit</param>
        /// <param name="status"> New status</param>
        public void ChangeGenericStatus(Order serviceOrder, GenericStatus status)
        {
            serviceOrder.GenericStatus = status;
            serviceOrder.DateUpdated = DateTime.Now;
            serviceOrder.EditorID = SecurityFacade.LoginService.GetAdministratorLogin().UserID;
            var result = OrderingFacade.OrderService.SaveOrder(serviceOrder, null);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Order saving failed. Reason: { result.GetMessage() }");
            }            
        }

        public void CancelServiceOrder(Order serviceOrder, bool isCustomerResponsible = true)
        {
            using (var context = new AutomationBaseDataContext())
            {                
                var reason = isCustomerResponsible 
                    ? context.ReasonCodes.AsNoTracking().Where(r => r.Action == "N").FirstOrDefault()
                    : context.ReasonCodes.AsNoTracking().Where(r => r.Action != "N").FirstOrDefault();

                if (reason == null)
                {
                    reason = new ReasonCode { Action = "N", IsCustomerResponsible = isCustomerResponsible, ID = new Random().Next(50000, 59999), Description = "any" };
                    context.ReasonCodes.Add(reason);
                    context.SaveChanges();
                }
                serviceOrder.GenericStatus = GenericStatus.Cancelled;
                serviceOrder.ReasonCodeID = reason.ID;
                serviceOrder.CancelRemark = reason.Description;
                serviceOrder.DateUpdated = DateTime.Now;
                serviceOrder.EditorID = SecurityFacade.LoginService.GetAdministratorLogin().UserID;
                var result = OrderingFacade.OrderService.SaveOrder(serviceOrder, null);

                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Order saving failed. Reason: { result.GetMessage() }");
                }
            }
        }

        /// <summary>
        /// Edit status of transport order
        /// </summary>
        /// <param name="contextTransportOrder"> Context transport order entity to edit</param>
        /// <param name="status"> New status</param>
        public void ChangeTransportOrderStatus(TransportOrder transportOrder, TransportOrderStatus status, bool isDefineServiceOrderStatus = false)
        {
            DataFacade.TransportOrder.Take(t => t.ID == transportOrder.ID)
                .With_Status(status)
                .Update(isDefineServiceOrderStatus: isDefineServiceOrderStatus);
        }

        public void FillRouteDataForTransportOrder(TransportOrder transportOrder, TransportOrderStatus status, TimeSpan arrivalTime, bool isDefineServiceOrderStatus = false)
        {           
            DataFacade.TransportOrder.Take(t => t.ID == transportOrder.ID)
                .With_Status(status)
                .With_StopArrivalTime(arrivalTime)
                .With_MasterRouteCode(this.GetMasterRouteCode(transportOrder.TransportDate))
                .With_MasterRouteDate(transportOrder.TransportDate)
                .Update(isDefineServiceOrderStatus: isDefineServiceOrderStatus);            
        }

        /// <summary>
        /// Create second transport order for particular service order for testing negative scenario of "update" action of Order CIT Allocation job
        /// </summary>
        /// <param name="serviceOrder"> Service order entity</param>
        /// <param name="location"> Service order -> location entity</param>
        public void CreateSecondTransportOrder(Order serviceOrder, Location location)
        {
            DataFacade.TransportOrder.New()
                .With_Location(serviceOrder.LocationID)
                .With_Site(location.ServicingDepotID)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(serviceOrder.ServiceDate.AddDays(1))
                .With_ServiceDate(serviceOrder.ServiceDate.AddDays(1))
                .With_Status(TransportOrderStatus.Registered)
                .With_ServiceOrder(serviceOrder)
                .With_ServiceType(DataFacade.ServiceType.Take(s => s.Code == serviceOrder.ServiceTypeCode))
                .SaveToDb();
        }

        public string GetMasterRouteCode(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Monday)
            {
                return "901";
            }
            if (date.DayOfWeek == DayOfWeek.Tuesday)
            {
                return "902";
            }
            if (date.DayOfWeek == DayOfWeek.Wednesday)
            {
                return "903";
            }
            if (date.DayOfWeek == DayOfWeek.Thursday)
            {
                return "904";
            }
            if (date.DayOfWeek == DayOfWeek.Friday)
            {
                return "905";
            }
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                return "906";
            }
            else
            {
                return "907";
            }
        }        

        public void SaveCitProcessSettingLink(Location location, DataBaseParams dbParams)
        {
            var citProcessSettingID = BaseDataFacade.CitProcessSettingServicingTimeWindowAction.UpdateByLocation(location, dbParams);
            var action = BaseDataFacade.CitProcessSettingLinkService;
            if (action != null)
            {
                var result = action.CreateByLocationWithoutMatching(location, citProcessSettingID, dbParams);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException(string.Format("Error on creating CIT process setting link for location. Reason: {0}", result.GetMessage()));
                }
            }
            return;
        }

        /// <summary>
        /// Clear created transport data after each test
        /// </summary>
        public void ClearTestData()
        {
            using (var context = new AutomationTransportDataContext())
            {                
                context.OrderCITAllocationLogs.RemoveRange(context.OrderCITAllocationLogs);
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.[Cwc_Transport_TransportOrderCallLink]");
                context.TransportOrderProducts.RemoveRange(context.TransportOrderProducts);
                context.TransportOrderServs.RemoveRange(context.TransportOrderServs);
                context.SaveChanges();
                context.PackageLifeCycleProcessingJobLogs.RemoveRange(context.PackageLifeCycleProcessingJobLogs);                
                context.TransportOrders.RemoveRange(context.TransportOrders);                
                context.PackageLifeCycleProcessedItems.RemoveRange(context.PackageLifeCycleProcessedItems);
                context.SaveChanges();
                context.PackageLifeCycles.RemoveRange(context.PackageLifeCycles);
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.[dai_line]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.[dai_coin]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.[dai_servicingcode]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.[SOService]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.[SOProduct]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.[SOline]");
                context.Database.ExecuteSqlCommand("DELETE FROM dbo.[ServiceOrder]"); /* cannot use TRUNCATE on database referenced by foreign key */
                context.SaveChanges();
            }
        }
    }
}
