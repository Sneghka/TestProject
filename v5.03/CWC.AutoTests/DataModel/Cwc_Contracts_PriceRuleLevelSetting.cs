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

    // Cwc_Contracts_PriceRuleLevelSetting
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Contracts_PriceRuleLevelSetting
    {
        public int id { get; set; } // id (Primary key)
        public int PriceRule { get; set; } // PriceRule
        public int LevelName { get; set; } // LevelName
        public int LevelValueType { get; set; } // LevelValueType
        public bool IsRangeLevel { get; set; } // IsRangeLevel
        public int SequenceNumber { get; set; } // SequenceNumber
        public bool IsEnabled { get; set; } // IsEnabled

        public Cwc_Contracts_PriceRuleLevelSetting()
        {
            IsRangeLevel = false;
            IsEnabled = true;
        }
    }

}
// </auto-generated>
