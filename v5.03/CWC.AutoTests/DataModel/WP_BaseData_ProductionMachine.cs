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

    // WP_BaseData_ProductionMachine
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_BaseData_ProductionMachine
    {
        public int id { get; set; } // id (Primary key)
        public string Number { get; set; } // Number (length: 30)
        public int Group { get; set; } // Group
        public int Status { get; set; } // Status
        public bool IsUnfitDetection { get; set; } // IsUnfitDetection
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime? DateUpdated { get; set; } // DateUpdated
        public int? ConnectionType { get; set; } // ConnectionType
        public string ComPort { get; set; } // ComPort (length: 20)
        public string BpsRate { get; set; } // BpsRate (length: 20)
        public string DataBits { get; set; } // DataBits (length: 20)
        public string StopBits { get; set; } // StopBits (length: 20)
        public int? Parity { get; set; } // Parity
        public string FilePath { get; set; } // FilePath (length: 255)
        public int AuthorID { get; set; } // AuthorID
        public int? EditorID { get; set; } // EditorID
        public int? WorkstationID { get; set; } // WorkstationID
        public string Description { get; set; } // Description
        public System.Guid UID { get; set; } // UID
        public string URL { get; set; } // URL (length: 1024)
        public bool IsAutostartCounting { get; set; } // IsAutostartCounting
        public bool IsSendEndCount { get; set; } // IsSendEndCount

        public WP_BaseData_ProductionMachine()
        {
            Group = 0;
            Status = 0;
            ConnectionType = 0;
            Parity = 0;
            UID = System.Guid.NewGuid();
            IsAutostartCounting = false;
            IsSendEndCount = false;
        }
    }

}
// </auto-generated>
