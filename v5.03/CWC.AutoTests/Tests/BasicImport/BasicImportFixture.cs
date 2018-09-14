using CWC.AutoTests.Model;
using System.IO;
using System.Linq;

namespace CWC.AutoTests.Tests.BasicImport
{
    [Xunit.CollectionDefinition("BasicImport")]
    public class BasicImportFixture
    {
        public string FolderPath { get; set; }

        public BasicImportFixture()
        {
            this.FolderPath = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Exchange\\Incoming"));
            Cwc.BaseData.Classes.ConfigurationKeySet.Load();
            Cwc.Sync.SyncConfiguration.LoadExportMappings();

            using (var context = new AutomationBaseDataContext())
            {
                var setting = context.SyncSettings.First(s => s.ID == 1);
                setting.ExchangeFolder = this.FolderPath;
                context.SaveChanges();
            }
        }        
    }
}
