using CWC.AutoTests.Core;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SpecFlow.Automation.UI.Pages;
using SpecFlow.Automation.UI.Resources;
using Xunit;

namespace SpecFlow.Automation.UI.Steps
{    
    [Binding]
    public class SmokeFormsTestsSteps
    {
        private IWebDriver driver;
        private BaseCreateNewInstancePage createNewInstancePage;
        private BaseInstancePage instancePage;

        public SmokeFormsTestsSteps()
        {
            driver = FeatureContext.Current.Get<IWebDriver>();            
        }

        [Given(@"I open ""(.*)"" page")]        
        public void WhenIOpenPage(string pageName)
        {
            instancePage = new BaseInstancePage();
            PageFactory.InitElements(driver, instancePage);
            driver.Navigate().GoToUrl($"{Configuration.Portal}{PagesDictionary.page[pageName]}");
            ScenarioContext.Current.Add("pageName", pageName);
        }

        [When(@"I click create button")]
        public void WhenIClickCreateButton()
        {
            instancePage.CreateBtn.Click();
        }

        [When(@"I click new button")]
        public void WhenIClickNewButton()
        {
            instancePage.NewBtn.Click();
        }

        [When(@"I click add image")]
        public void WhenIClickAddImage()
        {
            instancePage.AddImg.Click();
        }

        [When(@"I confirm copy all settings")]
        public void WhenIConfirmCopyAllSettings()
        {
            instancePage.WaitYesBtnLoad(driver);
            instancePage.YesBtn.Click();
        }

        [Then(@"Form with correct title is opened")]
        public void ThenFormWithCorrectTitleIsOpened()
        {
            createNewInstancePage = new BaseCreateNewInstancePage();
            PageFactory.InitElements(driver, createNewInstancePage);
            createNewInstancePage.WaitFormLoad(driver);
            var pageName = ScenarioContext.Current.Get<string>("pageName");
            var formName = createNewInstancePage.CreateFormName.Text;
            // delete last symbols because of In the dictionary pages names are in the plural, and the form name is in a single
            pageName = pageName.Remove(pageName.Length - 3).ToLower().Replace(" ", string.Empty).Replace("s", string.Empty);
            formName = formName.Remove(formName.Length - 4).ToLower().Replace(" ", string.Empty).Replace("s", string.Empty);
            Assert.True(pageName.Contains(formName), formName + " form isn't opened");
        }

        [Then(@"I see a form is opened successul")]
        public void ThenISeeAFormIsOpenedSuccessul()
        {
            createNewInstancePage = new BaseCreateNewInstancePage();
            PageFactory.InitElements(driver, createNewInstancePage);
            createNewInstancePage.WaitFormLoad(driver);
            Assert.True(createNewInstancePage.CreateFormName.Displayed, "Form isn't opened");
        }

        [Then(@"I see add row is shown")]
        public void ThenISeeAddRowIsShown()
        {
            instancePage.WaitAddRowLoad(driver);
            Assert.True(instancePage.AddRow.Displayed, "AddRow isn't shown");
        }
    }
}
