using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.CashCenter;
using Cwc.Common;
using Cwc.Constants;
using Cwc.Ordering;
using Cwc.Ordering.Interfaces;
using Cwc.Ordering.Services.Outbound;
using Cwc.Transport.Enums;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CWC.AutoTests.Tests.Outbound.DefineServiceOrderStatus
{
    public class DefineServiceOrderTests
    {
        string number;
        Location location;
        IResourceOutboundAction outboundAction;
        DataBaseParams dbParams;
        DateTime serviceDate = DateTime.Today;
        ServiceType serviceTypeDelv;
        ServiceType serviceTypePick;
        ServiceType serviceTypeRepl;
        Site site;
        public DefineServiceOrderTests()
        {
            number = $"SO3303{ DateTime.Now.ToString("ddMMyyyyhhmmss")}";
            location = DataFacade.Location.Take(x => x.Code == "SP02");
            outboundAction = OrderingFacade.ResourceOutboundSyncService.GetAction();
            dbParams = new DataBaseParams();
            serviceTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV");
            serviceTypeRepl = DataFacade.ServiceType.Take(x => x.Code == "REPL");
            serviceTypePick = DataFacade.ServiceType.Take(s => s.OldType == ServiceTypeConstants.PickAndPack).Build();            
            site = DataFacade.Site.Take(s => s.Branch_cd == "SP").Build();
        }

        [Fact(DisplayName = "When service type is not in {Coll, Delv, Serv, Repl} Then System doesn't process them")]
        public void VerifyThatOnlyCollDelvServReplTypesAreProcessed()
        {
            var pickandPack = DataFacade.ServiceType.Take(s => s.OldType == ServiceTypeConstants.PickAndPack).Build();
            var serviceOrder = DataFacade.Order.Take(x => x.ID != null);
            serviceOrder.Build().ServiceTypeCode = pickandPack.Code;

            Assert.Throws(typeof(Exception), () => outboundAction.SyncDefineServiceOrderOutboundStatus(serviceOrder, dbParams));
        }


        [Fact(DisplayName = "When doesn't exists pick and pack Stock Order Then System defines Service Order status as empty")]
        public void VerifyWhenDoesntExistsPickStockOrderThenSystemDefinesStatusEmpty()
        {

            var serviceOrder = DataFacade.Order.New(serviceDate, location, serviceTypeDelv.Code).SaveToDb().Build();

            var stockOrderFirst = DataFacade.StockOrder.New()
                            .With_Number(number)
                            .With_ServiceDate(DateTime.Now)
                            .With_TotalQuantity(0)
                            .With_TotalValue(0)
                            .With_TotalWeight(0)
                            .With_IsBilled(false)
                            .With_ServiceType_id(serviceTypeDelv.ID)
                            .With_Site_id(site.ID)
                            .With_CITDepotID(location.ServicingDepotID)
                            .With_LocationTo_id(location.ID)
                            .With_ServiceOrder_id(serviceOrder.ID)
                            .With_CustomerID(location.CompanyID)
                            .With_Status(StockOrderStatus.Registered)
                            .SaveToDb()
                            .Build();

            var definedStatus = outboundAction.SyncDefineServiceOrderOutboundStatus(serviceOrder, dbParams);
            Assert.Null(definedStatus);
        }

        [Theory(DisplayName = "When Service Order type not in {Delv, Repl} Then System returns empty Status")]
        [InlineData("COLL")]
        [InlineData("SERV")]
        public void VerifyThatNonDelvOrReplServiceOrdersReturnsEmptyStatus(string servType)
        {
            var loadedServiceType = DataFacade.ServiceType.Take(x => x.Code == servType).Build();
            var serviceOrder = DataFacade.Order.Take(x => x.ID != null).Build();

            serviceOrder.ServiceTypeCode = loadedServiceType.Code;

            var definedStatus = outboundAction.SyncDefineServiceOrderOutboundStatus(serviceOrder, dbParams);
            Assert.Null(definedStatus);
        }

        [Theory(DisplayName = "When all linked stock orders are deleted, cancelled Then System sets service order status as Cancelled")]
        [InlineData(StockOrderStatus.Cancelled, StockOrderStatus.Cancelled)]
        [InlineData(StockOrderStatus.Deleted, StockOrderStatus.Deleted)]
        [InlineData(StockOrderStatus.Cancelled, StockOrderStatus.Deleted)]
        public void VerifyWhenOutboundOrderAreCompletedOrDeletedThenSystemDefinesStatusAsCancelled(StockOrderStatus status1, StockOrderStatus status2)
        {
            var serviceOrder = DataFacade.Order.New(serviceDate, location, serviceTypeDelv.Code).SaveToDb().Build();

            var stockOrderFirst = DataFacade.StockOrder.New()
                            .With_Number(number)
                            .With_ServiceDate(DateTime.Now)
                            .With_TotalQuantity(0)
                            .With_TotalValue(0)
                            .With_TotalWeight(0)
                            .With_IsBilled(false)
                            .With_ServiceType_id(serviceTypePick.ID)
                            .With_Site_id(site.ID)
                            .With_CITDepotID(location.ServicingDepotID)
                            .With_LocationTo_id(location.ID)
                            .With_ServiceOrder_id(serviceOrder.ID)
                            .With_CustomerID(location.CompanyID)
                            .With_Status(status1)
                            .SaveToDb();

            var stockOrderSecond = DataFacade.StockOrder.New()
                            .With_Number(number+"1")
                            .With_ServiceDate(DateTime.Now)
                            .With_TotalQuantity(0)
                            .With_TotalValue(0)
                            .With_TotalWeight(0)
                            .With_IsBilled(false)
                            .With_ServiceType_id(serviceTypePick.ID)
                            .With_Site_id(site.ID)
                            .With_CITDepotID(location.ServicingDepotID)
                            .With_LocationTo_id(location.ID)
                            .With_ServiceOrder_id(serviceOrder.ID)
                            .With_CustomerID(location.CompanyID)
                            .With_Status(status2)
                            .SaveToDb();

            var definedStatus = outboundAction.SyncDefineServiceOrderOutboundStatus(serviceOrder, dbParams);

            Assert.True(definedStatus.HasValue);
            Assert.Equal(GenericStatus.Cancelled, definedStatus.Value);
        }

        [Theory(DisplayName = "When all linked stock orders are deleted, cancelled, completed Then System sets service order status as Completed")]
        [InlineData(StockOrderStatus.Cancelled, StockOrderStatus.Cancelled, StockOrderStatus.Completed)]
        [InlineData(StockOrderStatus.Deleted, StockOrderStatus.Deleted, StockOrderStatus.Completed)]
        [InlineData(StockOrderStatus.Cancelled, StockOrderStatus.Deleted, StockOrderStatus.Completed)]
        public void VerifyWhenAllLinkedStockOrdersAreCompletedCancelledDeletedThenSystemDefinesServiceOrderAsCompleted(StockOrderStatus status1, StockOrderStatus status2, StockOrderStatus status3)
        {
            var serviceOrder = DataFacade.Order.New(serviceDate, location, serviceTypeDelv.Code).SaveToDb().Build();

            var stockOrderFirst = DataFacade.StockOrder.New()
                            .With_Number(number)
                            .With_ServiceDate(DateTime.Now)
                            .With_TotalQuantity(0)
                            .With_TotalValue(0)
                            .With_TotalWeight(0)
                            .With_IsBilled(false)
                            .With_ServiceType_id(serviceTypePick.ID)
                            .With_Site_id(site.ID)
                            .With_CITDepotID(location.ServicingDepotID)
                            .With_LocationTo_id(location.ID)
                            .With_ServiceOrder_id(serviceOrder.ID)
                            .With_CustomerID(location.CompanyID)
                            .With_Status(status1)
                            .SaveToDb();

            var stockOrderSecond = DataFacade.StockOrder.New()
                            .With_Number(number + "1")
                            .With_ServiceDate(DateTime.Now)
                            .With_TotalQuantity(0)
                            .With_TotalValue(0)
                            .With_TotalWeight(0)
                            .With_IsBilled(false)
                            .With_ServiceType_id(serviceTypePick.ID)
                            .With_Site_id(site.ID)
                            .With_CITDepotID(location.ServicingDepotID)
                            .With_LocationTo_id(location.ID)
                            .With_ServiceOrder_id(serviceOrder.ID)
                            .With_CustomerID(location.CompanyID)
                            .With_Status(status2)
                            .SaveToDb();

            var stockOrderThird = DataFacade.StockOrder.New()
                           .With_Number(number + "2")
                           .With_ServiceDate(DateTime.Now)
                           .With_TotalQuantity(0)
                           .With_TotalValue(0)
                           .With_TotalWeight(0)
                           .With_IsBilled(false)
                           .With_ServiceType_id(serviceTypePick.ID)
                           .With_Site_id(site.ID)
                           .With_CITDepotID(location.ServicingDepotID)
                           .With_LocationTo_id(location.ID)
                           .With_ServiceOrder_id(serviceOrder.ID)
                           .With_CustomerID(location.CompanyID)
                           .With_Status(status3)
                           .SaveToDb();

            var definedStatus = outboundAction.SyncDefineServiceOrderOutboundStatus(serviceOrder, dbParams);

            Assert.True(definedStatus.HasValue);
            Assert.Equal(GenericStatus.Completed, definedStatus.Value);
        }

        [Theory(DisplayName = "When at least one linked stock order is InProgress Then System defines service order status as InProgress")]
        [InlineData(StockOrderStatus.Cancelled, StockOrderStatus.Cancelled, StockOrderStatus.Completed, StockOrderStatus.InProgress)]
        [InlineData(StockOrderStatus.Deleted, StockOrderStatus.Registered, StockOrderStatus.Completed, StockOrderStatus.InProgress)]
        [InlineData(StockOrderStatus.Cancelled, StockOrderStatus.Cancelled, StockOrderStatus.Cancelled, StockOrderStatus.InProgress)]
        public void VerifyWhenAtLeastOneLinkedStockOrderIsInProgressThenReturnedStatusIsInProgress(StockOrderStatus status1, StockOrderStatus status2, StockOrderStatus status3, StockOrderStatus status4)
        {            
            var serviceOrder = DataFacade.Order.New(serviceDate, location, serviceTypeRepl.Code).SaveToDb().Build();
            var stockOrderFirst = DataFacade.StockOrder.New()
                            .With_Number(number)
                            .With_ServiceDate(DateTime.Now)
                            .With_TotalQuantity(0)
                            .With_TotalValue(0)
                            .With_TotalWeight(0)
                            .With_IsBilled(false)
                            .With_ServiceType_id(serviceTypePick.ID)
                            .With_Site_id(site.ID)
                            .With_CITDepotID(location.ServicingDepotID)
                            .With_LocationTo_id(location.ID)
                            .With_ServiceOrder_id(serviceOrder.ID)
                            .With_CustomerID(location.CompanyID)
                            .With_Status(status1)
                            .SaveToDb();
            var stockOrderSecond = DataFacade.StockOrder.New()
                            .With_Number(number + "1")
                            .With_ServiceDate(DateTime.Now)
                            .With_TotalQuantity(0)
                            .With_TotalValue(0)
                            .With_TotalWeight(0)
                            .With_IsBilled(false)
                            .With_ServiceType_id(serviceTypePick.ID)
                            .With_Site_id(site.ID)
                            .With_CITDepotID(location.ServicingDepotID)
                            .With_LocationTo_id(location.ID)
                            .With_ServiceOrder_id(serviceOrder.ID)
                            .With_CustomerID(location.CompanyID)
                            .With_Status(status2)
                            .SaveToDb();
            var stockOrderThird = DataFacade.StockOrder.New()
                           .With_Number(number + "2")
                           .With_ServiceDate(DateTime.Now)
                           .With_TotalQuantity(0)
                           .With_TotalValue(0)
                           .With_TotalWeight(0)
                           .With_IsBilled(false)
                           .With_ServiceType_id(serviceTypePick.ID)
                           .With_Site_id(site.ID)
                           .With_CITDepotID(location.ServicingDepotID)
                           .With_LocationTo_id(location.ID)
                           .With_ServiceOrder_id(serviceOrder.ID)
                           .With_CustomerID(location.CompanyID)
                           .With_Status(status3)
                           .SaveToDb();
            var stockOrderFourth = DataFacade.StockOrder.New()
                           .With_Number(number + "3")
                           .With_ServiceDate(DateTime.Now)
                           .With_TotalQuantity(0)
                           .With_TotalValue(0)
                           .With_TotalWeight(0)
                           .With_IsBilled(false)
                           .With_ServiceType_id(serviceTypePick.ID)
                           .With_Site_id(site.ID)
                           .With_CITDepotID(location.ServicingDepotID)
                           .With_LocationTo_id(location.ID)
                           .With_ServiceOrder_id(serviceOrder.ID)
                           .With_CustomerID(location.CompanyID)
                           .With_Status(status4)
                           .SaveToDb();
            var definedStatus = outboundAction.SyncDefineServiceOrderOutboundStatus(serviceOrder, dbParams);

            Assert.True(definedStatus.HasValue);
            Assert.Equal(GenericStatus.InProgress, definedStatus.Value);
        }

        [Theory(DisplayName = "When at least one linked stock order is registered and there is no linked InProgress stock orders Then System defines Service Order status as InProgress")]
        [InlineData(StockOrderStatus.Cancelled, StockOrderStatus.Deleted, StockOrderStatus.Completed, StockOrderStatus.Registered)]
        [InlineData(StockOrderStatus.Deleted, StockOrderStatus.Registered, StockOrderStatus.Completed, StockOrderStatus.Registered)]
        [InlineData(StockOrderStatus.Cancelled, StockOrderStatus.Cancelled, StockOrderStatus.Cancelled, StockOrderStatus.Registered)]
        public void VerifyWhenAtLeastOneLinkedStockOrderIsRegisteredAndNoInproressThenSystemDefinesStatusAsRegistered(StockOrderStatus status1, StockOrderStatus status2, StockOrderStatus status3, StockOrderStatus status4)
        {           
            var serviceOrder = DataFacade.Order.New(serviceDate, location, serviceTypeRepl.Code).SaveToDb().Build();
            var stockOrderFirst = DataFacade.StockOrder.New()
                            .With_Number(number)
                            .With_ServiceDate(DateTime.Now)
                            .With_TotalQuantity(0)
                            .With_TotalValue(0)
                            .With_TotalWeight(0)
                            .With_IsBilled(false)
                            .With_ServiceType_id(serviceTypePick.ID)
                            .With_Site_id(site.ID)
                            .With_CITDepotID(location.ServicingDepotID)
                            .With_LocationTo_id(location.ID)
                            .With_ServiceOrder_id(serviceOrder.ID)
                            .With_CustomerID(location.CompanyID)
                            .With_Status(status1)
                            .SaveToDb();
            var stockOrderSecond = DataFacade.StockOrder.New()
                            .With_Number(number + "1")
                            .With_ServiceDate(DateTime.Now)
                            .With_TotalQuantity(0)
                            .With_TotalValue(0)
                            .With_TotalWeight(0)
                            .With_IsBilled(false)
                            .With_ServiceType_id(serviceTypePick.ID)
                            .With_Site_id(site.ID)
                            .With_CITDepotID(location.ServicingDepotID)
                            .With_LocationTo_id(location.ID)
                            .With_ServiceOrder_id(serviceOrder.ID)
                            .With_CustomerID(location.CompanyID)
                            .With_Status(status2)
                            .SaveToDb();
            var stockOrderThird = DataFacade.StockOrder.New()
                           .With_Number(number + "2")
                           .With_ServiceDate(DateTime.Now)
                           .With_TotalQuantity(0)
                           .With_TotalValue(0)
                           .With_TotalWeight(0)
                           .With_IsBilled(false)
                           .With_ServiceType_id(serviceTypePick.ID)
                           .With_Site_id(site.ID)
                           .With_CITDepotID(location.ServicingDepotID)
                           .With_LocationTo_id(location.ID)
                           .With_ServiceOrder_id(serviceOrder.ID)
                           .With_CustomerID(location.CompanyID)
                           .With_Status(status3)
                           .SaveToDb();
            var stockOrderFourth = DataFacade.StockOrder.New()
                           .With_Number(number + "3")
                           .With_ServiceDate(DateTime.Now)
                           .With_TotalQuantity(0)
                           .With_TotalValue(0)
                           .With_TotalWeight(0)
                           .With_IsBilled(false)
                           .With_ServiceType_id(serviceTypePick.ID)
                           .With_Site_id(site.ID)
                           .With_CITDepotID(location.ServicingDepotID)
                           .With_LocationTo_id(location.ID)
                           .With_ServiceOrder_id(serviceOrder.ID)
                           .With_CustomerID(location.CompanyID)
                           .With_Status(status4)
                           .SaveToDb();
            var definedStatus = outboundAction.SyncDefineServiceOrderOutboundStatus(serviceOrder, dbParams);

            Assert.True(definedStatus.HasValue);
            Assert.Equal(GenericStatus.InProgress, definedStatus.Value);
        }
    }
}
