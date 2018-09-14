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

    // Cwc_Contracts_PriceLineLevel
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Contracts_PriceLineLevel
    {
        public int id { get; set; } // id (Primary key)
        public bool IsLatestRevision { get; set; } // IsLatestRevision
        public int? RevisionNumber { get; set; } // RevisionNumber
        public System.DateTime? RevisionDate { get; set; } // RevisionDate
        public int? ReplacedRevisionNumber { get; set; } // ReplacedRevisionNumber
        public System.DateTime? ReplacedRevisionDate { get; set; } // ReplacedRevisionDate
        public int? AuthorID { get; set; } // AuthorID
        public int LevelName { get; set; } // LevelName
        public bool IsRangeLevel { get; set; } // IsRangeLevel
        public int LevelValueType { get; set; } // LevelValueType
        public int SequenceNumber { get; set; } // SequenceNumber
        public int PriceLineID { get; set; } // PriceLineID
        public int? LatestRevisionID { get; set; } // LatestRevisionID
        public System.Guid UID { get; set; } // UID
        public decimal? Value { get; set; } // Value
        public decimal? ValueFrom { get; set; } // ValueFrom
        public decimal? ValueTo { get; set; } // ValueTo

        public Cwc_Contracts_PriceLineLevel()
        {
            IsLatestRevision = true;
            IsRangeLevel = false;
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
