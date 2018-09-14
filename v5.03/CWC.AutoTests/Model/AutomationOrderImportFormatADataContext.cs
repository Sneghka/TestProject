using Cwc.Common;
using Cwc.Integration.OrderImportFormatA;
using System;

namespace CWC.AutoTests.Model
{
    public class AutomationOrderImportFormatADataContext : OrderImportFormatADataContext
    {
        public AutomationOrderImportFormatADataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }

        public AutomationOrderImportFormatADataContext(DataBaseParams dbParams) : base(dbParams)
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
