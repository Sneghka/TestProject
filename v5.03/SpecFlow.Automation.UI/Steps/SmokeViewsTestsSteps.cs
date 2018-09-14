using CWC.AutoTests.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecFlow.Automation.UI.Resources;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace SpecFlow.Automation.UI.Steps
{
    [Binding]
    public class SmokeViewsTestsSteps
    {
        private readonly IWebDriver driver;

        public SmokeViewsTestsSteps()
        {
            driver = FeatureContext.Current.Get<IWebDriver>();            
        }
                
        [When(@"I open ""(.*)"" page")]
        public void WhenIOpenPage(string pageName)
        {
            driver.Navigate().GoToUrl($"{Configuration.Portal}{PagesDictionary.page[pageName]}");
            ScenarioContext.Current.Add("pageName", pageName);
        }

        [Then(@"I see page is opened successul")]
        public void ThenISeePageIsOpenedSuccessul()
        {
            var pageName = ScenarioContext.Current.Get<string>("pageName");
            Assert.True(pageName.EndsWith(driver.Title), pageName + " page isn't opened");
        }

        private void WaitForPageLoad()
        {
            var waitForDocumentReady = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            waitForDocumentReady.Until((wdriver) => (driver as IJavaScriptExecutor).ExecuteScript("return document.readyState").Equals("complete"));
        }
    }
}
