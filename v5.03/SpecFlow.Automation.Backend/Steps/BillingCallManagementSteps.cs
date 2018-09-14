using Cwc.BaseData;
using Cwc.Billing.Model;
using Cwc.CallManagement;
using Cwc.Contracts;
using Cwc.Contracts.Enums;
using Cwc.Contracts.Model;
using Cwc.Security;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using Specflow.Automation.Backend.Hooks;
using Specflow.Automation.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Specflow.Automation.Backend.Steps
{
    [Binding]
    public sealed class BillingCallManagementSteps
    {
        List<PriceLineLevel> priceLineLevelList = new List<PriceLineLevel>();        
        BilledCase billedCase;
        Call call;
        Contract contract = ContractDataConfigurationHooks.NoteCompanyContract;
        Customer customer;
        PriceLine priceLine;
        PriceRule priceRule;
        UnitOfMeasure unitOfMeasure;

        [Given(@"Price level is configured")]
        public void GivenPriceLevelIsConfigured(Table table)
        {
            foreach (var row in table.Rows)
            {
                var priceRuleName = row["PriceRule"];
                var unitOfMeasureName = row["UnitOfMeasure"];
                var priceRuleLevelNameStr = row["PriceRuleLevelName"];
                var priceRuleLevelValueTypeName = row["PriceRuleLevelValueType"];
                var entityName = row["Entity"];
                var entityCode = row["Code"];
                var priceRule = priceRuleName.ConvertToPriceRule();
                var unitOfMeasure = unitOfMeasureName.ConvertToUnitOfMeasure();
                var priceRuleLevelName = priceRuleLevelNameStr.ConvertToPriceRuleLevelName();
                var priceRuleLevelValueType = priceRuleLevelValueTypeName.ConvertToPriceRuleLevelValueType();
                var entity = HelperFacade.BillingHelper.GetEntity(entityName, entityCode);
                var property = entity.GetType().GetProperty("IdentityId");
                var entityId = property != null ? property.GetValue(entity, null) : entity.GetType().GetProperty("ID").GetValue(entity, null);
                var priceLineLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(
                    priceRule,
                    unitOfMeasure,
                    priceRuleLevelName,
                    priceRuleLevelValueType,
                    Convert.ToDecimal(entityId),
                    entityCode);

                priceLineLevelList.Add(priceLineLevel);
            }
        }

        [Given(@"Price line is saved")]
        public void GivenPriceLineIsSaved(Table table)
        {
            foreach (var row in table.Rows)
            {
                var priceRuleName = row["PriceRule"];
                var unitOfMeasureName = row["UnitOfMeasure"];
                var units = Convert.ToInt32(row["Units"]);
                var price = Convert.ToDecimal(row["Price"]);
                priceRule = priceRuleName.ConvertToPriceRule();
                unitOfMeasure = unitOfMeasureName.ConvertToUnitOfMeasure();

                priceLine = HelperFacade.ContractHelper.CreatePriceLine(contract, priceRule, unitOfMeasure,
                    priceLineLevelList, units: units, price: price);
            }
        }

        [Given(@"Call is created")]
        public void GivenCallIsCreated(Table table)
        {
            var date = DateTime.Now;
            var callCategoryName = table.Rows[0]["CallCategory"];
            var customerCode = table.Rows[0]["Customer"];
            var locationCode = table.Rows[0]["Location"];
            var number = table.Rows[0]["Number"];            
            var priorityName = table.Rows[0]["Priority"];
            var solutionCodeName = table.Rows[0]["SolutionCode"];
            var statusName = table.Rows[0]["Status"];
            var requestorName = table.Rows[0]["Requestor"];
            if (!Enum.TryParse(statusName, out CallStatus status))
            {
                throw new InvalidCastException($"Error on parsing {statusName} enumeration.");
            }

            if (!Enum.TryParse(priorityName, out CallPriority priority))
            {
                throw new InvalidCastException($"Error on parsing {priorityName} enumeration.");
            }

            using (var context = new AutomationCallManagementDataContext())
            {
                var callCategory = context.CallCategories.Single(x => x.Name == callCategoryName);
                var solutionCode = context.SolutionCodes.Single(x => x.Code == solutionCodeName);
                customer = customerCode.ConvertToCustomer();
                var location = locationCode.ConvertToLocation();
                var requestor = requestorName.ConvertToRequestor();
                var callOperator = SecurityFacade.LoginService.GetAdministratorLogin();


                call = DataFacade.Call.New().
                    With_Status(status).
                    With_Priority(priority).
                    With_Subject(number).
                    With_DateOccurred(date).
                    With_CallCategory_id(callCategory.ID).
                    With_SolutionCode_id(solutionCode.ID).
                    With_Company_id(customer.ID).
                    With_Location_id(location.ID).
                    With_Requestor_id(requestor.ID).
                    With_Operator_id(callOperator.UserID).
                    SaveToDb().
                    Build();   
            }
        }

        [When(@"Billing job processes price line")]
        public void WhenBillingJobProcessesPriceLine()
        {
            BillingHelper.UpdateBillingJobSettingsWithNewCompany(customer);
            HelperFacade.BillingHelper.RunBillingJob();
        }

        [Then(@"System creates billed case")]
        public void ThenSystemCreatesBilledCase()
        {
            billedCase = HelperFacade.BillingHelper.GetTransportIncidentBilledCase(contract.ID, priceRule, unitOfMeasure, call.ID);
            Assert.NotNull(billedCase);
        }

        [Then(@"System creates billing line")]
        public void ThenSystemCreatesBillingLine(Table table)
        {
            var value = Convert.ToDecimal(table.Rows[0]["Value"]);
            var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
            Assert.True(billingLine.Value == value);
        }
    }
}
