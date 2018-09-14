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

    // WP_Group
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Group
    {
        public int id { get; set; } // id (Primary key)
        public string Name { get; set; } // Name (length: 255)
        public int Status { get; set; } // Status
        public string Comments { get; set; } // Comments (length: 255)
        public int Type { get; set; } // Type
        public int DepartmentType { get; set; } // DepartmentType
        public string ContactName { get; set; } // ContactName (length: 255)
        public string ContactEmail { get; set; } // ContactEmail (length: 255)
        public string ContactPhoneNumber { get; set; } // ContactPhoneNumber (length: 255)
        public string ContactFaxNumber { get; set; } // ContactFaxNumber (length: 255)
        public decimal? DepartmentsLocationID { get; set; } // DepartmentsLocationID
        public System.Guid UID { get; set; } // UID

        public WP_Group()
        {
            Type = 0;
            DepartmentType = 0;
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
