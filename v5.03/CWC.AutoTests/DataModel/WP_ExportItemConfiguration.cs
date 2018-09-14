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

    // WP_ExportItem
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_ExportItemConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_ExportItem>
    {
        public WP_ExportItemConfiguration()
            : this("dbo")
        {
        }

        public WP_ExportItemConfiguration(string schema)
        {
            ToTable("WP_ExportItem", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.FileName).HasColumnName(@"FileName").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.FileData).HasColumnName(@"FileData").HasColumnType("varbinary(max)").IsOptional();
            Property(x => x.alias).HasColumnName(@"alias").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.AdditionalInfo).HasColumnName(@"AdditionalInfo").HasColumnType("varbinary(max)").IsOptional();
            Property(x => x.NeedEncryption).HasColumnName(@"NeedEncryption").HasColumnType("bit").IsRequired();
        }
    }

}
// </auto-generated>
