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

    // Cwc_Transport_CitProcessSettingReplenishmentConfiguration
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Transport_CitProcessSettingReplenishmentConfigurationConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Transport_CitProcessSettingReplenishmentConfiguration>
    {
        public Cwc_Transport_CitProcessSettingReplenishmentConfigurationConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Transport_CitProcessSettingReplenishmentConfigurationConfiguration(string schema)
        {
            ToTable("Cwc_Transport_CitProcessSettingReplenishmentConfiguration", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.CitProcessSettingID).HasColumnName(@"CitProcessSettingID").HasColumnType("int").IsRequired();
            Property(x => x.ReplenishmentTypeID).HasColumnName(@"ReplenishmentTypeID").HasColumnType("int").IsRequired();
            Property(x => x.CollectionTypeID).HasColumnName(@"CollectionTypeID").HasColumnType("int").IsRequired();
            Property(x => x.DeliveryTypeID).HasColumnName(@"DeliveryTypeID").HasColumnType("int").IsRequired();
            Property(x => x.ServicingTypeID).HasColumnName(@"ServicingTypeID").HasColumnType("int").IsOptional();
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsRequired();
            Property(x => x.AuthorID).HasColumnName(@"AuthorID").HasColumnType("int").IsOptional();
            Property(x => x.EditorID).HasColumnName(@"EditorID").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
