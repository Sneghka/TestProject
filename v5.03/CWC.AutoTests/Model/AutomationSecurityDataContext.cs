using Cwc.Common;
using Cwc.Security;
using System;

namespace CWC.AutoTests.Model
{
    public class AutomationSecurityDataContext: SecurityDataContext
    {
        public AutomationSecurityDataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }

        public AutomationSecurityDataContext(DataBaseParams dbParams) : base(dbParams)
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
