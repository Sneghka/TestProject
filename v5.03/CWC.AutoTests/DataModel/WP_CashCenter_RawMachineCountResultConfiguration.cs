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

    // WP_CashCenter_RawMachineCountResult
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_RawMachineCountResultConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CashCenter_RawMachineCountResult>
    {
        public WP_CashCenter_RawMachineCountResultConfiguration()
            : this("dbo")
        {
        }

        public WP_CashCenter_RawMachineCountResultConfiguration(string schema)
        {
            ToTable("WP_CashCenter_RawMachineCountResult", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.OperatorID).HasColumnName(@"OperatorID").HasColumnType("int").IsRequired();
            Property(x => x.WorkstationID).HasColumnName(@"WorkstationID").HasColumnType("int").IsRequired();
            Property(x => x.StockContainerID).HasColumnName(@"StockContainerID").HasColumnType("bigint").IsRequired();
            Property(x => x.StockOwnerID).HasColumnName(@"StockOwnerID").HasColumnType("int").IsRequired();
            Property(x => x.Status).HasColumnName(@"Status").HasColumnType("int").IsRequired();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsRequired();
        }
    }

}
// </auto-generated>