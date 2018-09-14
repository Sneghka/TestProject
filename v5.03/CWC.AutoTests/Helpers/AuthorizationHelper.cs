using CWC.AutoTests.Core;
using CWC.AutoTests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CWC.AutoTests.Helpers
{
    public class AuthorizationHelper
    {
        
        /// <summary>
        /// Log in to portal
        /// </summary>
        /// <returns> Home page</returns>
        public static HomePage LogInToPortal(IWebDriver driver, string login, string password, string workstation)
        {                        
            var loginPage = new LoginPage(driver);
            var wait = DriverHelper.GetWaiter(loginPage.Driver);
            loginPage.Username.SendKeys(login);
            loginPage.Username.SendKeys(Keys.Tab);
            loginPage.Driver.SwitchTo().ActiveElement().SendKeys(password);
            loginPage.Driver.SwitchTo().ActiveElement().SendKeys(Keys.Tab);
            loginPage.Driver.SwitchTo().ActiveElement().SendKeys(workstation);
            loginPage.LoginBtn.Click();
            wait.Until(ExpectedConditions.UrlContains("Default.aspx?Params="));
            return new HomePage(loginPage.Driver);
        }        

        public void SetPermission(string permission)
        {
            // TO DO
        }
    }
}
