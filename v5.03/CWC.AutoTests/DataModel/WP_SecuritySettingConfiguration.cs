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

    // WP_SecuritySettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_SecuritySettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_SecuritySetting>
    {
        public WP_SecuritySettingConfiguration()
            : this("dbo")
        {
        }

        public WP_SecuritySettingConfiguration(string schema)
        {
            ToTable("WP_SecuritySettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.PasswordChange).HasColumnName(@"PasswordChange").HasColumnType("bit").IsRequired();
            Property(x => x.IsPasswordDuration).HasColumnName(@"IsPasswordDuration").HasColumnType("bit").IsRequired();
            Property(x => x.PasswordDuration).HasColumnName(@"PasswordDuration").HasColumnType("int").IsRequired();
            Property(x => x.IsPasswordLength).HasColumnName(@"IsPasswordLength").HasColumnType("bit").IsRequired();
            Property(x => x.PasswordLength).HasColumnName(@"PasswordLength").HasColumnType("int").IsRequired();
            Property(x => x.PasswordSymbols).HasColumnName(@"PasswordSymbols").HasColumnType("bit").IsRequired();
            Property(x => x.IsPasswordRepeat).HasColumnName(@"IsPasswordRepeat").HasColumnType("bit").IsRequired();
            Property(x => x.PasswordRepeat).HasColumnName(@"PasswordRepeat").HasColumnType("int").IsRequired();
            Property(x => x.IsAccessLimit).HasColumnName(@"IsAccessLimit").HasColumnType("bit").IsRequired();
            Property(x => x.AccesssLimit).HasColumnName(@"AccesssLimit").HasColumnType("int").IsRequired();
            Property(x => x.AccessLockout).HasColumnName(@"AccessLockout").HasColumnType("int").IsRequired();
            Property(x => x.AccessTimeout).HasColumnName(@"AccessTimeout").HasColumnType("int").IsRequired();
            Property(x => x.Safe).HasColumnName(@"Safe").HasColumnType("int").IsRequired();
            Property(x => x.ModificationDate).HasColumnName(@"ModificationDate").HasColumnType("datetime").IsRequired();
            Property(x => x.Author_id).HasColumnName(@"Author_id").HasColumnType("int").IsRequired();
            Property(x => x.IsShowRequestProcessingTime).HasColumnName(@"IsShowRequestProcessingTime").HasColumnType("bit").IsRequired();
            Property(x => x.IsSpecialSymbols).HasColumnName(@"IsSpecialSymbols").HasColumnType("bit").IsRequired();
        }
    }

}
// </auto-generated>