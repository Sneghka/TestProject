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

    // WP_CoinMachine
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CoinMachine
    {
        public int MachineId { get; set; } // MachineId (Primary key)
        public string Number { get; set; } // Number (length: 128)
        public string IpAddress { get; set; } // IpAddress (length: 50)
        public int? Type { get; set; } // Type
        public int MachineModelId { get; set; } // MachineModelId
        public int? Status { get; set; } // Status
        public string Comment { get; set; } // Comment
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime DateUpdated { get; set; } // DateUpdated
        public decimal LocationId { get; set; } // LocationId
        public decimal OwnerId { get; set; } // OwnerId
        public decimal SupplierId { get; set; } // SupplierId
        public int? AuthorId { get; set; } // AuthorId
        public int? EditorId { get; set; } // EditorId
        public int? Category { get; set; } // Category
        public string Name { get; set; } // Name (length: 50)
        public byte Optimization { get; set; } // Optimization
        public bool IndividualStockConfiguration { get; set; } // IndividualStockConfiguration
        public int? OrderOnTotal { get; set; } // OrderOnTotal
        public int RemainderOfStockPerCassette { get; set; } // RemainderOfStockPerCassette
        public bool OrderMandatory { get; set; } // OrderMandatory
        public int? ServiceTypeCollectDeliver { get; set; } // ServiceTypeCollectDeliver
        public int? ServiceTypeCollectOnly { get; set; } // ServiceTypeCollectOnly
        public bool ForceConfirmation { get; set; } // ForceConfirmation
        public int Replenishment { get; set; } // Replenishment
        public int AllowStockExpiration { get; set; } // AllowStockExpiration
        public System.DateTime? DateOrderCreationStart { get; set; } // DateOrderCreationStart
        public bool IsAllowManualTransactions { get; set; } // IsAllowManualTransactions

        public WP_CoinMachine()
        {
            Optimization = 0;
            IndividualStockConfiguration = false;
            OrderOnTotal = 0;
            RemainderOfStockPerCassette = 0;
            OrderMandatory = false;
            ForceConfirmation = false;
            Replenishment = 1;
            AllowStockExpiration = 1;
            IsAllowManualTransactions = false;
        }
    }

}
// </auto-generated>
