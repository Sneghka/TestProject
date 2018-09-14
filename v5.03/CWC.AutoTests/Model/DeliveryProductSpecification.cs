using Cwc.BaseData;

namespace CWC.AutoTests.Model
{
    public class DeliveryProductSpecification
    {        
        public Product Product { get; set; }
        public int Quantity { get; set; } = 0;
        public bool IsBarcoded { get; set; } = false;
        public bool IsLoose { get; set; } = false;

        public decimal TotalValue
        {
            get
            {
                if (Product != null && Quantity != 0)
                {
                    return Product.Value * Quantity;
                }
                else
                {
                    return 0;
                }
            }            
        }
    }
}
