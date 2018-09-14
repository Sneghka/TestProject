using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.CashCenter;
using Cwc.CashCenter.Fakes;
using Cwc.Common;
using Cwc.Contracts;
using CWC.AutoTests.ObjectBuilder;
using Microsoft.QualityTools.Testing.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CWC.AutoTests.Tests.AutoTests
{
    public class MatchStockContainerTests
    {
        #region DataInit
        StockContainer sc;
        StockContainerMatchingParams smp;
        string number, secondNumber, permanentNumber;
        StockLocation stockLocation;
        DateTime dateCollected;
        DateTime? transactionDate;
        Location location;
        DataBaseParams dbParams;     
        public MatchStockContainerTests()
        {
            ConfigurationKeySet.Load();
            dbParams = new DataBaseParams();
            sc = new StockContainer();
            number = GetNumber("Number");
            secondNumber = GetNumber("Second");
            permanentNumber = GetNumber("Permanent");
            location = DataFacade.Location.Take(x => x.Code == "SP01");
            dateCollected = new DateTime(2016, 7, 22, 15, 4, 10);
            stockLocation = CashCenterFacade.StockLocationService.Load(124, dbParams);
            smp = new StockContainerMatchingParams(number, null, null, null, null, stockLocation);
            transactionDate = CashCenterFacade.StockContainerService.GetCurrentTransactionDate(stockLocation.Site_id, dbParams, stockLocation.StockOwner_id).CurrentTransactionDate;       
        }
        #endregion

        [Theory(DisplayName = "When Customer / Transport prean is valid Then System should match it")]
        [InlineData(PreannouncementType.CustomerElectronic, ProcessName.Counting)]
        [InlineData(PreannouncementType.Transport, ProcessName.Counting)]
        [InlineData(PreannouncementType.CustomerElectronic, ProcessName.Capturing)]
        [InlineData(PreannouncementType.Transport, ProcessName.Capturing)]
        public void WhenPreanIsValidThenSystemMatchesIt(PreannouncementType type, ProcessName processName)
        {
            using (var shim = ShimsContext.Create())
            {
                ShimDepositProcessingService.AllInstances.MatchPreannouncementStockContainerMatchingParamsPreannouncementTypeDataBaseParams = (a, scp, pt, e) =>
                {
                    if (pt == type)
                    {
                        pt = type;
                        scp.Number = this.number;

                        var sc = new StockContainer();
                        sc.SetNumber(this.number);
                        sc.LocationFrom_id = location.ID;
                        sc.SetPreannouncementType(type);
                        sc.SetType(StockContainerType.Deposit);
                        sc.TotalValue = 1000;
                        sc.DateCollected = dateCollected;

                        return sc;
                    }

                    return null;
                };

                var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(this.smp, processName);

                var matchedSC = result.StockContainer;

                Assert.Equal(matchedSC.Number, this.number);
                Assert.Equal(matchedSC.LocationFrom_id, location.ID);
                Assert.Equal(matchedSC.TotalValue, 1000);
                Assert.Equal(matchedSC.DateCollected, dateCollected);
            }
        }

        [Theory(DisplayName = "When Customer / Transport prean is invalid Then System should create new Stock Container")]
        [InlineData(PreannouncementType.CustomerElectronic, ProcessName.Counting)]
        [InlineData(PreannouncementType.Transport, ProcessName.Counting)]
        [InlineData(PreannouncementType.CustomerElectronic, ProcessName.Capturing)]
        [InlineData(PreannouncementType.Transport, ProcessName.Capturing)]
        public void WhenPreanIsInvalidThenSystemShouldCreateNewStockContainer(PreannouncementType type, ProcessName processName)
        {
            using (var shimContext = ShimsContext.Create())
            {
                ShimDepositProcessingService.AllInstances.MatchPreannouncementStockContainerMatchingParamsPreannouncementTypeDataBaseParams = (a, scp, pt, e) =>
                {
                    if (pt == type)
                    {
                        pt = type;
                        scp.Number = this.number;

                        var sc = new StockContainer();
                        sc.SetNumber(this.number);
                        sc.LocationFrom_id = location.ID;
                        sc.SetPreannouncementType(type);
                        sc.SetType(StockContainerType.Deposit);
                        sc.TotalValue = 1000;
                        sc.DateCollected = dateCollected;

                        return sc;
                    }

                    return null;
                };

                ShimDepositProcessingService.AllInstances.ValidateFoundStockContainerStockContainerMatchingParamsStockContainerDataBaseParams = (s, scp, sc, dp) =>
                {
                    return false;
                };

                var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(this.smp, processName);

                var matchedSC = result.StockContainer;

                Assert.Equal(matchedSC.Number, this.number);
                Assert.Null(matchedSC.LocationFrom_id);
                Assert.Equal(matchedSC.TotalValue, 0);
            }
        }

        [Theory(DisplayName = "When physical Stock Containers are found Then System marks prean as invalid")]
        [InlineData(PreannouncementType.CustomerElectronic)]
        [InlineData(PreannouncementType.Transport)]
        public void WhenPreanExistsAndExistsPhysicalStockCOntainerTehnPreanIsInvalid(PreannouncementType type)
        {
            DataFacade.StockContainer.
                    New().
                    WithNumber(number).
                    WithPreanType(null).
                    WithType(StockContainerType.Deposit).
                    WithStatus(SealbagStatus.Received).
                    WithLocationFrom(location.ID).
                    WithDateCollected(dateCollected).
                    WithStockLocation(stockLocation.ID).
                    WithTotalValue(100)
                    .SaveToDb();

            sc = DataFacade.StockContainer.Take(sc => sc.Number == number && sc.PreannouncementType == null);

            sc.SetPreannouncementType(type);

            var matchpreanMethod = typeof(DepositProcessingService)
                                    .GetMethod("ValidateFoundStockContainer",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var validationResult = (bool)matchpreanMethod.Invoke(new DepositProcessingService(), new object[] { smp, sc, dbParams });

            Assert.False(validationResult);
        }

        [Theory(DisplayName = "When physical Stock Containers are not found Then System marks prean as valid")]
        [InlineData(PreannouncementType.CustomerElectronic)]
        [InlineData(PreannouncementType.Transport)]
        public void WhenPreanExistsAndDoesntExistPhysicalStockCOntainerTehnPreanIsVvalid(PreannouncementType type)
        {
            DataFacade.StockContainer.
                New().
                WithNumber(number).
                WithPreanType(type).
                WithType(StockContainerType.Deposit).
                WithStatus(SealbagStatus.Received).
                WithLocationFrom(location.ID).
                WithDateCollected(dateCollected).
                WithStockLocation(stockLocation.ID).
                WithTotalValue(100)
                .SaveToDb();

            sc = DataFacade.StockContainer.Take(sc => sc.Number == number && sc.PreannouncementType == type);

            var validpreanMethod = typeof(DepositProcessingService)
                                    .GetMethod("ValidateFoundStockContainer",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var validationResult = (bool)validpreanMethod.Invoke(new DepositProcessingService(), new object[] { smp, sc, dbParams });

            Assert.True(validationResult);
        }

        [Theory(DisplayName = "When more than one preans are found Then System takes with Max Date Collected, Max Date Created")]
        [InlineData(PreannouncementType.CustomerElectronic)]
        [InlineData(PreannouncementType.Transport)]
        public void WhenMoreThanOneValidPreansAreFoundSystemTakesWithMaxDate(PreannouncementType type)
        {
            DataFacade.StockContainer.
                New().
                WithNumber(number).
                WithPreanType(type).
                WithType(StockContainerType.Deposit).
                WithStatus(SealbagStatus.Received).
                WithLocationFrom(location.ID).
                WithDateCollected(dateCollected).
                WithStockLocation(stockLocation.ID).
                WithTotalValue(100).
                WithDateCreated(dateCollected).WithDateUpdated(dateCollected).SaveToDb();

            DataFacade.StockContainer.
                New().
                WithNumber(secondNumber).
                WithPreanType(type).
                WithType(StockContainerType.Deposit).
                WithStatus(SealbagStatus.Received).
                WithLocationFrom(location.ID).
                WithDateCollected(dateCollected.AddMinutes(10)).
                WithStockLocation(stockLocation.ID).
                WithSecondNumber(number).
                WithTotalValue(200).
                WithDateCreated(dateCollected.AddMinutes(10)).WithDateUpdated(dateCollected.AddMinutes(20)).SaveToDb();

            DataFacade.StockContainer.
                New().
                WithNumber(permanentNumber).
                WithPreanType(type).
                WithType(StockContainerType.Deposit).
                WithStatus(SealbagStatus.Received).
                WithLocationFrom(location.ID).
                WithDateCollected(dateCollected.AddMinutes(10)).
                WithStockLocation(stockLocation.ID).
                WithPermanentNumber(number).
                WithTotalValue(300).
                WithDateCreated(dateCollected.AddMinutes(15)).WithDateUpdated(dateCollected.AddMinutes(15)).SaveToDb();

            var matchpreanMethod = typeof(DepositProcessingService)
                                    .GetMethod("MatchPreannouncement",
                                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var matchedPrean = matchpreanMethod.Invoke(new DepositProcessingService(), new object[] { smp, type, dbParams }) as StockContainer;

            Assert.True(matchedPrean.TotalValue == 300, string.Format("{0} prean doesn't matched by Date Collected -> Date Created", type.GetDescription()));

            
        }

        [Theory(DisplayName = "When Customer Prean Date Collected < Trasnport Prean Date Collected Then System Uses Transport Prean")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        public void WhenCustomerPreanIsInvalidSystemUsesTransport(ProcessName processName)
        {
            DataFacade.StockContainer
                .New()
                .WithNumber(GetNumber("Number"))
                .WithPermanentNumber(number)
                .WithPreanType(PreannouncementType.CustomerElectronic)
                .WithType(StockContainerType.Deposit)
                .WithStatus(SealbagStatus.Received)
                .WithLocationFrom(location.ID)
                .WithStockLocation(stockLocation.ID)
                .WithDateCollected(dateCollected)
                .WithTotalValue(200).SaveToDb();

            DataFacade.StockContainer
                .New()
                .WithNumber(GetNumber("Number"))
                .WithPermanentNumber(number)
                .WithPreanType(PreannouncementType.Transport)
                .WithType(StockContainerType.Deposit)
                .WithStatus(SealbagStatus.Received)
                .WithLocationFrom(location.ID)
                .WithStockLocation(stockLocation.ID)
                .WithDateCollected(dateCollected.AddMinutes(20))
                .WithTotalValue(300).SaveToDb();

            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException("Matching result throws an error");
            }

            Assert.Equal(result.StockContainer.TotalValue, 300);
        }

        [Theory(DisplayName = "When Customer Prean Date Collected > Trasnport Prean Date Collected Then System Uses Transport Prean")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        public void WhenTransportPreanIsInvalidSystemUsesCustomer(ProcessName processName)
        {
            DataFacade.StockContainer
                .New()
                .WithNumber(GetNumber("Number"))
                .WithPermanentNumber(number)
                .WithPreanType(PreannouncementType.CustomerElectronic)
                .WithType(StockContainerType.Deposit)
                .WithStatus(SealbagStatus.Received)
                .WithLocationFrom(location.ID)
                .WithStockLocation(stockLocation.ID)
                .WithDateCollected(dateCollected)
                .WithTotalValue(200).SaveToDb();

            DataFacade.StockContainer
                .New()
                .WithNumber(GetNumber("Number"))
                .WithPermanentNumber(number)
                .WithPreanType(PreannouncementType.Transport)
                .WithType(StockContainerType.Deposit)
                .WithStatus(SealbagStatus.Received)
                .WithLocationFrom(location.ID)
                .WithStockLocation(stockLocation.ID)
                .WithDateCollected(dateCollected.AddMinutes(-20))
                .WithTotalValue(300).SaveToDb();

            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException("Matching result throws an error");
            }

            Assert.Equal(result.StockContainer.TotalValue, 200);
        }

        [Theory(DisplayName = "When Both Preans are invalid and only one physical is found Then System uses this Stock Container + change stock location")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        public void WhenBothPreansAreInvalidAndOnlyOnePhysicalFound(ProcessName processName)
        {
            //Used to check that Stock Location is updated when User is matching SC on diff locations on 1 Site
            var checkStockLocation = CashCenterFacade.StockLocationService.Load(60, dbParams);
            smp = new StockContainerMatchingParams(number, null, null, null, null, checkStockLocation);

            using (var shim = ShimsContext.Create())
            {     
                ShimDepositProcessingService.AllInstances.MatchPreannouncementStockContainerMatchingParamsPreannouncementTypeDataBaseParams = (a, scp, pt, e) =>
                {
                    scp.Number = this.number;
                    var sc = new StockContainer();
                    sc.SetNumber(this.number);
                    sc.LocationFrom_id = location.ID;
                    sc.SetType(StockContainerType.Deposit);
                    sc.TotalValue = 1000;
                    sc.DateCollected = dateCollected;

                    if (pt == PreannouncementType.CustomerElectronic)
                    {
                        pt = PreannouncementType.CustomerElectronic;
                        sc.SetPreannouncementType(PreannouncementType.CustomerElectronic);
                    }

                    else
                    {
                        pt = PreannouncementType.Transport;
                        sc.SetPreannouncementType(PreannouncementType.Transport);
                    }

                    return sc;
                };

                ShimDepositProcessingService.AllInstances.ValidateFoundStockContainerStockContainerMatchingParamsStockContainerDataBaseParams = (s, scp, sc, dp) =>
                {
                    return false;
                };

                sc = DataFacade.StockContainer
                .New()
                .WithNumber(number)
                .WithPreanType(null)
                .WithType(StockContainerType.Deposit)
                .WithStatus(SealbagStatus.Received)
                .WithLocationFrom(location.ID)
                .WithDateCollected(dateCollected)
                .WithTotalValue(200)
                .SaveToDb();

                var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

                Assert.Equal(result.StockContainer.TotalValue, 200);
                Assert.Equal(result.StockContainer.StockLocation_id, checkStockLocation.ID);
            }
        }

        [Theory(DisplayName = "When Both Preans are absent and only one physical is found Then System uses this Stock Container + change stock location")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        public void WhenBothPreansAreAbsentAndOnlyOnePhysicalFound(ProcessName processName)
        {
            var checkStockLocation = CashCenterFacade.StockLocationService.Load(60, dbParams);
            smp = new StockContainerMatchingParams(number, null, null, null, null, checkStockLocation);

            DataFacade.StockContainer
                .New()
                .WithNumber(number)
                .WithPreanType(null)
                .WithType(StockContainerType.Deposit)
                .WithStatus(SealbagStatus.Received)
                .WithLocationFrom(location.ID)
                .WithDateCollected(dateCollected)
                .WithTotalValue(300).SaveToDb();

            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException("Matching result throws an error");
            }

            Assert.Equal(result.StockContainer.TotalValue, 300);
            Assert.Equal(result.StockContainer.StockLocation_id, checkStockLocation.ID);
        }

        [Theory(DisplayName = "When more than one physical stock container with non empty transaction date is found Then System takes one with biggest Transaction Date")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        [InlineData(ProcessName.ExceptionManagement)]
        public void WhenMoreThanOnePhysicalByTransactionDate(ProcessName processName)
        {
            DataFacade.StockContainer
                .New()
                .WithNumber(number)
                .WithPreanType(null)
                .WithType(StockContainerType.Deposit)
                .WithLocationFrom(location.ID)
                .WithDateCollected(dateCollected)
                .WithTotalValue(300)
                .WithTransactiondate(transactionDate)
                .WithStatus(SealbagStatus.Received)
                .WithStockLocation(stockLocation.ID)
                .WithDateCreated(dateCollected.AddMinutes(-1)).WithDateUpdated(dateCollected).SaveToDb();

            DataFacade.StockContainer
                .New()
                .WithNumber(GetNumber("Number"))
                .WithPreanType(null)
                .WithType(StockContainerType.Deposit)
                .WithLocationFrom(location.ID)
                .WithDateCollected(dateCollected)
                .WithTotalValue(500)
                .WithTransactiondate(transactionDate.Value.AddDays(-1))
                .WithPermanentNumber(number)
                .WithStatus(SealbagStatus.Received)
                .WithStockLocation(stockLocation.ID)
                .WithDateCreated(dateCollected.AddMinutes(-2)).WithDateUpdated(dateCollected.AddHours(2)).SaveToDb();

            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.Equal(result.StockContainer.TotalValue, 300);
        }

        [Theory(DisplayName = "When more than one physical stock container with non empty transaction and equals date is found Then System takes one with biggest Date Updated")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        [InlineData(ProcessName.ExceptionManagement)]
        public void WhenMoreThanOnePhysicalByTransactionDateAndDateUpdated(ProcessName processName)
        {
            DataFacade.StockContainer
                .New()
                .WithNumber(number)
                .WithPreanType(null)
                .WithType(StockContainerType.Deposit)
                .WithLocationFrom(location.ID)
                .WithDateCollected(dateCollected)
                .WithTotalValue(300)
                .WithTransactiondate(transactionDate)
                .WithStatus(SealbagStatus.Received)
                .WithStockLocation(stockLocation.ID)
                .WithDateCreated(dateCollected.AddMinutes(-1)).WithDateUpdated(dateCollected).SaveToDb();

            DataFacade.StockContainer
                .New()
                .WithNumber(GetNumber("Number"))
                .WithPreanType(null)
                .WithType(StockContainerType.Deposit)
                .WithLocationFrom(location.ID)
                .WithDateCollected(dateCollected)
                .WithTotalValue(500)
                .WithTransactiondate(transactionDate)
                .WithPermanentNumber(number)
                .WithStatus(SealbagStatus.Received)
                .WithStockLocation(stockLocation.ID)
                .WithDateCreated(dateCollected.AddMinutes(-2)).WithDateUpdated(dateCollected.AddHours(2)).SaveToDb();

            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.Equal(result.StockContainer.TotalValue, 500);
        }

        [Theory(DisplayName = "When more than one physical stock container with empty transaction date is found Then System takes one with biggest Date Updated")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        [InlineData(ProcessName.ExceptionManagement)]
        public void WhenMoreThanOnePhysicalByMaxDateUpdated(ProcessName processName)
        {
            DataFacade.StockContainer
                   .New()
                   .WithNumber(number)
                   .WithPreanType(null)
                   .WithType(StockContainerType.Deposit)
                   .WithLocationFrom(location.ID)
                   .WithDateCollected(dateCollected)
                   .WithTotalValue(300)
                   .WithTransactiondate(null)
                   .WithStatus(SealbagStatus.Received)
                   .WithStockLocation(stockLocation.ID)
                   .WithDateCreated(dateCollected.AddMinutes(-1)).WithDateUpdated(dateCollected).SaveToDb();

            DataFacade.StockContainer
                .New()
                .WithNumber(GetNumber("Number"))
                .WithPreanType(null)
                .WithType(StockContainerType.Deposit)
                .WithLocationFrom(location.ID)
                .WithDateCollected(dateCollected)
                .WithTotalValue(500)
                .WithTransactiondate(null)
                .WithPermanentNumber(number)
                .WithStatus(SealbagStatus.Received)
                .WithStockLocation(stockLocation.ID)
                .WithDateCreated(dateCollected.AddMinutes(-2)).WithDateUpdated(dateCollected.AddHours(2)).SaveToDb();

            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.Equal(result.StockContainer.TotalValue, 500);

        }

        [Theory(DisplayName = "When more than one physical stock container with non empty past transaction date is found and any Permanemt Number = inputted Number Then System throws error message")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        [InlineData(ProcessName.ExceptionManagement)]
        public void WhenMoreThanOneWithPastTransactionDateAndPermanentNumber(ProcessName processName)
        {
            DataFacade.StockContainer
                   .New()
                   .WithNumber(GetNumber("Number"))
                   .WithPreanType(null)
                   .WithType(StockContainerType.Deposit)
                   .WithLocationFrom(location.ID)
                   .WithDateCollected(dateCollected)
                   .WithTotalValue(300)
                   .WithTransactiondate(transactionDate.Value.AddDays(-2))
                   .WithStatus(SealbagStatus.Received)
                   .WithStockLocation(stockLocation.ID)
                   .WithPermanentNumber(number)
                   .WithDateCreated(dateCollected.AddMinutes(-1)).WithDateUpdated(dateCollected).SaveToDb();

            DataFacade.StockContainer
                .New()
                .WithNumber(GetNumber("Number"))
                .WithPreanType(null)
                .WithType(StockContainerType.Deposit)
                .WithLocationFrom(location.ID)
                .WithDateCollected(dateCollected)
                .WithTotalValue(600)
                .WithTransactiondate(transactionDate.Value.AddDays(-1))
                .WithPermanentNumber(number)
                .WithStatus(SealbagStatus.Received)
                .WithStockLocation(stockLocation.ID)
                .WithDateCreated(dateCollected.AddMinutes(-2)).WithDateUpdated(dateCollected.AddHours(-2)).SaveToDb();

            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.Equal(result.Messages[0], $"More than one container with permanent number ‘{number}’ for the previous transaction dates exists. Please use deposit’s number instead of permanent number for defining exact container for matching.");
        }

        [Theory(DisplayName = "When more than one physical stock container with non empty past transaction date is found Then System takes one with Max Transaction date")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        [InlineData(ProcessName.ExceptionManagement)]
        public void WhenMoreThanOneWithPastTransactionDateAndNoPermanentNumber(ProcessName processName)
        {
            DataFacade.StockContainer
                   .New()
                   .WithNumber(number)
                   .WithPreanType(null)
                   .WithType(StockContainerType.Deposit)
                   .WithLocationFrom(location.ID)
                   .WithDateCollected(dateCollected)
                   .WithTotalValue(300)
                   .WithTransactiondate(transactionDate.Value.AddDays(-2))
                   .WithStatus(SealbagStatus.Received)
                   .WithStockLocation(stockLocation.ID)
                   .WithDateCreated(dateCollected.AddMinutes(-1)).WithDateUpdated(dateCollected).SaveToDb();

            DataFacade.StockContainer
                .New()
                .WithNumber(GetNumber("Number"))
                .WithPreanType(null)
                .WithType(StockContainerType.Deposit)
                .WithLocationFrom(location.ID)
                .WithDateCollected(dateCollected)
                .WithTotalValue(600)
                .WithTransactiondate(transactionDate.Value.AddDays(-1))
                .WithSecondNumber(number)
                .WithStatus(SealbagStatus.Received)
                .WithStockLocation(stockLocation.ID)
                .WithDateCreated(dateCollected.AddMinutes(-2)).WithDateUpdated(dateCollected.AddHours(-2)).SaveToDb();

            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.Equal(result.StockContainer.TotalValue, 600);
        }

        [Theory(DisplayName = "When more than one physical stock container with non empty past equals transaction date is found Then System takes one with Max date Updated")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        [InlineData(ProcessName.ExceptionManagement)]
        public void WhenMoreThanOneWithPastTransactionDateAndNoPermanentNumberSameTransactionDate(ProcessName processName)
        {
            DataFacade.StockContainer
                   .New()
                   .WithNumber(number)
                   .WithPreanType(null)
                   .WithType(StockContainerType.Deposit)
                   .WithLocationFrom(location.ID)
                   .WithDateCollected(dateCollected)
                   .WithTotalValue(300)
                   .WithTransactiondate(transactionDate.Value.AddDays(-1))
                   .WithStatus(SealbagStatus.Received)
                   .WithStockLocation(stockLocation.ID)
                   .WithDateCreated(dateCollected.AddMinutes(-1)).WithDateUpdated(dateCollected.AddMinutes(1)).SaveToDb();

            DataFacade.StockContainer
                .New()
                .WithNumber(GetNumber("Number"))
                .WithPreanType(null)
                .WithType(StockContainerType.Deposit)
                .WithLocationFrom(location.ID)
                .WithDateCollected(dateCollected)
                .WithTotalValue(600)
                .WithTransactiondate(transactionDate.Value.AddDays(-1))
                .WithSecondNumber(number)
                .WithStatus(SealbagStatus.Received)
                .WithStockLocation(stockLocation.ID)
                .WithDateCreated(dateCollected).WithDateUpdated(dateCollected).SaveToDb();

            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.Equal(result.StockContainer.TotalValue, 300);
        }

        [Theory(DisplayName = "When Process Name = 'Exception Management' and no physical stock container is found")]
        [InlineData(ProcessName.ExceptionManagement)]
        public void WhenProcessNameIsExceptionManagementAndStockContainerNotExists(ProcessName processName)
        {
            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.Equal(result.Messages[0], $"Container with '{number}' number doesn’t exist.");
        }

        [Theory(DisplayName = "When more than one physical stock container with non empty past equals transaction date is found and Permanent Number = Asset Then System takes this Asset")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        public void WhenMoreThanOnePhysicalContainerWithPermanentEqualsAssetWithGivenNumberExistsSystemUsedIt(ProcessName processName)
        {

            DataFacade.StockContainer
                   .New()
                   .WithNumber(GetNumber("Number"))
                   .WithPreanType(null)
                   .WithType(StockContainerType.Deposit)
                   .WithLocationFrom(location.ID)
                   .WithDateCollected(dateCollected.AddDays(-1))
                   .WithTotalValue(300)
                   .WithTransactiondate(transactionDate.Value.AddDays(-2))
                   .WithStatus(SealbagStatus.Received)
                   .WithStockLocation(stockLocation.ID)
                   .WithPermanentNumber("PA2000")
                   .WithDateCreated(dateCollected.AddMinutes(-1)).WithDateUpdated(dateCollected).SaveToDb();

            DataFacade.StockContainer
                .New()
                .WithNumber(GetNumber("Number"))
                .WithPreanType(null)
                .WithType(StockContainerType.Deposit)
                .WithLocationFrom(location.ID)
                .WithDateCollected(dateCollected.AddDays(-2))
                .WithTotalValue(600)
                .WithTransactiondate(transactionDate.Value.AddDays(-1))
                .WithPermanentNumber("PA2000")
                .WithStatus(SealbagStatus.Received)
                .WithStockLocation(stockLocation.ID)
                .WithDateCreated(dateCollected.AddMinutes(-2)).WithDateUpdated(dateCollected.AddHours(-2)).SaveToDb();

            smp.Number = "PA2000";

            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.True(result.StockContainer.Number == $"PA2000{DateTime.Now.ToString("yyyyMMdd")}" && result.StockContainer.PermanentNumber == "PA2000");

            Assert.Equal(result.StockContainer.DateCollected, DateTime.Now.Date);
        }

        [Theory(DisplayName = "When one physical stock container with non empty past equals transaction date is found and Permanent Number = Asset Then System takes this Asset")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        public void WhenOnePhysicalContainerWithPermanentEqualsAssetWithGivenNumberExistsSystemUsedIt(ProcessName processName)
        {

            DataFacade.StockContainer
                   .New()
                   .WithNumber(number)
                   .WithPreanType(null)
                   .WithType(StockContainerType.Deposit)
                   .WithLocationFrom(location.ID)
                   .WithDateCollected(dateCollected.AddDays(-1))
                   .WithTotalValue(300)
                   .WithTransactiondate(transactionDate.Value.AddDays(-2))
                   .WithStatus(SealbagStatus.Received)
                   .WithStockLocation(stockLocation.ID)
                   .WithPermanentNumber("PA2000").SaveToDb();

            smp.Number = "PA2000";

            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.True(result.StockContainer.Number == $"PA2000{DateTime.Now.ToString("yyyyMMdd")}" && result.StockContainer.PermanentNumber == "PA2000");

            Assert.Equal(result.StockContainer.DateCollected, DateTime.Now.Date);
        }

        [Theory(DisplayName = "When no physical nor prean neither asset are found then System creates new Stock Container from Matching params")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        public void WhenNorPhysicalNorPreanNeitherAssetAreFound(ProcessName processName)
        {
            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.True(result.StockContainer.Number == smp.Number);
            Assert.Null(result.StockContainer.LocationFrom_id);
            Assert.False(result.StockContainer.IsExpected);
            Assert.True(result.StockContainer.StockLocation_id == stockLocation.ID);
        }

        [Theory(DisplayName = "When for inner container exists parent Then System uses it's Location From")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        public void WhenParentIsNotEmptyThenSystemUsesItsLocationFromForInner(ProcessName processName)
        {
            DataFacade.StockContainer
                   .New()
                   .WithNumber(number)
                   .WithPreanType(null)
                   .WithType(StockContainerType.Deposit)
                   .WithLocationFrom(location.ID)
                   .WithStatus(SealbagStatus.Received)
                   .WithStockLocation(stockLocation.ID).SaveToDb();

            sc = DataFacade.StockContainer.Take(sc => sc.Number == number && sc.PreannouncementType == null);
            var innerNumber = $"{number}_1";

            DataFacade.StockContainer
                   .New()
                   .WithNumber(innerNumber)
                   .WithType(StockContainerType.Deposit)
                   .WithParentContainer(sc)
                   .WithStatus(SealbagStatus.Received)
                   .WithStockLocation(stockLocation.ID).SaveToDb();

            smp.Number = innerNumber;
            smp.ParentStockContainer = sc;
            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.Equal(result.StockContainer.Number, innerNumber);
            Assert.Equal(result.StockContainer.LocationFrom_id, sc.LocationFrom_id);
        }

        [Theory(DisplayName = "When Site Settings contains prefix of matched Stock Container Then System uses this location")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        public void WhenPrefixExistsThenSystemMatchesLocation(ProcessName processName)
        {
            var settings = DataFacade.CashCenterSiteSetting.GetSettings(s => s.Branch_cd == "SP");
            var prefix = new SiteSettingContainerPrefixBuilder()
                .WithPrefix("SPD")
                .WithAutostart(false)
                .WithLocation(location.ID)
                .WithSiteSettings(settings.ID)
                .SaveToDb();

            smp.Number = GetNumber("SPD");
            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.True(result.StockContainer.LocationFrom_id == location.ID, "Location wasn't substituted");

        }

        [Theory(DisplayName = "When Site Settings contains prefix of matched Stock Container and Substituted Location is empty Then System set location = empty")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        public void WhenPrefixExistsAndLocationIsEmptyThenSystemMatchesEmptyLocation(ProcessName processName)
        {
            var settings = DataFacade.CashCenterSiteSetting.GetSettings(s => s.Branch_cd == "SP");
            var prefix = new SiteSettingContainerPrefixBuilder()
                .WithPrefix("USPD")
                .WithAutostart(false)
                .WithLocation(null)
                .WithSiteSettings(settings.ID)
                .SaveToDb();

            smp.Number = GetNumber("USPD");
            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.True(result.StockContainer.LocationFrom_id == null, "Location wasn't substituted");

        }

        [Theory(DisplayName = "When Proces Settings -> Validate Bank Account = 'no', Then System matches bank account")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        public void WhenisValidateBankAccountNoThenSystemMatchesItAutomaticaly(ProcessName processName)
        {
            var siteSettings = DataFacade.CashCenterSiteSetting.GetSettings(s => s.Branch_cd == "SP");
            var procesSettings = DataFacade.CashCenterProcessSetting.GetSettings(ps => ps.Code == location.Code)
                .With_IsValidateBankAccount(false)
                .With_IsValidateBankAccountCounting(false)
                .SaveToDb();

            var bankAccount = BaseDataFacade.MatchingService.MatchBankAccount(location);
            smp.LocationFromId = location.ID;
            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.True(result.StockContainer.BankAccountID == bankAccount.ID, "Bank Account -> ID is not equals");
            Assert.True(result.StockContainer.BankAccountNumber == bankAccount.Number, "Bank Account -> Number is not equals");
        }

        [Theory(DisplayName = "When Proces Settings -> Validate Bank Account = 'yes', Then System sets empty bank account")]
        [InlineData(ProcessName.Capturing)]
        [InlineData(ProcessName.Counting)]
        public void WhenisValidateBankAccountYesThenMatchedBankAccountIsEmpty(ProcessName processName)
        {
            var siteSettings = DataFacade.CashCenterSiteSetting.GetSettings(s => s.Branch_cd == "SP");
            var procesSettings = DataFacade.CashCenterProcessSetting.GetSettings(ps => ps.Code == location.Code)
                .With_IsValidateBankAccount(true)
                .With_IsValidateBankAccountCounting(true)
                .SaveToDb();

            smp.LocationFromId = location.ID;
            var result = CashCenterFacade.DepositProcessingService.MatchStockContainer(smp, processName, dbParams);

            Assert.True(result.StockContainer.BankAccountID == null, "Bank Account -> ID is not null");
            Assert.True(result.StockContainer.BankAccountNumber == null, "Bank Account -> Number is not null");
        }
                
        private string GetNumber(string Prefix)
        {
            return String.Format("{0}{1}", Prefix, DateTime.Now.Ticks);
        }
    }
}
