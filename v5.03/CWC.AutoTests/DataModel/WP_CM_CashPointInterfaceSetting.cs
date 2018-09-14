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

    // WP_CM_CashPointInterfaceSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_CM_CashPointInterfaceSetting
    {
        public int id { get; set; } // id (Primary key)
        public bool? IsBusy { get; set; } // IsBusy
        public bool? IsExport { get; set; } // IsExport
        public int? ExportDayOfMonth { get; set; } // ExportDayOfMonth
        public double? ExportTime { get; set; } // ExportTime
        public string FilePrefix { get; set; } // FilePrefix (length: 255)
        public string LocationNamesSubstrings { get; set; } // LocationNamesSubstrings (length: 1000)
        public string ExportFolder { get; set; } // ExportFolder (length: 255)
        public int VersionNumber { get; set; } // VersionNumber

        public WP_CM_CashPointInterfaceSetting()
        {
            VersionNumber = 1;
        }
    }

}
// </auto-generated>
