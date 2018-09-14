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

    // Cwc_Transport_CitProcessSettingDestinationConfiguration
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Transport_CitProcessSettingDestinationConfiguration
    {
        public int id { get; set; } // id (Primary key)
        public int Weekday { get; set; } // Weekday
        public int CitProcessSettingID { get; set; } // CitProcessSettingID
        public int ContainerTypeID { get; set; } // ContainerTypeID
        public decimal? OnwardLocationID { get; set; } // OnwardLocationID
        public int? ServiceTypeID { get; set; } // ServiceTypeID
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.DateTime DateUpdated { get; set; } // DateUpdated
        public int? AuthorID { get; set; } // AuthorID
        public int? EditorID { get; set; } // EditorID
        public string Type { get; set; } // Type (length: 30)

        public Cwc_Transport_CitProcessSettingDestinationConfiguration()
        {
            DateCreated = System.DateTime.Now;
            DateUpdated = System.DateTime.Now;
        }
    }

}
// </auto-generated>
