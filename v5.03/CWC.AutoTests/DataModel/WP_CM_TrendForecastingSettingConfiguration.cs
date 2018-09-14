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

    // WP_CM_TrendForecastingSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CM_TrendForecastingSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CM_TrendForecastingSetting>
    {
        public WP_CM_TrendForecastingSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_CM_TrendForecastingSettingConfiguration(string schema)
        {
            ToTable("WP_CM_TrendForecastingSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.DeterminationMinimum).HasColumnName(@"DeterminationMinimum").HasColumnType("decimal").IsRequired().HasPrecision(26,8);
            Property(x => x.MaximumTrends).HasColumnName(@"MaximumTrends").HasColumnType("int").IsRequired();
            Property(x => x.SeriesLength).HasColumnName(@"SeriesLength").HasColumnType("int").IsRequired();
            Property(x => x.DeviationMinimum).HasColumnName(@"DeviationMinimum").HasColumnType("int").IsRequired();
            Property(x => x.TrendFactorApplication).HasColumnName(@"TrendFactorApplication").HasColumnType("int").IsRequired();
        }
    }

}
// </auto-generated>
