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

    // WP_Contracts_LocationServiceCreationJobSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Contracts_LocationServiceCreationJobSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Contracts_LocationServiceCreationJobSetting>
    {
        public WP_Contracts_LocationServiceCreationJobSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_Contracts_LocationServiceCreationJobSettingConfiguration(string schema)
        {
            ToTable("WP_Contracts_LocationServiceCreationJobSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Time).HasColumnName(@"Time").HasColumnType("float").IsOptional();
            Property(x => x.LastGeneration).HasColumnName(@"LastGeneration").HasColumnType("datetime").IsOptional();
            Property(x => x.UserGeneration).HasColumnName(@"UserGeneration").HasColumnType("datetime").IsOptional();
        }
    }

}
// </auto-generated>