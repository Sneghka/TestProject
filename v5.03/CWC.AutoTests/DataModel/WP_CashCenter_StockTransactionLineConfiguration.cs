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

    // WP_CashCenter_StockTransactionLine
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_StockTransactionLineConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CashCenter_StockTransactionLine>
    {
        public WP_CashCenter_StockTransactionLineConfiguration()
            : this("dbo")
        {
        }

        public WP_CashCenter_StockTransactionLineConfiguration(string schema)
        {
            ToTable("WP_CashCenter_StockTransactionLine", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("bigint").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Direction).HasColumnName(@"Direction").HasColumnType("int").IsRequired();
            Property(x => x.Quantity).HasColumnName(@"Quantity").HasColumnType("int").IsRequired();
            Property(x => x.Value).HasColumnName(@"Value").HasColumnType("numeric").IsRequired().HasPrecision(15,2);
            Property(x => x.Weight).HasColumnName(@"Weight").HasColumnType("numeric").IsRequired().HasPrecision(15,5);
            Property(x => x.IsVerified).HasColumnName(@"IsVerified").HasColumnType("bit").IsRequired();
            Property(x => x.QualificationType).HasColumnName(@"QualificationType").HasColumnType("int").IsRequired();
            Property(x => x.StockTransaction_id).HasColumnName(@"StockTransaction_id").HasColumnType("bigint").IsRequired();
            Property(x => x.Material_id).HasColumnName(@"Material_id").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.Product_id).HasColumnName(@"Product_id").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.StockContainer_id).HasColumnName(@"StockContainer_id").HasColumnType("bigint").IsOptional();
            Property(x => x.StockLocation_id).HasColumnName(@"StockLocation_id").HasColumnType("int").IsOptional();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.IsStockOwnerChanged).HasColumnName(@"IsStockOwnerChanged").HasColumnType("bit").IsRequired();
            Property(x => x.StockOwnerID).HasColumnName(@"StockOwnerID").HasColumnType("int").IsOptional();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsOptional();
            Property(x => x.EditorID).HasColumnName(@"EditorID").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
