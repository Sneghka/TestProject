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

    // Cwc_Billing_BillingLine
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Billing_BillingLineConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Billing_BillingLine>
    {
        public Cwc_Billing_BillingLineConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Billing_BillingLineConfiguration(string schema)
        {
            ToTable("Cwc_Billing_BillingLine", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.DateBilled).HasColumnName(@"DateBilled").HasColumnType("datetime").IsRequired();
            Property(x => x.Value).HasColumnName(@"Value").HasColumnType("decimal").IsRequired().HasPrecision(18,5);
            Property(x => x.ContractRevisionNumber).HasColumnName(@"ContractRevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.ValueSurcharge).HasColumnName(@"ValueSurcharge").HasColumnType("decimal").IsRequired().HasPrecision(18,5);
            Property(x => x.Units).HasColumnName(@"Units").HasColumnType("decimal").IsRequired().HasPrecision(18,2);
            Property(x => x.Comment).HasColumnName(@"Comment").HasColumnType("ntext").IsOptional().IsMaxLength();
            Property(x => x.PeriodBilledFrom).HasColumnName(@"PeriodBilledFrom").HasColumnType("datetime").IsOptional();
            Property(x => x.PeriodBilledTo).HasColumnName(@"PeriodBilledTo").HasColumnType("datetime").IsOptional();
            Property(x => x.ContractID).HasColumnName(@"ContractID").HasColumnType("int").IsOptional();
            Property(x => x.CounterpartyID).HasColumnName(@"CounterpartyID").HasColumnType("int").IsOptional();
            Property(x => x.IsManual).HasColumnName(@"IsManual").HasColumnType("bit").IsRequired();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsOptional();
            Property(x => x.AuthorID).HasColumnName(@"AuthorID").HasColumnType("int").IsOptional();
            Property(x => x.EditorID).HasColumnName(@"EditorID").HasColumnType("int").IsOptional();
            Property(x => x.PriceRule).HasColumnName(@"PriceRule").HasColumnType("int").IsOptional();
            Property(x => x.UnitOfMeasure).HasColumnName(@"UnitOfMeasure").HasColumnType("int").IsOptional();
            Property(x => x.ValueRegularPrice).HasColumnName(@"ValueRegularPrice").HasColumnType("decimal").IsRequired().HasPrecision(18,5);
            Property(x => x.ValueRangePrice).HasColumnName(@"ValueRangePrice").HasColumnType("decimal").IsRequired().HasPrecision(18,5);
            Property(x => x.ValueSurchargePercentage).HasColumnName(@"ValueSurchargePercentage").HasColumnType("decimal").IsRequired().HasPrecision(18,5);
            Property(x => x.BankAccountID).HasColumnName(@"BankAccountID").HasColumnType("int").IsOptional();
            Property(x => x.PriceLineID).HasColumnName(@"PriceLineID").HasColumnType("int").IsOptional();
            Property(x => x.SurchargeFixedPriceLineID).HasColumnName(@"SurchargeFixedPriceLineID").HasColumnType("int").IsOptional();
            Property(x => x.SurchargePercentagePriceLineID).HasColumnName(@"SurchargePercentagePriceLineID").HasColumnType("int").IsOptional();
            Property(x => x.CustomerID).HasColumnName(@"CustomerID").HasColumnType("int").IsRequired();
            Property(x => x.LocationID).HasColumnName(@"LocationID").HasColumnType("int").IsOptional();
            Property(x => x.DebtorID).HasColumnName(@"DebtorID").HasColumnType("int").IsOptional();
            Property(x => x.BilledCaseID).HasColumnName(@"BilledCaseID").HasColumnType("int").IsOptional();
            Property(x => x.VisitAddressID).HasColumnName(@"VisitAddressID").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
