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

    // WP_BaseData_BankAccountFormat
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_BankAccountFormatConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_BaseData_BankAccountFormat>
    {
        public WP_BaseData_BankAccountFormatConfiguration()
            : this("dbo")
        {
        }

        public WP_BaseData_BankAccountFormatConfiguration(string schema)
        {
            ToTable("WP_BaseData_BankAccountFormat", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.BankID).HasColumnName(@"BankID").HasColumnType("numeric").IsRequired().HasPrecision(18,0);
            Property(x => x.FormatID).HasColumnName(@"FormatID").HasColumnType("int").IsRequired();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>
