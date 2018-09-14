using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.CashCenter.InboundServices.ResourceInboundService;
using Cwc.Common;
using Cwc.Ordering;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace CWC.AutoTests.Tests.Inbound.DefineServiceOrderStatus
{
    public class DefineServiceOrderStatusTests
    {
        DataBaseParams dbParams;
        ResourceInboundService service;
        Location location;
        string containerNumber;
        StockLocation stockLocation;
        DateTime today = DateTime.Today;

        public DefineServiceOrderStatusTests()
        {
            dbParams = new DataBaseParams();
            service = new ResourceInboundService();
            location = DataFacade.Location.Take(x => x.Code == "SP02").Build();
            containerNumber = Utils.ValueGenerator.GenerateString("SPD", 12);
            stockLocation = CashCenterFacade.StockLocationService.LoadByNumber("SL124", dbParams);
        }

        [Fact(DisplayName = "When service order service type not {coll delv ser repl} Then System throws exception")]
        public void VerifyWhenServTypeNotInCollDelvSerReplTheSystemThrowsException()
        {
            var servOrder = DataFacade.Order.Take(x => x.ID != null);
            var countingServType = DataFacade.ServiceType.Take(x => x.OldType == "Counting");
            servOrder.With_ServiceTypeCode(countingServType.Build().Code);

            Assert.Throws(typeof(Exception), () => service.SyncDefineServiceOrderInboundStatus(servOrder, dbParams));
        }

        [Theory(DisplayName = "When order serv type is not coll or serv Then returned status is empty")]
        [InlineData("DELV")]
        [InlineData("SERV")]
        public void VerifyWhenServTypeNotInCollServThenReturnedStatusIsEmpty(string servType)
        {
            var servOrder = DataFacade.Order.Take(x => x.ID != null);
            servOrder.With_ServiceTypeCode(servType);

            var res = service.SyncDefineServiceOrderInboundStatus(servOrder, dbParams);

            Assert.Null(res);
        }

        [Theory(DisplayName = "When order serv type is coll or serv and appropriate stock container is not found Then returned status is empty")]
        [InlineData("COLL")]
        [InlineData("REPL")]
        public void VerifyWhenAppropriateStockContainerISNotExistThenDefinedStatusIsEmpty(string servType)
        {
            var servOrder = DataFacade.Order.Take(x => x.ID != null);
            servOrder.With_ServiceTypeCode(servType);

            var res = service.SyncDefineServiceOrderInboundStatus(servOrder, dbParams);

            Assert.Null(res);
        }

        [Fact(DisplayName = "When appropriate container is found and it's not counted Then defined status is InProgress")]
        public void VerifyWhenAppropriateContainerisFoundButNotCountedThenDefinedStatusIsInProgress()
        {
            using (new TransactionScope())
            {
                var locationCas = DataFacade.Location.Take(x => x.HandlingType == "CAS").Build();                
                var servOrder = DataFacade.Order.New(today, location, "COLL").SaveToDb();
                var stockcontainer = DataFacade.StockContainer.
                                                New().
                                                WithNumber(containerNumber).
                                                WithPreanType(null).
                                                WithType(StockContainerType.Deposit).
                                                WithStatus(SealbagStatus.Captured).
                                                WithLocationFrom(location.ID).
                                                WithLocationTo(locationCas.ID).
                                                WithDateCollected(today).
                                                WithStockLocation(stockLocation.ID).
                                                WithTotalValue(100).
                                                WithDateCreated(today).
                                                WithDateUpdated(today).SaveToDb();

                var res = service.SyncDefineServiceOrderInboundStatus(servOrder, dbParams);

                Assert.Equal(GenericStatus.InProgress, res.Value);
            }        
        }

        [Fact(DisplayName = "When appropriate container is found and it's counted Then defined status is Completed")]
        public void VerifyWhenAppropriateContainerIsCountedThenDefinedStatusIsCompleted()
        {
            using (new TransactionScope())
            {
                var locationCas = DataFacade.Location.Take(x => x.HandlingType == "CAS").Build();               
                var dateCollected = today.AddDays(1);
                var servOrder = DataFacade.Order.New(today, location, "COLL").SaveToDb();
                servOrder.With_NewServiceDate(dateCollected);
                var stockcontainer = DataFacade.StockContainer.
                                                New().
                                                WithNumber(containerNumber).
                                                WithPreanType(null).
                                                WithType(StockContainerType.Deposit).
                                                WithStatus(SealbagStatus.Counted).
                                                WithLocationFrom(location.ID).
                                                WithLocationTo(locationCas.ID).
                                                WithDateCollected(dateCollected).
                                                WithStockLocation(stockLocation.ID).
                                                WithTotalValue(100).
                                                WithDateCreated(today).WithDateUpdated(today).SaveToDb();

                var res = service.SyncDefineServiceOrderInboundStatus(servOrder, dbParams);

                Assert.Equal(GenericStatus.Completed, res.Value);
            }
        }

        [Fact(DisplayName = "When order line location has the same location as stock container Then System matches this container")]
        public void VerifyThatContainerCanBeMatchedByOrderLineLocation()
        {
            using (new TransactionScope())
            {
                var locationCas = DataFacade.Location.Take(x => x.HandlingType == "CAS").Build();
                var locaitonDiff = DataFacade.Location.Take(x => x.Code == "JG02").Build();                
                var servOrder = DataFacade.Order.New(today, location, "COLL").WithOrderLineLocation(locaitonDiff).SaveToDb();
                var stockcontainer = DataFacade.StockContainer.
                                                New().
                                                WithNumber(containerNumber).
                                                WithPreanType(null).
                                                WithType(StockContainerType.Deposit).
                                                WithStatus(SealbagStatus.Capturing).
                                                WithLocationFrom(locaitonDiff.ID).
                                                WithLocationTo(locationCas.ID).
                                                WithDateCollected(today).
                                                WithStockLocation(stockLocation.ID).
                                                WithTotalValue(100).
                                                WithDateCreated(today).WithDateUpdated(today).SaveToDb();
                var res = service.SyncDefineServiceOrderInboundStatus(servOrder, dbParams);

                Assert.Equal(GenericStatus.InProgress, res.Value);
            }
        }

        [Fact(DisplayName = "When Order -> New service date is empty Then System uses Service Date", Skip = "Should be clarified with developer why SetNewServiceDate takes as a param DateTime instead of nullable DateTime")]
        public void VerifyWhenNewServdateIsEmptyThenSystemUsesServiceDate()
        {
            using (new TransactionScope())
            {
                var locationCas = DataFacade.Location.Take(x => x.HandlingType == "CAS").Build();                
                var newDdate = today.AddDays(1);
                var servOrder = DataFacade.Order.New(today, location, "COLL").SaveToDb().Build();
                servOrder.ServiceDate = newDdate;

                {
                   
                }

                servOrder = DataFacade.Order.Take(x => x.ID == servOrder.ID);

                var stockcontainer = DataFacade.StockContainer.
                                                New().
                                                WithNumber(containerNumber).
                                                WithPreanType(null).
                                                WithType(StockContainerType.Deposit).
                                                WithStatus(SealbagStatus.Counted).
                                                WithLocationFrom(location.ID).
                                                WithLocationTo(locationCas.ID).
                                                WithDateCollected(newDdate).
                                                WithStockLocation(stockLocation.ID).
                                                WithTotalValue(100).
                                                WithDateCreated(today).WithDateUpdated(today).SaveToDb();

                var res = service.SyncDefineServiceOrderInboundStatus(servOrder, dbParams);

                Assert.Equal(GenericStatus.Completed, res.Value);
            }
        }

        [Fact(DisplayName = "When not all matched containers are counted Then System defines status as InProgress")]
        public void VerifyWhenNotAllContainersAreCountedThenDefinedStatusIsInProgress()
        {
            using (new TransactionScope())
            {
                var locationCas = DataFacade.Location.Take(x => x.HandlingType == "CAS").Build();               
                var servOrder = DataFacade.Order.New(today, location, "COLL").SaveToDb();
                var stockcontainer1 = DataFacade.StockContainer.
                                                New().
                                                WithNumber(containerNumber).
                                                WithPreanType(null).
                                                WithType(StockContainerType.Deposit).
                                                WithStatus(SealbagStatus.Captured).
                                                WithLocationFrom(location.ID).
                                                WithLocationTo(locationCas.ID).
                                                WithDateCollected(today).
                                                WithStockLocation(stockLocation.ID).
                                                WithTotalValue(100).
                                                WithDateCreated(today).WithDateUpdated(today).SaveToDb();

                var stockcontainer2 = DataFacade.StockContainer.
                                                New().
                                                WithNumber(containerNumber + "1").
                                                WithPreanType(null).
                                                WithType(StockContainerType.Deposit).
                                                WithStatus(SealbagStatus.Counted).
                                                WithLocationFrom(location.ID).
                                                WithLocationTo(locationCas.ID).
                                                WithDateCollected(today).
                                                WithStockLocation(stockLocation.ID).
                                                WithTotalValue(100).
                                                WithDateCreated(today).WithDateUpdated(today).SaveToDb();

                var res = service.SyncDefineServiceOrderInboundStatus(servOrder, dbParams);

                Assert.Equal(GenericStatus.InProgress, res.Value);
            }
        }

        [Fact(DisplayName = "When all matched containers are counted Then System defines status as Completed")]
        public void VerifyWhenAllContainersAreCountedThenDefinedStatusIsCompleted()
        {
            using (new TransactionScope())
            {
                var locationCas = DataFacade.Location.Take(x => x.HandlingType == "CAS").Build();                
                var servOrder = DataFacade.Order.New(today, location, "COLL").SaveToDb();
                var stockcontainer1 = DataFacade.StockContainer.
                                                New().
                                                WithNumber(containerNumber).
                                                WithPreanType(null).
                                                WithType(StockContainerType.Deposit).
                                                WithStatus(SealbagStatus.Counted).
                                                WithLocationFrom(location.ID).
                                                WithLocationTo(locationCas.ID).
                                                WithDateCollected(today).
                                                WithStockLocation(stockLocation.ID).
                                                WithTotalValue(100).
                                                WithDateCreated(today).WithDateUpdated(today).SaveToDb();

                var stockcontainer2 = DataFacade.StockContainer.
                                                New().
                                                WithNumber(containerNumber + "1").
                                                WithPreanType(null).
                                                WithType(StockContainerType.Deposit).
                                                WithStatus(SealbagStatus.Counted).
                                                WithLocationFrom(location.ID).
                                                WithLocationTo(locationCas.ID).
                                                WithDateCollected(today).
                                                WithStockLocation(stockLocation.ID).
                                                WithTotalValue(100).
                                                WithDateCreated(today).WithDateUpdated(today).SaveToDb();

                var res = service.SyncDefineServiceOrderInboundStatus(servOrder, dbParams);

                Assert.Equal(GenericStatus.Completed, res.Value);
            }
        }
    }
}
