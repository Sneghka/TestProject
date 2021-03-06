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

    // WP_CashCenter_ContainersBatch
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_ContainersBatch
    {
        public long id { get; set; } // id (Primary key)
        public int Status { get; set; } // Status
        public System.DateTime DateCreated { get; set; } // DateCreated
        public string Number { get; set; } // Number (length: 30)
        public string Description { get; set; } // Description
        public int Site_id { get; set; } // Site_id
        public int CreationMethod { get; set; } // CreationMethod
        public int Stream_id { get; set; } // Stream_id
        public int? Author_id { get; set; } // Author_id
        public int? Editor_id { get; set; } // Editor_id
        public System.DateTime DateUpdated { get; set; } // DateUpdated
        public int? StockOwnerID { get; set; } // StockOwnerID
        public int? AssignedToID { get; set; } // AssignedToID
        public System.Guid UID { get; set; } // UID
        public bool IsCrossCheckRequired { get; set; } // IsCrossCheckRequired

        public WP_CashCenter_ContainersBatch()
        {
            CreationMethod = 0;
            DateUpdated = System.DateTime.Now;
            UID = System.Guid.NewGuid();
            IsCrossCheckRequired = false;
        }
    }

}
// </auto-generated>
