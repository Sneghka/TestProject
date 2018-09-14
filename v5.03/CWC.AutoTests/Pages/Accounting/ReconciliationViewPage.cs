using System;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;

namespace CWC.AutoTests.Pages.Accounting
{
    public class ReconciliationViewPage : ViewPage
    {
        public ReconciliationViewPage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/Reconciliation/Frames/ReconciliationViewFrame.aspx";
        }

        [FindsBy(How = How.Id, Using = "ctl00_cphFilter_viewCtrl_pcMain_AT0T")]
        public IWebElement ReconciliationViewGraphTabHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_btUpdate_B")]
        public override IWebElement SearchBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridDiscrepanciesCtrl_GridDataViewCtrl_PagerBarB_FirstButton_B")]
        public override IWebElement FirstBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridDiscrepanciesCtrl_GridDataViewCtrl_PagerBarB_PrevButton_B")]
        public override IWebElement PrevBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridDiscrepanciesCtrl_GridDataViewCtrl_PagerBarB_NextButton_B")]
        public override IWebElement NextBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridDiscrepanciesCtrl_GridDataViewCtrl_PagerBarB_LastButton_B")]
        public override IWebElement LastBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridDiscrepanciesCtrl_GridDataViewCtrl_LPV")]
        public override IWebElement LoadingPopUp { get; set; }
    }
}
