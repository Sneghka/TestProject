// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.6
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace CWC.AutoTests.DataModel
{

    // Cwc_Billing_BillingExportSetting
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Billing_BillingExportSetting
    {
        public int id { get; set; } // id (Primary key)
        public bool IsExportBillingLinesXml { get; set; } // IsExportBillingLinesXml
        public System.TimeSpan ExportBillingLinesXmlTime { get; set; } // ExportBillingLinesXmlTime
        public int ExportBillingLinesXmlDay { get; set; } // ExportBillingLinesXmlDay
        public string ExportBillingLinesXmlFolder { get; set; } // ExportBillingLinesXmlFolder (length: 255)
        public System.DateTime? LastExecutionDateTime { get; set; } // LastExecutionDateTime

        public Cwc_Billing_BillingExportSetting()
        {
            IsExportBillingLinesXml = false;
            ExportBillingLinesXmlDay = 1;
        }
    }

}
// </auto-generated>
