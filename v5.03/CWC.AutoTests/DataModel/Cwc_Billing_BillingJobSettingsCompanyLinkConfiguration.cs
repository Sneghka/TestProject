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

    // Cwc_Billing_BillingJobSettingsCompanyLink
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Billing_BillingJobSettingsCompanyLinkConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Billing_BillingJobSettingsCompanyLink>
    {
        public Cwc_Billing_BillingJobSettingsCompanyLinkConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Billing_BillingJobSettingsCompanyLinkConfiguration(string schema)
        {
            ToTable("Cwc_Billing_BillingJobSettingsCompanyLink", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.BillingJobSettingsID).HasColumnName(@"BillingJobSettingsID").HasColumnType("int").IsRequired();
            Property(x => x.CompanyID).HasColumnName(@"CompanyID").HasColumnType("numeric").IsRequired().HasPrecision(15,0);
        }
    }

}
// </auto-generated>
