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

    // WP_BaseData_CustomerContactPersonLink
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_CustomerContactPersonLink
    {
        public int id { get; set; } // id (Primary key)
        public decimal CustomerID { get; set; } // CustomerID
        public int ContactPersonID { get; set; } // ContactPersonID
        public bool IsPrimary { get; set; } // IsPrimary
        public System.Guid UID { get; set; } // UID

        public WP_BaseData_CustomerContactPersonLink()
        {
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
