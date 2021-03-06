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

    // Cwc_Contracts_OrderingSettingServicingJob
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Contracts_OrderingSettingServicingJobConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Contracts_OrderingSettingServicingJob>
    {
        public Cwc_Contracts_OrderingSettingServicingJobConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Contracts_OrderingSettingServicingJobConfiguration(string schema)
        {
            ToTable("Cwc_Contracts_OrderingSettingServicingJob", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.IsLatestRevision).HasColumnName(@"IsLatestRevision").HasColumnType("bit").IsRequired();
            Property(x => x.RevisionNumber).HasColumnName(@"RevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.RevisionDate).HasColumnName(@"RevisionDate").HasColumnType("datetime").IsOptional();
            Property(x => x.ReplacedRevisionNumber).HasColumnName(@"ReplacedRevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.ReplacedRevisionDate).HasColumnName(@"ReplacedRevisionDate").HasColumnType("datetime").IsOptional();
            Property(x => x.Comments).HasColumnName(@"Comments").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.Author_id).HasColumnName(@"Author_id").HasColumnType("int").IsOptional();
            Property(x => x.OrderingSettingsId).HasColumnName(@"OrderingSettingsId").HasColumnType("int").IsRequired();
            Property(x => x.ServiceCode).HasColumnName(@"ServiceCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(8);
            Property(x => x.LatestRevision_id).HasColumnName(@"LatestRevision_id").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
