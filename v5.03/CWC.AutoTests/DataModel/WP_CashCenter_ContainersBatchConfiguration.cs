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

    // WP_CashCenter_ContainersBatch
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_ContainersBatchConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CashCenter_ContainersBatch>
    {
        public WP_CashCenter_ContainersBatchConfiguration()
            : this("dbo")
        {
        }

        public WP_CashCenter_ContainersBatchConfiguration(string schema)
        {
            ToTable("WP_CashCenter_ContainersBatch", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("bigint").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Status).HasColumnName(@"Status").HasColumnType("int").IsRequired();
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.Number).HasColumnName(@"Number").HasColumnType("nvarchar").IsRequired().HasMaxLength(30);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.Site_id).HasColumnName(@"Site_id").HasColumnType("int").IsRequired();
            Property(x => x.CreationMethod).HasColumnName(@"CreationMethod").HasColumnType("int").IsRequired();
            Property(x => x.Stream_id).HasColumnName(@"Stream_id").HasColumnType("int").IsRequired();
            Property(x => x.Author_id).HasColumnName(@"Author_id").HasColumnType("int").IsOptional();
            Property(x => x.Editor_id).HasColumnName(@"Editor_id").HasColumnType("int").IsOptional();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsRequired();
            Property(x => x.StockOwnerID).HasColumnName(@"StockOwnerID").HasColumnType("int").IsOptional();
            Property(x => x.AssignedToID).HasColumnName(@"AssignedToID").HasColumnType("int").IsOptional();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.IsCrossCheckRequired).HasColumnName(@"IsCrossCheckRequired").HasColumnType("bit").IsRequired();
        }
    }

}
// </auto-generated>
