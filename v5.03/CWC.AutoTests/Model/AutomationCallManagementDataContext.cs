using Cwc.CallManagement;
using Cwc.Common;
using System;
using System.Data.Entity;

namespace CWC.AutoTests.Model
{
    public class AutomationCallManagementDataContext : CallManagementDataContext
    {
        public AutomationCallManagementDataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }

        public AutomationCallManagementDataContext(DataBaseParams dbParams) : base(dbParams)
        {
        }

        public DbSet<SolutionCode> SolutionCodes { get; set; }

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
