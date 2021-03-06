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

    // Cwc_Ordering_OrderExportJobLog
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Ordering_OrderExportJobLogConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Ordering_OrderExportJobLog>
    {
        public Cwc_Ordering_OrderExportJobLogConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Ordering_OrderExportJobLogConfiguration(string schema)
        {
            ToTable("Cwc_Ordering_OrderExportJobLog", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.Result).HasColumnName(@"Result").HasColumnType("int").IsRequired();
            Property(x => x.Message).HasColumnName(@"Message").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.ResultingFileName).HasColumnName(@"ResultingFileName").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.ExportDate).HasColumnName(@"ExportDate").HasColumnType("datetime").IsOptional();
        }
    }

}
// </auto-generated>
