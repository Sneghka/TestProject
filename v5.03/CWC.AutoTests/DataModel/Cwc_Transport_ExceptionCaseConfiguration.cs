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

    // Cwc_Transport_ExceptionCase
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Transport_ExceptionCaseConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Transport_ExceptionCase>
    {
        public Cwc_Transport_ExceptionCaseConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Transport_ExceptionCaseConfiguration(string schema)
        {
            ToTable("Cwc_Transport_ExceptionCase", schema);
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName(@"ID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("int").IsRequired();
            Property(x => x.Category).HasColumnName(@"Category").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
