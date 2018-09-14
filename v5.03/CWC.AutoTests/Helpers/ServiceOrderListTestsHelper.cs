using CWC.AutoTests.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.Helpers
{
    public class ServiceOrderListTestsHelper
    {
        ModelContext _context;
        public ServiceOrderListTestsHelper()
        {
            _context = new ModelContext();
        }

        public string GetValidatedFeedingLogMessage(string refNum)
        {

            var foundMessage = string.Empty;
            try
            {
                foundMessage = _context.Cwc_Feedings_ValidatedFeedingLogs.OrderByDescending(f => f.DateCreated).FirstOrDefault(fm => fm.Message.Contains(refNum)).Message.Trim();
            }
            catch
            {
                throw new Exception($"Validated Feeding log for Service Order with Number '{refNum}' is not found");
            }

            return foundMessage.Replace(Environment.NewLine, "");
        }
    }
}
