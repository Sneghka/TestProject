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

    // WP_OpenTimeDates
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_OpenTimeDateConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_OpenTimeDate>
    {
        public WP_OpenTimeDateConfiguration()
            : this("dbo")
        {
        }

        public WP_OpenTimeDateConfiguration(string schema)
        {
            ToTable("WP_OpenTimeDates", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.OpenTimeSettingsId).HasColumnName(@"OpenTimeSettingsId").HasColumnType("int").IsRequired();
            Property(x => x.TimeFrom).HasColumnName(@"TimeFrom").HasColumnType("int").IsOptional();
            Property(x => x.TimeTo).HasColumnName(@"TimeTo").HasColumnType("int").IsOptional();
            Property(x => x.WeekDay).HasColumnName(@"WeekDay").HasColumnType("tinyint").IsRequired();
            Property(x => x.IntervalType).HasColumnName(@"IntervalType").HasColumnType("int").IsRequired();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>
