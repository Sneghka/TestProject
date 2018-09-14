using System.Collections.Generic;

namespace Specflow.Automation.Backend.Model
{
    public class ServiceOrderImportFormaBContainer
    {
        public ServiceOrderImportFormatBLeadingRecord LeadingRecord { get; set; }
        public ServiceOrderImportFormatBOrderRecord OrderRecord { get; set; }
        public List<ServiceOrderImportFormatBOrderItemRecord> OrderItemRecord { get; set; }
        public ServiceOrderImportFormatBOrderedDeliveryInfo OrderedDeliveryInfoRecord { get; set; }
        public ServiceOrderImportFormatBCloseRecord CloseRecord { get; set; }
        
        public ServiceOrderImportFormaBContainer()
        {
            LeadingRecord = new ServiceOrderImportFormatBLeadingRecord();
            OrderRecord = new ServiceOrderImportFormatBOrderRecord();
            OrderItemRecord = new List<ServiceOrderImportFormatBOrderItemRecord>();
            OrderedDeliveryInfoRecord = new ServiceOrderImportFormatBOrderedDeliveryInfo();
            CloseRecord = new ServiceOrderImportFormatBCloseRecord();
        }

    }
}
