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

    // WP_PasmuntBestandInterfaceJobInstanceSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_PasmuntBestandInterfaceJobInstanceSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_PasmuntBestandInterfaceJobInstanceSetting>
    {
        public WP_PasmuntBestandInterfaceJobInstanceSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_PasmuntBestandInterfaceJobInstanceSettingConfiguration(string schema)
        {
            ToTable("WP_PasmuntBestandInterfaceJobInstanceSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.GetFilesFolder).HasColumnName(@"GetFilesFolder").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.PutFilesFolder).HasColumnName(@"PutFilesFolder").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.ServiceType).HasColumnName(@"ServiceType").HasColumnType("int").IsRequired();
            Property(x => x.TakeFirsCharacters).HasColumnName(@"TakeFirsCharacters").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
