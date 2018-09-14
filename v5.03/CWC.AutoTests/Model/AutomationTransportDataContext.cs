using Cwc.Common;
using Cwc.Transport;
using System;

namespace CWC.AutoTests.Model
{
    public class AutomationTransportDataContext : TransportDataContext
    {
        public AutomationTransportDataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }

        public AutomationTransportDataContext(DataBaseParams dbParams) : base(dbParams)
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
