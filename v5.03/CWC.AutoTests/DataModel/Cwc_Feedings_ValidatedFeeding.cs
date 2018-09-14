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

    // Cwc_Feedings_ValidatedFeeding
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Cwc_Feedings_ValidatedFeeding
    {
        public int id { get; set; } // id (Primary key)
        public System.DateTime FeedingDate { get; set; } // FeedingDate
        public int ValidationResult { get; set; } // ValidationResult
        public bool Warning { get; set; } // Warning
        public string Content { get; set; } // Content (length: 1073741823)
        public string ErrorMessage { get; set; } // ErrorMessage (length: 1073741823)
        public int? User_id { get; set; } // User_id
        public int DeliveryMethod { get; set; } // DeliveryMethod
        public string Login { get; set; } // Login (length: 255)
        public string Password { get; set; } // Password
        public string Hash { get; set; } // Hash
        public string IPAddress { get; set; } // IPAddress (length: 255)
        public string FileName { get; set; } // FileName (length: 255)
        public string EntityType { get; set; } // EntityType (length: 255)
        public string ResponseContent { get; set; } // ResponseContent (length: 1073741823)
        public int? SenderID { get; set; } // SenderID
        public int? MapperID { get; set; } // MapperID
    }

}
// </auto-generated>
