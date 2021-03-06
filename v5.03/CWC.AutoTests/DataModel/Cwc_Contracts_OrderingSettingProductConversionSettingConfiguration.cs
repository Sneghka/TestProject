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

    // Cwc_Contracts_OrderingSettingProductConversionSetting
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Contracts_OrderingSettingProductConversionSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Contracts_OrderingSettingProductConversionSetting>
    {
        public Cwc_Contracts_OrderingSettingProductConversionSettingConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Contracts_OrderingSettingProductConversionSettingConfiguration(string schema)
        {
            ToTable("Cwc_Contracts_OrderingSettingProductConversionSetting", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.OrderedProductID).HasColumnName(@"OrderedProductID").HasColumnType("int").IsRequired();
            Property(x => x.ConvertedProductID).HasColumnName(@"ConvertedProductID").HasColumnType("int").IsRequired();
            Property(x => x.ContractOrderingSettingID).HasColumnName(@"ContractOrderingSettingID").HasColumnType("int").IsRequired();
            Property(x => x.IsLatestRevision).HasColumnName(@"IsLatestRevision").HasColumnType("bit").IsRequired();
            Property(x => x.RevisionNumber).HasColumnName(@"RevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.RevisionDate).HasColumnName(@"RevisionDate").HasColumnType("datetime").IsOptional();
            Property(x => x.ReplacedRevisionNumber).HasColumnName(@"ReplacedRevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.ReplacedRevisionDate).HasColumnName(@"ReplacedRevisionDate").HasColumnType("datetime").IsOptional();
            Property(x => x.AuthorID).HasColumnName(@"AuthorID").HasColumnType("int").IsOptional();
            Property(x => x.LatestRevisionID).HasColumnName(@"LatestRevisionID").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
