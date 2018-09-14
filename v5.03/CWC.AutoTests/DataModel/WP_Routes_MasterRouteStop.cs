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

    // WP_Routes_MasterRouteStop
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_Routes_MasterRouteStop
    {
        public int id { get; set; } // id (Primary key)
        public int SequenceNumber { get; set; } // SequenceNumber
        public double ArrivalTime { get; set; } // ArrivalTime
        public double OnSiteTime { get; set; } // OnSiteTime
        public double DepartureTime { get; set; } // DepartureTime
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime DateUpdated { get; set; } // DateUpdated
        public int MasterRoute_id { get; set; } // MasterRoute_id
        public decimal Location_id { get; set; } // Location_id
        public int? Author_id { get; set; } // Author_id
        public int? Editor_id { get; set; } // Editor_id
        public System.Guid UID { get; set; } // UID

        public WP_Routes_MasterRouteStop()
        {
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
