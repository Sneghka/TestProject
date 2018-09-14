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

    // WP_BaseData_Automate
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_AutomateConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_BaseData_Automate>
    {
        public WP_BaseData_AutomateConfiguration()
            : this("dbo")
        {
        }

        public WP_BaseData_AutomateConfiguration(string schema)
        {
            ToTable("WP_BaseData_Automate", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.Type).HasColumnName(@"Type").HasColumnType("int").IsRequired();
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
            Property(x => x.IsEnabled).HasColumnName(@"IsEnabled").HasColumnType("bit").IsRequired();
            Property(x => x.LocationID).HasColumnName(@"LocationID").HasColumnType("numeric").IsRequired().HasPrecision(15,0);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
        }
    }

}
// </auto-generated>