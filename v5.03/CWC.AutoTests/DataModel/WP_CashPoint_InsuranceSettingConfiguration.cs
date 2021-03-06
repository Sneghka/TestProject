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

    // WP_CashPoint_InsuranceSetting
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashPoint_InsuranceSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CashPoint_InsuranceSetting>
    {
        public WP_CashPoint_InsuranceSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_CashPoint_InsuranceSettingConfiguration(string schema)
        {
            ToTable("WP_CashPoint_InsuranceSetting", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.InsuranceLimit).HasColumnName(@"InsuranceLimit").HasColumnType("decimal").IsRequired().HasPrecision(15,2);
            Property(x => x.IsDefault).HasColumnName(@"IsDefault").HasColumnType("bit").IsRequired();
            Property(x => x.Level).HasColumnName(@"Level").HasColumnType("tinyint").IsRequired();
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsRequired();
            Property(x => x.CompanyId).HasColumnName(@"CompanyId").HasColumnType("int").IsOptional();
            Property(x => x.CashPointModelId).HasColumnName(@"CashPointModelId").HasColumnType("int").IsOptional();
            Property(x => x.CashPointId).HasColumnName(@"CashPointId").HasColumnType("int").IsOptional();
            Property(x => x.ParentInsuranceSettingId).HasColumnName(@"ParentInsuranceSettingId").HasColumnType("int").IsOptional();
            Property(x => x.AuthorId).HasColumnName(@"AuthorId").HasColumnType("int").IsOptional();
            Property(x => x.EditorId).HasColumnName(@"EditorId").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
