using Cwc.Common;
using Cwc.Contracts.Enums;
using Cwc.Contracts.Model;
using Cwc.Ordering;
using CWC.AutoTests.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace CWC.AutoTests.Tests.Billing
{
    [Xunit.Collection("Billing")]
    [Trait("Category", "Billing")]
    public class ContractBillingTests : BaseTest, IClassFixture<BillingTestFixture>, IDisposable
    {
        BillingTestFixture fixture;
        DateTime actualEffectiveDate;        
        DateTime actualDateCreated;
        List<decimal> locationIdList;      

        public ContractBillingTests(BillingTestFixture setupFixture)
        {
            this.fixture = setupFixture;
            this.actualEffectiveDate = fixture.CompanyContract.EffectiveDate;
            this.actualDateCreated = fixture.ExternalLocation.DateCreated;
        }

        public void Dispose()
        {
            try
            {
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, actualEffectiveDate);
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, actualDateCreated);
                if (this.locationIdList != null)
                {
                    HelperFacade.BillingHelper.LinkLocationsToCompany(this.locationIdList, (decimal)fixture.CompanyContract.CustomerID);
                }
            }            
            finally
            {
                HelperFacade.TransportHelper.ClearTestData();
                HelperFacade.BillingHelper.ClearTestData();
                HelperFacade.ContractHelper.ClearTestData();
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule of 1-month period when start date is the first day of the month")]
        public void VerifyBillingByPricePerPeriodRuleOfOneMonthPeriodWhenStartDateIsTheFirstDayOfTheMonth()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, 
                    UnitOfMeasure.Month, PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, 
                    fixture.ExternalLocation.Code);
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                   PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                    new List<PriceLineLevel> { contractedLocationLevel, collectServiceTypeLevel }, units: 1, price: 10);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-2));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID, 
                    (decimal)fixture.CompanyContract.CustomerID);
               
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID);
                Assert.True(billedCase.ActualUnits == 1);
                Assert.True(billedCase.DateBilled == null);
                Assert.True(billedCase.PeriodBilledFrom == currentMonthFirstDay.AddMonths(-1));
                Assert.True(billedCase.PeriodBilledTo == currentMonthFirstDay.AddDays(-1));

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Units == billedCase.ActualUnits);
                Assert.True(billingLine.Value == 10);                
                Assert.True(billingLine.PeriodBilledFrom == billedCase.PeriodBilledFrom);
                Assert.True(billingLine.PeriodBilledTo == billedCase.PeriodBilledTo);
                Assert.False(billingLine.IsManual);
                Assert.Null(billingLine.Comment);                
                Assert.True(billingLine.LocationID == fixture.ExternalLocation.IdentityID);                
                Assert.True(billingLine.ContractID == fixture.CompanyContract.ID);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule of 1-month period when start date is the last day of the month")]
        public void VerifyBillingByPricePerPeriodRuleOfOneMonthPeriodWhenStartDateIsTheLastDayOfTheMonth()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                   PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 1, price: 10);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-2));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                     (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID);
                Assert.NotNull(billedCase);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);                
                Assert.True(billingLine.Value == 10);
            }
            catch
            {
                throw;
            }
        }               

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule of 2-month period when start date is the first day of the month")]
        public void VerifyBillingByPricePerPeriodRuleOfTwoMonthPeriodWhenStartDateIsTheFirstDayOfTheMonth()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                   PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 1, price: 10);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(-2));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-3));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID, currentMonthFirstDay.AddMonths(-2),
                    currentMonthFirstDay.AddMonths(-1).AddDays(-1));
                Assert.NotNull(billedCase1);

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID, currentMonthFirstDay.AddMonths(-1),
                    currentMonthFirstDay.AddDays(-1));
                Assert.NotNull(billedCase2);

                var billedCase3 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID, currentMonthFirstDay,
                    currentMonthFirstDay.AddMonths(1).AddDays(-1));
                Assert.Null(billedCase3);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);                
                Assert.True(billingLine1.Value == 10);
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule of 2-month period when start date is the last day of the month")]
        public void VerifyBillingByPricePerPeriodRuleOfTwoMonthPeriodWhenStartDateIsTheLastDayOfTheMonth()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                  PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 1, price: 10);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(-1).AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-3));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID, currentMonthFirstDay.AddMonths(-1).AddDays(-1),
                    currentMonthFirstDay.AddMonths(-1).AddDays(-1));
                Assert.NotNull(billedCase1);

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID, currentMonthFirstDay.AddMonths(-1),
                    currentMonthFirstDay.AddDays(-1));
                Assert.NotNull(billedCase2);

                var billedCase3 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID, currentMonthFirstDay,
                    currentMonthFirstDay.AddMonths(1).AddDays(-1));
                Assert.Null(billedCase3);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 10);
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 10);
            }
            catch
            {
                throw;
            }
        }               

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule of 2-month period when start date is in the middle of the previous year month")]
        public void VerifyBillingByPricePerPeriodRuleOfTwoMonthPeriodWhenStartDateIsInTheMiddleOfThePreviousYearMonth()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                  PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 1, price: 10);

                var today = DateTime.Today;
                var currentYearFirstDay = new DateTime(today.Year, 1, 1);
                var previousYearEndDate = currentYearFirstDay.AddYears(-1).AddMonths(1); // February 1st of the previous year                
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, previousYearEndDate.AddMonths(-1).AddDays(-15));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, previousYearEndDate);
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, previousYearEndDate.AddMonths(-3));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID, previousYearEndDate.AddMonths(-1).AddDays(-15),
                    previousYearEndDate.AddMonths(-1).AddDays(-1));
                Assert.NotNull(billedCase1);

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID, previousYearEndDate.AddMonths(-1),
                    previousYearEndDate.AddDays(-1));
                Assert.NotNull(billedCase2);

                var billedCase3 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID, previousYearEndDate,
                    previousYearEndDate.AddDays(-1).AddMonths(1));
                Assert.Null(billedCase3);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 10);
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule of 1-month period when location start date is the first day of the month")]
        public void VerifyBillingByPricePerPeriodRuleOfOneMonthPeriodWhenLocationStartDateIsTheFirstDayOfTheMonth()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                  PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 1, price: 10);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(-2));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-1));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID);
                Assert.NotNull(billedCase);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule of 1-month period when location start date is the last day of the month")]
        public void VerifyBillingByPricePerPeriodRuleOfOneMonthPeriodWhenLocationStartDateIsTheLastDayOfTheMonth()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                  PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 1, price: 10);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(-2));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddDays(-1));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID);
                Assert.NotNull(billedCase);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule of 1-month period when contract end date is less than current date")]
        public void VerifyBillingByPricePerPeriodRuleOfOneMonthPeriodWhenContractEndDateIsLessThanCurrentDate()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                  PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 1, price: 10);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(-2));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(-1));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-3));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID);
                Assert.True(billedCase.PeriodBilledFrom == currentMonthFirstDay.AddMonths(-2));
                Assert.True(billedCase.PeriodBilledTo == currentMonthFirstDay.AddMonths(-1).AddDays(-1));

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule of 2-week period when start date is the first day of the monday week")]
        public void VerifyBillingByPricePerPeriodRuleOfTwoWeekPeriodWhenStartDateIsTheFirstDayOfTheMondayWeek()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Week,
                  PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Week,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 1, price: 10);

                HelperFacade.BillingHelper.SetFirstDayOfTheWeek(Weekday.Monday);
                var firstDayOfTheWeek = HelperFacade.BillingHelper.GetFirstDayOfTheWeekDate(DateTime.Now.Date);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, firstDayOfTheWeek.AddDays(-14));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, firstDayOfTheWeek.AddDays(5));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, firstDayOfTheWeek.AddDays(-21));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Week, fixture.ExternalLocation, fixture.CollectServiceTypeID, firstDayOfTheWeek.AddDays(-14),
                    firstDayOfTheWeek.AddDays(-8));
                Assert.NotNull(billedCase1);

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Week, fixture.ExternalLocation, fixture.CollectServiceTypeID, firstDayOfTheWeek.AddDays(-7),
                    firstDayOfTheWeek.AddDays(-1));
                Assert.NotNull(billedCase2);

                var billedCase3 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Week, fixture.ExternalLocation, fixture.CollectServiceTypeID, firstDayOfTheWeek,
                    firstDayOfTheWeek.AddDays(6));
                Assert.Null(billedCase3);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 10);
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule of 2-week period when location start date is the first day of the sunday week")]
        public void VerifyBillingByPricePerPeriodRuleOfTwoWeekPeriodWhenLocationStartDateIsTheFirstDayOfTheSundayWeek()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Week,
                  PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Week,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 1, price: 10);

                HelperFacade.BillingHelper.SetFirstDayOfTheWeek(Weekday.Sunday);
                var firstDayOfTheWeek = HelperFacade.BillingHelper.GetFirstDayOfTheWeekDate(DateTime.Now.Date);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, firstDayOfTheWeek.AddDays(-21));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, firstDayOfTheWeek.AddDays(5));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, firstDayOfTheWeek.AddDays(-14));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Week, fixture.ExternalLocation, fixture.CollectServiceTypeID, firstDayOfTheWeek.AddDays(-14),
                    firstDayOfTheWeek.AddDays(-8));
                Assert.NotNull(billedCase1);

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Week, fixture.ExternalLocation, fixture.CollectServiceTypeID, firstDayOfTheWeek.AddDays(-7),
                    firstDayOfTheWeek.AddDays(-1));
                Assert.NotNull(billedCase2);

                var billedCase3 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Week, fixture.ExternalLocation, fixture.CollectServiceTypeID, firstDayOfTheWeek,
                    firstDayOfTheWeek.AddDays(6));
                Assert.Null(billedCase3);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 10);
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 10);
            }
            catch
            {
                throw;
            }
            finally
            {                
                HelperFacade.BillingHelper.SetFirstDayOfTheWeek(Weekday.Monday);                
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule of 2-day period")]
        public void VerifyBillingByPricePerPeriodRuleOfSevenDayPeriod()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Day,
                  PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Day,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 1, price: 10);

                var today = DateTime.Today;                
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, today.AddMonths(-1));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Day, fixture.ExternalLocation, fixture.CollectServiceTypeID, today.AddDays(-1), today.AddDays(-1));
                Assert.NotNull(billedCase1);

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Day, fixture.ExternalLocation, fixture.CollectServiceTypeID, today, today);
                Assert.NotNull(billedCase2);

                var billedCase3 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Day, fixture.ExternalLocation, fixture.CollectServiceTypeID, today.AddDays(-2), today.AddDays(-2));
                Assert.Null(billedCase3);

                var billedCase4 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Day, fixture.ExternalLocation, fixture.CollectServiceTypeID, today.AddDays(1), today.AddDays(1));
                Assert.Null(billedCase4);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 10);
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule with days multiplier")]
        public void VerifyBillingByPricePerPeriodRuleWithMultiplier()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Day,
                  PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Day,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 2, price: 10, IsDecreasePriceProportionally: false);

                var today = DateTime.Today;
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-4));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, today.AddMonths(-1));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Day, fixture.ExternalLocation, fixture.CollectServiceTypeID, today.AddDays(-4), today.AddDays(-3));
                Assert.NotNull(billedCase1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 10);

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Day, fixture.ExternalLocation, fixture.CollectServiceTypeID, today.AddDays(-2), today.AddDays(-1));
                Assert.NotNull(billedCase2);

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 10);

                var billedCase3 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Day, fixture.ExternalLocation, fixture.CollectServiceTypeID, today, today.AddDays(1));
                Assert.Null(billedCase3);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule with weeks multiplier")]
        public void VerifyBillingByPricePerPeriodRuleWithWeeksMultiplier()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Week,
                  PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Week,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 2, price: 50, IsDecreasePriceProportionally: false);

                HelperFacade.BillingHelper.SetFirstDayOfTheWeek(Weekday.Monday);
                var firstDayOfTheWeek = HelperFacade.BillingHelper.GetFirstDayOfTheWeekDate(DateTime.Now.Date);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, firstDayOfTheWeek.AddDays(-28));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, firstDayOfTheWeek.AddMonths(-2));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Week, fixture.ExternalLocation, fixture.CollectServiceTypeID, firstDayOfTheWeek.AddDays(-28), 
                    firstDayOfTheWeek.AddDays(-15));
                Assert.NotNull(billedCase1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 50);

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Week, fixture.ExternalLocation, fixture.CollectServiceTypeID, firstDayOfTheWeek.AddDays(-14),
                    firstDayOfTheWeek.AddDays(-1));
                Assert.NotNull(billedCase2);

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 50);

                var billedCase3 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Week, fixture.ExternalLocation, fixture.CollectServiceTypeID, firstDayOfTheWeek, firstDayOfTheWeek.AddDays(13));
                Assert.Null(billedCase3);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for location and service type' rule with months multiplier")]
        public void VerifyBillingByPricePerPeriodRuleWithMonthsMultiplier()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                  PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForLocationAndServiceType, UnitOfMeasure.Month,
                    new List<PriceLineLevel> { collectServiceTypeLevel }, units: 2, price: 50, IsDecreasePriceProportionally: false);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(-4));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, today.AddMonths(-6));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID, currentMonthFirstDay.AddMonths(-4),
                    currentMonthFirstDay.AddMonths(-2).AddDays(-1));
                Assert.NotNull(billedCase1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 50);

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID, currentMonthFirstDay.AddMonths(-2),
                    currentMonthFirstDay.AddDays(-1));
                Assert.NotNull(billedCase2);

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 50);

                var billedCase3 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForLocationAndServiceType,
                    UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID, currentMonthFirstDay, 
                    currentMonthFirstDay.AddMonths(2).AddDays(-1));
                Assert.Null(billedCase3);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Price per period with proportionate decrease' rule for multiple service types")]
        public void VerifyBillingByPricePerPeriodWithProportionateDecreaseRuleForMultipleServiceTypes()
        {
            try
            {
                HelperFacade.ContractHelper.SaveProportionateDecreasePriceLinesForAllMonths(fixture.CollectServiceTypeID, BillingTestFixture.collectCode,
                    fixture.CompanyContract, 1, 10);
                HelperFacade.ContractHelper.SaveProportionateDecreasePriceLinesForAllMonths(fixture.DeliverServiceTypeID, BillingTestFixture.deliverCode,
                    fixture.CompanyContract, 1, 11);
                HelperFacade.ContractHelper.SaveProportionateDecreasePriceLinesForAllMonths(fixture.ReplenishmentServiceTypeID, 
                    BillingTestFixture.replenishmentCode, fixture.CompanyContract, 1, 12);
                HelperFacade.ContractHelper.SaveProportionateDecreasePriceLinesForAllMonths(fixture.ServicingServiceTypeID, BillingTestFixture.servicingCode,
                    fixture.CompanyContract, 1, 13);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddDays(-14));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-2));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                var collectOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode,
                   fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder.ID, currentMonthFirstDay.AddDays(-6));
                var deliveryOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.deliverCode, 
                    fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(deliveryOrder.ID, currentMonthFirstDay.AddDays(-7));                
                var replenishmentOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.replenishmentCode, 
                    fixture.ExternalLocation, withProducts: true, withServices: true);
                HelperFacade.TransportHelper.ChangeServiceDate(replenishmentOrder.ID, currentMonthFirstDay.AddDays(-3));
                var servicingOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.servicingCode, 
                    fixture.ExternalLocation, withProducts: false, withServices: true);
                HelperFacade.TransportHelper.ChangeServiceDate(servicingOrder.ID, currentMonthFirstDay.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, 
                    PriceRule.PricePerPeriodWithProportionateDecrease, UnitOfMeasure.ServiceOrder, fixture.ExternalLocation, fixture.CollectServiceTypeID);
                Assert.True(billedCase1.ActualUnits == 1);
                Assert.True(billedCase1.PeriodBilledFrom == currentMonthFirstDay.AddDays(-14));
                Assert.True(billedCase1.PeriodBilledTo == currentMonthFirstDay.AddDays(-1));

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, 
                    PriceRule.PricePerPeriodWithProportionateDecrease, UnitOfMeasure.ServiceOrder, fixture.ExternalLocation, fixture.DeliverServiceTypeID);
                Assert.NotNull(billedCase2);

                var billedCase3 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, 
                    PriceRule.PricePerPeriodWithProportionateDecrease, UnitOfMeasure.ServiceOrder, fixture.ExternalLocation, fixture.ReplenishmentServiceTypeID);
                Assert.NotNull(billedCase3);

                var billedCase4 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, 
                    PriceRule.PricePerPeriodWithProportionateDecrease, UnitOfMeasure.ServiceOrder, fixture.ExternalLocation, fixture.ServicingServiceTypeID);
                Assert.NotNull(billedCase4);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 10);
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 11);
                var billingLine3 = HelperFacade.BillingHelper.GetBillingLine(billedCase3.ID);
                Assert.True(billingLine3.Value == 12);
                var billingLine4 = HelperFacade.BillingHelper.GetBillingLine(billedCase4.ID);
                Assert.True(billingLine4.Value == 13);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per period with proportionate decrease' rule when service order quantity is greater than price line units")]
        public void VerifyBillingByPricePerPeriodWithProportionateDecreaseRuleWhenServiceOrderQuantityIsGreaterThanPriceLineUnits()
        {
            try
            {
                HelperFacade.ContractHelper.SaveProportionateDecreasePriceLinesForAllMonths(fixture.CollectServiceTypeID, BillingTestFixture.collectCode,
                    fixture.CompanyContract, 2, 10);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-2));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);
                
                var collectOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode, 
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder.ID, currentMonthFirstDay.AddDays(-1));
                var collectOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode, 
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder2.ID, currentMonthFirstDay.AddDays(-1));
                var collectOrder3 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode,
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder3.ID, currentMonthFirstDay.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerPeriodWithProportionateDecrease, UnitOfMeasure.ServiceOrder, fixture.ExternalLocation, fixture.CollectServiceTypeID);
                Assert.True(billedCase.ActualUnits == 2);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per period with proportionate decrease' rule when service order quantity is less than price line units")]
        public void VerifyBillingByPricePerPeriodWithProportionateDecreaseRuleWhenServiceOrderQuantityIsLessThanPriceLineUnits()
        {
            try
            {
                HelperFacade.ContractHelper.SaveProportionateDecreasePriceLinesForAllMonths(fixture.CollectServiceTypeID, BillingTestFixture.collectCode,
                    fixture.CompanyContract, 3, 30);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-2));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);
                
                var collectOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode,
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder.ID, currentMonthFirstDay.AddDays(-1));
                var collectOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode,
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder2.ID, currentMonthFirstDay.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerPeriodWithProportionateDecrease, UnitOfMeasure.ServiceOrder, fixture.ExternalLocation, fixture.CollectServiceTypeID);
                Assert.True(billedCase.ActualUnits == 2);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 20);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per period with proportionate decrease' rule when service order quantity is zero for time period")]
        public void VerifyBillingByPricePerPeriodWithProportionateDecreaseRuleWhenServiceOrderQuantityIsZeroForTimePeriod()
        {
            try
            {                
                HelperFacade.ContractHelper.SaveProportionateDecreasePriceLinesForAllMonths(fixture.CollectServiceTypeID, BillingTestFixture.collectCode,
                    fixture.CompanyContract, 1, 10); // price lines for all months are created because a) month level is mandatory, and b) we can test any month

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddDays(-1).AddMonths(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-3));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);
                
                var collectOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode, 
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder.ID, currentMonthFirstDay.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerPeriodWithProportionateDecrease, UnitOfMeasure.ServiceOrder, fixture.ExternalLocation, fixture.CollectServiceTypeID);
                Assert.True(billedCase.ActualUnits == 1);
                Assert.True(billedCase.PeriodBilledFrom == currentMonthFirstDay.AddMonths(-1));
                Assert.True(billedCase.PeriodBilledTo == currentMonthFirstDay.AddDays(-1));

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that service order quantity is defined properly for each time period")]
        public void VerifyThatServiceOrderQuantityIsDefinedProperlyForEachTimePeriod()
        {
            try
            {
                HelperFacade.ContractHelper.SaveProportionateDecreasePriceLinesForAllMonths(fixture.CollectServiceTypeID, BillingTestFixture.collectCode,
                    fixture.CompanyContract, 4, 40);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(-1).AddDays(-14));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-3));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);
                
                var collectOrder1Period0 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode, 
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder1Period0.ID, currentMonthFirstDay.AddMonths(-1).AddDays(-15));

                var collectOrder1Period1 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode,
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder1Period1.ID, currentMonthFirstDay.AddMonths(-1).AddDays(-14));

                var collectOrder2Period1 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode,
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder2Period1.ID, currentMonthFirstDay.AddMonths(-1).AddDays(-13));

                var collectOrder3Period1 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode,
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder3Period1.ID, currentMonthFirstDay.AddMonths(-1).AddDays(-1));

                var collectOrder1Period2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode,
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder1Period2.ID, currentMonthFirstDay.AddMonths(-1));

                var collectOrder2Period2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode,
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder2Period2.ID, currentMonthFirstDay.AddDays(-14));

                var collectOrder3Period2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode,
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder3Period2.ID, currentMonthFirstDay.AddDays(-1));

                var collectOrder2Period0 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode,
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder2Period0.ID, currentMonthFirstDay);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerPeriodWithProportionateDecrease,
                    UnitOfMeasure.ServiceOrder, fixture.ExternalLocation, fixture.CollectServiceTypeID, currentMonthFirstDay.AddMonths(-1).AddDays(-14),
                    currentMonthFirstDay.AddMonths(-1).AddDays(-1));                
                Assert.True(billedCase1.ActualUnits == 3);

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerPeriodWithProportionateDecrease,
                    UnitOfMeasure.ServiceOrder, fixture.ExternalLocation, fixture.CollectServiceTypeID, currentMonthFirstDay.AddMonths(-1),
                    currentMonthFirstDay.AddDays(-1));                
                Assert.True(billedCase2.ActualUnits == 3);

                var billedCase3 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerPeriodWithProportionateDecrease,
                    UnitOfMeasure.ServiceOrder, fixture.ExternalLocation, fixture.CollectServiceTypeID, currentMonthFirstDay,
                    currentMonthFirstDay.AddMonths(1).AddDays(-1));
                Assert.Null(billedCase3);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 30);
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 30);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period with at least one executed order' rule when service order quantity is zero for time period")]
        public void VerifyBillingByExecutedOrderRuleWhenServiceOrderQuantityIsZeroForTimePeriod()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodWithAtLeastOneExecutedOrder,
                    UnitOfMeasure.Month, PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID,
                    BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodWithAtLeastOneExecutedOrder,
                    UnitOfMeasure.Month, new List<PriceLineLevel> { collectServiceTypeLevel }, units: 1, price: 10);
                
                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(-1).AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-3));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);
                
                var collectOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode, 
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder.ID, currentMonthFirstDay.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID,
                    PriceRule.FixedPricePerPeriodWithAtLeastOneExecutedOrder, UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID);
                Assert.True(billedCase.ActualUnits == 1);
                Assert.True(billedCase.PeriodBilledFrom == currentMonthFirstDay.AddMonths(-1));
                Assert.True(billedCase.PeriodBilledTo == currentMonthFirstDay.AddDays(-1));

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period with at least one executed order' rule when there is previous billing line")]
        public void VerifyBillingByExecutedOrderRuleWhenThereIsPreviousBillingLine()
        {
            try
            {
                var collectServiceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.FixedPricePerPeriodWithAtLeastOneExecutedOrder,
                    UnitOfMeasure.Month, PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID,
                    BillingTestFixture.collectCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodWithAtLeastOneExecutedOrder,
                    UnitOfMeasure.Month, new List<PriceLineLevel> { collectServiceTypeLevel }, units: 1, price: 10);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);                
                var collectOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode, 
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder.ID, currentMonthFirstDay.AddMonths(-1).AddDays(-1));

                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(-1).AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddDays(-2));
                HelperFacade.BillingHelper.EditDateCreatedOfLocation(fixture.ExternalLocation, currentMonthFirstDay.AddMonths(-3));
                this.locationIdList = HelperFacade.BillingHelper.UnlinkAllLocationsExceptOneFromCompany(fixture.ExternalLocation.ID,
                    (decimal)fixture.CompanyContract.CustomerID);

                HelperFacade.BillingHelper.RunBillingJob();

                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                var collectOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Completed, BillingTestFixture.collectCode,
                    fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(collectOrder2.ID, currentMonthFirstDay.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID,
                    PriceRule.FixedPricePerPeriodWithAtLeastOneExecutedOrder, UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID,
                    currentMonthFirstDay.AddMonths(-1).AddDays(-1), currentMonthFirstDay.AddMonths(-1).AddDays(-1));

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 10);

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID,
                    PriceRule.FixedPricePerPeriodWithAtLeastOneExecutedOrder, UnitOfMeasure.Month, fixture.ExternalLocation, fixture.CollectServiceTypeID,
                    currentMonthFirstDay.AddMonths(-1), currentMonthFirstDay.AddDays(-1));
                Assert.True(billedCase2.ActualUnits == 1);                

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for company' rule of 2-month period when start date is in the middle of the previous year month")]
        public void VerifyBillingByFixedPricePerPeriodForCompanyRuleOfTwoMonthPeriodWhenStartDateIsInTheMiddleOfThePreviousYearMonth()
        {
            try
            {                
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForCompany, UnitOfMeasure.Month,
                    null, units: 1, price: 10);

                var today = DateTime.Today;
                var currentYearFirstDay = new DateTime(today.Year, 1, 1);
                var previousYearEndDate = currentYearFirstDay.AddYears(-1).AddMonths(1); // February 1st of the previous year                
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, previousYearEndDate.AddMonths(-1).AddDays(-15));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, previousYearEndDate);                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForCompany,
                    UnitOfMeasure.Month, fixture.Company.IdentityID, previousYearEndDate.AddMonths(-1).AddDays(-15),
                    previousYearEndDate.AddMonths(-1).AddDays(-1));
                Assert.NotNull(billedCase1);
                Assert.True(billedCase1.LocationID == null);
                Assert.True(billedCase1.ServiceTypeID == null);

                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForCompany,
                    UnitOfMeasure.Month, fixture.Company.IdentityID, previousYearEndDate.AddMonths(-1),
                    previousYearEndDate.AddDays(-1));
                Assert.NotNull(billedCase2);

                var billedCase3 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID, PriceRule.FixedPricePerPeriodForCompany,
                    UnitOfMeasure.Month, fixture.Company.IdentityID, previousYearEndDate,
                    previousYearEndDate.AddDays(-1).AddMonths(1));
                Assert.Null(billedCase3);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 10);
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'Fixed price per period for company' rule when there is previous billing line")]
        public void VerifyBillingByFixedPricePerPeriodForCompanyRuleWhenThereIsPreviousBillingLine()
        {
            try
            {                
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.FixedPricePerPeriodForCompany,
                    UnitOfMeasure.Month, null, units: 1, price: 10);

                var today = DateTime.Today;
                var currentMonthFirstDay = new DateTime(today.Year, today.Month, 1);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(-1).AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddDays(-2));                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID,
                    PriceRule.FixedPricePerPeriodForCompany, UnitOfMeasure.Month, fixture.Company.IdentityID, 
                    currentMonthFirstDay.AddMonths(-1).AddDays(-1), currentMonthFirstDay.AddMonths(-1).AddDays(-1));
                Assert.NotNull(billedCase1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 10);

                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, currentMonthFirstDay.AddMonths(1).AddDays(-2));
                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetPeriodBilledCase(fixture.CompanyContract.ID,
                    PriceRule.FixedPricePerPeriodForCompany, UnitOfMeasure.Month, fixture.Company.IdentityID, currentMonthFirstDay.AddMonths(-1), 
                    currentMonthFirstDay.AddDays(-1));
                Assert.NotNull(billedCase2);

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 10);
            }
            catch
            {
                throw;
            }
        }
    }
}
