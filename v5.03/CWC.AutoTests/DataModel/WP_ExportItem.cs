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

    // WP_ExportItem
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class WP_ExportItem
    {
        public int id { get; set; } // id (Primary key)
        public string FileName { get; set; } // FileName (length: 255)
        public byte[] FileData { get; set; } // FileData
        public string alias { get; set; } // alias (length: 255)
        public byte[] AdditionalInfo { get; set; } // AdditionalInfo
        public bool NeedEncryption { get; set; } // NeedEncryption

        public WP_ExportItem()
        {
            NeedEncryption = false;
        }
    }

}
// </auto-generated>
