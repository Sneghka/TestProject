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

    // WP_PasmuntBestandInterfaceDenominations
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_PasmuntBestandInterfaceDenominationConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_PasmuntBestandInterfaceDenomination>
    {
        public WP_PasmuntBestandInterfaceDenominationConfiguration()
            : this("dbo")
        {
        }

        public WP_PasmuntBestandInterfaceDenominationConfiguration(string schema)
        {
            ToTable("WP_PasmuntBestandInterfaceDenominations", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Denomination).HasColumnName(@"Denomination").HasColumnType("decimal").IsRequired().HasPrecision(26,8);
            Property(x => x.ProductCode).HasColumnName(@"ProductCode").HasColumnType("nvarchar(max)").IsRequired();
            Property(x => x.JobInstanceSettings).HasColumnName(@"JobInstanceSettings").HasColumnType("int").IsRequired();
        }
    }

}
// </auto-generated>
