﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.Pages.Order
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class UserOrdersPage : ViewPage
    {
        public UserOrdersPage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/Ordering/Frames/OrdersFrame.aspx?UserOrders";
        }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_gridOrdersCtrl_lblHeaderCtrl")]
        public IWebElement UserOrdersHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_searchCtrl_btSearchCtrl_B")]
        public override IWebElement SearchBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_gridOrdersCtrl_GridDataViewCtrl_PagerBarB_FirstButton_B")]
        public override IWebElement FirstBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_gridOrdersCtrl_GridDataViewCtrl_PagerBarB_PrevButton_B")]
        public override IWebElement PrevBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_gridOrdersCtrl_GridDataViewCtrl_PagerBarB_NextButton_B")]
        public override IWebElement NextBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_gridOrdersCtrl_GridDataViewCtrl_PagerBarB_LastButton_B")]
        public override IWebElement LastBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_orderCtrl_gridOrdersCtrl_GridDataViewCtrl_LPV")]
        public override IWebElement LoadingPopUp { get; set; }
    }
}