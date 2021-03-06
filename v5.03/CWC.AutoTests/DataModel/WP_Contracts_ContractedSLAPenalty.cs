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

    // WP_Contracts_ContractedSLAPenalty
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Contracts_ContractedSLAPenalty
    {
        public int id { get; set; } // id (Primary key)
        public bool IsLatestRevision { get; set; } // IsLatestRevision
        public int? RevisionNumber { get; set; } // RevisionNumber
        public System.DateTime? RevisionDate { get; set; } // RevisionDate
        public int? ReplacedRevisionNumber { get; set; } // ReplacedRevisionNumber
        public System.DateTime? ReplacedRevisionDate { get; set; } // ReplacedRevisionDate
        public string Comments { get; set; } // Comments
        public int? Author_id { get; set; } // Author_id
        public decimal PricePerCall { get; set; } // PricePerCall
        public int PercentStart { get; set; } // PercentStart
        public int PercentEnd { get; set; } // PercentEnd
        public int ContractLine_id { get; set; } // ContractLine_id
        public int CallCategory { get; set; } // CallCategory
        public int? FailureCode { get; set; } // FailureCode
        public int? LatestRevision_id { get; set; } // LatestRevision_id
        public System.Guid UID { get; set; } // UID

        public WP_Contracts_ContractedSLAPenalty()
        {
            IsLatestRevision = true;
            PricePerCall = 0m;
            PercentStart = 0;
            PercentEnd = 0;
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
