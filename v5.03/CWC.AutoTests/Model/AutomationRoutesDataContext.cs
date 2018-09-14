using Cwc.Common;
using Cwc.Routes;
using System;

namespace CWC.AutoTests.Model
{
    public class AutomationRoutesDataContext : RoutesDataContext
    {
        public AutomationRoutesDataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }

        public AutomationRoutesDataContext(DataBaseParams dbParams) : base(dbParams)
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
