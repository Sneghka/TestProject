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

    // WP_GroupCustomer
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_GroupCustomerConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_GroupCustomer>
    {
        public WP_GroupCustomerConfiguration()
            : this("dbo")
        {
        }

        public WP_GroupCustomerConfiguration(string schema)
        {
            ToTable("WP_GroupCustomer", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Group_id).HasColumnName(@"Group_id").HasColumnType("int").IsRequired();
            Property(x => x.Customer_id).HasColumnName(@"Customer_id").HasColumnType("numeric").IsRequired().HasPrecision(15,0);
        }
    }

}
// </auto-generated>
