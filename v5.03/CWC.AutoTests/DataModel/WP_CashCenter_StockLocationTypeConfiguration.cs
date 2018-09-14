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

    // WP_CashCenter_StockLocationType
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_StockLocationTypeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CashCenter_StockLocationType>
    {
        public WP_CashCenter_StockLocationTypeConfiguration()
            : this("dbo")
        {
        }

        public WP_CashCenter_StockLocationTypeConfiguration(string schema)
        {
            ToTable("WP_CashCenter_StockLocationType", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
            Property(x => x.Level).HasColumnName(@"Level").HasColumnType("int").IsRequired();
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsOptional();
            Property(x => x.SiteID).HasColumnName(@"SiteID").HasColumnType("int").IsRequired();
            Property(x => x.AuthorID).HasColumnName(@"AuthorID").HasColumnType("int").IsRequired();
            Property(x => x.EditorID).HasColumnName(@"EditorID").HasColumnType("int").IsOptional();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>
