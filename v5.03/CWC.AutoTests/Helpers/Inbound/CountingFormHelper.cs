using Cwc.FeedingsEncryptor;
using CWC.AutoTests.Core;
using CWC.AutoTests.Pages.Inbound;
using Cwc.BaseData;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CWC.AutoTests.Helpers.Inbound
{
    public class CountingFormHelper : FormHelper<CountingPage>
    {        
        public bool isMinimized = true;
        public bool isPrefilled = true;                

        public CountingFormHelper(CountingPage page) : base(page)
        {            
        }

        /// <summary>
        /// Consecutive counting of Deposit Numbers list
        /// </summary>
        /// <param name="depositNumberList">List of Deposit Numbers for consecutive counting</param>
        /// <param name="shouldBeMinimized">Flag to check that form should be minimized</param>
        public void CountDeposits(List<string> depositNumberList, bool shouldBeMinimized)
        {
            if (page.IsVisible(page.LocationFrom) && shouldBeMinimized)
            {
                isMinimized = false;
                return;
            }

            string locationFromValue = Configuration.LocationFrom;
            var depositInProcessing = CheckDepositInProcessing();

            if (depositInProcessing == null)
            {

                foreach (string item in depositNumberList)
                {
                    page.ScanDeposit(item);                    
                    CommonHelper.WaitUntilElementIsFilled(page.DepositInProcessing);
                    /*if (!page.ErrorMsg.Displayed && !page.IsVisible(page.DeclaredValueTotal))
                    {
                        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Cancel Count')]")));
                    }*/
                    if (page.IsVisible(page.EditBtn))
                    {
                        page.EditBtn.Click();
                        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Cancel Count')]")));
                        if (page.LocationFrom.GetAttribute("value") != locationFromValue)
                        {
                            isPrefilled = false;                            
                        }
                        page.Unfit100Eur.SendKeys("10");
                        page.CompleteCountBtn.Click();
                        page.OpenPage<CountingPage>();
                    }

                    else
                    {
                        string initialValue = page.LocationFrom.GetAttribute("value");
                        ContainerProcessingHelper.InputLocation(page.LocationFrom, Configuration.LocationCode);                        
                        CommonHelper.WaitUntilValueIsChanged(page.LocationFrom, initialValue);

                        if (page.IsVisible(page.Ref1) && page.Ref1.Enabled)
                        {
                            page.Ref1.SendKeys(item);
                        }

                        if (page.IsVisible(page.Ref2) && page.Ref2.Enabled)
                        {
                            page.Ref1.SendKeys(Keys.Tab);
                            page.Ref2.SendKeys(item);
                        }

                        if (page.IsVisible(page.AutoId) && page.AutoId.Enabled)
                        {
                            page.Ref2.SendKeys(Keys.Tab);
                            page.AutoId.SendKeys(item);
                        }

                        if (page.IsVisible(page.TillId) && page.TillId.Enabled)
                        {
                            page.AutoId.SendKeys(Keys.Tab);
                            page.TillId.SendKeys(item);
                        }
                        
                        page.Unfit100Eur.SendKeys("10");
                        page.CompleteCountBtn.Click();
                        page.OpenPage<CountingPage>();
                    }
                }
            }

            else
            {
                page.ScanDeposit(depositInProcessing);
                System.Threading.Thread.Sleep(1500);
                if (page.IsVisible(page.EditBtn))
                {
                    page.EditBtn.Click();
                }

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Cancel Count')]")));
                if (page.LocationFrom.GetAttribute("value") != locationFromValue)
                {
                    ContainerProcessingHelper.InputLocation(page.LocationFrom, Configuration.LocationCode);
                    System.Threading.Thread.Sleep(2000);
                }

                if (page.IsVisible(page.Ref1) && page.Ref1.Enabled)
                {
                    page.Ref1.SendKeys(depositInProcessing);
                }

                if (page.IsVisible(page.Ref2) && page.Ref2.Enabled)
                {
                    page.Ref1.SendKeys(Keys.Tab);
                    page.Ref2.SendKeys(depositInProcessing);
                }

                if (page.IsVisible(page.AutoId) && page.AutoId.Enabled)
                {
                    page.Ref2.SendKeys(Keys.Tab);
                    page.AutoId.SendKeys(depositInProcessing);
                }

                if (page.IsVisible(page.TillId) && page.TillId.Enabled)
                {
                    page.AutoId.SendKeys(Keys.Tab);
                    page.TillId.SendKeys(depositInProcessing);
                }

                page.Unfit100Eur.SendKeys("10");
                page.CompleteCountBtn.Click();
                page.OpenPage<CountingPage>();
                if (page.IsVisible(page.LocationFrom) && shouldBeMinimized)
                {
                    isMinimized = false;
                    return;
                }

                foreach (string item in depositNumberList)
                {
                    page.ScanDeposit(item);
                    //System.Threading.Thread.Sleep(1500);
                    CommonHelper.WaitUntilElementIsFilled(page.DepositInProcessing);
                    /*if (!page.ErrorMsg.Displayed && !page.IsVisible(page.DeclaredValueTotal))
                    {
                        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'" + item + "')]")));
                    }*/
                    if (page.IsVisible(page.EditBtn))
                    {
                        page.EditBtn.Click();
                        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Cancel Count')]")));
                        if (page.LocationFrom.GetAttribute("value") != locationFromValue)
                        {
                            isPrefilled = false;
                            ContainerProcessingHelper.InputLocation(page.LocationFrom, Configuration.LocationCode);
                        }

                        page.Unfit100Eur.SendKeys("10");
                        page.CompleteCountBtn.Click();
                        page.OpenPage<CountingPage>();
                    }

                    else
                    {
                        isPrefilled = false;
                        ContainerProcessingHelper.InputLocation(page.LocationFrom, Configuration.LocationCode);
                        //System.Threading.Thread.Sleep(2000);
                        CommonHelper.WaitUntilElementIsFilled(page.LocationFrom);

                        if (page.IsVisible(page.Ref1) && page.Ref1.Enabled)
                        {
                            page.Ref1.SendKeys(item);
                        }

                        if (page.IsVisible(page.Ref2) && page.Ref2.Enabled)
                        {
                            page.Ref1.SendKeys(Keys.Tab);
                            page.Ref2.SendKeys(item);
                        }

                        if (page.IsVisible(page.AutoId) && page.AutoId.Enabled)
                        {
                            page.Ref2.SendKeys(Keys.Tab);
                            page.AutoId.SendKeys(item);
                        }

                        if (page.IsVisible(page.TillId) && page.TillId.Enabled)
                        {
                            page.AutoId.SendKeys(Keys.Tab);
                            page.TillId.SendKeys(item);
                        }

                        page.Unfit100Eur.SendKeys("10");
                        page.CompleteCountBtn.Click();
                        page.OpenPage<CountingPage>();
                        if (page.IsVisible(page.LocationFrom) && shouldBeMinimized)
                        {
                            isMinimized = false;
                            return;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Consecutive counting of mother deposits with inner deposits
        /// </summary>
        /// <param name="preannouncements"> Multidimensional array of Deposit Numbers for consecutive counting </param>
        /// <param name="shouldBeMinimized"> Flag to check that form should be minimized</param>
        public void CountDeposits(string[,] preannouncements, bool shouldBeMinimized)
        {
            int i, j;

            if (page.IsVisible(page.LocationFrom) && shouldBeMinimized)
            {
                isMinimized = false;
                return;
            }

            string locationFromValue = Configuration.LocationFrom;            

            for (i = 0; i < preannouncements.GetLength(0); i++)               
            {
                page.ScanDeposit(preannouncements[i, 0]);
                CommonHelper.WaitUntilElementIsFilled(page.DepositInProcessing);                
            //    if (!page.ErrorMsg.Displayed && !page.IsVisible(page.DeclaredValueTotal))
            //    {
            //        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Cancel Count')]")));
            //    }
                if (page.IsVisible(page.EditBtn))
                {
                    page.EditBtn.Click();
                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Cancel Count')]")));
                    if (page.LocationFrom.GetAttribute("value") != locationFromValue)
                    {
                        isPrefilled = false;
                        return;
                    }

                    FillReferences(preannouncements[i, 0]);
                    if (page.IsVisible(page.CountInnerItemsBtn) && page.CountInnerItemsBtn.Enabled)
                    {
                        page.CountInnerItemsBtn.Click();
                    }
                    else
                    {
                        throw new Exception("Count Inner Items button is disabled! Please check site settings.");
                    }

                    for (j = 1; j < preannouncements.GetLength(1); j++)
                    {
                        CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                        page.ScanDeposit(preannouncements[i, j]);
                        CommonHelper.WaitUntilElementIsFilled(page.DepositInProcessing);
                        if (page.IsVisible(page.EditBtn))
                        {
                            page.EditBtn.Click();
                            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Cancel Count')]")));
                            if (page.LocationFrom.GetAttribute("value") != locationFromValue)
                            {
                                isPrefilled = false;
                                return;
                            }

                            FillReferences(preannouncements[i, j]);
                            page.Unfit100Eur.SendKeys("10");
                            page.CompleteCountBtn.Click();                            
                        }

                        else
                        {
                            isPrefilled = false;
                            return;
                        }
                    }

                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("lblDeclarednnerItems")));
                    page.Unfit100Eur.SendKeys("10");
                    page.CompleteCountBtn.Click();
                    CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                }

                else
                {
                    ContainerProcessingHelper.InputLocation(page.LocationFrom, Configuration.LocationCode);
                    System.Threading.Thread.Sleep(2000);                    
                    FillReferences(preannouncements[i, 0]);
                    page.CountInnerItemsBtn.Click();
                    for (j = 1; j < preannouncements.GetLength(1); j++)
                    {
                        CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                        page.ScanDeposit(preannouncements[i, j]);
                        CommonHelper.WaitUntilElementIsFilled(page.DepositInProcessing);
                        /*if (page.IsVisible(page.EditBtn))
                        {
                            page.EditBtn.Click();
                            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Cancel Count')]")));
                            if (page.LocationFrom.GetAttribute("value") != locationFromValue)
                            {
                                isPrefilled = false;
                                return;
                            }*/
                        FillReferences(preannouncements[i, j]);
                        page.Unfit100Eur.SendKeys("10");
                        page.CompleteCountBtn.Click();
                        CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                        /*}

                        else
                        {
                            isPrefilled = false;
                            return;
                        }*/
                    }

                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("lblDeclarednnerItems")));
                    page.Unfit100Eur.SendKeys("10");
                    page.CompleteCountBtn.Click();
                    CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                }
            }
        }

        /// <summary>
        /// Count all deposits on Mother Counting form
        /// </summary>
        /// <param name="preannouncements">Array of deposit numbers to scan</param>
        /// <param name="shouldBeMinimized">Flag to check that form should be minimized</param>
        /// <param name="announced">Flag whether pre-announcement exists</param>
        /// <param name="machineCounting">Flag whether deposit should be counted with machine count results or manually</param>
        /// <param name="fastCounting">Flag whether depsosit should be completed on minimized form or in Edit mode</param>
        public void CountAllDepositsOnMotherForm(string[,] preannouncements, bool shouldBeMinimized, bool announced, bool machineCounting, bool fastCounting = false)
        {
            int i;
            string locationFromValue = Configuration.LocationFrom;
            string[] depositNumberList = new string[preannouncements.GetLength(0) * preannouncements.GetLength(1)];
            depositNumberList = ContainerProcessingHelper.MultiArrayToSingleArray(preannouncements);

            if (page.IsVisible(page.LocationFrom) && shouldBeMinimized)
            {
                isMinimized = false;
                return;
            }

            for (i = 0; i < depositNumberList.GetLength(0); i++)
            {
                page.ScanDeposit(depositNumberList[i]);
                CommonHelper.WaitUntilElementIsFilled(page.DepositInProcessing);
                
                // failure on activating edit mode
                if (shouldBeMinimized && page.IsDisabled(page.EditBtn) && !page.IsVisible(page.LocationFrom))
                {
                    throw new Exception("Edit button is not clickable! :(");
                }
                // manual counting in edit mode
                if (shouldBeMinimized && !fastCounting && !machineCounting)
                {
                    page.EditBtn.Click();
                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Cancel Count')]")));
                    if (announced && page.LocationFrom.GetAttribute("value") != locationFromValue)
                    {
                        isPrefilled = false;
                        return;
                    }

                    /*if (page.IsVisible(page.Ref1) && page.Ref1.Enabled)
                    {
                        page.Ref1.SendKeys(depositNumberList[i]);
                    }

                    if (page.IsVisible(page.Ref2) && page.Ref2.Enabled)
                    {
                        page.Ref1.SendKeys(Keys.Tab);
                        page.Ref2.SendKeys(depositNumberList[i]);
                    }

                    if (page.IsVisible(page.AutoId) && page.AutoId.Enabled)
                    {
                        page.Ref2.SendKeys(Keys.Tab);
                        page.AutoId.SendKeys(depositNumberList[i]);
                    }

                    if (page.IsVisible(page.TillId) && page.TillId.Enabled)
                    {
                        page.AutoId.SendKeys(Keys.Tab);
                        page.TillId.SendKeys(DateTime.Now.ToString("ddMMyyyy"));
                    }*/
                    FillReferences(depositNumberList[i]);
                    if (!page.IsDisabled(page.Unfit100Eur))
                    {
                        page.Unfit100Eur.SendKeys("10");
                        page.CompleteCountBtn.Click();
                        CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                    }
                    else
                    {
                        throw new Exception("Unfit qualification type is disabled! Please check site settings.");
                    }
                }                
                // manual counting on minimized form (zero count results)
                else if (shouldBeMinimized && fastCounting && !machineCounting)
                {
                    // TODO -- discrepancy case
                    if (announced)
                    {
                        throw new Exception("Discrepancy case is not supported yet!");
                    }
                    else
                    {
                        page.CompleteCountBtn.Click();
                        CommonHelper.WaitUntilElementIsDisplayed(page.ZeroCountResultsOKBtn);
                        page.ZeroCountResultsOKBtn.Click();
                        //TODO <!-- if (!declaredValue) --> if Declared Value is not mandatory, deposit should be counted successfully ()
                        //{
                        CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                        //}
                        //else
                        //{
                        // TODO -- wait until pop up concerning mandatory declared value is displayed 
                        //}
                    }
                }
                // machine counting on minimized form
                else if (shouldBeMinimized && fastCounting && machineCounting)
                {
                    SendCountingFeeding(Configuration.Workstation);
                    page.CompleteCountBtn.Click();
                    CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                }
                // machine counting in edit mode
                else if (shouldBeMinimized && !fastCounting && machineCounting)
                {
                    page.EditBtn.Click();
                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Cancel Count')]")));
                    if (announced && page.LocationFrom.GetAttribute("value") != locationFromValue)
                    {
                        isPrefilled = false;
                        return;
                    }

                    FillReferences(depositNumberList[i]);
                    page.StartCountBtn.Click();
                    SendCountingFeeding(Configuration.Workstation);
                    page.CompleteCountBtn.Click();
                    CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                }
                // machine counting on regular Counting form
                else if (!shouldBeMinimized && !fastCounting && machineCounting)
                {
                    if (announced && page.LocationFrom.GetAttribute("value") != locationFromValue)
                    {
                        isPrefilled = false;
                        return;
                    }

                    page.StartCountBtn.Click();
                    SendCountingFeeding(Configuration.Workstation);
                    page.CompleteCountBtn.Click();
                    CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                }
                // manual counting on regular Counting form
                else
                {
                    ContainerProcessingHelper.InputLocation(page.LocationFrom, Configuration.LocationCode);
                    System.Threading.Thread.Sleep(2000);
                    FillReferences(depositNumberList[i]);
                    //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("lblDeclarednnerItems")));
                    page.Unfit100Eur.SendKeys("10");
                    page.CompleteCountBtn.Click();
                    CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
                }
            }
        }

        /// <summary>
        /// Consecutive counting of single deposits with submitting of machine count results
        /// </summary>
        /// <param name="depositNumberList"> List of Deposit Numbers for consecutive counting </param>
        /// <param name="shouldBeMinimized"> Flag to check that form should be minimized </param>
        public void MachineCountDeposits(List<string> depositNumberList, bool shouldBeMinimized)
        {
            if (page.IsVisible(page.LocationFrom) && shouldBeMinimized)
            {
                isMinimized = false;
                return;
            }

            string locationFromValue = Configuration.LocationFrom;
            
            foreach (string item in depositNumberList)
            {
                page.ScanDeposit(item);
                CommonHelper.WaitUntilElementIsFilled(page.DepositInProcessing);
                SendCountingFeeding(Configuration.Workstation);
                page.CompleteCountBtn.Click();
                CommonHelper.WaitUntilElementIsEmpty(page.DepositNumber);
            }            
        }
        
        /// <summary>
        /// Check whether there is deposit in processing on Counting form
        /// </summary>
        /// <returns> Deposit Number </returns>
        private string CheckDepositInProcessing()
        {
            string depositInProcessing = String.IsNullOrEmpty(page.DepositInProcessing.GetAttribute("value")) ? null : page.DepositInProcessing.GetAttribute("value");

            if (depositInProcessing != null)
            {
                return depositInProcessing = GetDepositNumber(depositInProcessing);
            }

            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get Number of Deposit in processing to complete its counting
        /// </summary>
        /// <param name="depositInProcessing"> Displayed string in "Deposit in processing" field </param>
        /// <returns> Deposit Number </returns>
        private string GetDepositNumber(string depositInProcessing)
        {
            int pos = depositInProcessing.IndexOf("/");
            int len = depositInProcessing.Length;

            if (pos > 0)
            {
                depositInProcessing = depositInProcessing.Remove(pos - 1, len - pos + 1);
            }
            
            return depositInProcessing;
        }

        private void SendCountingFeeding(string workstationName/*, out string response, out bool warning, out string errorMsg, out string responseHash*/)
        {
            var xml = string.Format(@"<?xml version='1.0' encoding='utf-8'?>" +
                                    "<MachineCountResult xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                                    @"<CountResultDataSetLines>                           
                                    <MachineCountResultLine>
                                     <Currency>EUR</Currency>
                                     <Denomination>100.00</Denomination>
                                     <MaterialTypeCode>NOTE</MaterialTypeCode>
                                     <QualificationType>Fit</QualificationType>
                                     <Quantity>10</Quantity>
                                    </MachineCountResultLine>
                                    </CountResultDataSetLines>
                                    <CountingMachineType>Numeron</CountingMachineType>
                                    <EndDate>{1}T14:00:05</EndDate>
                                    <HeaderCardNumber />
                                    <StartDate>{1}T14:00:05</StartDate>
                                    <StockTransactionType>MachineCountResult</StockTransactionType>
                                    <WorkstationName>{0}</WorkstationName>
                                    <MachineNumber></MachineNumber>
                                    </MachineCountResult>", workstationName, DateTime.Now.ToString("yyyy-MM-dd"));
            this.SendFeeding(xml/*, out response, out warning, out errorMsg, out responseHash*/);
        }

        private void SendFeeding(string content/*, out string response, out bool warning, out string errorMsg, out string responseHash*/)
        {
            var login = "admin";
            var password = "sa";

            System.Security.Cryptography.SHA1Managed sha1 = new System.Security.Cryptography.SHA1Managed();
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(password);
            byte[] hash = sha1.ComputeHash(bytes);
            password = Convert.ToBase64String(hash);

            string sendPassword = FeedingsEncryptorFacade.UniqueInstance.GetHash64(content);

            content = FeedingsEncryptorFacade.UniqueInstance.GetEncryptedXML(content, login, password);
            string sendHash = FeedingsEncryptorFacade.UniqueInstance.GetHash64(content);
            string sendContent = content;

            using (var service = new WebServiceFeedings.WebServiceFeedingsSoapClient())
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var x = service.SendFeedingAsync(login, sendPassword, sendHash, content/*, out warning, out errorMsg, out response, out responseHash*/);
                Debug.WriteLine("Thread id: {0}. Elapsed: {1}", Thread.CurrentThread.ManagedThreadId, stopwatch.ElapsedMilliseconds);                
            }
        }        

        public bool CheckCountingFormInitialState()
        {
            bool state = true;

            if (!page.IsDisabled(page.CompleteCountBtn) || 
                !page.IsDisabled(page.EditBtn))
            {
                return state = false;
            }

            return state;
        }

        private void FillReferences(string reference1, string reference2 = "", string autoId = "")
        {
            if (!page.IsDisabled(page.Ref1))
            {
                page.Ref1.SendKeys(reference1);
            }

            if (!page.IsDisabled(page.Ref2))
            {
                page.Ref1.SendKeys(Keys.Tab);
                if (reference2 == "")
                {
                    page.Ref2.SendKeys(reference1);
                }
                else
                {
                    page.Ref2.SendKeys(reference2);
                }
            }

            if (!page.IsDisabled(page.AutoId))
            {
                page.Ref2.SendKeys(Keys.Tab);
                if (autoId == "")
                {
                    page.Ref2.SendKeys(reference1);
                }
                else
                {
                    page.AutoId.SendKeys(autoId);
                }
            }

            if (!page.IsDisabled(page.TillId))
            {
                page.AutoId.SendKeys(Keys.Tab);
                page.TillId.SendKeys(DateTime.Now.ToString("ddMMyyyy"));
            }
        }

        public void WaitForElement(IWebElement eleemnt, int seconds)
        {
            var wait = new WebDriverWait(page.Driver, new TimeSpan(0,0,seconds));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id(@"ctl00_ctl00_cphCR_cphFM_plMain_LPV")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id(@"ctl00_ctl00_cphCR_cphFM_plMain_LPV")));
        }

    }
}
