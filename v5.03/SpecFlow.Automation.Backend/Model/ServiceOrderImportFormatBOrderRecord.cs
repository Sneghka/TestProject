namespace Specflow.Automation.Backend.Model
{
    public class ServiceOrderImportFormatBOrderRecord
    {
        public int RecordType { get; set; }
        public string AddressCode { get; set; }
        public string AccountNumber { get; set; }
        public string DeliveryDate { get; set; }
        public string BagType { get; set; }
        public string Reference { get; set; }
        public string BankReference { get; set; }
    }
}
