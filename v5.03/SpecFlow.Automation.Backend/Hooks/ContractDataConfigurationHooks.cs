using Cwc.BaseData;
using Cwc.Contracts;
using Cwc.Security;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Specflow.Automation.Backend.Hooks
{
    [Binding, Scope(Tag = "contract-data-generation-required")]
    public class ContractDataConfigurationHooks
    {
        private const string noteContractNumber = "55051";
        private const string coinContractNumber = "55061";
        private const string foreignContractNumber = "55071";

        private static DateTime today = DateTime.Today;
        private static LoginResult login = SecurityFacade.LoginService.GetAdministratorLogin();

        public static Contract NoteCompanyContract { get; private set; }
        public static Contract CoinCompanyContract { get; private set; }
        public static Contract ForeignCompanyContract { get; private set; }


        [BeforeFeature(Order = 1)]
        public static void Init()
        {
            if (!BaseDataConfigurationHooks.IsBaseDataConfigured)
            {
                BaseDataConfigurationHooks.ConfigureBaseData();
            }
        }

        [BeforeFeature(Order = 2)]
        public static void ConfigureContracts()
        {
            using (var context = new ContractsDataContext())
            {
                try
                {
                    NoteCompanyContract = context.Contracts.AsNoTracking().FirstOrDefault(c => c.Number == noteContractNumber);
                    var looseProductIdList = new List<int> { BaseDataConfigurationHooks.Eur20LooseProduct.ID, BaseDataConfigurationHooks.Usd20LooseProduct.ID };

                    if (NoteCompanyContract == null)
                    {
                        NoteCompanyContract = DataFacade.Contract.New()
                            .With_IsDefault(false)
                            .With_Number(noteContractNumber)
                            .With_Currency_code("EUR")
                            .With_Customer_id(BaseDataConfigurationHooks.NoteCompany.ID)
                            .With_Date(today)
                            .With_EffectiveDate(today)
                            .With_StartDate(today)
                            .With_EndDate(today.AddDays(30))
                            .With_InterestRate(0)
                            .With_CustomerType(CustomerType.Direct)
                            .With_IsLatestRevision(true)
                            .SaveToDb();

                        ContractsFacade.ContractService.ActivateContract(NoteCompanyContract, new UserParams(login));
                        // Re-configure ordering settings for main service types: COLL, DELV, SERV, REPL
                        // with clearing of all existing ordering settings of custom contract  
                        HelperFacade.ContractHelper.ReconfigureOrderingSettings(NoteCompanyContract.ID);
                        // Re-configure schedule settings for main service types: COLL, DELV, SERV, REPL 
                        // with clearing of all existing schedule settings and schedule lines of custom contract                    
                        HelperFacade.ContractHelper.ReconfigureScheduleSettings(NoteCompanyContract);
                        HelperFacade.ContractHelper.ConfigureOrderingSettingLooseProductLinkForDeliveryServiceType(NoteCompanyContract.ID, looseProductIdList);
                    }
                    else
                    {
                        NoteCompanyContract.EndDate = today.AddDays(30); // this action is applied to always have valid contract
                        context.SaveChanges();
                    }

                    CoinCompanyContract = context.Contracts.AsNoTracking().FirstOrDefault(c => c.Number == coinContractNumber);
                    if (CoinCompanyContract == null)
                    {
                        CoinCompanyContract = DataFacade.Contract.New()
                            .With_IsDefault(false)
                            .With_Number(coinContractNumber)
                            .With_Currency_code("EUR")
                            .With_Customer_id(BaseDataConfigurationHooks.CoinCompany.ID)
                            .With_Date(today)
                            .With_EffectiveDate(today)
                            .With_StartDate(today)
                            .With_EndDate(today.AddDays(30))
                            .With_InterestRate(0)
                            .With_CustomerType(CustomerType.Direct)
                            .With_IsLatestRevision(true)
                            .SaveToDb();

                        ContractsFacade.ContractService.ActivateContract(CoinCompanyContract, new UserParams(login));
                        // Re-configure ordering settings for main service types: COLL, DELV, SERV, REPL
                        // with clearing of all existing ordering settings of custom contract  
                        HelperFacade.ContractHelper.ReconfigureOrderingSettings(CoinCompanyContract.ID);
                        // Re-configure schedule settings for main service types: COLL, DELV, SERV, REPL 
                        // with clearing of all existing schedule settings and schedule lines of custom contract                    
                        HelperFacade.ContractHelper.ReconfigureScheduleSettings(CoinCompanyContract);
                    }
                    else
                    {
                        CoinCompanyContract.EndDate = today.AddDays(30); // this action is applied to always have valid contract
                        context.SaveChanges();
                    }

                    ForeignCompanyContract = context.Contracts.AsNoTracking().FirstOrDefault(c => c.Number == foreignContractNumber);
                    if (ForeignCompanyContract == null)
                    {
                        ForeignCompanyContract = DataFacade.Contract.New()
                            .With_IsDefault(false)
                            .With_Number(foreignContractNumber)
                            .With_Currency_code("EUR")
                            .With_Customer_id(BaseDataConfigurationHooks.ForeignCompany.ID)
                            .With_Date(today)
                            .With_EffectiveDate(today)
                            .With_StartDate(today)
                            .With_EndDate(today.AddDays(30))
                            .With_InterestRate(0)
                            .With_CustomerType(CustomerType.Direct)
                            .With_IsLatestRevision(true)
                            .SaveToDb();

                        ContractsFacade.ContractService.ActivateContract(ForeignCompanyContract, new UserParams(login));
                        // Re-configure ordering settings for main service types: COLL, DELV, SERV, REPL
                        // with clearing of all existing ordering settings of custom contract  
                        HelperFacade.ContractHelper.ReconfigureOrderingSettings(ForeignCompanyContract.ID);

                        // Re-configure schedule settings for main service types: COLL, DELV, SERV, REPL 
                        // with clearing of all existing schedule settings and schedule lines of custom contract                    
                        HelperFacade.ContractHelper.ReconfigureScheduleSettings(ForeignCompanyContract);
                    }
                    else
                    {
                        ForeignCompanyContract.EndDate = today.AddDays(30); // this action is applied to always have valid contract
                        context.SaveChanges();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
        [BeforeFeature(Order = 0)]
        [AfterFeature]
        public static void ClearTestData()
        {
            HelperFacade.BillingHelper.ClearTestData();
            using (var context = new AutomationContractDataContext())
            {
                var priceLinesQuery = context.PriceLines.ToList();
                var priceLineLevelsQuery = context.PriceLineLevels.ToList();
                var priceLineUnitRangesQuery = context.PriceLineUnitsRanges.ToList();                                 
                context.PriceLineUnitsRanges.RemoveRange(priceLineUnitRangesQuery);
                context.PriceLineLevels.RemoveRange(priceLineLevelsQuery);
                context.SaveChanges();
                context.PriceLines.RemoveRange(priceLinesQuery);
                context.SaveChanges();
            }
        }
    }
}
