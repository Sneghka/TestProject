using System;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using CWC.AutoTests.Core;

namespace CWC.AutoTests.Pages.CashPoint
{
    public class CashPointMonitorPage : ViewPage
    {
        public CashPointMonitorPage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/CashPoint/Frames/CashPointsMonitorFrame.aspx";
        }
                                       
        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridCoinMachinesCtrl_lblHeaderCtrl")]
        public IWebElement TotalsHeader { get; set; }
                                        
        [FindsBy(How = How.Id, Using = "ctl00_cphCR_panel_ASPxPageControl1_AT5T")]
        public IWebElement TransactionsTab { get; set; }
       
        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_btSearchCtrl_B")]
        public override IWebElement SearchBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_txtNumber_I")]
        public IWebElement CashPointNumber { get; set; }
                                        
        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_ecmbStatus_I")]
        public IWebElement CashPointStatusCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridCoinMachinesCtrl_GridDataViewCtrl_LPV")]
        public override IWebElement LoadingPopUp { get; set; }
    }
}
