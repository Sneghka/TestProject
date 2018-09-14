using Cwc.Billing;
using Cwc.Common;
using System;

namespace CWC.AutoTests.Model
{
    public class AutomationBillingContext : BillingContext
    {
        public AutomationBillingContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }

        public AutomationBillingContext(DataBaseParams dbParams) : base(dbParams)
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

