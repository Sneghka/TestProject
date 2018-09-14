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

    // WP_CashControl_JobServiceSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashControl_JobServiceSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CashControl_JobServiceSetting>
    {
        public WP_CashControl_JobServiceSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_CashControl_JobServiceSettingConfiguration(string schema)
        {
            ToTable("WP_CashControl_JobServiceSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.IsRunDeviceStateService).HasColumnName(@"IsRunDeviceStateService").HasColumnType("bit").IsRequired();
            Property(x => x.DeviceStateServiceLink).HasColumnName(@"DeviceStateServiceLink").HasColumnType("nvarchar").IsOptional().HasMaxLength(256);
            Property(x => x.DeviceStateServiceTimeout).HasColumnName(@"DeviceStateServiceTimeout").HasColumnType("int").IsRequired();
            Property(x => x.IsRunTransactionService).HasColumnName(@"IsRunTransactionService").HasColumnType("bit").IsRequired();
            Property(x => x.TransactionServiceLink).HasColumnName(@"TransactionServiceLink").HasColumnType("nvarchar").IsOptional().HasMaxLength(256);
            Property(x => x.TransactionServiceTimeout).HasColumnName(@"TransactionServiceTimeout").HasColumnType("int").IsRequired();
            Property(x => x.IsRunBalanceTransactionService).HasColumnName(@"IsRunBalanceTransactionService").HasColumnType("bit").IsRequired();
            Property(x => x.BalanceTransactionServiceLink).HasColumnName(@"BalanceTransactionServiceLink").HasColumnType("nvarchar").IsOptional().HasMaxLength(256);
            Property(x => x.BalanceTransactionServiceTimeout).HasColumnName(@"BalanceTransactionServiceTimeout").HasColumnType("int").IsRequired();
            Property(x => x.DateTransactionServiceSuccessfulRun).HasColumnName(@"DateTransactionServiceSuccessfulRun").HasColumnType("datetime").IsOptional();
            Property(x => x.CollectionServiceLink).HasColumnName(@"CollectionServiceLink").HasColumnType("nvarchar").IsOptional().HasMaxLength(256);
            Property(x => x.DateCollectionServiceSuccessfulRun).HasColumnName(@"DateCollectionServiceSuccessfulRun").HasColumnType("datetime").IsOptional();
        }
    }

}
// </auto-generated>