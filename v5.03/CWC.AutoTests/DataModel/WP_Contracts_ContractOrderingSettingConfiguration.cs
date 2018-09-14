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

    // WP_Contracts_ContractOrderingSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Contracts_ContractOrderingSettingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<WP_Contracts_ContractOrderingSetting>
    {
        public WP_Contracts_ContractOrderingSettingConfiguration()
            : this("dbo")
        {
        }

        public WP_Contracts_ContractOrderingSettingConfiguration(string schema)
        {
            ToTable("WP_Contracts_ContractOrderingSettings", schema);
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName(@"id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.IsLatestRevision).HasColumnName(@"IsLatestRevision").HasColumnType("bit").IsRequired();
            Property(x => x.RevisionNumber).HasColumnName(@"RevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.RevisionDate).HasColumnName(@"RevisionDate").HasColumnType("datetime").IsOptional();
            Property(x => x.ReplacedRevisionNumber).HasColumnName(@"ReplacedRevisionNumber").HasColumnType("int").IsOptional();
            Property(x => x.ReplacedRevisionDate).HasColumnName(@"ReplacedRevisionDate").HasColumnType("datetime").IsOptional();
            Property(x => x.Comments).HasColumnName(@"Comments").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.Author_id).HasColumnName(@"Author_id").HasColumnType("int").IsOptional();
            Property(x => x.LatestRevision_id).HasColumnName(@"LatestRevision_id").HasColumnType("int").IsOptional();
            Property(x => x.LeadTime).HasColumnName(@"LeadTime").HasColumnType("int").IsRequired();
            Property(x => x.Period).HasColumnName(@"Period").HasColumnType("int").IsRequired();
            Property(x => x.CutOffTime).HasColumnName(@"CutOffTime").HasColumnType("float").IsRequired();
            Property(x => x.MaxValue).HasColumnName(@"MaxValue").HasColumnType("decimal").IsOptional().HasPrecision(18,2);
            Property(x => x.AllowTotalPreannouncement).HasColumnName(@"AllowTotalPreannouncement").HasColumnType("bit").IsRequired();
            Property(x => x.IsNotes).HasColumnName(@"IsNotes").HasColumnType("bit").IsRequired();
            Property(x => x.IsCoins).HasColumnName(@"IsCoins").HasColumnType("bit").IsRequired();
            Property(x => x.IsConsumables).HasColumnName(@"IsConsumables").HasColumnType("bit").IsRequired();
            Property(x => x.IsSpecialCoins).HasColumnName(@"IsSpecialCoins").HasColumnType("bit").IsRequired();
            Property(x => x.IsServicingCodes).HasColumnName(@"IsServicingCodes").HasColumnType("bit").IsRequired();
            Property(x => x.IsPreAnnouncement).HasColumnName(@"IsPreAnnouncement").HasColumnType("bit").IsRequired();
            Property(x => x.AllowReference).HasColumnName(@"AllowReference").HasColumnType("bit").IsRequired();
            Property(x => x.AllowComments).HasColumnName(@"AllowComments").HasColumnType("bit").IsRequired();
            Property(x => x.AskForAnother).HasColumnName(@"AskForAnother").HasColumnType("bit").IsRequired();
            Property(x => x.IsRelease).HasColumnName(@"IsRelease").HasColumnType("bit").IsRequired();
            Property(x => x.EnableProducts).HasColumnName(@"EnableProducts").HasColumnType("int").IsRequired();
            Property(x => x.Contract_id).HasColumnName(@"Contract_id").HasColumnType("int").IsRequired();
            Property(x => x.ServiceType_id).HasColumnName(@"ServiceType_id").HasColumnType("int").IsRequired();
            Property(x => x.LocationType_code).HasColumnName(@"LocationType_code").HasColumnType("nvarchar").IsOptional().HasMaxLength(16);
            Property(x => x.Location_id).HasColumnName(@"Location_id").HasColumnType("numeric").IsOptional().HasPrecision(15,0);
            Property(x => x.UID).HasColumnName(@"UID").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.AllowBankReference).HasColumnName(@"AllowBankReference").HasColumnType("bit").IsRequired();
            Property(x => x.AllowCitReference).HasColumnName(@"AllowCitReference").HasColumnType("bit").IsRequired();
            Property(x => x.CCLeadTime).HasColumnName(@"CCLeadTime").HasColumnType("int").IsRequired();
            Property(x => x.CITLeadTime).HasColumnName(@"CITLeadTime").HasColumnType("int").IsRequired();
            Property(x => x.CashPointTypeID).HasColumnName(@"CashPointTypeID").HasColumnType("int").IsOptional();
            Property(x => x.AllowSecondCustomer).HasColumnName(@"AllowSecondCustomer").HasColumnType("bit").IsRequired();
            Property(x => x.IsNotesLooseProductDelivery).HasColumnName(@"IsNotesLooseProductDelivery").HasColumnType("bit").IsRequired();
            Property(x => x.IsCoinsLooseProductDelivery).HasColumnName(@"IsCoinsLooseProductDelivery").HasColumnType("bit").IsRequired();
            Property(x => x.EnableLooseProducts).HasColumnName(@"EnableLooseProducts").HasColumnType("int").IsRequired();
            Property(x => x.RegionID).HasColumnName(@"RegionID").HasColumnType("int").IsOptional();
        }
    }

}
// </auto-generated>
