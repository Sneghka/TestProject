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

    // ServicingCode
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class ServicingCodeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ServicingCode>
    {
        public ServicingCodeConfiguration()
            : this("dbo")
        {
        }

        public ServicingCodeConfiguration(string schema)
        {
            ToTable("ServicingCode", schema);
            HasKey(x => x.servCode);

            Property(x => x.servCode).HasColumnName(@"servCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(8).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.servDesc).HasColumnName(@"servDesc").HasColumnType("nvarchar").IsOptional().HasMaxLength(40);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }
    }

}
// </auto-generated>
