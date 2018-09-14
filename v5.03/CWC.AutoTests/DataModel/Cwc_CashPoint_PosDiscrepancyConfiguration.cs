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

    // Cwc_CashPoint_PosDiscrepancy
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_CashPoint_PosDiscrepancyConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_CashPoint_PosDiscrepancy>
    {
        public Cwc_CashPoint_PosDiscrepancyConfiguration()
            : this("dbo")
        {
        }

        public Cwc_CashPoint_PosDiscrepancyConfiguration(string schema)
        {
            ToTable("Cwc_CashPoint_PosDiscrepancy", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.PosValue).HasColumnName(@"PosValue").HasColumnType("numeric").IsRequired().HasPrecision(18,2);
            Property(x => x.CashPointValue).HasColumnName(@"CashPointValue").HasColumnType("numeric").IsRequired().HasPrecision(18,2);
            Property(x => x.DiscrepancyValue).HasColumnName(@"DiscrepancyValue").HasColumnType("numeric").IsRequired().HasPrecision(18,2);
            Property(x => x.Status).HasColumnName(@"Status").HasColumnType("int").IsRequired();
            Property(x => x.PosReconciliationPeriodID).HasColumnName(@"PosReconciliationPeriodID").HasColumnType("int").IsRequired();
            Property(x => x.CurrencyID).HasColumnName(@"CurrencyID").HasColumnType("nvarchar").IsRequired().HasMaxLength(3);
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsRequired();
            Property(x => x.AuthorID).HasColumnName(@"AuthorID").HasColumnType("int").IsOptional();
            Property(x => x.EditorID).HasColumnName(@"EditorID").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
