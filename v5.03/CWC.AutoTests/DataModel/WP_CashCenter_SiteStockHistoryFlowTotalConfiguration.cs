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

    // WP_CashCenter_SiteStockHistoryFlowTotal
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_SiteStockHistoryFlowTotalConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CashCenter_SiteStockHistoryFlowTotal>
    {
        public WP_CashCenter_SiteStockHistoryFlowTotalConfiguration()
            : this("dbo")
        {
        }

        public WP_CashCenter_SiteStockHistoryFlowTotalConfiguration(string schema)
        {
            ToTable("WP_CashCenter_SiteStockHistoryFlowTotal", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Flow).HasColumnName(@"Flow").HasColumnType("int").IsRequired();
            Property(x => x.MaterialType).HasColumnName(@"MaterialType").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.ProcessQuantity).HasColumnName(@"ProcessQuantity").HasColumnType("int").IsRequired();
            Property(x => x.QuantityDescription).HasColumnName(@"QuantityDescription").HasColumnType("int").IsRequired();
            Property(x => x.MaterialsQuantity).HasColumnName(@"MaterialsQuantity").HasColumnType("int").IsRequired();
            Property(x => x.MaterialsValue).HasColumnName(@"MaterialsValue").HasColumnType("numeric").IsRequired().HasPrecision(15,2);
            Property(x => x.SiteStockHistoryID).HasColumnName(@"SiteStockHistoryID").HasColumnType("int").IsRequired();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.QualificationType).HasColumnName(@"QualificationType").HasColumnType("int").IsRequired();
        }
    }

}
// </auto-generated>
