﻿using System;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using CWC.AutoTests.Core;

namespace CWC.AutoTests.Pages.CashPoint
{
    public class ErrorsPerCashPointPage : ViewPage
    {
        public ErrorsPerCashPointPage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/CashPoint/Frames/ErrorsPerCashPoint.aspx";
        }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_gridWorstPerformingMachinesCtrl_lblHeaderCtrl")]
        public IWebElement ErrorsPerCashPointHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_btSearchCtrl_B")]
        public override IWebElement SearchBtn { get; set; }
        
        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_gridWorstPerformingMachinesCtrl_GridDataViewCtrl_PagerBarB_FirstButton_B")]
        public override IWebElement FirstBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_gridWorstPerformingMachinesCtrl_GridDataViewCtrl_PagerBarB_PrevButton_B")]
        public override IWebElement PrevBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_gridWorstPerformingMachinesCtrl_GridDataViewCtrl_PagerBarB_NextButton_B")]
        public override IWebElement NextBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_gridWorstPerformingMachinesCtrl_GridDataViewCtrl_PagerBarB_LastButton_B")]
        public override IWebElement LastBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_gridWorstPerformingMachinesCtrl_GridDataViewCtrl_LPV")]
        public override IWebElement LoadingPopUp { get; set; }
    }
}