using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.Pages
{
    using CWC.AutoTests.Core;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class ViewPage : BasePage
    {
        public ViewPage(IWebDriver driver) : base(driver)
        {
        }
        
        [FindsBy(How = How.XPath, Using = "//*[contains(@class, 'TD_GridViewLabelHeader')]")]
        public virtual IWebElement GridViewLabelHeader { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class, 'dxgvGroupPanel')]")]
        public virtual IWebElement GroupByColumnRow { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*[contains(@class, 'GridHeaderFilterButton')]")]
        public virtual IList<IWebElement> FilterRowBtnList { get; set; }

        [FindsBy(How = How.ClassName, Using = "dxgvFilterPopupSelectedItem")]
        public virtual IWebElement FilterRowPopupSelectedItem { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class, 'dxgvHeader')]")]
        public virtual IList<IWebElement> ColumnHeaderList { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class, 'dxGridView_gvHeaderSortUp')]")]
        public virtual IWebElement ColumnHeaderSortUp { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class, 'dxGridView_gvHeaderSortDown')]")]
        public virtual IWebElement ColumnHeaderSortDown { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class, 'dxgvFilterPopupItemsArea')]")]
        public virtual IWebElement FilterPopupWindow { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_searchCtrl_btSearchCtrl_B")]
        public virtual IWebElement SearchBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_gridOrdersCtrl_GridDataViewCtrl_PagerBarB_FirstButton_B")]
        public virtual IWebElement FirstBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_gridOrdersCtrl_GridDataViewCtrl_PagerBarB_PrevButton_B")]
        public virtual IWebElement PrevBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_gridOrdersCtrl_GridDataViewCtrl_PagerBarB_NextButton_B")]
        public virtual IWebElement NextBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_gridOrdersCtrl_GridDataViewCtrl_PagerBarB_LastButton_B")]
        public virtual IWebElement LastBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxOperationPeriodTypeCtrl_B-1")]
        public virtual IWebElement DatePeriodTypeCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxOperationPeriodTypeCtrl_DDD_L_LBI0T0")]
        public virtual IWebElement DuringOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxOperationPeriodTypeCtrl_DDD_L_LBI1T0")]
        public virtual IWebElement FromToOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxOperationPeriodTypeCtrl_DDD_L_LBI2T0")]
        public virtual IWebElement IgnoreDateOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_startPeriodCTrl_I")]
        public virtual IWebElement DatePeriodFromCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_endPeriodCtrl_I")]
        public virtual IWebElement DatePeriodToCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxPeriodTypeCtrl_B-1")]
        public virtual IWebElement DatePeriodSubTypeCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxPeriodTypeCtrl_DDD_L_LBI0T0")]
        public virtual IWebElement LastOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxPeriodTypeCtrl_DDD_L_LBI1T0")]
        public virtual IWebElement CurrentOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxPeriodTypeCtrl_DDD_L_LBI2T0")]
        public virtual IWebElement NextOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_txBxIntervalValueCtrl_I")]
        public virtual IWebElement DatePeriodNumberCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxIntervalCtrl_B-1")]
        public virtual IWebElement DatePeriodIntervalCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxIntervalCtrl_DDD_L_LBI0T0")]
        public virtual IWebElement DaysOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxIntervalCtrl_DDD_L_LBI1T0")]
        public virtual IWebElement WeeksOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_periodParametersCtrl_panelCallBackMainCtrl_cmBxIntervalCtrl_DDD_L_LBI2T0")]
        public virtual IWebElement MonthsOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_gridOrdersCtrl_GridDataViewCtrl_LPV")]
        public virtual IWebElement LoadingPopUp { get; set; }
    }
}
