using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.Common;
using System;
using System.Data.Entity;

namespace CWC.AutoTests.Model
{
    public class AutomationCashCenterDataContext : CashCenterDataContext
    {
        public AutomationCashCenterDataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }

        public AutomationCashCenterDataContext(DataBaseParams dbParams) : base(dbParams)
        {
        }        
                
        public DbSet<ContainersBatch> ContainersBatches { get; set; }
        public DbSet<BagTypeMaterialTypeLink> BagTypeMaterialTypeLinks { get; set; }
        public DbSet<PackingLine> PackingLines { get; set; }
        public DbSet<PackingLineWorkstationLink> PackingLineWorkstationLinks { get; set; }
        public DbSet<RawMachineCountResult> RawMachineCountResults { get; set; }
        public DbSet<AutomationCashCenterProcessSettingBagTypeMaterialTypeLink> AutomationCashCenterProcessSettingBagTypeMaterialTypeLinks { get; set; }
        public DbSet<CashCenterProcessSettingMaterialTypeLink> CashCenterProcessSettingMaterialTypeLinks { get; set; }        
        public DbSet<CashCenterSiteSettingMaterialTypeLink> CashCenterSiteSettingMaterialTypeLinks { get; set; }
        public DbSet<CashCenterSiteSettingQualificationType> CashCenterSiteSettingQualificationTypes { get; set; }
        public DbSet<SiteStockHistoryFlowTotal> SiteStockHistoryFlowTotals { get; set; }
        public DbSet<SiteStockMovementHistory> SiteStockMovementHistories { get; set; }
        public DbSet<SiteStockPositionHistory> SiteStockPositionHistories { get; set; }
        public DbSet<SiteStockHistory> SiteStockHistories { get; set; }       
        public DbSet<StockLocationSetting> StockLocationSettings { get; set; }
        public DbSet<CashCenterSiteSettingContainerPrefix> CashCenterSiteSettingContainerPrefixes { get; set; }


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
