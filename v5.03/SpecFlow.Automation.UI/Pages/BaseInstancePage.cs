using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace SpecFlow.Automation.UI.Pages
{
    public class BaseInstancePage
    {
        [FindsBy(How = How.XPath, Using = "//span[contains(text(), 'Create')]")]
        public IWebElement CreateBtn;

        [FindsBy(How = How.XPath, Using = "//span[contains(text(), 'New')]")]
        public IWebElement NewBtn;

        [FindsBy(How=How.CssSelector, Using = "img[src*='AddAction.small.gif']")]
        public IWebElement AddImg;

        [FindsBy(How = How.CssSelector, Using = "tr.dxgvInlineEditRow")]
        public IWebElement AddRow;

        [FindsBy(How = How.CssSelector, Using = "div[id*='btnYes_CD']")]
        public IWebElement YesBtn;

        public void WaitAddRowLoad(IWebDriver driver)
        {               
            var waitForDocumentReady = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            waitForDocumentReady.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(("tr.dxgvInlineEditRow"))));
        }

        public void WaitYesBtnLoad(IWebDriver driver)
        {
            var waitForDocumentReady = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            waitForDocumentReady.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(("div[id*='btnYes_CD']"))));
        }
    }
}
