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

    // Cwc_CashCenter_Integration_CCExportDataInterfaceActionSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_CashCenter_Integration_CCExportDataInterfaceActionSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_CashCenter_Integration_CCExportDataInterfaceActionSetting>
    {
        public Cwc_CashCenter_Integration_CCExportDataInterfaceActionSettingConfiguration()
            : this("dbo")
        {
        }

        public Cwc_CashCenter_Integration_CCExportDataInterfaceActionSettingConfiguration(string schema)
        {
            ToTable("Cwc_CashCenter_Integration_CCExportDataInterfaceActionSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Flow).HasColumnName(@"Flow").HasColumnType("int").IsRequired();
            Property(x => x.DiscrepancyValue).HasColumnName(@"DiscrepancyValue").HasColumnType("int").IsOptional();
            Property(x => x.CurrencyType).HasColumnName(@"CurrencyType").HasColumnType("int").IsOptional();
            Property(x => x.MaterialType).HasColumnName(@"MaterialType").HasColumnType("int").IsRequired();
            Property(x => x.ServiceType).HasColumnName(@"ServiceType").HasColumnType("int").IsOptional();
            Property(x => x.ActionID).HasColumnName(@"ActionID").HasColumnType("int").IsRequired();
        }
    }

}
// </auto-generated>
