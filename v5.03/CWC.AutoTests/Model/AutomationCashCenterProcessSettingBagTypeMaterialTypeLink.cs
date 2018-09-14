using Cwc.CashCenter;
using Cwc.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace CWC.AutoTests.Model
{
    [Table("WP_CashCenter_ProcessSettingBagTypeMaterialTypeLink", Schema = "dbo")]
    public class AutomationCashCenterProcessSettingBagTypeMaterialTypeLink : DataBaseObject
    {
        [PropertyDescription(IsNullable = false)]
        [Key]
        [Column(Order = 1)]
        public int CashCenterProcessSettingID
        {
            get { return this.GetPropertyValue<int>("CashCenterProcessSettingID"); }
            private set { this.SetPropertyValue(value, "CashCenterProcessSettingID"); }
        }

        [Reference("CashCenterProcessSettingID", DeleteRule = Rule.Cascade)]
        [Transient]
        public CashCenterProcessSetting ProcessSetting { get; set; }

        [PropertyDescription(IsNullable = false)]
        [Key]
        [Column(Order = 2)]
        public int BagTypeID
        {
            get { return this.GetPropertyValue<int>("BagTypeID"); }
            set { this.SetPropertyValue(value, "BagTypeID"); }
        }

        [PropertyDescription(IsNullable = false)]
        [Key]
        [Column(Order = 3)]
        public string MaterialTypeID
        {
            get { return this.GetPropertyValue<string>("MaterialTypeID"); }
            set { this.SetPropertyValue(value, "MaterialTypeID"); }
        } 

        [PropertyDescription(IsNullable = false)]
        public int MinimumQuantity
        {
            get { return this.GetPropertyValue<int>("MinimumQuantity"); }
            set { this.SetPropertyValue(value, "MinimumQuantity"); }
        }

        [PropertyDescription(IsNullable = false)]
        public int MaximumQuantity
        {
            get { return this.GetPropertyValue<int>("MaximumQuantity"); }
            set { this.SetPropertyValue(value, "MaximumQuantity"); }
        }
        
        [Transient]
        public override bool IsNew
        {
            get { return true; }
        }
        
        public void SetCashCenterProcessSettingID(int processSettingID)
        {
            this.SetPropertyValue(processSettingID, "CashCenterProcessSettingID");
        }
    }
}
