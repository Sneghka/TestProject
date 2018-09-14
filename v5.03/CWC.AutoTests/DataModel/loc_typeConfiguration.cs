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

    // loc_type
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class loc_typeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<loc_type>
    {
        public loc_typeConfiguration()
            : this("dbo")
        {
        }

        public loc_typeConfiguration(string schema)
        {
            ToTable("loc_type", schema);
            HasKey(x => x.lctyp_cd);

            Property(x => x.descript).HasColumnName(@"descript").HasColumnType("nvarchar").IsRequired().HasMaxLength(60);
            Property(x => x.lctyp_cd).HasColumnName(@"lctyp_cd").HasColumnType("nvarchar").IsRequired().HasMaxLength(6).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.locat_lg).HasColumnName(@"locat_lg").HasColumnType("bit").IsOptional();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>