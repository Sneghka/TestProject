using Cwc.CashPoint;
using Cwc.Coin;
using Cwc.Common;
using System;
using System.Data.Entity;

namespace CWC.AutoTests.Model
{
    public class AutomationCashPointDataContext : CashPointDataContext
    {
        public AutomationCashPointDataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }

        public AutomationCashPointDataContext(DataBaseParams dbParams) : base(dbParams)
        {
        }

        public DbSet<AutomatedOrderCreationSettings> AutomatedOrderCreationSettings { get; set; }

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
