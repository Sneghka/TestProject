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

    // WP_CashCenter_StockLocationSettingMaterialTypeLink
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_StockLocationSettingMaterialTypeLinkConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CashCenter_StockLocationSettingMaterialTypeLink>
    {
        public WP_CashCenter_StockLocationSettingMaterialTypeLinkConfiguration()
            : this("dbo")
        {
        }

        public WP_CashCenter_StockLocationSettingMaterialTypeLinkConfiguration(string schema)
        {
            ToTable("WP_CashCenter_StockLocationSettingMaterialTypeLink", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.StockLocationSettingID).HasColumnName(@"StockLocationSettingID").HasColumnType("int").IsRequired();
            Property(x => x.MaterialTypeCode).HasColumnName(@"MaterialTypeCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>