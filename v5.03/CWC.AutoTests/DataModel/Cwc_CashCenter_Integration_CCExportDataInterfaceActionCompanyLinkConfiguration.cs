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

    // Cwc_CashCenter_Integration_CCExportDataInterfaceActionCompanyLink
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_CashCenter_Integration_CCExportDataInterfaceActionCompanyLinkConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_CashCenter_Integration_CCExportDataInterfaceActionCompanyLink>
    {
        public Cwc_CashCenter_Integration_CCExportDataInterfaceActionCompanyLinkConfiguration()
            : this("dbo")
        {
        }

        public Cwc_CashCenter_Integration_CCExportDataInterfaceActionCompanyLinkConfiguration(string schema)
        {
            ToTable("Cwc_CashCenter_Integration_CCExportDataInterfaceActionCompanyLink", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ActionID).HasColumnName(@"ActionID").HasColumnType("int").IsRequired();
            Property(x => x.CompanyID).HasColumnName(@"CompanyID").HasColumnType("int").IsRequired();
        }
    }

}
// </auto-generated>
