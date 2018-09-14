using System;

namespace Specflow.Automation.Backend.Model
{
    public class ServiceOrderImportFormatBLeadingRecord
    {
        public int RecordType { get; set; }
        public int SequenceNumberOfFile { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string BankIdentification { get; set; }
    }
}
