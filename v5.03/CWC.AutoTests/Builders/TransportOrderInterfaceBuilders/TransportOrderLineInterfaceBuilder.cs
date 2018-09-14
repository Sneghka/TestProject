using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CWC.AutoTests.ObjectBuilder.TransportOrderInterfaceBuilder
{
    public class TransportOrderLineInterfaceBuilder
    {
        XDocument documentElement;
        XElement transportOrderLine;

        public TransportOrderLineInterfaceBuilder()
        {
            documentElement = new XDocument(new XElement("DocumentElement"));
        }

        public TransportOrderLineInterfaceBuilder New(int action)
        {
            transportOrderLine = new XElement("SOline", new XAttribute("act", action));
            documentElement.Root.Add(transportOrderLine);
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithOrderLineID(string value)
        {
            transportOrderLine.Add(new XElement("OrderLine_ID", value));
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithOrderID(string value)
        {
            transportOrderLine.Add(new XElement("Order_ID", value));
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithOrderLineStatus(string value)
        {
            transportOrderLine.Add(new XElement("Orderline_status", value));
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithlocationID(decimal? value)
        {
            if (value.HasValue)
            {
                transportOrderLine.Add(new XElement("Loc_nr", value));
            }
           
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithMasterRoute(string value)
        {
            transportOrderLine.Add(new XElement("Mast_cd", value));
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithDayNumber(int? value)
        {
            if (value.HasValue)
            {
                transportOrderLine.Add(new XElement("Day_nr", value));
            }
            
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithDaiDate(string value)
        {
            transportOrderLine.Add(new XElement("Dai_date", value));
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithArrivalTime(string value)
        {
            transportOrderLine.Add(new XElement("a_time", value));
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithVisitSequence(int? value)
        {
            if (value.HasValue)
            {
                transportOrderLine.Add(new XElement("Visit_Sequence", value));
            }
           
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithBranchNumber(int? value)
        {
            if (value.HasValue)
            {
                transportOrderLine.Add(new XElement("branch_nr", value));
            }
            
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithServiceType(string value)
        {
            transportOrderLine.Add(new XElement("Serv_type", value));
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithOrderLineTime1(string value)
        {
            transportOrderLine.Add(new XElement("Orderline_timew1", value));
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithOrderLineTime2(string value)
        {
            transportOrderLine.Add(new XElement("Orderline_timew2", value));
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithRevenue(string value)
        {
            transportOrderLine.Add(new XElement("Revenue", value));
            return this;
        }

        public TransportOrderLineInterfaceBuilder WithOrderLineValue(int? value)
        {
            if (value.HasValue)
            {
                transportOrderLine.Add(new XElement("Orderline_value", value));
            }
            
            return this;
        }

        public TransportOrderLineInterfaceBuilder SaveToFolder(string path, string filename)
        {
            documentElement.Save($"{path}\\{filename}.xml");

            return this;
        }
    }
}