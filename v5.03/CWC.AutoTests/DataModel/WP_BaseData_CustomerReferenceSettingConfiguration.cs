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

    // WP_BaseData_CustomerReferenceSetting
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_CustomerReferenceSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_BaseData_CustomerReferenceSetting>
    {
        public WP_BaseData_CustomerReferenceSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_BaseData_CustomerReferenceSettingConfiguration(string schema)
        {
            ToTable("WP_BaseData_CustomerReferenceSetting", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.UIName).HasColumnName(@"UIName").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
            Property(x => x.Type).HasColumnName(@"Type").HasColumnType("int").IsRequired();
            Property(x => x.Mask).HasColumnName(@"Mask").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.CustomerID).HasColumnName(@"CustomerID").HasColumnType("numeric").IsRequired().HasPrecision(15,0);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.CodeFormatID).HasColumnName(@"CodeFormatID").HasColumnType("int").IsOptional();
            Property(x => x.IsMandatory).HasColumnName(@"IsMandatory").HasColumnType("bit").IsRequired();
            Property(x => x.PseudoValue).HasColumnName(@"PseudoValue").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
        }
    }

}
// </auto-generated>
