using Bkc.WebPortal.EP.Sync;
using Cwc.BaseData;
using Cwc.Common;
using Cwc.Localization;
using Cwc.Replication;
using Cwc.Security;
using Cwc.Sync;
using System;
using System.Data.Entity;

namespace CWC.AutoTests.Model
{
    public class AutomationBaseDataContext : BaseDataContext
    {
        public AutomationBaseDataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }

        public AutomationBaseDataContext(DataBaseParams dbParams) : base(dbParams)
        {
        }              
        
        public DbSet<ExportEntitySettings> ExportEntitySettings { get; set; }
        public DbSet<ExportItem> ExportItems { get; set; }
        public DbSet<SyncSettings> SyncSettings { get; set; }
        public DbSet<CustomerBankAccount> CustomerBankAccounts { get; set; }
        public DbSet<ReplicationParty> ReplicationParties { get; set; }

        public new void SaveChanges()
        {
            var result = base.SaveChanges();            
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Unexpected error on saving an entity. Reason: {result.GetMessage()}");
            }
        }
    }
}
