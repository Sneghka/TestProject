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

    // Material
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class MaterialConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Material>
    {
        public MaterialConfiguration()
            : this("dbo")
        {
        }

        public MaterialConfiguration(string schema)
        {
            ToTable("Material", schema);
            HasKey(x => x.WP_id);

            Property(x => x.materialID).HasColumnName(@"materialID").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
            Property(x => x.materialNum).HasColumnName(@"materialNum").HasColumnType("int").IsOptional();
            Property(x => x.MaterialDesc).HasColumnName(@"MaterialDesc").HasColumnType("nvarchar").IsOptional().HasMaxLength(30);
            Property(x => x.matTypeCode).HasColumnName(@"matTypeCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
            Property(x => x.curCode).HasColumnName(@"curCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(3);
            Property(x => x.denomination).HasColumnName(@"denomination").HasColumnType("decimal").IsOptional().HasPrecision(26,8);
            Property(x => x.weight).HasColumnName(@"weight").HasColumnType("decimal").IsOptional().HasPrecision(26,8);
            Property(x => x.WP_id).HasColumnName(@"WP_id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.MaximumValue).HasColumnName(@"MaximumValue").HasColumnType("int").IsOptional();
            Property(x => x.ReferenceCode).HasColumnName(@"ReferenceCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
        }
    }

}
// </auto-generated>
