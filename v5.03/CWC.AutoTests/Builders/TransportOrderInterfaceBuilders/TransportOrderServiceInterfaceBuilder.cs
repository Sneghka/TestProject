using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CWC.AutoTests.ObjectBuilder.TransportOrderInterfaceBuilder
{
    public class TransportOrderServiceInterfaceBuilder
    {
        XDocument documentElement;
        XElement transportOrderService;

        public TransportOrderServiceInterfaceBuilder()
        {
            documentElement = new XDocument(new XElement("DocumentElement"));
        }

        public TransportOrderServiceInterfaceBuilder New(int action)
        {
            transportOrderService = new XElement("SOService", new XAttribute("act", action));
            documentElement.Root.Add(transportOrderService);
            return this;
        }

        public TransportOrderServiceInterfaceBuilder WithOrderLineID(string value)
        {
            transportOrderService.Add(new XElement("OrderLine_ID", value));
            return this;
        }

        public TransportOrderServiceInterfaceBuilder WithServicingCode(string value)
        {
            transportOrderService.Add(new XElement("ServiceCode", value));
            return this;
        }

        public TransportOrderServiceInterfaceBuilder WithIsServicePerformed(bool? value)
        {
            if(value.HasValue)
            {
                transportOrderService.Add(new XElement("Service_Performed", value));
            }
            
            return this;
        }

        public TransportOrderServiceInterfaceBuilder WithIsServicePlanned(bool? value)
        {
            if (value.HasValue)
            {
                transportOrderService.Add(new XElement("Service_Planned", value));
            }
            
            return this;
        }

        public TransportOrderServiceInterfaceBuilder SaveToFolder(string path, string filename)
        {
            documentElement.Save($"{path}\\{filename}.xml");

            return this;
        }
    }
}
