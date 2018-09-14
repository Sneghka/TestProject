using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CWC.AutoTests.ObjectBuilder.TransportOrderInterfaceBuilder
{
    public class TransportOrderProductInterfaceBuilder
    {
        XDocument documentElement;
        XElement transportOrderProduct;

        public TransportOrderProductInterfaceBuilder()
        {
            documentElement = new XDocument(new XElement("DocumentElement"));
        }

        public TransportOrderProductInterfaceBuilder New(int action)
        {
            transportOrderProduct = new XElement("SOProduct", new XAttribute("act", action));
            documentElement.Root.Add(transportOrderProduct);
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithOrderLineID(string value)
        {
            transportOrderProduct.Add(new XElement("Orderline_ID", value));
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithProductCode(string value)
        {
            transportOrderProduct.Add(new XElement("ProductCode", value));
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithOrderProductNumber(int? value)
        {
            if (value.HasValue)
            {
                transportOrderProduct.Add(new XElement("OrderProduct_Number", value));
            }
            
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithOrderProductValue(decimal? value)
        {
            if (value.HasValue)
            {
                transportOrderProduct.Add(new XElement("OrderProduct_Value", value));
            }
            
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithActualNumber(int? value)
        {
            if (value.HasValue)
            {
                transportOrderProduct.Add(new XElement("Actual_Number", value));
            }
            
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithActualValue(decimal? value)
        {
            if (value.HasValue)
            {
                transportOrderProduct.Add(new XElement("Actual_Value", value));
            }
            
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithPreanQty(int? value)
        {
            if (value.HasValue)
            {
                transportOrderProduct.Add(new XElement("WP_prean_qty", value));
            }
           
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithPreanValue(decimal? value)
        {
            if (value.HasValue)
            {
                transportOrderProduct.Add(new XElement("WP_prean_value", value));
            }
            
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithCountDate(string value)
        {
            transportOrderProduct.Add(new XElement("WP_count_date", value));
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithPackNumber(string value)
        {
            transportOrderProduct.Add(new XElement("WP_pack_nr", value));
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithMaterialID(string value)
        {
            transportOrderProduct.Add(new XElement("WP_material_id", value));
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithBookinddate(string value)
        {
            transportOrderProduct.Add(new XElement("WP_booking_date", value));
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithTotalOnly(int? value)
        {
            if (value.HasValue)
            {
                transportOrderProduct.Add(new XElement("WP_total_only", value));
            }
            
            return this;
        }
        public TransportOrderProductInterfaceBuilder WithReject(int? value)
        {
            if (value.HasValue)
            {
                transportOrderProduct.Add(new XElement("WP_reject", value));
            }
            
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithLocationNumber(string value)
        {
            transportOrderProduct.Add(new XElement("WP_ref_loc_nr", value));
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithSubmitDate(string value)
        {
            transportOrderProduct.Add(new XElement("WP_Submit_date", value));
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithPickedQty(string value)
        {
            transportOrderProduct.Add(new XElement("WP_Picked_Qty", value));
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithPickedValue(string value)
        {
            transportOrderProduct.Add(new XElement("WP_Picked_Value", value));
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithPickedDate(string value)
        {
            transportOrderProduct.Add(new XElement("WP_Picked_Date", value));
            return this;
        }

        public TransportOrderProductInterfaceBuilder WithCurrency(string value)
        {
            transportOrderProduct.Add(new XElement("CurrencyID", value));
            return this;
        }

        public TransportOrderProductInterfaceBuilder SaveToFolder(string path, string filename)
        {
            documentElement.Save($"{path}\\{filename}.xml");

            return this;
        }
    }
}
