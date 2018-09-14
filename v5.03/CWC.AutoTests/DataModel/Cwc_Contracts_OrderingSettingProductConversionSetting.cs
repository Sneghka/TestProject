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

    // Cwc_Contracts_OrderingSettingProductConversionSetting
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Contracts_OrderingSettingProductConversionSetting
    {
        public int id { get; set; } // id (Primary key)
        public int OrderedProductID { get; set; } // OrderedProductID
        public int ConvertedProductID { get; set; } // ConvertedProductID
        public int ContractOrderingSettingID { get; set; } // ContractOrderingSettingID
        public bool IsLatestRevision { get; set; } // IsLatestRevision
        public int? RevisionNumber { get; set; } // RevisionNumber
        public System.DateTime? RevisionDate { get; set; } // RevisionDate
        public int? ReplacedRevisionNumber { get; set; } // ReplacedRevisionNumber
        public System.DateTime? ReplacedRevisionDate { get; set; } // ReplacedRevisionDate
        public int? AuthorID { get; set; } // AuthorID
        public int? LatestRevisionID { get; set; } // LatestRevisionID

        public Cwc_Contracts_OrderingSettingProductConversionSetting()
        {
            IsLatestRevision = true;
        }
    }

}
// </auto-generated>