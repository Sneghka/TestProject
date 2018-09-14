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

    // his_pack
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class his_packConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<his_pack>
    {
        public his_packConfiguration()
            : this("dbo")
        {
        }

        public his_packConfiguration(string schema)
        {
            ToTable("his_pack", schema);
            HasKey(x => x.id);

            Property(x => x.a_date).HasColumnName(@"a_date").HasColumnType("datetime").IsOptional();
            Property(x => x.a_time).HasColumnName(@"a_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.a_status).HasColumnName(@"a_status").HasColumnType("nvarchar").IsOptional().HasMaxLength(40);
            Property(x => x.fr_loc_nr).HasColumnName(@"fr_loc_nr").HasColumnType("decimal").IsOptional().HasPrecision(26,8);
            Property(x => x.to_loc_nr).HasColumnName(@"to_loc_nr").HasColumnType("decimal").IsOptional().HasPrecision(26,8);
            Property(x => x.user_cd).HasColumnName(@"user_cd").HasColumnType("nvarchar").IsOptional().HasMaxLength(16);
            Property(x => x.pack_val).HasColumnName(@"pack_val").HasColumnType("decimal").IsOptional().HasPrecision(26,8);
            Property(x => x.real_time).HasColumnName(@"real_time").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.dec_val).HasColumnName(@"dec_val").HasColumnType("decimal").IsOptional().HasPrecision(26,8);
            Property(x => x.bgtyp_nr).HasColumnName(@"bgtyp_nr").HasColumnType("int").IsOptional();
            Property(x => x.pack_nr).HasColumnName(@"pack_nr").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.old_pack_nr).HasColumnName(@"old_pack_nr").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.reason).HasColumnName(@"reason").HasColumnType("nvarchar").IsOptional().HasMaxLength(100);
            Property(x => x.mast_cd).HasColumnName(@"mast_cd").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.real_date).HasColumnName(@"real_date").HasColumnType("datetime").IsOptional();
            Property(x => x.remark).HasColumnName(@"remark").HasColumnType("nvarchar").IsOptional().HasMaxLength(504);
            Property(x => x.stat_desc).HasColumnName(@"stat_desc").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.fr_loc_cd).HasColumnName(@"fr_loc_cd").HasColumnType("nvarchar").IsOptional().HasMaxLength(64);
            Property(x => x.to_loc_cd).HasColumnName(@"to_loc_cd").HasColumnType("nvarchar").IsOptional().HasMaxLength(64);
            Property(x => x.branch_cd).HasColumnName(@"branch_cd").HasColumnType("nvarchar").IsOptional().HasMaxLength(10);
            Property(x => x.WP_order_id).HasColumnName(@"WP_order_id").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.WP_reported).HasColumnName(@"WP_reported").HasColumnType("bit").IsOptional();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.new_delivery_date).HasColumnName(@"new_delivery_date").HasColumnType("datetime").IsOptional();
            Property(x => x.IsRecollected).HasColumnName(@"IsRecollected").HasColumnType("bit").IsOptional();
            Property(x => x.CitIdentifier).HasColumnName(@"CitIdentifier").HasColumnType("nvarchar").IsOptional().HasMaxLength(100);
            Property(x => x.CitCode).HasColumnName(@"CitCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(100);
        }
    }

}
// </auto-generated>
