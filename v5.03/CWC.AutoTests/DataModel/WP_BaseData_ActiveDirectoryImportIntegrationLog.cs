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

    // WP_BaseData_ActiveDirectoryImportIntegrationLog
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_ActiveDirectoryImportIntegrationLog
    {
        public int id { get; set; } // id (Primary key)
        public System.DateTime DateCreated { get; set; } // DateCreated
        public int Result { get; set; } // Result
        public string Message { get; set; } // Message
        public string Path { get; set; } // Path (length: 255)
        public string Domain { get; set; } // Domain (length: 100)
        public string ADGroup { get; set; } // ADGroup (length: 255)
        public string CWCGroup { get; set; } // CWCGroup (length: 255)
    }

}
// </auto-generated>
