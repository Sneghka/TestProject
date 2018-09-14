#define performance

using CWC.AutoTests.Core;
using CWC.AutoTests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.Threading;

namespace CWC.AutoTests.Helpers
{
    using System.Linq;

    using Xunit;

    public class ViewHelper<T> where T : BasePage 
    {
        private T page;
        private WebDriverWait wait;
        private string initialValue;
        public bool isPagingPresent = true;

        // performance indicators
        public string figure;
        public string figure1;
        public string figure2;
        public string figure3;
        public string figure4;

        public ViewHelper(T page)
        {
            this.page = page;
            this.wait = DriverHelper.GetWaiter(page.Driver);
        }

        #region NavigateViewPage method

        public void NavigateViewPage<T>(T page) where T : ViewPage
        {
            var timer = new Stopwatch();
            if (page.IsVisible(page.NextBtn) && page.NextBtn.Enabled)
            {
                page.NextBtn.Click();
                timer.Start();
                CommonHelper.WaitUntilLoadingIsCompleted(page.LoadingPopUp);
                timer.Stop();
                figure1 = string.Format("{0:F}", timer.Elapsed.TotalSeconds);
                try
                {
                    page.PrevBtn.Click();
                }
                catch (StaleElementReferenceException)
                {
                    Thread.Sleep(500);
                    page.PrevBtn.Click();
                    timer.Restart();
                    CommonHelper.WaitUntilLoadingIsCompleted(page.LoadingPopUp);
                    timer.Stop();
                    figure2 = string.Format("{0:F}", timer.Elapsed.TotalSeconds);
                }
                finally
                {
                    timer.Restart();
                    CommonHelper.WaitUntilLoadingIsCompleted(page.LoadingPopUp);
                    timer.Stop();
                    figure2 = string.Format("{0:F}", timer.Elapsed.TotalSeconds);
                }

                try
                {
                    page.LastBtn.Click();
                }
                catch (StaleElementReferenceException)
                {
                    Thread.Sleep(500);
                    page.LastBtn.Click();
                    timer.Restart();
                    CommonHelper.WaitUntilLoadingIsCompleted(page.LoadingPopUp);
                    timer.Stop();
                    figure3 = string.Format("{0:F}", timer.Elapsed.TotalSeconds);
                }
                finally
                {
                    timer.Restart();
                    CommonHelper.WaitUntilLoadingIsCompleted(page.LoadingPopUp);
                    timer.Stop();
                    figure3 = string.Format("{0:F}", timer.Elapsed.TotalSeconds);
                }

                try
                {
                    page.FirstBtn.Click();
                }
                catch (StaleElementReferenceException)
                {
                    Thread.Sleep(500);
                    page.FirstBtn.Click();
                    timer.Restart();
                    CommonHelper.WaitUntilLoadingIsCompleted(page.LoadingPopUp);
                    timer.Stop();
                    figure4 = string.Format("{0:F}", timer.Elapsed.TotalSeconds);
                }
                finally
                {
                    timer.Restart();
                    CommonHelper.WaitUntilLoadingIsCompleted(page.LoadingPopUp);
                    timer.Stop();
                    figure4 = string.Format("{0:F}", timer.Elapsed.TotalSeconds);
                }

                return;
            }
            else
            {
                isPagingPresent = false;
                return;
            }
        }

        #endregion

        #region ApplyFilterByDate method

        /// <summary>
        /// Apply filter by date
        /// </summary>
        /// <typeparam name="T"> T = generic type of Cash Point page inheriting default CashPointPage </typeparam>
        /// <param name="page"> Cash Point page that has date period control </param>
        /// <param name="type"> Date period type (receives "during" or "fromto" or "ignoredate" string) </param>
        /// <param name="subtype"> Date period subtype (receives "last" or "current" or "next" string) </param>
        /// <param name="number"> Number of intervals received as string</param>
        /// <param name="interval"> Date period interval (receives "days" or "weeks" or "months" string) </param>
        public void ApplyFilterByDate<T>(T page, string type, string subtype, string number, string interval) where T : ViewPage
        {
            page.DatePeriodTypeCtrl.Click();
            CommonHelper.WaitUntilElementIsDisplayed(page.FromToOption);
            switch (type)
            {
                case "during":
                    {
                        page.DuringOption.Click();
                        CommonHelper.WaitUntilElementIsDisplayed(page.DatePeriodSubTypeCtrl);
                        page.DatePeriodSubTypeCtrl.Click();
                        CommonHelper.WaitUntilElementIsDisplayed(page.NextOption);
                        switch (subtype)
                        {
                            case "last":
                                {
                                    page.LastOption.Click();
                                    break;
                                }

                            case "current":
                                {
                                    page.CurrentOption.Click();
                                    break;
                                }

                            case "next":
                                {
                                    page.NextOption.Click();
                                    break;
                                }
                        }                        
                        page.DatePeriodNumberCtrl.Clear();
                        page.DatePeriodNumberCtrl.SendKeys(number);
                        page.DatePeriodIntervalCtrl.Click();
                        CommonHelper.WaitUntilElementIsDisplayed(page.MonthsOption);
                        switch (interval)
                        {
                            case "days":
                                {
                                    page.DaysOption.Click();
                                    break;
                                }

                            case "weeks":
                                {
                                    page.WeeksOption.Click();
                                    break;
                                }

                            case "months":
                                {
                                    page.MonthsOption.Click();
                                    break;
                                }
                        }
                        break;
                    }

                // currently not supported
                case "fromto":
                    {
                        page.FromToOption.Click();
                        break;
                    }
                
                case "ignoredate":
                    {
                        page.IgnoreDateOption.Click();
                        break;
                    }
            }
            initialValue = CommonHelper.GetElementText((IWebElement)page.PerformanceCounter);
            try
            {
                page.SearchBtn.Click();
                var timer = Stopwatch.StartNew();
                CommonHelper.WaitUntilValueIsChanged((IWebElement)page.PerformanceCounter, initialValue);
                timer.Stop();
                figure = string.Format("{0:F}", timer.Elapsed.TotalSeconds);
                Thread.Sleep(1000);
            }
            catch (StaleElementReferenceException)
            {
                page.SearchBtn.Click();
                var timer = Stopwatch.StartNew();
                CommonHelper.WaitUntilValueIsChanged((IWebElement)page.PerformanceCounter, initialValue);
                timer.Stop();
                figure = string.Format("{0:F}", timer.Elapsed.TotalSeconds);
                Thread.Sleep(1000);
            }
            catch (Exception e)
            {
                PerformanceHelper.SaveException(e);
            }
        }

        #endregion

        //public void CheckGridViewFilteringRow<T>(T page) where T : ViewPage
        //{
        //    Assert.True(page.GroupByColumnRow.Displayed);
        //    foreach (var filterRowBtn in page.FilterRowBtnList)
        //    {
        //        try
        //        {
        //            filterRowBtn.Click();
        //            if (page.FilterPopupWindow.Displayed)
        //            {
        //                Driver.driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
        //                page.FilterPopupSelectedItem.Text.CompareTo("(ShowAll)");
        //            }
        //        }
        //        catch (StaleElementReferenceException e)
        //        {
        //            PerformanceHelper.SaveException(e);
        //            break;
        //        }
        //    }
        //}

        public void CheckGridViewFilteringRow<T>(T page) where T : ViewPage
        {
            Assert.True(page.GroupByColumnRow.Displayed);
            IWebElement[] filterRowBtns = page.FilterRowBtnList.ToArray();
            try
            {
                for (int i = 0; i < filterRowBtns.Length; i++)
                {                    
                    IWebElement[] filterRowBtns1 = page.FilterRowBtnList.ToArray();
                    if (filterRowBtns1[i].Displayed)
                    {
                        filterRowBtns1[i].Click();
                        if (page.FilterPopupWindow.Displayed)
                        {
                            Assert.True(page.FilterRowPopupSelectedItem.Text.Equals("(All)"));
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (StaleElementReferenceException e)
            {
                Debug.WriteLine("Exception: {0}", e);
                PerformanceHelper.SaveException(e);
            }
        }


        public void CheckGridViewHeaderSorting<T>(T page) where T : ViewPage
        {            
            IWebElement[] columnHeaders = page.ColumnHeaderList.ToArray();
            try
            {
                for (int i = 0; i < columnHeaders.Length; i++)
                {
                    IWebElement[] columnHeaders1 = page.ColumnHeaderList.ToArray();
                    Thread.Sleep(500);
                    if (columnHeaders1[i].Displayed)
                    {
                        Thread.Sleep(50);
                        if (columnHeaders1[i].Displayed && (columnHeaders1[i].Text != " " && columnHeaders1[i].Text != "#"))
                        {
                            columnHeaders1[i].Click();
                            IWebElement[] columnHeaders2 = page.ColumnHeaderList.ToArray();
                            Assert.True(page.ColumnHeaderSortUp.Displayed);
                            if (page.ColumnHeaderSortUp.Displayed || columnHeaders2[i].Text != " ")
                            {
                                page.ColumnHeaderSortUp.Click();
                                Assert.True(page.ColumnHeaderSortDown.Displayed);
                            }
                            else
                            {
                                i++;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (StaleElementReferenceException e)
            {
                Debug.WriteLine("Exception: {0}", e);
                PerformanceHelper.SaveException(e);
            }
        }

        public void CheckGridViewHeaderCss<T>(T page) where T : ViewPage
        {
            try
            {
                Assert.True(page.GridViewLabelHeader.GetCssValue("text-indent").Equals("10px"));
                Assert.True(page.GridViewLabelHeader.GetCssValue("font-weight").Equals("bold"));
                Assert.True(page.GridViewLabelHeader.GetCssValue("color").Equals("#FFF"));
                Assert.True(page.GridViewLabelHeader.GetCssValue("background-color").Equals("#3A3764"));
            }
            catch (Exception e)
            {
                PerformanceHelper.SaveException(e);
            }
        }

        public object ExecuteJavaScript(string javaScript, params object[] args)
        {
            Trace.WriteLine("Executes JavaScript", "Document");
            var javaScriptExecutor = (IJavaScriptExecutor)page.Driver;

            return javaScriptExecutor.ExecuteScript(javaScript, args);
        }
    }
}
