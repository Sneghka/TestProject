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

    // WP_ClieopJobInstanceSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_ClieopJobInstanceSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_ClieopJobInstanceSetting>
    {
        public WP_ClieopJobInstanceSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_ClieopJobInstanceSettingConfiguration(string schema)
        {
            ToTable("WP_ClieopJobInstanceSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.IsBusy).HasColumnName(@"IsBusy").HasColumnType("bit").IsOptional();
            Property(x => x.ExportDate).HasColumnName(@"ExportDate").HasColumnType("datetime").IsRequired();
            Property(x => x.FileNumber).HasColumnName(@"FileNumber").HasColumnType("int").IsRequired();
            Property(x => x.IsExcludeTodayCountResults).HasColumnName(@"IsExcludeTodayCountResults").HasColumnType("bit").IsRequired();
            Property(x => x.ExportStartDate).HasColumnName(@"ExportStartDate").HasColumnType("datetime").IsOptional();
        }
    }

}
// </auto-generated>