using CWC.AutoTests.Helpers;
using CWC.AutoTests.Pages;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CWC.AutoTests.Core
{
    public class LoadTestFixture : IDisposable
    {
        public List<HomePage> homePages;

        public LoadTestFixture()
        {
            // this code will be run once in the beginning of the testing
            var userSessionsList = UserSessionHelper.GetUserSessionSettings();
            homePages = new List<HomePage>();

            foreach (var userSession in userSessionsList)
            {
                var driver = DriverHelper.InitDriver(Configuration.Browser);                
                var homePage = AuthorizationHelper.LogInToPortal(driver, userSession.Login, userSession.Password, userSession.Workstation);
                homePages.Add(homePage);
            }
        }

        /// <summary>
        /// Close browser after testing is completed
        /// </summary>
        public void Dispose()
        {
            foreach (var homePage in homePages)
            {
                DriverHelper.DisposeDriver(homePage.Driver);
            }
        }
    }
}
