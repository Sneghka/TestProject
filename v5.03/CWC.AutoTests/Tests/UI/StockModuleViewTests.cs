//#define runPerformance

using Xunit;
using CWC.AutoTests.Core;

namespace CWC.AutoTests.Tests
{
    using CWC.AutoTests.Helpers;    
    using CWC.AutoTests.Pages.Stock;

    [Collection("Stock Module Collection #1")]
    public class StockViewTests : BaseTest, IClassFixture<TestFixture>
    {
        TestFixture fixture;

        public StockViewTests(TestFixture setupClass)
        {
            // Add code to be executed before every test
            this.fixture = setupClass;

            #if (runPerformance)
                PerformanceHelper.SaveStartTime();
            #endif
        }

        [Fact(DisplayName = "UI - Open Actual Stock Value View Page")]
        public void OpenActualStockValuePage()
        {
            var actualStockValuePage = fixture.HomePage.OpenPage<ActualStockValuePage>();
            Assert.True(actualStockValuePage.ActualStockValueHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Container Movements Page")]
        public void OpenContainerMovementsPage()
        {
            var containerMovementsPage = fixture.HomePage.OpenPage<ContainerMovementsPage>();
            Assert.True(containerMovementsPage.ContainerMovementsHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Stock Containers Page")]
        public void OpenStockContainersPage()
        {
            var stockContainersPage = fixture.HomePage.OpenPage<StockContainersPage>();
            Assert.True(stockContainersPage.StockContainersHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Stock Dashboard Page")]
        public void OpenStockDashboardPage()
        {
            var stockDashboardPage = fixture.HomePage.OpenPage<StockDashboardPage>();
            Assert.True(stockDashboardPage.StockDashboardHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Stock Transactions Page")]
        public void OpenStockTransactionsPage()
        {
            var stockTransactionsPage = fixture.HomePage.OpenPage<StockTransactionsPage>();
            Assert.True(stockTransactionsPage.StockTransactionsHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Stock Value History Page")]
        public void OpenStockValueHistoryPage()
        {
            var stockValueHistoryPage = fixture.HomePage.OpenPage<StockValueHistoryPage>();
            Assert.True(stockValueHistoryPage.StockValueHistoryHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Take into Storage Orders View Page")]
        public void OpenTakeIntoStorageOrdersViewPage()
        {
            var takeIntoStorageOrdersViewPage = fixture.HomePage.OpenPage<TakeIntoStorageOrdersViewPage>();
            Assert.True(takeIntoStorageOrdersViewPage.TakeIntoStorageOrdersViewHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Value Movements Page")]
        public void OpenValueMovementsPage()
        {
            var valueMovementsPage = fixture.HomePage.OpenPage<ValueMovementsPage>();
            Assert.True(valueMovementsPage.ValueMovementsHeader.Displayed);
        }
        /*       
        [Fact(DisplayName = "Check Actual Stock Value View Page UI")]
        public void CheckActualStockValuePageUi()
        {
            var actualStockValuePage = fixture.homePage.OpenPage<ActualStockValuePage>();
            var viewHelper = new ViewHelper<ActualStockValuePage>(actualStockValuePage);
            viewHelper.CheckGridViewHeaderCss<ActualStockValuePage>(actualStockValuePage);
            viewHelper.CheckGridViewFilteringRow<ActualStockValuePage>(actualStockValuePage);
            viewHelper.CheckGridViewHeaderSorting<ActualStockValuePage>(actualStockValuePage);
        }

        [Fact(DisplayName = "Check Container Movements Page UI")]
        public void CheckContainerMovementsPageUi()
        {
            var containerMovementsPage = fixture.homePage.OpenPage<ContainerMovementsPage>();
            var viewHelper = new ViewHelper<ContainerMovementsPage>(containerMovementsPage);
            viewHelper.CheckGridViewHeaderCss<ContainerMovementsPage>(containerMovementsPage);
            viewHelper.CheckGridViewFilteringRow<ContainerMovementsPage>(containerMovementsPage);
            viewHelper.CheckGridViewHeaderSorting<ContainerMovementsPage>(containerMovementsPage);
        }

        [Fact(DisplayName = "Check Stock Containers Page UI")]
        public void CheckStockContainersPageUi()
        {
            var stockContainersPage = fixture.homePage.OpenPage<StockContainersPage>();
            var viewHelper = new ViewHelper<StockContainersPage>(stockContainersPage);
            viewHelper.CheckGridViewHeaderCss<StockContainersPage>(stockContainersPage);
            viewHelper.CheckGridViewFilteringRow<StockContainersPage>(stockContainersPage);
            viewHelper.CheckGridViewHeaderSorting<StockContainersPage>(stockContainersPage);
        }

        [Fact(DisplayName = "Check Stock Dashboard Page UI")]
        public void CheckStockDashboardPageUi()
        {
            var stockDashboardPage = fixture.homePage.OpenPage<StockDashboardPage>();
            var viewHelper = new ViewHelper<StockDashboardPage>(stockDashboardPage);
            viewHelper.CheckGridViewHeaderCss<StockDashboardPage>(stockDashboardPage);
            viewHelper.CheckGridViewFilteringRow<StockDashboardPage>(stockDashboardPage);
            viewHelper.CheckGridViewHeaderSorting<StockDashboardPage>(stockDashboardPage);
        }

        [Fact(DisplayName = "Check Stock Transactions Page UI")]
        public void CheckStockTransactionsPageUi()
        {
            var stockTransactionsPage = fixture.homePage.OpenPage<StockTransactionsPage>();
            var viewHelper = new ViewHelper<StockTransactionsPage>(stockTransactionsPage);
            viewHelper.CheckGridViewHeaderCss<StockTransactionsPage>(stockTransactionsPage);
            viewHelper.CheckGridViewFilteringRow<StockTransactionsPage>(stockTransactionsPage);
            viewHelper.CheckGridViewHeaderSorting<StockTransactionsPage>(stockTransactionsPage);
        }

        [Fact(DisplayName = "Check Stock Value History Page UI")]
        public void CheckStockValueHistoryPageUi()
        {
            var stockValueHistoryPage = fixture.homePage.OpenPage<StockValueHistoryPage>();
            var viewHelper = new ViewHelper<StockValueHistoryPage>(stockValueHistoryPage);
            viewHelper.CheckGridViewHeaderCss<StockValueHistoryPage>(stockValueHistoryPage);
            viewHelper.CheckGridViewFilteringRow<StockValueHistoryPage>(stockValueHistoryPage);
            viewHelper.CheckGridViewHeaderSorting<StockValueHistoryPage>(stockValueHistoryPage);
        }

        [Fact(DisplayName = "Check Take into Storage Orders View Page UI")]
        public void CheckTakeIntoStorageOrdersViewPageUi()
        {
            var takeIntoStorageOrdersViewPage = fixture.homePage.OpenPage<TakeIntoStorageOrdersViewPage>();
            var viewHelper = new ViewHelper<TakeIntoStorageOrdersViewPage>(takeIntoStorageOrdersViewPage);
            viewHelper.CheckGridViewHeaderCss<TakeIntoStorageOrdersViewPage>(takeIntoStorageOrdersViewPage);
            viewHelper.CheckGridViewFilteringRow<TakeIntoStorageOrdersViewPage>(takeIntoStorageOrdersViewPage);
            viewHelper.CheckGridViewHeaderSorting<TakeIntoStorageOrdersViewPage>(takeIntoStorageOrdersViewPage);
        }

        [Fact(DisplayName = "Check Value Movements Page UI")]
        public void CheckValueMovementsPageUi()
        {
            var valueMovementsPage = fixture.homePage.OpenPage<ValueMovementsPage>();
            var viewHelper = new ViewHelper<ValueMovementsPage>(valueMovementsPage);
            viewHelper.CheckGridViewHeaderCss<ValueMovementsPage>(valueMovementsPage);
            viewHelper.CheckGridViewFilteringRow<ValueMovementsPage>(valueMovementsPage);
            viewHelper.CheckGridViewHeaderSorting<ValueMovementsPage>(valueMovementsPage);
        }*/

        #if (runPerformance)
        [Fact(DisplayName = "Actual Stock Value Page Navigation Performance")]
        public void ActualStockValuePageNavigationPerformance()
        {
            var actualStockValuePage = fixture.homePage.OpenPage<ActualStockValuePage>();
            var viewHelper = new ViewHelper<ActualStockValuePage>(actualStockValuePage);
            viewHelper.NavigateViewPage(actualStockValuePage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(actualStockValuePage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Container Movements Page Navigation Performance")]
        public void ContainerMovementsPageNavigationPerformance()
        {
            var containerMovementsPage = fixture.homePage.OpenPage<ContainerMovementsPage>();
            var viewHelper = new ViewHelper<ContainerMovementsPage>(containerMovementsPage);
            viewHelper.NavigateViewPage(containerMovementsPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(containerMovementsPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Container Movements Page Filter by Date Performance")]
        public void ContainerMovementsPageFilterByDatePerformance()
        {
            var containerMovementsPage = fixture.homePage.OpenPage<ContainerMovementsPage>();
            var viewHelper = new ViewHelper<ContainerMovementsPage>(containerMovementsPage);
            viewHelper.ApplyFilterByDate<ContainerMovementsPage>(containerMovementsPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(containerMovementsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ContainerMovementsPage>(containerMovementsPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(containerMovementsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ContainerMovementsPage>(containerMovementsPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(containerMovementsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ContainerMovementsPage>(containerMovementsPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(containerMovementsPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<ContainerMovementsPage>(containerMovementsPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(containerMovementsPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<ContainerMovementsPage>(containerMovementsPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(containerMovementsPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Stock Containers View Page Navigation Performance")]
        public void StockContainersPageNavigationPerformance()
        {
            var stockContainersPage = fixture.homePage.OpenPage<StockContainersPage>();
            var viewHelper = new ViewHelper<StockContainersPage>(stockContainersPage);
            viewHelper.NavigateViewPage(stockContainersPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(stockContainersPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Stock Containers View Page Filter by Date Performance")]
        public void StockContainersPageFilterByDatePerformance()
        {
            var stockContainersPage = fixture.homePage.OpenPage<StockContainersPage>();
            var viewHelper = new ViewHelper<StockContainersPage>(stockContainersPage);
            viewHelper.ApplyFilterByDate<StockContainersPage>(stockContainersPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(stockContainersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<StockContainersPage>(stockContainersPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(stockContainersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<StockContainersPage>(stockContainersPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(stockContainersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<StockContainersPage>(stockContainersPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(stockContainersPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<StockContainersPage>(stockContainersPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(stockContainersPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<StockContainersPage>(stockContainersPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(stockContainersPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Stock Transactions Page Navigation Performance")]
        public void StockTransactionsPageNavigationPerformance()
        {
            var stockTransactionsPage = fixture.homePage.OpenPage<StockTransactionsPage>();
            var viewHelper = new ViewHelper<StockTransactionsPage>(stockTransactionsPage);
            viewHelper.NavigateViewPage(stockTransactionsPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(stockTransactionsPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Stock Transactions Page Filter by Date Performance")]
        public void StockTransactionsPageFilterByDatePerformance()
        {
            var stockTransactionsPage = fixture.homePage.OpenPage<StockTransactionsPage>();
            var viewHelper = new ViewHelper<StockTransactionsPage>(stockTransactionsPage);
            viewHelper.ApplyFilterByDate<StockTransactionsPage>(stockTransactionsPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(stockTransactionsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<StockTransactionsPage>(stockTransactionsPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(stockTransactionsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<StockTransactionsPage>(stockTransactionsPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(stockTransactionsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<StockTransactionsPage>(stockTransactionsPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(stockTransactionsPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<StockTransactionsPage>(stockTransactionsPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(stockTransactionsPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<StockTransactionsPage>(stockTransactionsPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(stockTransactionsPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Stock Value History Page Navigation Performance")]
        public void StockValueHistoryPageNavigationPerformance()
        {
            var stockValueHistoryPage = fixture.homePage.OpenPage<StockValueHistoryPage>();
            var viewHelper = new ViewHelper<StockValueHistoryPage>(stockValueHistoryPage);
            viewHelper.NavigateViewPage(stockValueHistoryPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(stockValueHistoryPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Stock Value History Page Filter by Date Performance")]
        public void StockValueHistoryPageFilterByDatePerformance()
        {
            var stockValueHistoryPage = fixture.homePage.OpenPage<StockValueHistoryPage>();
            var viewHelper = new ViewHelper<StockValueHistoryPage>(stockValueHistoryPage);
            viewHelper.ApplyFilterByDate<StockValueHistoryPage>(stockValueHistoryPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(stockValueHistoryPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<StockValueHistoryPage>(stockValueHistoryPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(stockValueHistoryPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<StockValueHistoryPage>(stockValueHistoryPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(stockValueHistoryPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<StockValueHistoryPage>(stockValueHistoryPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(stockValueHistoryPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<StockValueHistoryPage>(stockValueHistoryPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(stockValueHistoryPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<StockValueHistoryPage>(stockValueHistoryPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(stockValueHistoryPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Take into Storage Orders View Page Navigation Performance")]
        public void TakeIntoStorageOrdersViewPageNavigationPerformance()
        {
            var takeIntoStorageOrdersViewPage = fixture.homePage.OpenPage<TakeIntoStorageOrdersViewPage>();
            var viewHelper = new ViewHelper<TakeIntoStorageOrdersViewPage>(takeIntoStorageOrdersViewPage);
            viewHelper.NavigateViewPage(takeIntoStorageOrdersViewPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(takeIntoStorageOrdersViewPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Take into Storage Orders View Page Filter by Date Performance")]
        public void TakeIntoStorageOrdersViewPageFilterByDatePerformance()
        {
            var takeIntoStorageOrdersViewPage = fixture.homePage.OpenPage<TakeIntoStorageOrdersViewPage>();
            var viewHelper = new ViewHelper<TakeIntoStorageOrdersViewPage>(takeIntoStorageOrdersViewPage);
            viewHelper.ApplyFilterByDate<TakeIntoStorageOrdersViewPage>(takeIntoStorageOrdersViewPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(takeIntoStorageOrdersViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<TakeIntoStorageOrdersViewPage>(takeIntoStorageOrdersViewPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(takeIntoStorageOrdersViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<TakeIntoStorageOrdersViewPage>(takeIntoStorageOrdersViewPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(takeIntoStorageOrdersViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<TakeIntoStorageOrdersViewPage>(takeIntoStorageOrdersViewPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(takeIntoStorageOrdersViewPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<TakeIntoStorageOrdersViewPage>(takeIntoStorageOrdersViewPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(takeIntoStorageOrdersViewPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<TakeIntoStorageOrdersViewPage>(takeIntoStorageOrdersViewPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(takeIntoStorageOrdersViewPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Value Movements Page Navigation Performance")]
        public void ValueMovementsPageNavigationPerformance()
        {
            var valueMovementsPage = fixture.homePage.OpenPage<ValueMovementsPage>();
            var viewHelper = new ViewHelper<ValueMovementsPage>(valueMovementsPage);
            viewHelper.NavigateViewPage(valueMovementsPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(valueMovementsPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Value Movements Page Filter by Date Performance")]
        public void ValueMovementsPageFilterByDatePerformance()
        {
            var valueMovementsPage = fixture.homePage.OpenPage<ValueMovementsPage>();
            var viewHelper = new ViewHelper<ValueMovementsPage>(valueMovementsPage);
            viewHelper.ApplyFilterByDate<ValueMovementsPage>(valueMovementsPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(valueMovementsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ValueMovementsPage>(valueMovementsPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(valueMovementsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ValueMovementsPage>(valueMovementsPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(valueMovementsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ValueMovementsPage>(valueMovementsPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(valueMovementsPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<ValueMovementsPage>(valueMovementsPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(valueMovementsPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<ValueMovementsPage>(valueMovementsPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(valueMovementsPage.GetType().Name, viewHelper.figure);
        }
        #endif
    }
}
