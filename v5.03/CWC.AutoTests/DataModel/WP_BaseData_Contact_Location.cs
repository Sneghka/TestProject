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

    // WP_BaseData_Contact_Location
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_Contact_Location
    {
        public int id { get; set; } // id (Primary key)
        public int Contact_id { get; set; } // Contact_id
        public string Location_Code { get; set; } // Location_Code (length: 16)
        public System.Guid UID { get; set; } // UID

        public WP_BaseData_Contact_Location()
        {
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
