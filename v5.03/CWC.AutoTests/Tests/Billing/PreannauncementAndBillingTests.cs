using Cwc.Billing.Jobs;
using Cwc.Contracts.Enums;
using Cwc.Contracts.Model;
using Cwc.Ordering;
using Cwc.Transport.Enums;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using Xunit;

namespace CWC.AutoTests.Tests.Billing
{
    public class PreannauncementAndBillingTests: IClassFixture<BillingTestFixture>, IDisposable
    {
        private string filePath;
        private DateTime actualEffectiveDate;
        private const string companyGroupName = "AutoTestManagement";
        private const string collectCode = "COLL";
        BillingTestFixture fixtureBilling;
        DateTime today = DateTime.Now;

        public PreannauncementAndBillingTests(BillingTestFixture setUpFixture)
        {
            fixtureBilling = setUpFixture;
            actualEffectiveDate = fixtureBilling.CompanyContract.EffectiveDate;
            var folder = System.IO.Path.Combine("Exchange", "PreannauncementAndBillingExport", "");
            var basePath = System.IO.Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            filePath = System.IO.Path.Combine(basePath, folder);
            //Configure folder for Preannauncement Settings
            HelperFacade.PreannauncementAndBillingHelper.ConfigureFolderForPreannauncementSettings(filePath);
        }

        public void Dispose()
        {

            try
            {
                HelperFacade.ContractHelper.EditEffectiveDateOfContract(fixtureBilling.CompanyContract, actualEffectiveDate);
                HelperFacade.ContractHelper.EditEndDateOfContract(fixtureBilling.CompanyContract, null);
            }
            finally
            {
                HelperFacade.TransportHelper.ClearTestData();
                HelperFacade.BillingHelper.ClearTestData();
                HelperFacade.ContractHelper.ClearTestData();
            }
        }

        [Fact(DisplayName ="Preannauncement And Billing Tests - Verify that file is generated")]
        public void VerifyThatFileIsGenerated()
        {
            try
            {
                //Preconditions (ServiceOrder, TransportOrder, dai_Line creation)
                var contractedLocationLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.ContractedLocation, PriceRuleLevelValueType.Location, fixtureBilling.ExternalLocation.IdentityID, fixtureBilling.ExternalLocation.Code);
                var locationGroupLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    PriceRuleLevelName.LocationGroup, PriceRuleLevelValueType.LocationGroup, fixtureBilling.LocationGroup.ID, fixtureBilling.LocationGroup.Code);
                var priceLine = HelperFacade.ContractHelper.CreatePriceLine(fixtureBilling.CompanyContract, PriceRule.PricePerVisit, UnitOfMeasure.LocationStop,
                    new List<PriceLineLevel> { contractedLocationLevel, locationGroupLevel }, units: 1, price: 33.468m);

                var serviceOrder = HelperFacade.TransportHelper.CreateServiceOrderWithContentLite(collectCode, today, GenericStatus.Confirmed, fixtureBilling.ExternalLocation,
                new List<DeliveryProductSpecification>());

                //Generate Transport Order
                HelperFacade.TransportHelper.RunCitAllocationJob();

                var transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID);
                HelperFacade.TransportHelper.FillRouteDataForTransportOrder(transportOrder, TransportOrderStatus.Completed, new TimeSpan(10, 0, 0));
                transportOrder = HelperFacade.TransportHelper.FindTransportOrder(serviceOrder.ID); ;
                var stop = HelperFacade.TransportHelper.CreateDaiLine(transportOrder, fixtureBilling.ExternalLocation, "100000", "103000");

                //Generate Billing Lines
                HelperFacade.BillingHelper.RunBillingJob();

                //Create CompanyGroup and Link Company to this group if group with such company is not exists
                var companyLink = DataFacade.PreannouncementBillingExportSettingGroupCompanyLink.Take(pbe=>pbe.CompanyID == fixtureBilling.Company.IdentityID);
                if (companyLink == null)
                {
                    var group = HelperFacade.PreannauncementAndBillingHelper.CreateNewCompanyGroup(companyGroupName);
                    HelperFacade.PreannauncementAndBillingHelper.AddCompanyToGroup(fixtureBilling.Company.IdentityID, group.ID);
                }

                //Generate XML file
                HelperFacade.PreannauncementAndBillingHelper.RunExportBillingPreannouncementsJob();

                //Parse xml file
                

            }
            catch
            {
                throw;
            }
        }
    }
}
