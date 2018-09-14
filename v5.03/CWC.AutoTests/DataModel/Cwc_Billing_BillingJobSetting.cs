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

    // Cwc_Billing_BillingJobSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Billing_BillingJobSetting
    {
        public int BillingDailyAtTime { get; set; } // BillingDailyAtTime (Primary key)
        public System.DateTime? LastBilledDate { get; set; } // LastBilledDate
        public bool IsAffectServicePerPeriodAllCustomers { get; set; } // IsAffectServicePerPeriodAllCustomers (Primary key)
        public System.DateTime? UserBillingDateTime { get; set; } // UserBillingDateTime
        public int id { get; set; } // id (Primary key)
        public bool IsSaveBillingLinesWithZeroValue { get; set; } // IsSaveBillingLinesWithZeroValue (Primary key)
        public bool IsJobBusy { get; set; } // IsJobBusy (Primary key)
        public bool IsBillCollectedTransportOrder { get; set; } // IsBillCollectedTransportOrder (Primary key)

        public Cwc_Billing_BillingJobSetting()
        {
            IsAffectServicePerPeriodAllCustomers = false;
            IsSaveBillingLinesWithZeroValue = false;
            IsJobBusy = false;
            IsBillCollectedTransportOrder = false;
        }
    }

}
// </auto-generated>
