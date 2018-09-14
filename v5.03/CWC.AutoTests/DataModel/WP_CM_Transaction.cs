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

    // WP_CM_Transaction
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CM_Transaction
    {
        public int id { get; set; } // id (Primary key)
        public int? Type { get; set; } // Type
        public int? Number { get; set; } // Number
        public System.DateTime? StartDate { get; set; } // StartDate
        public System.DateTime? EndDate { get; set; } // EndDate
        public string BankNumber { get; set; } // BankNumber (length: 255)
        public string AccountNumber { get; set; } // AccountNumber (length: 255)
        public decimal? Value { get; set; } // Value
        public decimal? Change { get; set; } // Change
        public decimal? Fee { get; set; } // Fee
        public decimal? Weight { get; set; } // Weight
        public int? Quantity { get; set; } // Quantity
        public int? PaymentMethod { get; set; } // PaymentMethod
        public int? Machine_id { get; set; } // Machine_id
        public string CurrencyCode { get; set; } // CurrencyCode (length: 3)
        public bool Replenishment { get; set; } // Replenishment
        public string Bic { get; set; } // Bic (length: 20)
        public string SealNumber { get; set; } // SealNumber (length: 100)
        public System.DateTime DateCreated { get; set; } // DateCreated
        public string StaffNumber { get; set; } // StaffNumber (length: 255)
        public string SecondStaffNumber { get; set; } // SecondStaffNumber (length: 255)
        public bool IsBackdated { get; set; } // IsBackdated
        public bool IsProcessBackdated { get; set; } // IsProcessBackdated
        public bool IsUseBackdated { get; set; } // IsUseBackdated
        public string Comment { get; set; } // Comment (length: 255)

        public WP_CM_Transaction()
        {
            Replenishment = false;
            IsBackdated = false;
            IsProcessBackdated = false;
            IsUseBackdated = false;
        }
    }

}
// </auto-generated>
