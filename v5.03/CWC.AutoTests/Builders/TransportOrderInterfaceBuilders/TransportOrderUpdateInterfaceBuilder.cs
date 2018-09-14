using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CWC.AutoTests.ObjectBuilder.TransportOrderInterfaceBuilder
{
    public class TransportOrderUpdateInterfaceBuilder
    {
        XDocument documentElement;
        XElement transportOrderUpdate;

        public TransportOrderUpdateInterfaceBuilder()
        {
            documentElement = new XDocument(new XElement("DocumentElement"));
        }

        public TransportOrderUpdateInterfaceBuilder New(int action)
        {
            transportOrderUpdate = new XElement("ServiceOrderUpdate", new XAttribute("act", action));
            documentElement.Root.Add(transportOrderUpdate);
            return this;
        }

        public TransportOrderUpdateInterfaceBuilder WithOrderID(string value)
        {
            transportOrderUpdate.Add(new XElement("Order_ID", value));
            return this;
        }

        public TransportOrderUpdateInterfaceBuilder WithOrderStatus(string value)
        {
            transportOrderUpdate.Add(new XElement("Order_Status", value));
            return this;
        }

        public TransportOrderUpdateInterfaceBuilder WithException(int? value)
        {
            if (value.HasValue)
            {
                transportOrderUpdate.Add(new XElement("WP_WithException", value));
            }
            
            return this;
        }

        public TransportOrderUpdateInterfaceBuilder WithNewServiceDate(string value)
        {
            transportOrderUpdate.Add(new XElement("WP_NewServiceDate", value));
            return this;
        }

        public TransportOrderUpdateInterfaceBuilder WithMasterRoute(string value)
        {
            transportOrderUpdate.Add(new XElement("mast_cd", value));
            return this;
        }

        public TransportOrderUpdateInterfaceBuilder WithBranchCode(string value)
        {
            transportOrderUpdate.Add(new XElement("WP_branch_cd", value));
            return this;
        }

        public TransportOrderUpdateInterfaceBuilder WithLocationID(decimal? value)
        {
            if (value.HasValue)
            {
                transportOrderUpdate.Add(new XElement("WP_loc_nr", value));
            }
            
            return this;
        }

        public TransportOrderUpdateInterfaceBuilder WithDateUpdated(string value)
        {
            transportOrderUpdate.Add(new XElement("WP_DateUpdated", value));
            return this;
        }

        public TransportOrderUpdateInterfaceBuilder SaveToFolder(string path, string filename)
        {
            documentElement.Save($"{path}\\{filename}.xml");

            return this;
        }
    }
}
