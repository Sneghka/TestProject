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

    // WP_Routes_MasterRouteMappingSetting
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Routes_MasterRouteMappingSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Routes_MasterRouteMappingSetting>
    {
        public WP_Routes_MasterRouteMappingSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_Routes_MasterRouteMappingSettingConfiguration(string schema)
        {
            ToTable("WP_Routes_MasterRouteMappingSetting", schema);
            HasKey(x => x.MasterRouteNumber);

            Property(x => x.MasterRouteNumberOld).HasColumnName(@"MasterRouteNumberOld").HasColumnType("int").IsRequired();
            Property(x => x.MasterRouteNumber).HasColumnName(@"MasterRouteNumber").HasColumnType("nvarchar").IsRequired().HasMaxLength(255).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
    }

}
// </auto-generated>
