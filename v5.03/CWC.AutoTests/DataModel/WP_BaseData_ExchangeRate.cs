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

    // WP_BaseData_ExchangeRate
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_ExchangeRate
    {
        public int ID { get; set; } // ID (Primary key)
        public decimal Rate { get; set; } // Rate
        public System.DateTime ExchangeDate { get; set; } // ExchangeDate
        public System.DateTime DateCreated { get; set; } // DateCreated
        public bool IsActive { get; set; } // IsActive
        public System.DateTime? DateDeactivated { get; set; } // DateDeactivated
        public string CurrencyFromID { get; set; } // CurrencyFromID (length: 3)
        public string CurrencyToID { get; set; } // CurrencyToID (length: 3)
        public int? AuthorID { get; set; } // AuthorID
        public int? ReplacedByID { get; set; } // ReplacedByID
        public System.Guid UID { get; set; } // UID

        public WP_BaseData_ExchangeRate()
        {
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
