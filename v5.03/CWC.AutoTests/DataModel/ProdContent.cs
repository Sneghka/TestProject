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

    // ProdContent
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class ProdContent
    {
        public string ProductCode { get; set; } // ProductCode (length: 10)
        public string materialID { get; set; } // materialID (length: 10)
        public int? numItems { get; set; } // numItems
        public System.Guid UID { get; set; } // UID
        public int id { get; set; } // id (Primary key)

        public ProdContent()
        {
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>