using System;
using System.Collections.Generic;
using System.Threading;
using CWC.AutoTests.Pages;
using CWC.AutoTests.Core;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace CWC.AutoTests.Helpers
{
    public class FormHelper<T> where T : BasePage
    {
        protected T page;
        protected WebDriverWait wait;

        // flag for checking whether Default Page is successfully opened on closing form
        public bool isDefaultPageOpened = true;

        public FormHelper(T page)
        {
            this.page = page;
            this.wait = DriverHelper.GetWaiter(page.Driver);
        }

        /// <summary>
        /// Close form
        /// </summary>
        public void CloseForm()
        {
            var homePage = this.NavigateToHomePage();
            Thread.Sleep(2000); // TODO - in case test will fail with such wait time, "WaitUntilElementIsDisplayed" method should be used instead
            if (!homePage.IsVisible(homePage.WelcomeLbl))
            {
                isDefaultPageOpened = false;
                return;
            }
            return;
        }

        /// <summary>
        /// Helper method for closing form
        /// </summary>
        /// <returns>Instance of Default page</returns>
        private HomePage NavigateToHomePage()
        {
            page.CloseBtn.Click();
            return new HomePage(page.Driver);
        }
    }
}
