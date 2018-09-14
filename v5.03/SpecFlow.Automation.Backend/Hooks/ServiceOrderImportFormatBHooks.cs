using System;
using TechTalk.SpecFlow;
using CWC.AutoTests.Model;
using Cwc.Integration.OrderImportFormatB;
using CWC.AutoTests.ObjectBuilder;
using Cwc.BaseData.Enums;
using Cwc.BaseData;
using Cwc.CashCenter;
using System.Linq;
using System.IO;
using Cwc.Contracts;
using CWC.AutoTests.Helpers;
using Cwc.Security;
using Specflow.Automation.Backend.Helpers;

namespace Specflow.Automation.Backend.Hooks
{
    [Binding, Scope(Tag = "service-order-import-format-b")]
    public class ServiceOrderImportFormatBHooks : IDisposable
    {
        private const string code = "20180205";
        private const string name = "SO";
        private static string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Exchange\\ServiceOrderImportFormatB"));

        public static LocationType LocationType { get; private set; }
        public static StockContainer StockContainer { get; private set; }

        [BeforeFeature(Order = 1)]
        public static void Init()
        {
            if (!BaseDataConfigurationHooks.IsBaseDataConfigured)
            {
                BaseDataConfigurationHooks.ConfigureBaseData();
            }
        }

        [BeforeFeature(Order = 2)]
        public static void ConfigureLocationType()
        {
            using (var context = new AutomationBaseDataContext())
            {
                LocationType = context.LocationTypes.Where(lt => lt.ltCode == code).FirstOrDefault();
                if (LocationType == null)
                {
                    LocationType = DataFacade.LocationType.New().
                       With_ltCode(code).
                       With_ltDesc(name).
                       SaveToDb().Build();
                }
            }
        }

        //[BeforeFeature(Order = 3)]
        //public static void ConfigureContract()
        //{
        //    var login = SecurityFacade.LoginService.GetAdministratorLogin();
        //    using (var context = new AutomationContractDataContext())
        //    {
        //        var CompanyContract = context.Contracts.AsNoTracking().FirstOrDefault(c => c.CustomerID == BaseDataConfigurationHooks.NoteCompany.ID
        //                && c.Currency_code == "EUR"
        //                && c.IsLatestRevision
        //                && c.Status == ContractStatus.Final);

        //        if (CompanyContract == null)
        //        {
        //            CompanyContract = DataFacade.Contract.New()
        //                .With_IsDefault(false)
        //                .With_Number(BaseDataConfigurationHooks.NoteCompany.ID.ToString())
        //                .With_Currency_code("EUR")
        //                .With_Customer_id(BaseDataConfigurationHooks.NoteCompany.ID)
        //                .With_Date(DateTime.Now)
        //                .With_EffectiveDate(DateTime.Now)
        //                .With_StartDate(DateTime.Now)
        //                .With_EndDate(DateTime.Now.AddDays(30))
        //                .With_InterestRate(0)
        //                .With_CustomerType(CustomerType.Direct)
        //                .With_IsLatestRevision(true)
        //                .SaveToDb();

        //            ContractsFacade.ContractService.ActivateContract(CompanyContract, new UserParams(login));
        //        }

        //        HelperFacade.ContractHelper.ReconfigureOrderingSettings(CompanyContract.ID);
        //        HelperFacade.ContractHelper.ReconfigureScheduleSettings(CompanyContract);
        //    }
        //}

        [BeforeFeature(Order = 4)]
        public static void ConfigureJobSettings()
        {
            using (var context = new AutomationOrderImportFormatBDataContext())
            {
                try
                {
                    var entity = ServiceOrderImportFormatBHelper.TakeJobSettings();
                    if (entity == null)
                    {
                        entity = DataFacade.OrderImportFormatBJobSettings.New().
                            With_PostcodesIncomingFileFolder(path).
                            With_IncomingFileFolder(path).
                            With_OutgoingFileFolder(path).
                            With_CompanyID(BaseDataConfigurationHooks.NoteCompany.IdentityID).
                            With_LastFileSequenceNumber(1).
                            With_OrderingDepartmentID(BaseDataConfigurationHooks.OrderingDepartment.ID).
                            With_ServiceTypeID(DataFacade.ServiceType.Take(t => t.Type == TypeOfServiceType.Deliver).Build().ID).
                            With_LocationTypeID(LocationType.ID).
                            SaveToDb().
                            Build();
                    }

                    entity.PostcodesIncomingFileFolder = path;
                    entity.IncomingFileFolder = path;
                    entity.OutgoingFileFolder = path;
                    var saveSettings = OrderImportFormatBFacade.OrderImportFormatBJobSettingsService.Save(entity);
                }
                catch
                {
                    throw;
                }
            }
        }

        [AfterFeature(Order = 1)]
        public static void SaveJobSettings()
        {
            var entity = ServiceOrderImportFormatBHelper.TakeJobSettings();
            var saveSettings = OrderImportFormatBFacade.OrderImportFormatBJobSettingsService.Save(entity);
        }



        public void Dispose()
        {

        }
    }
}
