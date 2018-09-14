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

    // LocationType
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class LocationTypeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<LocationType>
    {
        public LocationTypeConfiguration()
            : this("dbo")
        {
        }

        public LocationTypeConfiguration(string schema)
        {
            ToTable("LocationType", schema);
            HasKey(x => x.id);

            Property(x => x.ltCode).HasColumnName(@"ltCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(16);
            Property(x => x.ltDesc).HasColumnName(@"ltDesc").HasColumnType("nvarchar").IsRequired().HasMaxLength(80);
            Property(x => x.prcatCode).HasColumnName(@"prcatCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(16);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Category).HasColumnName(@"Category").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>