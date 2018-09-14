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

    // WP_BaseData_ProductionMachineProductLink
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_ProductionMachineProductLinkConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_BaseData_ProductionMachineProductLink>
    {
        public WP_BaseData_ProductionMachineProductLinkConfiguration()
            : this("dbo")
        {
        }

        public WP_BaseData_ProductionMachineProductLinkConfiguration(string schema)
        {
            ToTable("WP_BaseData_ProductionMachineProductLink", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ProductionMachineID).HasColumnName(@"ProductionMachineID").HasColumnType("int").IsRequired();
            Property(x => x.ProductCode).HasColumnName(@"ProductCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>