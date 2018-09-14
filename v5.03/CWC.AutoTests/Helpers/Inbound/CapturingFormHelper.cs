using Cwc.CashCenter;
using CWC.AutoTests.Core;
using CWC.AutoTests.Pages.Inbound;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace CWC.AutoTests.Helpers.Inbound
{
    public class CapturingFormHelper : FormHelper<CapturingPage>
    {
        // expected Capturing Form mode
        public bool isFastway = true;

        // Error message is displayed
        public bool isErrorDisplayed = false;

        // flag for checking whether Location From is pre-selected on the Capturing form
        public bool isLocationPreselected = true;

        public CapturingFormHelper(CapturingPage page) : base(page)
        {            
        }

        /// <summary>
        /// Consecutive capturing of Deposit Numbers list
        /// </summary>
        /// <param name="depositNumberList"> List of Deposit Numbers for consecutive capturing </param>
        /// <param name="capturingMode"> Capturing mode: 0 = no batch, 1 = one-step count, 2 = two-step count </param>
        /// <param name="waitForMe"> Flag whether barcodes should be scanned fast or delay should be made until previous barcode is captured </param>
        public void CaptureDeposits(List<string> depositNumberList, CapturingMode capturingMode, bool waitForMe)
        {
            // string value of Location From field
            string locationFromValue = Configuration.LocationFrom;

            if (capturingMode == CapturingMode.OneStepCount)
            {
                page.CapturingModeCtrl.Click();
                CommonHelper.WaitUntilElementIsDisplayed(page.NoBatchOption);
                wait.Until(ExpectedConditions.ElementToBeClickable(page.OneStepCountOption));
                page.OneStepCountOption.Click();
            }

            if (capturingMode == CapturingMode.TwoStepCount)
            {
                page.CapturingModeCtrl.Click();
                CommonHelper.WaitUntilElementIsDisplayed(page.NoBatchOption);
                wait.Until(ExpectedConditions.ElementToBeClickable(page.TwoStepCountOption));
                page.TwoStepCountOption.Click();
            }

            if (capturingMode == CapturingMode.NoBatch)
            {
                page.CapturingModeCtrl.Click();
                CommonHelper.WaitUntilElementIsDisplayed(page.NoBatchOption);
                wait.Until(ExpectedConditions.ElementToBeClickable(page.NoBatchOption));
                page.NoBatchOption.Click();
            }

            if (waitForMe)
            {
                foreach (string item in depositNumberList)
                {                    
                    page.ScanDeposit(item);                    
                    CommonHelper.WaitUntilElementIsFilled(page.DepositNumber);
                    if (!page.ErrorMsg.Displayed && !page.IsVisible(page.DeclaredValueTotal))
                    {
                        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'" + item + "')]")));
                    }

                    else if (!page.ErrorMsg.Displayed && page.DeclaredValueTotal.Displayed)
                    {
                        isFastway = false;
                        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'" + item + "')]")));
                        if (page.DeclaredValueTotal.Enabled)
                        {
                            page.DeclaredValueTotal.SendKeys("1000");
                        }

                        page.ConfirmNextBtn.Click();
                        CommonHelper.WaitUntilElementIsNotDisplayed(page.DepositNumber);
                        CommonHelper.WaitUntilElementIsDisplayed(page.DepositNumber);
                        //page.OpenPage<CapturingPage>();
                    }

                    else
                    {
                        isFastway = false;
                        isErrorDisplayed = true;
                        page.ErrorMsgCloseBtn.Click();
                        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'" + item + "')]")));
                        if (page.LocationFromCtrl.GetAttribute("value") != locationFromValue)
                        {
                            ContainerProcessingHelper.InputLocation(page.LocationFromCtrl, Configuration.LocationCode);
                            System.Threading.Thread.Sleep(2000);
                        }

                        // Let's check that references fields are enabled and fill them
                        // only mandatory if there was any error on current form 
                        // that may or may not be connected with mandatory references fields
                        // but better be safe than sorry
                        if (page.Ref1.Enabled)
                        {
                            page.Ref1.SendKeys(item);
                        }

                        if (page.Ref2.Enabled)
                        {
                            page.Ref1.SendKeys(Keys.Tab);
                            page.Ref2.SendKeys(item);
                        }

                        if (page.AutoId.Enabled)
                        {
                            page.Ref2.SendKeys(Keys.Tab);
                            page.AutoId.SendKeys(item);
                        }

                        if (page.TillId.Enabled)
                        {
                            page.AutoId.SendKeys(Keys.Tab);
                            page.TillId.SendKeys(item);
                        }

                        if (page.DeclaredValueTotal.Enabled)
                        {
                            page.DeclaredValueTotal.SendKeys("1000");
                        }

                        IJavaScriptExecutor js = page.Driver as IJavaScriptExecutor;
                        js.ExecuteScript("arguments[0].click();", page.ConfirmNextBtn);
                        page.OpenPage<CapturingPage>();
                    }
                }
            }

            else
            {
                foreach (string item in depositNumberList)
                {                    
                    page.ScanDeposit(item);                    
                }
                //wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'" + item + "')]")));
                System.Threading.Thread.Sleep(5000);
            }

        }

        public void CaptureDeposits(string[,] preannouncements, CapturingMode capturingMode, bool electronic)
        {
            int i, j;

            // string value of Location From field
            string locationFromValue = Configuration.LocationFrom;

            // executor of javasript commands, basically for clicking buttons that are not accessible by native selenium methods
            IJavaScriptExecutor js = page.Driver as IJavaScriptExecutor;

            if (capturingMode == CapturingMode.OneStepCount)
            {
                page.CapturingModeCtrl.Click();
                CommonHelper.WaitUntilElementIsDisplayed(page.NoBatchOption);
                wait.Until(ExpectedConditions.ElementToBeClickable(page.OneStepCountOption));
                page.OneStepCountOption.Click();
            }

            if (capturingMode == CapturingMode.TwoStepCount)
            {
                page.CapturingModeCtrl.Click();
                CommonHelper.WaitUntilElementIsDisplayed(page.NoBatchOption);
                wait.Until(ExpectedConditions.ElementToBeClickable(page.TwoStepCountOption));
                page.TwoStepCountOption.Click();
            }

            if (capturingMode == CapturingMode.NoBatch)
            {
                page.CapturingModeCtrl.Click();
                CommonHelper.WaitUntilElementIsDisplayed(page.NoBatchOption);                              
                wait.Until(ExpectedConditions.ElementToBeClickable(page.NoBatchOption));
                page.NoBatchOption.Click();                
            }

            for (i = 0; i < preannouncements.GetLength(0); i++)
            {                
                page.ScanDeposit(preannouncements[i, 0]);
                //System.Threading.Thread.Sleep(1000);
                CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                if (!page.IsVisible(page.ErrorMsg) && !page.IsVisible(page.DeclaredValueTotal))
                {
                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'" + preannouncements[i, 0] + "')]")));
                }
                
                // this scenario is to be tested
                else if (!page.IsVisible(page.ErrorMsg) && page.IsVisible(page.DeclaredValueTotal))
                {
                    isFastway = false;
                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'" + preannouncements[i, 0] + "')]")));
                    page.DeclaredValueTotal.SendKeys("1000");
                    page.DeclareInnerItemsBtn.Click();                    
                }                
                else
                {                    
                    page.ErrorMsgCloseBtn.Click();
                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'" + preannouncements[i, 0] + "')]")));                    
                    page.DeclaredValueTotal.SendKeys("1000");                    
                    js.ExecuteScript("arguments[0].click();", page.DeclareInnerItemsBtn);                    
                }

                for (j = 1; j < preannouncements.GetLength(1); j++)
                {
                    page.ScanDeposit(preannouncements[i, j]);
                    
                    if (!electronic)
                    {
                        isFastway = false;
                        CommonHelper.WaitUntilElementIsFilled(page.DepositInProcessingCtrl);
                        if (!page.IsVisible(page.ErrorMsg) && page.IsVisible(page.DeclaredValueTotal))
                        {
                            if (page.LocationFromCtrl.GetAttribute("value") != locationFromValue)
                            {
                                isLocationPreselected = false;
                                ContainerProcessingHelper.InputLocation(page.LocationFromCtrl, Configuration.LocationCode);                                
                                CommonHelper.WaitUntilElementIsFilled(page.LocationFromCtrl);
                                page.DeclaredValueTotal.SendKeys("1000");
                                page.ConfirmNextBtn.Click();
                                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(page.ScanFirstRow)));
                                System.Threading.Thread.Sleep(2000);
                            }
                            else
                            {
                                page.DeclaredValueTotal.SendKeys("1000");
                                page.ConfirmNextBtn.Click();
                                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(page.ScanFirstRow)));                                
                                CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                            }
                        }
                        else if (page.IsVisible(page.ErrorMsg))
                        {                            
                            isErrorDisplayed = true;
                            page.ErrorMsgCloseBtn.Click();
                            if (page.LocationFromCtrl.GetAttribute("value") != locationFromValue)
                            {
                                isLocationPreselected = false;
                                ContainerProcessingHelper.InputLocation(page.LocationFromCtrl, Configuration.LocationCode);
                                System.Threading.Thread.Sleep(2000);
                                page.DeclaredValueTotal.SendKeys("1000");
                                js.ExecuteScript("arguments[0].click();", page.ConfirmNextBtn);
                                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(page.ScanFirstRow)));
                                System.Threading.Thread.Sleep(2000);
                            }
                            else
                            {
                                page.DeclaredValueTotal.SendKeys("1000");
                                js.ExecuteScript("arguments[0].click();", page.ConfirmNextBtn);
                                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(page.ScanFirstRow)));
                                System.Threading.Thread.Sleep(2000);
                            }
                        }
                    }                    
                }
                                
                //wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(page.DeclaredValueTotalXpath)));               
                //page.DeclaredValueTotal.SendKeys("1000");
                //js.ExecuteScript("arguments[0].click();", page.ConfirmNextBtn);
                //System.Threading.Thread.Sleep(3000);
                CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);                
            }
        }

        /// <summary>
        /// Check state of particular controls on Capturing form while Batch is in progress
        /// </summary>
        /// <returns> Success or failure </returns>
        public bool CheckUIWithBatchInProgress()
        {
            bool state = true;

            if (String.IsNullOrEmpty(page.BatchNumberCtrl.GetAttribute("value")) || 
                !page.IsDisabled(page.StockOwnerCtrl) || page.IsDisabled(page.CloseBatchBtn))
            {
                return state = false;
            }
            
            return state;
        }

        /// <summary>
        /// Close Batch after testing of capturing in one-step count or two-step count mode is completed
        /// </summary>
        /// <returns> New instance of Capturing page </returns>
        public CapturingPage CloseBatch()
        {
            page.CloseBatchBtn.Click();
            return new CapturingPage(page.Driver);
        }
    }
}
