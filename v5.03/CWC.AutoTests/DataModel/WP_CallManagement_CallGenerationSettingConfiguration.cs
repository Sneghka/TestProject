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

    // WP_CallManagement_CallGenerationSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CallManagement_CallGenerationSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CallManagement_CallGenerationSetting>
    {
        public WP_CallManagement_CallGenerationSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_CallManagement_CallGenerationSettingConfiguration(string schema)
        {
            ToTable("WP_CallManagement_CallGenerationSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.MachineOwner_id).HasColumnName(@"MachineOwner_id").HasColumnType("decimal").IsOptional().HasPrecision(18,0);
            Property(x => x.MachineType_id).HasColumnName(@"MachineType_id").HasColumnType("int").IsOptional();
            Property(x => x.MachineModel_id).HasColumnName(@"MachineModel_id").HasColumnType("int").IsOptional();
            Property(x => x.StatusCode_id).HasColumnName(@"StatusCode_id").HasColumnType("int").IsOptional();
            Property(x => x.SubstatusCode).HasColumnName(@"SubstatusCode").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.CallCategory_id).HasColumnName(@"CallCategory_id").HasColumnType("int").IsOptional();
            Property(x => x.Requestor_id).HasColumnName(@"Requestor_id").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>