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

    // WP_Contracts_ScheduleSetting
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Contracts_ScheduleSetting
    {
        public int id { get; set; } // id (Primary key)
        public bool IsLatestRevision { get; set; } // IsLatestRevision
        public int? RevisionNumber { get; set; } // RevisionNumber
        public System.DateTime? RevisionDate { get; set; } // RevisionDate
        public int? ReplacedRevisionNumber { get; set; } // ReplacedRevisionNumber
        public System.DateTime? ReplacedRevisionDate { get; set; } // ReplacedRevisionDate
        public string Comments { get; set; } // Comments
        public int? Author_id { get; set; } // Author_id
        public int? LatestRevision_id { get; set; } // LatestRevision_id
        public decimal? Location_id { get; set; } // Location_id
        public int Contract_id { get; set; } // Contract_id
        public System.Guid UID { get; set; } // UID
        public int? ServiceTypeID { get; set; } // ServiceTypeID
        public System.DateTime PeriodStartDate { get; set; } // PeriodStartDate

        public WP_Contracts_ScheduleSetting()
        {
            IsLatestRevision = true;
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
