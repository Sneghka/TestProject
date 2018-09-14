//#define runPerformance

using Xunit;
using CWC.AutoTests.Pages.CashPoint;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Core;

namespace CWC.AutoTests.Tests
{
    [Collection("Cash Point Collection #1")]
    public class CashPointModuleTests : BaseTest, IClassFixture<TestFixture>
    {
        TestFixture fixture;

        public void SetFixture(TestFixture setupClass)
        {
        }

        public CashPointModuleTests(TestFixture setupClass)
        {
            // Add code to be executed before every test
            this.fixture = setupClass;

            #if (runPerformance)
                PerformanceHelper.SaveStartTime();
            #endif
        }
        
        [Fact(DisplayName = "UI - Open Cash Point Monitor Page")]
        public void OpenCashPointMonitorPage()
        {
            var cashPointMonitorPage = fixture.HomePage.OpenPage<CashPointMonitorPage>();
            Assert.True(cashPointMonitorPage.TotalsHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Cash Availability Page")]
        public void OpenCashAvailabilityPage()
        {
            var cashAvailabilityPage = fixture.HomePage.OpenPage<CashAvailabilityPage>();
            Assert.True(cashAvailabilityPage.CashAvailabilityHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Cash Point Manage Page")]
        public void OpenCashPointManagePage()
        {
            var cashPointManagePage = fixture.HomePage.OpenPage<CashPointManagePage>();
            Assert.True(cashPointManagePage.ManagePageHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Cash Point Uptime Page")]
        public void OpenCashPointUptimePage()
        {
            var cashPointUptimePage = fixture.HomePage.OpenPage<CashPointUptimePage>();
            Assert.True(cashPointUptimePage.CashPointUptimeHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Collected and Issued Values Page")]
        public void OpenCollectedAndIssuedValuesPage()
        {
            var collectedAndIssuedValuesPage = fixture.HomePage.OpenPage<CollectedAndIssuedValuesPage>();
            Assert.True(collectedAndIssuedValuesPage.CollectedAndIssuedValuesHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Errors per Cash Point Page")]
        public void OpenErrorsPerCashPointPage()
        {
            var errorsPerCashPointPage = fixture.HomePage.OpenPage<ErrorsPerCashPointPage>();
            Assert.True(errorsPerCashPointPage.ErrorsPerCashPointHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Errors per Category Page")]
        public void OpenErrorsPerCategoryPage()
        {
            var errorsPerCategoryPage = fixture.HomePage.OpenPage<ErrorsPerCategoryPage>();
            Assert.True(errorsPerCategoryPage.ErrorsPerCategoryHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Event View Page")]
        public void OpenEventViewPage()
        {
            var eventsViewPage = fixture.HomePage.OpenPage<EventsViewPage>();
            Assert.True(eventsViewPage.EventsHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Residual Percentage Page")]
        public void OpenResidualPercentagePage()
        {
            var residualPercentagePage = fixture.HomePage.OpenPage<ResidualPercentagePage>();
            Assert.True(residualPercentagePage.ResidualPercentageHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Status Messages Page")]
        public void OpenStatusMessagesPage()
        {
            var statusMessagesPage = fixture.HomePage.OpenPage<StatusMessagesPage>();
            Assert.True(statusMessagesPage.StatusMessagesHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Stock Value Page")]
        public void OpenStockValuePage()
        {
            var stockValuePage = fixture.HomePage.OpenPage<StockValuePage>();
            Assert.True(stockValuePage.StockValueHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Transaction Details Page")]
        public void OpenTransactionDetailsPage()
        {
            var transactionDetailsPage = fixture.HomePage.OpenPage<TransactionDetailsPage>();
            Assert.True(transactionDetailsPage.TransactionDetailsHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Transactions Page")]
        public void OpenTransactionsPage()
        {
            var transactionsPage = fixture.HomePage.OpenPage<TransactionsPage>();
            Assert.True(transactionsPage.TransactionsHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Transactions per Cash Point Page")]
        public void OpenTransactionsPerCashPointPage()
        {
            var transactionsPerCashPointPage = fixture.HomePage.OpenPage<TransactionsPerCashPointPage>();
            Assert.True(transactionsPerCashPointPage.TransactionsPerCashPointHeader.Displayed);
        }
        /*
        [Fact(DisplayName = "Check Cash Point Monitor Page UI")]
        public void CheckCashPointMonitorPageUi()
        {
            var cashPointMonitorPage = fixture.homePage.OpenPage<CashPointMonitorPage>();
            var viewHelper = new ViewHelper<CashPointMonitorPage>(cashPointMonitorPage);
            viewHelper.CheckGridViewHeaderCss<CashPointMonitorPage>(cashPointMonitorPage);
            viewHelper.CheckGridViewFilteringRow<CashPointMonitorPage>(cashPointMonitorPage);
            viewHelper.CheckGridViewHeaderSorting<CashPointMonitorPage>(cashPointMonitorPage);
        }

        [Fact(DisplayName = "Check Cash Availability Page UI")]
        public void CheckCashAvailabilityPageUi()
        {
            var cashAvailabilityPage = fixture.homePage.OpenPage<CashAvailabilityPage>();
            var viewHelper = new ViewHelper<CashAvailabilityPage>(cashAvailabilityPage);
            viewHelper.CheckGridViewHeaderCss<CashAvailabilityPage>(cashAvailabilityPage);
            viewHelper.CheckGridViewFilteringRow<CashAvailabilityPage>(cashAvailabilityPage);
            viewHelper.CheckGridViewHeaderSorting<CashAvailabilityPage>(cashAvailabilityPage);
        }

        [Fact(DisplayName = "Check Cash Point Manage Page UI")]
        public void CheckCashPointManagePageUi()
        {
            var cashPointManagePage = fixture.homePage.OpenPage<CashPointManagePage>();
            var viewHelper = new ViewHelper<CashPointManagePage>(cashPointManagePage);
            viewHelper.CheckGridViewHeaderCss<CashPointManagePage>(cashPointManagePage);
            viewHelper.CheckGridViewFilteringRow<CashPointManagePage>(cashPointManagePage);
            viewHelper.CheckGridViewHeaderSorting<CashPointManagePage>(cashPointManagePage);
        }

        [Fact(DisplayName = "Check Cash Point Uptime Page UI")]
        public void CheckCashPointUptimePageUi()
        {
            var cashPointUptimePage = fixture.homePage.OpenPage<CashPointUptimePage>();
            var viewHelper = new ViewHelper<CashPointUptimePage>(cashPointUptimePage);
            viewHelper.CheckGridViewHeaderCss<CashPointUptimePage>(cashPointUptimePage);
            viewHelper.CheckGridViewFilteringRow<CashPointUptimePage>(cashPointUptimePage);
            viewHelper.CheckGridViewHeaderSorting<CashPointUptimePage>(cashPointUptimePage);
        }

        [Fact(DisplayName = "Check Collected and Issued Values Page UI")]
        public void CheckCollectedAndIssuedValuesPageUi()
        {
            var collectedAndIssuedValuesPage = fixture.homePage.OpenPage<CollectedAndIssuedValuesPage>();
            var viewHelper = new ViewHelper<CollectedAndIssuedValuesPage>(collectedAndIssuedValuesPage);
            viewHelper.CheckGridViewHeaderCss<CollectedAndIssuedValuesPage>(collectedAndIssuedValuesPage);
            viewHelper.CheckGridViewFilteringRow<CollectedAndIssuedValuesPage>(collectedAndIssuedValuesPage);
            viewHelper.CheckGridViewHeaderSorting<CollectedAndIssuedValuesPage>(collectedAndIssuedValuesPage);
        }

        [Fact(DisplayName = "Check Errors per Cash Point Page UI")]
        public void CheckErrorsPerCashPointPageUi()
        {
            var errorsPerCashPointPage = fixture.homePage.OpenPage<ErrorsPerCashPointPage>();
            var viewHelper = new ViewHelper<ErrorsPerCashPointPage>(errorsPerCashPointPage);
            viewHelper.CheckGridViewHeaderCss<ErrorsPerCashPointPage>(errorsPerCashPointPage);
            viewHelper.CheckGridViewFilteringRow<ErrorsPerCashPointPage>(errorsPerCashPointPage);
            viewHelper.CheckGridViewHeaderSorting<ErrorsPerCashPointPage>(errorsPerCashPointPage);
        }

        [Fact(DisplayName = "Check Errors per Category Page UI")]
        public void CheckErrorsPerCategoryPageUi()
        {
            var errorsPerCategoryPage = fixture.homePage.OpenPage<ErrorsPerCategoryPage>();
            var viewHelper = new ViewHelper<ErrorsPerCategoryPage>(errorsPerCategoryPage);
            viewHelper.CheckGridViewHeaderCss<ErrorsPerCategoryPage>(errorsPerCategoryPage);
            viewHelper.CheckGridViewFilteringRow<ErrorsPerCategoryPage>(errorsPerCategoryPage);
            viewHelper.CheckGridViewHeaderSorting<ErrorsPerCategoryPage>(errorsPerCategoryPage);
        }

        [Fact(DisplayName = "Check Event View Page UI")]
        public void CheckEventViewPageUi()
        {
            var eventsViewPage = fixture.homePage.OpenPage<EventsViewPage>();
            var viewHelper = new ViewHelper<EventsViewPage>(eventsViewPage);
            viewHelper.CheckGridViewHeaderCss<EventsViewPage>(eventsViewPage);
            viewHelper.CheckGridViewFilteringRow<EventsViewPage>(eventsViewPage);
            viewHelper.CheckGridViewHeaderSorting<EventsViewPage>(eventsViewPage);
        }

        [Fact(DisplayName = "Check Residual Percentage Page UI")]
        public void CheckResidualPercentagePageUi()
        {
            var residualPercentagePage = fixture.homePage.OpenPage<ResidualPercentagePage>();
            var viewHelper = new ViewHelper<ResidualPercentagePage>(residualPercentagePage);
            viewHelper.CheckGridViewHeaderCss<ResidualPercentagePage>(residualPercentagePage);
            viewHelper.CheckGridViewFilteringRow<ResidualPercentagePage>(residualPercentagePage);
            viewHelper.CheckGridViewHeaderSorting<ResidualPercentagePage>(residualPercentagePage);
        }

        [Fact(DisplayName = "Check Status Messages Page UI")]
        public void CheckStatusMessagesPageUi()
        {
            var statusMessagesPage = fixture.homePage.OpenPage<StatusMessagesPage>();
            var viewHelper = new ViewHelper<StatusMessagesPage>(statusMessagesPage);
            viewHelper.CheckGridViewHeaderCss<StatusMessagesPage>(statusMessagesPage);
            viewHelper.CheckGridViewFilteringRow<StatusMessagesPage>(statusMessagesPage);
            viewHelper.CheckGridViewHeaderSorting<StatusMessagesPage>(statusMessagesPage);
        }

        [Fact(DisplayName = "Check Stock Value Page UI")]
        public void CheckStockValuePageUi()
        {
            var stockValuePage = fixture.homePage.OpenPage<StockValuePage>();
            var viewHelper = new ViewHelper<StockValuePage>(stockValuePage);
            viewHelper.CheckGridViewHeaderCss<StockValuePage>(stockValuePage);
            viewHelper.CheckGridViewFilteringRow<StockValuePage>(stockValuePage);
            viewHelper.CheckGridViewHeaderSorting<StockValuePage>(stockValuePage);
        }

        [Fact(DisplayName = "Check Transaction Details Page UI")]
        public void CheckTransactionDetailsPageUi()
        {
            var transactionDetailsPage = fixture.homePage.OpenPage<TransactionDetailsPage>();
            var viewHelper = new ViewHelper<TransactionDetailsPage>(transactionDetailsPage);
            viewHelper.CheckGridViewHeaderCss<TransactionDetailsPage>(transactionDetailsPage);
            viewHelper.CheckGridViewFilteringRow<TransactionDetailsPage>(transactionDetailsPage);
            viewHelper.CheckGridViewHeaderSorting<TransactionDetailsPage>(transactionDetailsPage);
        }

        [Fact(DisplayName = "Check Transactions Page UI")]
        public void CheckTransactionsPageUi()
        {
            var transactionsPage = fixture.homePage.OpenPage<TransactionsPage>();
            var viewHelper = new ViewHelper<TransactionsPage>(transactionsPage);
            viewHelper.CheckGridViewHeaderCss<TransactionsPage>(transactionsPage);
            viewHelper.CheckGridViewFilteringRow<TransactionsPage>(transactionsPage);
            viewHelper.CheckGridViewHeaderSorting<TransactionsPage>(transactionsPage);
        }

        [Fact(DisplayName = "Check Transactions per Cash Point Page UI")]
        public void CheckTransactionsPerCashPointPageUi()
        {
            var transactionsPerCashPointPage = fixture.homePage.OpenPage<TransactionsPerCashPointPage>();
            var viewHelper = new ViewHelper<TransactionsPerCashPointPage>(transactionsPerCashPointPage);
            viewHelper.CheckGridViewHeaderCss<TransactionsPerCashPointPage>(transactionsPerCashPointPage);
            viewHelper.CheckGridViewFilteringRow<TransactionsPerCashPointPage>(transactionsPerCashPointPage);
            viewHelper.CheckGridViewHeaderSorting<TransactionsPerCashPointPage>(transactionsPerCashPointPage);
        }*/

        #if (runPerformance)
        
            [Fact(DisplayName = "Stock Value Page Filter by Date Performance")]
            public void StockValuePageFilterByDatePerformance()
            {
                var stockValuePage = fixture.homePage.OpenPage<StockValuePage>();
                var viewHelper = new ViewHelper<StockValuePage>(stockValuePage);
                viewHelper.ApplyFilterByDate<StockValuePage>(stockValuePage, "during", "last", "1", "days");
                PerformanceHelper.SaveFilterPerformance(stockValuePage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<StockValuePage>(stockValuePage, "during", "last", "1", "weeks");
                PerformanceHelper.SaveFilterPerformance(stockValuePage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<StockValuePage>(stockValuePage, "during", "last", "1", "months");
                PerformanceHelper.SaveFilterPerformance(stockValuePage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<StockValuePage>(stockValuePage, "during", "last", "3", "months");
                PerformanceHelper.SaveFilterPerformance(stockValuePage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<StockValuePage>(stockValuePage, "during", "last", "6", "months");
                PerformanceHelper.SaveFilterPerformance(stockValuePage.GetType().Name, viewHelper.figure);
            }
        
            [Fact(DisplayName = "Transactions Page Filter by Date Performance")]
            public void TransactionsPageFilterByDatePerformance()
            {
                var transactionsPage = fixture.homePage.OpenPage<TransactionsPage>();
                var viewHelper = new ViewHelper<TransactionsPage>(transactionsPage);
                viewHelper.ApplyFilterByDate<TransactionsPage>(transactionsPage, "during", "last", "1", "days");
                PerformanceHelper.SaveFilterPerformance(transactionsPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<TransactionsPage>(transactionsPage, "during", "last", "1", "weeks");
                PerformanceHelper.SaveFilterPerformance(transactionsPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<TransactionsPage>(transactionsPage, "during", "last", "1", "months");
                PerformanceHelper.SaveFilterPerformance(transactionsPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<TransactionsPage>(transactionsPage, "during", "last", "3", "months");
                PerformanceHelper.SaveFilterPerformance(transactionsPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<TransactionsPage>(transactionsPage, "during", "last", "6", "months");
                PerformanceHelper.SaveFilterPerformance(transactionsPage.GetType().Name, viewHelper.figure);
            }

            [Fact(DisplayName = "Transaction Details Page Filter by Date Performance")]
            public void TransactionDetailsPageFilterByDatePerformance()
            {
                var transactionDetailsPage = fixture.homePage.OpenPage<TransactionDetailsPage>();
                var viewHelper = new ViewHelper<TransactionDetailsPage>(transactionDetailsPage);
                viewHelper.ApplyFilterByDate<TransactionDetailsPage>(transactionDetailsPage, "during", "last", "1", "days");
                PerformanceHelper.SaveFilterPerformance(transactionDetailsPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<TransactionDetailsPage>(transactionDetailsPage, "during", "last", "1", "weeks");
                PerformanceHelper.SaveFilterPerformance(transactionDetailsPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<TransactionDetailsPage>(transactionDetailsPage, "during", "last", "1", "months");
                PerformanceHelper.SaveFilterPerformance(transactionDetailsPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<TransactionDetailsPage>(transactionDetailsPage, "during", "last", "3", "months");
                PerformanceHelper.SaveFilterPerformance(transactionDetailsPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<TransactionDetailsPage>(transactionDetailsPage, "during", "last", "6", "months");
                PerformanceHelper.SaveFilterPerformance(transactionDetailsPage.GetType().Name, viewHelper.figure);
            }

            [Fact(DisplayName = "Status Messages Page Filter by Date Performance")]
            public void StatusMessagesPageFilterByDatePerformance()
            {
                var statusMessagesPage = fixture.homePage.OpenPage<StatusMessagesPage>();
                var viewHelper = new ViewHelper<StatusMessagesPage>(statusMessagesPage);
                viewHelper.ApplyFilterByDate<StatusMessagesPage>(statusMessagesPage, "during", "last", "1", "days");
                PerformanceHelper.SaveFilterPerformance(statusMessagesPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<StatusMessagesPage>(statusMessagesPage, "during", "last", "1", "weeks");
                PerformanceHelper.SaveFilterPerformance(statusMessagesPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<StatusMessagesPage>(statusMessagesPage, "during", "last", "1", "months");
                PerformanceHelper.SaveFilterPerformance(statusMessagesPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<StatusMessagesPage>(statusMessagesPage, "during", "last", "3", "months");
                PerformanceHelper.SaveFilterPerformance(statusMessagesPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<StatusMessagesPage>(statusMessagesPage, "during", "last", "6", "months");
                PerformanceHelper.SaveFilterPerformance(statusMessagesPage.GetType().Name, viewHelper.figure);
            }

            [Fact(DisplayName = "Errors per Cash Point Page Filter by Date Performance")]
            public void ErrorsPerCashPointPageFilterByDatePerformance()
            {
                var errorsPerCashPointPage = fixture.homePage.OpenPage<ErrorsPerCashPointPage>();
                var viewHelper = new ViewHelper<ErrorsPerCashPointPage>(errorsPerCashPointPage);
                viewHelper.ApplyFilterByDate<ErrorsPerCashPointPage>(errorsPerCashPointPage, "during", "last", "1", "days");
                PerformanceHelper.SaveFilterPerformance(errorsPerCashPointPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<ErrorsPerCashPointPage>(errorsPerCashPointPage, "during", "last", "1", "weeks");
                PerformanceHelper.SaveFilterPerformance(errorsPerCashPointPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<ErrorsPerCashPointPage>(errorsPerCashPointPage, "during", "last", "1", "months");
                PerformanceHelper.SaveFilterPerformance(errorsPerCashPointPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<ErrorsPerCashPointPage>(errorsPerCashPointPage, "during", "last", "3", "months");
                PerformanceHelper.SaveFilterPerformance(errorsPerCashPointPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<ErrorsPerCashPointPage>(errorsPerCashPointPage, "during", "last", "6", "months");
                PerformanceHelper.SaveFilterPerformance(errorsPerCashPointPage.GetType().Name, viewHelper.figure);
            }

            [Fact(DisplayName = "Errors per Category Page Filter by Date Performance")]
            public void ErrorsPerCategoryPageFilterByDatePerformance()
            {
                var errorsPerCategoryPage = fixture.homePage.OpenPage<ErrorsPerCategoryPage>();
                var viewHelper = new ViewHelper<ErrorsPerCategoryPage>(errorsPerCategoryPage);
                viewHelper.ApplyFilterByDate<ErrorsPerCategoryPage>(errorsPerCategoryPage, "during", "last", "1", "days");
                PerformanceHelper.SaveFilterPerformance(errorsPerCategoryPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<ErrorsPerCategoryPage>(errorsPerCategoryPage, "during", "last", "1", "weeks");
                PerformanceHelper.SaveFilterPerformance(errorsPerCategoryPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<ErrorsPerCategoryPage>(errorsPerCategoryPage, "during", "last", "1", "months");
                PerformanceHelper.SaveFilterPerformance(errorsPerCategoryPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<ErrorsPerCategoryPage>(errorsPerCategoryPage, "during", "last", "3", "months");
                PerformanceHelper.SaveFilterPerformance(errorsPerCategoryPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<ErrorsPerCategoryPage>(errorsPerCategoryPage, "during", "last", "6", "months");
                PerformanceHelper.SaveFilterPerformance(errorsPerCategoryPage.GetType().Name, viewHelper.figure);
            }

            [Fact(DisplayName = "Transactions per Cash Point Page Filter by Date Performance")]
            public void TransactionsPerCashPointPageFilterByDatePerformance()
            {
                var transactionsPerCashPointPage = fixture.homePage.OpenPage<TransactionsPerCashPointPage>();
                var viewHelper = new ViewHelper<TransactionsPerCashPointPage>(transactionsPerCashPointPage);
                viewHelper.ApplyFilterByDate<TransactionsPerCashPointPage>(transactionsPerCashPointPage, "during", "last", "1", "days");
                PerformanceHelper.SaveFilterPerformance(transactionsPerCashPointPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<TransactionsPerCashPointPage>(transactionsPerCashPointPage, "during", "last", "1", "weeks");
                PerformanceHelper.SaveFilterPerformance(transactionsPerCashPointPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<TransactionsPerCashPointPage>(transactionsPerCashPointPage, "during", "last", "1", "months");
                PerformanceHelper.SaveFilterPerformance(transactionsPerCashPointPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<TransactionsPerCashPointPage>(transactionsPerCashPointPage, "during", "last", "3", "months");
                PerformanceHelper.SaveFilterPerformance(transactionsPerCashPointPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<TransactionsPerCashPointPage>(transactionsPerCashPointPage, "during", "last", "6", "months");
                PerformanceHelper.SaveFilterPerformance(transactionsPerCashPointPage.GetType().Name, viewHelper.figure);
            }

            [Fact(DisplayName = "Collected and Issued Values Page Filter by Date Performance")]
            public void CollectedAndIssuedValuesPageFilterByDatePerformance()
            {
                var collectedAndIssuedValuesPage = fixture.homePage.OpenPage<CollectedAndIssuedValuesPage>();
                var viewHelper = new ViewHelper<CollectedAndIssuedValuesPage>(collectedAndIssuedValuesPage);
                viewHelper.ApplyFilterByDate<CollectedAndIssuedValuesPage>(collectedAndIssuedValuesPage, "during", "last", "1", "days");
                PerformanceHelper.SaveFilterPerformance(collectedAndIssuedValuesPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<CollectedAndIssuedValuesPage>(collectedAndIssuedValuesPage, "during", "last", "1", "weeks");
                PerformanceHelper.SaveFilterPerformance(collectedAndIssuedValuesPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<CollectedAndIssuedValuesPage>(collectedAndIssuedValuesPage, "during", "last", "1", "months");
                PerformanceHelper.SaveFilterPerformance(collectedAndIssuedValuesPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<CollectedAndIssuedValuesPage>(collectedAndIssuedValuesPage, "during", "last", "3", "months");
                PerformanceHelper.SaveFilterPerformance(collectedAndIssuedValuesPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<CollectedAndIssuedValuesPage>(collectedAndIssuedValuesPage, "during", "last", "6", "months");
                PerformanceHelper.SaveFilterPerformance(collectedAndIssuedValuesPage.GetType().Name, viewHelper.figure);
            }

            [Fact(DisplayName = "Cash Point Uptime Page Filter by Date Performance")]
            public void CashPointUptimePageFilterByDatePerformance()
            {
                var cashPointUptimePage = fixture.homePage.OpenPage<CashPointUptimePage>();
                var viewHelper = new ViewHelper<CashPointUptimePage>(cashPointUptimePage);
                viewHelper.ApplyFilterByDate<CashPointUptimePage>(cashPointUptimePage, "during", "last", "1", "days");
                PerformanceHelper.SaveFilterPerformance(cashPointUptimePage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<CashPointUptimePage>(cashPointUptimePage, "during", "last", "1", "weeks");
                PerformanceHelper.SaveFilterPerformance(cashPointUptimePage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<CashPointUptimePage>(cashPointUptimePage, "during", "last", "1", "months");
                PerformanceHelper.SaveFilterPerformance(cashPointUptimePage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<CashPointUptimePage>(cashPointUptimePage, "during", "last", "3", "months");
                PerformanceHelper.SaveFilterPerformance(cashPointUptimePage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<CashPointUptimePage>(cashPointUptimePage, "during", "last", "6", "months");
                PerformanceHelper.SaveFilterPerformance(cashPointUptimePage.GetType().Name, viewHelper.figure);
            }

            [Fact(DisplayName = "Cash Availability Page Filter by Date Performance")]
            public void CashAvailabilityPageFilterByDatePerformance()
            {
                var cashAvailabilityPage = fixture.homePage.OpenPage<CashAvailabilityPage>();
                var viewHelper = new ViewHelper<CashAvailabilityPage>(cashAvailabilityPage);
                viewHelper.ApplyFilterByDate<CashAvailabilityPage>(cashAvailabilityPage, "during", "last", "1", "days");
                PerformanceHelper.SaveFilterPerformance(cashAvailabilityPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<CashAvailabilityPage>(cashAvailabilityPage, "during", "last", "1", "weeks");
                PerformanceHelper.SaveFilterPerformance(cashAvailabilityPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<CashAvailabilityPage>(cashAvailabilityPage, "during", "last", "1", "months");
                PerformanceHelper.SaveFilterPerformance(cashAvailabilityPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<CashAvailabilityPage>(cashAvailabilityPage, "during", "last", "3", "months");
                PerformanceHelper.SaveFilterPerformance(cashAvailabilityPage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<CashAvailabilityPage>(cashAvailabilityPage, "during", "last", "6", "months");
                PerformanceHelper.SaveFilterPerformance(cashAvailabilityPage.GetType().Name, viewHelper.figure);
            }

            [Fact(DisplayName = "Residual Percentage Page Filter by Date Performance")]
            public void ResidualPercentagePageFilterByDatePerformance()
            {
                var residualPercentagePage = fixture.homePage.OpenPage<ResidualPercentagePage>();
                var viewHelper = new ViewHelper<ResidualPercentagePage>(residualPercentagePage);
                viewHelper.ApplyFilterByDate<ResidualPercentagePage>(residualPercentagePage, "during", "last", "1", "days");
                PerformanceHelper.SaveFilterPerformance(residualPercentagePage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<ResidualPercentagePage>(residualPercentagePage, "during", "last", "1", "weeks");
                PerformanceHelper.SaveFilterPerformance(residualPercentagePage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<ResidualPercentagePage>(residualPercentagePage, "during", "last", "1", "months");
                PerformanceHelper.SaveFilterPerformance(residualPercentagePage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<ResidualPercentagePage>(residualPercentagePage, "during", "last", "3", "months");
                PerformanceHelper.SaveFilterPerformance(residualPercentagePage.GetType().Name, viewHelper.figure);
                viewHelper.ApplyFilterByDate<ResidualPercentagePage>(residualPercentagePage, "during", "last", "6", "months");
                PerformanceHelper.SaveFilterPerformance(residualPercentagePage.GetType().Name, viewHelper.figure);
            }
        
            [Fact(DisplayName = "Monitor Page Navigation Performance")]
            public void MonitorPageNavigationPerformance()
            {
                var cashPointMonitorPage = fixture.homePage.OpenPage<CashPointMonitorPage>();
                var viewHelper = new ViewHelper<CashPointMonitorPage>(cashPointMonitorPage);
                viewHelper.NavigateViewPage(cashPointMonitorPage);
                Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
                PerformanceHelper.SaveNavigationPerformance(cashPointMonitorPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
            }

            [Fact(DisplayName = "Cash Point Manage Page Navigation Performance")]
            public void CashPointManagePageNavigationPerformance()
            {
                var cashPointManagePage = fixture.homePage.OpenPage<CashPointManagePage>();
                var viewHelper = new ViewHelper<CashPointManagePage>(cashPointManagePage);
                viewHelper.NavigateViewPage(cashPointManagePage);
                Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
                PerformanceHelper.SaveNavigationPerformance(cashPointManagePage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
            }

            [Fact(DisplayName = "Stock Value Page Navigation Performance")]
            public void StockValuePageNavigationPerformance()
            {
                var stockValuePage = fixture.homePage.OpenPage<StockValuePage>();
                var viewHelper = new ViewHelper<StockValuePage>(stockValuePage);
                viewHelper.NavigateViewPage(stockValuePage);
                Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
                PerformanceHelper.SaveNavigationPerformance(stockValuePage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
            }

            [Fact(DisplayName = "Transactions Page Navigation Performance")]
            public void TransactionsPageNavigationPerformance()
            {
                var transactionsPage = fixture.homePage.OpenPage<TransactionsPage>();
                var viewHelper = new ViewHelper<TransactionsPage>(transactionsPage);
                viewHelper.NavigateViewPage(transactionsPage);
                Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
                PerformanceHelper.SaveNavigationPerformance(transactionsPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
            }

            [Fact(DisplayName = "Transaction Details Page Navigation Performance")]
            public void TransactionDetailsPageNavigationPerformance()
            {
                var transactionDetailsPage = fixture.homePage.OpenPage<TransactionDetailsPage>();
                var viewHelper = new ViewHelper<TransactionDetailsPage>(transactionDetailsPage);
                viewHelper.NavigateViewPage(transactionDetailsPage);
                Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
                PerformanceHelper.SaveNavigationPerformance(transactionDetailsPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
            }

            [Fact(DisplayName = "Status Messages Page Navigation Performance")]
            public void StatusMessagesPageNavigationPerformance()
            {
                var statusMessagesPage = fixture.homePage.OpenPage<StatusMessagesPage>();
                var viewHelper = new ViewHelper<StatusMessagesPage>(statusMessagesPage);
                viewHelper.NavigateViewPage(statusMessagesPage);
                Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
                PerformanceHelper.SaveNavigationPerformance(statusMessagesPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
            }

            [Fact(DisplayName = "Errors per Cash Point Page Navigation Performance")]
            public void ErrorsPerCashPointPageNavigationPerformance()
            {
                var errorsPerCashPointPage = fixture.homePage.OpenPage<ErrorsPerCashPointPage>();
                var viewHelper = new ViewHelper<ErrorsPerCashPointPage>(errorsPerCashPointPage);
                viewHelper.NavigateViewPage(errorsPerCashPointPage);
                Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
                PerformanceHelper.SaveNavigationPerformance(errorsPerCashPointPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
            }

            [Fact(DisplayName = "Transactions per Cash Point Page Navigation Performance")]
            public void TransactionsPerCashPointPageNavigationPerformance()
            {
                var transactionsPerCashPointPage = fixture.homePage.OpenPage<TransactionsPerCashPointPage>();
                var viewHelper = new ViewHelper<TransactionsPerCashPointPage>(transactionsPerCashPointPage);
                viewHelper.NavigateViewPage(transactionsPerCashPointPage);
                Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
                PerformanceHelper.SaveNavigationPerformance(transactionsPerCashPointPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
            }

            [Fact(DisplayName = "Collected and Issued Values Page Navigation Performance")]
            public void CollectedAndIssuedValuesPageNavigationPerformance()
            {
                var collectedAndIssuedValuesPage = fixture.homePage.OpenPage<CollectedAndIssuedValuesPage>();
                var viewHelper = new ViewHelper<CollectedAndIssuedValuesPage>(collectedAndIssuedValuesPage);
                viewHelper.NavigateViewPage(collectedAndIssuedValuesPage);
                Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
                PerformanceHelper.SaveNavigationPerformance(collectedAndIssuedValuesPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
            }

            [Fact(DisplayName = "Cash Point Uptime Page Navigation Performance")]
            public void CashPointUptimePageNavigationPerformance()
            {
                var cashPointUptimePage = fixture.homePage.OpenPage<CashPointUptimePage>();
                var viewHelper = new ViewHelper<CashPointUptimePage>(cashPointUptimePage);
                viewHelper.NavigateViewPage(cashPointUptimePage);
                Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
                PerformanceHelper.SaveNavigationPerformance(cashPointUptimePage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
            }

            [Fact(DisplayName = "Cash Availability Page Navigation Performance")]
            public void CashAvailabilityPageNavigationPerformance()
            {
                var cashAvailabilityPage = fixture.homePage.OpenPage<CashAvailabilityPage>();
                var viewHelper = new ViewHelper<CashAvailabilityPage>(cashAvailabilityPage);
                viewHelper.NavigateViewPage(cashAvailabilityPage);
                Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
                PerformanceHelper.SaveNavigationPerformance(cashAvailabilityPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
            }

            [Fact(DisplayName = "Residual Percentage Page Navigation Performance")]
            public void ResidualPercentagePageNavigationPerformance()
            {
                var residualPercentagePage = fixture.homePage.OpenPage<ResidualPercentagePage>();
                var viewHelper = new ViewHelper<ResidualPercentagePage>(residualPercentagePage);
                viewHelper.NavigateViewPage(residualPercentagePage);
                Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
                PerformanceHelper.SaveNavigationPerformance(residualPercentagePage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
            }

        #endif
    }
}
