using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CWC.AutoTests.ObjectBuilder.TransportOrderInterfaceBuilder
{
    public class TransportOrderInterfaceBuilder
    {
        XDocument documentElement;
        XElement transportOrder;

        public TransportOrderInterfaceBuilder()
        {
            documentElement = new XDocument(new XElement("DocumentElement"));
        }

        public TransportOrderInterfaceBuilder New(int action)
        {
            transportOrder = new XElement("ServiceOrder", new XAttribute("act", action));
            documentElement.Root.Add(transportOrder);
            return this;
        }

        public TransportOrderInterfaceBuilder WithOrderID(string value)
        {
            transportOrder.Add(new XElement("Order_ID", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithCusnr(string value)
        {
            transportOrder.Add(new XElement("Cus_nr", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithServiceDate(string value)
        {
            transportOrder.Add(new XElement("Service_Date", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithOrder_Status(string value)
        {
            transportOrder.Add(new XElement("Order_Status", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithOrder_Type(int? value)
        {
            if (value.HasValue)
            {
                transportOrder.Add(new XElement("Order_Type", value));
            }
            
            return this;
        }

        public TransportOrderInterfaceBuilder WithOrder_Level(int? value)
        {
            if (value.HasValue)
            {
                transportOrder.Add(new XElement("Order_Level", value));
            }
            
            return this;
        }

        public TransportOrderInterfaceBuilder WithReasonCode(string value)
        {
            transportOrder.Add(new XElement("reason_cd", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithCancel_Reason(string value)
        {
            transportOrder.Add(new XElement("Cancel_Reason", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithReference_ID(string value)
        {
            transportOrder.Add(new XElement("Reference_ID", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithServiceType_Code(string value)
        {
            transportOrder.Add(new XElement("WP_ServiceType_Code", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithOrderedValue(int? value)
        {
            if (value.HasValue)
            {
                transportOrder.Add(new XElement("WP_OrderedValue", value));
            }
           
            return this;
        }

        public TransportOrderInterfaceBuilder WithOrderedWeight(int? value)
        {
            if (value.HasValue)
            {
                transportOrder.Add(new XElement("WP_OrderedWeight", value));
            }
           
            return this;
        }

        public TransportOrderInterfaceBuilder WithPreannouncedValue(int? value)
        {
            if (value.HasValue)
            {
                transportOrder.Add(new XElement("WP_PreannouncedValue", value));
            }
            
            return this;
        }

        public TransportOrderInterfaceBuilder WithSpecialCoinsValue(int? value)
        {
            if (value.HasValue)
            {
                transportOrder.Add(new XElement("WP_SpecialCoinsValue", value));
            }
     
            return this;
        }

        public TransportOrderInterfaceBuilder WithLocationID(decimal? value)
        {
            if (value.HasValue)
            {
                transportOrder.Add(new XElement("WP_loc_nr", value));
            }
            
            return this;
        }

        public TransportOrderInterfaceBuilder WithLocationCode(string value)
        {
            transportOrder.Add(new XElement("WP_ref_loc_nr", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithMasterRote(string value)
        {
            transportOrder.Add(new XElement("mast_cd", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithSiteCode(string value)
        {
            transportOrder.Add(new XElement("WP_branch_cd", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithCurrency(string value)
        {
            transportOrder.Add(new XElement("WP_CurrencyCode", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithComments(string value)
        {
            transportOrder.Add(new XElement("WP_Comments", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithDateCreated(string value)
        {
            transportOrder.Add(new XElement("WP_DateCreated", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithDateUpdated(string value)
        {
            transportOrder.Add(new XElement("WP_DateUpdated", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithEmail(string value)
        {
            transportOrder.Add(new XElement("WP_Email", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithPickUpLocation(string value)
        {
            transportOrder.Add(new XElement("WP_PickUp_ref_loc_nr", value));
            return this;
        }

        public TransportOrderInterfaceBuilder WithOldOrderID(string value)
        {
            transportOrder.Add(new XElement("Old_Order_ID", value));
            return this;
        }

        public TransportOrderInterfaceBuilder SaveToFolder(string path, string filename)
        {

            documentElement.Save($"{path}\\{filename}.xml");

            return this;
        }
    }
}
