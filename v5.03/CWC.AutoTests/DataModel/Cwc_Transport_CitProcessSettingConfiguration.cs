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

    // Cwc_Transport_CitProcessSetting
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Transport_CitProcessSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Transport_CitProcessSetting>
    {
        public Cwc_Transport_CitProcessSettingConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Transport_CitProcessSettingConfiguration(string schema)
        {
            ToTable("Cwc_Transport_CitProcessSetting", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsRequired();
            Property(x => x.AuthorID).HasColumnName(@"AuthorID").HasColumnType("int").IsOptional();
            Property(x => x.EditorID).HasColumnName(@"EditorID").HasColumnType("int").IsOptional();
            Property(x => x.CustomerID).HasColumnName(@"CustomerID").HasColumnType("numeric").IsOptional().HasPrecision(15,0);
            Property(x => x.LocationTypeID).HasColumnName(@"LocationTypeID").HasColumnType("int").IsOptional();
            Property(x => x.ServicePointID).HasColumnName(@"ServicePointID").HasColumnType("numeric").IsOptional().HasPrecision(15,0);
            Property(x => x.BasicStopDuration).HasColumnName(@"BasicStopDuration").HasColumnType("int").IsRequired();
            Property(x => x.IsServicing24x7).HasColumnName(@"IsServicing24x7").HasColumnType("bit").IsRequired();
            Property(x => x.CashPointTypeID).HasColumnName(@"CashPointTypeID").HasColumnType("int").IsOptional();
            Property(x => x.VisitAddressID).HasColumnName(@"VisitAddressID").HasColumnType("numeric").IsOptional().HasPrecision(15,0);
            Property(x => x.IsNotifyUponReschedule).HasColumnName(@"IsNotifyUponReschedule").HasColumnType("bit").IsRequired();
            Property(x => x.OnHoldDaysNumber).HasColumnName(@"OnHoldDaysNumber").HasColumnType("int").IsOptional();
            Property(x => x.IsAdditionalDeliveryRequired).HasColumnName(@"IsAdditionalDeliveryRequired").HasColumnType("bit").IsRequired();
            Property(x => x.DefaultAdditionalDeliveryServiceTypeId).HasColumnName(@"DefaultAdditionalDeliveryServiceTypeId").HasColumnType("int").IsOptional();
            Property(x => x.IsDefault).HasColumnName(@"IsDefault").HasColumnType("bit").IsRequired();
        }
    }

}
// </auto-generated>