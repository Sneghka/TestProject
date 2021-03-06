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

    // WP_ContractedProduct
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_ContractedProduct
    {
        public string ProductCode { get; set; } // ProductCode (Primary key) (length: 50)
        public int OrderingSettingsId { get; set; } // OrderingSettingsId (Primary key)
        public int? MaxQty { get; set; } // MaxQty
        public int? PredefinedQty { get; set; } // PredefinedQty
        public int AuthorId { get; set; } // AuthorId
        public int EditorId { get; set; } // EditorId
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime DateUpdated { get; set; } // DateUpdated
    }

}
// </auto-generated>
