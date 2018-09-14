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

    // WP_Common_LockObject
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Common_LockObjectConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Common_LockObject>
    {
        public WP_Common_LockObjectConfiguration()
            : this("dbo")
        {
        }

        public WP_Common_LockObjectConfiguration(string schema)
        {
            ToTable("WP_Common_LockObject", schema);
            HasKey(x => new { x.Object_id, x.Class_id });

            Property(x => x.Object_id).HasColumnName(@"Object_id").HasColumnType("bigint").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Class_id).HasColumnName(@"Class_id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.User_id).HasColumnName(@"User_id").HasColumnType("int").IsRequired();
            Property(x => x.Session_id).HasColumnName(@"Session_id").HasColumnType("nvarchar(max)").IsRequired();
            Property(x => x.LockDateTime).HasColumnName(@"LockDateTime").HasColumnType("datetime").IsRequired();
        }
    }

}
// </auto-generated>