using Cwc.BaseData;
using Cwc.Billing;
using Cwc.Billing.Model;
using Cwc.Billing.Services.Impl;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;

namespace CWC.AutoTests.Helpers
{
    public class PreannauncementAndBillingHelper: BasicFileWorkHelper
    {
        PreannouncementBillingExportSetting settings = null;
         
        public PreannauncementAndBillingHelper()
        {

        }

        private PreannouncementBillingExportSetting FindPreannauncementBillingSettings()
        {
            using (var context = new AutomationBillingContext())
            {
                return BillingFacade.PreannouncementBillingExportSettingService.Load(context);                
            }            
        }

        private void SavePreannauncementBillingSettings(PreannouncementBillingExportSetting settings)
        {
            using (var context = new AutomationBillingContext())
            {
                BillingFacade.PreannouncementBillingExportSettingService.Save(settings);
            }
        }
        public void ConfigureFolderForPreannauncementSettings(string preannauncementXMLPath)
        {
            settings = FindPreannauncementBillingSettings();
            settings.ExportPreannouncementsFolder = preannauncementXMLPath;
            SavePreannauncementBillingSettings(settings);
        }
        public void ConfigureFolderForBillingSettings(string billingXMLPath)
        {
            settings = FindPreannauncementBillingSettings();
            settings.ExportBillingLinesXmlFolder = billingXMLPath;
            SavePreannauncementBillingSettings(settings);
        }

        public PreannouncementBillingExportSettingGroup CreateNewCompanyGroup(string groupName)
        {
            return DataFacade.PreannouncementBillingExportSettingGroup.New().
                With_LastSequenceNumberBilling(0).
                With_LastSequenceNumberPreann(0).
                With_GroupName(groupName).
                SaveToDb();
        }

        public void AddCompanyToGroup(int companyIdentityID, int groupId)
        {
            DataFacade.PreannouncementBillingExportSettingGroupCompanyLink.New().
                With_GroupID(groupId).
                With_CompanyID(companyIdentityID).SaveToDb();
        }

        public void RunExportBillingPreannouncementsJob()
        {
            using (var context = new AutomationBillingContext())
            {
                settings = FindPreannauncementBillingSettings();
                try
                {
                    new PreannouncementBillingExportJobService().ExportBillingPreannouncementsXml(false, settings, context);
                }
                catch
                {
                    throw;
                }
            }
        }


    }
}
