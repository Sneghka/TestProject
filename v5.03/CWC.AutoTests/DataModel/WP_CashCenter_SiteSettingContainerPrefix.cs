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

    // WP_CashCenter_SiteSettingContainerPrefix
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_SiteSettingContainerPrefix
    {
        public int id { get; set; } // id (Primary key)
        public string ContainerPrefix { get; set; } // ContainerPrefix (length: 30)
        public bool IsAutostartCounting { get; set; } // IsAutostartCounting
        public int CashCenterSiteSettingID { get; set; } // CashCenterSiteSettingID
        public decimal? LocationSubstitutionID { get; set; } // LocationSubstitutionID
        public System.Guid UID { get; set; } // UID

        public WP_CashCenter_SiteSettingContainerPrefix()
        {
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>