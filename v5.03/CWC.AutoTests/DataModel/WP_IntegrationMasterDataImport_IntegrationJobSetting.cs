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

    // WP_IntegrationMasterDataImport_IntegrationJobSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_IntegrationMasterDataImport_IntegrationJobSetting
    {
        public int id { get; set; } // id (Primary key)
        public string FolderToGetFiles { get; set; } // FolderToGetFiles (length: 2000)
        public int ServiceTypeID { get; set; } // ServiceTypeID

        public WP_IntegrationMasterDataImport_IntegrationJobSetting()
        {
            ServiceTypeID = -1;
        }
    }

}
// </auto-generated>
