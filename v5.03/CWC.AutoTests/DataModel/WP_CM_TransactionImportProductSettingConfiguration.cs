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

    // WP_CM_TransactionImportProductSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CM_TransactionImportProductSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CM_TransactionImportProductSetting>
    {
        public WP_CM_TransactionImportProductSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_CM_TransactionImportProductSettingConfiguration(string schema)
        {
            ToTable("WP_CM_TransactionImportProductSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Denomination).HasColumnName(@"Denomination").HasColumnType("int").IsRequired();
            Property(x => x.ProductID).HasColumnName(@"ProductID").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
        }
    }

}
// </auto-generated>