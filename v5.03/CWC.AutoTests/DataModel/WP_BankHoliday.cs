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

    // WP_BankHoliday
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BankHoliday
    {
        public int id { get; set; } // id (Primary key)
        public System.DateTime Date { get; set; } // Date
        public string Description { get; set; } // Description (length: 255)
        public int BankingHolidaySettingID { get; set; } // BankingHolidaySettingID
        public System.Guid UID { get; set; } // UID

        public WP_BankHoliday()
        {
            BankingHolidaySettingID = 1;
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
