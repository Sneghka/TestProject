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

    // WP_CM_StatusMessage
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CM_StatusMessageConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_CM_StatusMessage>
    {
        public WP_CM_StatusMessageConfiguration()
            : this("dbo")
        {
        }

        public WP_CM_StatusMessageConfiguration(string schema)
        {
            ToTable("WP_CM_StatusMessage", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Date).HasColumnName(@"Date").HasColumnType("datetime").IsOptional();
            Property(x => x.MessageNumber).HasColumnName(@"MessageNumber").HasColumnType("int").IsOptional();
            Property(x => x.StatusCode_id).HasColumnName(@"StatusCode_id").HasColumnType("int").IsOptional();
            Property(x => x.SubstatusCode).HasColumnName(@"SubstatusCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.Version).HasColumnName(@"Version").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.DeviceName).HasColumnName(@"DeviceName").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.DeviceID).HasColumnName(@"DeviceID").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.DeviceType).HasColumnName(@"DeviceType").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.DeviceVersion).HasColumnName(@"DeviceVersion").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.Manufacturer).HasColumnName(@"Manufacturer").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.Category).HasColumnName(@"Category").HasColumnType("int").IsOptional();
            Property(x => x.OrderId).HasColumnName(@"OrderId").HasColumnType("nvarchar").IsOptional().HasMaxLength(20);
            Property(x => x.Resolved).HasColumnName(@"Resolved").HasColumnType("int").IsRequired();
            Property(x => x.ResolveTime).HasColumnName(@"ResolveTime").HasColumnType("datetime").IsOptional();
            Property(x => x.DateResolved).HasColumnName(@"DateResolved").HasColumnType("datetime").IsOptional();
            Property(x => x.Machine_id).HasColumnName(@"Machine_id").HasColumnType("int").IsOptional();
            Property(x => x.TimeOffline).HasColumnName(@"TimeOffline").HasColumnType("float").IsOptional();
            Property(x => x.IsCallGenerated).HasColumnName(@"IsCallGenerated").HasColumnType("bit").IsOptional();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsRequired();
        }
    }

}
// </auto-generated>