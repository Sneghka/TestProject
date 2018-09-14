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

    // Cwc_Transport_PackageLifeCycleProcessingJobLog
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Transport_PackageLifeCycleProcessingJobLogConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Transport_PackageLifeCycleProcessingJobLog>
    {
        public Cwc_Transport_PackageLifeCycleProcessingJobLogConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Transport_PackageLifeCycleProcessingJobLogConfiguration(string schema)
        {
            ToTable("Cwc_Transport_PackageLifeCycleProcessingJobLog", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Result).HasColumnName(@"Result").HasColumnType("int").IsRequired();
            Property(x => x.Action).HasColumnName(@"Action").HasColumnType("int").IsRequired();
            Property(x => x.Message).HasColumnName(@"Message").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.PackageLifeCycleId).HasColumnName(@"PackageLifeCycleId").HasColumnType("int").IsRequired();
            Property(x => x.TransportOrderOldId).HasColumnName(@"TransportOrderOldId").HasColumnType("int").IsOptional();
            Property(x => x.TransportOrderNewId).HasColumnName(@"TransportOrderNewId").HasColumnType("int").IsOptional();
            Property(x => x.ServiceOrderNewId).HasColumnName(@"ServiceOrderNewId").HasColumnType("int").IsOptional();
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsOptional();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsOptional();
            Property(x => x.AuthorID).HasColumnName(@"AuthorID").HasColumnType("int").IsOptional();
            Property(x => x.EditorID).HasColumnName(@"EditorID").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>