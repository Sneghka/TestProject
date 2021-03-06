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

    // WP_Archiving_GraphUserSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Archiving_GraphUserSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Archiving_GraphUserSetting>
    {
        public WP_Archiving_GraphUserSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_Archiving_GraphUserSettingConfiguration(string schema)
        {
            ToTable("WP_Archiving_GraphUserSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("int").IsRequired();
            Property(x => x.AggregationType).HasColumnName(@"AggregationType").HasColumnType("int").IsRequired();
            Property(x => x.OperationType).HasColumnName(@"OperationType").HasColumnType("int").IsRequired();
            Property(x => x.StartPeriod).HasColumnName(@"StartPeriod").HasColumnType("datetime").IsOptional();
            Property(x => x.EndPeriod).HasColumnName(@"EndPeriod").HasColumnType("datetime").IsOptional();
            Property(x => x.Interval).HasColumnName(@"Interval").HasColumnType("int").IsOptional();
            Property(x => x.IntervalType).HasColumnName(@"IntervalType").HasColumnType("int").IsOptional();
            Property(x => x.PeriodType).HasColumnName(@"PeriodType").HasColumnType("int").IsOptional();
            Property(x => x.ArchivingLevel).HasColumnName(@"ArchivingLevel").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
