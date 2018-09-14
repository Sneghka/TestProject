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

    // Cwc_Forecast_TurnoverPercentageSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Forecast_TurnoverPercentageSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Forecast_TurnoverPercentageSetting>
    {
        public Cwc_Forecast_TurnoverPercentageSettingConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Forecast_TurnoverPercentageSettingConfiguration(string schema)
        {
            ToTable("Cwc_Forecast_TurnoverPercentageSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Weekday).HasColumnName(@"Weekday").HasColumnType("int").IsRequired();
            Property(x => x.ServiceDateType).HasColumnName(@"ServiceDateType").HasColumnType("int").IsRequired();
            Property(x => x.TurnoverPercentage).HasColumnName(@"TurnoverPercentage").HasColumnType("int").IsRequired();
            Property(x => x.OptimizationSettingsID).HasColumnName(@"OptimizationSettingsID").HasColumnType("int").IsRequired();
        }
    }

}
// </auto-generated>