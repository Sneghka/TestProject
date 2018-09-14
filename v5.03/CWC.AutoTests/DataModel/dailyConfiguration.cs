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

    // daily
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class dailyConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<daily>
    {
        public dailyConfiguration()
            : this("dbo")
        {
        }

        public dailyConfiguration(string schema)
        {
            ToTable("daily", schema);
            HasKey(x => x.UID);

            Property(x => x.compl_lg).HasColumnName(@"compl_lg").HasColumnType("bit").IsOptional();
            Property(x => x.conf_lg).HasColumnName(@"conf_lg").HasColumnType("bit").IsOptional();
            Property(x => x.dai_date).HasColumnName(@"dai_date").HasColumnType("datetime").IsRequired();
            Property(x => x.day_nr).HasColumnName(@"day_nr").HasColumnType("int").IsRequired();
            Property(x => x.on_rt_lg).HasColumnName(@"on_rt_lg").HasColumnType("bit").IsOptional();
            Property(x => x.truck_nr).HasColumnName(@"truck_nr").HasColumnType("decimal").IsOptional().HasPrecision(26,8);
            Property(x => x.s_km).HasColumnName(@"s_km").HasColumnType("decimal").IsOptional().HasPrecision(26,8);
            Property(x => x.e_km).HasColumnName(@"e_km").HasColumnType("decimal").IsOptional().HasPrecision(26,8);
            Property(x => x.t_km).HasColumnName(@"t_km").HasColumnType("decimal").IsOptional().HasPrecision(26,8);
            Property(x => x.p_km).HasColumnName(@"p_km").HasColumnType("decimal").IsOptional().HasPrecision(26,8);
            Property(x => x.a_s_date).HasColumnName(@"a_s_date").HasColumnType("datetime").IsOptional();
            Property(x => x.a_s_time).HasColumnName(@"a_s_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.tib_date).HasColumnName(@"tib_date").HasColumnType("datetime").IsOptional();
            Property(x => x.tib_time).HasColumnName(@"tib_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.logon_date).HasColumnName(@"logon_date").HasColumnType("datetime").IsOptional();
            Property(x => x.logon_time).HasColumnName(@"logon_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.a_e_date).HasColumnName(@"a_e_date").HasColumnType("datetime").IsOptional();
            Property(x => x.a_e_time).HasColumnName(@"a_e_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.a_d_time).HasColumnName(@"a_d_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.p_e_date).HasColumnName(@"p_e_date").HasColumnType("datetime").IsOptional();
            Property(x => x.p_d_time).HasColumnName(@"p_d_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.p_e_time).HasColumnName(@"p_e_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.p_s_date).HasColumnName(@"p_s_date").HasColumnType("datetime").IsOptional();
            Property(x => x.p_s_time).HasColumnName(@"p_s_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.upld_date).HasColumnName(@"upld_date").HasColumnType("datetime").IsOptional();
            Property(x => x.upld_time).HasColumnName(@"upld_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.dwnl_date).HasColumnName(@"dwnl_date").HasColumnType("datetime").IsOptional();
            Property(x => x.dwnl_time).HasColumnName(@"dwnl_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.gen_date).HasColumnName(@"gen_date").HasColumnType("datetime").IsOptional();
            Property(x => x.gen_time).HasColumnName(@"gen_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.vcard_nr).HasColumnName(@"vcard_nr").HasColumnType("nvarchar").IsOptional().HasMaxLength(25);
            Property(x => x.dai_rt_txt).HasColumnName(@"dai_rt_txt").HasColumnType("nvarchar").IsOptional().HasMaxLength(500);
            Property(x => x.mast_cd).HasColumnName(@"mast_cd").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.orgdepot_nr).HasColumnName(@"orgdepot_nr").HasColumnType("int").IsOptional();
            Property(x => x.subdepot_nr).HasColumnName(@"subdepot_nr").HasColumnType("int").IsOptional();
            Property(x => x.branch_cd).HasColumnName(@"branch_cd").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.CitCode).HasColumnName(@"CitCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(100);
        }
    }

}
// </auto-generated>
