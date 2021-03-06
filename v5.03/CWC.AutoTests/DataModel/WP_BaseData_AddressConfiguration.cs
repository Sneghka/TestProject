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

    // WP_BaseData_Address
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_AddressConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_BaseData_Address>
    {
        public WP_BaseData_AddressConfiguration()
            : this("dbo")
        {
        }

        public WP_BaseData_AddressConfiguration(string schema)
        {
            ToTable("WP_BaseData_Address", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Street).HasColumnName(@"Street").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(80);
            Property(x => x.PostalCode).HasColumnName(@"PostalCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.City).HasColumnName(@"City").HasColumnType("nvarchar").IsRequired().HasMaxLength(60);
            Property(x => x.State).HasColumnName(@"State").HasColumnType("nvarchar").IsOptional().HasMaxLength(80);
            Property(x => x.Country).HasColumnName(@"Country").HasColumnType("nvarchar").IsOptional().HasMaxLength(80);
            Property(x => x.ExtraAddressInfo).HasColumnName(@"ExtraAddressInfo").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.Purpose).HasColumnName(@"Purpose").HasColumnType("int").IsOptional();
            Property(x => x.ObjectID).HasColumnName(@"ObjectID").HasColumnType("decimal").IsRequired().HasPrecision(18,0);
            Property(x => x.ObjectClassID).HasColumnName(@"ObjectClassID").HasColumnType("varchar(max)").IsRequired().IsUnicode(false);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>
