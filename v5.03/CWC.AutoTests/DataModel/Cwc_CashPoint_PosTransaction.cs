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

    // Cwc_CashPoint_PosTransaction
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_CashPoint_PosTransaction
    {
        public int id { get; set; } // id (Primary key)
        public string StaffNumber { get; set; } // StaffNumber (length: 255)
        public int Direction { get; set; } // Direction
        public decimal Value { get; set; } // Value
        public System.DateTime TransactionDate { get; set; } // TransactionDate
        public int AutomateID { get; set; } // AutomateID
        public decimal LocationID { get; set; } // LocationID
        public string CurrencyID { get; set; } // CurrencyID (length: 3)
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime DateUpdated { get; set; } // DateUpdated
        public int? AuthorID { get; set; } // AuthorID
        public int? EditorID { get; set; } // EditorID
    }

}
// </auto-generated>
