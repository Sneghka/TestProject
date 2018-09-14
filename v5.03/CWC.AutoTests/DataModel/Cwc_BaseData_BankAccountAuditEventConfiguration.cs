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

    // Cwc_BaseData_BankAccountAuditEvent
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_BaseData_BankAccountAuditEventConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_BaseData_BankAccountAuditEvent>
    {
        public Cwc_BaseData_BankAccountAuditEventConfiguration()
            : this("dbo")
        {
        }

        public Cwc_BaseData_BankAccountAuditEventConfiguration(string schema)
        {
            ToTable("Cwc_BaseData_BankAccountAuditEvent", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.EventDate).HasColumnName(@"EventDate").HasColumnType("datetime").IsRequired();
            Property(x => x.EventType).HasColumnName(@"EventType").HasColumnType("int").IsRequired();
            Property(x => x.EventActionDescription).HasColumnName(@"EventActionDescription").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.BankAccountID).HasColumnName(@"BankAccountID").HasColumnType("int").IsRequired();
            Property(x => x.UserID).HasColumnName(@"UserID").HasColumnType("int").IsOptional();
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>