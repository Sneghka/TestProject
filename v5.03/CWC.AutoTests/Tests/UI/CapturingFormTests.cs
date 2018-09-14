using Cwc.CashCenter;
using System;
using System.Collections.Generic;
using Xunit;
using CWC.AutoTests.Helpers.Inbound;
using CWC.AutoTests.Pages.Inbound;
using CWC.AutoTests.Core;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Enums;

namespace CWC.AutoTests.Tests
{
    [Collection("Test Collection #1")]
    public class CapturingFormTests : BaseTest, IClassFixture<TestFixture>
    {        
        TestFixture fixture;        

        public CapturingFormTests(TestFixture setupClass)
        {
            // Add code to be executed before every test
            this.fixture = setupClass;
        }

        /// <summary>
        /// Capturing form can be opened
        /// </summary> 
        [Fact(DisplayName = "UI - Open Capturing Form")]
        public void OpenCapturingForm()
        {
            CapturingPage capturingPage = fixture.HomePage.OpenPage<CapturingPage>();
            Assert.True(capturingPage.CapturingHeader.Displayed);           
        }
                
        /// <summary>
        /// Capturing can be made with following settings:
        /// Fast Way Capturing = 'yes', Declared Value = 'yes', Pre-announcement Expected = 'yes'
        /// Pre-announcements received:
        /// - Customer Electronic
        /// </summary>
        [Fact(DisplayName = "UI - Fast Way Capturing with checked Declared Value and Pre-announcement Expected flags")]
        public void FastWayCapturingWithCheckedDeclaredValueAndPreannouncementExpected()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {                
                {"IsFastWayCapturing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {                
                {"IsDeclaredValueMandatoryCapturing", true},
                {"IsElectronicPreannouncement", true},
                {"IsAutomaticallyConfirmCapturing", true}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            List<string> depositNumberList = new List<string>();
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();
            CapturingPage capturingPage = fixture.HomePage.OpenPage<CapturingPage>();
            CapturingFormHelper capturingFormHelper = new CapturingFormHelper(capturingPage);           
            //depositNumberList = dataGenerationHelper.GenerateElectronicPreannouncements(1, Configuration.LocationCode);
            depositNumberList.Add("test_" + DateTime.Now.ToString("ddMMyyyyhhmmss"));
            capturingFormHelper.CaptureDeposits(depositNumberList, CapturingMode.NoBatch, true);
            Assert.True(capturingFormHelper.isFastway, "Capturing is processed not in a fast way!");
            f = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Capturing, depositNumberList);
            Assert.True(f, "Some of captured deposits have invalid status.");
            dataGenerationHelper.ClearList(depositNumberList);            
        }         
        
        /// <summary>
        /// Capturing can be made with following settings:
        /// Fast Way Capturing = 'yes', Declared Value = 'yes', Pre-announcement Expected = 'no'
        /// Pre-announcements received:
        /// - Customer Electronic
        /// </summary>
        [Fact(DisplayName = "UI - Fast Way Capturing with checked Declared Value and unchecked Pre-announcement Expected flags")]
        public void FastWayCapturingWithCheckedDeclaredValueAndUncheckedPreannouncementExpected()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {                
                {"IsFastWayCapturing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {                
                {"IsDeclaredValueMandatoryCapturing", true},
                {"IsElectronicPreannouncement", false},
                {"IsAutomaticallyConfirmCapturing", true}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            List<string> depositNumberList = new List<string>();            
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();       
            CapturingPage capturingPage = fixture.HomePage.OpenPage<CapturingPage>();
            CapturingFormHelper capturingFormHelper = new CapturingFormHelper(capturingPage);
            depositNumberList = dataGenerationHelper.GenerateElectronicPreannouncements(1, Configuration.LocationCode);
            capturingFormHelper.CaptureDeposits(depositNumberList, CapturingMode.NoBatch, true);
            Assert.True(capturingFormHelper.isFastway, "Capturing is processed not in a fast way!");
            f = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Capturing, depositNumberList);
            Assert.True(f, "Some of captured deposits have invalid status.");
            dataGenerationHelper.ClearList(depositNumberList);
        }
                
        /// <summary>
        /// Capturing can be made with following settings:
        /// Fast Way Capturing = 'yes', Declared Value = 'no', Pre-announcement Expected = 'yes'
        /// Pre-announcements received:
        /// - Customer Electronic
        /// </summary>
        [Fact(DisplayName = "UI - Fast Way Capturing with unchecked Declared Value and checked Pre-announcement Expected flags")]
        public void FastWayCapturingWithUncheckedDeclaredValueAndCheckedPreannouncementExpected()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {                
                {"IsFastWayCapturing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {                
                {"IsDeclaredValueMandatoryCapturing", false},
                {"IsElectronicPreannouncement", true},
                {"IsAutomaticallyConfirmCapturing", true}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            List<string> depositNumberList = new List<string>();
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();
            CapturingPage capturingPage = fixture.HomePage.OpenPage<CapturingPage>();
            CapturingFormHelper capturingFormHelper = new CapturingFormHelper(capturingPage);
            depositNumberList = dataGenerationHelper.GenerateElectronicPreannouncements(1, Configuration.LocationCode);
            capturingFormHelper.CaptureDeposits(depositNumberList, CapturingMode.NoBatch, true);
            Assert.True(capturingFormHelper.isFastway, "Capturing is processed not in a fast way!");
            f = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Capturing, depositNumberList);
            Assert.True(f, "Some of captured deposits have invalid status.");
            dataGenerationHelper.ClearList(depositNumberList);
        }        
        
        /// <summary>
        /// Capturing can be made with following settings:
        /// Fast Way Capturing = 'yes', Declared Value = 'no', Pre-announcement Expected = 'no'
        /// Pre-announcements received:
        /// - Customer Electronic
        /// </summary>
        [Fact(DisplayName = "UI - Fast Way Capturing with unchecked Declared Value and Pre-announcement Expected flags")]
        public void FastWayCapturingWithUncheckedDeclaredValueAndPreannouncementExpected()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {                
                {"IsFastWayCapturing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {                
                {"IsDeclaredValueMandatoryCapturing", false},
                {"IsElectronicPreannouncement", false},
                {"IsAutomaticallyConfirmCapturing", true}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            List<string> depositNumberList = new List<string>();
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();
            CapturingPage capturingPage = fixture.HomePage.OpenPage<CapturingPage>();
            CapturingFormHelper capturingFormHelper = new CapturingFormHelper(capturingPage);
            depositNumberList = dataGenerationHelper.GenerateElectronicPreannouncements(1, Configuration.LocationCode);
            capturingFormHelper.CaptureDeposits(depositNumberList, CapturingMode.NoBatch, true);
            Assert.True(capturingFormHelper.isFastway, "Capturing is processed not in a fast way!");
            f = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Capturing, depositNumberList);
            Assert.True(f, "Some of captured deposits have invalid status.");
            dataGenerationHelper.ClearList(depositNumberList);
        }
        
        /// <summary>
        /// Capturing can be made with following settings:
        /// Fast Way Capturing = 'yes', Declared Value = 'yes', Pre-announcement Expected = 'yes'
        /// Pre-announcements received:
        /// - Transport
        /// </summary>
        [Fact(DisplayName = "UI - Fast Way Capturing of mother and inner deposits with Transport pre-announcement")]
        public void FastWayCapturingTransport()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {                
                {"IsFastWayCapturing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {                
                {"IsDeclaredValueMandatoryCapturing", true},
                {"IsElectronicPreannouncement", true},
                {"IsAutomaticallyConfirmCapturing", true}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            string[,] preannouncements;
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();
            CapturingPage capturingPage = fixture.HomePage.OpenPage<CapturingPage>();
            CapturingFormHelper capturingFormHelper = new CapturingFormHelper(capturingPage);
            preannouncements = dataGenerationHelper.GenerateTransportPreannouncements(1, 1, Configuration.LocationCode, false);
            capturingFormHelper.CaptureDeposits(preannouncements, CapturingMode.NoBatch, false);
            Assert.True(capturingFormHelper.isLocationPreselected, "Expected location has not been pre-selected as Location From.");
            f = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Capturing, preannouncements);
            Assert.True(f, "Some of captured deposits have invalid status.");
        }

        /// <summary>
        /// Capturing can be made with following settings:
        /// Fast Way Capturing = 'yes', Declared Value = 'yes', Pre-announcement Expected = 'yes'
        /// Pre-announcements received:
        /// - Transport
        /// - Customer Electronic
        /// </summary>
        [Fact(DisplayName = "UI - Fast Way Capturing of mother and inner deposits with Transport and Electronic pre-announcements")]
        public void FastWayCapturingTransportElectronic()
        {
            bool f;
            string[,] preannouncements;
            var siteSettings = new Dictionary<string, bool>()
            {                
                {"IsFastWayCapturing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {                
                {"IsDeclaredValueMandatoryCapturing", true},
                {"IsElectronicPreannouncement", true},
                {"IsAutomaticallyConfirmCapturing", true}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            DataGenerationHelper dataGenerationHelper = new DataGenerationHelper();
            CapturingPage capturingPage = fixture.HomePage.OpenPage<CapturingPage>();
            CapturingFormHelper capturingFormHelper = new CapturingFormHelper(capturingPage);
            preannouncements = dataGenerationHelper.GenerateTransportPreannouncements(1, 1, Configuration.LocationCode, true);
            capturingFormHelper.CaptureDeposits(preannouncements, CapturingMode.NoBatch, true);
            Assert.True(capturingFormHelper.isFastway, "Capturing is processed not in a fast way!");
            f = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Capturing, preannouncements);
            Assert.True(f, "Some of captured deposits have invalid status.");
        }

        /// <summary>
        /// Fast Way Capturing can be completed for fast scanned barcodes with following settings:
        /// Fast Way Capturing = 'yes', Declared Value = 'yes', Pre-announcement Expected = 'yes'
        /// Pre-announcements received:
        /// - Customer Electronic
        /// </summary>
        [Fact(DisplayName = "UI - Fast Way Capturing No Waiting")]
        public void FastWayCapturingNoWaiting()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {                
                {"IsFastWayCapturing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {                
                {"IsDeclaredValueMandatoryCapturing", true},
                {"IsElectronicPreannouncement", true},
                {"IsAutomaticallyConfirmCapturing", true}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            var depositNumberList = new List<string>();
            var dataGenerationHelper = new DataGenerationHelper();
            var capturingPage = fixture.HomePage.OpenPage<CapturingPage>();
            var capturingFormHelper = new CapturingFormHelper(capturingPage);
            depositNumberList = dataGenerationHelper.GenerateElectronicPreannouncements(20, Configuration.LocationCode);
            capturingFormHelper.CaptureDeposits(depositNumberList, CapturingMode.NoBatch, false);
            Assert.True(capturingFormHelper.isFastway, "Capturing is processed not in a fast way!");
            f = ContainerProcessingHelper.CheckContainerStatus(InboundProcess.Capturing, depositNumberList);
            Assert.True(f, "Some of captured deposits have invalid status.");
            dataGenerationHelper.ClearList(depositNumberList);
        }

        [Fact(DisplayName = "UI - Capturing form UI with Batch in progress")]
        public void CapturingFormUIWithBatchInProgress()
        {
            bool f;
            var siteSettings = new Dictionary<string, bool>()
            {                
                {"IsFastWayCapturing", true}
            };

            var processSettings = new Dictionary<string, bool>()
            {                
                {"IsDeclaredValueMandatoryCapturing", true},
                {"IsElectronicPreannouncement", true},
                {"IsAutomaticallyConfirmCapturing", true}
            };

            SettingsHelper.SetUpCashCenterSettings(siteSettings, processSettings, null);
            var depositNumberList = new List<string>();
            var dataGenerationHelper = new DataGenerationHelper();
            var capturingPage = fixture.HomePage.OpenPage<CapturingPage>();
            var capturingFormHelper = new CapturingFormHelper(capturingPage);
            depositNumberList = dataGenerationHelper.GenerateElectronicPreannouncements(1, Configuration.LocationCode);
            capturingFormHelper.CaptureDeposits(depositNumberList, CapturingMode.OneStepCount, false);
            f = capturingFormHelper.CheckUIWithBatchInProgress();
            Assert.True(f, "Capturing form has some of controls having invalid state for 'Batch in Progress' mode!");
            dataGenerationHelper.ClearList(depositNumberList);
            capturingFormHelper.CloseBatch();
        }

        [Fact(DisplayName = "UI - Capturing Date is current")]
        public void CheckCapturingFormDate()
        {
            bool f;
            var capturingPage = fixture.HomePage.OpenPage<CapturingPage>();            
            f = CommonHelper.CheckDateIsCurrent(capturingPage.DateLbl);            
            Assert.True(f, "Capturing Date is not current date!");
        }

        [Fact(DisplayName = "UI - Capturing form is closed correctly")]
        public void CloseCapturingForm()
        {
            var capturingPage = fixture.HomePage.OpenPage<CapturingPage>();
            var formHelper = new FormHelper<CapturingPage>(capturingPage);
            formHelper.CloseForm();           
            Assert.True(formHelper.isDefaultPageOpened, "Capturing form is not properly closed!");
        }
    }
}
