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

    // WP_Contracts_CashCenterContractLine
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Contracts_CashCenterContractLineConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Contracts_CashCenterContractLine>
    {
        public WP_Contracts_CashCenterContractLineConfiguration()
            : this("dbo")
        {
        }

        public WP_Contracts_CashCenterContractLineConfiguration(string schema)
        {
            ToTable("WP_Contracts_CashCenterContractLine", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.IsLatestRevision).HasColumnName(@"IsLatestRevision").HasColumnType("bit").IsRequired();
            Property(x => x.RevisionNumber).HasColumnName(@"RevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.RevisionDate).HasColumnName(@"RevisionDate").HasColumnType("datetime").IsOptional();
            Property(x => x.ReplacedRevisionNumber).HasColumnName(@"ReplacedRevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.ReplacedRevisionDate).HasColumnName(@"ReplacedRevisionDate").HasColumnType("datetime").IsOptional();
            Property(x => x.Comments).HasColumnName(@"Comments").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.Author_id).HasColumnName(@"Author_id").HasColumnType("int").IsOptional();
            Property(x => x.PriceMethod).HasColumnName(@"PriceMethod").HasColumnType("int").IsRequired();
            Property(x => x.Price).HasColumnName(@"Price").HasColumnType("decimal").IsRequired().HasPrecision(18,2);
            Property(x => x.ContractID).HasColumnName(@"ContractID").HasColumnType("int").IsRequired();
            Property(x => x.ServiceTypeID).HasColumnName(@"ServiceTypeID").HasColumnType("int").IsRequired();
            Property(x => x.LocationTypeCode).HasColumnName(@"LocationTypeCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(16);
            Property(x => x.RegionID).HasColumnName(@"RegionID").HasColumnType("numeric").IsOptional().HasPrecision(15,0);
            Property(x => x.LocationID).HasColumnName(@"LocationID").HasColumnType("numeric").IsOptional().HasPrecision(15,0);
            Property(x => x.DebtorID).HasColumnName(@"DebtorID").HasColumnType("numeric").IsOptional().HasPrecision(15,0);
            Property(x => x.BankAccountID).HasColumnName(@"BankAccountID").HasColumnType("int").IsOptional();
            Property(x => x.LatestRevision_id).HasColumnName(@"LatestRevision_id").HasColumnType("int").IsOptional();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>