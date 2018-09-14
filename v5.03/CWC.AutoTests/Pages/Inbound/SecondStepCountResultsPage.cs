using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.Pages.Inbound
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class SecondStepCountResultsPage : ViewPage
    {
        public SecondStepCountResultsPage(IWebDriver driver): base(driver)
        {
            this.PageUrl += "Solutions/CashCenter/Frames/SecondStepCountResult/SecondStepCountResultsViewFrameNew.aspx";
        }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridSecondStepCountResultsCtrl_lblHeaderCtrl")]
        public IWebElement SecondStepCountResultsHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_btUpdate_B")]
        public override IWebElement SearchBtn { get; set; }
        
        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridSecondStepCountResultsCtrl_GridDataViewCtrl_PagerBarB_FirstButton_B")]
        public override IWebElement FirstBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridSecondStepCountResultsCtrl_GridDataViewCtrl_PagerBarB_PrevButton_B")]
        public override IWebElement PrevBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridSecondStepCountResultsCtrl_GridDataViewCtrl_PagerBarB_NextButton_B")]
        public override IWebElement NextBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridSecondStepCountResultsCtrl_GridDataViewCtrl_PagerBarB_LastButton_B")]
        public override IWebElement LastBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridSecondStepCountResultsCtrl_GridDataViewCtrl_LPV")]
        public override IWebElement LoadingPopUp { get; set; }
    }
}
