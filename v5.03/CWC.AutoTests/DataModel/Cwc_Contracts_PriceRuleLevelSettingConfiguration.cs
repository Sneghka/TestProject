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

    // Cwc_Contracts_PriceRuleLevelSetting
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Contracts_PriceRuleLevelSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Contracts_PriceRuleLevelSetting>
    {
        public Cwc_Contracts_PriceRuleLevelSettingConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Contracts_PriceRuleLevelSettingConfiguration(string schema)
        {
            ToTable("Cwc_Contracts_PriceRuleLevelSetting", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.PriceRule).HasColumnName(@"PriceRule").HasColumnType("int").IsRequired();
            Property(x => x.LevelName).HasColumnName(@"LevelName").HasColumnType("int").IsRequired();
            Property(x => x.LevelValueType).HasColumnName(@"LevelValueType").HasColumnType("int").IsRequired();
            Property(x => x.IsRangeLevel).HasColumnName(@"IsRangeLevel").HasColumnType("bit").IsRequired();
            Property(x => x.SequenceNumber).HasColumnName(@"SequenceNumber").HasColumnType("int").IsRequired();
            Property(x => x.IsEnabled).HasColumnName(@"IsEnabled").HasColumnType("bit").IsRequired();
        }
    }

}
// </auto-generated>
