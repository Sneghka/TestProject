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

    // WP_BaseData_BagTypeMaterialTypeLink
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_BagTypeMaterialTypeLinkConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_BaseData_BagTypeMaterialTypeLink>
    {
        public WP_BaseData_BagTypeMaterialTypeLinkConfiguration()
            : this("dbo")
        {
        }

        public WP_BaseData_BagTypeMaterialTypeLinkConfiguration(string schema)
        {
            ToTable("WP_BaseData_BagTypeMaterialTypeLink", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.BagTypeID).HasColumnName(@"BagTypeID").HasColumnType("int").IsRequired();
            Property(x => x.MaterialTypeID).HasColumnName(@"MaterialTypeID").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
            Property(x => x.MinimumQuantity).HasColumnName(@"MinimumQuantity").HasColumnType("int").IsRequired();
            Property(x => x.MaximumQuantity).HasColumnName(@"MaximumQuantity").HasColumnType("int").IsRequired();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>
