using System;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using CWC.AutoTests.Core;

namespace CWC.AutoTests.Pages.CashPoint
{
    public class StockValuePage : ViewPage
    {
        public StockValuePage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/CashPoint/Frames/StockValueFrame.aspx";
        }              

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_gridStockValueItems_gridCtrl_lblHeaderCtrl")]
        public IWebElement StockValueHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_AT1")]
        public IWebElement GraphTab { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_btUpdate_B")]
        public override IWebElement SearchBtn { get; set; }
        
        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_gridDailyStockValueCtrl_GridDataViewCtrl_PagerBarB_FirstButton_B")]
        public override IWebElement FirstBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_gridDailyStockValueCtrl_GridDataViewCtrl_PagerBarB_PrevButton_B")]
        public override IWebElement PrevBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_gridDailyStockValueCtrl_GridDataViewCtrl_PagerBarB_NextButton_B")]
        public override IWebElement NextBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_gridDailyStockValueCtrl_GridDataViewCtrl_PagerBarB_LastButton_B")]
        public override IWebElement LastBtn { get; set; }        

        [FindsBy(How = How.Id, Using = "ctl00_cphTopMenu_searchCtrl_txtCoinMachineId_I")]
        public IWebElement CashPointNumber { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_pcMain_gridDailyStockValueCtrl_GridDataViewCtrl_LPV")]
        public override IWebElement LoadingPopUp { get; set; }
    }
}
