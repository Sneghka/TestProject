using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specflow.Automation.Backend.Model
{
    public class OrderImportFormatBItem
    {
        private int? orderProductNumber;
        private string bankProductCode;


        #region Proeprty 'OrderProductNumber'
        /// <summary>
        /// Gets or sets OrderProductNumber
        /// </summary>  
        public int? OrderProductNumber
        {
            get
            {
                return this.orderProductNumber.HasValue ? this.orderProductNumber.Value : 0;
            }
            set
            {
                if (value != 0)
                {
                    this.orderProductNumber = value;
                }
            }
        }
        #endregion

        #region Proeprty 'BankProductCode'
        /// <summary>
        /// Gets or sets OrderProductNumber
        /// </summary>  
        public string BankProductCode
        {
            get
            {
                return this.bankProductCode != null ? this.bankProductCode : null;
            }
            set
            {
                if (value != null)
                {
                    this.bankProductCode = value;
                }
            }
        }
        #endregion
    }
}
