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

    // WP_CoinMachineType
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CoinMachineTypeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CoinMachineType>
    {
        public WP_CoinMachineTypeConfiguration()
            : this("dbo")
        {
        }

        public WP_CoinMachineTypeConfiguration(string schema)
        {
            ToTable("WP_CoinMachineType", schema);
            HasKey(x => x.CoinMachineTypeId);

            Property(x => x.CoinMachineTypeId).HasColumnName(@"CoinMachineTypeId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar").IsOptional().HasMaxLength(250);
            Property(x => x.IsCollect).HasColumnName(@"IsCollect").HasColumnType("bit").IsRequired();
            Property(x => x.IsIssue).HasColumnName(@"IsIssue").HasColumnType("bit").IsRequired();
            Property(x => x.IsRecycle).HasColumnName(@"IsRecycle").HasColumnType("bit").IsRequired();
            Property(x => x.HandlingType).HasColumnName(@"HandlingType").HasColumnType("nvarchar").IsRequired().HasMaxLength(6);
            Property(x => x.Number).HasColumnName(@"Number").HasColumnType("int").IsRequired();
            Property(x => x.IsUseInOptimization).HasColumnName(@"IsUseInOptimization").HasColumnType("bit").IsRequired();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>
