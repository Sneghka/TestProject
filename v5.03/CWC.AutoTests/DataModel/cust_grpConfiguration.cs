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

    // cust_grp
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class cust_grpConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<cust_grp>
    {
        public cust_grpConfiguration()
            : this("dbo")
        {
        }

        public cust_grpConfiguration(string schema)
        {
            ToTable("cust_grp", schema);
            HasKey(x => x.csgrp_cd);

            Property(x => x.csgrp_cd).HasColumnName(@"csgrp_cd").HasColumnType("nvarchar").IsRequired().HasMaxLength(6).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.descript).HasColumnName(@"descript").HasColumnType("nvarchar").IsRequired().HasMaxLength(60);
        }
    }

}
// </auto-generated>