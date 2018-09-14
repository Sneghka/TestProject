using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.Pages.Outbound
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class DispatchOrdersViewPage : ViewPage
    {
        public DispatchOrdersViewPage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/CashCenter/DispatchManagement/Frames/DispatchOrdersView.aspx";
        }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridDispatchOrdersViewCtrl_lblHeaderCtrl")]
        public IWebElement DispatchOrdersViewHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_btUpdate_B")]
        public override IWebElement SearchBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxOperationPeriodTypeCtrl_B-1")]
        public override IWebElement DatePeriodTypeCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxOperationPeriodTypeCtrl_DDD_L_LBI0T0")]
        public override IWebElement DuringOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxOperationPeriodTypeCtrl_DDD_L_LBI1T0")]
        public override IWebElement FromToOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxOperationPeriodTypeCtrl_DDD_L_LBI2T0")]
        public override IWebElement IgnoreDateOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_startPeriodCTrl_I")]
        public override IWebElement DatePeriodFromCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_endPeriodCtrl_I")]
        public override IWebElement DatePeriodToCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxPeriodTypeCtrl_B-1")]
        public override IWebElement DatePeriodSubTypeCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxPeriodTypeCtrl_DDD_L_LBI0T0")]
        public override IWebElement LastOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxPeriodTypeCtrl_DDD_L_LBI1T0")]
        public override IWebElement CurrentOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxPeriodTypeCtrl_DDD_L_LBI2T0")]
        public override IWebElement NextOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_txBxIntervalValueCtrl_I")]
        public override IWebElement DatePeriodNumberCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxIntervalCtrl_B-1")]
        public override IWebElement DatePeriodIntervalCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxIntervalCtrl_DDD_L_LBI0T0")]
        public override IWebElement DaysOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxIntervalCtrl_DDD_L_LBI1T0")]
        public override IWebElement WeeksOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxIntervalCtrl_DDD_L_LBI2T0")]
        public override IWebElement MonthsOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridDispatchOrdersViewCtrl_GridDataViewCtrl_PagerBarB_FirstButton_B")]
        public override IWebElement FirstBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridDispatchOrdersViewCtrl_GridDataViewCtrl_PagerBarB_PrevButton_B")]
        public override IWebElement PrevBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridDispatchOrdersViewCtrl_GridDataViewCtrl_PagerBarB_NextButton_B")]
        public override IWebElement NextBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridDispatchOrdersViewCtrl_GridDataViewCtrl_PagerBarB_LastButton_B")]
        public override IWebElement LastBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridDispatchOrdersViewCtrl_GridDataViewCtrl_LPV")]
        public override IWebElement LoadingPopUp { get; set; }
    }
}
