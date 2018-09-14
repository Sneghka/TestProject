//#define runPerformance

using CWC.AutoTests.Core;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Pages.Accounting;
using Xunit;

namespace CWC.AutoTests.Tests
{
    [Collection("Test Collection #1")]
    public class AccountingModuleViewTests : BaseTest, IClassFixture<TestFixture>
    {
        TestFixture fixture;

        public void SetFixture(TestFixture setupClass)
        {
        }


        public AccountingModuleViewTests(TestFixture setupClass)
        {
            // Add code to be executed before every test
            this.fixture = setupClass;

            #if (runPerformance)
            PerformanceHelper.SaveStartTime();
            #endif
        }

        [Fact(DisplayName = "UI - Open Billing Lines Page")]
        public void OpenBillingLinesPage()
        {
            var billingLinesPage = fixture.HomePage.OpenPage<BillingLinesPage>();
            Assert.True(billingLinesPage.BillingLinesHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Cash Flow Information Page")]
        public void OpenCashFlowInformationPage()
        {
            var cashFlowInformationPage = fixture.HomePage.OpenPage<CashFlowInformationPage>();
            Assert.True(cashFlowInformationPage.CashFlowInformationHeader.Displayed);

        }

        [Fact(DisplayName = "UI - Open Discrepancies Page")]
        public void OpenDiscrepanciesPage()
        {
            var discrepanciesPage = fixture.HomePage.OpenPage<DiscrepanciesPage>();
            Assert.True(discrepanciesPage.DiscrepanciesHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Invoice Lines Page")]
        public void OpenInvoiceLinesPage()
        {
            var invoiceLinesPage = fixture.HomePage.OpenPage<InvoiceLinesPage>();
            Assert.True(invoiceLinesPage.InvoiceLinesHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Order Pre-announcements Page")]
        public void OpenOrderPreAnnouncementsPage()
        {
            var orderPreAnnouncementsPage = fixture.HomePage.OpenPage<OrderPreAnnouncementsPage>();
            Assert.True(orderPreAnnouncementsPage.OrderPreAnnouncementsHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Pre-crediting Value View Page")]
        public void OpenPreCreditingValueViewPage()
        {
            var preCreditingValueViewPage = fixture.HomePage.OpenPage<PreCreditingValueViewPage>();
            Assert.True(preCreditingValueViewPage.PreCreditingValueViewHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Re-billing Page")]
        public void OpenReBillingPage()
        {
            var reBillingPage = fixture.HomePage.OpenPage<ReBillingPage>();
            Assert.True(reBillingPage.ReBillingLinesHeader.Displayed);
        }

        [Fact(DisplayName = "UI - Open Reconciliation View Page")]
        public void OpenReconciliationViewPage()
        {
            var reconciliationViewPage = fixture.HomePage.OpenPage<ReconciliationViewPage>();
            Assert.True(reconciliationViewPage.ReconciliationViewGraphTabHeader.Displayed);
        }
        /*
        [Fact(DisplayName = "Check Billing Lines Page UI")]
        public void CheckBillingLinesPageUi()
        {
            var billingLinesPage = fixture.homePage.OpenPage<BillingLinesPage>();
            var viewHelper = new ViewHelper<BillingLinesPage>(billingLinesPage);
            viewHelper.CheckGridViewHeaderCss<BillingLinesPage>(billingLinesPage);
            viewHelper.CheckGridViewFilteringRow<BillingLinesPage>(billingLinesPage);
            viewHelper.CheckGridViewHeaderSorting<BillingLinesPage>(billingLinesPage);
        }

        [Fact(DisplayName = "Check Cash Flow Information Page UI")]
        public void CheckCashFlowInformationPageUi()
        {
            var cashFlowInformationPage = fixture.homePage.OpenPage<CashFlowInformationPage>();
            var viewHelper = new ViewHelper<CashFlowInformationPage>(cashFlowInformationPage);
            viewHelper.CheckGridViewHeaderCss<CashFlowInformationPage>(cashFlowInformationPage);
            viewHelper.CheckGridViewFilteringRow<CashFlowInformationPage>(cashFlowInformationPage);
            viewHelper.CheckGridViewHeaderSorting<CashFlowInformationPage>(cashFlowInformationPage);
        }

        [Fact(DisplayName = "Check Discrepancies Page UI")]
        public void CheckDiscrepanciesPageUi()
        {
            var discrepanciesPage = fixture.homePage.OpenPage<DiscrepanciesPage>();
            var viewHelper = new ViewHelper<DiscrepanciesPage>(discrepanciesPage);
            viewHelper.CheckGridViewHeaderCss<DiscrepanciesPage>(discrepanciesPage);
            viewHelper.CheckGridViewFilteringRow<DiscrepanciesPage>(discrepanciesPage);
            viewHelper.CheckGridViewHeaderSorting<DiscrepanciesPage>(discrepanciesPage);
        }

        [Fact(DisplayName = "Check Invoice Lines Page UI")]
        public void CheckInvoiceLinesPageUi()
        {
            var invoiceLinesPage = fixture.homePage.OpenPage<InvoiceLinesPage>();
            var viewHelper = new ViewHelper<InvoiceLinesPage>(invoiceLinesPage);
            viewHelper.CheckGridViewHeaderCss<InvoiceLinesPage>(invoiceLinesPage);
            viewHelper.CheckGridViewFilteringRow<InvoiceLinesPage>(invoiceLinesPage);
            viewHelper.CheckGridViewHeaderSorting<InvoiceLinesPage>(invoiceLinesPage);
        }

        [Fact(DisplayName = "Check Order Pre-announcements Page UI")]
        public void CheckOrderPreAnnouncementsPageUi()
        {
            var orderPreAnnouncementsPage = fixture.homePage.OpenPage<OrderPreAnnouncementsPage>();
            var viewHelper = new ViewHelper<OrderPreAnnouncementsPage>(orderPreAnnouncementsPage);
            viewHelper.CheckGridViewHeaderCss<OrderPreAnnouncementsPage>(orderPreAnnouncementsPage);
            viewHelper.CheckGridViewFilteringRow<OrderPreAnnouncementsPage>(orderPreAnnouncementsPage);
            viewHelper.CheckGridViewHeaderSorting<OrderPreAnnouncementsPage>(orderPreAnnouncementsPage);
        }

        [Fact(DisplayName = "Check Pre-crediting Value View Page UI")]
        public void CheckPreCreditingValueViewPageUi()
        {
            var preCreditingValueViewPage = fixture.homePage.OpenPage<PreCreditingValueViewPage>();
            var viewHelper = new ViewHelper<PreCreditingValueViewPage>(preCreditingValueViewPage);
            viewHelper.CheckGridViewHeaderCss<PreCreditingValueViewPage>(preCreditingValueViewPage);
            viewHelper.CheckGridViewFilteringRow<PreCreditingValueViewPage>(preCreditingValueViewPage);
            viewHelper.CheckGridViewHeaderSorting<PreCreditingValueViewPage>(preCreditingValueViewPage);
        }

        [Fact(DisplayName = "Check Re-billing Page UI")]
        public void CheckReBillingPageUi()
        {
            var reBillingPage = fixture.homePage.OpenPage<ReBillingPage>();
            var viewHelper = new ViewHelper<ReBillingPage>(reBillingPage);
            viewHelper.CheckGridViewHeaderCss<ReBillingPage>(reBillingPage);
            viewHelper.CheckGridViewFilteringRow<ReBillingPage>(reBillingPage);
            viewHelper.CheckGridViewHeaderSorting<ReBillingPage>(reBillingPage);
        }

        [Fact(DisplayName = "Check Reconciliation View Page UI")]
        public void CheckReconciliationViewPageUi()
        {
            var reconciliationViewPage = fixture.homePage.OpenPage<ReconciliationViewPage>();
            var viewHelper = new ViewHelper<ReconciliationViewPage>(reconciliationViewPage);
            viewHelper.CheckGridViewHeaderCss<ReconciliationViewPage>(reconciliationViewPage);
            viewHelper.CheckGridViewFilteringRow<ReconciliationViewPage>(reconciliationViewPage);
            viewHelper.CheckGridViewHeaderSorting<ReconciliationViewPage>(reconciliationViewPage);
        }*/

        #if (runPerformance)
        [Fact(DisplayName = "Billing Lines Page Navigation Performance")]
        public void BillingLinesPageNavigationPerformance()
        {
            var billingLinesPage = fixture.homePage.OpenPage<BillingLinesPage>();
            var viewHelper = new ViewHelper<BillingLinesPage>(billingLinesPage);
            viewHelper.NavigateViewPage(billingLinesPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(billingLinesPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Billing Lines Page Filter by Date Performance")]
        public void BillingLinesPageFilterByDatePerformance()
        {
            var billingLinesPage = fixture.homePage.OpenPage<BillingLinesPage>();
            var viewHelper = new ViewHelper<BillingLinesPage>(billingLinesPage);
            viewHelper.ApplyFilterByDate<BillingLinesPage>(billingLinesPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(billingLinesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<BillingLinesPage>(billingLinesPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(billingLinesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<BillingLinesPage>(billingLinesPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(billingLinesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<BillingLinesPage>(billingLinesPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(billingLinesPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<BillingLinesPage>(billingLinesPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(billingLinesPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<BillingLinesPage>(billingLinesPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(billingLinesPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Cash Flow Information Page Navigation Performance")]
        public void CashFlowInformationPageNavigationPerformance()
        {
            var cashFlowInformationPage = fixture.homePage.OpenPage<CashFlowInformationPage>();
            var viewHelper = new ViewHelper<CashFlowInformationPage>(cashFlowInformationPage);
            viewHelper.NavigateViewPage(cashFlowInformationPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(cashFlowInformationPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Cash Flow Information Page Filter by Date Performance")]
        public void CashFlowInformationPageFilterByDatePerformance()
        {
            var cashFlowInformationPage = fixture.homePage.OpenPage<CashFlowInformationPage>();
            var viewHelper = new ViewHelper<CashFlowInformationPage>(cashFlowInformationPage);
            viewHelper.ApplyFilterByDate<CashFlowInformationPage>(cashFlowInformationPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(cashFlowInformationPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<CashFlowInformationPage>(cashFlowInformationPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(cashFlowInformationPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<CashFlowInformationPage>(cashFlowInformationPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(cashFlowInformationPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<CashFlowInformationPage>(cashFlowInformationPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(cashFlowInformationPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<CashFlowInformationPage>(cashFlowInformationPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(cashFlowInformationPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<CashFlowInformationPage>(cashFlowInformationPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(cashFlowInformationPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Discrepancies Page Navigation Performance")]
        public void DiscrepanciesPageNavigationPerformance()
        {
            var discrepanciesPage = fixture.homePage.OpenPage<DiscrepanciesPage>();
            var viewHelper = new ViewHelper<DiscrepanciesPage>(discrepanciesPage);
            viewHelper.NavigateViewPage(discrepanciesPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(discrepanciesPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Discrepancies Page Filter by Date Performance")]
        public void DiscrepanciesPageFilterByDatePerformance()
        {
            var discrepanciesPage = fixture.homePage.OpenPage<DiscrepanciesPage>();
            var viewHelper = new ViewHelper<DiscrepanciesPage>(discrepanciesPage);
            viewHelper.ApplyFilterByDate<DiscrepanciesPage>(discrepanciesPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(discrepanciesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<DiscrepanciesPage>(discrepanciesPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(discrepanciesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<DiscrepanciesPage>(discrepanciesPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(discrepanciesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<DiscrepanciesPage>(discrepanciesPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(discrepanciesPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<DiscrepanciesPage>(discrepanciesPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(discrepanciesPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<DiscrepanciesPage>(discrepanciesPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(discrepanciesPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Invoice Lines Page Navigation Performance")]
        public void InvoiceLinesPageNavigationPerformance()
        {
            var invoiceLinesPage = fixture.homePage.OpenPage<InvoiceLinesPage>();
            var viewHelper = new ViewHelper<InvoiceLinesPage>(invoiceLinesPage);
            viewHelper.NavigateViewPage(invoiceLinesPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(invoiceLinesPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Invoice Lines Page Filter by Date Performance")]
        public void InvoiceLinesPageFilterByDatePerformance()
        {
            var invoiceLinesPage = fixture.homePage.OpenPage<InvoiceLinesPage>();
            var viewHelper = new ViewHelper<InvoiceLinesPage>(invoiceLinesPage);
            viewHelper.ApplyFilterByDate<InvoiceLinesPage>(invoiceLinesPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(invoiceLinesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<InvoiceLinesPage>(invoiceLinesPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(invoiceLinesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<InvoiceLinesPage>(invoiceLinesPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(invoiceLinesPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<InvoiceLinesPage>(invoiceLinesPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(invoiceLinesPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<InvoiceLinesPage>(invoiceLinesPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(invoiceLinesPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<InvoiceLinesPage>(invoiceLinesPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(invoiceLinesPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Order Pre-announcements Page Navigation Performance")]
        public void OrderPreAnnouncementsPageNavigationPerformance()
        {
            var orderPreAnnouncementsPage = fixture.homePage.OpenPage<OrderPreAnnouncementsPage>();
            var viewHelper = new ViewHelper<OrderPreAnnouncementsPage>(orderPreAnnouncementsPage);
            viewHelper.NavigateViewPage(orderPreAnnouncementsPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(orderPreAnnouncementsPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Order Pre-announcements Page Filter by Date Performance")]
        public void OrderPreAnnouncementsPageFilterByDatePerformance()
        {
            var orderPreAnnouncementsPage = fixture.homePage.OpenPage<OrderPreAnnouncementsPage>();
            var viewHelper = new ViewHelper<OrderPreAnnouncementsPage>(orderPreAnnouncementsPage);
            viewHelper.ApplyFilterByDate<OrderPreAnnouncementsPage>(orderPreAnnouncementsPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(orderPreAnnouncementsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrderPreAnnouncementsPage>(orderPreAnnouncementsPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(orderPreAnnouncementsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrderPreAnnouncementsPage>(orderPreAnnouncementsPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(orderPreAnnouncementsPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<OrderPreAnnouncementsPage>(orderPreAnnouncementsPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(orderPreAnnouncementsPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<OrderPreAnnouncementsPage>(orderPreAnnouncementsPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(orderPreAnnouncementsPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<OrderPreAnnouncementsPage>(orderPreAnnouncementsPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(orderPreAnnouncementsPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Pre-crediting Value View Page Navigation Performance")]
        public void PreCreditingValueViewPageNavigationPerformance()
        {
            var preCreditingValueViewPage = fixture.homePage.OpenPage<PreCreditingValueViewPage>();
            var viewHelper = new ViewHelper<PreCreditingValueViewPage>(preCreditingValueViewPage);
            viewHelper.NavigateViewPage(preCreditingValueViewPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(preCreditingValueViewPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Pre-crediting Value View Page Filter by Date Performance")]
        public void PreCreditingValueViewPageFilterByDatePerformance()
        {
            var preCreditingValueViewPage = fixture.homePage.OpenPage<PreCreditingValueViewPage>();
            var viewHelper = new ViewHelper<PreCreditingValueViewPage>(preCreditingValueViewPage);
            viewHelper.ApplyFilterByDate<PreCreditingValueViewPage>(preCreditingValueViewPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(preCreditingValueViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<PreCreditingValueViewPage>(preCreditingValueViewPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(preCreditingValueViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<PreCreditingValueViewPage>(preCreditingValueViewPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(preCreditingValueViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<PreCreditingValueViewPage>(preCreditingValueViewPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(preCreditingValueViewPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<PreCreditingValueViewPage>(preCreditingValueViewPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(preCreditingValueViewPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<PreCreditingValueViewPage>(preCreditingValueViewPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(preCreditingValueViewPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Re-Billing Page Navigation Performance")]
        public void ReBillingPageNavigationPerformance()
        {
            var reBillingPage = fixture.homePage.OpenPage<ReBillingPage>();
            var viewHelper = new ViewHelper<ReBillingPage>(reBillingPage);
            viewHelper.NavigateViewPage(reBillingPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(reBillingPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Re-Billing Page Filter by Date Performance")]
        public void ReBillingPageFilterByDatePerformance()
        {
            var reBillingPage = fixture.homePage.OpenPage<ReBillingPage>();
            var viewHelper = new ViewHelper<ReBillingPage>(reBillingPage);
            viewHelper.ApplyFilterByDate<ReBillingPage>(reBillingPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(reBillingPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ReBillingPage>(reBillingPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(reBillingPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ReBillingPage>(reBillingPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(reBillingPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ReBillingPage>(reBillingPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(reBillingPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<ReBillingPage>(reBillingPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(reBillingPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<ReBillingPage>(reBillingPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(reBillingPage.GetType().Name, viewHelper.figure);
        }

        [Fact(DisplayName = "Reconciliation View Page Navigation Performance")]
        public void ReconciliationViewPageNavigationPerformance()
        {
            var reconciliationViewPage = fixture.homePage.OpenPage<ReconciliationViewPage>();
            var viewHelper = new ViewHelper<ReconciliationViewPage>(reconciliationViewPage);
            viewHelper.NavigateViewPage(reconciliationViewPage);
            Assert.True(viewHelper.isPagingPresent, "Paging elements were not found on page. Please adjust date period settings on filter form.");
            PerformanceHelper.SaveNavigationPerformance(reconciliationViewPage.GetType().Name, viewHelper.figure1, viewHelper.figure2, viewHelper.figure3, viewHelper.figure4);
        }

        [Fact(DisplayName = "Reconciliation View Page Filter by Date Performance")]
        public void ReconciliationViewPageFilterByDatePerformance()
        {
            var reconciliationViewPage = fixture.homePage.OpenPage<ReconciliationViewPage>();
            var viewHelper = new ViewHelper<ReconciliationViewPage>(reconciliationViewPage);
            viewHelper.ApplyFilterByDate<ReconciliationViewPage>(reconciliationViewPage, "during", "last", "1", "days");
            PerformanceHelper.SaveFilterPerformance(reconciliationViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ReconciliationViewPage>(reconciliationViewPage, "during", "last", "1", "weeks");
            PerformanceHelper.SaveFilterPerformance(reconciliationViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ReconciliationViewPage>(reconciliationViewPage, "during", "last", "1", "months");
            PerformanceHelper.SaveFilterPerformance(reconciliationViewPage.GetType().Name, viewHelper.figure);
            viewHelper.ApplyFilterByDate<ReconciliationViewPage>(reconciliationViewPage, "during", "last", "3", "months");
            PerformanceHelper.SaveFilterPerformance(reconciliationViewPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<ReconciliationViewPage>(reconciliationViewPage, "during", "last", "6", "months");
            //PerformanceHelper.SaveFilterPerformance(reconciliationViewPage.GetType().Name, viewHelper.figure);
            //viewHelper.ApplyFilterByDate<ReconciliationViewPage>(reconciliationViewPage, "ignoredate", null, null, null);
            //PerformanceHelper.SaveFilterPerformance(reconciliationViewPage.GetType().Name, viewHelper.figure);
        }
        #endif  
    }
}
