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

    // WP_BaseData_ContactPerson
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_ContactPersonConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_BaseData_ContactPerson>
    {
        public WP_BaseData_ContactPersonConfiguration()
            : this("dbo")
        {
        }

        public WP_BaseData_ContactPersonConfiguration(string schema)
        {
            ToTable("WP_BaseData_ContactPerson", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(255);
            Property(x => x.MiddleName).HasColumnName(@"MiddleName").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(255);
            Property(x => x.LastName).HasColumnName(@"LastName").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(255);
            Property(x => x.Email).HasColumnName(@"Email").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(255);
            Property(x => x.PhoneNumber).HasColumnName(@"PhoneNumber").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(30);
            Property(x => x.FaxNumber).HasColumnName(@"FaxNumber").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(30);
            Property(x => x.Type).HasColumnName(@"Type").HasColumnType("int").IsOptional();
            Property(x => x.IsPreferred).HasColumnName(@"IsPreferred").HasColumnType("bit").IsRequired();
            Property(x => x.JobTitle).HasColumnName(@"JobTitle").HasColumnType("varchar(max)").IsOptional().IsUnicode(false);
            Property(x => x.OtherPhone).HasColumnName(@"OtherPhone").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(30);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>
