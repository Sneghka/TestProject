//#define runPerformance

using Xunit;
using CWC.AutoTests.Core;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Pages.Order;

namespace CWC.AutoTests.Tests
{    
    [Collection("Orders Collection #1")]
    public class OrderViewTest : BaseTest, IClassFixture<TestFixture>
    {
        TestFixture fixture;

        public void SetFixture(TestFixture setupClass)
        {
        }

        public OrderViewTest(TestFixture setupClass)
        {
            // Add code to be executed before every test
            this.fixture = setupClass;

            #if (runPerformance)
                PerformanceHelper.SaveStartTime();
            #endif
        }

        [Fact(DisplayName = "Open Orders Page")]
        public void OpenOrdersPage()
        {
            var ordersPage = fixture.HomePage.OpenPage<OrdersPage>();
            Assert.True(ordersPage.OrdersHeader.Displayed);
        }

        [Fact(DisplayName = "Open User Orders Page")]
        public void OpenUserOrdersPage()
        {
            var userOrdersPage = fixture.HomePage.OpenPage<UserOrdersPage>();
            Assert.True(userOrdersPage.UserOrdersHeader.Displayed);
        }

        [Fact(DisplayName = "Open Orders per Denominations Page")]
        public void OpenOrdersPerDenominationsPage()
        {
            var ordersPerDenominationPage = fixture.HomePage.OpenPage<OrdersPerDenominationPage>();
            Assert.True(ordersPerDenominationPage.OrdersPerDenominationHeader.Displayed);
        }
        /*
        [Fact(DisplayName = "Check Orders Page UI")]
        public void CheckOrdersPageUi()
        {
            var ordersPage = fixture.homePage.OpenPage<OrdersPage>();
            var viewHelper = new ViewHelper<OrdersPage>(ordersPage);
            viewHelper.CheckGridViewHeaderCss<OrdersPage>(ordersPage);
            viewHelper.CheckGridViewFilteringRow<OrdersPage>(ordersPage);
            viewHelper.CheckGridViewHeaderSorting<OrdersPage>(ordersPage);
        }

        [Fact(DisplayName = "Check User Orders Page UI")]
        public void CheckUserOrdersPageUi()
        {
            var userOrdersPage = fixture.homePage.OpenPage<UserOrdersPage>();
            var viewHelper = new ViewHelper<UserOrdersPage>(userOrdersPage);
            viewHelper.CheckGridViewHeaderCss<UserOrdersPage>(userOrdersPage);
            viewHelper.CheckGridViewFilteringRow<UserOrdersPage>(userOrdersPage);
            viewHelper.CheckGridViewHeaderSorting<UserOrdersPage>(userOrdersPage);
        }

        [Fact(DisplayName = "Check Orders per Denominations Page UI")]
        public void CheckOrdersPerDenominationsPageUi()
        {
            var ordersPerDenominationPage = fixture.homePage.OpenPage<OrdersPerDenominationPage>();
            var viewHelper = new ViewHelper<OrdersPerDenominationPage>(ordersPerDenominationPage);
            viewHelper.CheckGridViewHeaderCss<OrdersPerDenominationPage>(ordersPerDenominationPage);
            viewHelper.CheckGridViewFilteringRow<OrdersPerDenominationPage>(ordersPerDenominationPage);
            viewHelper.CheckGridViewHeaderSorting<OrdersPerDenominationPage>(ordersPerDenominationPage);
        }

        #if (runPerformance)
        [Fact(DisplayName = "Orders Page Navigation Performance")]
        public void OrdersPageNavigationPerformance()
        {
            var ordersPage = fixture.homePage.OpenPage<OrdersPage>();
            var viewHelper = new ViewHelper<OrdersPage>(ordersPage);
            viewHelper.NavigateViewPage(ordersPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(ordersPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Orders Page Filter by Date Performance")]
        public void OrdersPageFilterByDatePerformance()
        {
            var ordersPage = fixture.homePage.OpenPage<OrdersPage>();
            var viewHelper = new ViewHelper<OrdersPage>(ordersPage);
            viewHelper.ApplyFilterByDate<OrdersPage>(ordersPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(ordersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrdersPage>(ordersPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(ordersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrdersPage>(ordersPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(ordersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrdersPage>(ordersPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(ordersPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<OrdersPage>(ordersPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(ordersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrdersPage>(ordersPage, "ignoredate", null, null, null);
            PerformanceHelper.SaveFilterPerformance(ordersPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "User Orders Page Navigation Performance")]
        public void UserOrdersPageNavigationPerformance()
        {
            var userOrdersPage = fixture.homePage.OpenPage<UserOrdersPage>();
            var viewHelper = new ViewHelper<UserOrdersPage>(userOrdersPage);
            viewHelper.NavigateViewPage(userOrdersPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(userOrdersPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Orders per Denomination Page Navigation Performance")]
        public void OrdersPerDenominationPageNavigationPerformance()
        {
            var ordersPerDenominationPage = fixture.homePage.OpenPage<OrdersPerDenominationPage>();
            var viewHelper = new ViewHelper<OrdersPerDenominationPage>(ordersPerDenominationPage);
            viewHelper.NavigateViewPage(ordersPerDenominationPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(ordersPerDenominationPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Orders per Denomination Page Filter by Date Performance")]
        public void OrdersPerDenominationPageFilterByDatePerformance()
        {
            var ordersPerDenominationPage = fixture.homePage.OpenPage<OrdersPerDenominationPage>();
            var viewHelper = new ViewHelper<OrdersPerDenominationPage>(ordersPerDenominationPage);
            viewHelper.ApplyFilterByDate<OrdersPerDenominationPage>(ordersPerDenominationPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(ordersPerDenominationPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrdersPerDenominationPage>(ordersPerDenominationPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(ordersPerDenominationPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrdersPerDenominationPage>(ordersPerDenominationPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(ordersPerDenominationPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrdersPerDenominationPage>(ordersPerDenominationPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(ordersPerDenominationPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<OrdersPerDenominationPage>(ordersPerDenominationPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(ordersPerDenominationPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<OrdersPerDenominationPage>(ordersPerDenominationPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(ordersPerDenominationPage.GetType().Name, viewHelper.figure);
        }
        #endif*/
    }
}
