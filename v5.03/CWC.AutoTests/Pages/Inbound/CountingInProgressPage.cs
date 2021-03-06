﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.Pages.Inbound
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class CountingInProgressPage : ViewPage
    {
        public CountingInProgressPage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/CashCenter/Frames/CountingInProgressViewFrame.aspx";
        }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_nbCenter_GHE2")]
        public IWebElement CountingInProgressHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_btUpdate_B")]
        public override IWebElement SearchBtn { get; set; }
        
        [FindsBy(How = How.Id, Using = "ctl00_cphCR_nbCenter_ITC2i0_gridCountingInProgressVsSLA_PagerBarB_FirstButton_B")]
        public override IWebElement FirstBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_nbCenter_ITC2i0_gridCountingInProgressVsSLA_PagerBarB_PrevButton_B")]
        public override IWebElement PrevBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_nbCenter_ITC2i0_gridCountingInProgressVsSLA_PagerBarB_NextButton_B")]
        public override IWebElement NextBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_nbCenter_ITC2i0_gridCountingInProgressVsSLA_PagerBarB_LastButton_B")]
        public override IWebElement LastBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_nbCenter_ITC2i0_gridCountingInProgressVsSLA_GridDataViewCtrl_LPV")]
        public override IWebElement LoadingPopUp { get; set; }
    }
}
