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

    // WP_CashCenter_StockPosition
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_StockPosition
    {
        public long id { get; set; } // id (Primary key)
        public bool IsVerified { get; set; } // IsVerified
        public int QualificationType { get; set; } // QualificationType
        public int Status { get; set; } // Status
        public int Quantity { get; set; } // Quantity
        public decimal Value { get; set; } // Value
        public decimal Weight { get; set; } // Weight
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime DateUpdated { get; set; } // DateUpdated
        public string Material_id { get; set; } // Material_id (length: 10)
        public string Product_id { get; set; } // Product_id (length: 10)
        public long? StockContainer_id { get; set; } // StockContainer_id
        public int? StockLocation_id { get; set; } // StockLocation_id
        public string Currency_id { get; set; } // Currency_id (length: 3)
        public bool IsTotal { get; set; } // IsTotal
        public int? StockOwner_id { get; set; } // StockOwner_id
        public int? Author_id { get; set; } // Author_id
        public int? Editor_id { get; set; } // Editor_id
        public System.Guid UID { get; set; } // UID
        public System.DateTime? TransactionDate { get; set; } // TransactionDate

        public WP_CashCenter_StockPosition()
        {
            IsVerified = false;
            IsTotal = false;
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
