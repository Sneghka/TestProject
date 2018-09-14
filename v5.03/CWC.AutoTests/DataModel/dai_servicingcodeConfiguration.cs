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

    // dai_servicingcode
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class dai_servicingcodeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<dai_servicingcode>
    {
        public dai_servicingcodeConfiguration()
            : this("dbo")
        {
        }

        public dai_servicingcodeConfiguration(string schema)
        {
            ToTable("dai_servicingcode", schema);
            HasKey(x => x.UID);

            Property(x => x.dai_date).HasColumnName(@"dai_date").HasColumnType("datetime").IsRequired();
            Property(x => x.day_nr).HasColumnName(@"day_nr").HasColumnType("int").IsRequired();
            Property(x => x.dai_date_a).HasColumnName(@"dai_date_a").HasColumnType("datetime").IsRequired();
            Property(x => x.a_time).HasColumnName(@"a_time").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
            Property(x => x.servCode).HasColumnName(@"servCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(8);
            Property(x => x.s_time).HasColumnName(@"s_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.e_time).HasColumnName(@"e_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.branch_cd).HasColumnName(@"branch_cd").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.mast_cd).HasColumnName(@"mast_cd").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }
    }

}
// </auto-generated>
