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

    // Cwc_Ordering_AutoReleaseJobSettings
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Ordering_AutoReleaseJobSetting
    {
        public int id { get; set; } // id (Primary key)
        public bool IsUnconfirmed { get; set; } // IsUnconfirmed
        public bool IsConfirmed { get; set; } // IsConfirmed
        public bool IsReleaseAtOnce { get; set; } // IsReleaseAtOnce

        public Cwc_Ordering_AutoReleaseJobSetting()
        {
            IsReleaseAtOnce = false;
        }
    }

}
// </auto-generated>
