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
    public class WP_IntegrationMasterDataImport_IntegrationJobSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_IntegrationMasterDataImport_IntegrationJobSetting>
    {
        public WP_IntegrationMasterDataImport_IntegrationJobSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_IntegrationMasterDataImport_IntegrationJobSettingConfiguration(string schema)
        {
            ToTable("WP_IntegrationMasterDataImport_IntegrationJobSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.FolderToGetFiles).HasColumnName(@"FolderToGetFiles").HasColumnType("nvarchar").IsRequired().HasMaxLength(2000);
            Property(x => x.ServiceTypeID).HasColumnName(@"ServiceTypeID").HasColumnType("int").IsRequired();
        }
    }

}
// </auto-generated>