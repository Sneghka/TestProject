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

    // WP_Ordering_FixedOrderCreationLog
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Ordering_FixedOrderCreationLogConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Ordering_FixedOrderCreationLog>
    {
        public WP_Ordering_FixedOrderCreationLogConfiguration()
            : this("dbo")
        {
        }

        public WP_Ordering_FixedOrderCreationLogConfiguration(string schema)
        {
            ToTable("WP_Ordering_FixedOrderCreationLog", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.Result).HasColumnName(@"Result").HasColumnType("int").IsRequired();
            Property(x => x.Comment).HasColumnName(@"Comment").HasColumnType("ntext").IsOptional().IsMaxLength();
            Property(x => x.Location_id).HasColumnName(@"Location_id").HasColumnType("numeric").IsRequired().HasPrecision(15,0);
            Property(x => x.Order_id).HasColumnName(@"Order_id").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
        }
    }

}
// </auto-generated>
