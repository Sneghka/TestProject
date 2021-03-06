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

    // WP_CashControl_JobServiceSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashControl_JobServiceSetting
    {
        public int id { get; set; } // id (Primary key)
        public bool IsRunDeviceStateService { get; set; } // IsRunDeviceStateService
        public string DeviceStateServiceLink { get; set; } // DeviceStateServiceLink (length: 256)
        public int DeviceStateServiceTimeout { get; set; } // DeviceStateServiceTimeout
        public bool IsRunTransactionService { get; set; } // IsRunTransactionService
        public string TransactionServiceLink { get; set; } // TransactionServiceLink (length: 256)
        public int TransactionServiceTimeout { get; set; } // TransactionServiceTimeout
        public bool IsRunBalanceTransactionService { get; set; } // IsRunBalanceTransactionService
        public string BalanceTransactionServiceLink { get; set; } // BalanceTransactionServiceLink (length: 256)
        public int BalanceTransactionServiceTimeout { get; set; } // BalanceTransactionServiceTimeout
        public System.DateTime? DateTransactionServiceSuccessfulRun { get; set; } // DateTransactionServiceSuccessfulRun
        public string CollectionServiceLink { get; set; } // CollectionServiceLink (length: 256)
        public System.DateTime? DateCollectionServiceSuccessfulRun { get; set; } // DateCollectionServiceSuccessfulRun

        public WP_CashControl_JobServiceSetting()
        {
            IsRunDeviceStateService = false;
            DeviceStateServiceTimeout = 100000;
            IsRunTransactionService = false;
            TransactionServiceTimeout = 100000;
            IsRunBalanceTransactionService = false;
            BalanceTransactionServiceTimeout = 100000;
            DateCollectionServiceSuccessfulRun = System.DateTime.Now;
        }
    }

}
// </auto-generated>
