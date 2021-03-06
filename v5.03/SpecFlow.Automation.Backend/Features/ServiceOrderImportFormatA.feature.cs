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
    [Xunit.TraitAttribute("Category", "service-order-import-format-a")]
    public partial class ServiceOrderImportFormatAInterfaceFeature : Xunit.IClassFixture<ServiceOrderImportFormatAInterfaceFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "ServiceOrderImportFormatA.feature"
#line hidden
        
        public ServiceOrderImportFormatAInterfaceFeature(ServiceOrderImportFormatAInterfaceFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Service Order Import Format A Interface", "\t\tAs a Sanid employee \r\n\t\tI want to import Bank Orders\r\n\t\tSo that delivery orders" +
                    " could be processed in CWC", ProgrammingLanguage.CSharp, new string[] {
                        "service-order-import-format-a"});
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
        
        [Xunit.FactAttribute(DisplayName="Service order is imported correctly by Service Order Import Format A Interface")]
        [Xunit.TraitAttribute("FeatureTitle", "Service Order Import Format A Interface")]
        [Xunit.TraitAttribute("Description", "Service order is imported correctly by Service Order Import Format A Interface")]
        public virtual void ServiceOrderIsImportedCorrectlyByServiceOrderImportFormatAInterface()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Service order is imported correctly by Service Order Import Format A Interface", null, ((string[])(null)));
#line 7
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Key",
                        "Value"});
            table1.AddRow(new string[] {
                        "Company Name",
                        "Blamburlam"});
            table1.AddRow(new string[] {
                        "Company Number",
                        "3303"});
            table1.AddRow(new string[] {
                        "Service Type",
                        "DELV"});
#line 8
 testRunner.Given("Excel file is created with following header attributes and service date in 2 days" +
                    "", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Location Name",
                        "Location Code",
                        "SAR 10",
                        "SAR 50",
                        "SAR 100",
                        "SAR 500",
                        "USD 100"});
            table2.AddRow(new string[] {
                        "Order Import FA01",
                        "ATM01",
                        "2",
                        "2",
                        "2",
                        "2",
                        "2"});
#line 13
   testRunner.And("Row with following attributes is added to created excel file", ((string)(null)), table2, "And ");
#line 16
  testRunner.When("Service Order Import Format A Interface processes excel file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
  testRunner.Then("System creates service order with correct attributes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 18
   testRunner.And("System creates service products correctly", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
   testRunner.And("Service Order Import Format A Log record is created with \'Ok\' result", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                ServiceOrderImportFormatAInterfaceFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ServiceOrderImportFormatAInterfaceFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
