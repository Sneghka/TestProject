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

    // WP_CemacsIntegrationSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CemacsIntegrationSetting
    {
        public int id { get; set; } // id (Primary key)
        public string PutFilesFolder { get; set; } // PutFilesFolder (length: 255)
        public System.DateTime DateCreated { get; set; } // DateCreated
        public int User_id { get; set; } // User_id
        public bool IsExportCctvHttp { get; set; } // IsExportCctvHttp
        public bool IsExportCctvFile { get; set; } // IsExportCctvFile

        public WP_CemacsIntegrationSetting()
        {
            IsExportCctvHttp = false;
            IsExportCctvFile = false;
        }
    }

}
// </auto-generated>