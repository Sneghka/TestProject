using System;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using CWC.AutoTests.Core;

namespace CWC.AutoTests.Pages.CashPoint
{
    public class EventPage : BasePage
    {
        public EventPage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/Coin/Forms/EventForm.aspx";
        }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_lblHeader2Ctrl")]
        public IWebElement EventHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_btnSaveAndNew_B")]
        public IWebElement SaveAndNewBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_btnSave_B")]
        public IWebElement SaveAndCloseBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_btnBack_B")]
        public IWebElement BackBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pageCtrl_txtName_I")]
        public IWebElement EventName { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pageCtrl_txtEventFactor_I")]
        public IWebElement EventFactor { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pageCtrl_dtStartDate_I")]
        public IWebElement StartDateCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pageCtrl_dtEndDate_I")]
        public IWebElement EndDateCtrl { get; set; }
    }
}
