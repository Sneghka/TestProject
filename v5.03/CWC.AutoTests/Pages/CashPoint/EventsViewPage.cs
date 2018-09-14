using System;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using CWC.AutoTests.Core;

namespace CWC.AutoTests.Pages.CashPoint
{
    public class EventsViewPage : ViewPage
    {
        public EventsViewPage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/CashPoint/Frames/EventsFrame.aspx";
        }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridEventsCtrl_lblHeaderCtrl")]
        public IWebElement EventsHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_btnNew_B")]
        public IWebElement NewBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_btnDlete_B")]
        public IWebElement DeleteBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridEventsCtrl_GridDataViewCtrl_PagerBarB_FirstButton_B")]
        public override IWebElement FirstBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridEventsCtrl_GridDataViewCtrl_PagerBarB_PrevButton_B")]
        public override IWebElement PrevBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridEventsCtrl_GridDataViewCtrl_PagerBarB_NextButton_B")]
        public override IWebElement NextBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridEventsCtrl_GridDataViewCtrl_PagerBarB_LastButton_B")]
        public override IWebElement LastBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridEventsCtrl_GridDataViewCtrl_LPV")]
        public override IWebElement LoadingPopUp { get; set; }
    }
}
