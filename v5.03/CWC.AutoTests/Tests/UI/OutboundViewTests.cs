//#define runPerformance

using Xunit;
using CWC.AutoTests.Core;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Pages.Outbound;

namespace CWC.AutoTests.Tests
{ 
    [Collection("Outbound Collection #1")]
    public class OutboundViewTests : BaseTest, IClassFixture<TestFixture>
    {
        TestFixture fixture;        

        public OutboundViewTests(TestFixture setupClass)
        {
            // Add code to be executed before every test
            this.fixture = setupClass;

            #if (runPerformance)
                PerformanceHelper.SaveStartTime();
            #endif
        }

        [Fact(DisplayName = "UI - Open Outbound Containers Page")]
        public void OpenOrdersPage()
        {
            var outboundContainersPage = fixture.HomePage.OpenPage<OutboundContainersPage>();
            Assert.True(outboundContainersPage.OutboundContainersHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Dispatch Orders View Page")]
        public void OpenDispatchOrdersViewPage()
        {
            var dispatchOrdersViewPage = fixture.HomePage.OpenPage<DispatchOrdersViewPage>();
            Assert.True(dispatchOrdersViewPage.DispatchOrdersViewHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Order Batches Page")]
        public void OpenOrdersPerDenominationsPage()
        {
            var orderBatchesPage = fixture.HomePage.OpenPage<OrderBatchesPage>();
            Assert.True(orderBatchesPage.OrderBatchesHeader.Displayed);
        }


        [Fact(DisplayName = "UI - Open Outbound Orders View Page")]
        public void OpenOutboundOrdersViewPage()
        {
            var outboundOrdersViewPage = fixture.HomePage.OpenPage<OutboundOrdersViewPage>();
            Assert.True(outboundOrdersViewPage.OutboundOrdersViewHeader.Displayed);
        }
        /*
        [Fact(DisplayName = "Check Containers In Packing Page UI")]
        public void CheckOrdersPageUi()
        {
            var containersInPackingPage = fixture.homePage.OpenPage<ContainersInPackingPage>();
            var viewHelper = new ViewHelper<ContainersInPackingPage>(containersInPackingPage);
            viewHelper.CheckGridViewHeaderCss<ContainersInPackingPage>(containersInPackingPage);
            viewHelper.CheckGridViewFilteringRow<ContainersInPackingPage>(containersInPackingPage);
            viewHelper.CheckGridViewHeaderSorting<ContainersInPackingPage>(containersInPackingPage);
        }

        [Fact(DisplayName = "Check Dispatch Orders View Page UI")]
        public void CheckDispatchOrdersViewPageUi()
        {
            var dispatchOrdersViewPage = fixture.homePage.OpenPage<DispatchOrdersViewPage>();
            var viewHelper = new ViewHelper<DispatchOrdersViewPage>(dispatchOrdersViewPage);
            viewHelper.CheckGridViewHeaderCss<DispatchOrdersViewPage>(dispatchOrdersViewPage);
            viewHelper.CheckGridViewFilteringRow<DispatchOrdersViewPage>(dispatchOrdersViewPage);
            viewHelper.CheckGridViewHeaderSorting<DispatchOrdersViewPage>(dispatchOrdersViewPage);
        }

        [Fact(DisplayName = "Check Order Batches Page UI")]
        public void CheckOrdersPerDenominationsPageUi()
        {
            var orderBatchesPage = fixture.homePage.OpenPage<OrderBatchesPage>();
            var viewHelper = new ViewHelper<OrderBatchesPage>(orderBatchesPage);
            viewHelper.CheckGridViewHeaderCss<OrderBatchesPage>(orderBatchesPage);
            viewHelper.CheckGridViewFilteringRow<OrderBatchesPage>(orderBatchesPage);
            viewHelper.CheckGridViewHeaderSorting<OrderBatchesPage>(orderBatchesPage);
        }


        [Fact(DisplayName = "Check Outbound Orders View Page UI")]
        public void CheckOutboundOrdersViewPageUi()
        {
            var outboundOrdersViewPage = fixture.homePage.OpenPage<OutboundOrdersViewPage>();
            var viewHelper = new ViewHelper<OutboundOrdersViewPage>(outboundOrdersViewPage);
            viewHelper.CheckGridViewHeaderCss<OutboundOrdersViewPage>(outboundOrdersViewPage);
            viewHelper.CheckGridViewFilteringRow<OutboundOrdersViewPage>(outboundOrdersViewPage);
            viewHelper.CheckGridViewHeaderSorting<OutboundOrdersViewPage>(outboundOrdersViewPage);
        }


        #if (runPerformance)
        [Fact(DisplayName = "Containers in Packing Page Navigation Performance")]
        public void ContainersInPackingPageNavigationPerformance()
        {
            var containersInPackingPage = fixture.homePage.OpenPage<ContainersInPackingPage>();
            var viewHelper = new ViewHelper<ContainersInPackingPage>(containersInPackingPage);
            viewHelper.NavigateViewPage(containersInPackingPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(containersInPackingPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Containers in Packing Page Filter by Date Performance")]
        public void ContainersInPackingPageFilterByDatePerformance()
        {
            var containersInPackingPage = fixture.homePage.OpenPage<ContainersInPackingPage>();
            var viewHelper = new ViewHelper<ContainersInPackingPage>(containersInPackingPage);
            viewHelper.ApplyFilterByDate<ContainersInPackingPage>(containersInPackingPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(containersInPackingPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ContainersInPackingPage>(containersInPackingPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(containersInPackingPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ContainersInPackingPage>(containersInPackingPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(containersInPackingPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ContainersInPackingPage>(containersInPackingPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(containersInPackingPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<ContainersInPackingPage>(containersInPackingPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(containersInPackingPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ContainersInPackingPage>(containersInPackingPage, "ignoredate", null, null, null);
            PerformanceHelper.SaveFilterPerformance(containersInPackingPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Dispatch Orders View Page Navigation Performance")]
        public void DispatchOrdersViewPageNavigationPerformance()
        {
            var dispatchOrdersViewPage = fixture.homePage.OpenPage<DispatchOrdersViewPage>();
            var viewHelper = new ViewHelper<DispatchOrdersViewPage>(dispatchOrdersViewPage);
            viewHelper.NavigateViewPage(dispatchOrdersViewPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(dispatchOrdersViewPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Dispatch Orders View Page Filter by Date Performance")]
        public void DispatchOrdersViewPageFilterByDatePerformance()
        {
            var dispatchOrdersViewPage = fixture.homePage.OpenPage<DispatchOrdersViewPage>();
            var viewHelper = new ViewHelper<DispatchOrdersViewPage>(dispatchOrdersViewPage);
            viewHelper.ApplyFilterByDate<DispatchOrdersViewPage>(dispatchOrdersViewPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(dispatchOrdersViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<DispatchOrdersViewPage>(dispatchOrdersViewPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(dispatchOrdersViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<DispatchOrdersViewPage>(dispatchOrdersViewPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(dispatchOrdersViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<DispatchOrdersViewPage>(dispatchOrdersViewPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(dispatchOrdersViewPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<DispatchOrdersViewPage>(dispatchOrdersViewPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(dispatchOrdersViewPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<DispatchOrdersViewPage>(dispatchOrdersViewPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(dispatchOrdersViewPage.GetType().Name, viewHelper.figure);
        }


        [Fact(DisplayName = "Order Batches Page Navigation Performance")]
        public void OrderBatchesPageNavigationPerformance()
        {
            var orderBatchesPage = fixture.homePage.OpenPage<OrderBatchesPage>();
            var viewHelper = new ViewHelper<OrderBatchesPage>(orderBatchesPage);
            viewHelper.NavigateViewPage(orderBatchesPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(orderBatchesPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Order Batches Page Filter by Date Performance")]
        public void OrderBatchesPageFilterByDatePerformance()
        {
            var orderBatchesPage = fixture.homePage.OpenPage<OrderBatchesPage>();
            var viewHelper = new ViewHelper<OrderBatchesPage>(orderBatchesPage);
            viewHelper.ApplyFilterByDate<OrderBatchesPage>(orderBatchesPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(orderBatchesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrderBatchesPage>(orderBatchesPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(orderBatchesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrderBatchesPage>(orderBatchesPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(orderBatchesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrderBatchesPage>(orderBatchesPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(orderBatchesPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<OrderBatchesPage>(orderBatchesPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(orderBatchesPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<OrderBatchesPage>(orderBatchesPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(orderBatchesPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Outbound Orders View Page Navigation Performance")]
        public void OutboundOrdersViewPageNavigationPerformance()
        {
            var outboundOrdersViewPage = fixture.homePage.OpenPage<OutboundOrdersViewPage>();
            var viewHelper = new ViewHelper<OutboundOrdersViewPage>(outboundOrdersViewPage);
            viewHelper.NavigateViewPage(outboundOrdersViewPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(outboundOrdersViewPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Outbound Orders View Page Filter by Date Performance")]
        public void OutboundOrdersViewPageFilterByDatePerformance()
        {
            var outboundOrdersViewPage = fixture.homePage.OpenPage<OutboundOrdersViewPage>();
            var viewHelper = new ViewHelper<OutboundOrdersViewPage>(outboundOrdersViewPage);
            viewHelper.ApplyFilterByDate<OutboundOrdersViewPage>(outboundOrdersViewPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(outboundOrdersViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OutboundOrdersViewPage>(outboundOrdersViewPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(outboundOrdersViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OutboundOrdersViewPage>(outboundOrdersViewPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(outboundOrdersViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OutboundOrdersViewPage>(outboundOrdersViewPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(outboundOrdersViewPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<OutboundOrdersViewPage>(outboundOrdersViewPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(outboundOrdersViewPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<OutboundOrdersViewPage>(outboundOrdersViewPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(outboundOrdersViewPage.GetType().Name, viewHelper.figure);
        }
        #endif*/
    }
}
