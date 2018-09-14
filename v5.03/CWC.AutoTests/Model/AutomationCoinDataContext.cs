using Cwc.Coin;
using Cwc.Common;
using System;
using System.Data.Entity;

namespace CWC.AutoTests.Model
{
    public class AutomationCoinDataContext : CoinDataContext
    {
        public AutomationCoinDataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }

        public AutomationCoinDataContext(DataBaseParams dbParams) : base(dbParams)
        {
        }

        public DbSet<MachineModel> MachineModels { get; set; }

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
