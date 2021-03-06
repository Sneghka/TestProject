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

    // WP_BaseData_ActiveDirectoryImportIntegrationLog
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_ActiveDirectoryImportIntegrationLogConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_BaseData_ActiveDirectoryImportIntegrationLog>
    {
        public WP_BaseData_ActiveDirectoryImportIntegrationLogConfiguration()
            : this("dbo")
        {
        }

        public WP_BaseData_ActiveDirectoryImportIntegrationLogConfiguration(string schema)
        {
            ToTable("WP_BaseData_ActiveDirectoryImportIntegrationLog", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.Result).HasColumnName(@"Result").HasColumnType("int").IsRequired();
            Property(x => x.Message).HasColumnName(@"Message").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.Path).HasColumnName(@"Path").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.Domain).HasColumnName(@"Domain").HasColumnType("nvarchar").IsOptional().HasMaxLength(100);
            Property(x => x.ADGroup).HasColumnName(@"ADGroup").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.CWCGroup).HasColumnName(@"CWCGroup").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
        }
    }

}
// </auto-generated>
