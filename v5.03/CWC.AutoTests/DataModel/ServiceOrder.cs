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

    // ServiceOrder
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class ServiceOrder
    {
        public string Order_ID { get; set; } // Order_ID (length: 255)
        public decimal? Cus_nr { get; set; } // Cus_nr
        public System.DateTime? Service_Date { get; set; } // Service_Date
        public int? Order_Type { get; set; } // Order_Type
        public int? Order_Level { get; set; } // Order_Level
        public string Cancel_Reason { get; set; } // Cancel_Reason (length: 100)
        public string Reference_ID { get; set; } // Reference_ID (length: 30)
        public long? WP_Order_ID { get; set; } // WP_Order_ID
        public string WP_Comments { get; set; } // WP_Comments
        public System.DateTime? WP_DateCreated { get; set; } // WP_DateCreated
        public System.DateTime? WP_DateUpdated { get; set; } // WP_DateUpdated
        public int? WP_Author_id { get; set; } // WP_Author_id
        public int? WP_Editor_id { get; set; } // WP_Editor_id
        public string WP_Email { get; set; } // WP_Email (length: 255)
        public decimal? WP_OrderedValue { get; set; } // WP_OrderedValue
        public decimal? WP_PreannouncedValue { get; set; } // WP_PreannouncedValue
        public string WP_ref_loc_nr { get; set; } // WP_ref_loc_nr (length: 64)
        public string WP_mast_cd { get; set; } // WP_mast_cd (length: 255)
        public string WP_branch_cd { get; set; } // WP_branch_cd (length: 255)
        public string WP_CurrencyCode { get; set; } // WP_CurrencyCode (length: 3)
        public string WP_ServiceType_Code { get; set; } // WP_ServiceType_Code (length: 255)
        public decimal? WP_loc_nr { get; set; } // WP_loc_nr
        public string WP_PickUp_ref_loc_nr { get; set; } // WP_PickUp_ref_loc_nr (length: 64)
        public decimal? WP_SpecialCoinsValue { get; set; } // WP_SpecialCoinsValue
        public int? WP_GenericStatus { get; set; } // WP_GenericStatus
        public bool WP_WithException { get; set; } // WP_WithException (Primary key)
        public System.DateTime? WP_NewServiceDate { get; set; } // WP_NewServiceDate
        public bool WP_Optimized { get; set; } // WP_Optimized (Primary key)
        public System.DateTime? WP_ReleaseDeadline { get; set; } // WP_ReleaseDeadline
        public decimal? WP_TransOptimizerOrder_id { get; set; } // WP_TransOptimizerOrder_id
        public System.Guid UID { get; set; } // UID (Primary key)
        public int? reason_cd { get; set; } // reason_cd
        public decimal? WP_OrderedWeight { get; set; } // WP_OrderedWeight
        public System.DateTime? WP_CollectPrecreditingDate { get; set; } // WP_CollectPrecreditingDate
        public System.DateTime? WP_DateCompleted { get; set; } // WP_DateCompleted
        public string WP_BankReference { get; set; } // WP_BankReference (length: 30)
        public string WP_CitReference { get; set; } // WP_CitReference (length: 30)
        public int id { get; set; } // id (Primary key)
        public string SecondCustomerReference { get; set; } // SecondCustomerReference (length: 30)
        public int? MasterRouteID { get; set; } // MasterRouteID

        public ServiceOrder()
        {
            WP_WithException = false;
            WP_Optimized = false;
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
