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

    // WP_DynamicsIntegrationCustomerReferenceSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_DynamicsIntegrationCustomerReferenceSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_DynamicsIntegrationCustomerReferenceSetting>
    {
        public WP_DynamicsIntegrationCustomerReferenceSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_DynamicsIntegrationCustomerReferenceSettingConfiguration(string schema)
        {
            ToTable("WP_DynamicsIntegrationCustomerReferenceSettings", schema);
            HasKey(x => x.Type);

            Property(x => x.Type).HasColumnName(@"Type").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.UIName).HasColumnName(@"UIName").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
        }
    }

}
// </auto-generated>
