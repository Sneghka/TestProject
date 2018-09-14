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

    // WP_CM_OptimizationSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CM_OptimizationSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CM_OptimizationSetting>
    {
        public WP_CM_OptimizationSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_CM_OptimizationSettingConfiguration(string schema)
        {
            ToTable("WP_CM_OptimizationSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.CorrectionFactor).HasColumnName(@"CorrectionFactor").HasColumnType("decimal").IsRequired().HasPrecision(26,8);
            Property(x => x.WeeksNumber).HasColumnName(@"WeeksNumber").HasColumnType("int").IsRequired();
            Property(x => x.DeterminationMinimum).HasColumnName(@"DeterminationMinimum").HasColumnType("decimal").IsRequired().HasPrecision(26,8);
            Property(x => x.ExecutionTime).HasColumnName(@"ExecutionTime").HasColumnType("float").IsRequired();
            Property(x => x.IsBusy).HasColumnName(@"IsBusy").HasColumnType("bit").IsRequired();
            Property(x => x.EnableOrderCreation).HasColumnName(@"EnableOrderCreation").HasColumnType("bit").IsRequired();
            Property(x => x.ForceOrderConfirmation).HasColumnName(@"ForceOrderConfirmation").HasColumnType("bit").IsRequired();
            Property(x => x.AbnormalUsageGreenValue).HasColumnName(@"AbnormalUsageGreenValue").HasColumnType("int").IsOptional();
            Property(x => x.AbnormalUsageOrangeValue).HasColumnName(@"AbnormalUsageOrangeValue").HasColumnType("int").IsOptional();
            Property(x => x.FallbackPeriod).HasColumnName(@"FallbackPeriod").HasColumnType("int").IsRequired();
            Property(x => x.LastOptimizationDate).HasColumnName(@"LastOptimizationDate").HasColumnType("datetime").IsOptional();
            Property(x => x.IsFillDaily).HasColumnName(@"IsFillDaily").HasColumnType("bit").IsRequired();
            Property(x => x.AverageTurnoverPeriod).HasColumnName(@"AverageTurnoverPeriod").HasColumnType("int").IsRequired();
            Property(x => x.TrustOrdersInProgress).HasColumnName(@"TrustOrdersInProgress").HasColumnType("bit").IsRequired();
            Property(x => x.ForecastPeriod).HasColumnName(@"ForecastPeriod").HasColumnType("int").IsRequired();
            Property(x => x.CompanyID).HasColumnName(@"CompanyID").HasColumnType("decimal").IsOptional().HasPrecision(15,0);
            Property(x => x.MachineTypeID).HasColumnName(@"MachineTypeID").HasColumnType("int").IsOptional();
            Property(x => x.RoundOrderQuantityDown).HasColumnName(@"RoundOrderQuantityDown").HasColumnType("bit").IsRequired();
            Property(x => x.LocationID).HasColumnName(@"LocationID").HasColumnType("numeric").IsOptional().HasPrecision(15,0);
            Property(x => x.InterestRate).HasColumnName(@"InterestRate").HasColumnType("decimal").IsRequired().HasPrecision(5,2);
        }
    }

}
// </auto-generated>
