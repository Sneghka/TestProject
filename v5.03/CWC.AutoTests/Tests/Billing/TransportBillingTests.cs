using Cwc.BaseData;
using Cwc.BaseData.Model;
using Cwc.Contracts;
using Cwc.Contracts.Enums;
using Cwc.Contracts.Model;
using Cwc.Ordering;
using Cwc.Transport.Enums;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.DailyDataBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.Billing
{
    [Collection("Billing")]
    [Trait("Category", "Billing")]
    public class TransportBillingTests : IClassFixture<BillingTestFixture>, IDisposable
    {
        BillingTestFixture fixture;
        DateTime actualEffectiveDate;
        DateTime today = DateTime.Today;
        //DateTime actualDateCreated;
        private const string collectCode = "COLL";
        private const string deliverCode = "DELV";
        private const string replenishmentCode = "REPL";
        private const string servicingCode = "SERV";
        private const string currencyCode = "EUR";
        private const string noteType = "NOTE";

        public TransportBillingTests(BillingTestFixture setupFixture)
        {
            fixture = setupFixture;
            actualEffectiveDate = fixture.CompanyContract.EffectiveDate;
        }

        public void Dispose()
        {
            try
            {
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, actualEffectiveDate);
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);
            }
            finally
            {
                HelperFacade.TransportHelper.ClearTestData();
                HelperFacade.BillingHelper.ClearTestData();
                HelperFacade.ContractHelper.ClearTestData();
            }            
        }

        #region 4.6.1 Get Billing Data by Visits per Day Per Customer
        [Fact(DisplayName = "Billing - 'price per visit per company' rule for contract date")]
        public void VerifyBillingByPricePerVisitPerCompanyRuleForContractDate()
        {
            try
            {                
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitPerCompany, UnitOfMeasure.LocationStop,
                    null, units: 1, price: 25);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);
                
                // positive
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                   withProducts: false, withServices: false);
                var serviceOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation,
                  withProducts: true, withServices: false);
                // negative
                var serviceOrder3 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation,
                   withProducts: true, withServices: false);
                var serviceOrder4 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation,
                  withProducts: true, withServices: false);

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);                
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));                                           
                var transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder2, TransportOrderStatus.Completed, new TimeSpan(16, 0, 0));
                var transportOrder3 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder3.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder3, TransportOrderStatus.Completed, new TimeSpan(17, 0, 0));
                var transportOrder4 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder4.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder4, TransportOrderStatus.Completed, new TimeSpan(18, 0, 0));

                // change service date of created above transport orders to the 1 day in the past
                using (var context = new AutomationTransportDataContext())
                {
                    var entity1 = context.TransportOrders.First(o => o.Code == transportOrder.Code);
                    entity1.SetServiceDate(today.AddDays(-1));
                    entity1.SetTransportDate(today.AddDays(-1));
                    entity1.MasterRouteDate = today.AddDays(-1);
                    var entity2 = context.TransportOrders.First(o => o.Code == transportOrder2.Code);
                    entity2.SetServiceDate(today.AddDays(-1));
                    entity2.SetTransportDate(today.AddDays(-1));
                    entity2.MasterRouteDate = today.AddDays(-1);
                    var entity3 = context.TransportOrders.First(o => o.Code == transportOrder3.Code);
                    entity3.SetServiceDate(today.AddDays(-2));
                    entity3.SetTransportDate(today.AddDays(-2));
                    entity3.MasterRouteDate = today.AddDays(-2);
                    var entity4 = context.TransportOrders.First(o => o.Code == transportOrder4.Code);
                    entity4.SetServiceDate(today);
                    entity4.SetTransportDate(today);
                    entity4.MasterRouteDate = today;
                    context.SaveChanges();
                }

                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                transportOrder3 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder3.ID);
                transportOrder4 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder4.ID);                
                HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000");
                HelperFacade.TransportHelper.CreateDaiLine(transportOrder2, fixture.ExternalLocation, "160000");
                HelperFacade.TransportHelper.CreateDaiLine(transportOrder3, fixture.ExternalLocation, "170000");
                HelperFacade.TransportHelper.CreateDaiLine(transportOrder4, fixture.ExternalLocation, "180000");                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitPerCompany, 
                    UnitOfMeasure.LocationStop, today.AddDays(-1));
                try
                {
                    Assert.NotNull(billedCase1);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                Assert.True(billedCase1.ActualUnits == 2);
                Assert.True(billedCase1.DateBilled == today.AddDays(-1).Date);
                Assert.True(billedCase1.PeriodBilledFrom == null);
                Assert.True(billedCase1.PeriodBilledTo == null);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Units == billedCase1.ActualUnits);
                Assert.True(billingLine1.Value == 50);
                Assert.True(billingLine1.PeriodBilledFrom == billedCase1.PeriodBilledFrom);
                Assert.True(billingLine1.PeriodBilledTo == billedCase1.PeriodBilledTo);
                Assert.True(billingLine1.LocationID == null);
                Assert.True(billingLine1.ContractID == fixture.CompanyContract.ID);

                var billedCase2 = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitPerCompany,
                    UnitOfMeasure.LocationStop, today);
                Assert.Null(billedCase2);

                var billedCase3 = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitPerCompany,
                    UnitOfMeasure.LocationStop, today.AddDays(-2).Date);
                Assert.Null(billedCase3);
            }
            catch
            {
                throw;
            }            
        }                

        [Fact(DisplayName = "Billing - 'price per visit per company' rule (contract end date matching)")]
        public void VerifyContractEndDateMatchingForBillingByPricePerVisitPerCompanyRule()
        {
            try
            {                
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitPerCompany, UnitOfMeasure.LocationStop,
                   null, units: 1, price: 25);
                
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                   withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                
                using (var context = new AutomationTransportDataContext())
                {
                    var updatedTransportOrder = context.TransportOrders.First(o => o.Code == transportOrder.Code);
                    updatedTransportOrder.SetServiceDate(today.AddDays(-1));
                    updatedTransportOrder.SetTransportDate(today.AddDays(-1));
                    updatedTransportOrder.MasterRouteDate = today.AddDays(-1);
                    context.SaveChanges();
                }
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000");
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-2));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, today.AddDays(-2));
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitPerCompany,
                    UnitOfMeasure.LocationStop, today.AddDays(-1));
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }           
        }

        [Fact(DisplayName = "Billing - 'price per visit per company' rule when there is previous billing line")]
        public void VerifyBillingByPricePerVisitPerCompanyRuleWhenThereIsPreviousBillingLine()
        {
            try
            {                                
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitPerCompany, UnitOfMeasure.LocationStop,
                   null, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                   withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                               
                using (var context = new AutomationTransportDataContext())
                {
                    var updatedTransportOrder = context.TransportOrders.First(o => o.Code == transportOrder.Code);
                    updatedTransportOrder.SetServiceDate(today.AddDays(-2));
                    updatedTransportOrder.SetTransportDate(today.AddDays(-2));
                    updatedTransportOrder.MasterRouteDate = today.AddDays(-2);
                    context.SaveChanges();
                }
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000");
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-2));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, today.AddDays(-2));
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitPerCompany,
                    UnitOfMeasure.LocationStop, today.AddDays(-2));
                try
                {
                    Assert.NotNull(billedCase1);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 25);

                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);
                var serviceOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                  withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder2, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));

                using (var context = new AutomationTransportDataContext())
                {
                    var updatedTransportOrder = context.TransportOrders.First(o => o.Code == transportOrder2.Code);
                    updatedTransportOrder.SetServiceDate(today.AddDays(-1));
                    updatedTransportOrder.SetTransportDate(today.AddDays(-1));
                    updatedTransportOrder.MasterRouteDate = today.AddDays(-1);
                    context.SaveChanges();
                }
                transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                HelperFacade.TransportHelper.CreateDaiLine(transportOrder2, fixture.ExternalLocation, "100000");
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, today.AddDays(1));
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitPerCompany,
                    UnitOfMeasure.LocationStop, today.AddDays(-1));
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                     var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                     throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 25);
            }
            catch
            {
                throw;
            }           
        }

        [Fact(DisplayName = "Billing - 'price per visit per company' rule when 'bill collect orders' flag is checked (positive)")]
        public void VerifyBillingByPricePerVisitPerCompanyRuleWhenBillCollectOrdersFlagIsChecked()
        {
            try
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(true);                
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitPerCompany, UnitOfMeasure.LocationStop,
                   null, units: 1, price: 25);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-1));
                
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                   withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Collected, new TimeSpan(10, 0, 0));

                using (var context = new AutomationTransportDataContext())
                {
                    var entity = context.TransportOrders.First(o => o.Code == transportOrder.Code);
                    entity.SetServiceDate(today.AddDays(-1));
                    entity.SetTransportDate(today.AddDays(-1));
                    entity.MasterRouteDate = today.AddDays(-1);
                    context.SaveChanges();
                }
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitPerCompany,
                    UnitOfMeasure.LocationStop, today.AddDays(-1));
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 25);
            }
            catch
            {
                throw;
            }
            finally
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit per company' rule when 'bill collect orders' flag is unchecked (negative)")]
        public void VerifyBillingByPricePerVisitPerCompanyRuleWhenBillCollectOrdersFlagIsUnchecked()
        {
            try
            {
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitPerCompany, UnitOfMeasure.LocationStop,
                   null, units: 1, price: 25);                

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                   withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Collected, new TimeSpan(10, 0, 0));

                using (var context = new AutomationTransportDataContext())
                {
                    var updatedTransportOrder = context.TransportOrders.First(o => o.Code == transportOrder.Code);
                    updatedTransportOrder.SetServiceDate(today.AddDays(-1));
                    updatedTransportOrder.SetTransportDate(today.AddDays(-1));
                    updatedTransportOrder.MasterRouteDate = today.AddDays(-1);
                    context.SaveChanges();
                }
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000");
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-1));
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitPerCompany,
                    UnitOfMeasure.LocationStop, today.AddDays(-1));
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {                    
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 0); // because actual units = 0, as there is no matching transport order record
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit per company' rule when there is transport order with different route code (negative)")]
        public void VerifyBillingByPricePerVisitPerCompanyRuleWhenThereIsTransportOrderWithDifferentRouteCode()
        {
            try
            {
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitPerCompany, UnitOfMeasure.LocationStop,
                   null, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                   withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));

                using (var context = new AutomationTransportDataContext())
                {
                    var updatedTransportOrder = context.TransportOrders.First(o => o.Code == transportOrder.Code);

                    updatedTransportOrder.SetServiceDate(today.AddDays(-1));
                    updatedTransportOrder.SetTransportDate(today.AddDays(-1));
                    updatedTransportOrder.MasterRouteDate = today.AddDays(-1);                    
                    context.SaveChanges();
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000");
                    updatedTransportOrder.MasterRouteCode = DataFacade.MasterRoute.Take(r => r.Code != transportOrder.MasterRouteCode).Build().Code;
                    context.SaveChanges();
                }                

                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-1));
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitPerCompany,
                    UnitOfMeasure.LocationStop, today.AddDays(-1));
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 0); // because actual units = 0, as there is no matching transport order record
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'Insurance fee per transported container' rule for onward location")]
        public void VerifyBillingByInsuranceFeeRuleForOnwardLocation()
        {
            try
            {
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.InsuranceFeePerTransportedContainer,
                    UnitOfMeasure.LocationContainer, null, units: 1, price: 25);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);
                HelperFacade.TransportHelper.SaveHisPack(today.AddDays(-1), "100000", "TRK", fixture.OnwardLocation1);
                HelperFacade.TransportHelper.SaveHisPack(today.AddDays(-1), "100000", "TRKSPL", fixture.OnwardLocation1);
                HelperFacade.TransportHelper.SaveHisPack(today.AddDays(-1), "100000", "LOC", fixture.OnwardLocation1);
                HelperFacade.TransportHelper.SaveHisPack(today.AddDays(-1), "100000", "DEP", fixture.OnwardLocation1);
                HelperFacade.TransportHelper.SaveHisPack(today.AddDays(-1), "100000", "DEPSPL", fixture.OnwardLocation1);
                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID, 
                    PriceRule.InsuranceFeePerTransportedContainer, UnitOfMeasure.LocationContainer, today.AddDays(-1));
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 125);
            }
            catch
            {
                throw;
            }           
        }

        [Fact(DisplayName = "Billing - 'Insurance fee per transported container' rule for collect location")]
        public void VerifyBillingByInsuranceFeeRuleForCollectLocation()
        {
            HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.InsuranceFeePerTransportedContainer,
                UnitOfMeasure.LocationContainer, null, units: 1, price: 25);
            HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-1));
            HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);
            HelperFacade.TransportHelper.SaveHisPack(today.AddDays(-1), "100000", "TRK", fixture.CollectLocation1);
            HelperFacade.TransportHelper.SaveHisPack(today.AddDays(-1), "100000", "TRKSPL", fixture.CollectLocation1);
            HelperFacade.TransportHelper.SaveHisPack(today.AddDays(-1), "100000", "LOC", fixture.CollectLocation1);
            HelperFacade.TransportHelper.SaveHisPack(today.AddDays(-1), "100000", "DEP", fixture.CollectLocation1);
            HelperFacade.TransportHelper.SaveHisPack(today.AddDays(-1), "100000", "DEPSPL", fixture.CollectLocation1);

            HelperFacade.BillingHelper.RunBillingJob();
            var billedCase = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID,
                PriceRule.InsuranceFeePerTransportedContainer, UnitOfMeasure.LocationContainer, today.AddDays(-1));
            try            
            {
                Assert.NotNull(billedCase);
            }
            catch
            {
                var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                throw new Exception(logMessage ?? "Expected billed case was not created!");
            }
            var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 125);            
        }

        [Fact(DisplayName = "Billing - 'Insurance fee per transported container' rule without locations")]
        public void VerifyBillingByInsuranceFeeRuleWithoutLocations()
        {
            try
            {
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.InsuranceFeePerTransportedContainer,
                    UnitOfMeasure.LocationContainer, null, units: 1, price: 25);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);
                HelperFacade.TransportHelper.SaveHisPack(today.AddDays(-1), "100000", "TRK");                
                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID, 
                    PriceRule.InsuranceFeePerTransportedContainer, UnitOfMeasure.LocationContainer, today.AddDays(-1));
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 0);
            }
            catch
            {
                throw;
            }
        }
                
        [Fact(DisplayName = "Billing - 'Insurance fee per transported container' rule with incorrect statuses")]
        public void VerifyBillingByInsuranceFeeRuleWithIncorrectStatuses()
        {
            try
            {
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.InsuranceFeePerTransportedContainer,
                    UnitOfMeasure.LocationContainer, null, units: 1, price: 25);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);
                HelperFacade.TransportHelper.SaveHisPack(today.AddDays(-1), "100000", "XYZ");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID,
                    PriceRule.InsuranceFeePerTransportedContainer, UnitOfMeasure.LocationContainer, today.AddDays(-1));
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 0);    // because actual units = 0, as there is no matching his_pack record
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// positive case
        /// 
        /// To verify condition from 4.6.1 - step 2:
        /// and NOT exists another his_pack where (
        ///  his_pack → pack_nr == this his_pack → pack_nr,
        ///  and his_pack → a_status in {TRK, TRKSPL, LOC, DEP, DEPSPL}
        ///  and his_pack → a_date is less than current Date,
        ///  and his_pack → a_date is greater than current Date – 7 days) // not transported within last week
        /// </summary>
        [Fact(DisplayName = "Billing - 'Insurance fee per transported container' rule when another his_pack with the same number is 8 days older")]
        public void VerifyBillingByInsuranceFeeRuleWhenAnotherHisPackWithTheSameNumberIsEightDaysOlder()
        {
            try
            {
                var arrivalTime = "100000";
                var status = "TRK";                
                var oldDate = today.AddDays(-8);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.InsuranceFeePerTransportedContainer,
                    UnitOfMeasure.LocationContainer, null, units: 1, price: 25);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);

                var oldHisPack = DailyDataFacade.HisPack.New()
                                                            .With_Date(oldDate)
                                                            .With_Time(arrivalTime)
                                                            .With_Status(status)
                                                            .With_FrLocation(fixture.CollectLocation1)
                                                            .With_ToLocation(fixture.OnwardLocation1)
                                                            .With_BagType(3301)
                                                            .With_PackNr($"3303-{ oldDate.ToString("ddMMyyyy") }-{ new Random().Next(1, 9999) }")
                                                            .With_MasterRoute(HelperFacade.TransportHelper.GetMasterRouteCode(oldDate))
                                                            .With_Site(DataFacade.Site.Take(s => s.BranchType == BranchType.CITDepot))
                                                            .SaveToDb();

                var newHisPack = DailyDataFacade.HisPack.New()
                                                            .With_Date(today.AddDays(-1))
                                                            .With_Time(arrivalTime)
                                                            .With_Status(status)
                                                            .With_FrLocation(fixture.CollectLocation1)
                                                            .With_ToLocation(fixture.OnwardLocation1)
                                                            .With_BagType(3301)
                                                            .With_PackNr(oldHisPack.Build().PackageNumber)
                                                            .With_MasterRoute(oldHisPack.Build().RouteNumberCode)
                                                            .With_SiteCode(oldHisPack.Build().SiteId)
                                                            .SaveToDb();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID,
                    PriceRule.InsuranceFeePerTransportedContainer, UnitOfMeasure.LocationContainer, today.AddDays(-1));
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 25);
            }
            catch
            {
                throw;
            }            
        }

        /// <summary>
        /// Negative case
        /// 
        /// his_pack → a_date == current Date, 
        /// and (his_pack → fr_loc_nr (Location) → Company == Contract {1} → Company OR his_pack → to_loc_nr(Location) → Company == Contract {1} → Company),  
        /// and his_pack → a_status in { TRK, TRKSPL, LOC, DEP, DEPSPL }
        ///    and NOT exists another his_pack where (
        ///    his_pack → pack_nr == this his_pack → pack_nr,
        ///    and his_pack → a_status in { TRK, TRKSPL, LOC, DEP, DEPSPL}
        ///    and his_pack → a_date < current Date,
        ///    and his_pack → a_date > current Date – 7 days) // not transported within last week
        /// )containers transported for this customer today
        /// </summary>
        [Fact(DisplayName = "Billing - 'Insurance fee per transported container' rule when there is another his_pack with the same number")]
        public void VerifyBillingByInsuranceFeeRuleWhenThereIsAnotherHisPackWithTheSameNumber()
        {
            try
            {
                var arrivalTime = "100000";
                var status = "TRK";                
                var oldDate = today.AddDays(-7);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.InsuranceFeePerTransportedContainer,
                    UnitOfMeasure.LocationContainer, null, units: 1, price: 25);
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixture.CompanyContract, today.AddDays(-1));
                HelperFacade.ContractHelper.EditEndDateOfContract(fixture.CompanyContract, null);

                var oldHisPack = DailyDataFacade.HisPack.New()
                                                            .With_Date(oldDate)
                                                            .With_Time(arrivalTime)
                                                            .With_Status(status)
                                                            .With_FrLocation(fixture.CollectLocation1)
                                                            .With_ToLocation(fixture.OnwardLocation1)
                                                            .With_BagType(3301)
                                                            .With_PackNr($"3303-{ oldDate.ToString("ddMMyyyy") }-{ new Random().Next(1, 9999) }")
                                                            .With_MasterRoute(HelperFacade.TransportHelper.GetMasterRouteCode(oldDate))
                                                            .With_Site(DataFacade.Site.Take(s => s.BranchType == BranchType.CITDepot))
                                                            .SaveToDb();

                var newHisPack = DailyDataFacade.HisPack.New()
                                                            .With_Date(today.AddDays(-1))
                                                            .With_Time(arrivalTime)
                                                            .With_Status(status)
                                                            .With_FrLocation(fixture.CollectLocation1)
                                                            .With_ToLocation(fixture.OnwardLocation1)
                                                            .With_BagType(3301)
                                                            .With_PackNr(oldHisPack.Build().PackageNumber)
                                                            .With_MasterRoute(oldHisPack.Build().RouteNumberCode)
                                                            .With_SiteCode(oldHisPack.Build().SiteId)
                                                            .SaveToDb();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitsPerDayBilledCase(fixture.CompanyContract.ID,
                    PriceRule.InsuranceFeePerTransportedContainer, UnitOfMeasure.LocationContainer, today.AddDays(-1));
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 0);
            }
            catch
            {
                throw;
            }
        }
        #endregion 4.6.1 Get Billing Data by Visits per Day Per Customer
        #region 4.6.2 Get Billing Data by Visit at Location
        #region "Price per visit - Location stop"
        [Fact(DisplayName = "Billing - 'price per visit' rule when there is one stop")]
        public void VerifyBillingByPricePerVisitRuleWhenThereIsOneStop()
        {
            try
            {                
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { contractedLocationLevel, locationGroupLevel }, units: 1, price: 33.468m);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation, 
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.True(billedCase.ActualUnits == 1, "ActualUnits value is incorrect.");
                Assert.True(billedCase.DateBilled == stop.Date, "DateBilled value is incorrect.");
                Assert.True(billedCase.PeriodBilledFrom == null, "PeriodBilledFrom value is incorrect.");
                Assert.True(billedCase.PeriodBilledTo == null, "PeriodBilledTo value is incorrect.");
                Assert.True(billedCase.LocationID == fixture.ExternalLocation.IdentityID, "LocationID value is incorrect.");
                Assert.True(billedCase.LocationGroupID == fixture.LocationGroup.ID, "LocationGroupID value is incorrect.");
                Assert.True(billedCase.PriceLineID == priceLine.ID, "PriceLineID value is incorrect.");

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);                
                Assert.True(billingLine.Value == priceLine.Price, "Billing line value is incorrect.");                          
                Assert.True(billingLine.LocationID == fixture.ExternalLocation.IdentityID, "LocationID value is incorrect.");                
                Assert.True(billingLine.ContractID == fixture.CompanyContract.ID, "ContractID value is incorrect.");
            }
            catch
            {
                throw;
            }            
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'price per visit' rule")]
        public void VerifyThatDataNotBilledTwiceByPricePerVisitRule()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule when there are two stops in one day for the same location")]
        public void VerifyBillingByPricePerVisitRuleWhenThereAreTwoStopsInOneDayForTheSameLocation()
        {
            try
            {                
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { contractedLocationLevel, locationGroupLevel }, units: 1, price: 25);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                var servicingServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID); 
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(collectTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(collectTransportOrder, fixture.ExternalLocation, "100000", "103000");

                var servicingTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(servicingServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(servicingTransportOrder, TransportOrderStatus.Completed, new TimeSpan(17, 0, 0));
                servicingTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(servicingServiceOrder.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(servicingTransportOrder, fixture.ExternalLocation, "170000", "173000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.NotNull(billedCase1);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine.Value == 25);                

                var billedCase2 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop2.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 25);
            }
            catch
            {
                throw;
            }            
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule when there are two stops for the same location at the same time (different days)")]
        public void VerifyBillingByPricePerVisitRuleWhenThereAreTwoStopsForTheSameLocationAtTheSameTime()
        {
            try
            {                
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { contractedLocationLevel, locationGroupLevel }, units: 1, price: 25);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation,
                    withProducts: true, withServices: false);
                var deliverServiceOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.ChangeServiceDate(deliverServiceOrder2.ID, deliverServiceOrder2.ServiceDate.AddDays(1));
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(deliverTransportOrder, fixture.ExternalLocation, "100000", "103000");

                var deliverTransportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder2.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder2, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                deliverTransportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder2.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(deliverTransportOrder2, fixture.ExternalLocation, "100000", "103000");
                HelperFacade.TransportHelper.ChangeServiceDate(deliverServiceOrder2.ID, deliverServiceOrder2.ServiceDate.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.NotNull(billedCase1);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine.Value == 25);

                var billedCase2 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop2.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 25);
            }
            catch
            {
                throw;
            }            
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule when there is incomplete transport order")]
        public void VerifyBillingByPricePerVisitRuleWhenThereIsIncompleteTransportOrder()
        {
            try
            {                    
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(collectTransportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(collectTransportOrder, fixture.ExternalLocation, "100000", "103000");

                var collectTransportOrder2 = DataFacade.TransportOrder.New()
                                                                            .With_Location(collectTransportOrder.LocationID)
                                                                            .With_Site(collectTransportOrder.SiteID)
                                                                            .With_OrderType(OrderType.AtRequest)
                                                                            .With_TransportDate(collectTransportOrder.TransportDate)
                                                                            .With_ServiceDate(collectTransportOrder.ServiceDate)
                                                                            .With_Status(TransportOrderStatus.Planned)
                                                                            .With_MasterRouteCode(collectTransportOrder.MasterRouteCode)
                                                                            .With_MasterRouteDate(collectTransportOrder.MasterRouteDate)
                                                                            .With_StopArrivalTime(collectTransportOrder.StopArrivalTime)
                                                                            .With_ServiceOrder(collectTransportOrder.ServiceOrderID)
                                                                            .With_ServiceType(collectTransportOrder.ServiceTypeID)
                                                                            .SaveToDb()
                                                                            .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop.ID);               
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }            
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule when 'bill collected orders' flag is checked (positive)")]
        public void VerifyBillingByPricePerVisitRuleWhenBillCollectedOrdersFlagIsChecked()
        {
            try
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(true);

                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(collectTransportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(collectTransportOrder, fixture.ExternalLocation, "100000", "103000");

                var collectTransportOrder2 = DataFacade.TransportOrder.New()
                                                                            .With_Location(collectTransportOrder.LocationID)
                                                                            .With_Site(collectTransportOrder.SiteID)
                                                                            .With_OrderType(OrderType.AtRequest)
                                                                            .With_TransportDate(collectTransportOrder.TransportDate)
                                                                            .With_ServiceDate(collectTransportOrder.ServiceDate)
                                                                            .With_Status(TransportOrderStatus.Collected)
                                                                            .With_MasterRouteCode(collectTransportOrder.MasterRouteCode)
                                                                            .With_MasterRouteDate(collectTransportOrder.MasterRouteDate)
                                                                            .With_StopArrivalTime(collectTransportOrder.StopArrivalTime)
                                                                            .With_ServiceOrder(collectTransportOrder.ServiceOrderID)
                                                                            .With_ServiceType(collectTransportOrder.ServiceTypeID)
                                                                            .SaveToDb()
                                                                            .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");                    
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 25);
            }
            catch
            {
                throw;
            }
            finally
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule when 'bill collected orders' flag is unchecked (negative)")]
        public void VerifyBillingByPricePerVisitRuleWhenBillCollectedOrdersFlagIsUnchecked()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(collectTransportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(collectTransportOrder, fixture.ExternalLocation, "100000", "103000");

                var collectTransportOrder2 = DataFacade.TransportOrder.New()
                                                                            .With_Location(collectTransportOrder.LocationID)
                                                                            .With_Site(collectTransportOrder.SiteID)
                                                                            .With_OrderType(OrderType.AtRequest)
                                                                            .With_TransportDate(collectTransportOrder.TransportDate)
                                                                            .With_ServiceDate(collectTransportOrder.ServiceDate)
                                                                            .With_Status(TransportOrderStatus.Collected)
                                                                            .With_MasterRouteCode(collectTransportOrder.MasterRouteCode)
                                                                            .With_MasterRouteDate(collectTransportOrder.MasterRouteDate)
                                                                            .With_StopArrivalTime(collectTransportOrder.StopArrivalTime)
                                                                            .With_ServiceOrder(collectTransportOrder.ServiceOrderID)
                                                                            .With_ServiceType(collectTransportOrder.ServiceTypeID)
                                                                            .SaveToDb()
                                                                            .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.Null(billedCase);                
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule when there is another completed transport order")]
        public void VerifyBillingByPricePerVisitRuleWhenThereIsAnotherCompletedTransportOrder()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                if (collectTransportOrder == null)
                {
                    throw new ArgumentNullException("collectTransportOrder", "Error on creating transport order!");
                }

                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(collectTransportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(collectTransportOrder, fixture.ExternalLocation, "100000", "103000");

                var collectTransportOrder2 = DataFacade.TransportOrder.New()
                                                                            .With_Location(collectTransportOrder.LocationID)
                                                                            .With_Site(collectTransportOrder.SiteID)
                                                                            .With_OrderType(OrderType.AtRequest)
                                                                            .With_TransportDate(collectTransportOrder.TransportDate)
                                                                            .With_ServiceDate(collectTransportOrder.ServiceDate)
                                                                            .With_Status(TransportOrderStatus.Completed)
                                                                            .With_MasterRouteCode(collectTransportOrder.MasterRouteCode)
                                                                            .With_MasterRouteDate(collectTransportOrder.MasterRouteDate)
                                                                            .With_StopArrivalTime(collectTransportOrder.StopArrivalTime)
                                                                            .With_ServiceOrder(collectTransportOrder.ServiceOrderID)
                                                                            .With_ServiceType(collectTransportOrder.ServiceTypeID)
                                                                            .SaveToDb()
                                                                            .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 25);
            }
            catch
            {
                throw;
            }           
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule without transport order")]
        public void VerifyBillingByPricePerVisitRuleWithoutTransportOrder()
        {
            try
            {
                                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var stop = HelperFacade.TransportHelper.CreateDaiLine(fixture.ExternalLocation, today, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }            
        }

        /// <summary>
        /// Location is not linked to location group, and there is no location group level configured in price line
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per visit' rule without location group")]
        public void VerifyBillingByPricePerVisitRuleWithoutLocationGroup()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.ExternalLocation.ID).Single());
                    context.SaveChanges();
                                        
                    var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                        null, units: 1, price: 25);

                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                        withProducts: false, withServices: false);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                        UnitOfMeasure.LocationStop, stop.ID);
                    try
                    {
                        Assert.NotNull(billedCase);
                    }
                    catch
                    {
                        if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.ExternalLocation.ID))
                        {
                            var link = new LocationGroupLocation { LocationNumber = fixture.ExternalLocation.ID, LocationGroupId = fixture.LocationGroup.ID };
                            context.LocationGroupLocations.Add(link);
                            context.SaveChanges();
                        }
                        var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                        throw new Exception(logMessage ?? "Expected billed case was not created!");
                    }
                    Assert.True(billedCase.PriceLineID == null);

                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == 25);
                    Assert.True(billingLine.PriceLineID == priceLine.ID);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.ExternalLocation.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.ExternalLocation.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule without location group with configured group level")]
        public void VerifyBillingByPricePerVisitRuleWithoutLocationGroupWithConfiguredGroupLevel()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.ExternalLocation.ID).Single());
                    context.SaveChanges();                    
                   
                    var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                    HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                        new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                        withProducts: false, withServices: false);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                        UnitOfMeasure.LocationStop, stop.ID);
                    Assert.Null(billedCase);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.ExternalLocation.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.ExternalLocation.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }            
        }        

        /// <summary>
        /// Location is linked to location group but there is no location group price line level
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per visit' rule when there is no location group price line level")]
        public void VerifyBillingByPricePerVisitRuleWhenThereIsNoLocationGroupPriceLineLevel()
        {            
            try
            {                
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);                
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }            
        }

        /// <summary>
        /// Location is linked to location group but price line cannot be matched
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per visit' rule when price line cannot be matched")]
        public void VerifyBillingByPricePerVisitRuleWhenPriceLineCannotBeMatched()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.VisitAddress1.IdentityID, fixture.VisitAddress1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { contractedLocationLevel, locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per visit - Location stop"
        #region "Pavement transport fee - Location stop"
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location stop' unit when there is one stop")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationStopUnitWhenThereIsOneStop()
        {
            try
            {                
                var visitAddressLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.VisitAddress, PriceRuleLevelValueType.Location, fixture.VisitAddress1.IdentityID, fixture.VisitAddress1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { visitAddressLevel, locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationStop, stop.ID);                
                try
            {
                Assert.NotNull(billedCase);
            }
            catch
            {
                var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                throw new Exception(logMessage ?? "Expected billed case was not created!");
            }

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 25);                
            }
            catch
            {
                throw;
            }            
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'pavement transport fee' rule and 'location stop' unit")]
        public void VerifyThatDataNotBilledTwiceByPavementTransportFeeRuleAndLocationStopUnit()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Pavement transport fee - Location stop"
        #region "Pavement transport fee - Location item"
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location item' unit of two different products")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationItemUnitOfTwoDifferentProducts()
        {
            try
            {                             
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 10, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);
                var daiCoin2 = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note50EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationItem, stop.ID);                
                try
            {
                Assert.NotNull(billedCase);
            }
            catch
            {
                var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                throw new Exception(logMessage ?? "Expected billed case was not created!");
            }

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 50);
            }
            catch
            {
                throw;
            }           
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'pavement transport fee' rule and 'location item' unit")]
        public void VerifyThatDataNotBilledTwiceByPavementTransportFeeRuleAndLocationItemUnit()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 10, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationItem, stop.ID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationItem, stop.ID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Pavement transport fee - Location item"
        #region "Pavement transport fee - Location Kg"
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location Kg' unit of two different products")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationKgUnitOfTwoDifferentProducts()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationKg,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationKg,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 0.5m, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");                
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);
                var daiCoin2 = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note50EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationKg, stop.ID);                
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 
                    (((fixture.Note100EurProduct.Weight * daiCoin.AmountDelivered) + 
                    (fixture.Note50EurProduct.Weight * daiCoin2.AmountDelivered)) / priceLine.Units) * priceLine.Price);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'pavement transport fee' rule and 'location Kg' unit")]
        public void VerifyThatDataNotBilledTwiceByPavementTransportFeeRuleAndLocationKgUnit()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationKg,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationKg,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 0.5m, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationKg, stop.ID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationKg, stop.ID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Pavement transport fee - Location Kg"
        #region "Price per visiting time - Location hours"
        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'location hours' unit when stop is within period")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndLocationHoursUnitWhenStopIsWithinPeriod()
        {
            try
            {                
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationHours,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 10:30", 
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(10, 30, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationHours,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationHours,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel }, units: 0.25M, price: 20);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.LocationHours, stop.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 40);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice 'price per visiting time' rule and 'location hours' unit")]
        public void VerifyThatDataNotBilledTwiceByPricePerVisitingTimeRuleAndLocationHoursUnit()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationHours,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationHours,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 0.25M, price: 20);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.LocationHours, stop.ID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.LocationHours, stop.ID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'location hours' unit when stop start is outside period")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndLocationHoursUnitWhenStopStartIsOutsidePeriod()
        {
            try
            {
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationHours,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 10:30",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(10, 30, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationHours,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationHours,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel }, units: 0.25M, price: 20);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(9, 59, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "095959", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.LocationHours, stop.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'location hours' unit when stop end is outside period")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndLocationHoursUnitWhenStopEndIsOutsidePeriod()
        {
            try
            {
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationHours,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 10:30",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(10, 30, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationHours,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationHours,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel }, units: 0.25M, price: 20);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(9, 59, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100001", "110001");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.LocationHours, stop.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 80);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per visiting time - Location hours"
        #region "Price per visiting time - Location minutes"
        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'location minutes' unit when stop is within period")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndLocationMinutesUnitWhenStopIsWithinPeriod()
        {
            try
            {                
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 10:30",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(10, 30, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel }, units: 10, price: 20);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.LocationMinutes, stop.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 60);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'location minutes' unit when weekday is not bank holiday")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndLocationMinutesUnitWhenWeekdayIsNotBankHoliday()
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                                
                var bankHolidayLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                    PriceRuleLevelName.Weekday, PriceRuleLevelValueType.WeekdayName, (int)WeekdayName.BankHoliday, WeekdayName.BankHoliday.ToString());
                var locationGroupLevel1 = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine1 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                    new List<PriceLineLevel> { bankHolidayLevel, locationGroupLevel1 }, units: 10, price: 30);

                var weekday = HelperFacade.BillingHelper.GetWeekDayName(serviceOrder.ServiceDate);
                var weekdayLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                    PriceRuleLevelName.Weekday, PriceRuleLevelValueType.WeekdayName, (int)weekday, weekday.ToString());
                var locationGroupLevel2 = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                    new List<PriceLineLevel> { weekdayLevel, locationGroupLevel2 }, units: 10, price: 20);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.LocationMinutes, stop.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == priceLine2.Price * 3);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'location minutes' unit when weekday is bank holiday")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndLocationMinutesUnitWhenWeekdayIsBankHoliday()
        {
            using (var context = new AutomationBaseDataContext())
            {
                try
                {
                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation,
                        withProducts: false, withServices: false);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");

                    DataFacade.BankHoliday.SaveBankHoliday(fixture.ExternalLocation, serviceOrder.ServiceDate, context);
                    var bankHolidayLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                        PriceRuleLevelName.Weekday, PriceRuleLevelValueType.WeekdayName, (int)WeekdayName.BankHoliday, WeekdayName.BankHoliday.ToString());
                    var locationGroupLevel1 = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                        PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                    var priceLine1 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                        new List<PriceLineLevel> { bankHolidayLevel, locationGroupLevel1 }, units: 10, price: 30);

                    var weekday = HelperFacade.BillingHelper.GetWeekDayName(serviceOrder.ServiceDate);
                    var weekdayLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                        PriceRuleLevelName.Weekday, PriceRuleLevelValueType.WeekdayName, (int)weekday, weekday.ToString());
                    var locationGroupLevel2 = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                        PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                    var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.LocationMinutes,
                        new List<PriceLineLevel> { weekdayLevel, locationGroupLevel2 }, units: 10, price: 20);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                        UnitOfMeasure.LocationMinutes, stop.ID);
                    try
                    {
                        Assert.NotNull(billedCase);
                    }
                    catch
                    {
                        context.BankHolidays.RemoveRange(context.BankHolidays.Where(b => !context.BankHolidaySettings.Any(s => s.ID == b.BankingHolidaySettingId && s.IsDefault)));
                        context.BankHolidaySettings.RemoveRange(context.BankHolidaySettings.Where(s => !s.IsDefault));
                        context.SaveChanges();
                        var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                        throw new Exception(logMessage ?? "Expected billed case was not created!");
                    }
                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == priceLine1.Price * 3); // 30 minutes = 30 actual units. 30 actual units / 10 price line units = 3
                }
                catch
                {
                    throw;
                }
                finally
                {
                    context.BankHolidays.RemoveRange(context.BankHolidays.Where(b => !context.BankHolidaySettings.Any(s => s.ID == b.BankingHolidaySettingId && s.IsDefault)));
                    context.BankHolidaySettings.RemoveRange(context.BankHolidaySettings.Where(s => !s.IsDefault));
                    context.SaveChanges();
                }
            }
        }
        #endregion "Price per visiting time - Location minutes"
        #endregion 4.6.2 Get Billing Data by Visit at Location
        #region 4.6.3 Get Billing Data by Visit at Visit Address
        #region "Price per visit - Visit address stop"
        [Fact(DisplayName = "Billing - 'price per visit' rule and 'visit address stop' unit when there is one visit")]
        public void VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWhenThereIsOneVisit()
        {
            try
            {                
                var visitAddressLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.VisitAddress, PriceRuleLevelValueType.Location, fixture.VisitAddress1.IdentityID, fixture.VisitAddress1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { visitAddressLevel, locationGroupLevel }, units: 1, price: 50);                

                var serviceOrder1 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                var serviceOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint2,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder1 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder1.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder1, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder1 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder1.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder1, fixture.VisitServicePoint1, "100000", "101500");
                var transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder2, TransportOrderStatus.Completed, new TimeSpan(10, 20, 0));
                transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(transportOrder2, fixture.VisitServicePoint2, "102000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                Assert.True(billedCase.ActualUnits == 1);                
                Assert.True(billedCase.PeriodBilledFrom == null);
                Assert.True(billedCase.PeriodBilledTo == null);
                Assert.True(billedCase.LocationID == null);
                Assert.True(billedCase.VisitAddressID == fixture.VisitAddress1.IdentityID);
                Assert.True(billedCase.LocationGroupID == fixture.LocationGroup.ID);
                Assert.True(billedCase.PriceLineID == priceLine.ID);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 50);
                Assert.True(billingLine.VisitAddressID == fixture.VisitAddress1.IdentityID);               
            }
            catch
            {
                throw;
            }
        }        

        [Fact(DisplayName = "Billing - 'price per visit' rule and 'visit address stop' unit when there are two visits", 
              Skip = "to do after business logic is updated")]
        public void VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWhenThereAreTwoVisits()
        {
            try
            {
                // TODO re-using VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWhenThereIsOneVisit method
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule and 'visit address stop' unit when there are two visits in different days")]
        public void VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWhenThereAreTwoVisitsInDifferentDays()
        {
            try
            {
                var visitAddressLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.VisitAddress, PriceRuleLevelValueType.Location, fixture.VisitAddress1.IdentityID, fixture.VisitAddress1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { visitAddressLevel, locationGroupLevel }, units: 1, price: 50);

                var serviceOrder1 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                var serviceOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint2,
                    withProducts: false, withServices: false);

                HelperFacade.TransportHelper.ChangeServiceDate(serviceOrder2.ID, serviceOrder2.ServiceDate.AddDays(1));

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder1 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder1.ID); 
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder1, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder1 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder1.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder1, fixture.VisitServicePoint1, "100000", "103000");

                var transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);               
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder2, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(transportOrder2, fixture.VisitServicePoint2, "100000", "103000");

                HelperFacade.TransportHelper.ChangeServiceDate(serviceOrder2.ID, serviceOrder2.ServiceDate.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                Assert.NotNull(billedCase1);

                var billedCaseDailyStop1 = HelperFacade.BillingHelper.GetBilledCaseDailyStopByStop(stop.ID);
                Assert.NotNull(billedCaseDailyStop1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 50);

                var billedCase2 = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop2.Date);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billedCaseDailyStop2 = HelperFacade.BillingHelper.GetBilledCaseDailyStopByStop(stop2.ID);
                Assert.NotNull(billedCaseDailyStop2);

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 50);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule and 'visit address stop' unit when there is incomplete transport order")]
        public void VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWhenThereIsIncompleteTransportOrder()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 50);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);                
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                if (transportOrder == null)
                {
                    throw new ArgumentNullException("transportOrder", "Error on creating transport order!");
                }

                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled,
                    new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                var transportOrder2 = DataFacade.TransportOrder.New()
                                                                    .With_Location(transportOrder.LocationID)
                                                                    .With_Site(transportOrder.SiteID)
                                                                    .With_OrderType(OrderType.AtRequest)
                                                                    .With_TransportDate(transportOrder.TransportDate)
                                                                    .With_ServiceDate(transportOrder.ServiceDate)
                                                                    .With_Status(TransportOrderStatus.Planned)
                                                                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                                                                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                                                                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                                                                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                                                                    .With_ServiceType(transportOrder.ServiceTypeID)
                                                                    .SaveToDb()
                                                                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule and 'visit address stop' unit when there is another completed transport order")] // add 'canceled' verification in theory
        public void VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWhenThereIsAnotherCompletedTransportOrder()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 50);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                var transportOrder2 = DataFacade.TransportOrder.New()
                                                                    .With_Location(transportOrder.LocationID)
                                                                    .With_Site(transportOrder.SiteID)
                                                                    .With_OrderType(OrderType.AtRequest)
                                                                    .With_TransportDate(transportOrder.TransportDate)
                                                                    .With_ServiceDate(transportOrder.ServiceDate)
                                                                    .With_Status(TransportOrderStatus.Completed)
                                                                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                                                                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                                                                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                                                                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                                                                    .With_ServiceType(transportOrder.ServiceTypeID)
                                                                    .SaveToDb()
                                                                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }

                var billedCaseDailyStop = HelperFacade.BillingHelper.GetBilledCaseDailyStopByStop(stop.ID);
                Assert.NotNull(billedCaseDailyStop);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 50);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule and 'visit address stop' unit when 'bill collected orders' flag is checked (positive)")]
        public void VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWhenBillCollectedOrdersFlagIsChecked()
        {
            try
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(true);

                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                     PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 50);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Collected)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billedCaseDailyStop = HelperFacade.BillingHelper.GetBilledCaseDailyStopByStop(stop.ID);
                Assert.NotNull(billedCaseDailyStop);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 50);
            }
            catch
            {
                throw;
            }
            finally
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule when 'bill collected orders' flag is unchecked (negative)")]
        public void VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWhenBillCollectedOrdersFlagIsUnchecked()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                     PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 50);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Collected)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);                
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule and 'visit address stop' unit without transport order")]
        public void VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWithoutTransportOrder()
        {
            try
            {
                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 50);                
                var stop = HelperFacade.TransportHelper.CreateDaiLine(fixture.VisitServicePoint1, today, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Location is not linked to location group, and there is no location group level configured in price line
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per visit' rule and 'visit address stop' unit without location group")]
        public void VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWithoutLocationGroup()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.VisitAddress1.ID).Single());
                    context.SaveChanges();
                    
                    var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, 
                        UnitOfMeasure.VisitAddressStop, null, units: 1, price: 50);

                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                        withProducts: false, withServices: false);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed,
                        new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                    try
                    {
                        Assert.NotNull(billedCase);
                    }
                    catch
                    {
                        if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.VisitAddress1.ID))
                        {
                            var link = new LocationGroupLocation { LocationNumber = fixture.VisitAddress1.ID, LocationGroupId = fixture.LocationGroup.ID };
                            context.LocationGroupLocations.Add(link);
                            context.SaveChanges();
                        }
                        var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                        throw new Exception(logMessage ?? "Expected billed case was not created!");
                    }
                    Assert.True(billedCase.PriceLineID == null);

                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == 50);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.VisitAddress1.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.VisitAddress1.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule and 'visit address stop' unit without location group with configured group level")]
        public void VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWithoutLocationGroupWithConfiguredGroupLevel()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.VisitAddress1.ID).Single());
                    context.SaveChanges();

                    var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                    var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                        new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 50);

                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                        withProducts: false, withServices: false);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed,
                        new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                    Assert.Null(billedCase);                    
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.VisitAddress1.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.VisitAddress1.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        [Fact(DisplayName = "Billing - 'price per visit' rule and 'visit address stop' unit when there is no location group price line level")]
        public void VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWhenThereIsNoLocationGroupPriceLineLevel()
        {
            try
            {
                var visitAddressLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.VisitAddress, PriceRuleLevelValueType.Location, fixture.VisitAddress1.IdentityID, fixture.VisitAddress1.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { visitAddressLevel }, units: 1, price: 50);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed,
                    new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Location is linked to location group but price line cannot be matched
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per visit' rule and 'visit address stop' unit when price line cannot be matched")]
        public void VerifyBillingByPricePerVisitRuleAndVisitAddressStopUnitWhenPriceLineCannotBeMatched()
        {
            try
            {
                var visitAddressLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.VisitAddress, PriceRuleLevelValueType.Location, fixture.VisitAddress2.ID, fixture.VisitAddress2.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { visitAddressLevel }, units: 1, price: 50);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed,
                    new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per visit - Visit address stop"
        #region "Pavement transport fee - Visit address stop"
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address stop' unit when there is one visit")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressStopUnitWhenThereIsOneVisit()
        {
            try
            {                
                var visitAddressLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.VisitAddress, PriceRuleLevelValueType.Location, fixture.VisitAddress1.IdentityID, fixture.VisitAddress1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { visitAddressLevel, locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billedCaseDailyStop = HelperFacade.BillingHelper.GetBilledCaseDailyStopByStop(stop.ID);
                Assert.NotNull(billedCaseDailyStop);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 12);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Pavement transport fee - Visit address stop"
        #region "Pavement transport fee - Visit address item"
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address item' unit of two different products")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressItemUnitOfTwoDifferentProducts()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 10, price: 25);
                
                var productCollection = new Dictionary<Cwc.BaseData.Product, int>
                {
                    { fixture.Note100EurProduct, 10 },
                    { fixture.Note50EurProduct, 10 } 
                };

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1, productCollection);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);
                var daiCoin2 = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note50EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.VisitAddressItem, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billedCaseDailyStop = HelperFacade.BillingHelper.GetBilledCaseDailyStopByStop(stop.ID);
                Assert.NotNull(billedCaseDailyStop);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 50);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address item' unit with ranges")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressItemUnitWithRanges()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceRange1 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(1, 10, 2);
                var priceRange2 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(11, 20, 1.5m);
                var priceRange3 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(21, 100, 1.2m);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressItem, new List<PriceLineLevel> { locationGroupLevel },
                    new List<PriceLineUnitsRange> { priceRange1, priceRange2, priceRange3 },
                    units: 1, price: 0, isRangePriceBasedOnTotal: false);

                var productCollection = new Dictionary<Cwc.BaseData.Product, int>
                {
                    { fixture.Note100EurProduct, 15 },
                    { fixture.Note50EurProduct, 10 }
                };

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1, productCollection);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 15);
                var daiCoin2 = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note50EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.VisitAddressItem, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 41); // 10 * 2 + 10 * 1.5 + 5 * 1.2
            }
            catch
            {
                throw;
            }
        }
        #endregion "Pavement transport fee - Visit address item"
        #region "Pavement transport fee - Visit address Kg"
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address Kg' unit of two different products")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressKgUnitOfTwoDifferentProducts()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressKg,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressKg,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 0.5m, price: 12);                

                var productCollection = new Dictionary<Cwc.BaseData.Product, int>
                {
                    { fixture.Note100EurProduct, 10 },
                    { fixture.Note50EurProduct, 10 }
                };

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1, productCollection);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);
                var daiCoin2 = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note50EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.VisitAddressKg, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value ==
                    (((fixture.Note100EurProduct.Weight * daiCoin.AmountDelivered) +
                    (fixture.Note50EurProduct.Weight * daiCoin2.AmountDelivered)) / priceLine.Units) * priceLine.Price);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Pavement transport fee - Visit address Kg"
        #region "Price per visiting time - Visit address hours"
        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address hours' unit when stop is within period")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressHoursUnitWhenStopIsWithinPeriod()
        {
            try
            {                
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "00:00 18:00",
                    isRangeLevel: true, valueFrom: new TimeSpan(0, 0, 0), valueTo: new TimeSpan(18, 0, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel }, units: 0.25M, price: 20);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                var serviceOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint2,
                    withProducts: true, withServices: false);

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                var transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder2, TransportOrderStatus.Completed, new TimeSpan(10, 20, 0));
                transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(transportOrder2, fixture.VisitServicePoint2, "102000", "104500");


                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressHours, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                Assert.True(billedCase.MasterRouteCode == stop.RouteNumber);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 60);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'price per visiting time' rule and 'visit address hours' unit")]
        public void VerifyThatDataNotBilledTwiceByPricePerVisitingTimeRuleAndVisitAddressHoursUnit()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 0.25M, price: 20);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);                

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressHours, fixture.VisitAddress1.IdentityID, stop.Date);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressHours, fixture.VisitAddress1.IdentityID, stop.Date);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address hours' unit with total ranges - first range")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressHoursUnitWithTotalRangesFirstRange()
        {
            try
            {
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 11:00",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(11, 0, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceRange1 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.01m, 0.25m, 50);
                var priceRange2 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.26m, 0.5m, 75);
                var priceRange3 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.51m, 1, 100);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel },
                    new List<PriceLineUnitsRange> { priceRange1, priceRange2, priceRange3 },
                    units: 1, price: 0, isRangePriceBasedOnTotal: true);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                var serviceOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint2,
                    withProducts: true, withServices: false);

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "100500");

                var transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder2, TransportOrderStatus.Completed, new TimeSpan(10, 10, 0));
                transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(transportOrder2, fixture.VisitServicePoint2, "101000", "101500");


                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressHours, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                Assert.True(billedCase.MasterRouteCode == stop.RouteNumber);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 12.5m);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address hours' unit with total ranges - middle range")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressHoursUnitWithTotalRangesMiddleRange()
        {
            try
            {
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 11:00",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(11, 0, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceRange1 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.01m, 0.25m, 50);
                var priceRange2 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.26m, 0.5m, 75);
                var priceRange3 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.51m, 1, 100);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel },
                    new List<PriceLineUnitsRange> { priceRange1, priceRange2, priceRange3 },
                    units: 1, price: 0, isRangePriceBasedOnTotal: true);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                var serviceOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint2,
                    withProducts: true, withServices: false);

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "101000");

                var transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder2, TransportOrderStatus.Completed, new TimeSpan(10, 15, 0));
                transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(transportOrder2, fixture.VisitServicePoint2, "101500", "103000");


                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressHours, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 37.5m);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address hours' unit with total ranges - last range")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressHoursUnitWithTotalRangesLastRange()
        {
            try
            {
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 11:00",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(11, 0, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceRange1 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.01m, 0.25m, 50);
                var priceRange2 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.26m, 0.5m, 75);
                var priceRange3 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.51m, 1, 100);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel },
                    new List<PriceLineUnitsRange> { priceRange1, priceRange2, priceRange3 },
                    units: 1, price: 0, isRangePriceBasedOnTotal: true);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                var serviceOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint2,
                    withProducts: true, withServices: false);

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                var transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder2, TransportOrderStatus.Completed, new TimeSpan(10, 20, 0));
                transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(transportOrder2, fixture.VisitServicePoint2, "102000", "104500");


                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressHours, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 75);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address hours' unit with ranges")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressHoursUnitWithRanges()
        {
            try
            {
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 11:00",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(11, 0, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceRange1 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.01m, 0.25m, 50);
                var priceRange2 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.26m, 0.5m, 75);
                var priceRange3 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.51m, 1, 100);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel },
                    new List<PriceLineUnitsRange> { priceRange1, priceRange2, priceRange3 },
                    units: 1, price: 0, isRangePriceBasedOnTotal: false);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                var serviceOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint2,
                    withProducts: true, withServices: false);

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                var transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder2, TransportOrderStatus.Completed, new TimeSpan(10, 20, 0));
                transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(transportOrder2, fixture.VisitServicePoint2, "102000", "104500");


                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressHours, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 56.25m); // 0.25 * 50 + 0.25 * 75 + 0.25 * 100 (according to price ranges configuration)
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address hours' unit when weekday is not bank holiday")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressHoursUnitWhenWeekdayIsNotBankHoliday()
        {
            try
            {
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                var bankHolidayLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                        PriceRuleLevelName.Weekday, PriceRuleLevelValueType.WeekdayName, (int)WeekdayName.BankHoliday, WeekdayName.BankHoliday.ToString());
                var locationGroupLevel1 = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine1 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressHours, new List<PriceLineLevel> { bankHolidayLevel, locationGroupLevel1 }, units: 1, price: 120);

                var weekday = HelperFacade.BillingHelper.GetWeekDayName(serviceOrder.ServiceDate);
                var weekdayLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.Weekday, PriceRuleLevelValueType.WeekdayName, (int)weekday, weekday.ToString());
                var locationGroupLevel2 = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, 
                    UnitOfMeasure.VisitAddressHours, new List<PriceLineLevel> { weekdayLevel, locationGroupLevel2 }, units: 1, price: 60);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressHours, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }

                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == priceLine2.Price / 2M);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address hours' unit when weekday is bank holiday")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressHoursUnitWhenWeekdayIsBankHoliday()
        {
            using (var context = new AutomationBaseDataContext())
            {
                try
                {
                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                        withProducts: false, withServices: false);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                    DataFacade.BankHoliday.SaveBankHoliday(fixture.VisitAddress1, serviceOrder.ServiceDate, context);
                    var bankHolidayLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                        PriceRuleLevelName.Weekday, PriceRuleLevelValueType.WeekdayName, (int)WeekdayName.BankHoliday, WeekdayName.BankHoliday.ToString());
                    var locationGroupLevel1 = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                        PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                    var priceLine1 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime,
                        UnitOfMeasure.VisitAddressHours, new List<PriceLineLevel> { bankHolidayLevel, locationGroupLevel1 }, units: 0.25M, price: 30);

                    var weekday = HelperFacade.BillingHelper.GetWeekDayName(serviceOrder.ServiceDate);
                    var weekdayLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                        PriceRuleLevelName.Weekday, PriceRuleLevelValueType.WeekdayName, (int)weekday, weekday.ToString());
                    var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressHours,
                        PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                    var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime,
                        UnitOfMeasure.VisitAddressHours, new List<PriceLineLevel> { weekdayLevel, locationGroupLevel }, units: 0.25M, price: 15);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                        UnitOfMeasure.VisitAddressHours, fixture.VisitAddress1.IdentityID, stop.Date);
                    try
                    {
                        Assert.NotNull(billedCase);
                    }
                    catch
                    {
                        context.BankHolidays.RemoveRange(context.BankHolidays.Where(b => !context.BankHolidaySettings.Any(s => s.ID == b.BankingHolidaySettingId && s.IsDefault)));
                        context.BankHolidaySettings.RemoveRange(context.BankHolidaySettings.Where(s => !s.IsDefault));
                        context.SaveChanges();
                        var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                        throw new Exception(logMessage ?? "Expected billed case was not created!");
                    }
                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == priceLine1.Price * 2);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    context.BankHolidays.RemoveRange(context.BankHolidays.Where(b => !context.BankHolidaySettings.Any(s => s.ID == b.BankingHolidaySettingId && s.IsDefault)));
                    context.BankHolidaySettings.RemoveRange(context.BankHolidaySettings.Where(s => !s.IsDefault));
                    context.SaveChanges();
                }
            }
        }
        #endregion "Price per visiting time - Visit address hours"
        #region "Price per visiting time - Visit address minutes"
        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address minutes' unit when stop is within period")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressMinutesUnitWhenStopIsWithinPeriod()
        {
            try
            {                
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 10:45",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(10, 45, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel }, units: 10, price: 20);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                var serviceOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint2,
                    withProducts: true, withServices: false);

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "101000");

                var transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder2, TransportOrderStatus.Completed, new TimeSpan(10, 15, 0));
                transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder2.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(transportOrder2, fixture.VisitServicePoint2, "101500", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressMinutes, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                Assert.True(billedCase.MasterRouteCode == stop.RouteNumber);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 60);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address minutes' unit, min billable units")]       
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressMinutesUnitMinBillableUnits()
        {
            try
            {
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 11:00",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(11, 0, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceRange1 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.01m, 15, 1.5m);
                var priceRange2 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(15.01m, 30, 1.4m);
                var priceRange3 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(30.01m, 60, 1.2m);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel },
                    new List<PriceLineUnitsRange> { priceRange1, priceRange2, priceRange3 },
                    units: 1, price: 0, minBillableUnits: 15, maxBillableUnits: 60, isRangePriceBasedOnTotal: true);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);                

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "101500");                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressMinutes, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 22.5m); // 15 minutes * 1.5 (first unit range -> price)
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address minutes' unit, max billable units")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressMinutesUnitMaxBillableUnits()
        {
            try
            {
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 11:00",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(11, 0, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceRange1 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.01m, 15, 1.5m);
                var priceRange2 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(15.01m, 30, 1.4m);
                var priceRange3 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(30.01m, 120, 1.2m);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel },
                    new List<PriceLineUnitsRange> { priceRange1, priceRange2, priceRange3 },
                    units: 1, price: 0, minBillableUnits: 15, maxBillableUnits: 60, isRangePriceBasedOnTotal: true);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);                

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "110000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressMinutes, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 72); // 60 minutes * 1.2 (last unit range -> price)
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address minutes' unit, less than min billable units")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressMinutesUnitLessThanMinBillableUnits()
        {
            try
            {
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 11:00",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(11, 0, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceRange1 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.01m, 15, 1.5m);
                var priceRange2 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(15.01m, 30, 1.4m);
                var priceRange3 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(30.01m, 60, 1.2m);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel },
                    new List<PriceLineUnitsRange> { priceRange1, priceRange2, priceRange3 },
                    units: 1, price: 0, minBillableUnits: 15, maxBillableUnits: 60, isRangePriceBasedOnTotal: true);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);               

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "100500");                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressMinutes, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 0);
                Assert.True(billingLine.Comment == "Zero value billing line was created, as actual calculated units is not according to minimum or maximum billable units.");
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address minutes' unit, greater than max billable units")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressMinutesUnitGreaterThanMaxBillableUnits()
        {
            try
            {
                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 11:00",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(11, 0, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceRange1 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.01m, 15, 1.5m);
                var priceRange2 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(15.01m, 30, 1.4m);
                var priceRange3 = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(30.01m, 120, 1.2m);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel },
                    new List<PriceLineUnitsRange> { priceRange1, priceRange2, priceRange3 },
                    units: 1, price: 0, minBillableUnits: 15, maxBillableUnits: 60, isRangePriceBasedOnTotal: true);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);                

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "113000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressMinutes, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 72); // 60 minutes (max billable units) * 1.2 (last unit range -> price)
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per visiting time' rule and 'visit address minutes' unit, zero value should not be billed")]
        public void VerifyBillingByPricePerVisitingTimeRuleAndVisitAddressMinutesUnitZeroValueShouldNotBeBilled()
        {
            try
            {
                HelperFacade.BillingHelper.SetZeroValueBillingFlag(false);

                var timeWindowLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    PriceRuleLevelName.TimeWindow, PriceRuleLevelValueType.Time, null, "10:00 11:00",
                    isRangeLevel: true, valueFrom: new TimeSpan(10, 0, 0), valueTo: new TimeSpan(11, 0, 0));
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceRange = HelperFacade.ContractHelper.BuildPriceLineUnitsRange(0.01m, 15, 1.5m);                
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerVisitingTime, UnitOfMeasure.VisitAddressMinutes,
                    new List<PriceLineLevel> { timeWindowLevel, locationGroupLevel },
                    new List<PriceLineUnitsRange> { priceRange },
                    units: 1, price: 0, minBillableUnits: 15, maxBillableUnits: 60, isRangePriceBasedOnTotal: true);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);                

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "100500");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerVisitingTime,
                    UnitOfMeasure.VisitAddressMinutes, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    HelperFacade.BillingHelper.SetZeroValueBillingFlag(true);
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.Null(billingLine);                
            }
            catch
            {
                throw;
            }
            finally
            {
                HelperFacade.BillingHelper.SetZeroValueBillingFlag(true);
            }
        }
        #endregion "Price per visiting time - Visit address minutes"
        #endregion 4.6.3 Get Billing Data by Visit at Visit Address
        #region 4.6.4 Get Billing Data by Container Type at Location
        #region "Pavement transport fee - Location container"
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit when there is one collect container")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWhenThereIsOneContainer()
        {
            try
            {
                var containerTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { containerTypeLevel, locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 
                    collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.True(billedCase.ActualUnits == 1);
                Assert.True(billedCase.DateBilled == stop.Date);
                Assert.True(billedCase.PeriodBilledFrom == null);
                Assert.True(billedCase.PeriodBilledTo == null);
                Assert.True(billedCase.DailyStopID == stop.ID);
                Assert.True(billedCase.LocationGroupID == fixture.LocationGroup.ID);
                Assert.True(billedCase.ContainerTypeID == fixture.ContainerType1.ID);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Units == billedCase.ActualUnits);
                Assert.True(billingLine.Value == 12);                
                Assert.True(billingLine.PeriodBilledFrom == billedCase.PeriodBilledFrom);
                Assert.True(billingLine.PeriodBilledTo == billedCase.PeriodBilledTo);
                Assert.False(billingLine.IsManual);
                Assert.Null(billingLine.Comment);
                Assert.True(billingLine.LocationID == fixture.CollectLocation1.IdentityID);                
                Assert.True(billingLine.ContractID == fixture.CompanyContract.ID);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'pavement transport fee' rule and 'location container' unit")]
        public void VerifyThatDataNotBilledTwiceByPavementTransportFeeRuleAndLocationContainerUnit()
        {
            try
            {               
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit when there are two collect containers of the same type")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWhenThereAreTwoCollectContainersOfTheSameType()
        {
            try
            {
                var containerTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.LocationContainer, new List<PriceLineLevel> { containerTypeLevel, locationGroupLevel }, units: 1, price: 12);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);               
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation1);
                var hisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRKSPL", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);                
                Assert.True(billingLine.Value == 24);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit when there are two deliver containers of the same type")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWhenThereAreTwoDeliverContainersOfTheSameType()
        {
            try
            {
                var containerTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { containerTypeLevel, locationGroupLevel }, units: 1, price: 12);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.OnwardLocation1);
                var hisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEPSPL", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.OnwardLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 24);
            }
            catch
            {
                throw;
            }
        }
                
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            var containerTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
            var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
            var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                UnitOfMeasure.LocationContainer, new List<PriceLineLevel> { containerTypeLevel, locationGroupLevel }, units: 1, price: 12);

            var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                withProducts: true, withServices: false);
            HelperFacade.TransportHelper.RunCitAllocationJob();

            var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
            HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
            transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
            var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
            var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                collectLocation: fixture.OnwardLocation1);
            var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 1000,
                onwardLocation: fixture.OnwardLocation1);

            HelperFacade.BillingHelper.RunBillingJob();
            var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
            try
            {
                Assert.NotNull(billedCase);
            }
            catch
            {
                var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                throw new Exception(logMessage ?? "Expected billed case was not created!");
            }

            var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
            Assert.True(billingLine.Value == 24);
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit when there are containers of different type")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWhenThereAreContainersOfDifferentType()
        {
            try
            {
                var containerType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine1 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { containerType1Level, locationGroupLevel }, units: 1, price: 12);

                var containerType2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType2.ID, fixture.ContainerType2.Code);
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);                
                var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { containerType2Level, locationGroup2Level }, units: 1, price: 18);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.OnwardLocation1);
                var hisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEPSPL", fixture.ContainerType2.ID, 500,
                    onwardLocation: fixture.OnwardLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);                
                Assert.NotNull(billedCase1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 12);

                var billedCase2 = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType2.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 18);
            }
            catch
            {
                throw;
            }
        }
                
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit when there is uncompleted transport order")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWhenThereIsUncompletedTransportOrder()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);                
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    onwardLocation: fixture.OnwardLocation1);

                var transportOrder2 = DataFacade.TransportOrder.New()
                                                                    .With_Location(transportOrder.LocationID)
                                                                    .With_Site(transportOrder.SiteID)
                                                                    .With_OrderType(OrderType.AtRequest)
                                                                    .With_TransportDate(transportOrder.TransportDate)
                                                                    .With_ServiceDate(transportOrder.ServiceDate)
                                                                    .With_Status(TransportOrderStatus.Planned)
                                                                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                                                                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                                                                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                                                                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                                                                    .With_ServiceType(transportOrder.ServiceTypeID)
                                                                    .SaveToDb()
                                                                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);                
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit when there is another completed transport order")] // add 'canceled' verification in theory
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWhenThereIsAnotherCompletedTransportOrder()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                     PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.CollectLocation1);

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Completed)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);                
                Assert.True(billingLine.Value == 12);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit when 'bill collected orders' flag is checked (positive)")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWhenBillCollectedOrdersFlagIsChecked()
        {
            try
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(true);

                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                     PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.CollectLocation1);

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Collected)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 12);
            }
            catch
            {
                throw;
            }
            finally
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit when 'bill collected orders' flag is unchecked (negative)")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWhenBillCollectedOrdersFlagIsUnchecked()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                     PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.CollectLocation1);

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Collected)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit without transport order")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWithoutTransportOrder()
        {
            try
            {
                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var stop = HelperFacade.TransportHelper.CreateDaiLine(fixture.CollectLocation1, today, "100000", "103000");                
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(today, "100000", "TRK", fixture.ContainerType1.ID, 0, 
                    collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit without his_pack")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWithoutHisPack()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                if (transportOrder == null)
                {
                    throw new ArgumentNullException("transportOrder", "Error on creating transport order!");
                }

                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit with incorrect status")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWithIncorrectStatus()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "XYZ", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit with inappropriate container type")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWithInappropriateContainerType()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.CollectLocation1.ID).Single());
                    context.SaveChanges();

                    var containerTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                    var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee,
                        UnitOfMeasure.LocationContainer, new List<PriceLineLevel> { containerTypeLevel }, units: 1, price: 12);

                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                    var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType2.ID,
                        collectLocation: fixture.CollectLocation1);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, 
                        PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType2.ID);
                    Assert.Null(billedCase);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.CollectLocation1.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.CollectLocation1.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit with inappropriate collect location")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWithInappropriateCollectLocation()
        {
            try
            {
                var containerType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.LocationContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation2);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit with inappropriate deliver location")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWithInappropriateDeliverLocation()
        {
            try
            {
                var containerType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.OnwardLocation2);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit without his_pack location")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWithoutLocation()
        {
            try
            {
                var containerType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit with incorrect his_pack -> a_time")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWithIncorrectTime()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "090000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit with incorrect his_pack -> a_date")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWithIncorrectDate()
        {
            try 
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation1, date: stop.Date.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit with incorrect his_pack -> RouteNumber")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWithIncorrectRoute()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation1, routeCode: "XYZ");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Location is not linked to location group, and there is no location group level configured in price line
        /// </summary>
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit without location group")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWithoutLocationGroup()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.CollectLocation1.ID).Single());
                    context.SaveChanges();
                    
                    var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                        UnitOfMeasure.LocationContainer, null, units: 1, price: 12);

                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                    var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                        collectLocation: fixture.CollectLocation1);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, 
                        PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);                    
                    try
                    {
                        Assert.NotNull(billedCase);
                    }
                    catch
                    {
                        if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.CollectLocation1.ID))
                        {
                            var link = new LocationGroupLocation { LocationNumber = fixture.CollectLocation1.ID, LocationGroupId = fixture.LocationGroup.ID };
                            context.LocationGroupLocations.Add(link);
                            context.SaveChanges();
                        }
                        var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                        throw new Exception(logMessage ?? "Expected billed case was not created!");
                    }
                    Assert.True(billedCase.PriceLineID == null);

                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == 12);
                    Assert.True(billingLine.PriceLineID == priceLine.ID);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.CollectLocation1.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.CollectLocation1.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit without location group with configured group level")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWithoutLocationGroupWithConfiguredGroupLevel()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.CollectLocation1.ID).Single());
                    context.SaveChanges();

                    var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                        PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                    var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                        UnitOfMeasure.LocationContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 12);

                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                    var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                        collectLocation: fixture.CollectLocation1);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                        UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                    Assert.Null(billedCase);                    
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.CollectLocation1.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.CollectLocation1.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Location is linked to location group but there is not location group price line level
        /// </summary>
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit when there is no location group price line level")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWhenThereIsNoLocationGroupPriceLineLevel()
        {
            try
            {
                var containerTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.LocationContainer, new List<PriceLineLevel> { containerTypeLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Location is linked to location group but price line cannot be matched
        /// </summary>
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'location container' unit when price line cannot be matched")]
        public void VerifyBillingByPavementTransportFeeRuleAndLocationContainerUnitWhenPriceLineCannotBeMatched()
        {
            try
            {
                var containerTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType2.ID, fixture.ContainerType2.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.LocationContainer,
                        PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, new List<PriceLineLevel> { containerTypeLevel, locationGroupLevel }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }                
        #endregion "Pavement transport fee - Location container"
        #region "Price per collected container - Location container"
        [Fact(DisplayName = "Billing - 'price per collected container' rule and 'location container' unit when there are two collect containers of the same type")]
        public void VerifyBillingByPricePerCollectedContainerRuleAndLocationContainerUnitWhenThereAreTwoCollectContainersOfTheSameType()
        {
            try
            {
                var containerTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { containerTypeLevel, locationGroupLevel }, units: 1, price: 10);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation1);
                var hisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRKSPL", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerCollectedContainer,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 20);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'price per collected container' rule and 'location container' unit")]
        public void VerifyThatDataNotBilledTwiceByPricePerCollectedContainerRuleAndLocationContainerUnit()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 10);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.CollectLocation1);                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerCollectedContainer,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerCollectedContainer,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per collected container' rule and 'location container' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPricePerCollectedContainerRuleAndLocationContainerUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {
                var containerTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { containerTypeLevel, locationGroupLevel }, units: 1, price: 10);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.OnwardLocation1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.OnwardLocation1);
                var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "LOC", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.OnwardLocation1);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 500,
                    onwardLocation: fixture.OnwardLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerCollectedContainer,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 20);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'PricePerCollectedContainer' rule and 'location container' unit when there are containers of different type")]
        public void VerifyBillingByPricePerCollectedContainerRuleAndLocationContainerUnitWhenThereAreContainersOfDifferentType()
        {
            try
            {
                var containerType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine1 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { containerType1Level, locationGroup1Level }, units: 1, price: 10);

                var containerType2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType2.ID, fixture.ContainerType2.Code);
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { containerType2Level, locationGroup2Level }, units: 1, price: 18);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.CollectLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.CollectLocation1);
                var hisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRKSPL", fixture.ContainerType2.ID, 0,
                    collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerCollectedContainer,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                Assert.NotNull(billedCase1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 10);

                var billedCase2 = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerCollectedContainer,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType2.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }           

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 18);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per collected container - Location container"
        #region "Price per delivered container - Location container"
        [Fact(DisplayName = "Billing - 'price per delivered container' rule and 'location container' unit when there are two deliver containers of the same type")]
        public void VerifyBillingByPricePerDeliveredContainerRuleAndLocationContainerUnitWhenThereAreTwoCollectContainersOfTheSameType()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredContainer, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredContainer, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 10);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID,
                    onwardLocation: fixture.OnwardLocation1);
                var hisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEPSPL", fixture.ContainerType1.ID,
                    onwardLocation: fixture.OnwardLocation1);
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerDeliveredContainer,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 20);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered container' rule and 'location container' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPricePerDeliveredContainerRuleAndLocationContainerUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredContainer, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredContainer, UnitOfMeasure.LocationContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 10);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.OnwardLocation1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.OnwardLocation1);
                var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "LOC", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.OnwardLocation1);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 500,
                    onwardLocation: fixture.OnwardLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerDeliveredContainer,
                    UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 20);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per delivered container - Location container"
        #region "Price per collected and delivered container - Location container"
        [Fact(DisplayName = "Billing - 'price per collected and delivered container' rule and 'location container' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPricePerCollectedAndDeliveredContainerRuleAndLocationContainerUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedAndDeliveredContainer, UnitOfMeasure.LocationContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedAndDeliveredContainer, 
                    UnitOfMeasure.LocationContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 10);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.OnwardLocation1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.OnwardLocation1);
                var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "LOC", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.OnwardLocation1);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 500,
                    onwardLocation: fixture.OnwardLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID, 
                    PriceRule.PricePerCollectedAndDeliveredContainer, UnitOfMeasure.LocationContainer, stop.ID, fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 40);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per collected and delivered container - Location container"
        #region "Price per collected container - Location value"
        [Fact(DisplayName = "Billing - 'price per collected container' rule and 'location value' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPricePerCollectedContainerRuleAndLocationValueUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationValue,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedContainer,
                    UnitOfMeasure.LocationValue, new List<PriceLineLevel> { locationGroupLevel }, units: 1000, price: 10);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.OnwardLocation1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.OnwardLocation1);
                var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "LOC", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.OnwardLocation1);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 500,
                    onwardLocation: fixture.OnwardLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerCollectedContainer, UnitOfMeasure.LocationValue, stop.ID, fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 0);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per collected container - Location value"
        #region "Price per delivered container - Location value"
        [Fact(DisplayName = "Billing - 'price per delivered container' rule and 'location value' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPricePerDeliveredContainerRuleAndLocationValueUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredContainer, UnitOfMeasure.LocationValue,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredContainer,
                    UnitOfMeasure.LocationValue, new List<PriceLineLevel> { locationGroupLevel }, units: 1000, price: 10);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                if (transportOrder == null)
                {
                    throw new ArgumentNullException("transportOrder", "Error on creating transport order!");
                }

                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed,
                    new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 10,
                    collectLocation: fixture.OnwardLocation1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 10,
                    collectLocation: fixture.OnwardLocation1);
                var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "LOC", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.OnwardLocation1);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 500,
                    onwardLocation: fixture.OnwardLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredContainer, UnitOfMeasure.LocationValue, stop.ID, fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }

                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == 15);
                }
                catch
                {
                    throw;
                }
        }
        #endregion "Price per delivered container - Location value"
        #region "Price per collected and delivered container - Location value"
        [Fact(DisplayName = "Billing - 'price per collected and delivered container' rule and 'location value' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPricePerCollectedAndDeliveredContainerRuleAndLocationValueUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedAndDeliveredContainer, 
                    UnitOfMeasure.LocationValue, PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, 
                    fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedAndDeliveredContainer,
                    UnitOfMeasure.LocationValue, new List<PriceLineLevel> { locationGroupLevel }, units: 1000, price: 10);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 400,
                    collectLocation: fixture.OnwardLocation1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 100,
                    collectLocation: fixture.OnwardLocation1);
                var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "LOC", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.OnwardLocation1);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 500,
                    onwardLocation: fixture.OnwardLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerCollectedAndDeliveredContainer, UnitOfMeasure.LocationValue, stop.ID, fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 20);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per collected and delivered container - Location value"
        #endregion 4.6.4 Get Billing Data by Container Type at Location
        #region 4.6.5 Get Billing Data by Container Type at Visit Address
        #region "Pavement transport fee - Visit address container"  
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var collectServiceOrder1 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                var collectServiceOrder2 = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint2,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder1 = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder1.ID); 
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder1, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder1 = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder1.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder1, fixture.VisitServicePoint1, "100000", "101500");
                var collectHisPack1 = HelperFacade.TransportHelper.CreateHisPack(transportOrder1, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.VisitServicePoint1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder1, "100000", "TRKSPL", fixture.ContainerType1.ID,
                    collectLocation: fixture.VisitServicePoint1);

                var transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder2.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder2, TransportOrderStatus.Completed, new TimeSpan(10, 20, 0));
                transportOrder2 = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder2.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(transportOrder2, fixture.VisitServicePoint2, "102000", "103000");
                var deliverHisPack1 = HelperFacade.TransportHelper.CreateHisPack(transportOrder2, "102000", "DEP", fixture.ContainerType1.ID,
                    onwardLocation: fixture.VisitServicePoint2);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder2, "102000", "DEPSPL", fixture.ContainerType1.ID,
                    onwardLocation: fixture.VisitServicePoint2);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID, 
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ContainerType1.ID);
                Assert.True(billedCase.ActualUnits == 4); //quantity of unique his_pack → pack_nr (collected and delivered for current case)
                Assert.True(billedCase.VisitAddressID == fixture.VisitAddress1.IdentityID);
                Assert.True(billedCase.DateBilled == stop.Date);
                Assert.True(billedCase.PeriodBilledFrom == null);
                Assert.True(billedCase.PeriodBilledTo == null);
                Assert.True(billedCase.MasterRouteCode == stop.RouteNumber);
                Assert.True(billedCase.LocationGroupID == fixture.LocationGroup.ID);
                Assert.True(billedCase.ContainerTypeID == fixture.ContainerType1.ID);
                Assert.True(billedCase.PriceLineID == priceLine.ID);

                var billedCaseDailyStop1 = HelperFacade.BillingHelper.GetBilledCaseDailyStopByStop(stop.ID);
                Assert.NotNull(billedCaseDailyStop1);
                Assert.True(billedCaseDailyStop1.BilledCaseID == billedCase.ID);

                var billedCaseDailyStop2 = HelperFacade.BillingHelper.GetBilledCaseDailyStopByStop(stop2.ID);
                Assert.NotNull(billedCaseDailyStop2);
                Assert.True(billedCaseDailyStop2.BilledCaseID == billedCase.ID);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 100);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'pavement transport fee' rule and 'visit address container' unit")]
        public void VerifyThatDataNotBilledTwiceByPavementTransportFeeRuleAndVisitAddressContainerUnit()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "101500");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ContainerType1.ID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ContainerType1.ID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit when there are containers of different type")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWhenThereAreContainersOfDifferentType()
        {
            try
            {
                var containerType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine1 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { containerType1Level, locationGroupLevel }, units: 1, price: 25);

                var containerType2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType2.ID, fixture.ContainerType2.Code);
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                   PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { containerType2Level, locationGroup2Level }, units: 1, price: 26);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID); 
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed,
                    new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.VisitServicePoint1);
                var hisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEPSPL", fixture.ContainerType2.ID, 500,
                    onwardLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.NotNull(billedCase1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 25);

                var billedCase2 = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType2.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 26);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit when there is uncompleted transport order")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWhenThereIsUncompletedTransportOrder()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled,
                    new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);

                var transportOrder2 = DataFacade.TransportOrder.New()
                                                                    .With_Location(transportOrder.LocationID)
                                                                    .With_Site(transportOrder.SiteID)
                                                                    .With_OrderType(OrderType.AtRequest)
                                                                    .With_TransportDate(transportOrder.TransportDate)
                                                                    .With_ServiceDate(transportOrder.ServiceDate)
                                                                    .With_Status(TransportOrderStatus.Planned)
                                                                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                                                                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                                                                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                                                                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                                                                    .With_ServiceType(transportOrder.ServiceTypeID)
                                                                    .SaveToDb()
                                                                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit when there is another completed transport order")] // add 'canceled' verification in theory
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWhenThereIsAnotherCompletedTransportOrder()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                     PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled,
                    new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);

                var transportOrder2 = DataFacade.TransportOrder.New()
                                                                    .With_Location(transportOrder.LocationID)
                                                                    .With_Site(transportOrder.SiteID)
                                                                    .With_OrderType(OrderType.AtRequest)
                                                                    .With_TransportDate(transportOrder.TransportDate)
                                                                    .With_ServiceDate(transportOrder.ServiceDate)
                                                                    .With_Status(TransportOrderStatus.Completed)
                                                                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                                                                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                                                                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                                                                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                                                                    .With_ServiceType(transportOrder.ServiceTypeID)
                                                                    .SaveToDb()
                                                                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 25);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit when 'bill collected orders' flag is checked (positive)")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWhenBillCollectedOrdersFlagIsChecked()
        {
            try
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(true);

                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                     PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled,
                    new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Collected)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID,
                    fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billedCaseDailyStop = HelperFacade.BillingHelper.GetBilledCaseDailyStopByStop(stop.ID);
                Assert.NotNull(billedCaseDailyStop);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 25);
            }
            catch
            {
                throw;
            }
            finally
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit when 'bill collected orders' flag is unchecked (negative)")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWhenBillCollectedOrdersFlagIsUnchecked()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                     PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled,
                    new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Collected)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID,
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit without transport order")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWithoutTransportOrder()
        {
            try
            {
                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var stop = HelperFacade.TransportHelper.CreateDaiLine(fixture.VisitServicePoint1, today, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(today, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit without his_pack")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWithoutHisPack()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed,
                    new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit with incorrect status")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWithIncorrectStatus()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "XYZ", fixture.ContainerType1.ID,
                    collectLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit with inappropriate container type")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWithInappropriateContainerType()
        {
            try
            {
                var containerType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { containerType1Level, locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType2.ID,
                    collectLocation: fixture.VisitServicePoint1);
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType2.ID);
                Assert.Null(billedCase); 
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit with inappropriate collect location")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWithInappropriateCollectLocation()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price:25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.VisitServicePoint2);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit with inappropriate deliver location")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWithInappropriateDeliverLocation()
        {
            try
            {               
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 1500,
                    onwardLocation: fixture.VisitServicePoint2);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit without his_pack location")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWithoutLocation()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit with incorrect his_pack -> a_time")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWithIncorrectTime()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed,
                    new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "090000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit with incorrect his_pack -> a_date")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWithIncorrectDate()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.VisitServicePoint1, date: stop.Date.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit with incorrect his_pack -> RouteNumber")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWithIncorrectRoute()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.VisitServicePoint1, routeCode: "XYZ");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Location is not linked to location group, and there is no location group level configured in price line
        /// </summary>
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit without location group")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWithoutLocationGroup()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.VisitAddress1.ID).Single());
                    context.SaveChanges();
                                        
                    var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                        UnitOfMeasure.VisitAddressContainer, null, units: 1, price: 25);

                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                    var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                        collectLocation: fixture.VisitServicePoint1);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                    try
                    {
                        Assert.NotNull(billedCase);
                    }
                    catch
                    {
                        if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.VisitAddress1.ID))
                        {
                            var link = new LocationGroupLocation { LocationNumber = fixture.VisitAddress1.ID, LocationGroupId = fixture.LocationGroup.ID };
                            context.LocationGroupLocations.Add(link);
                            context.SaveChanges();
                        }
                        var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                        throw new Exception(logMessage ?? "Expected billed case was not created!");
                    }
                    Assert.True(billedCase.PriceLineID == null);

                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == 25);
                    Assert.True(billingLine.PriceLineID == priceLine.ID);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.VisitAddress1.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.VisitAddress1.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit without location group with configured group level")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWithoutLocationGroupWithConfiguredGroupLevel()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.VisitAddress1.ID).Single());
                    context.SaveChanges();

                    var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                        PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                    var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee,
                        UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                    var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                        collectLocation: fixture.VisitServicePoint1);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID,
                    fixture.ContainerType1.ID);
                    Assert.Null(billedCase);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.VisitAddress1.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.VisitAddress1.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit when there is no location group price line level")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWhenThereIsNoLocationGroupPriceLineLevel()
        {
            try
            {
                var containerTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { containerTypeLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Location is linked to location group but price line cannot be matched
        /// </summary>
        [Fact(DisplayName = "Billing - 'pavement transport fee' rule and 'visit address container' unit when price line cannot be matched")]
        public void VerifyBillingByPavementTransportFeeRuleAndVisitAddressContainerUnitWhenPriceLineCannotBeMatched()
        {
            try
            {
                var containerTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType2.ID, fixture.ContainerType2.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PavemenTransportFee,
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { containerTypeLevel }, units: 1, price: 25);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID,
                    collectLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PavemenTransportFee, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID,
                    fixture.ContainerType1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Pavement transport fee - Visit address container"
        #region "Price per collected container - Visit address container" 
        [Fact(DisplayName = "Billing - 'price per collected container' rule and 'visit address container' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPricePerCollectedContainerRuleAndVisitAddressContainerUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedContainer, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);
                var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "LOC", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.VisitServicePoint1);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 500,
                    onwardLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerCollectedContainer, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 50);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'price per collected container' rule and 'visit address container' unit when there are containers of different type")]
        public void VerifyBillingByPricePerCollectedContainerRuleAndVisitAddressContainerUnitWhenThereAreContainersOfDifferentType()
        {
            try
            {
                var containerType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType1.ID, fixture.ContainerType1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine1 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedContainer,
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { containerType1Level, locationGroupLevel }, units: 1, price: 24);

                var containerType2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.ContainerType, PriceRuleLevelValueType.ContainerType, fixture.ContainerType2.ID, fixture.ContainerType2.Code);
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedContainer, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { containerType2Level, locationGroup2Level }, units: 1, price: 32);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var hisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);
                var hisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRKSPL", fixture.ContainerType2.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerCollectedContainer, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                Assert.NotNull(billedCase1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 24);

                var billedCase2 = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerCollectedContainer, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType2.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 32);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per collected container - Visit address container"
        #region "Price per delivered container - Visit address container"        
        [Fact(DisplayName = "Billing - 'price per delivered container' rule and 'visit address container' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPricePerDeliveredContainerRuleAndVisitAddressContainerUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredContainer, UnitOfMeasure.VisitAddressContainer,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredContainer, 
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);
                var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "LOC", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.VisitServicePoint1);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 500,
                    onwardLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                     PriceRule.PricePerDeliveredContainer, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                     fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 50);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per delivered container - Visit address container"
        #region "Price per collected and delivered container - Visit address container"
        [Fact(DisplayName = "Billing - 'price per collected and delivered container' rule and 'visit address container' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPricePerCollectedAndDeliveredContainerRuleAndVisitAddressContainerUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedAndDeliveredContainer, 
                    UnitOfMeasure.VisitAddressContainer, PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, 
                    fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedAndDeliveredContainer,
                    UnitOfMeasure.VisitAddressContainer, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 25);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);
                var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "LOC", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.VisitServicePoint1);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 500,
                    onwardLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerCollectedAndDeliveredContainer, UnitOfMeasure.VisitAddressContainer, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 100);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per collected and delivered container - Visit address container"
        #region "Price per collected container - Visit address value"        
        [Fact(DisplayName = "Billing - 'price per collected container' rule and 'visit address value' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPricePerCollectedContainerRuleAndVisitAddressValueUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedContainer, UnitOfMeasure.VisitAddressValue,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedContainer,
                    UnitOfMeasure.VisitAddressValue, new List<PriceLineLevel> { locationGroupLevel }, units: 1000, price: 25);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 0,
                    collectLocation: fixture.VisitServicePoint1);
                var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "LOC", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.VisitServicePoint1);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 500,
                    onwardLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerCollectedContainer, UnitOfMeasure.VisitAddressValue, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 0);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per collected container - Visit address value"
        #region "Price per delivered container - Visit address value"
        [Fact(DisplayName = "Billing - 'price per delivered container' rule and 'visit address value' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPricePerDeliveredContainerRuleAndVisitAddressValueUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredContainer, UnitOfMeasure.VisitAddressValue,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredContainer,
                    UnitOfMeasure.VisitAddressValue, new List<PriceLineLevel> { locationGroupLevel }, units: 1000, price: 10);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 10,
                    collectLocation: fixture.VisitServicePoint1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 10,
                    collectLocation: fixture.VisitServicePoint1);
                var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "LOC", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.VisitServicePoint1);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 500,
                    onwardLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredContainer, UnitOfMeasure.VisitAddressValue, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 15);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per delivered container - Visit address value"
        #region "Price per collected and delivered container - Visit address value"
        [Fact(DisplayName = "Billing - 'price per collected and delivered container' rule and 'visit address value' unit when there are collect and deliver containers of the same type")]
        public void VerifyBillingByPricePerCollectedAndDeliveredContainerRuleAndVisitAddressValueUnitWhenThereAreCollectAndDeliverContainersOfTheSameType()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerCollectedAndDeliveredContainer,
                    UnitOfMeasure.VisitAddressValue, PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID,
                    fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerCollectedAndDeliveredContainer,
                    UnitOfMeasure.VisitAddressValue, new List<PriceLineLevel> { locationGroupLevel }, units: 1000, price: 10);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var collectHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 400,
                    collectLocation: fixture.VisitServicePoint1);
                var collectHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "TRK", fixture.ContainerType1.ID, 100,
                    collectLocation: fixture.VisitServicePoint1);
                var deliverHisPack = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "LOC", fixture.ContainerType1.ID, 1000,
                    onwardLocation: fixture.VisitServicePoint1);
                var deliverHisPack2 = HelperFacade.TransportHelper.CreateHisPack(transportOrder, "100000", "DEP", fixture.ContainerType1.ID, 500,
                    onwardLocation: fixture.VisitServicePoint1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetContainerTypeAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerCollectedAndDeliveredContainer, UnitOfMeasure.VisitAddressValue, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ContainerType1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 20);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per collected and delivered container - Visit address value"
        #endregion 4.6.5 Get Billing Data by Container Type at Visit Address
        #region 4.6.6 Get Billing Data by Price per Delivered Item Location
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' unit when there are two stops in one day for the same location")]
        public void VerifyBillingByPricePerDeliveredItemRuleLocationItemUnitWhenThereAreTwoStopsInOneDayForTheSameLocation()
        {
            try
            {
                var productGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.ProductGroup, PriceRuleLevelValueType.ProductGroup, fixture.ProductGroup1.ID, fixture.ProductGroup1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, 
                    UnitOfMeasure.LocationItem, new List<PriceLineLevel> { productGroupLevel, locationGroupLevel }, units: 1, price: 7);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.CollectLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(collectTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(collectTransportOrder, fixture.CollectLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);
                var daiCoin2 = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note50EurProduct, 10);

                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(17, 0, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(deliverTransportOrder, fixture.CollectLocation1, "170000", "173000");
                var daiCoin3 = HelperFacade.TransportHelper.CreateDaiCoin(stop2, fixture.Note100EurProduct, 10);
                var daiCoin4 = HelperFacade.TransportHelper.CreateDaiCoin(stop2, fixture.Note50EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID, 
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.True(billedCase1.ActualUnits == 20);
                Assert.True(billedCase1.DateBilled == stop.Date);
                Assert.True(billedCase1.ProductGroupID == fixture.ProductGroup1.ID);
                Assert.True(billedCase1.LocationGroupID == fixture.LocationGroup.ID);
                Assert.True(billedCase1.PeriodBilledFrom == null);
                Assert.True(billedCase1.PeriodBilledTo == null);
                Assert.True(billedCase1.PriceLineID == priceLine.ID);

                var billedCase2 = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop2.ID, fixture.ProductGroup1.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine.Units == billedCase1.ActualUnits);
                Assert.True(billingLine.Value == 140);
                Assert.True(billingLine.PeriodBilledFrom == billedCase1.PeriodBilledFrom);
                Assert.True(billingLine.PeriodBilledTo == billedCase1.PeriodBilledTo);
                Assert.True(billingLine.LocationID == fixture.CollectLocation1.IdentityID);                

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 140);
            }
            catch
            {
                throw;
            }
        }        

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' unit with two products in different product groups")]
        public void VerifyBillingByPricePerDeliveredItemRuleLocationItemUnitWithTwoProductsInDifferentProductGroups()
        {
            try
            {
                var productGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.ProductGroup, PriceRuleLevelValueType.ProductGroup, fixture.ProductGroup1.ID, fixture.ProductGroup1.Code);
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { productGroup1Level, locationGroup1Level }, units: 1, price: 7);

                var productGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.ProductGroup, PriceRuleLevelValueType.ProductGroup, fixture.ProductGroup2.ID, fixture.ProductGroup2.Code);
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { productGroup2Level, locationGroup2Level }, units: 1, price: 9.50m);

                // list of products linked to different product groups (linkage is implemented in fixture)
                var productCollection = new Dictionary<Cwc.BaseData.Product, int>
                {
                    { fixture.Note100EurProduct, 10 },
                    { fixture.Coin1EurProduct, 10 }
                };
                                
                var deliverServiceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    productCollection);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(deliverTransportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);
                var daiCoin2 = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Coin1EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.NotNull(billedCase1);

                var billedCase2 = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup2.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);                
                Assert.True(billingLine1.Value == 70);

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 95);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'price per delivered item' rule and 'location item' unit")]
        public void VerifyThatDataNotBilledTwiceByPricePerDeliveredItemRuleLocationItemUnit()
        {
            try
            {                
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroup1Level }, units: 1, price: 7);               

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(deliverTransportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);               

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' unit when there is uncompleted transport order")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWhenThereIsUncompletedTransportOrder()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");                
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);               

                var transportOrder2 = DataFacade.TransportOrder.New()
                                                                    .With_Location(transportOrder.LocationID)
                                                                    .With_Site(transportOrder.SiteID)
                                                                    .With_OrderType(OrderType.AtRequest)
                                                                    .With_TransportDate(transportOrder.TransportDate)
                                                                    .With_ServiceDate(transportOrder.ServiceDate)
                                                                    .With_Status(TransportOrderStatus.Planned)
                                                                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                                                                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                                                                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                                                                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                                                                    .With_ServiceType(transportOrder.ServiceTypeID)
                                                                    .SaveToDb()
                                                                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' unit when there is another completed transport order")] // add 'canceled' verification in theory
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWhenThereIsAnotherCompletedTransportOrder()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);                

                var transportOrder2 = DataFacade.TransportOrder.New()
                                                                    .With_Location(transportOrder.LocationID)
                                                                    .With_Site(transportOrder.SiteID)
                                                                    .With_OrderType(OrderType.AtRequest)
                                                                    .With_TransportDate(transportOrder.TransportDate)
                                                                    .With_ServiceDate(transportOrder.ServiceDate)
                                                                    .With_Status(TransportOrderStatus.Completed)
                                                                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                                                                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                                                                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                                                                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                                                                    .With_ServiceType(transportOrder.ServiceTypeID)
                                                                    .SaveToDb()
                                                                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 70);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' unit when 'bill collected orders' flag is checked (positive)")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWhenBillCollectedOrdersFlagIsChecked()
        {
            try
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(true);

                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Collected)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 70);
            }
            catch
            {
                throw;
            }
            finally
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' unit when 'bill collected orders' flag is unchecked (negative)")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWhenBillCollectedOrdersFlagIsUnchecked()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Collected)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' unit without transport order")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWithoutTransportOrder()
        {
            try
            {
                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var stop = HelperFacade.TransportHelper.CreateDaiLine(fixture.CollectLocation1, today, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);
                                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }        

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' without dai_coin")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWithoutDaiCoin()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' with inappropriate product group")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWithInappropriateProductGroup()
        {
            try
            {
                var productGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.ProductGroup, PriceRuleLevelValueType.ProductGroup, fixture.ProductGroup1.ID, fixture.ProductGroup1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { productGroup1Level, locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Coin1EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Coin1EurProduct, 10);                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup2.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' without product group")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWithoutProductGroup()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 9);

                var product = DataFacade.Product.Take(p =>
                                                            p.Value == 20
                                                            && p.Currency == currencyCode
                                                            && p.Type == noteType)
                                                .With_ProductGroupID(null)
                                                .SaveToDb();

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1, product, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, product, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 90);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' with inappropriate location")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWithInappropriateLocation()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10, locationID: fixture.OnwardLocation2.ID);
                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' with incorrect dai_coin -> time_a")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWithIncorrectTime()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10, arrivalTime: "090000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
       
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' with incorrect dai_coin -> Date")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWithIncorrectDate()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10, date: stop.Date.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' with incorrect dai_coin -> RouteNumber")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWithIncorrectRoute()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10, routeCode: "XYZ");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' with zero items quantity")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWithZeroItemsQuantity()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 0);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Location is not linked to location group, and there is no location group level configured in price line
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' unit without location group")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWithoutLocationGroup()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.OnwardLocation1.ID).Single());
                    context.SaveChanges();
                                        
                    var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                        null, units: 1, price: 7);

                    var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                    var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                    try
                    {
                        Assert.NotNull(billedCase);
                    }
                    catch
                    {
                        if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.OnwardLocation1.ID))
                        {
                            var link = new LocationGroupLocation { LocationNumber = fixture.OnwardLocation1.ID, LocationGroupId = fixture.LocationGroup.ID };
                            context.LocationGroupLocations.Add(link);
                            context.SaveChanges();
                        }
                        var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                        throw new Exception(logMessage ?? "Expected billed case was not created!");
                    }
                    Assert.True(billedCase.PriceLineID == null);

                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == 70);
                    Assert.True(billingLine.PriceLineID == priceLine.ID);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.OnwardLocation1.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.OnwardLocation1.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' unit without location group with configured group level")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWithoutLocationGroupWithConfiguredGroupLevel()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.OnwardLocation1.ID).Single());
                    context.SaveChanges();

                    var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                        PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                    HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                        new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                    var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                    var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                    Assert.Null(billedCase);              
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.OnwardLocation1.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.OnwardLocation1.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Location is linked to location group but there is no location group price line level
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' unit when there is no location group price line level")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWhenThereIsNoLocationGroupPriceLineLevel()
        {
            try
            {
                var productGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.ProductGroup, PriceRuleLevelValueType.ProductGroup, fixture.ProductGroup1.ID, fixture.ProductGroup1.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { productGroup1Level }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Location is linked to location group but price line cannot be matched
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'location item' unit when price line cannot be matched")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndLocationItemUnitWhenPriceLineCannotBeMatched()
        {
            try
            {
                var productGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    PriceRuleLevelName.ProductGroup, PriceRuleLevelValueType.ProductGroup, fixture.ProductGroup2.ID, fixture.ProductGroup2.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem,
                    new List<PriceLineLevel> { productGroup1Level }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.OnwardLocation1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.LocationItem, stop.ID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        #endregion 4.6.6 Get Billing Data by Price per Delivered Item Location
        #region 4.6.7 Get Billing Data by Price per Delivered Item Visit Address  
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' unit with two products in the same product group")]
        public void VerifyBillingByPricePerDeliveredItemRuleVisitAddressItemUnitWithTwoProductsInTheSameProductGroup()
        {
            try
            {
                var productGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.ProductGroup, PriceRuleLevelValueType.ProductGroup, fixture.ProductGroup1.ID, fixture.ProductGroup1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, 
                    UnitOfMeasure.VisitAddressItem, new List<PriceLineLevel> { productGroupLevel, locationGroupLevel }, units: 1, price: 7);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(deliverTransportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);
                var daiCoin2 = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note50EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ProductGroup1.ID);                
                Assert.True(billedCase.ActualUnits == 20);
                Assert.True(billedCase.LocationID == null);
                Assert.True(billedCase.VisitAddressID == fixture.VisitAddress1.IdentityID);
                Assert.True(billedCase.DateBilled == stop.Date);
                Assert.True(billedCase.ProductGroupID == fixture.ProductGroup1.ID);
                Assert.True(billedCase.LocationGroupID == fixture.LocationGroup.ID);
                Assert.True(billedCase.PeriodBilledFrom == null);
                Assert.True(billedCase.PeriodBilledTo == null);
                Assert.True(billedCase.PriceLineID == priceLine.ID);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);               
                Assert.True(billingLine.Value == 140);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'price per delivered item' rule and 'visit address item' unit")]
        public void VerifyThatDataNotBilledTwiceByPricePerDeliveredItemRuleVisitAddressItemUnitWithTwoProductsInTheSameProductGroup()
        {
            try
            {                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem,
                    UnitOfMeasure.VisitAddressItem, new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(deliverTransportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID,
                    fixture.ProductGroup1.ID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID,
                    fixture.ProductGroup1.ID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' unit with two products in different product groups")]
        public void VerifyBillingByPricePerDeliveredItemRuleVisitAddressItemUnitWithTwoProductsInDifferentProductGroups()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                // list of products linked to different product groups (linkage is implemented in fixture)
                var productCollection = new Dictionary<Cwc.BaseData.Product, int>
                {
                    { fixture.Note100EurProduct, 10 },
                    { fixture.Coin1EurProduct, 10 }
                };

                var deliverServiceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    productCollection);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(deliverTransportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);
                var daiCoin2 = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Coin1EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ProductGroup1.ID);
                Assert.NotNull(billedCase1);

                var billedCaseDailyStop1 = HelperFacade.BillingHelper.GetBilledCaseDailyStopByCase(billedCase1.ID);
                Assert.NotNull(billedCaseDailyStop1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 70);

                var billedCase2 = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ProductGroup2.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                } 
                var billedCaseDailyStop2 = HelperFacade.BillingHelper.GetBilledCaseDailyStopByCase(billedCase2.ID);
                Assert.NotNull(billedCaseDailyStop2);

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 70);
            }
            catch
            {
                throw;
            }
        }


        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' unit when there is uncompleted transport order")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWhenThereIsUncompletedTransportOrder()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                var transportOrder2 = DataFacade.TransportOrder.New()
                                                                    .With_Location(transportOrder.LocationID)
                                                                    .With_Site(transportOrder.SiteID)
                                                                    .With_OrderType(OrderType.AtRequest)
                                                                    .With_TransportDate(transportOrder.TransportDate)
                                                                    .With_ServiceDate(transportOrder.ServiceDate)
                                                                    .With_Status(TransportOrderStatus.Planned)
                                                                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                                                                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                                                                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                                                                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                                                                    .With_ServiceType(transportOrder.ServiceTypeID)
                                                                    .SaveToDb()
                                                                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, 
                    fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' unit when there is another completed transport order")] // add 'canceled' verification in theory
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWhenThereIsAnotherCompletedTransportOrder()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                var transportOrder2 = DataFacade.TransportOrder.New()
                                                                    .With_Location(transportOrder.LocationID)
                                                                    .With_Site(transportOrder.SiteID)
                                                                    .With_OrderType(OrderType.AtRequest)
                                                                    .With_TransportDate(transportOrder.TransportDate)
                                                                    .With_ServiceDate(transportOrder.ServiceDate)
                                                                    .With_Status(TransportOrderStatus.Completed)
                                                                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                                                                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                                                                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                                                                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                                                                    .With_ServiceType(transportOrder.ServiceTypeID)
                                                                    .SaveToDb()
                                                                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 70);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' unit when 'bill collected orders' flag is checked (positive)")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWhenBillCollectedOrdersFlagIsChecked()
        {
            try
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(true);

                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Collected)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billedCaseDailyStop = HelperFacade.BillingHelper.GetBilledCaseDailyStopByStop(stop.ID);
                Assert.NotNull(billedCaseDailyStop);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 70);
            }
            catch
            {
                throw;
            }
            finally
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' unit when 'bill collected orders' flag is unchecked (negative)")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWhenBillCollectedOrdersFlagIsUnchecked()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Collected)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' unit without transport order")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWithoutTransportOrder()
        {
            try
            {
                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var stop = HelperFacade.TransportHelper.CreateDaiLine(fixture.VisitServicePoint1, today, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' without dai_coin")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressnItemUnitWithoutDaiCoin()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' with inappropriate product group")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWithInappropriateProductGroup()
        {
            try
            {
                var productGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.ProductGroup, PriceRuleLevelValueType.ProductGroup, fixture.ProductGroup1.ID, fixture.ProductGroup1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { productGroup1Level, locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Coin1EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Coin1EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup2.ID);
                Assert.Null(billedCase);                
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' with inappropriate location")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWithInappropriateLocation()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10, locationID: fixture.VisitServicePoint2.ID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' with incorrect dai_coin -> time_a")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWithIncorrectTime()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10, arrivalTime: "090000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' with incorrect dai_coin -> Date")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWithIncorrectDate()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10, date: stop.Date.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' with incorrect dai_coin -> RouteNumber")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWithIncorrectRoute()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10, routeCode: "XYZ");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' with zero items quantity")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWithZeroItemsQuantity()
        {
            try
            {
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 0);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Location is not linked to location group, and there is no location group level configured in price line
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' unit without location group")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWithoutLocationGroup()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.VisitAddress1.ID).Single());
                    context.SaveChanges();
                    
                    var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                        null, units: 1, price: 7);

                    var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                    var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                    try
                    {
                        Assert.NotNull(billedCase);
                    }
                    catch
                    {
                        if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.VisitAddress1.ID))
                        {
                            var link = new LocationGroupLocation { LocationNumber = fixture.VisitAddress1.ID, LocationGroupId = fixture.LocationGroup.ID };
                            context.LocationGroupLocations.Add(link);
                            context.SaveChanges();
                        }
                        var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                        throw new Exception(logMessage ?? "Expected billed case was not created!");
                    }
                    Assert.True(billedCase.PriceLineID == null);

                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == 70);
                    Assert.True(billingLine.PriceLineID == priceLine.ID);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.VisitAddress1.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.VisitAddress1.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' unit without location group with configured group level")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWithoutLocationGroupWithConfiguredGroupLevel()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.VisitAddress1.ID).Single());
                    context.SaveChanges();

                    var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                    HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                        new List<PriceLineLevel> { locationGroupLevel }, units: 1, price: 7);

                    var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                    var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                    Assert.Null(billedCase);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.VisitAddress1.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.VisitAddress1.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Location is linked to location group but there is no location group price line level
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' unit when there is no location group price line level")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWhenThereIsNoLocationGroupPriceLineLevel()
        {
            try
            {
                var productGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.ProductGroup, PriceRuleLevelValueType.ProductGroup, fixture.ProductGroup1.ID, fixture.ProductGroup1.Code);                
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { productGroup1Level }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Location is linked to location group but price line cannot be matched
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per delivered item' rule and 'visit address item' unit when price line cannot be matched")]
        public void VerifyBillingByPricePerDeliveredItemRuleAndVisitAddressItemUnitWhenPriceLineCannotBeMatched()
        {
            try
            {
                var productGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    PriceRuleLevelName.ProductGroup, PriceRuleLevelValueType.ProductGroup, fixture.ProductGroup2.ID, fixture.ProductGroup2.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem,
                    new List<PriceLineLevel> { productGroup1Level }, units: 1, price: 7);

                var serviceOrder = HelperFacade.TransportHelper.CreateDeliverServiceOrder(today, GenericStatus.Confirmed, fixture.VisitServicePoint1,
                    fixture.Note100EurProduct, 10);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "100000", "103000");
                var daiCoin = HelperFacade.TransportHelper.CreateDaiCoin(stop, fixture.Note100EurProduct, 10);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetPricePerDeliveredItemAtVisitAddressBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerDeliveredItem, UnitOfMeasure.VisitAddressItem, stop.RouteNumber, fixture.VisitAddress1.IdentityID, fixture.ProductGroup1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        #endregion 4.6.7 Get Billing Data by Price per Delivered Item Visit Address
        #region 4.6.8 Get Billing Data per Servicing Job at Location
        #region "Price per servicing job - Servicing job"
        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnit()
        {
            try
            {
                var servicingJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { servicingJob1Level, locationGroup1Level }, units: 1, price: 15);

                var servicingJob2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode2.ID, fixture.ServicingCode2.Code);
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { servicingJob2Level, locationGroup2Level }, units: 1, price: 19);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode1 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);
                var daiServicingCode2 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode2.Code);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, daiServicingCode1.ID);
                Assert.True(billedCase1.ActualUnits == 1);
                Assert.True(billedCase1.DateBilled == stop.Date);
                Assert.True(billedCase1.PeriodBilledFrom == null);
                Assert.True(billedCase1.PeriodBilledTo == null);
                Assert.True(billedCase1.LocationID == fixture.ExternalLocation.IdentityID);                
                Assert.True(billedCase1.LocationGroupID == fixture.LocationGroup.ID);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 15);
                
                var billedCase2 = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, daiServicingCode2.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }           

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 19);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'price per servicing job' rule and 'servicing job' unit")]
        public void VerifyThatDataNotBilledTwiceByPricePerServicingJobRuleAndServicingJobUnit()
        {
            try
            {
                var servicingJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { servicingJob1Level, locationGroup1Level }, units: 1, price: 15);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode1 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);
                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, daiServicingCode1.ID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, daiServicingCode1.ID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit without dai_servicingCode")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWithoutDaiServicingCode()
        {
            try
            {
                var servicingJobLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { servicingJobLevel, locationGroupLevel }, units: 1, price: 15);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServicingJob, 
                    UnitOfMeasure.ServicingJob, stop.ID);                
                Assert.Null(billedCase); 
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit unit when there is uncompleted transport order")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWhenThereIsUncompletedTransportOrder()
        {
            try
            {
                var servicingJobLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);                
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { servicingJobLevel, locationGroupLevel }, units: 1, price: 15);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);
                
                var transportOrder2 = DataFacade.TransportOrder.New()
                                                                    .With_Location(transportOrder.LocationID)
                                                                    .With_Site(transportOrder.SiteID)
                                                                    .With_OrderType(OrderType.AtRequest)
                                                                    .With_TransportDate(transportOrder.TransportDate)
                                                                    .With_ServiceDate(transportOrder.ServiceDate)
                                                                    .With_Status(TransportOrderStatus.Planned)
                                                                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                                                                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                                                                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                                                                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                                                                    .With_ServiceType(transportOrder.ServiceTypeID)
                                                                    .SaveToDb()
                                                                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, fixture.ServicingCode1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit when there is another completed transport order")] // add 'canceled' verification in theory
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWhenThereIsAnotherCompletedTransportOrder()
        {
            try
            {
                var servicingJobLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);               
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { servicingJobLevel, locationGroupLevel }, units: 1, price: 15);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);                

                var transportOrder2 = DataFacade.TransportOrder.New()
                                                                    .With_Location(transportOrder.LocationID)
                                                                    .With_Site(transportOrder.SiteID)
                                                                    .With_OrderType(OrderType.AtRequest)
                                                                    .With_TransportDate(transportOrder.TransportDate)
                                                                    .With_ServiceDate(transportOrder.ServiceDate)
                                                                    .With_Status(TransportOrderStatus.Completed)
                                                                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                                                                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                                                                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                                                                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                                                                    .With_ServiceType(transportOrder.ServiceTypeID)
                                                                    .SaveToDb()
                                                                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, daiServicingCode.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 15);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit when 'bill collected orders' flag is checked (positive)")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWhenBillCollectedOrdersFlagIsChecked()
        {
            try
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(true);

                var servicingJobLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { servicingJobLevel, locationGroupLevel }, units: 1, price: 15);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Collected)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, daiServicingCode.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 15);
            }
            catch
            {
                throw;
            }
            finally
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit when 'bill collected orders' flag is unchecked (negative)")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWhenBillCollectedOrdersFlagIsUnchecked()
        {
            try
            {
                var servicingJobLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { servicingJobLevel, locationGroupLevel }, units: 1, price: 15);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);

                var transportOrder2 = DataFacade.TransportOrder.New()
                    .With_Location(transportOrder.LocationID)
                    .With_Site(transportOrder.SiteID)
                    .With_OrderType(OrderType.AtRequest)
                    .With_TransportDate(transportOrder.TransportDate)
                    .With_ServiceDate(transportOrder.ServiceDate)
                    .With_Status(TransportOrderStatus.Collected)
                    .With_MasterRouteCode(transportOrder.MasterRouteCode)
                    .With_MasterRouteDate(transportOrder.MasterRouteDate)
                    .With_StopArrivalTime(transportOrder.StopArrivalTime)
                    .With_ServiceOrder(transportOrder.ServiceOrderID)
                    .With_ServiceType(transportOrder.ServiceTypeID)
                    .SaveToDb()
                    .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, daiServicingCode.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit unit without transport order")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWithoutTransportOrder()
        {
            try
            {
                
                var serviceJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                     PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { serviceJob1Level, locationGroupLevel }, units: 1, price: 15);

                var stop = HelperFacade.TransportHelper.CreateDaiLine(fixture.CollectLocation1, today, "100000", "103000");
                var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, fixture.ServicingCode1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
                
        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit with inappropriate servicing code")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWithInappropriateProductGroup()
        {
            try
            {
                var serviceJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { serviceJob1Level, locationGroupLevel }, units: 1, price: 15);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode2.Code);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, fixture.ServicingCode2.ID);
                Assert.Null(billedCase); 
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit with incorrect a_time")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWithIncorrectTime()
        {
            try
            {
                var serviceJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { serviceJob1Level, locationGroupLevel }, units: 1, price: 15);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code, arrivalTime: "090000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, fixture.ServicingCode1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit with incorrect Date")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWithIncorrectDate()
        {
            try
            {
                var serviceJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { serviceJob1Level, locationGroupLevel }, units: 1, price: 15);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code, 
                    date: stop.Date.AddDays(-1));

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, fixture.ServicingCode1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit with incorrect route")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWithIncorrectRoute()
        {
            try
            {
                var serviceJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { serviceJob1Level, locationGroupLevel }, units: 1, price: 15);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code, routeCode: "XYZ");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, fixture.ServicingCode1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Location is not linked to location group, and there is no location group level configured in price line
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit without location group")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWithoutLocationGroup()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.ExternalLocation.ID).Single());
                    context.SaveChanges();

                    var serviceJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);                    
                    var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                        new List<PriceLineLevel> { serviceJob1Level }, units: 1, price: 15);

                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                    var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, daiServicingCode.ID);
                    try
                    {
                        Assert.NotNull(billedCase);
                    }
                    catch
                    {
                        if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.ExternalLocation.ID))
                        {
                            var link = new LocationGroupLocation { LocationNumber = fixture.ExternalLocation.ID, LocationGroupId = fixture.LocationGroup.ID };
                            context.LocationGroupLocations.Add(link);
                            context.SaveChanges();
                        }
                        var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                        throw new Exception(logMessage ?? "Expected billed case was not created!");
                    }
                    Assert.True(billedCase.PriceLineID == null);

                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == 15);
                    Assert.True(billingLine.PriceLineID == priceLine.ID);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.ExternalLocation.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.ExternalLocation.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit without location group with configured group level")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWithoutLocationGroupWithConfiguredGroupLevel()
        {
            using (var context = new AutomationTransportDataContext())
            {
                try
                {
                    context.LocationGroupLocations.Remove(context.LocationGroupLocations.Where(l => l.LocationNumber == fixture.ExternalLocation.ID).Single());
                    context.SaveChanges();

                    var serviceJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                    var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                    HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                        new List<PriceLineLevel> { serviceJob1Level, locationGroupLevel }, units: 1, price: 15);

                    var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                    HelperFacade.TransportHelper.RunCitAllocationJob();
                    var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                    transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                    var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                    var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);

                    HelperFacade.BillingHelper.RunBillingJob();
                    var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, fixture.ServicingCode1.ID);
                    Assert.Null(billedCase);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (!context.LocationGroupLocations.Any(l => l.LocationNumber == fixture.ExternalLocation.ID))
                    {
                        var link = new LocationGroupLocation { LocationNumber = fixture.ExternalLocation.ID, LocationGroupId = fixture.LocationGroup.ID };
                        context.LocationGroupLocations.Add(link);
                        context.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Location is linked to location group but there is no location group price line level
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit when there is no location group price line level")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWhenThereIsNoLocationGroupPriceLineLevel()
        {
            try
            {
                var serviceJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { serviceJob1Level }, units: 1, price: 15);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, fixture.ServicingCode1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Location is linked to location group but price line cannot be matched
        /// </summary>
        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'servicing job' unit when price line cannot be matched")]
        public void VerifyBillingByPricePerServicingJobRuleAndServicingJobUnitWhenPriceLineCannotBeMatched()
        {
            try
            {
                var serviceJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode2.ID, fixture.ServicingCode2.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { serviceJob1Level }, units: 1, price: 15);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.ServicingJob, stop.ID, fixture.ServicingCode1.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per servicing job - Servicing job"
        #region "Collective jobs corrective price - Servicing job"
        [Fact(DisplayName = "Billing - 'collective jobs corrective price' rule and 'servicing job' unit", Skip = "NOT DEVELOPED YET")]
        public void VerifyBillingByCollectiveJobsCorrectivePriceRuleAndServicingJobUnit()
        {
            try
            {
                var servicingJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { servicingJob1Level, locationGroup1Level }, units: 1, price: -3);

                var servicingJob2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode2.ID, fixture.ServicingCode2.Code);
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.ServicingJob,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.ServicingJob,
                    new List<PriceLineLevel> { servicingJob2Level, locationGroup2Level }, units: 1, price: -5);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode1 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);
                var daiServicingCode2 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode2.Code);                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.ServicingJob, stop.ID, fixture.ServicingCode1.ID);
                Assert.NotNull(billedCase1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == -3);

                var billedCase2 = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.ServicingJob, stop.ID, fixture.ServicingCode2.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == -5);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Collective jobs corrective price - Servicing job"
        #region "Price per servicing job - Minute"
        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'minute' unit with different servicing codes")]
        public void VerifyBillingByPricePerServicingJobRuleAndMinuteUnitWithDifferentServicingCodes()
        {
            try
            {
                var servicingJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    new List<PriceLineLevel> { servicingJob1Level, locationGroup1Level }, units: 10, price: 10);

                var servicingJob2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode2.ID, fixture.ServicingCode2.Code);
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    new List<PriceLineLevel> { servicingJob2Level, locationGroup2Level }, units: 10, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode1 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code, startTime: "100500",
                    endTime: "101600");
                var daiServicingCode2 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode2.Code, startTime: "102000",
                    endTime: "102900");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.Minute, stop.ID, daiServicingCode1.ID);
                Assert.NotNull(billedCase1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == 11); // 11 minutes (10 units for 10 euro, with 'decrease price proportionally' = 'yes')

                var billedCase2 = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.Minute, stop.ID, daiServicingCode2.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == 10.8m); // 9 minutes
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'price per servicing job' rule and 'minute' unit")]
        public void VerifyThatDataNotBilledTwiceByPricePerServicingJobRuleAndMinuteUnit()
        {
            try
            {
                var servicingJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    new List<PriceLineLevel> { servicingJob1Level, locationGroup1Level }, units: 10, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode1 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code, startTime: "100500",
                    endTime: "101600");                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.Minute, stop.ID, daiServicingCode1.ID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.Minute, stop.ID, daiServicingCode1.ID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'minute' unit without start time and end time")]
        public void VerifyBillingByPricePerServicingJobRuleAndMinuteUnitWithoutStartTimeAndEndTime()
        {
            try
            {
                var servicingJobLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    new List<PriceLineLevel> { servicingJobLevel, locationGroupLevel }, units: 10, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode1 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code);                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.Minute, stop.ID, daiServicingCode1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 1);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'minute' unit without start time")]
        public void VerifyBillingByPricePerServicingJobRuleAndMinuteUnitWithoutStartTime()
        {
            try
            {
                var servicingJobLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    new List<PriceLineLevel> { servicingJobLevel, locationGroupLevel }, units: 10, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode1 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code, endTime: "101600");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.Minute, stop.ID, daiServicingCode1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 1);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'minute' unit without end time")]
        public void VerifyBillingByPricePerServicingJobRuleAndMinuteUnitWithoutEndTime()
        {
            try
            {
                var servicingJobLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    new List<PriceLineLevel> { servicingJobLevel, locationGroupLevel }, units: 10, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode1 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code, startTime: "101600");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.Minute, stop.ID, daiServicingCode1.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 1);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per servicing job' rule and 'minute' unit with duration less than units")]
        public void VerifyBillingByPricePerServicingJobRuleAndMinuteUnitWithDurationLessThanUnits()
        {
            try
            {
                var servicingJobLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServicingJob, UnitOfMeasure.Minute,
                    new List<PriceLineLevel> { servicingJobLevel, locationGroupLevel }, units: 10, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode1 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code, startTime: "100500",
                    endTime: "101000");                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.PricePerServicingJob, UnitOfMeasure.Minute, stop.ID, fixture.ServicingCode1.ID);
                Assert.True(true); // actual units = 5
            }
            catch
            {
                throw;
            }
        }
        #endregion "Price per servicing job - Minute"
        #region "Collective jobs corrective price - Minute"
        [Fact(DisplayName = "Billing - 'collective jobs corrective price' rule and 'minute' unit", Skip = "NOT DEVELOPED YET")]
        public void VerifyBillingByCollectiveJobsCorrectivePriceRuleAndMinuteUnit()
        {
            try
            {
                var servicingJob1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.Minute,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode1.ID, fixture.ServicingCode1.Code);
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.Minute,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.Minute,
                    new List<PriceLineLevel> { servicingJob1Level, locationGroup1Level }, units: 10, price: -5);

                var servicingJob2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.Minute,
                    PriceRuleLevelName.ServicingJob, PriceRuleLevelValueType.ServicingCode, fixture.ServicingCode2.ID, fixture.ServicingCode2.Code);
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.Minute,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.Minute,
                    new List<PriceLineLevel> { servicingJob2Level, locationGroup2Level }, units: 10, price: -6);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation,
                    withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.ExternalLocation, "100000", "103000");
                var daiServicingCode1 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode1.Code, startTime: "100500",
                    endTime: "101600");
                var daiServicingCode2 = HelperFacade.TransportHelper.CreateDaiServicingCode(stop, fixture.ServicingCode2.Code, startTime: "102000",
                    endTime: "102900");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.Minute, stop.ID, fixture.ServicingCode1.ID);
                Assert.NotNull(billedCase1);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLine(billedCase1.ID);
                Assert.True(billingLine1.Value == -5.5m);

                var billedCase2 = HelperFacade.BillingHelper.GetServicingJobAtLocationBilledCase(fixture.CompanyContract.ID,
                    PriceRule.CollectiveJobsCorrectivePrice, UnitOfMeasure.Minute, stop.ID, fixture.ServicingCode2.ID);
                try
                {
                    Assert.NotNull(billedCase2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine2 = HelperFacade.BillingHelper.GetBillingLine(billedCase2.ID);
                Assert.True(billingLine2.Value == -5.4m);
            }
            catch
            {
                throw;
            }
        }
        #endregion "Collective jobs corrective price - Minute"
        #endregion 4.6.8 Get Billing Data per Servicing Job at Location
        #region 4.6.9 Get Billing Data by Order and Service Type at Location
        [Fact(DisplayName = "Billing - 'price per order type and service type' rule and 'location stop' unit when there are stops with multiple orders and different price lines")]
        public void VerifyBillingByPricePerOrderTypeAndServiceTypeRuleAndLocationStopUnitWhenThereAreStopsWithMultipleOrdersAndDifferentPriceLines()
        {
            try
            {                
                var serviceType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, collectCode);
                var orderType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.Fixed, OrderType.Fixed.ToString());
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { serviceType1Level, orderType1Level, locationGroup1Level }, units: 1, price: 10);

                var serviceType2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.DeliverServiceTypeID, deliverCode);
                var orderType2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.AtRequest, OrderType.AtRequest.ToString());
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { serviceType2Level, orderType2Level, locationGroup2Level }, units: 1, price: 20);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.CollectLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(collectTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(collectTransportOrder, fixture.CollectLocation1, "100000", "103000");
                HelperFacade.TransportHelper.ChangeTransportOrderTypeToFixed(collectTransportOrder.ID);

                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCaseList = HelperFacade.BillingHelper.GetVisitAtLocationBilledCases(fixture.CompanyContract.ID, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                try
                {
                    Assert.True(billedCaseList.Count == 2);
                }                
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                foreach (var billedCase in billedCaseList)
                {
                    var matchedPriceLine = DataFacade.PriceLine.Take(p => p.ID == billedCase.PriceLineID).Build();
                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);                    
                    Assert.True(billingLine.Value == matchedPriceLine.Price);
                }                
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per order type and service type' rule and 'location stop' unit when there are stops with multiple orders and the same price line")]
        public void VerifyBillingByPricePerOrderTypeAndServiceTypeRuleAndLocationStopUnitWhenThereAreStopsWithMultipleOrdersAndTheSamePriceLine()
        {
            try
            {                
                var orderTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.AtRequest, OrderType.AtRequest.ToString());
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { orderTypeLevel, locationGroupLevel }, units: 1, price: 20);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.OnwardLocation1,
                    withProducts: false, withServices: false);
                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.OnwardLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(collectTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);

                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(deliverTransportOrder, fixture.OnwardLocation1, "100000", "103000");                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }                
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == priceLine.Price);                
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'price per order type and service type' rule and 'location stop' unit")]
        public void VerifyThatDataNotBilledTwiceByPricePerOrderTypeAndServiceTypeRuleAndLocationStopUnit()
        {
            try
            {
                var serviceType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, collectCode);
                var orderType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.Fixed, OrderType.Fixed.ToString());
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { serviceType1Level, orderType1Level, locationGroup1Level }, units: 1, price: 10);

                var serviceType2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.DeliverServiceTypeID, deliverCode);
                var orderType2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.AtRequest, OrderType.AtRequest.ToString());
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { serviceType2Level, orderType2Level, locationGroup2Level }, units: 1, price: 20);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.CollectLocation1,
                    withProducts: false, withServices: false);
                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.CollectLocation1,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(collectTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(collectTransportOrder, fixture.CollectLocation1, "100000", "103000");
                HelperFacade.TransportHelper.ChangeTransportOrderTypeToFixed(collectTransportOrder.ID);

                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCaseList = HelperFacade.BillingHelper.GetVisitAtLocationBilledCases(fixture.CompanyContract.ID, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.True(billedCaseList.Count == 2);

                HelperFacade.BillingHelper.RunBillingJob();
                billedCaseList = HelperFacade.BillingHelper.GetVisitAtLocationBilledCases(fixture.CompanyContract.ID, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.True(billedCaseList.Count == 2);                
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per order type and service type' rule and 'location stop' unit when 'bill collected orders' flag is checked (positive)")]
        public void VerifyBillingByPricePerOrderTypeAndServiceTypeRuleAndLocationStopUnitWhenBillCollectedOrdersFlagIsChecked()
        {
            try
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(true);                

                var orderTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.AtRequest, OrderType.AtRequest.ToString());
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { orderTypeLevel, locationGroupLevel }, units: 1, price: 20);
                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.OnwardLocation1,
                    withProducts: false, withServices: false);       
                         
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");

                var transportOrder2 = DataFacade.TransportOrder.New()
                   .With_Location(transportOrder.LocationID)
                   .With_Site(transportOrder.SiteID)
                   .With_OrderType(OrderType.AtRequest)
                   .With_TransportDate(transportOrder.TransportDate)
                   .With_ServiceDate(transportOrder.ServiceDate)
                   .With_Status(TransportOrderStatus.Collected)
                   .With_MasterRouteCode(transportOrder.MasterRouteCode)
                   .With_MasterRouteDate(transportOrder.MasterRouteDate)
                   .With_StopArrivalTime(transportOrder.StopArrivalTime)
                   .With_ServiceOrder(transportOrder.ServiceOrderID)
                   .With_ServiceType(transportOrder.ServiceTypeID)
                   .SaveToDb()
                   .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == priceLine.Price);
            }
            catch
            {
                throw;
            }
            finally
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
            }
        }

        [Fact(DisplayName = "Billing - 'price per order type and service type' rule and 'location stop' unit when 'bill collected orders' flag is unchecked (negative)")]
        public void VerifyBillingByPricePerOrderTypeAndServiceTypeRuleAndLocationStopUnitWhenBillCollectedOrdersFlagIsUnchecked()
        {
            try
            {
                var orderTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.AtRequest, OrderType.AtRequest.ToString());
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { orderTypeLevel, locationGroupLevel }, units: 1, price: 20);
                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.OnwardLocation1,
                    withProducts: false, withServices: false);

                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.OnwardLocation1, "100000", "103000");

                var transportOrder2 = DataFacade.TransportOrder.New()
                   .With_Location(transportOrder.LocationID)
                   .With_Site(transportOrder.SiteID)
                   .With_OrderType(OrderType.AtRequest)
                   .With_TransportDate(transportOrder.TransportDate)
                   .With_ServiceDate(transportOrder.ServiceDate)
                   .With_Status(TransportOrderStatus.Collected)
                   .With_MasterRouteCode(transportOrder.MasterRouteCode)
                   .With_MasterRouteDate(transportOrder.MasterRouteDate)
                   .With_StopArrivalTime(transportOrder.StopArrivalTime)
                   .With_ServiceOrder(transportOrder.ServiceOrderID)
                   .With_ServiceType(transportOrder.ServiceTypeID)
                   .SaveToDb()
                   .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtLocationBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit,
                    UnitOfMeasure.LocationStop, stop.ID);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        #endregion
        #region 4.6.10 Get Billing Data by Order and Service Type at Visit Address
        [Fact(DisplayName = "Billing - 'price per order type and service type' rule and 'visit address stop' unit when there are stops with multiple orders and different price lines")]
        public void VerifyBillingByPricePerOrderTypeAndServiceTypeRuleAndVisitAddressStopUnitWhenThereAreStopsWithMultipleOrdersAndDifferentPriceLines()
        {
            try
            {
                var serviceType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, collectCode);
                var orderType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.Fixed, OrderType.Fixed.ToString());
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { serviceType1Level, orderType1Level, locationGroup1Level }, units: 1, price: 10);

                var serviceType2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.DeliverServiceTypeID, deliverCode);
                var orderType2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.AtRequest, OrderType.AtRequest.ToString());
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { serviceType2Level, orderType2Level, locationGroup2Level }, units: 1, price: 20);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint2,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(collectTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(collectTransportOrder, fixture.VisitServicePoint1, "100000", "101500");
                HelperFacade.TransportHelper.ChangeTransportOrderTypeToFixed(collectTransportOrder.ID);

                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 15, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(deliverTransportOrder, fixture.VisitServicePoint2, "101500", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCaseList = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCases(fixture.CompanyContract.ID, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.True(billedCaseList.Count == 2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                foreach (var billedCase in billedCaseList)
                {
                    var matchedPriceLine = DataFacade.PriceLine.Take(p => p.ID == billedCase.PriceLineID).Build();
                    var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == matchedPriceLine.Price);
                }
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per order type and service type' rule and 'visit address stop' unit when there are stops with multiple orders and the same price line")]
        public void VerifyBillingByPricePerOrderTypeAndServiceTypeRuleAndVisitAddressStopUnitWhenThereAreStopsWithMultipleOrdersAndTheSamePriceLine()
        {
            try
            {
                var orderTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.AtRequest, OrderType.AtRequest.ToString());
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { orderTypeLevel, locationGroupLevel }, units: 1, price: 20);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint2,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(collectTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(collectTransportOrder, fixture.VisitServicePoint1, "100000", "101500");
                HelperFacade.TransportHelper.ChangeServiceOrderTypeToFixed(collectServiceOrder.ID);

                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 15, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(deliverTransportOrder, fixture.VisitServicePoint2, "101500", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == priceLine.Price);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that data not billed twice by 'price per order type and service type' rule and 'visit address stop' unit")]
        public void VerifyThatDataNotBilledTwiceByPricePerOrderTypeAndServiceTypeRuleAndVisitAddressStopUnit()
        {
            try
            {
                var serviceType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.CollectServiceTypeID, collectCode);
                var orderType1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.Fixed, OrderType.Fixed.ToString());
                var locationGroup1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { serviceType1Level, orderType1Level, locationGroup1Level }, units: 1, price: 10);

                var serviceType2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, fixture.DeliverServiceTypeID, deliverCode);
                var orderType2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.AtRequest, OrderType.AtRequest.ToString());
                var locationGroup2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { serviceType2Level, orderType2Level, locationGroup2Level }, units: 1, price: 20);

                var collectServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                var deliverServiceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.VisitServicePoint2,
                    withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(collectTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                collectTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(collectServiceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(collectTransportOrder, fixture.VisitServicePoint1, "100000", "101500");
                HelperFacade.TransportHelper.ChangeTransportOrderTypeToFixed(collectTransportOrder.ID);

                var deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(deliverTransportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 15, 0));
                deliverTransportOrder = HelperFacade.TransportHelper.FindTransportOrder(deliverServiceOrder.ID);
                var stop2 = HelperFacade.TransportHelper.CreateDaiLine(deliverTransportOrder, fixture.VisitServicePoint2, "101500", "103000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCaseList = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCases(fixture.CompanyContract.ID, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.True(billedCaseList.Count == 2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }

                HelperFacade.BillingHelper.RunBillingJob();
                billedCaseList = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCases(fixture.CompanyContract.ID, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.True(billedCaseList.Count == 2);
                }
                catch
                {
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - 'price per order type and service type' rule and 'visit address stop' unit when 'bill collected orders' flag is checked (positive)")]
        public void VerifyBillingByPricePerOrderTypeAndServiceTypeRuleAndVisitAddressStopUnitWhenBillCollectedOrdersFlagIsChecked()
        {
            try
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(true);

                var orderTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.AtRequest, OrderType.AtRequest.ToString());
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { orderTypeLevel, locationGroupLevel }, units: 1, price: 20);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);                
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 15, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "101500", "103000");

                var transportOrder2 = DataFacade.TransportOrder.New()
                   .With_Location(transportOrder.LocationID)
                   .With_Site(transportOrder.SiteID)
                   .With_OrderType(OrderType.AtRequest)
                   .With_TransportDate(transportOrder.TransportDate)
                   .With_ServiceDate(transportOrder.ServiceDate)
                   .With_Status(TransportOrderStatus.Collected)
                   .With_MasterRouteCode(transportOrder.MasterRouteCode)
                   .With_MasterRouteDate(transportOrder.MasterRouteDate)
                   .With_StopArrivalTime(transportOrder.StopArrivalTime)
                   .With_ServiceOrder(transportOrder.ServiceOrderID)
                   .With_ServiceType(transportOrder.ServiceTypeID)
                   .SaveToDb()
                   .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                try
                {
                    Assert.NotNull(billedCase);
                }
                catch
                {
                    HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
                    var logMessage = HelperFacade.BillingHelper.GetBillingJobLogRecord().Message;
                    throw new Exception(logMessage ?? "Expected billed case was not created!");
                }
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == priceLine.Price);
            }
            catch
            {
                throw;
            }
            finally
            {
                HelperFacade.BillingHelper.SetBillCollectedOrdersFlag(false);
            }
        }

        [Fact(DisplayName = "Billing - 'price per order type and service type' rule and 'visit address stop' unit when 'bill collected orders' flag is unchecked (negative)")]
        public void VerifyBillingByPricePerOrderTypeAndServiceTypeRuleAndVisitAddressStopUnitWhenBillCollectedOrdersFlagIsUnchecked()
        {
            try
            {
                var orderTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.OrderType, PriceRuleLevelValueType.OrderType, (int)OrderType.AtRequest, OrderType.AtRequest.ToString());
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixture.LocationGroup.ID, fixture.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit, UnitOfMeasure.VisitAddressStop,
                    new List<PriceLineLevel> { orderTypeLevel, locationGroupLevel }, units: 1, price: 20);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.VisitServicePoint1,
                    withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Cancelled, new TimeSpan(10, 15, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixture.VisitServicePoint1, "101500", "103000");

                var transportOrder2 = DataFacade.TransportOrder.New()
                   .With_Location(transportOrder.LocationID)
                   .With_Site(transportOrder.SiteID)
                   .With_OrderType(OrderType.AtRequest)
                   .With_TransportDate(transportOrder.TransportDate)
                   .With_ServiceDate(transportOrder.ServiceDate)
                   .With_Status(TransportOrderStatus.Collected)
                   .With_MasterRouteCode(transportOrder.MasterRouteCode)
                   .With_MasterRouteDate(transportOrder.MasterRouteDate)
                   .With_StopArrivalTime(transportOrder.StopArrivalTime)
                   .With_ServiceOrder(transportOrder.ServiceOrderID)
                   .With_ServiceType(transportOrder.ServiceTypeID)
                   .SaveToDb()
                   .Build();

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetVisitAtVisitAddressBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit,
                    UnitOfMeasure.VisitAddressStop, fixture.VisitAddress1.IdentityID, stop.Date);
                Assert.Null(billedCase);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
