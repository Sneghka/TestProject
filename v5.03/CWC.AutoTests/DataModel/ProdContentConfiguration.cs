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

    // ProdContent
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class ProdContentConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProdContent>
    {
        public ProdContentConfiguration()
            : this("dbo")
        {
        }

        public ProdContentConfiguration(string schema)
        {
            ToTable("ProdContent", schema);
            HasKey(x => x.id);

            Property(x => x.ProductCode).HasColumnName(@"ProductCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
            Property(x => x.materialID).HasColumnName(@"materialID").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
            Property(x => x.numItems).HasColumnName(@"numItems").HasColumnType("int").IsOptional();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }
    }

}
// </auto-generated>
