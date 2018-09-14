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

    // WP_BaseData_PersonCustomer
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_PersonCustomer
    {
        public int id { get; set; } // id (Primary key)
        public int Type { get; set; } // Type
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime? DateUpdated { get; set; } // DateUpdated
        public decimal CustomerID { get; set; } // CustomerID
        public int AuthorID { get; set; } // AuthorID
        public int? EditorID { get; set; } // EditorID
        public int PersonID { get; set; } // PersonID
        public int? PersonRoleID { get; set; } // PersonRoleID
        public System.Guid UID { get; set; } // UID

        public WP_BaseData_PersonCustomer()
        {
            Type = 0;
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>