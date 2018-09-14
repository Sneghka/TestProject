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

    // WP_IntegrationMasterDataImport_LocationSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_IntegrationMasterDataImport_LocationSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_IntegrationMasterDataImport_LocationSetting>
    {
        public WP_IntegrationMasterDataImport_LocationSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_IntegrationMasterDataImport_LocationSettingConfiguration(string schema)
        {
            ToTable("WP_IntegrationMasterDataImport_LocationSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ExternalLocationType).HasColumnName(@"ExternalLocationType").HasColumnType("nvarchar").IsRequired().HasMaxLength(2000);
            Property(x => x.LocationTypeID).HasColumnName(@"LocationTypeID").HasColumnType("int").IsRequired();
            Property(x => x.LocationHandlingTypeCode).HasColumnName(@"LocationHandlingTypeCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(6);
            Property(x => x.JobSettingsID).HasColumnName(@"JobSettingsID").HasColumnType("int").IsRequired();
        }
    }

}
// </auto-generated>