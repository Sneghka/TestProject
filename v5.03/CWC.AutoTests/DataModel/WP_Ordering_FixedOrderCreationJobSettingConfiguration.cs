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

    // WP_Ordering_FixedOrderCreationJobSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Ordering_FixedOrderCreationJobSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Ordering_FixedOrderCreationJobSetting>
    {
        public WP_Ordering_FixedOrderCreationJobSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_Ordering_FixedOrderCreationJobSettingConfiguration(string schema)
        {
            ToTable("WP_Ordering_FixedOrderCreationJobSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.EnabledConfirmation).HasColumnName(@"EnabledConfirmation").HasColumnType("bit").IsRequired();
            Property(x => x.JobDailyAtTime).HasColumnName(@"JobDailyAtTime").HasColumnType("float").IsRequired();
            Property(x => x.JobLastExecutionDate).HasColumnName(@"JobLastExecutionDate").HasColumnType("datetime").IsOptional();
        }
    }

}
// </auto-generated>
