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

    // Cwc_Billing_BillingLine
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Billing_BillingLine
    {
        public int id { get; set; } // id (Primary key)
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime DateBilled { get; set; } // DateBilled
        public decimal Value { get; set; } // Value
        public int? ContractRevisionNumber { get; set; } // ContractRevisionNumber
        public decimal ValueSurcharge { get; set; } // ValueSurcharge
        public decimal Units { get; set; } // Units
        public string Comment { get; set; } // Comment (length: 1073741823)
        public System.DateTime? PeriodBilledFrom { get; set; } // PeriodBilledFrom
        public System.DateTime? PeriodBilledTo { get; set; } // PeriodBilledTo
        public int? ContractID { get; set; } // ContractID
        public int? CounterpartyID { get; set; } // CounterpartyID
        public bool IsManual { get; set; } // IsManual
        public System.DateTime? DateUpdated { get; set; } // DateUpdated
        public int? AuthorID { get; set; } // AuthorID
        public int? EditorID { get; set; } // EditorID
        public int? PriceRule { get; set; } // PriceRule
        public int? UnitOfMeasure { get; set; } // UnitOfMeasure
        public decimal ValueRegularPrice { get; set; } // ValueRegularPrice
        public decimal ValueRangePrice { get; set; } // ValueRangePrice
        public decimal ValueSurchargePercentage { get; set; } // ValueSurchargePercentage
        public int? BankAccountID { get; set; } // BankAccountID
        public int? PriceLineID { get; set; } // PriceLineID
        public int? SurchargeFixedPriceLineID { get; set; } // SurchargeFixedPriceLineID
        public int? SurchargePercentagePriceLineID { get; set; } // SurchargePercentagePriceLineID
        public int CustomerID { get; set; } // CustomerID
        public int? LocationID { get; set; } // LocationID
        public int? DebtorID { get; set; } // DebtorID
        public int? BilledCaseID { get; set; } // BilledCaseID
        public int? VisitAddressID { get; set; } // VisitAddressID

        public Cwc_Billing_BillingLine()
        {
            Value = 0m;
            ValueSurcharge = 0m;
            Units = 0m;
            IsManual = false;
            ValueRegularPrice = 0m;
            ValueRangePrice = 0m;
            ValueSurchargePercentage = 0m;
        }
    }

}
// </auto-generated>