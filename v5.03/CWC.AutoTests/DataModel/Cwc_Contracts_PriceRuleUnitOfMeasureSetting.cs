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

    // Cwc_Contracts_PriceRuleUnitOfMeasureSetting
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Contracts_PriceRuleUnitOfMeasureSetting
    {
        public int id { get; set; } // id (Primary key)
        public int PriceRule { get; set; } // PriceRule
        public int UnitOfMeasure { get; set; } // UnitOfMeasure
        public string UserAssistanceMessage { get; set; } // UserAssistanceMessage (length: 225)
        public bool IsEnabled { get; set; } // IsEnabled

        public Cwc_Contracts_PriceRuleUnitOfMeasureSetting()
        {
            IsEnabled = true;
        }
    }

}
// </auto-generated>
