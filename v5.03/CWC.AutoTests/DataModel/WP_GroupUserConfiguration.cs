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

    // WP_GroupUser
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_GroupUserConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_GroupUser>
    {
        public WP_GroupUserConfiguration()
            : this("dbo")
        {
        }

        public WP_GroupUserConfiguration(string schema)
        {
            ToTable("WP_GroupUser", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Group_id).HasColumnName(@"Group_id").HasColumnType("int").IsRequired();
            Property(x => x.User_id).HasColumnName(@"User_id").HasColumnType("int").IsRequired();
        }
    }

}
// </auto-generated>
