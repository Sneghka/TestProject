﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.4.0.0
//      SpecFlow Generator Version:2.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Specflow.Automation.Backend.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "cashcenter-data-generation-required")]
    [Xunit.TraitAttribute("Category", "contract-data-generation-required")]
    [Xunit.TraitAttribute("Category", "service-order-import-format-b")]
    public partial class ServiceOrderImportFormatBInterfaceFeature : Xunit.IClassFixture<ServiceOrderImportFormatBInterfaceFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "ServiceOrderImportFormatB.feature"
#line hidden
        
        public ServiceOrderImportFormatBInterfaceFeature(ServiceOrderImportFormatBInterfaceFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Service Order Import Format B Interface", "\t\tAs a Sanid employee \r\n\t\tI want to import Bank Orders\r\n\t\tSo that delivery orders" +
                    " could be processed in CWC", ProgrammingLanguage.CSharp, new string[] {
                        "cashcenter-data-generation-required",
                        "contract-data-generation-required",
                        "service-order-import-format-b"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="Service order is imported correctly by Service Order Import Format B Interface")]
        [Xunit.TraitAttribute("FeatureTitle", "Service Order Import Format B Interface")]
        [Xunit.TraitAttribute("Description", "Service order is imported correctly by Service Order Import Format B Interface")]
        public virtual void ServiceOrderIsImportedCorrectlyByServiceOrderImportFormatBInterface()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Service order is imported correctly by Service Order Import Format B Interface", null, ((string[])(null)));
#line 9
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Record type",
                        "Bank identification"});
            table1.AddRow(new string[] {
                        "1",
                        "100020"});
#line 10
 testRunner.Given("Import Express Delivery Order File is created with following Leading record", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Record type",
                        "Address code",
                        "Account number",
                        "Bag type",
                        "Reference",
                        "Bank reference"});
            table2.AddRow(new string[] {
                        "2",
                        "A18841",
                        "8839123008",
                        "3306",
                        "Referenceb",
                        "BRef77a485fbec894378"});
#line 13
   testRunner.And("Import Express Delivery Order File is created with following Order record", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Record type",
                        "Article code",
                        "Quantity"});
            table3.AddRow(new string[] {
                        "3",
                        "B005B001",
                        "2"});
            table3.AddRow(new string[] {
                        "3",
                        "B200B001",
                        "3"});
#line 16
   testRunner.And("Import Express Delivery Order File is created with following Order item record", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Record type",
                        "Name",
                        "Date of birth",
                        "Phone number",
                        "Street",
                        "House number",
                        "House number addition",
                        "Post code",
                        "City"});
            table4.AddRow(new string[] {
                        "4",
                        "1225ATM010A",
                        "19500101",
                        "987263567990123",
                        "7Street",
                        "46",
                        "A",
                        "1000AA",
                        "Kyiv"});
#line 20
   testRunner.And("Import Express Delivery Order File is created with following Ordered delivery inf" +
                    "ormation record", ((string)(null)), table4, "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Record type",
                        "Number Of Detail Records"});
            table5.AddRow(new string[] {
                        "9",
                        "4"});
#line 23
   testRunner.And("Import Express Delivery Order File is created with following Close record", ((string)(null)), table5, "And ");
#line 26
  testRunner.When("Service Order Import Format B Interface processes file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 27
  testRunner.Then("Service order was created with correct attributes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 28
   testRunner.And("Service products was created correctly", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                ServiceOrderImportFormatBInterfaceFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ServiceOrderImportFormatBInterfaceFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
