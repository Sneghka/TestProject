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

    // WP_Security_ActionVisibilitySettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Security_ActionVisibilitySettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Security_ActionVisibilitySetting>
    {
        public WP_Security_ActionVisibilitySettingConfiguration()
            : this("dbo")
        {
        }

        public WP_Security_ActionVisibilitySettingConfiguration(string schema)
        {
            ToTable("WP_Security_ActionVisibilitySettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
            Property(x => x.IsVisible).HasColumnName(@"IsVisible").HasColumnType("bit").IsRequired();
            Property(x => x.InternalName).HasColumnName(@"InternalName").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
        }
    }

}
// </auto-generated>