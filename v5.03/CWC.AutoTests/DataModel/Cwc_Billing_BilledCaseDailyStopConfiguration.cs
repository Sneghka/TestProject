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

    // Cwc_Billing_BilledCaseDailyStop
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Billing_BilledCaseDailyStopConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Billing_BilledCaseDailyStop>
    {
        public Cwc_Billing_BilledCaseDailyStopConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Billing_BilledCaseDailyStopConfiguration(string schema)
        {
            ToTable("Cwc_Billing_BilledCaseDailyStop", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.BilledCaseID).HasColumnName(@"BilledCaseID").HasColumnType("int").IsRequired();
            Property(x => x.DailyStopID).HasColumnName(@"DailyStopID").HasColumnType("int").IsRequired();
        }
    }

}
// </auto-generated>
