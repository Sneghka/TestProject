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

    // WP_BaseData_CustomerMaterialLink
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_CustomerMaterialLink
    {
        public int ID { get; set; } // ID (Primary key)
        public string MaterialCode { get; set; } // MaterialCode (length: 10)
        public decimal CustomerID { get; set; } // CustomerID
        public System.Guid UID { get; set; } // UID

        public WP_BaseData_CustomerMaterialLink()
        {
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
