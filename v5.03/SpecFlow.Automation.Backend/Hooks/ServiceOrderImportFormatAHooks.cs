using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.Contracts;
using Cwc.Security;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using Specflow.Automation.Backend.Objects;
using System;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;

namespace Specflow.Automation.Backend.Hooks
{
    [Binding, Scope(Tag = "service-order-import-format-a")]
    public class ServiceOrderImportFormatAHooks : IDisposable
    {        
        private const string currencyCode = "EUR";
        private const string usdCurrencyCode = "USD";
        private const string noteType = "NOTE";
        private const string referenceNumber = "3303";
        private static Contract companyContract;        

        [BeforeFeature(Order = 1)]
        public static void InitServiceImportFormatAInterface()
        {
            ServiceOrderImportFormatAContainer.FolderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Exchange\\ServiceOrderImportFormatA"));
            ServiceOrderImportFormatAContainer.FileName = $"BankOrder{DateTime.Today.ToString("yyyyMMdd")}V1.xlsx";

            using (var context = new AutomationOrderImportFormatADataContext())
            {
                var setting = context.OrderImportFormatAJobSettings.First(s => s.ID == 1);
                setting.IncomingFileFolder = ServiceOrderImportFormatAContainer.FolderPath;

                try
                {
                    ServiceOrderImportFormatAContainer.Sar500Product = DataFacade.Product.Take(p =>
                                                                                                    p.Value == 500
                                                                                                    && p.Denomination == 500m
                                                                                                    && p.Currency == currencyCode
                                                                                                    && p.Type == noteType);
                    ServiceOrderImportFormatAContainer.Sar100Product = DataFacade.Product.Take(p =>
                                                                                                    p.Value == 100
                                                                                                    && p.Denomination == 100m
                                                                                                    && p.Currency == currencyCode
                                                                                                    && p.Type == noteType);
                    ServiceOrderImportFormatAContainer.Sar50Product = DataFacade.Product.Take(p =>
                                                                                                    p.Value == 50
                                                                                                    && p.Denomination == 50m
                                                                                                    && p.Currency == currencyCode
                                                                                                    && p.Type == noteType);
                    ServiceOrderImportFormatAContainer.Sar10Product = DataFacade.Product.Take(p =>
                                                                                                    p.Value == 10
                                                                                                    && p.Denomination == 10m
                                                                                                    && p.Currency == currencyCode
                                                                                                    && p.Type == noteType);
                    ServiceOrderImportFormatAContainer.Usd100Product = DataFacade.Product.Take(p =>
                                                                                                    p.Value == 100
                                                                                                    && p.Denomination == 100m
                                                                                                    && p.Currency == usdCurrencyCode
                                                                                                    && p.Type == noteType);
                }
                catch
                {
                    throw new Exception("Error on selecting required product! Please check product configuration in DB!");
                }

                var sar500ProductLink = context.OrderImportFormatAJobProductLink.First(l =>
                                                                                            l.Denomination == 500
                                                                                            && l.CurrencyCode == "SAR");
                sar500ProductLink.ProductID = ServiceOrderImportFormatAContainer.Sar500Product.ID;
                var sar100ProductLink = context.OrderImportFormatAJobProductLink.First(l =>
                                                                                            l.Denomination == 100
                                                                                            && l.CurrencyCode == "SAR");
                sar100ProductLink.ProductID = ServiceOrderImportFormatAContainer.Sar100Product.ID;
                var sar50ProductLink = context.OrderImportFormatAJobProductLink.First(l =>
                                                                                            l.Denomination == 50
                                                                                            && l.CurrencyCode == "SAR");
                sar50ProductLink.ProductID = ServiceOrderImportFormatAContainer.Sar50Product.ID;
                var sar10ProductLink = context.OrderImportFormatAJobProductLink.First(l =>
                                                                                             l.Denomination == 10
                                                                                             && l.CurrencyCode == "SAR");
                sar10ProductLink.ProductID = ServiceOrderImportFormatAContainer.Sar10Product.ID;
                var usd100ProductLink = context.OrderImportFormatAJobProductLink.First(l =>
                                                                                             l.Denomination == 100
                                                                                             && l.CurrencyCode == usdCurrencyCode);
                usd100ProductLink.ProductID = ServiceOrderImportFormatAContainer.Usd100Product.ID;

                context.SaveChanges();
            }
        }

        [BeforeFeature(Order = 2)]
        public static void ConfigureSettings()
        {
            var companyId = DataFacade.Customer.Take(c => c.ReferenceNumber == referenceNumber).Build().ID;

            using (var context = new AutomationContractDataContext())
            {
                companyContract = context.Contracts.AsNoTracking().FirstOrDefault(
                    c => c.CustomerID == companyId
                    && c.Currency_code == currencyCode
                    && c.IsLatestRevision
                    && c.Status == ContractStatus.Final);

                if (companyContract == null)
                {
                    companyContract = DataFacade.Contract.New()
                        .With_IsDefault(false)
                        .With_Number(referenceNumber)
                        .With_Currency_code(currencyCode)
                        .With_Customer_id(companyId)
                        .With_Date(DateTime.Now)
                        .With_EffectiveDate(DateTime.Now)
                        .With_StartDate(DateTime.Now)
                        .With_EndDate(DateTime.Now.AddDays(30))
                        .With_InterestRate(0)
                        .With_CustomerType(CustomerType.Direct)
                        .With_IsLatestRevision(true)
                        .SaveToDb();

                    ContractsFacade.ContractService.ActivateContract(companyContract, new UserParams(SecurityFacade.LoginService.GetAdministratorLogin()));
                }                    
                else
                {
                    companyContract.EndDate = DateTime.Now.AddDays(30);
                    context.SaveChanges();
                }

                HelperFacade.ContractHelper.ReconfigureOrderingSettings(companyContract.ID);
                HelperFacade.ContractHelper.ReconfigureScheduleSettings(companyContract);
            }                            
        }

        [BeforeFeature(Order = 3)]
        public static void LoadConfigurationKeySet()
        {
            ConfigurationKeySet.Load();
        }
        

        [BeforeScenario]        
        public void ClearTestData()
        {
            HelperFacade.TransportHelper.ClearTestData();
        }
        
        public void Dispose()
        {
            HelperFacade.TransportHelper.ClearTestData();
        }
    }
}    


