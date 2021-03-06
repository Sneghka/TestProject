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

    // WP_CashCenter_OrdersBatch
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_OrdersBatchConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CashCenter_OrdersBatch>
    {
        public WP_CashCenter_OrdersBatchConfiguration()
            : this("dbo")
        {
        }

        public WP_CashCenter_OrdersBatchConfiguration(string schema)
        {
            ToTable("WP_CashCenter_OrdersBatch", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("bigint").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Number).HasColumnName(@"Number").HasColumnType("nvarchar").IsRequired().HasMaxLength(30);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.CreationMethod).HasColumnName(@"CreationMethod").HasColumnType("int").IsRequired();
            Property(x => x.Status).HasColumnName(@"Status").HasColumnType("int").IsRequired();
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsRequired();
            Property(x => x.Site_id).HasColumnName(@"Site_id").HasColumnType("int").IsOptional();
            Property(x => x.Author_id).HasColumnName(@"Author_id").HasColumnType("int").IsOptional();
            Property(x => x.Editor_id).HasColumnName(@"Editor_id").HasColumnType("int").IsOptional();
            Property(x => x.AssignedTo_id).HasColumnName(@"AssignedTo_id").HasColumnType("int").IsOptional();
            Property(x => x.CITDepot_id).HasColumnName(@"CITDepot_id").HasColumnType("int").IsOptional();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.PackingMode).HasColumnName(@"PackingMode").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
