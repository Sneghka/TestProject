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

    // WP_CashCenter_SiteProcessSetting_MaterialType_Link
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_SiteProcessSetting_MaterialType_LinkConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CashCenter_SiteProcessSetting_MaterialType_Link>
    {
        public WP_CashCenter_SiteProcessSetting_MaterialType_LinkConfiguration()
            : this("dbo")
        {
        }

        public WP_CashCenter_SiteProcessSetting_MaterialType_LinkConfiguration(string schema)
        {
            ToTable("WP_CashCenter_SiteProcessSetting_MaterialType_Link", schema);
            HasKey(x => x.Id);

            Property(x => x.MaterialTypeCode).HasColumnName(@"MaterialTypeCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.IsCaptureValue).HasColumnName(@"IsCaptureValue").HasColumnType("bit").IsRequired();
            Property(x => x.IsCaptureQuantity).HasColumnName(@"IsCaptureQuantity").HasColumnType("bit").IsRequired();
            Property(x => x.IsCaptureWeight).HasColumnName(@"IsCaptureWeight").HasColumnType("bit").IsRequired();
            Property(x => x.ErrorMarginValue).HasColumnName(@"ErrorMarginValue").HasColumnType("decimal").IsRequired().HasPrecision(15,2);
            Property(x => x.ErrorMarginQuantity).HasColumnName(@"ErrorMarginQuantity").HasColumnType("int").IsRequired();
            Property(x => x.ErrorMarginWeight).HasColumnName(@"ErrorMarginWeight").HasColumnType("decimal").IsRequired().HasPrecision(15,5);
            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.CashCenterProcessSettingID).HasColumnName(@"CashCenterProcessSettingID").HasColumnType("int").IsRequired();
            Property(x => x.SubProcess).HasColumnName(@"SubProcess").HasColumnType("int").IsRequired();
            Property(x => x.ErrorMarginPercent).HasColumnName(@"ErrorMarginPercent").HasColumnType("decimal").IsRequired().HasPrecision(4,2);
        }
    }

}
// </auto-generated>
