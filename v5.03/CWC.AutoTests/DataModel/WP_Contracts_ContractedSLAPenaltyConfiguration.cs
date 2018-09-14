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

    // WP_Contracts_ContractedSLAPenalty
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Contracts_ContractedSLAPenaltyConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Contracts_ContractedSLAPenalty>
    {
        public WP_Contracts_ContractedSLAPenaltyConfiguration()
            : this("dbo")
        {
        }

        public WP_Contracts_ContractedSLAPenaltyConfiguration(string schema)
        {
            ToTable("WP_Contracts_ContractedSLAPenalty", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.IsLatestRevision).HasColumnName(@"IsLatestRevision").HasColumnType("bit").IsRequired();
            Property(x => x.RevisionNumber).HasColumnName(@"RevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.RevisionDate).HasColumnName(@"RevisionDate").HasColumnType("datetime").IsOptional();
            Property(x => x.ReplacedRevisionNumber).HasColumnName(@"ReplacedRevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.ReplacedRevisionDate).HasColumnName(@"ReplacedRevisionDate").HasColumnType("datetime").IsOptional();
            Property(x => x.Comments).HasColumnName(@"Comments").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.Author_id).HasColumnName(@"Author_id").HasColumnType("int").IsOptional();
            Property(x => x.PricePerCall).HasColumnName(@"PricePerCall").HasColumnType("decimal").IsRequired().HasPrecision(18,2);
            Property(x => x.PercentStart).HasColumnName(@"PercentStart").HasColumnType("int").IsRequired();
            Property(x => x.PercentEnd).HasColumnName(@"PercentEnd").HasColumnType("int").IsRequired();
            Property(x => x.ContractLine_id).HasColumnName(@"ContractLine_id").HasColumnType("int").IsRequired();
            Property(x => x.CallCategory).HasColumnName(@"CallCategory").HasColumnType("int").IsRequired();
            Property(x => x.FailureCode).HasColumnName(@"FailureCode").HasColumnType("int").IsOptional();
            Property(x => x.LatestRevision_id).HasColumnName(@"LatestRevision_id").HasColumnType("int").IsOptional();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>