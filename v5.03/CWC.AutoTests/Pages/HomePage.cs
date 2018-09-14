using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using CWC.AutoTests.Core;

namespace CWC.AutoTests.Pages
{
    public class HomePage : BasePage
    {
        [FindsBy(How = How.Id, Using = "ctl00_lblUserName")]
        public IWebElement OperatorLbl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_cphCR_lblUserName2")]
        public IWebElement WelcomeLbl { get; set; }
 
        public HomePage(IWebDriver driver) : base(driver)
        {            
        }
    }
}
