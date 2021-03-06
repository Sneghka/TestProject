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

    // Cwc_Transport_TransportOrderIntegrationJobReplenishmentQueue
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Transport_TransportOrderIntegrationJobReplenishmentQueue
    {
        public int id { get; set; } // id (Primary key)
        public string OrderID { get; set; } // OrderID (length: 255)
        public int Entity { get; set; } // Entity
        public string FileName { get; set; } // FileName (length: 255)
        public byte[] File { get; set; } // File
        public int Action { get; set; } // Action
        public int? SkipNumber { get; set; } // SkipNumber
        public int? TransportOrderID { get; set; } // TransportOrderID
        public System.DateTime? DateCreated { get; set; } // DateCreated
        public int? Status { get; set; } // Status

        public Cwc_Transport_TransportOrderIntegrationJobReplenishmentQueue()
        {
            SkipNumber = 0;
        }
    }

}
// </auto-generated>
