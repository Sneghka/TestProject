using CWC.AutoTests.Helpers;
using CWC.AutoTests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;

namespace CWC.AutoTests.Core
{
    public class TestFixture : IDisposable
    {
        public IWebDriver Driver { get; set; }
        public HomePage HomePage { get; set; }
        public TestFixture()
        {
            // this code will be run once in the beginning of the testing for all test classes inheriting this class
            this.Driver = DriverHelper.InitDriver(Configuration.Browser);
            //this.Driver = new PhantomJSDriver(@"D:\Programs\PhantomJS\phantomjs-2.1.1-windows\phantomjs-2.1.1-windows\bin");            
            this.HomePage = AuthorizationHelper.LogInToPortal(this.Driver, Configuration.Username, Configuration.Password, Configuration.Workstation);            
        }

        public void Dispose()
        {
            DriverHelper.DisposeDriver(HomePage.Driver);
        }
    }
}
