using System;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;

namespace CWC.AutoTests.Pages.Accounting
{
    public class OrderPreAnnouncementsPage : ViewPage
    {
        public OrderPreAnnouncementsPage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/Settlement/Frames/OrderPreannouncementsFrame.aspx";
        }        

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridOrdersCtrl_lblHeaderCtrl")]
        public IWebElement OrderPreAnnouncementsHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_btSearchCtrl_B")]
        public override IWebElement SearchBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridOrdersCtrl_GridDataViewCtrl_PagerBarB_FirstButton_B")]
        public override IWebElement FirstBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridOrdersCtrl_GridDataViewCtrl_PagerBarB_PrevButton_B")]
        public override IWebElement PrevBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridOrdersCtrl_GridDataViewCtrl_PagerBarB_NextButton_B")]
        public override IWebElement NextBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridOrdersCtrl_GridDataViewCtrl_PagerBarB_LastButton_B")]
        public override IWebElement LastBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridOrdersCtrl_GridDataViewCtrl_LPV")]
        public override IWebElement LoadingPopUp { get; set; }
    }
}
