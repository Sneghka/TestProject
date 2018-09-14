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

    // WP_CashCenter_Stream
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CashCenter_Stream
    {
        public int id { get; set; } // id (Primary key)
        public string Name { get; set; } // Name (length: 255)
        public string Description { get; set; } // Description
        public int StockOwner_id { get; set; } // StockOwner_id
        public int Site_id { get; set; } // Site_id
        public int Author_id { get; set; } // Author_id
        public int Editor_id { get; set; } // Editor_id
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime DateUpdated { get; set; } // DateUpdated
        public int Process { get; set; } // Process
        public System.Guid UID { get; set; } // UID
        public decimal? DestinationLocationID { get; set; } // DestinationLocationID
        public int? DestinationStockLocationID { get; set; } // DestinationStockLocationID

        public WP_CashCenter_Stream()
        {
            Author_id = 0;
            Editor_id = 0;
            DateCreated = System.DateTime.Now;
            DateUpdated = System.DateTime.Now;
            Process = 0;
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
