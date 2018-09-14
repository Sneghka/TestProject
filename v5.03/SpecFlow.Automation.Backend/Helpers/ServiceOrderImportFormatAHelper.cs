using Cwc.Integration.OrderImportFormatA;
using Cwc.Integration.OrderImportFormatA.Model;

namespace Specflow.Automation.Backend.Helpers
{
    public class ServiceOrderImportFormatAHelper
    {
        public ServiceOrderImportFormatAHelper()
        {            
        }

        public void RunOrderImportFormatAJob()
        {
            OrderImportFormatAJobSettings settings = null;
            using (var context = new OrderImportFormatADataContext())
            {
                settings = OrderImportFormatAFacade.OrderImportFormatAJobSettingsService.LoadOrderImportFormatAJobSettings(context);                
            }
            
            var result = OrderImportFormatAFacade.OrderImportFormatAJobService.ProcessOrderImportFormatAFiles(settings);
        }
    }
}

