using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Reflection;
using OpenQA.Selenium.Support.UI;

namespace CWC.AutoTests.Core
{
    public class BasePage
    {
        public IWebDriver Driver { get; set; }
        private string pageUrl;        

        // Portal link used for construction of links for custom pages        
        protected string PageUrl
        {
            get
            {
                if (pageUrl == null)
                {
                    pageUrl = Configuration.Portal;
                }
                return pageUrl;
            }
            set
            {
                pageUrl = value;
            }
        } 
        
        public BasePage(IWebDriver driver)
        {
            this.Driver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "ctl00_lblPerformanceCounter")]
        public virtual IWebElement PerformanceCounter { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_lblPerformanceCounter")]
        public virtual IWebElement CloseBtn { get; set; }

        /// <summary>
        /// Open required page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns> Instance of opened page </returns>
        public T OpenPage<T>() where T : BasePage
        {
            T instance = Activator.CreateInstance(typeof(T), new object[] { Driver }) as T;            
            instance.Driver.Navigate().GoToUrl(instance.GetType().GetProperty("PageUrl", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance).ToString());
            instance.Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);                      
            return instance;
        }

        /// <summary>
        /// Check that required element is displayed on page
        /// </summary>
        /// <param name="element"> Element that should be displayed on page </param>
        /// <returns> Visibility flag </returns>
        public bool IsVisible(IWebElement element)
        {
            bool visible = true;

            try
            {
                if (element.Displayed)
                {                    
                    return visible;
                }
                else
                {
                    return visible = false;
                }                
            }
            catch (NoSuchElementException)
            {
                return visible = false;
            }
        }

        public bool IsDisabled(IWebElement element)
        {
            bool disabled = true;
            try
            {
                if (element.GetAttribute("class").Contains("dxbDisabled") ||
                    element.GetAttribute("class").Contains("dxeDisabled"))
                {
                    return disabled;
                }
                else
                {
                    return disabled = false;
                }
            }
            catch (NoSuchElementException)
            {
                return disabled;
            }
        }
    }
}
