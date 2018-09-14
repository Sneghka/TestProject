using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;

namespace CWC.AutoTests.Core
{
    public class DriverHelper
    {
        public static WebDriverWait GetWaiter(IWebDriver driver)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        public static IWebDriver InitDriver(string browser)
        {
            switch (browser)
            {
                case "Firefox":
                    {
                        //Environment.SetEnvironmentVariable("webdriver.gecko.driver", @"C:\TFS\Test\v5.03\geckodriver.exe");                       
                        //FirefoxProfile profile = new FirefoxProfile();
                        //profile.SetPreference("browser.startup.homepage", "about:blank");
                        //profile.SetPreference("startup.homepage_welcome_url", "about:blank");
                        //profile.SetPreference("startup.homepage_welcome_url.additional", "about:blank");                      
                        //FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(@"C:\", "geckodriver.exe");
                        //service.FirefoxBinaryPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
                        var driver = new FirefoxDriver();                        
                        SetPortalUrl(driver);
                        return driver;
                    }

                case "IE":
                    {
                        var driver = new InternetExplorerDriver();                        
                        SetPortalUrl(driver);
                        return driver;
                    }

                default:
                    {
                        throw new ArgumentException(String.Format("{0} is not currently supported.", browser), "browser");
                    }
            }
        }

        // Set Webdriver Url to Portal Url
        public static void SetPortalUrl(IWebDriver driver)
        {
            driver.Url = Configuration.Portal;
        }

        public static void DisposeDriver(IWebDriver driver)
        {
            driver.Quit();
        }
    }
}
