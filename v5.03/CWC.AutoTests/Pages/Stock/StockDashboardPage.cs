using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.Pages.Stock
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class StockDashboardPage : ViewPage
    {
        public StockDashboardPage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/CashCenter/StockManagement/Frames/StockDashboardFrame.aspx";
        }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_callbackPanel1_wgtVerifiedStock_pnlCallbackVerifiedStockCtrl_lblHeaderCtrl")]
        public IWebElement StockDashboardHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_gridSockContainersViewCtrl_GridDataViewCtrl_LPV")]
        public override IWebElement LoadingPopUp { get; set; }
    }
}
