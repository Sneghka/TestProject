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

    // WP_Jobs_NotificationJobSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Jobs_NotificationJobSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Jobs_NotificationJobSetting>
    {
        public WP_Jobs_NotificationJobSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_Jobs_NotificationJobSettingConfiguration(string schema)
        {
            ToTable("WP_Jobs_NotificationJobSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Timeout).HasColumnName(@"Timeout").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
