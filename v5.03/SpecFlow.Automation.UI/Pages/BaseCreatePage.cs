using System;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace SpecFlow.Automation.UI.Pages
{
    public class BaseCreateNewInstancePage
    {
        [FindsBy(How = How.CssSelector, Using = "*[class^=TableForm] td[class*='Header']")]
        public IWebElement CreateFormName;

        public void WaitFormLoad(IWebDriver driver)
        {
            var waitForDocumentReady = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            waitForDocumentReady.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(("*[class^=TableForm] td[class*='Header']"))));
        }         
    }
}