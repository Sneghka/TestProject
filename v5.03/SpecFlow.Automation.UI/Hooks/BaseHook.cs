using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;
using CWC.AutoTests.Core;
using CWC.AutoTests.Helpers;

namespace SpecFlow.Automation.UI.Hooks
{
    [Binding]
    public class BaseHook
    {
        //[BeforeScenario]
        //public static void InitDriver()
        //{
        //    IWebDriver driver = new FirefoxDriver();
        //    ScenarioContext.Current.Set(driver);
        //}

        //[AfterScenario]
        //public static void QuitDriver()
        //{
        //    var driver = ScenarioContext.Current.Get<IWebDriver>();
        //    driver.Quit();
        //}

        [BeforeFeature]
        public static void InitDriver()
        {
            IWebDriver driver = DriverHelper.InitDriver(Configuration.Browser);
            FeatureContext.Current.Set(driver);
            var homePage = AuthorizationHelper.LogInToPortal(driver, Configuration.Username, Configuration.Password, Configuration.Workstation);
        }

        [AfterFeature]
        public static void QuitDriver()
        {
            var driver = FeatureContext.Current.Get<IWebDriver>();
            driver.Quit();
        }
    }
}
