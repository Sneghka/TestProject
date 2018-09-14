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

    // WP_BagTypeContentSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BagTypeContentSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_BagTypeContentSetting>
    {
        public WP_BagTypeContentSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_BagTypeContentSettingConfiguration(string schema)
        {
            ToTable("WP_BagTypeContentSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.BagType_id).HasColumnName(@"BagType_id").HasColumnType("int").IsRequired();
            Property(x => x.Material_id).HasColumnName(@"Material_id").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.Product_id).HasColumnName(@"Product_id").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.StandardQuantity).HasColumnName(@"StandardQuantity").HasColumnType("int").IsOptional();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.MaxQuantity).HasColumnName(@"MaxQuantity").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>