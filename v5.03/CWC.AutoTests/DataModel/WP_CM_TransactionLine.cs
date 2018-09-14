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

    // WP_CM_TransactionLine
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CM_TransactionLine
    {
        public int id { get; set; } // id (Primary key)
        public int? Type { get; set; } // Type
        public int? Quantity { get; set; } // Quantity
        public decimal? Value { get; set; } // Value
        public decimal? Weight { get; set; } // Weight
        public string MaterialID { get; set; } // MaterialID (length: 10)
        public string ProductCode { get; set; } // ProductCode (length: 10)
        public int? Transaction_id { get; set; } // Transaction_id
        public int CassetteNumber { get; set; } // CassetteNumber
        public int? ResidualQuantity { get; set; } // ResidualQuantity
        public int StockPositionDirection { get; set; } // StockPositionDirection
        public string CassetteExternalNumber { get; set; } // CassetteExternalNumber (length: 50)
        public bool IsMixed { get; set; } // IsMixed
        public string MaterialTypeCode { get; set; } // MaterialTypeCode (length: 10)
        public string CurrencyCode { get; set; } // CurrencyCode (length: 3)
        public int? StockPositionID { get; set; } // StockPositionID
        public string SealNumber { get; set; } // SealNumber (length: 100)

        public WP_CM_TransactionLine()
        {
            CassetteNumber = 1;
            IsMixed = false;
        }
    }

}
// </auto-generated>
