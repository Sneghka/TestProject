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

    // WP_CashCenter_SiteStockHistoryJobLog
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_SiteStockHistoryJobLogConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CashCenter_SiteStockHistoryJobLog>
    {
        public WP_CashCenter_SiteStockHistoryJobLogConfiguration()
            : this("dbo")
        {
        }

        public WP_CashCenter_SiteStockHistoryJobLogConfiguration(string schema)
        {
            ToTable("WP_CashCenter_SiteStockHistoryJobLog", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.Result).HasColumnName(@"Result").HasColumnType("int").IsRequired();
            Property(x => x.Message).HasColumnName(@"Message").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.StockDate).HasColumnName(@"StockDate").HasColumnType("date").IsRequired();
            Property(x => x.StockType).HasColumnName(@"StockType").HasColumnType("int").IsRequired();
            Property(x => x.StockOwnerID).HasColumnName(@"StockOwnerID").HasColumnType("int").IsRequired();
            Property(x => x.CurrencyID).HasColumnName(@"CurrencyID").HasColumnType("nvarchar").IsRequired().HasMaxLength(3);
            Property(x => x.IsFinalized).HasColumnName(@"IsFinalized").HasColumnType("bit").IsRequired();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsRequired();
            Property(x => x.StockLocationID).HasColumnName(@"StockLocationID").HasColumnType("int").IsRequired();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>