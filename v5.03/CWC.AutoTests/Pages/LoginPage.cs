using CWC.AutoTests.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace CWC.AutoTests.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        [FindsBy(How = How.Id, Using = "ctl00_lgnMain_txBxLogin_I")]
        public IWebElement Username { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[contains(@class, 'dxbButton')]")]
        public IWebElement LoginBtn { get; set; }
    }
}
