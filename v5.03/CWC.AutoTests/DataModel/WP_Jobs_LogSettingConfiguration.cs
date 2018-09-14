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

    // WP_Jobs_LogSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Jobs_LogSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Jobs_LogSetting>
    {
        public WP_Jobs_LogSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_Jobs_LogSettingConfiguration(string schema)
        {
            ToTable("WP_Jobs_LogSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
            Property(x => x.Size).HasColumnName(@"Size").HasColumnType("int").IsRequired();
            Property(x => x.Default).HasColumnName(@"Default").HasColumnType("bit").IsRequired();
            Property(x => x.Inherited).HasColumnName(@"Inherited").HasColumnType("bit").IsRequired();
            Property(x => x.ClearLogType).HasColumnName(@"ClearLogType").HasColumnType("int").IsRequired();
            Property(x => x.ClearDays).HasColumnName(@"ClearDays").HasColumnType("int").IsOptional();
            Property(x => x.TopLevelSettingsID).HasColumnName(@"TopLevelSettingsID").HasColumnType("int").IsOptional();
            Property(x => x.LogManager).HasColumnName(@"LogManager").HasColumnType("nvarchar").IsOptional().HasMaxLength(1000);
            Property(x => x.UserClearDateTime).HasColumnName(@"UserClearDateTime").HasColumnType("datetime").IsOptional();
            Property(x => x.JobId).HasColumnName(@"JobId").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>