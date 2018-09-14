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

    // WP_CashCenter_StockContainer
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_StockContainer
    {
        public long id { get; set; } // id (Primary key)
        public string Number { get; set; } // Number (length: 100)
        public int? Type { get; set; } // Type
        public int? PreannouncementType { get; set; } // PreannouncementType
        public System.DateTime? ServiceDate { get; set; } // ServiceDate
        public int? Status { get; set; } // Status
        public int TotalQuantity { get; set; } // TotalQuantity
        public decimal TotalValue { get; set; } // TotalValue
        public decimal TotalWeight { get; set; } // TotalWeight
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime DateUpdated { get; set; } // DateUpdated
        public long? StockOrder_id { get; set; } // StockOrder_id
        public long? ParentContainer_id { get; set; } // ParentContainer_id
        public int? DestinationStockLocation_id { get; set; } // DestinationStockLocation_id
        public decimal? LocationFrom_id { get; set; } // LocationFrom_id
        public decimal? LocationTo_id { get; set; } // LocationTo_id
        public long? ContainersBatch_id { get; set; } // ContainersBatch_id
        public bool IsExpected { get; set; } // IsExpected
        public int? StockLocation_id { get; set; } // StockLocation_id
        public bool IsOpen { get; set; } // IsOpen
        public int? Author_id { get; set; } // Author_id
        public int? Editor_id { get; set; } // Editor_id
        public int? DeclaredTotalInners { get; set; } // DeclaredTotalInners
        public int? ActualTotalInners { get; set; } // ActualTotalInners
        public bool IsMissing { get; set; } // IsMissing
        public long? PickListNumber { get; set; } // PickListNumber
        public int? BagTypeID { get; set; } // BagTypeID
        public long? OrdersBatch { get; set; } // OrdersBatch
        public bool IsMisscan { get; set; } // IsMisscan
        public System.DateTime? DateCollected { get; set; } // DateCollected
        public string CustomerReference1 { get; set; } // CustomerReference1
        public string CustomerReference2 { get; set; } // CustomerReference2
        public string AutomateID { get; set; } // AutomateID
        public string TillID { get; set; } // TillID
        public string PermanentNumber { get; set; } // PermanentNumber (length: 100)
        public bool IsMultiEnvelope { get; set; } // IsMultiEnvelope
        public System.Guid UID { get; set; } // UID
        public bool IsPreCrediting { get; set; } // IsPreCrediting
        public bool IsCancelled { get; set; } // IsCancelled
        public bool IsForDispatching { get; set; } // IsForDispatching
        public bool IsDetonated { get; set; } // IsDetonated
        public string SecondNumber { get; set; } // SecondNumber (length: 100)
        public bool IsOnHold { get; set; } // IsOnHold
        public long? CapturingBatch_id { get; set; } // CapturingBatch_id
        public System.DateTime? TransactionDate { get; set; } // TransactionDate
        public string BankAccountNumber { get; set; } // BankAccountNumber (length: 255)
        public bool IsDestroyed { get; set; } // IsDestroyed
        public int? BankAccountID { get; set; } // BankAccountID
        public string ServiceOrderID { get; set; } // ServiceOrderID (length: 256)
        public int? MasterRouteID { get; set; } // MasterRouteID
        public long? DispatchOrderID { get; set; } // DispatchOrderID

        public WP_CashCenter_StockContainer()
        {
            IsExpected = true;
            IsOpen = false;
            IsMissing = false;
            IsMisscan = false;
            IsMultiEnvelope = false;
            UID = System.Guid.NewGuid();
            IsPreCrediting = false;
            IsCancelled = false;
            IsForDispatching = false;
            IsDetonated = false;
            IsOnHold = false;
            IsDestroyed = false;
        }
    }

}
// </auto-generated>
