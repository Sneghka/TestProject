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

    // WP_OracleInvoiceExportJobSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_OracleInvoiceExportJobSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_OracleInvoiceExportJobSetting>
    {
        public WP_OracleInvoiceExportJobSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_OracleInvoiceExportJobSettingConfiguration(string schema)
        {
            ToTable("WP_OracleInvoiceExportJobSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ExecutionTime).HasColumnName(@"ExecutionTime").HasColumnType("time").IsRequired();
            Property(x => x.IsExport).HasColumnName(@"IsExport").HasColumnType("bit").IsRequired();
            Property(x => x.ServerHost).HasColumnName(@"ServerHost").HasColumnType("nvarchar").IsRequired().HasMaxLength(256);
            Property(x => x.ServerPort).HasColumnName(@"ServerPort").HasColumnType("int").IsRequired();
            Property(x => x.UserLogin).HasColumnName(@"UserLogin").HasColumnType("nvarchar").IsRequired().HasMaxLength(256);
            Property(x => x.UserPassword).HasColumnName(@"UserPassword").HasColumnType("varbinary").IsRequired().HasMaxLength(32);
            Property(x => x.DatabaseHost).HasColumnName(@"DatabaseHost").HasColumnType("nvarchar").IsRequired().HasMaxLength(256);
            Property(x => x.DatabasePort).HasColumnName(@"DatabasePort").HasColumnType("int").IsRequired();
            Property(x => x.DatabaseName).HasColumnName(@"DatabaseName").HasColumnType("nvarchar").IsRequired().HasMaxLength(256);
        }
    }

}
// </auto-generated>
