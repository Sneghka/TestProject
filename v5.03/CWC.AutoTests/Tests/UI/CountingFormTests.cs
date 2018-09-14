using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using CWC.AutoTests.Pages.Inbound;
using CWC.AutoTests.Helpers.Inbound;
using CWC.AutoTests.Core;
using OpenQA.Selenium;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Enums;

namespace CWC.AutoTests.Tests
{
    [Collection("Test Collection #2")]
    public class CountingFormTests : BaseTest, IClassFixture<TestFixture>
    {
        TestFixture fixture;

        public CountingFormTests(TestFixture setupClass)
        {
            // Add code to be executed before every test
            this.fixture = setupClass;
        }
        
        /// <summary>
        /// Counting form can be opened
        /// </summary>
        [Fact(DisplayName = "Open Counting Form")]
        public void OpenCountingForm()
        {
            //authorizationHelper.SetPermission("Manage Base Data");
            CountingPage countingPage = fixture.HomePage.OpenPage<CountingPage>();            
            Assert.True(countingPage.CountingHeader.Displayed);
        }

        /// <summary>
        /// Count single deposits without pre-announcement on minimized form
        /// </summary>
        /// <remarks>
        /// 'Declared Value' is 'false'
        /// Expected behaviour:
        /// Form should be minimized on loading and after counting is completed
        /// Deposits have 'counted' status when testcase is completed
        /// </remarks>         
        [Fact(DisplayName = "Count Single Deposits without Preannouncements (Minimized)")]
        public void CountSingleDepositsWithoutPreannouncementsMinimized()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {
                {"IsSkipCountResultsEditing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {
                {"IsDeclaredValueMandatory", false}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            List<string> depositNumberList = new List<string>();
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();            
            ContainerProcessingHelper.ClearProcessingSession(Configuration.Workstation);
            CountingPage countingPage = fixture.HomePage.OpenPage<CountingPage>();
            CountingFormHelper countingFormHelper = new CountingFormHelper(countingPage);
            depositNumberList = dataGenerationHelper.GenerateDepositNumberList(2, Configuration.LocationCode);
            countingFormHelper.CountDeposits(depositNumberList, true);
            Assert.True(countingFormHelper.isMinimized, "Deposit is processed not in expected way!");
            f = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Counting, depositNumberList);
            Assert.True(f, "Some of counted deposits have invalid status.");
        }

        /// <summary>
        /// Count single deposits with pre-announcements on minimized form
        /// </summary>
        /// <remarks>
        /// 'Declared Value' is 'false'
        /// Expected behaviour:
        /// Form should be minimized on loading and after counting is completed
        /// Form should be pre-filled with container information 
        /// Deposits have 'counted' status when testcase is completed
        /// </remarks>         
        [Fact(DisplayName = "Count Single Deposits with Preannouncements (Minimized)")]
        public void CountSingleDepositsWithPreannouncementsMinimized()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {
                {"IsSkipCountResultsEditing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {
                {"IsDeclaredValueMandatory", false}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            List<string> depositNumberList = new List<string>();
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();
            ContainerProcessingHelper.ClearProcessingSession(Configuration.Workstation);
            CountingPage countingPage = fixture.HomePage.OpenPage<CountingPage>();
            CountingFormHelper countingFormHelper = new CountingFormHelper(countingPage);
            depositNumberList = dataGenerationHelper.GenerateElectronicPreannouncements(2, Configuration.LocationCode);
            countingFormHelper.CountDeposits(depositNumberList, true);
            Assert.True(countingFormHelper.isMinimized, "Deposit is processed not in expected way!");
            Assert.True(countingFormHelper.isPrefilled, "Deposit information should be pre-filled!");
            f = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Counting, depositNumberList);
            Assert.True(f, "Some of counted deposits have invalid status.");
        }

        /// <summary>
        /// Count mother and inner deposits without pre-announcements on minimized form
        /// </summary>
        /// <remarks>
        /// 'Declared Value' is 'false'
        /// Expected behaviour:
        /// Deposit is processed in full edit mode
        /// Deposit has 'counted' status when testcase is completed
        /// </remarks>
        [Fact(DisplayName = "Count Mother and Inner Deposits without Electronic Preannouncements (Minimized)")]
        public void CountMotherAndInnerDepositsWithoutElectronicPreannouncementsMinimized()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {
                {"IsSkipCountResultsEditing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {
                {"IsDeclaredValueMandatory", false}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            string[,] preannouncements;
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();            
            ContainerProcessingHelper.ClearProcessingSession(Configuration.Workstation);
            CountingPage countingPage = fixture.HomePage.OpenPage<CountingPage>();
            CountingFormHelper countingFormHelper = new CountingFormHelper(countingPage);
            preannouncements = dataGenerationHelper.GenerateTransportPreannouncements(2, 2, Configuration.LocationCode, false);
            countingFormHelper.CountDeposits(preannouncements, true);
            Assert.True(countingFormHelper.isMinimized, "Deposit is processed not in expected way!");
            Assert.True(countingFormHelper.isPrefilled, "Deposit information should be pre-filled!");
            f = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Counting, preannouncements);
            Assert.True(f, "Some of counted deposits have invalid status.");
        }

        /// <summary>
        /// Count mother and inner deposits without pre-announcements on minimized form
        /// </summary>
        /// <remarks>
        /// 'Declared Value' is 'false'
        /// Expected behaviour:
        /// Deposit is processed in full edit mode
        /// Deposit has 'counted' status when testcase is completed
        /// </remarks>
        [Fact(DisplayName = "Count Mother and Inner Deposits with Electronic Preannouncements (Minimized)")]
        public void CountMotherAndInnerDepositsWithElectronicPreannouncementsMinimized()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {
                {"IsSkipCountResultsEditing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {
                {"IsDeclaredValueMandatory", false}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            string[,] preannouncements;
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();            
            ContainerProcessingHelper.ClearProcessingSession(Configuration.Workstation);
            CountingPage countingPage = fixture.HomePage.OpenPage<CountingPage>();
            CountingFormHelper countingFormHelper = new CountingFormHelper(countingPage);
            preannouncements = dataGenerationHelper.GenerateTransportPreannouncements(2, 2, Configuration.LocationCode, true);
            countingFormHelper.CountDeposits(preannouncements, true);
            Assert.True(countingFormHelper.isMinimized, "Deposit is processed not in expected way!");
            Assert.True(countingFormHelper.isPrefilled, "Deposit information should be pre-filled!");
            f = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Counting, preannouncements);
            Assert.True(f, "Some of counted deposits have invalid status.");
        }

        /// <summary>
        /// Count single deposits with pre-announcements on minimized form
        /// </summary>
        /// <remarks>
        /// 'Declared Value' is 'false'
        /// Expected behaviour:
        /// Form should be minimized on loading and after counting is completed
        /// Form should be pre-filled with container information 
        /// Deposits have 'counted' status when testcase is completed
        /// </remarks>         
        [Fact(DisplayName = "Machine Count Single Deposits with Preannouncements (Minimized)")]
        public void MachineCountSingleDepositsWithPreannouncementsMinimized()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {
                {"IsSkipCountResultsEditing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {
                {"IsDeclaredValueMandatory", false}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            List<string> depositNumberList = new List<string>();
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();            
            ContainerProcessingHelper.ClearProcessingSession(Configuration.Workstation);
            CountingPage countingPage = fixture.HomePage.OpenPage<CountingPage>();
            CountingFormHelper countingFormHelper = new CountingFormHelper(countingPage);
            depositNumberList = dataGenerationHelper.GenerateElectronicPreannouncements(3, Configuration.LocationCode);
            countingFormHelper.MachineCountDeposits(depositNumberList, true);
            Assert.True(countingFormHelper.isMinimized, "Deposit is processed not in expected way!");
            Assert.True(countingFormHelper.isPrefilled, "Deposit information should be pre-filled!");
            f = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Counting, depositNumberList);
            Assert.True(f, "Some of counted deposits have invalid status.");
        }

        /// <summary>
        /// Execute fast way capturing and minimized counting of single deposits with pre-announcements
        /// </summary>
        /// <remarks>
        /// 'Declared Value' is 'true'
        /// Expected behaviour:
        /// Fast way capturing is completed correctly
        /// Counting form should be minimized on loading and after counting is completed        
        /// Deposits have 'counted' status when testcase is completed
        /// </remarks>         
        [Fact(DisplayName = "Capture and Count Single Deposits with Preannouncements (Minimized)")]
        public void CaptureAndCountSingleDepositsWithPreannouncementsMinimized()
        {
            bool capturingStatus;
            bool countingStatus;
            var siteSettings = new Dictionary<string, bool>()
            {
                {"IsSkipCountResultsEditing", true},
                {"IsFastWayCapturing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {
                {"IsDeclaredValueMandatory", true},
                {"IsDeclaredValueMandatoryCapturing", true},
                {"IsElectronicPreannouncement", true},
                {"IsAutomaticallyConfirmCapturing", true}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            List<string> depositNumberList = new List<string>();
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();
            ContainerProcessingHelper.ClearProcessingSession(Configuration.Workstation);            
            var capturingPage = fixture.HomePage.OpenPage<CapturingPage>();
            var capturingFormHelper = new CapturingFormHelper(capturingPage);
            depositNumberList = dataGenerationHelper.GenerateElectronicPreannouncements(1, Configuration.LocationCode);
            capturingFormHelper.CaptureDeposits(depositNumberList, 0, true);
            Assert.True(capturingFormHelper.isFastway, "Capturing is processed not in a fast way!");
            capturingStatus = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Capturing, depositNumberList);
            Assert.True(capturingStatus, "Some of captured deposits have invalid status.");            

            var countingPage = fixture.HomePage.OpenPage<CountingPage>();
            var countingFormHelper = new CountingFormHelper(countingPage);            
            countingFormHelper.CountDeposits(depositNumberList, true);
            Assert.True(countingFormHelper.isMinimized, "Deposit is processed not in expected way!");
            Assert.True(countingFormHelper.isPrefilled, "Deposit information should be pre-filled!");
            countingStatus = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Counting, depositNumberList);
            Assert.True(countingStatus, "Some of counted deposits have invalid status.");
            dataGenerationHelper.ClearList(depositNumberList);
        }

        /// <summary>
        /// Execute fast way capturing and minimized counting of mother and inner deposits with pre-announcements
        /// </summary>
        /// <remarks>
        /// 'Declared Value' is 'true'
        /// Expected behaviour:
        /// Fast way capturing is completed correctly
        /// Counting form should be minimized on loading and after counting is completed        
        /// Deposits have 'counted' status when testcase is completed
        /// </remarks>         
        [Fact(DisplayName = "Capture and Count Single Deposits with Preannouncements (Minimized)")]
        public void CaptureAndCountMotherDepositsWithPreannouncementsMinimized()
        {
            bool capturingStatus;
            bool countingStatus;
            string[,] preannouncements;
            var siteSettings = new Dictionary<string, bool>()
            {
                {"IsSkipCountResultsEditing", true},
                {"IsFastWayCapturing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {
                {"IsDeclaredValueMandatory", true},
                {"IsDeclaredValueMandatoryCapturing", true},
                {"IsElectronicPreannouncement", true},
                {"IsAutomaticallyConfirmCapturing", true}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            var dataGenerationHelper = new DataGenerationHelper();
            ContainerProcessingHelper.ClearProcessingSession(Configuration.Workstation);
            var capturingPage = fixture.HomePage.OpenPage<CapturingPage>();
            var capturingFormHelper = new CapturingFormHelper(capturingPage);
            preannouncements = dataGenerationHelper.GenerateTransportPreannouncements(1, 2, Configuration.LocationCode, true);
            capturingFormHelper.CaptureDeposits(preannouncements, 0, true);
            Assert.True(capturingFormHelper.isFastway, "Capturing is processed not in a fast way!");
            capturingStatus = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Capturing, preannouncements);
            Assert.True(capturingStatus, "Some of captured deposits have invalid status.");
            var countingPage = fixture.HomePage.OpenPage<CountingPage>();
            var countingFormHelper = new CountingFormHelper(countingPage);
            countingFormHelper.CountDeposits(preannouncements, true);
            Assert.True(countingFormHelper.isMinimized, "Deposit is processed not in expected way!");
            Assert.True(countingFormHelper.isPrefilled, "Deposit information should be pre-filled!");
            countingStatus = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Counting, preannouncements);
            Assert.True(countingStatus, "Some of counted deposits have invalid status.");            
        }

        [Fact(Skip = "not finished", DisplayName = "Count Inner Deposits on Mother form (Manual, Minimized)")]
        public void CountInnerDepositsOnMotherFormMinimized()
        {            
            bool countingStatus;
            string[,] preannouncements;
            var siteSettings = new Dictionary<string, bool>()
            {
                {"IsSkipCountResultsEditing", true},
                {"IsFastWayCapturing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {
                {"IsDeclaredValueMandatory", true},
                {"IsDeclaredValueMandatoryCapturing", true},
                {"IsElectronicPreannouncement", true},
                {"IsAutomaticallyConfirmCapturing", true},
                {"IsCountInnersDirectly", true},
                {"IsShowInnersCounting", true}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            var dataGenerationHelper = new DataGenerationHelper();            
            ContainerProcessingHelper.ClearProcessingSession(Configuration.Workstation);
            var countingPage = fixture.HomePage.OpenPage<CountingPage>();
            var countingFormHelper = new CountingFormHelper(countingPage);
            preannouncements = dataGenerationHelper.GenerateTransportPreannouncements(2, 2, Configuration.LocationCode, true);
            countingFormHelper.CountAllDepositsOnMotherForm(preannouncements, true, false, true);
            Assert.True(countingFormHelper.isMinimized, "Deposit is processed not in expected way!");
            countingStatus = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Counting, preannouncements);
            Assert.True(countingStatus, "Some of counted deposits have invalid status.");
        }

        [Fact(DisplayName = "Counting Date is current")]
        public void CheckCountingFormDate()
        {
            bool f;
            var countingPage = fixture.HomePage.OpenPage<CountingPage>();
            f = CommonHelper.CheckDateIsCurrent(countingPage.DateLbl);
            Assert.True(f, "Counting Date is not current!");
        }

        [Fact(DisplayName = "Counting form is closed correctly")]
        public void CloseCountingForm()
        {           
            var countingPage = fixture.HomePage.OpenPage<CountingPage>();
            var formHelper = new FormHelper<CountingPage>(countingPage);
            formHelper.CloseForm();
            Assert.True(formHelper.isDefaultPageOpened, "Counting form is not properly closed!");
        }
        
        [Fact(DisplayName = "Complete Count and Edit buttons are disabled when Counting form is opened")]
        public void CheckMinimizedCountingFormInitialState()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {
                {"IsSkipCountResultsEditing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {
                {"IsDeclaredValueMandatory", true}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            var countingPage = fixture.HomePage.OpenPage<CountingPage>();
            var countingFormHelper = new CountingFormHelper(countingPage);
            f = countingFormHelper.CheckCountingFormInitialState();            
            Assert.True(f, "Counting form is not properly closed!");
        }

        // helper for generating electronic pre-announcements without testing anything
        [Fact(Skip = "skip", DisplayName = "Generate Electronic Preannouncements")]
        public void GenerateElectronicPreannouncements()
        {            
            List<string> depositNumberList = new List<string>();
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();            
            depositNumberList = dataGenerationHelper.GenerateElectronicPreannouncements(1, Configuration.LocationCode);            
        }        
    }
}
