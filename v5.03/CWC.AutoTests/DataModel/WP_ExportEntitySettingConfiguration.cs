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

    // WP_ExportEntitySettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_ExportEntitySettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_ExportEntitySetting>
    {
        public WP_ExportEntitySettingConfiguration()
            : this("dbo")
        {
        }

        public WP_ExportEntitySettingConfiguration(string schema)
        {
            ToTable("WP_ExportEntitySettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Alias).HasColumnName(@"Alias").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
            Property(x => x.ExportTables).HasColumnName(@"ExportTables").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
            Property(x => x.IsExportAvaible).HasColumnName(@"IsExportAvaible").HasColumnType("bit").IsRequired();
            Property(x => x.IsExcludeImportedRecords).HasColumnName(@"IsExcludeImportedRecords").HasColumnType("bit").IsRequired();
            Property(x => x.IsHidden).HasColumnName(@"IsHidden").HasColumnType("bit").IsRequired();
        }
    }

}
// </auto-generated>
