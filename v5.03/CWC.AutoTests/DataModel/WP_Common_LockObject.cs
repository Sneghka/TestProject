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

    // WP_Common_LockObject
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Common_LockObject
    {
        public long Object_id { get; set; } // Object_id (Primary key via unique index IDX_WP_Common_LockObject_ObjectID_ClassID)
        public int Class_id { get; set; } // Class_id (Primary key via unique index IDX_WP_Common_LockObject_ObjectID_ClassID)
        public int User_id { get; set; } // User_id
        public string Session_id { get; set; } // Session_id
        public System.DateTime LockDateTime { get; set; } // LockDateTime
    }

}
// </auto-generated>