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

    // Cwc_Transport_TransportOrder
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Transport_TransportOrderConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cwc_Transport_TransportOrder>
    {
        public Cwc_Transport_TransportOrderConfiguration()
            : this("dbo")
        {
        }

        public Cwc_Transport_TransportOrderConfiguration(string schema)
        {
            ToTable("Cwc_Transport_TransportOrder", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
            Property(x => x.ReferenceCode).HasColumnName(@"ReferenceCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(30);
            Property(x => x.OrderType).HasColumnName(@"OrderType").HasColumnType("int").IsRequired();
            Property(x => x.TransportDate).HasColumnName(@"TransportDate").HasColumnType("datetime").IsRequired();
            Property(x => x.ServiceDate).HasColumnName(@"ServiceDate").HasColumnType("datetime").IsOptional();
            Property(x => x.Status).HasColumnName(@"Status").HasColumnType("int").IsRequired();
            Property(x => x.IsWithException).HasColumnName(@"IsWithException").HasColumnType("bit").IsRequired();
            Property(x => x.IsPdaAdHoc).HasColumnName(@"IsPdaAdHoc").HasColumnType("bit").IsRequired();
            Property(x => x.MasterRouteCode).HasColumnName(@"MasterRouteCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.MasterRouteDate).HasColumnName(@"MasterRouteDate").HasColumnType("datetime").IsOptional();
            Property(x => x.StopArrivalTime).HasColumnName(@"StopArrivalTime").HasColumnType("time").IsOptional();
            Property(x => x.VisitSequence).HasColumnName(@"VisitSequence").HasColumnType("int").IsOptional();
            Property(x => x.IsBillable).HasColumnName(@"IsBillable").HasColumnType("bit").IsRequired();
            Property(x => x.ServiceTypeID).HasColumnName(@"ServiceTypeID").HasColumnType("int").IsRequired();
            Property(x => x.SiteID).HasColumnName(@"SiteID").HasColumnType("int").IsRequired();
            Property(x => x.LocationID).HasColumnName(@"LocationID").HasColumnType("numeric").IsOptional().HasPrecision(15,0);
            Property(x => x.LocationPickUpCode).HasColumnName(@"LocationPickUpCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(64);
            Property(x => x.ServiceOrderID).HasColumnName(@"ServiceOrderID").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(255);
            Property(x => x.PDAReasonCodeID).HasColumnName(@"PDAReasonCodeID").HasColumnType("int").IsOptional();
            Property(x => x.DateCreated).HasColumnName(@"DateCreated").HasColumnType("datetime").IsRequired();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsRequired();
            Property(x => x.EditorID).HasColumnName(@"EditorID").HasColumnType("int").IsOptional();
            Property(x => x.AuthorID).HasColumnName(@"AuthorID").HasColumnType("int").IsOptional();
            Property(x => x.CollectServiceTypeId).HasColumnName(@"CollectServiceTypeId").HasColumnType("int").IsOptional();
            Property(x => x.DeliveryServiceTypeId).HasColumnName(@"DeliveryServiceTypeId").HasColumnType("int").IsOptional();
            Property(x => x.ServicingServiceTypeId).HasColumnName(@"ServicingServiceTypeId").HasColumnType("int").IsOptional();
            Property(x => x.IsOnHold).HasColumnName(@"IsOnHold").HasColumnType("bit").IsRequired();
            Property(x => x.DatePuttedOnHold).HasColumnName(@"DatePuttedOnHold").HasColumnType("datetime").IsOptional();
            Property(x => x.IsNewTransportOrderRequired).HasColumnName(@"IsNewTransportOrderRequired").HasColumnType("bit").IsRequired();
            Property(x => x.OldTransportOrderCode).HasColumnName(@"OldTransportOrderCode").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.CitReference).HasColumnName(@"CitReference").HasColumnType("nvarchar").IsOptional().HasMaxLength(30);
        }
    }

}
// </auto-generated>