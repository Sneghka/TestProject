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

    // WP_CashCenter_ProcessSetting
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_ProcessSetting
    {
        public int Id { get; set; } // Id (Primary key)
        public bool Default { get; set; } // Default
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime DateUpdated { get; set; } // DateUpdated
        public int ConfirmationUnpacking { get; set; } // ConfirmationUnpacking
        public bool IsCapturingAllowed { get; set; } // IsCapturingAllowed
        public bool IsCaptureTotals { get; set; } // IsCaptureTotals
        public bool IsCaptureWeight { get; set; } // IsCaptureWeight
        public bool IsForceBulkCount { get; set; } // IsForceBulkCount
        public int? AuthorID { get; set; } // AuthorID
        public int? EditorId { get; set; } // EditorId
        public decimal? CustomerId { get; set; } // CustomerId
        public string LocationTypeCode { get; set; } // LocationTypeCode (length: 16)
        public decimal? LocationId { get; set; } // LocationId
        public bool IsApplyStockUnit { get; set; } // IsApplyStockUnit
        public int AllocationMethod { get; set; } // AllocationMethod
        public bool IsReconcileMotherDeposit { get; set; } // IsReconcileMotherDeposit
        public bool IsElectronicPreannouncement { get; set; } // IsElectronicPreannouncement
        public int? AnomalyCodeID { get; set; } // AnomalyCodeID
        public bool IsMotherDepositOnly { get; set; } // IsMotherDepositOnly
        public bool IsSeparateCapturing { get; set; } // IsSeparateCapturing
        public bool IsDeclaredValueMandatory { get; set; } // IsDeclaredValueMandatory
        public bool IsDeclaredValuesCounting { get; set; } // IsDeclaredValuesCounting
        public bool IsCaptureChequeDetails { get; set; } // IsCaptureChequeDetails
        public int? PackingBarcodeFormatID { get; set; } // PackingBarcodeFormatID
        public bool IsBankAccountLeading { get; set; } // IsBankAccountLeading
        public bool IsCountInnersDirectly { get; set; } // IsCountInnersDirectly
        public bool IsShowReferencesCounting { get; set; } // IsShowReferencesCounting
        public bool IsShowInnersCounting { get; set; } // IsShowInnersCounting
        public bool IsDeclaredValueMandatoryCapturing { get; set; } // IsDeclaredValueMandatoryCapturing
        public System.Guid UID { get; set; } // UID
        public bool IsAutomaticallyConfirmCapturing { get; set; } // IsAutomaticallyConfirmCapturing
        public bool IsShowCustomerReference { get; set; } // IsShowCustomerReference
        public bool IsShowBankReference { get; set; } // IsShowBankReference
        public bool IsShowCitReference { get; set; } // IsShowCitReference
        public string CustomerReferenceText { get; set; } // CustomerReferenceText (length: 12)
        public bool IsValidateBankAccount { get; set; } // IsValidateBankAccount
        public bool IsShowHolderName { get; set; } // IsShowHolderName
        public bool IsValidateBankAccountCounting { get; set; } // IsValidateBankAccountCounting
        public bool IsShowHolderNameCounting { get; set; } // IsShowHolderNameCounting
        public bool IsBankAccountLeadingCounting { get; set; } // IsBankAccountLeadingCounting
        public bool IsDisableInnersLocationFrom { get; set; } // IsDisableInnersLocationFrom
        public int? CashPointTypeID { get; set; } // CashPointTypeID
        public int? VisitAddressID { get; set; } // VisitAddressID
        public bool? IsLooseProductsPacking { get; set; } // IsLooseProductsPacking

        public WP_CashCenter_ProcessSetting()
        {
            Default = false;
            DateCreated = System.DateTime.Now;
            DateUpdated = System.DateTime.Now;
            IsCapturingAllowed = true;
            IsCaptureTotals = true;
            IsCaptureWeight = true;
            IsForceBulkCount = false;
            IsApplyStockUnit = false;
            AllocationMethod = 0;
            IsReconcileMotherDeposit = false;
            IsElectronicPreannouncement = false;
            IsMotherDepositOnly = false;
            IsSeparateCapturing = false;
            IsDeclaredValueMandatory = false;
            IsDeclaredValuesCounting = true;
            IsCaptureChequeDetails = false;
            IsBankAccountLeading = true;
            IsCountInnersDirectly = false;
            IsShowReferencesCounting = true;
            IsShowInnersCounting = true;
            IsDeclaredValueMandatoryCapturing = false;
            UID = System.Guid.NewGuid();
            IsAutomaticallyConfirmCapturing = false;
            IsShowCustomerReference = false;
            IsShowBankReference = false;
            IsShowCitReference = false;
            IsValidateBankAccount = false;
            IsShowHolderName = false;
            IsValidateBankAccountCounting = false;
            IsShowHolderNameCounting = false;
            IsBankAccountLeadingCounting = false;
            IsDisableInnersLocationFrom = false;
        }
    }

}
// </auto-generated>
