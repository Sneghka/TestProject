//#define runPerformance
using CWC.AutoTests.Core;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Pages.Inbound;
using Xunit;

namespace CWC.AutoTests.Tests
{
    [Collection("Inbound Collection #1")]
    public class InboundViewTest : BaseTest, IClassFixture<TestFixture>
    {
        TestFixture fixture;

        public void SetFixture(TestFixture setupClass)
        {
        }

        public InboundViewTest(TestFixture setupClass)
        {
            // Add code to be executed before every test
            this.fixture = setupClass;

            #if (runPerformance)
                        PerformanceHelper.SaveStartTime();
            #endif
        }
        /*
        [Fact(DisplayName = "Check Received Containers Page UI")]
        public void CheckReceivedContainersPageUi()
        {
            var receivedContainersPage = fixture.homePage.OpenPage<ReceivedContainersPage>();
            var viewHelper = new ViewHelper<ReceivedContainersPage>(receivedContainersPage);
            viewHelper.CheckGridViewHeaderCss<ReceivedContainersPage>(receivedContainersPage);
            viewHelper.CheckGridViewFilteringRow<ReceivedContainersPage>(receivedContainersPage);
            viewHelper.CheckGridViewHeaderSorting<ReceivedContainersPage>(receivedContainersPage);
        }

        [Fact(DisplayName = "Check PreAnnouncements Page UI")]
        public void CheckPreAnnouncementsPageUi()
        {
            var preAnnouncementsPage = fixture.homePage.OpenPage<PreAnnouncementsPage>();
            var viewHelper = new ViewHelper<PreAnnouncementsPage>(preAnnouncementsPage);
            viewHelper.CheckGridViewHeaderCss<PreAnnouncementsPage>(preAnnouncementsPage);
            viewHelper.CheckGridViewFilteringRow<PreAnnouncementsPage>(preAnnouncementsPage);
            viewHelper.CheckGridViewHeaderSorting<PreAnnouncementsPage>(preAnnouncementsPage);
        }

        [Fact(DisplayName = "Check Count Orders Page UI")]
        public void CheckCountOrdersPageUi()
        {
            var countOrdersPage = fixture.homePage.OpenPage<CountOrdersPage>();
            var viewHelper = new ViewHelper<CountOrdersPage>(countOrdersPage);
            viewHelper.CheckGridViewHeaderCss<CountOrdersPage>(countOrdersPage);
            viewHelper.CheckGridViewFilteringRow<CountOrdersPage>(countOrdersPage);
            viewHelper.CheckGridViewHeaderSorting<CountOrdersPage>(countOrdersPage);
        }

        [Fact(DisplayName = "Check Container Batches Page UI")]
        public void CheckContainerBatchesPageUi()
        {
            var containerBatchesPage = fixture.homePage.OpenPage<ContainerBatchesPage>();
            var viewHelper = new ViewHelper<ContainerBatchesPage>(containerBatchesPage);
            viewHelper.CheckGridViewHeaderCss<ContainerBatchesPage>(containerBatchesPage);
            viewHelper.CheckGridViewFilteringRow<ContainerBatchesPage>(containerBatchesPage);
            viewHelper.CheckGridViewHeaderSorting<ContainerBatchesPage>(containerBatchesPage);
        }

        [Fact(DisplayName = "Check First Step Count Results Page UI")]
        public void CheckFirstStepCountResultsPageUi()
        {
            var firstStepCountResultsPage = fixture.homePage.OpenPage<FirstStepCountResultsPage>();
            var viewHelper = new ViewHelper<FirstStepCountResultsPage>(firstStepCountResultsPage);
            viewHelper.CheckGridViewHeaderCss<FirstStepCountResultsPage>(firstStepCountResultsPage);
            viewHelper.CheckGridViewFilteringRow<FirstStepCountResultsPage>(firstStepCountResultsPage);
            viewHelper.CheckGridViewHeaderSorting<FirstStepCountResultsPage>(firstStepCountResultsPage);
        }

        [Fact(DisplayName = "Check Second Step Count Results Page UI")]
        public void CheckSecondStepCountResultsPageUi()
        {
            var secondStepCountResultsPage = fixture.homePage.OpenPage<SecondStepCountResultsPage>();
            var viewHelper = new ViewHelper<SecondStepCountResultsPage>(secondStepCountResultsPage);
            viewHelper.CheckGridViewHeaderCss<SecondStepCountResultsPage>(secondStepCountResultsPage);
            viewHelper.CheckGridViewFilteringRow<SecondStepCountResultsPage>(secondStepCountResultsPage);
            viewHelper.CheckGridViewHeaderSorting<SecondStepCountResultsPage>(secondStepCountResultsPage);
        }

        [Fact(DisplayName = "Check Stock Owner Count Results Page UI")]
        public void CheckStockOwnerCountResultsPageUi()
        {
            var stockOwnerCountResultsPage = fixture.homePage.OpenPage<StockOwnerCountResultsPage>();
            var viewHelper = new ViewHelper<StockOwnerCountResultsPage>(stockOwnerCountResultsPage);
            viewHelper.CheckGridViewHeaderCss<StockOwnerCountResultsPage>(stockOwnerCountResultsPage);
            viewHelper.CheckGridViewFilteringRow<StockOwnerCountResultsPage>(stockOwnerCountResultsPage);
            viewHelper.CheckGridViewHeaderSorting<StockOwnerCountResultsPage>(stockOwnerCountResultsPage);
        }

        [Fact(DisplayName = "Check Counting In Progress Page UI")]
        public void CheckCountingInProgressPageUi()
        {
            var countingInProgressPage = fixture.homePage.OpenPage<CountingInProgressPage>();
            var viewHelper = new ViewHelper<CountingInProgressPage>(countingInProgressPage);
            viewHelper.CheckGridViewHeaderCss<CountingInProgressPage>(countingInProgressPage);
            viewHelper.CheckGridViewFilteringRow<CountingInProgressPage>(countingInProgressPage);
            viewHelper.CheckGridViewHeaderSorting<CountingInProgressPage>(countingInProgressPage);
        }*/

        [Fact(DisplayName = "UI - Open Received Containers Page")]
        public void OpenReceivedContainersPage()
        {
            var receivedContainersPage = fixture.HomePage.OpenPage<ReceivedContainersPage>();
            Assert.True(receivedContainersPage.ReceivedContainersHeader.Displayed);
        }

        [Fact(DisplayName = "Open Pre-announcements Page")]
        public void OpenPreAnnouncementsPage()
        {
            var preAnnouncementsPage = fixture.HomePage.OpenPage<PreAnnouncementsPage>();
            Assert.True(preAnnouncementsPage.PreAnnouncementsHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Count Orders Page")]
        public void OpenCountOrdersPage()
        {
            var countOrdersPage = fixture.HomePage.OpenPage<CountOrdersPage>();
            Assert.True(countOrdersPage.CountOrdersHeader.Displayed);
        }

        [Fact(DisplayName = "Open Container Batches Page")]
        public void OpenContainerBatchesPage()
        {
            var containerBatchesPage = fixture.HomePage.OpenPage<ContainerBatchesPage>();
            Assert.True(containerBatchesPage.ContainerBatchesHeader.Displayed);
        }

        [Fact(DisplayName = "Open First Step Count Results Page")]
        public void OpenFirstStepCountResultsPage()
        {
            var firstStepCountResultsPage = fixture.HomePage.OpenPage<FirstStepCountResultsPage>();
            Assert.True(firstStepCountResultsPage.FirstStepCountResultsHeader.Displayed);
        }

        [Fact(DisplayName = "Open Second Step Count Results Page")]
        public void OpenSecondStepCountResultsPage()
        {
            var secondStepCountResultsPage = fixture.HomePage.OpenPage<SecondStepCountResultsPage>();
            Assert.True(secondStepCountResultsPage.SecondStepCountResultsHeader.Displayed);
        }

        [Fact(DisplayName = "Open Stock Owner Count Results Page")]
        public void OpenStockOwnerCountResultsPage()
        {
            var stockOwnerCountResultsPage = fixture.HomePage.OpenPage<StockOwnerCountResultsPage>();
            Assert.True(stockOwnerCountResultsPage.StockOwnerCountResultsHeader.Displayed);
        }

        [Fact(DisplayName = "Open Counting in Progress Page")]
        public void OpenCountingInProgressPage()
        {
            var countingInProgressPage = fixture.HomePage.OpenPage<CountingInProgressPage>();
            Assert.True(countingInProgressPage.CountingInProgressHeader.Displayed);
        }
        

        #if (runPerformance)
        [Fact(DisplayName = "Pre-announcements Page Filter by Date Performance")]
        public void PreAnnouncementsPageFilterByDatePerformance()
        {
            var preAnnouncementsPage = fixture.homePage.OpenPage<PreAnnouncementsPage>();
            var viewHelper = new ViewHelper<PreAnnouncementsPage>(preAnnouncementsPage);
            viewHelper.ApplyFilterByDate<PreAnnouncementsPage>(preAnnouncementsPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(preAnnouncementsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<PreAnnouncementsPage>(preAnnouncementsPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(preAnnouncementsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<PreAnnouncementsPage>(preAnnouncementsPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(preAnnouncementsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<PreAnnouncementsPage>(preAnnouncementsPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(preAnnouncementsPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<PreAnnouncementsPage>(preAnnouncementsPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(preAnnouncementsPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Received Containers Page Filter by Date Performance")]
        public void ReceivedContainersPageFilterByDatePerformance()
        {
            var receivedContainersPage = fixture.homePage.OpenPage<ReceivedContainersPage>();
            var viewHelper = new ViewHelper<ReceivedContainersPage>(receivedContainersPage);
            viewHelper.ApplyFilterByDate<ReceivedContainersPage>(receivedContainersPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(receivedContainersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ReceivedContainersPage>(receivedContainersPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(receivedContainersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ReceivedContainersPage>(receivedContainersPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(receivedContainersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ReceivedContainersPage>(receivedContainersPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(receivedContainersPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<ReceivedContainersPage>(receivedContainersPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(receivedContainersPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Count Orders Page Filter by Date Performance")]
        public void CountOrdersPageFilterByDatePerformance()
        {
            var countOrdersPage = fixture.homePage.OpenPage<CountOrdersPage>();
            var viewHelper = new ViewHelper<CountOrdersPage>(countOrdersPage);
            viewHelper.ApplyFilterByDate<CountOrdersPage>(countOrdersPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(countOrdersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<CountOrdersPage>(countOrdersPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(countOrdersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<CountOrdersPage>(countOrdersPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(countOrdersPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<CountOrdersPage>(countOrdersPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(countOrdersPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<CountOrdersPage>(countOrdersPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(countOrdersPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Container Batches Page Filter by Date Performance")]
        public void ContainerBatchesPageFilterByDatePerformance()
        {
            var containerBatchesPage = fixture.homePage.OpenPage<ContainerBatchesPage>();
            var viewHelper = new ViewHelper<ContainerBatchesPage>(containerBatchesPage);
            viewHelper.ApplyFilterByDate<ContainerBatchesPage>(containerBatchesPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(containerBatchesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ContainerBatchesPage>(containerBatchesPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(containerBatchesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ContainerBatchesPage>(containerBatchesPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(containerBatchesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ContainerBatchesPage>(containerBatchesPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(containerBatchesPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<ContainerBatchesPage>(containerBatchesPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(containerBatchesPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "First Step Count Results Page Filter by Date Performance")]
        public void FirstStepCountResultsPageFilterByDatePerformance()
        {
            var firstStepCountResultsPage = fixture.homePage.OpenPage<FirstStepCountResultsPage>();
            var viewHelper = new ViewHelper<FirstStepCountResultsPage>(firstStepCountResultsPage);
            viewHelper.ApplyFilterByDate<FirstStepCountResultsPage>(firstStepCountResultsPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(firstStepCountResultsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<FirstStepCountResultsPage>(firstStepCountResultsPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(firstStepCountResultsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<FirstStepCountResultsPage>(firstStepCountResultsPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(firstStepCountResultsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<FirstStepCountResultsPage>(firstStepCountResultsPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(firstStepCountResultsPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<FirstStepCountResultsPage>(firstStepCountResultsPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(firstStepCountResultsPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Second Step Count Results Page Filter by Date Performance")]
        public void SecondStepCountResultsPageFilterByDatePerformance()
        {
            var secondStepCountResultsPage = fixture.homePage.OpenPage<SecondStepCountResultsPage>();
            var viewHelper = new ViewHelper<SecondStepCountResultsPage>(secondStepCountResultsPage);
            viewHelper.ApplyFilterByDate<SecondStepCountResultsPage>(secondStepCountResultsPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(secondStepCountResultsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<SecondStepCountResultsPage>(secondStepCountResultsPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(secondStepCountResultsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<SecondStepCountResultsPage>(secondStepCountResultsPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(secondStepCountResultsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<SecondStepCountResultsPage>(secondStepCountResultsPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(secondStepCountResultsPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<SecondStepCountResultsPage>(secondStepCountResultsPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(secondStepCountResultsPage.GetType().Name, viewHelper.figure);
        }
        
        [Fact(DisplayName = "Stock Owner Count Results Page Filter by Date Performance")]
        public void StockOwnerCountResultsPageFilterByDatePerformance()
        {
            var stockOwnerCountResultsPage = fixture.homePage.OpenPage<StockOwnerCountResultsPage>();
            var viewHelper = new ViewHelper<StockOwnerCountResultsPage>(stockOwnerCountResultsPage);
            viewHelper.ApplyFilterByDate<StockOwnerCountResultsPage>(stockOwnerCountResultsPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(stockOwnerCountResultsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<StockOwnerCountResultsPage>(stockOwnerCountResultsPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(stockOwnerCountResultsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<StockOwnerCountResultsPage>(stockOwnerCountResultsPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(stockOwnerCountResultsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<StockOwnerCountResultsPage>(stockOwnerCountResultsPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(stockOwnerCountResultsPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<StockOwnerCountResultsPage>(stockOwnerCountResultsPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(stockOwnerCountResultsPage.GetType().Name, viewHelper.figure);
        }
        
        [Fact(DisplayName = "Pre-announcements Page Navigation Performance")]
        public void PreAnnouncementsPageNavigationPerformance()
        {
            var preAnnouncementsPage = fixture.homePage.OpenPage<PreAnnouncementsPage>();
            var viewHelper = new ViewHelper<PreAnnouncementsPage>(preAnnouncementsPage);
            viewHelper.NavigateViewPage(preAnnouncementsPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(preAnnouncementsPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }
        
        [Fact(DisplayName = "Received Containers Page Navigation Performance")]
        public void ReceivedContainersPageNavigationPerformance()
        {
            var receivedContainersPage = fixture.homePage.OpenPage<ReceivedContainersPage>();
            var viewHelper = new ViewHelper<ReceivedContainersPage>(receivedContainersPage);
            viewHelper.NavigateViewPage(receivedContainersPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(receivedContainersPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Count Orders Page Navigation Performance")]
        public void CountOrdersPageNavigationPerformance()
        {
            var countOrdersPage = fixture.homePage.OpenPage<CountOrdersPage>();
            var viewHelper = new ViewHelper<CountOrdersPage>(countOrdersPage);
            viewHelper.NavigateViewPage(countOrdersPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(countOrdersPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Container Batches Page Navigation Performance")]
        public void ContainerBatchesPageNavigationPerformance()
        {
            var containerBatchesPage = fixture.homePage.OpenPage<ContainerBatchesPage>();
            var viewHelper = new ViewHelper<ContainerBatchesPage>(containerBatchesPage);
            viewHelper.NavigateViewPage(containerBatchesPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(containerBatchesPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "First Step Count Results Page Navigation Performance")]
        public void FirstStepCountResultsPageNavigationPerformance()
        {
            var firstStepCountResultsPage = fixture.homePage.OpenPage<FirstStepCountResultsPage>();
            var viewHelper = new ViewHelper<FirstStepCountResultsPage>(firstStepCountResultsPage);
            viewHelper.NavigateViewPage(firstStepCountResultsPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(firstStepCountResultsPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Second Step Count Results Page Navigation Performance")]
        public void SecondStepCountResultsPageNavigationPerformance()
        {
            var secondStepCountResultsPage = fixture.homePage.OpenPage<SecondStepCountResultsPage>();
            var viewHelper = new ViewHelper<SecondStepCountResultsPage>(secondStepCountResultsPage);
            viewHelper.NavigateViewPage(secondStepCountResultsPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(secondStepCountResultsPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }
        
        [Fact(DisplayName = "Stock Owner Count Results Page Navigation Performance")]
        public void StockOwnerCountResultsPageNavigationPerformance()
        {
            var stockOwnerCountResultsPage = fixture.homePage.OpenPage<StockOwnerCountResultsPage>();
            var viewHelper = new ViewHelper<StockOwnerCountResultsPage>(stockOwnerCountResultsPage);
            viewHelper.NavigateViewPage(stockOwnerCountResultsPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(stockOwnerCountResultsPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }
        #endif
    }
}
