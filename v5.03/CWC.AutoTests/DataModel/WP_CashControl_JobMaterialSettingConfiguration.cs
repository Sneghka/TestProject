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

    // WP_CashControl_JobMaterialSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashControl_JobMaterialSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CashControl_JobMaterialSetting>
    {
        public WP_CashControl_JobMaterialSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_CashControl_JobMaterialSettingConfiguration(string schema)
        {
            ToTable("WP_CashControl_JobMaterialSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Denomination).HasColumnName(@"Denomination").HasColumnType("decimal").IsRequired().HasPrecision(26,8);
            Property(x => x.MaterialID).HasColumnName(@"MaterialID").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
            Property(x => x.CurrencyCode).HasColumnName(@"CurrencyCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(3);
        }
    }

}
// </auto-generated>
