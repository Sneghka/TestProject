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

    // WP_ConfigureStockPositionSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_ConfigureStockPositionSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_ConfigureStockPositionSetting>
    {
        public WP_ConfigureStockPositionSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_ConfigureStockPositionSettingConfiguration(string schema)
        {
            ToTable("WP_ConfigureStockPositionSettings", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("int").IsRequired();
            Property(x => x.MachineTypeId).HasColumnName(@"MachineTypeId").HasColumnType("int").IsOptional();
            Property(x => x.MachineModelId).HasColumnName(@"MachineModelId").HasColumnType("int").IsOptional();
            Property(x => x.TabIndex).HasColumnName(@"TabIndex").HasColumnType("int").IsOptional();
            Property(x => x.MachineId).HasColumnName(@"MachineId").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
