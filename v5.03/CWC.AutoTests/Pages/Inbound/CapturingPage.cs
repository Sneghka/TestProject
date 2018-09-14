using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using CWC.AutoTests.Core;

namespace CWC.AutoTests.Pages.Inbound
{
    public class CapturingPage : BasePage
    {
        public CapturingPage(IWebDriver driver) : base(driver)
        {
            this.PageUrl += "Solutions/CashCenter/Forms/CaptureDepositForm.aspx";
        }
        // URL of Capturing Form
        // "Remove (0, 2)" removes "~/" symbols from received URL
        // post build event - copy /y "$(SolutionDir)WebSite\App_Data\ActionsConfiguration.xml"  "$(TargetDir)App_Data\ActionsConfiguration.xml"
        //public string pageUrl = BasePage.baseUrl + "Solutions/CashCenter/Forms/CaptureDepositForm.aspx"; //ActionsHelper.GetAction(typeof(CreateNewDepositG4SBE)).URL.Remove(0, 2);
                
        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_lblHeaderFormCtrl")]
        public IWebElement CapturingHeader { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_lblRegistrationDateCtrl")]
        public IWebElement DateLbl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_cmbxCapturingModeCtrl_B-1")]
        public IWebElement CapturingModeCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_txbxBatchNumberCtrl_I")]
        public IWebElement BatchNumberCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_cmbxCapturingModeCtrl_DDD_L_LBI0T0")]
        public IWebElement TwoStepCountOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_cmbxCapturingModeCtrl_DDD_L_LBI1T0")]
        public IWebElement OneStepCountOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_cmbxCapturingModeCtrl_DDD_L_LBI2T0")]
        public IWebElement NoBatchOption { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_cmbxStockOwnerCtrl_cmBxLookupCtrl_I")]
        public IWebElement StockOwnerCtrl { get; set; }
                                        
        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_txBxDepositNumberCtrl_I")]
        public IWebElement DepositNumber { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_txbxInProcessingCtrl_I")]
        public IWebElement DepositInProcessingCtrl { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_cmbxLocationFromCtrl_cmBxLookupCtrl_cmBxLookupCtrl_I")]
        public IWebElement LocationFromCtrl { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@class = 'editableField ng-pristine ng-valid'][@tabindex = '13']")]
        public IWebElement DeclaredValueTotal { get; set; }

        public string DeclaredValueTotalXpath = "//input[@class = 'editableField ng-pristine ng-valid'][@tabindex = '13']";
                
        public string ScanFirstRow = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_tabCtrl_gridBarcodeScanHistory_DXDataRow0";
       
        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_btConfirmAndNextOnClientCtrl_B")]
        public IWebElement ConfirmNextBtn { get; set; }
                                        
        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_btDeclareInnerItemsOnClientCtrl_B")]
        public IWebElement DeclareInnerItemsBtn { get; set; }
                                        
        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_txBxCustomerReference1Ctrl_I")]
        public IWebElement Ref1 { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_txBxCustomerReference2Ctrl_I")]
        public IWebElement Ref2 { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_txBxAutomateIdCtrl_I")]
        public IWebElement AutoId { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_txBxTillIdCtrl_I")]
        public IWebElement TillId { get; set; }
                
        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_btnBack_B")]
        public override IWebElement CloseBtn { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_btnCloseBatchCtrl_B")]
        public IWebElement CloseBatchBtn { get; set; }
        // not currently used
        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_LPV")]
        public IWebElement LoadingDialog { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_msgFormCtrl_HCB-1Img")]
        public IWebElement ErrorMsg { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ctl00_cphCR_cphFM_pnlCallbackMainCtrl_msgFormCtrl_btMsgCloseCtrl_CD")]
        public IWebElement ErrorMsgCloseBtn { get; set; }               

        /// <summary>
        /// Scan deposit for capturing
        /// </summary>
        /// <param name="containerNumber"> Deposit Number </param>
        public void ScanDeposit(string containerNumber)
        {
            this.DepositNumber.SendKeys(containerNumber);
            this.DepositNumber.SendKeys(Keys.Enter);
        }
    }
}
