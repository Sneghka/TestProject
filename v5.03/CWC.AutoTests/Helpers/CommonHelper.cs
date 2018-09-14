using CWC.AutoTests.Core;
using OpenQA.Selenium;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;

namespace CWC.AutoTests.Helpers
{
    public class CommonHelper
    {              
             public CommonHelper()
        {
        }

        public static bool CheckDateIsCurrent(IWebElement element)
        {
            bool result = true;
            DateTime currentDate = DateTime.Now;
            DateTime dateToCheck;
            string inputString = !String.IsNullOrEmpty(element.GetAttribute("value")) ? element.GetAttribute("value") : element.Text;
            var regex = new Regex(@"\d{2}\W\d{2}\W\d{4}");
            string[] formats = {"dd.MM.yyyy", "dd/MM/yyyy", "dd-MM-yyyy", "dd:MM:yyyy", "ddMMyyyy", "MM.dd.yyyy", "MM/dd/yyyy",
                                "MM-dd-yyyy", "MM:dd:yyyy", "MMddyyyy", "yyyy-MM-dd", "yyyy/MM/dd", "yyyy.MM.dd", "yyyy:MM:dd"};

            Match match = regex.Match(inputString);
            if (DateTime.TryParseExact(match.Value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateToCheck))
            {
                if (currentDate.Date == dateToCheck.Date)
                {
                    return result;
                }
                else
                {
                    return result = false;
                }
            }
            else return result = false;
        }

        public static void WaitUntilElementIsEmpty(IWebElement element, int waitTime = 10000)
        {
            try
            {
                while (element.GetAttribute("value") != "" && waitTime > 0)
                {
                    Thread.Sleep(100);
                    waitTime -= 100;
                }
            }

            catch (Exception)
            {
                return;
            }
        }

        public static void WaitUntilElementIsFilled(IWebElement element, int waitTime = 10000)
        {
            int timeLeft = waitTime;
            try
            {
                while (String.IsNullOrEmpty(element.GetAttribute("value")) && timeLeft > 0)
                {
                    Thread.Sleep(100);
                    timeLeft -= 100;
                }
            }

            catch (Exception)
            {
                return;
            }
        }

        public static void WaitUntilValueIsChanged(IWebElement element, string initialValue)
        {
            try
            {
                if (!String.IsNullOrEmpty(element.Text))
                {
                    while (initialValue.Equals(element.Text))
                    {
                        Thread.Sleep(50);
                        WaitUntilValueIsChanged(element, initialValue);
                    }
                }
                else
                {
                    while (initialValue.Equals(element.GetAttribute("value")))
                    {
                        Thread.Sleep(50);
                        WaitUntilValueIsChanged(element, initialValue);
                    }
                }
            }
            catch (NoSuchElementException)
            {
                Thread.Sleep(50);
                WaitUntilValueIsChanged(element, initialValue);
            }
        }

        public static void WaitUntilElementIsDisplayed(IWebElement element)
        {
            try
            {
                if (element.Displayed)
                {
                    return;
                }
            }

            catch (NoSuchElementException)
            {
                Thread.Sleep(50);
                WaitUntilElementIsDisplayed(element);
            }

            catch (StaleElementReferenceException)
            {
                Thread.Sleep(50);
                WaitUntilElementIsDisplayed(element);
            }
        }

        public static void WaitUntilElementIsNotDisplayed(IWebElement element, int waitTime = 10000)
        {
            try
            {
                //if (element.Displayed)
                //{
                //    Thread.Sleep(50);                    
                //    WaitUntilElementIsNotDisplayed(element);
                //}
                while (element.Displayed && waitTime > 0)
                {
                    Thread.Sleep(100);
                    waitTime -= 100;
                }
            }
            catch (NoSuchElementException)
            {
                return;
            }
            catch (StaleElementReferenceException)
            {
                return;
            }
        }

        public static void WaitUntilLoadingIsCompleted(IWebElement element)
        {
            WaitUntilElementIsDisplayed(element);
            WaitUntilElementIsNotDisplayed(element);
            return;
        }

        /// <summary>
        /// Get text string of element
        /// </summary>
        /// <param name="element"> Required element </param>
        /// <returns> Text of the element </returns>
        public static string GetElementText(IWebElement element)
        {
            if (!string.IsNullOrEmpty(element.Text))
            {
                return element.Text;
            }

            else
            {
                return element.GetAttribute("value");
            }
        }

        /// <summary>
        /// Validation if text is present on page
        /// </summary>
        /// <param name="text">Text to find</param>
        /// <returns>Result</returns>
        public static bool IsTextPresent(BasePage page, string text)
        {
            try
            {
                return page.Driver.FindElement(By.XPath("//*[contains(text(),'" + text + "')]")).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }        
    }
}
