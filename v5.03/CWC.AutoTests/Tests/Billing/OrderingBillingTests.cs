using Cwc.Billing.Model;
using Cwc.Contracts.Enums;
using Cwc.Contracts.Model;
using Cwc.Ordering;
using Cwc.Transport.Enums;
using CWC.AutoTests.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace CWC.AutoTests.Tests.Billing
{
    [Xunit.Collection("Billing")]
    [Trait("Category", "Billing")]
    public class OrderingBillingTests : BaseTest, IClassFixture<BillingTestFixture>, IDisposable
    {
        private BillingTestFixture fixture;
        private BillingLine billingLine;
        private PriceLine priceLine;
        private const string collectCode = "COLL";
        private const string deliverCode = "DELV";
        private const string replenishmentCode = "REPL";
        private const string servicingCode = "SERV";
        DateTime today = DateTime.Today;

        public OrderingBillingTests(BillingTestFixture setupFixture)
        {
            this.fixture = setupFixture;
        }

        public void Dispose()
        {
            HelperFacade.TransportHelper.ClearTestData();
            HelperFacade.BillingHelper.ClearTestData();
            HelperFacade.ContractHelper.ClearPriceLines(fixture.CompanyContract.ID);
        }                

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for completed collect service order - contracted level")]
        public void VerifyBillingByPricePerServiceOrderRuleForCompletedCollectServiceOrderContractedLevel()
        {            
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);
                              
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "DEP", onwardLocation: fixture.OnwardLocation1);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "DEP", onwardLocation: fixture.OnwardLocation2);
                                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCaseList = HelperFacade.BillingHelper.GetServiceOrderBilledCases(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.True(billedCaseList.Count == 2);

                foreach(var billedCase in billedCaseList)
                {
                    billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == priceLine.Price);                    
                }
            }
            catch
            { 
                throw;
            }            
        }        

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for collect service order with multiple onward hispacks [SC20498]")]
        public void VerifyBillingByPricePerServiceOrderRuleForCollectServiceOrderWithMultipleOnwardHispacks()
        {
            try
            {
                // onward location level is configured for OnwardLocation2 because his_pack with this location should be matched
                var onwardLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.OnwardLocation, PriceRuleLevelValueType.Location, fixture.OnwardLocation2.IdentityID, fixture.OnwardLocation2.Code);
                priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { onwardLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "DEP", onwardLocation: fixture.OnwardLocation1, packageNumber: "1");
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "110000", "DEP", onwardLocation: fixture.OnwardLocation2, packageNumber: "1");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.NotNull(billedCase);

                billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == priceLine.Price);
                
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for delivery service order with multiple collect hispacks [SC20498]")]
        public void VerifyBillingByPricePerServiceOrderRuleForDeliveryServiceOrderWithMultipleCollectHispacks()
        {
            try
            {
                // Collect location level is configured for CollectLocation2 because his_pack with this location should be matched
                var collectLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.CollectLocation, PriceRuleLevelValueType.Location, fixture.CollectLocation2.IdentityID, fixture.CollectLocation2.Code);
                priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { collectLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "DEP", collectLocation: fixture.CollectLocation1, packageNumber: "2");
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "110000", "DEP", collectLocation: fixture.CollectLocation2, packageNumber: "2");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.NotNull(billedCase);

                billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == priceLine.Price);

            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for completed collect service order - onward level")]
        public void VerifyBillingByPricePerServiceOrderRuleForCompletedCollectServiceOrderOnwardLevel()
        {
            try
            {
                var onwardLocationLevel1 = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.OnwardLocation, PriceRuleLevelValueType.Location, fixture.OnwardLocation1.IdentityID, fixture.OnwardLocation1.Code);
                var priceLine1 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, 
                    UnitOfMeasure.ServiceOrder, new List<PriceLineLevel> { onwardLocationLevel1 }, units: 1, price: 10);

                var onwardLocationLevel2 = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.OnwardLocation, PriceRuleLevelValueType.Location, fixture.OnwardLocation2.IdentityID, fixture.OnwardLocation2.Code);
                var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, new List<PriceLineLevel> { onwardLocationLevel2 }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "DEP", onwardLocation: fixture.OnwardLocation1);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "DEP", onwardLocation: fixture.OnwardLocation2);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCaseList = HelperFacade.BillingHelper.GetServiceOrderBilledCases(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.Equal(2, billedCaseList.Count);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLineByPriceLine(priceLine1.ID);
                Assert.True(billingLine1.Value == priceLine1.Price);

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLineByPriceLine(priceLine2.ID);
                Assert.True(billingLine2.Value == priceLine2.Price);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for completed deliver service order - collect level")]
        public void VerifyBillingByPricePerServiceOrderRuleForCompletedDeliverServiceOrderCollectLevel()
        {
            try
            {
                var collectLocationLevel1 = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.CollectLocation, PriceRuleLevelValueType.Location, fixture.CollectLocation1.IdentityID, fixture.CollectLocation1.Code);
                var priceLine1 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, new List<PriceLineLevel> { collectLocationLevel1 }, units: 1, price: 10);

                var collectLocationLevel2 = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.CollectLocation, PriceRuleLevelValueType.Location, fixture.CollectLocation2.IdentityID, fixture.CollectLocation2.Code);
                var priceLine2 = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, new List<PriceLineLevel> { collectLocationLevel2 }, units: 1, price: 12);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "TRK", collectLocation: fixture.CollectLocation1);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "TRK", collectLocation: fixture.CollectLocation2);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCaseList = HelperFacade.BillingHelper.GetServiceOrderBilledCases(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.Equal(2, billedCaseList.Count);

                var billingLine1 = HelperFacade.BillingHelper.GetBillingLineByPriceLine(priceLine1.ID);
                Assert.True(billingLine1.Value == priceLine1.Price);

                var billingLine2 = HelperFacade.BillingHelper.GetBillingLineByPriceLine(priceLine2.ID);
                Assert.True(billingLine2.Value == priceLine2.Price);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for completed replenishment service order")]
        public void VerifyBillingByPricePerServiceOrderRuleForCompletedReplenishmentServiceOrder()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, replenishmentCode, fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "TRK", collectLocation: fixture.CollectLocation1);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "101000", "TRK", collectLocation: fixture.CollectLocation2);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "102000", "DEP", onwardLocation: fixture.OnwardLocation1);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "103000", "DEPSPL", onwardLocation: fixture.OnwardLocation2);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCaseList = HelperFacade.BillingHelper.GetServiceOrderBilledCases(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.Equal(4, billedCaseList.Count);

                foreach (var billedCase in billedCaseList)
                {
                    billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == priceLine.Price);
                }
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for canceled by customer collect service order")]
        public void VerifyBillingByPricePerServiceOrderRuleForCanceledByCustomerCollectServiceOrder()
        {
            try
            {                
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.CancelServiceOrder(serviceOrder, isCustomerResponsible: true);                
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "TRK", onwardLocation: fixture.OnwardLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);                
                Assert.NotNull(billedCase);

                billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);                
                Assert.True(billingLine.Value == 10);
            }
            catch
            { 
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for canceled by CIT collect service order with existing transport order")]
        public void VerifyBillingByPricePerServiceOrderRuleForCanceledByCitCollectServiceOrderWithExistingTransportOrder()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.CancelServiceOrder(serviceOrder, isCustomerResponsible: false);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "TRK", onwardLocation: fixture.OnwardLocation1);                
                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);                
                Assert.True(billedCase.ActualUnits == 1);
                Assert.True(billedCase.DateBilled == serviceOrder.ServiceDate);

                billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Units == billedCase.ActualUnits);
                Assert.True(billingLine.Value == 10);
                Assert.True(billingLine.DateBilled == billedCase.DateBilled);
                Assert.True(billingLine.PeriodBilledFrom == billedCase.PeriodBilledFrom);
                Assert.True(billingLine.PeriodBilledTo == billedCase.PeriodBilledTo);
                Assert.True(billingLine.LocationID == fixture.ExternalLocation.IdentityID);
                Assert.True(billingLine.ContractID == fixture.CompanyContract.ID);
            }
            catch
            {
                throw;
            }            
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for completed deliver service order without transport order")]
        public void VerifyBillingByPricePerServiceOrderRuleForCompletedServicingServiceOrderWithoutTransportOrder()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation, withProducts: false, withServices: true);                
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.NotNull(billedCase);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for canceled by customer deliver service order")]
        public void VerifyBillingByPricePerServiceOrderRuleForCanceledByCustomerDeliverServiceOrder()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.CancelServiceOrder(serviceOrder, isCustomerResponsible: true);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "TRKSPL", collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.NotNull(billedCase);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for canceled by CIT deliver service order with existing transport order")]
        public void VerifyBillingByPricePerServiceOrderRuleForCanceledByCitDeliverServiceOrderWithExistingTransportOrder()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.CancelServiceOrder(serviceOrder, isCustomerResponsible: false);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "TRKSPL", collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.NotNull(billedCase);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 10);
            }
            catch
            {                
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for canceled by customer replenishment service order")]
        public void VerifyBillingByPricePerServiceOrderRuleForCanceledByCustomerReplenishmentServiceOrder()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, replenishmentCode, fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.CancelServiceOrder(serviceOrder, isCustomerResponsible: true);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "120000", "LOC", onwardLocation: fixture.OnwardLocation1);                
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "DEP", collectLocation: fixture.CollectLocation1);
                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.NotNull(billedCase);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 10);
            }
            catch
            { 
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for canceled by CIT replenishment service order with existing transport order")] // completed + canceled theory
        public void VerifyBillingByPricePerServiceOrderRuleForCanceledByCitReplenishmentServiceOrderWithExistingTransportOrder()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, replenishmentCode, fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.CancelServiceOrder(serviceOrder, isCustomerResponsible: false);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "120000", "LOC", onwardLocation: fixture.OnwardLocation1);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "DEP", collectLocation: fixture.CollectLocation1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.NotNull(billedCase);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 10);
            }
            catch 
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule for completed servicing service order")] 
        public void VerifyBillingByPricePerServiceOrderRuleForCompletedServicingServiceOrder()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation, withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.NotNull(billedCase);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == 10);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule with surcharge for completed deliver service order")]
        public void VerifyBillingByPricePerServiceOrderRuleWithSurchargeForCompletedDeliverServiceOrder()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10, isApplySurcharges: true);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.SurchargeFixedPrice, UnitOfMeasure.Value,
                    null, price: 5); // 5 EUR surcharge
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.SurchargePercentPrice, UnitOfMeasure.Percentage,
                    null, units: 5); // 5% surcharge
                  
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "TRKSPL", collectLocation: fixture.CollectLocation1);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "TRKSPL", collectLocation: fixture.CollectLocation2);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCaseList = HelperFacade.BillingHelper.GetServiceOrderBilledCases(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.Equal(2, billedCaseList.Count);

                foreach (var billedCase in billedCaseList)
                {
                    billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == 15.5m);
                }
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order' rule with surcharge without settings for completed deliver service order")]
        public void VerifyBillingByPricePerServiceOrderRuleWithSurchargeWithoutSettingsForCompletedDeliverServiceOrder()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10, isApplySurcharges: true);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "TRKSPL", collectLocation: fixture.CollectLocation1);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "TRKSPL", collectLocation: fixture.CollectLocation2);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCaseList = HelperFacade.BillingHelper.GetServiceOrderBilledCases(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.Equal(2, billedCaseList.Count);

                foreach (var billedCase in billedCaseList)
                {
                    billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                    Assert.True(billingLine.Value == priceLine.Price);
                }
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify billing by 'price per ordered product' rule for completed service order with multiple products", Skip = "NOT DEVELOPED YET")]
        public void VerifyBillingByPricePerOrderedProductRuleForCompletedServiceOrder()
        {
            try
            {
                var product1Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderedProduct, UnitOfMeasure.Product,
                   PriceRuleLevelName.Product, PriceRuleLevelValueType.Product, fixture.Note100EurProduct.ID, fixture.Note100EurProduct.ProductCode);                
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderedProduct, UnitOfMeasure.Product,
                    new List<PriceLineLevel> { product1Level }, units: 1, price: 12);

                var product2Level = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerOrderedProduct, UnitOfMeasure.Product,
                   PriceRuleLevelName.Product, PriceRuleLevelValueType.Product, fixture.Note50EurProduct.ID, fixture.Note50EurProduct.ProductCode);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerOrderedProduct, UnitOfMeasure.Product,
                    new List<PriceLineLevel> { product2Level }, units: 1, price: 13);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetOrderedProductBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerOrderedProduct,
                    UnitOfMeasure.Product, serviceOrder.IdentityID, fixture.Note100EurProduct.ID);
                var billedCase2 = HelperFacade.BillingHelper.GetOrderedProductBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerOrderedProduct,
                    UnitOfMeasure.Product, serviceOrder.IdentityID, fixture.Note50EurProduct.ID);
                Assert.True(true); // TO DO
            }
            catch
            {               
                throw;
            }            
        }        

        [Fact(DisplayName = "Billing - Verify billing by 'price per service order packing start' rule for completed service order")]
        public void VerifyBillingByPricePerServiceOrderPackingStartRuleForCompletedServiceOrder()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrderPackingStart, 
                    UnitOfMeasure.ServiceOrder, PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, 
                    fixture.ExternalLocation.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrderPackingStart, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);
                
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.CashCenterHelper.CreatePickAndPackStockOrder(serviceOrder, fixture.ExternalLocation);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                
                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrderPackingStart,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.True(billedCase.ActualUnits == 1);
                Assert.True(billedCase.DateBilled == serviceOrder.ServiceDate);

                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);                
                Assert.True(billingLine.Value == 10);
            }
            catch
            {
                throw;
            }
        }               

        [Fact(DisplayName = "Billing - Verify billing by 'price per canceled service order' rule")]
        public void VerifyBillingByPricePerCanceledServiceOrderRule()
        {

            var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrderPackingStart,
                UnitOfMeasure.ServiceOrder, PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID,
                fixture.ExternalLocation.Code);
            HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrderPackingStart, UnitOfMeasure.ServiceOrder,
                new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);

            var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation, withProducts: true, withServices: false);
            HelperFacade.TransportHelper.CancelServiceOrder(serviceOrder, isCustomerResponsible: true);

            HelperFacade.BillingHelper.RunBillingJob();
            var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrderPackingStart,
                UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
            Assert.True(true); // TO DO
        }

        [Fact(DisplayName = "Billing - Verify billing by 'corrective price per service order for co-located location groups' rule for completed servicing service order", Skip = "NOT DEVELOPED YET")] 
        public void VerifyBillingByCorrectivePricePerServiceOrderForColocatedLocationGroupsRuleForCompletedServicingServiceOrder()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.CorrectivePricePerServiceOrderForColocatedLocationGroups,
                   UnitOfMeasure.ServiceOrder, PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID,
                   fixture.ExternalLocation.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.CorrectivePricePerServiceOrderForColocatedLocationGroups, 
                    UnitOfMeasure.ServiceOrder, new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: -10);
                
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, servicingCode, fixture.ExternalLocation, withProducts: false, withServices: true);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(12, 0, 0));
                HelperFacade.TransportHelper.SaveDaiLine(transportOrder, fixture.OnwardLocation1, "100000");

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, 
                    PriceRule.CorrectivePricePerServiceOrderForColocatedLocationGroups, UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.NotNull(billedCase); 
                
                var billingLine = HelperFacade.BillingHelper.GetBillingLine(billedCase.ID);
                Assert.True(billingLine.Value == -10);
            }
            catch
            {
                throw;
            }
        }

        // NEGATIVE CASES

        [Fact(DisplayName = "Billing - Verify that canceled by CIT order is not billed when price rule is 'price per canceled service order'")] //~ COLL, DELV, REPL, SERV theory
        public void VerifyThatCanceledByCitOrderIsNotBilledWhenPriceRuleIsPricePerCanceledServiceOrder()
        {
            try
            {
                //HelperFacade.ContractHelper.ConfigurePriceLine(fixture.companyContract, PriceRule.PricePerCancelledServiceOrder, UnitOfMeasure.ServiceOrder, 
                //    PriceRuleLevelName.OrderType, units: 1, price: 10, isApplySurcharges: false);                
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.CancelServiceOrder(serviceOrder, isCustomerResponsible: false);
                HelperFacade.BillingHelper.RunBillingJob();
                Assert.True(true); // TO DO
            }
            catch
            {                
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that order not billed twice by 'price per service order' rule")]
        public void VerifyThatOrderNotBilledTwiceByPricePerServiceOrderRule()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                   PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID, fixture.ExternalLocation.Code);
                priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrder, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, collectCode, fixture.ExternalLocation, withProducts: false, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);
                HelperFacade.TransportHelper.SaveHisPack(transportOrder, "100000", "DEP", onwardLocation: fixture.OnwardLocation1);                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrder,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);                
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that order not billed twice by 'price per service order packing start' rule")]
        public void VerifyThatOrderNotBilledTwiceByPricePerServiceOrderPackingStartRule()
        {
            try
            {
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerServiceOrderPackingStart,
                    UnitOfMeasure.ServiceOrder, PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixture.ExternalLocation.IdentityID,
                    fixture.ExternalLocation.Code);
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrderPackingStart, UnitOfMeasure.ServiceOrder,
                    new List<PriceLineLevel> { contractedLocationLevel }, units: 1, price: 10);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today, GenericStatus.Confirmed, deliverCode, fixture.ExternalLocation, withProducts: true, withServices: false);
                HelperFacade.TransportHelper.RunCitAllocationJob();
                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.CashCenterHelper.CreatePickAndPackStockOrder(serviceOrder, fixture.ExternalLocation);
                HelperFacade.TransportHelper.ChangeGenericStatus(serviceOrder, GenericStatus.Completed);
                HelperFacade.TransportHelper.ChangeTransportOrderStatus(transportOrder, TransportOrderStatus.Completed);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase1 = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrderPackingStart,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.NotNull(billedCase1);

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase2 = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrderPackingStart,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.True(billedCase2.DateCreated == billedCase1.DateCreated);
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Billing - Verify that order of another company not billed by 'price per service order packing start' rule")]
        public void VerifyThatOrderOfAnotherCompanyNotBilledByPricePerServiceOrderPackingStartRule()
        {
            try
            {                
                HelperFacade.ContractHelper.CreatePriceLine(fixture.CompanyContract, PriceRule.PricePerServiceOrderPackingStart, UnitOfMeasure.ServiceOrder,
                    null, units: 1, price: 10);
                var location = ObjectBuilder.DataFacade.Location.Take(l => l.Code == "JG04").Build();
                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrder(today.AddDays(1), GenericStatus.Confirmed, deliverCode, location, withProducts: true, withServices: false);                
                HelperFacade.CashCenterHelper.CreatePickAndPackStockOrder(serviceOrder, fixture.ExternalLocation);                

                HelperFacade.BillingHelper.RunBillingJob();
                var billedCase = HelperFacade.BillingHelper.GetServiceOrderBilledCase(fixture.CompanyContract.ID, PriceRule.PricePerServiceOrderPackingStart,
                    UnitOfMeasure.ServiceOrder, serviceOrder.IdentityID);
                Assert.Null(billedCase);                
            }
            catch
            {
                throw;
            }
        }
    }
}
