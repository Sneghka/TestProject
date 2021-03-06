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

    // Cwc_Billing_BillingExportLog
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Billing_BillingExportLog
    {
        public int id { get; set; } // id (Primary key)
        public int Result { get; set; } // Result
        public string Message { get; set; } // Message
        public int GenerationMethod { get; set; } // GenerationMethod
        public string ResultingFileName { get; set; } // ResultingFileName (length: 255)
        public System.DateTime? ExportDateFrom { get; set; } // ExportDateFrom
        public System.DateTime? ExportDateTo { get; set; } // ExportDateTo
        public System.DateTime DateCreated { get; set; } // DateCreated

        public Cwc_Billing_BillingExportLog()
        {
            Result = 0;
            GenerationMethod = 0;
            DateCreated = System.DateTime.Now;
        }
    }

}
// </auto-generated>
