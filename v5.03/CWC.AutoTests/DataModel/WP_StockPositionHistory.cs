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

    // WP_StockPositionHistory
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_StockPositionHistory
    {
        public int id { get; set; } // id (Primary key)
        public int CassetteNumber { get; set; } // CassetteNumber
        public int Capacity { get; set; } // Capacity
        public int Quantity { get; set; } // Quantity
        public decimal Value { get; set; } // Value
        public decimal Weight { get; set; } // Weight
        public int Direction { get; set; } // Direction
        public byte Totals { get; set; } // Totals
        public byte? Indicator { get; set; } // Indicator
        public int? CollectIndicatorColor { get; set; } // CollectIndicatorColor
        public System.DateTime DateCreated { get; set; } // DateCreated
        public string MaterialID { get; set; } // MaterialID (length: 10)
        public string ProductCode { get; set; } // ProductCode (length: 10)
        public int Machine_id { get; set; } // Machine_id
        public bool IsGrandTotal { get; set; } // IsGrandTotal
        public string CurrencyID { get; set; } // CurrencyID (length: 3)
        public int? IssueIndicatorColor { get; set; } // IssueIndicatorColor
        public string CassetteExternalNumber { get; set; } // CassetteExternalNumber (length: 50)
        public bool IsMixed { get; set; } // IsMixed
        public string MaterialTypeCode { get; set; } // MaterialTypeCode (length: 10)
        public int? StockPositionID { get; set; } // StockPositionID
        public bool IsCounterfeits { get; set; } // IsCounterfeits

        public WP_StockPositionHistory()
        {
            CassetteNumber = 1;
            Capacity = 0;
            Quantity = 0;
            Value = 0m;
            Weight = 0m;
            Totals = 1;
            IsGrandTotal = false;
            IsMixed = false;
            IsCounterfeits = false;
        }
    }

}
// </auto-generated>
