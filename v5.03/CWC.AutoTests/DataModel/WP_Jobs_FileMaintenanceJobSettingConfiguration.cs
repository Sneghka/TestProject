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

    // WP_Jobs_FileMaintenanceJobSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Jobs_FileMaintenanceJobSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Jobs_FileMaintenanceJobSetting>
    {
        public WP_Jobs_FileMaintenanceJobSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_Jobs_FileMaintenanceJobSettingConfiguration(string schema)
        {
            ToTable("WP_Jobs_FileMaintenanceJobSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Days).HasColumnName(@"Days").HasColumnType("int").IsRequired();
        }
    }

}
// </auto-generated>