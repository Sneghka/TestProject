using Cwc.Common;
using System.Xml.Serialization;

namespace CWC.AutoTests.Tests.OrderExport
{
    public class ProductExportTempItem
    {
        #region Fields
        private int materialId;
        private int numberOfItems;
        private int? quantity;
        private int? orderProductNumber;
        #endregion

        public int MaterialID
        {
            get { return this.materialId; }
            set { this.materialId = value; }
        }

        #region Proeprty 'NumberOfItems'
        /// <summary>
        /// Gets or sets NumberOfItems
        /// </summary>   
        public int NumberOfItems
        {
            get
            {
                return this.numberOfItems;
            }
            set
            {
                if (value != 0)
                {
                    this.numberOfItems = value;
                }
            }
        }
        #endregion

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

        #region Proeprty 'Quantity'
        /// <summary>
        /// Gets or sets NumberOfItems
        /// </summary>   
        public int? Quantity
        {
            get
            {
                return this.quantity.HasValue ? this.quantity.Value : 0;
            }
            set
            {
                if (value != 0)
                {
                    this.quantity = value;
                }
            }
        }
        #endregion
    }
}
