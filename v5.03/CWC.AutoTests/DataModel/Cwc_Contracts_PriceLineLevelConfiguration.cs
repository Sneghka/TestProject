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

    // Cwc_Contracts_PriceLineLevel
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Contracts_PriceLineLevelConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Contracts_PriceLineLevel>
    {
        public Cwc_Contracts_PriceLineLevelConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Contracts_PriceLineLevelConfiguration(string schema)
        {
            ToTable("Cwc_Contracts_PriceLineLevel", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.IsLatestRevision).HasColumnName(@"IsLatestRevision").HasColumnType("bit").IsRequired();
            Property(x => x.RevisionNumber).HasColumnName(@"RevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.RevisionDate).HasColumnName(@"RevisionDate").HasColumnType("datetime").IsOptional();
            Property(x => x.ReplacedRevisionNumber).HasColumnName(@"ReplacedRevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.ReplacedRevisionDate).HasColumnName(@"ReplacedRevisionDate").HasColumnType("datetime").IsOptional();
            Property(x => x.AuthorID).HasColumnName(@"AuthorID").HasColumnType("int").IsOptional();
            Property(x => x.LevelName).HasColumnName(@"LevelName").HasColumnType("int").IsRequired();
            Property(x => x.IsRangeLevel).HasColumnName(@"IsRangeLevel").HasColumnType("bit").IsRequired();
            Property(x => x.LevelValueType).HasColumnName(@"LevelValueType").HasColumnType("int").IsRequired();
            Property(x => x.SequenceNumber).HasColumnName(@"SequenceNumber").HasColumnType("int").IsRequired();
            Property(x => x.PriceLineID).HasColumnName(@"PriceLineID").HasColumnType("int").IsRequired();
            Property(x => x.LatestRevisionID).HasColumnName(@"LatestRevisionID").HasColumnType("int").IsOptional();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.Value).HasColumnName(@"Value").HasColumnType("decimal").IsOptional().HasPrecision(18,5);
            Property(x => x.ValueFrom).HasColumnName(@"ValueFrom").HasColumnType("decimal").IsOptional().HasPrecision(18,5);
            Property(x => x.ValueTo).HasColumnName(@"ValueTo").HasColumnType("decimal").IsOptional().HasPrecision(18,5);
        }
    }

}
// </auto-generated>
