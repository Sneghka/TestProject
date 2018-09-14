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

    // dai_line
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class dai_line
    {
        public string a_time { get; set; } // a_time (length: 10)
        public string a_time_a { get; set; } // a_time_a (length: 10)
        public int? amt_col_p { get; set; } // amt_col_p
        public int? amt_col_a { get; set; } // amt_col_a
        public int? amt_del { get; set; } // amt_del
        public int? amt_del_a { get; set; } // amt_del_a
        public System.DateTime dai_date { get; set; } // dai_date
        public int day_nr { get; set; } // day_nr
        public bool? inv_lg { get; set; } // inv_lg
        public decimal? loc_nr { get; set; } // loc_nr
        public decimal? lcrep_nr { get; set; } // lcrep_nr
        public decimal? ord_nr { get; set; } // ord_nr
        public string s_time { get; set; } // s_time (length: 10)
        public string s_time_a { get; set; } // s_time_a (length: 10)
        public int? amt_col_o { get; set; } // amt_col_o
        public int? amt_col_c { get; set; } // amt_col_c
        public string d_time { get; set; } // d_time (length: 10)
        public string d_time_a { get; set; } // d_time_a (length: 10)
        public int? reason_cd { get; set; } // reason_cd
        public string id_scan { get; set; } // id_scan (length: 10)
        public System.DateTime dai_date_a { get; set; } // dai_date_a
        public string user_cd_next { get; set; } // user_cd_next (length: 8)
        public System.DateTime? dai_date_next { get; set; } // dai_date_next
        public int? mast_nr_next { get; set; } // mast_nr_next
        public int? action_cd { get; set; } // action_cd
        public int? reason_cd_mc_p { get; set; } // reason_cd_mc_p
        public bool? loc_cd_typed { get; set; } // loc_cd_typed
        public string contact_name { get; set; } // contact_name (length: 30)
        public string contact_time { get; set; } // contact_time (length: 10)
        public string branch_cd { get; set; } // branch_cd (length: 10)
        public string mast_cd { get; set; } // mast_cd (length: 10)
        public string ref_loc_nr { get; set; } // ref_loc_nr (length: 64)
        public System.Guid UID { get; set; } // UID (Primary key via unique index idx_dai_line_UID)
        public int id { get; set; } // id
        public string CitIdentifier { get; set; } // CitIdentifier (length: 100)
        public string CitCode { get; set; } // CitCode (length: 100)
        public int? ReCollectReasonCode { get; set; } // ReCollectReasonCode

        public dai_line()
        {
            UID = System.Guid.NewGuid();
        }
    }

}
// </auto-generated>
