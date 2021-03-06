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

    // WP_CM_OfflineTimeHistory
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CM_OfflineTimeHistory
    {
        public int id { get; set; } // id (Primary key)
        public double TimeOffline { get; set; } // TimeOffline
        public double TimeOpen { get; set; } // TimeOpen
        public System.DateTime Date { get; set; } // Date
        public int? CategoryID { get; set; } // CategoryID
        public int MachineID { get; set; } // MachineID
        public double? TimeCorrection { get; set; } // TimeCorrection
        public bool IsTotal { get; set; } // IsTotal
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime DateUpdated { get; set; } // DateUpdated
        public int? StatusCodeId { get; set; } // StatusCodeId
        public int Errors { get; set; } // Errors

        public WP_CM_OfflineTimeHistory()
        {
            TimeOffline = 0;
            TimeOpen = 0;
            Errors = 0;
        }
    }

}
// </auto-generated>
