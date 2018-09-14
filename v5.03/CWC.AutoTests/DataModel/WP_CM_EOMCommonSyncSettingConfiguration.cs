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

    // WP_CM_EOMCommonSyncSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CM_EOMCommonSyncSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CM_EOMCommonSyncSetting>
    {
        public WP_CM_EOMCommonSyncSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_CM_EOMCommonSyncSettingConfiguration(string schema)
        {
            ToTable("WP_CM_EOMCommonSyncSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.FolderToGetFiles).HasColumnName(@"FolderToGetFiles").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.FileKind).HasColumnName(@"FileKind").HasColumnType("nvarchar").IsOptional().HasMaxLength(4);
            Property(x => x.FolderToGetBoekBestandFiles).HasColumnName(@"FolderToGetBoekBestandFiles").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.ResidualCode).HasColumnName(@"ResidualCode").HasColumnType("int").IsRequired();
            Property(x => x.RejectsCode).HasColumnName(@"RejectsCode").HasColumnType("int").IsRequired();
            Property(x => x.RetractsCode).HasColumnName(@"RetractsCode").HasColumnType("int").IsRequired();
            Property(x => x.FolderToGetFallbackOrdersFiles).HasColumnName(@"FolderToGetFallbackOrdersFiles").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.FallbackOrdersFileKind).HasColumnName(@"FallbackOrdersFileKind").HasColumnType("nvarchar").IsOptional().HasMaxLength(4);
        }
    }

}
// </auto-generated>
