using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace SpecFlow.Automation.UI.Pages
{
    public class LoginPage
    {
        [FindsBy(How = How.Id, Using = "ctl00_lgnMain_txBxLogin_I")]
        public IWebElement UserNameField;

        [FindsBy(How = How.Id, Using = "ctl00_lgnMain_txBxPassword_I")]
        public IWebElement PasswordField;

        [FindsBy(How = How.Id, Using = "ctl00_lgnMain_txBxWorkstation_I")]
        public IWebElement WorkstationField;

        [FindsBy(How = How.Id, Using = "ctl00_lgnMain_btnLoginOk_B")]
        public IWebElement LoginBtn;
    }
}
