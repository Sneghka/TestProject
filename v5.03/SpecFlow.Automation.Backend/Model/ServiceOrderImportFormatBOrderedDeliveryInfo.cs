using System;

namespace Specflow.Automation.Backend.Model
{
    public class ServiceOrderImportFormatBOrderedDeliveryInfo
    {
        public int RecordType { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public int PhoneNumber { get; set; } 
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string HouseNumberAddition { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
    }
}
