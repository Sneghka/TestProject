using Cwc.Common;
using Cwc.Jobs;
using Cwc.MasterDataImport;
using CWC.AutoTests.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.ObjectBuilder.MasterDataImportEntities
{
    public class MasterDataImportService : MasterDataImportIntegrationJob
    {

        DataBaseParams par;
        public MasterDataImportService()
        {

            par = new DataBaseParams();
        }
        public void Go()
        {
            var set = MasterDataImportIntegrationFacade.SettingsService.LoadSettings(par);

            set.FolderToGetFiles = @"D:\MasterDataImport";
            
            //var jobInfo = JobsFacade.JobInfoService.LoadAll(par).Where(j => j.Name.Contains("Master Data")).FirstOrDefault();
            //var settings = this.LoadSettings(JobsFacade.JobInstanceService.LoadAll(par).Where(j => j.JobInfo_id == jobInfo.ID).FirstOrDefault(), par).FirstOrDefault();
            //settings.ExchangeFolder = @"D:\MasterDataImport";
            this.Step();
            

        }
        private new void Step()
        {
            //base.Start();
            base.Step();
        }
        public static string GetString()
        {
            return AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }
    }
}
