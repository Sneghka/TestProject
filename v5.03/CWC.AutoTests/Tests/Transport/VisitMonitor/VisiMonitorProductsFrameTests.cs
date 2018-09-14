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
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.Transport.VisitMonitor
{
    [Collection("Transport Order Collection")]
    public class VisiMonitorProductsFrameTests : IDisposable
    {
        DataModel.ModelContext _context;
        string routeCode;
        DateTime today = DateTime.Today;
        TimeSpan arrival;
        ServiceType serviceTypeColl;
        ServiceType serviceTypeDelv;
        Product product;
        Location location;
        Site site;
        Cwc.Ordering.Order serviceOrder;
        DateTime defaultTransportOrderDate;
        DataModel.reason reason;

        public VisiMonitorProductsFrameTests()
        {
            _context = new DataModel.ModelContext();
            routeCode = "SP1001";           
            arrival = new TimeSpan(8, 0, 0);
            serviceTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL");
            serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV");
            location = DataFacade.Location.Take(l => l.Code == "SP02").Build();            
            site = DataFacade.Site.Take(s => s.ID == location.ServicingDepotID).Build();
            serviceOrder = DataFacade.Order.New(today, location, serviceTypeDelv.Code).SaveToDb();
            product = DataFacade.Product.Take(x => x.IsBarcodedProduct == false);
            reason = _context.reasons.First();
            
        }

        [Fact(DisplayName = "Products Dependent View - When transport orders contains products Then System shows them on dependent view")]
        public void VerifyThatSystemShowsProductsProperly()
        {
            var quantity = 1;
            defaultTransportOrderDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(location, serviceTypeDelv, OrderType.AtRequest);

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeDelv)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(defaultTransportOrderDate).With_ServiceDate(defaultTransportOrderDate)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var orderProduct = DataFacade.TransportOrderProduct.New()
                .With_TransportOrder(transportOrer)
                .With_Product(product)
                .With_OrderedQuantity(quantity)
                .With_OrderedValue(quantity * (int)product.Value)
                .With_CurrencyID("EUR")
                .With_DateCreated(today)
                .With_DateUpdated(today)
                .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = defaultTransportOrderDate, EndPeriod = defaultTransportOrderDate, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, TransportOrderCode = transportOrer.Build().Code };
            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = defaultTransportOrderDate };

            var query = TransportFacade.ScheduleMonitoringService.GetProductsQuery(settings, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);
            
            Assert.Equal(product.ProductCode, res[0][VisitMonitorProductHelper.Code]);
            Assert.Equal(product.Description, res[0][VisitMonitorProductHelper.Description]);
            Assert.Equal(quantity, res[0][VisitMonitorProductHelper.RequiredQuantity]);
            Assert.Equal(0, res[0][VisitMonitorProductHelper.CollectedQuantity]);
            Assert.Equal(0, res[0][VisitMonitorProductHelper.DeliveredQuantity]);
        }

        [Fact(DisplayName = "Products Dependent View - When dai coins are matched Then Sysytem shows them")]
        public void VerifyThatSystemShowsDaiProductsProperly()
        {
            var amountCol = 5;
            var amountDel = 3;

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeColl)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb().Build();

            var dailyLine = DailyDataFacade.DailyStop.New()
                .With_MasterRoute(routeCode)
                .With_DaiDate(today)
                .With_ArrivalTime(arrival.ToString("hhmm"))
                .With_Location(location).With_Site(site)
                .With_Reason(reason.ID)
                .With_ActualDaiDate(today)
                .With_ActualArrivalTime(arrival.ToString("hhmm"))
                .SaveToDb();

            var daiCoin = DailyDataFacade.DaiCoin.New()
                .With_DaiDate(today)
                .With_Time(arrival.ToString("hhmm"))
                .With_Product(product)
                .With_Location(location)
                .With_Site(site)
                .With_MasterRoute(routeCode)
                .With_AmountCol(amountCol)
                .With_AmountDel(amountDel)
                .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo, TransportOrderCode = transportOrer.Code };
            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today, DailyRouteCode = routeCode, DailyRouteDate = today, PlannedArrivalTime = arrival };

            var query = TransportFacade.ScheduleMonitoringService.GetProductsQuery(settings, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.True(res.Any(), "Query returns no result");
            Assert.Equal(product.ProductCode, res[0][VisitMonitorProductHelper.Code]);
            Assert.Equal(product.Description, res[0][VisitMonitorProductHelper.Description]);
            Assert.Equal(0, res[0][VisitMonitorProductHelper.RequiredQuantity]);
            Assert.Equal(amountCol, res[0][VisitMonitorProductHelper.CollectedQuantity]);
            Assert.Equal(amountDel, res[0][VisitMonitorProductHelper.DeliveredQuantity]);
        }

        [Theory(DisplayName = "Products Dependent View - When transport orders contains equals products Then System sums them")]
        [InlineData(1, 1)]
        [InlineData(3, 2)]
        [InlineData(1, 4)]
        public void VerifyThatSYstemSumsProductsByProductCode(int firstQty, int secondQty)
        {

            var transportOrerFirst = DataFacade.TransportOrder.New()
                .With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeDelv)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var transportOrerSecond = DataFacade.TransportOrder.New()
                .With_Location(location).With_Site(location.ServicingDepotID).With_ServiceType(serviceTypeColl)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(today).With_ServiceDate(today)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).With_MasterRouteCode(routeCode)
                .With_MasterRouteDate(today).With_StopArrivalTime(arrival).SaveToDb();

            var orderProductFirst = DataFacade.TransportOrderProduct.New()
                .With_TransportOrder(transportOrerFirst)
                .With_Product(product)
                .With_OrderedQuantity(firstQty)
                .With_OrderedValue(firstQty * (int)product.Value)
                .With_CurrencyID("EUR")
                .With_DateCreated(today)
                .With_DateUpdated(today)
                .SaveToDb();

            var orderProductSecond = DataFacade.TransportOrderProduct.New()
                .With_TransportOrder(transportOrerSecond)
                .With_Product(product)
                .With_OrderedQuantity(secondQty)
                .With_OrderedValue(secondQty * (int)product.Value)
                .With_CurrencyID("EUR")
                .With_DateCreated(today)
                .With_DateUpdated(today)
                .SaveToDb();

            var settings = new VisitMonitorsFilterSetting { LocationID = location.ID, StartPeriod = today, EndPeriod = today, OperationType = Cwc.Common.Data.OperationPeriodType.FromTo};
            var selectedRow = new VisitMonitorInnerFilterItem { LocationID = location.ID, SiteID = location.ServicingDepotID.Value, TransportDate = today };

            var query = TransportFacade.ScheduleMonitoringService.GetProductsQuery(settings, selectedRow);

            var res = HelperFacade.VisitMonitorHelper.GetResult(query);

            Assert.Equal(1, res.Count);
            Assert.Equal(product.ProductCode, res[0][VisitMonitorProductHelper.Code]);
            Assert.Equal(product.Description, res[0][VisitMonitorProductHelper.Description]);
            Assert.Equal(firstQty + secondQty, res[0][VisitMonitorProductHelper.RequiredQuantity]);
        }

        public void Dispose()
        {
            var ids = _context.Cwc_Transport_TransportOrders.Where(x=>x.MasterRouteCode == routeCode).Select(x => x.id).ToArray();

            _context.Cwc_Transport_TransportOrderProducts.RemoveRange(_context.Cwc_Transport_TransportOrderProducts.Where(top => ids.Contains(top.TransportOrderID)));
            _context.Cwc_Transport_TransportOrderServs.RemoveRange(_context.Cwc_Transport_TransportOrderServs.Where(top => ids.Contains(top.TransportOrderID)));
            _context.SaveChanges();

            _context.dai_coins.RemoveRange(_context.dai_coins.Where(p => p.mast_cd == routeCode));
            _context.Cwc_Transport_TransportOrders.RemoveRange(_context.Cwc_Transport_TransportOrders.Where(tr => ids.Contains(tr.id)));
            _context.dai_lines.RemoveRange(_context.dai_lines.Where(d => d.mast_cd.StartsWith("SP")));
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
