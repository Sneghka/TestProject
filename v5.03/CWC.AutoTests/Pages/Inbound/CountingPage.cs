using CWC.AutoTests.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace CWC.AutoTests.Pages.Inbound
{
    public class CountingPage : BasePage
    {
        public CountingPage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/CashCenter/Forms/CountDepositForm.aspx";
        }

        //public string pageUrl = baseUrl + "Solutions/CashCenter/Forms/CountDepositForm.aspx";    // + ActionsHelper.GetAction(typeof(CountDepositGBE)).URL.Remove(0, 2);   

        [FindsBy(How = How.Id, Using = "lblHeaderFormCtrl")]
        public IWebElement CountingHeader { get; set; }

        [FindsBy(How = How.Id, Using = "lblLocationAndDateCtrl")]
        public IWebElement DateLbl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_plMain_txBxInProcessingCtrl_I")]
        public IWebElement DepositInProcessing { get; set; }

        [FindsBy(How = How.Id, Using = "txBxDepositNumberCtrl_I")]
        public IWebElement DepositNumber { get; set; }
                                        
        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_plMain_cmbxLocationFromCtrl_cmBxLookupCtrl_cmBxLookupCtrl_I")]
        public IWebElement LocationFrom { get; set; }

        [FindsBy(How = How.Id, Using = "txBxCustomerReference1Ctrl_I")]
        public IWebElement Ref1 { get; set; }

        [FindsBy(How = How.Id, Using = "txBxCustomerReference2Ctrl_I")]
        public IWebElement Ref2 { get; set; }

        [FindsBy(How = How.Id, Using = "txBxAutomateIdCtrl_I")]
        public IWebElement AutoId { get; set; }

        [FindsBy(How = How.Id, Using = "txBxTillIdCtrl_I")]
        public IWebElement TillId { get; set; }

        [FindsBy(How = How.Id, Using = "contentCtrl_gdTotals_DN0_CL1000000_I")]
        public IWebElement DeclaredValueTotal { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id, 'countCtrl_gdNotes_DN100.00000000_CL3')]")]
        public IWebElement Unfit100Eur { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_plMain_pnlContentCountData_countCtrl_btnCountInnerItemsCtrl_B")]
        public IWebElement CountInnerItemsBtn { get; set; }

        [FindsBy(How = How.Id, Using = "lblHeaderInnerPartFormCtrl")]
        public IWebElement InnerFormHeader { get; set; }

        [FindsBy(How = How.Id, Using = "btCompleteCountOnClientCtrl_B")]
        public IWebElement CompleteCountBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_plMain_btnEditCtrl_B")]
        public IWebElement EditBtn { get; set; }

        [FindsBy(How = How.Id, Using = "btCheckOnHoldCtrl_B")]
        public IWebElement OnHoldBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_plMain_popupMessageCtrl_PW-1")]
        public IWebElement ErrorMsgDlg { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_plMain_popupMessageCtrl_HCB-1")]
        public IWebElement ErrorMsgDlgClose { get; set; }

        [FindsBy(How = How.Id, Using = "btnBack_B")]
        public override IWebElement CloseBtn { get; set; }

        [FindsBy(How = How.Id, Using = "CountDepositFormGBE.ButtonOk")]
        public IWebElement ZeroCountResultsOKBtn { get; set; }

        [FindsBy(How = How.Id, Using = "CountDepositFormGBE.ButtonBack")]
        public IWebElement ZeroCountResultsBackBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_plMain_pnlContentCountData_countCtrl_btnStartCountCtrl_B")]
        public IWebElement StartCountBtn { get; set; }

        /// <summary>
        /// Scan Deposit for counting
        /// </summary>
        /// <param name="containerNumber"> Deposit Number </param>
        public void ScanDeposit(string containerNumber)
        {
            this.DepositNumber.SendKeys(containerNumber);
            this.DepositNumber.SendKeys(Keys.Enter);
        }
    }
}
