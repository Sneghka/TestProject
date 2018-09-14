using Cwc.Common;
using Cwc.Contracts;
using System;

namespace CWC.AutoTests.Model
{
    public class AutomationContractDataContext : ContractsDataContext
    {
        public AutomationContractDataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }

        public AutomationContractDataContext(DataBaseParams dbParams) : base(dbParams)
        {
        }

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