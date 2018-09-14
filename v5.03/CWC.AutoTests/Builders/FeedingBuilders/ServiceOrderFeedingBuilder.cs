using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CWC.AutoTests.ObjectBuilder.FeedingBuilder
{
    public class ServiceOrderFeedingBuilder
    {
        XDocument documentElement;
        XElement serviceOrderFeeding;
        XElement orderLineFeeding;
        XElement productFeeding;
        XElement serviceFeeding;
        public ServiceOrderFeedingBuilder()
        {
            documentElement = new XDocument(new XElement("DocumentElement"));

            
        }

        public ServiceOrderFeedingBuilder New()
        {
            serviceOrderFeeding = new XElement("OrderFeeding");
            documentElement.Root.Add(serviceOrderFeeding);
            return this;
        }

        public ServiceOrderFeedingBuilder With_ReferenceID(string value)
        {
            if (value != null)
            {
                serviceOrderFeeding.Add(new XElement("Reference_ID", value));
            }

            return this;
        }

        public ServiceOrderFeedingBuilder With_BankReferenceID(string value)
        {
            if (value != null)
            {
                serviceOrderFeeding.Add(new XElement("BankReference", value));
            }

            return this;
        }

        public ServiceOrderFeedingBuilder With_CiteferenceID(string value)
        {
            if (value != null)
            {
                serviceOrderFeeding.Add(new XElement("CITReference", value));
            }

            return this;
        }

        public ServiceOrderFeedingBuilder With_Customer(string value)
        {
            if (value != null)
            {
                serviceOrderFeeding.Add(new XElement("Cus_nr", value));
            }

            return this;
        }

        public ServiceOrderFeedingBuilder With_ServiceDate(string value)
        {
            if (value != null)
            {
                serviceOrderFeeding.Add(new XElement("Service_Date", value));
            }

            return this;
        }

        public ServiceOrderFeedingBuilder With_ServiceCode(string value)
        {
            if (value != null)
            {
                serviceOrderFeeding.Add(new XElement("WP_ServiceType_Code", value));
            }

            return this;
        }

        public ServiceOrderFeedingBuilder With_CurrencyCode(string value)
        {
            if (value != null)
            {
                serviceOrderFeeding.Add(new XElement("WP_CurrencyCode", value));
            }

            return this;
        }

        public ServiceOrderFeedingBuilder With_Location(string value)
        {
            if (value != null)
            {
                serviceOrderFeeding.Add(new XElement("WP_ref_loc_nr", value));
            }

            return this;
        }

        public ServiceOrderFeedingBuilder With_Email(string value)
        {
            if (value != null)
            {
                serviceOrderFeeding.Add(new XElement("WP_Email", value));
            }

            return this;
        }

        public ServiceOrderFeedingBuilder With_OrderLine(string location)
        {
            orderLineFeeding = new XElement("OrderLineFeeding");
            serviceOrderFeeding.Add(orderLineFeeding);
            orderLineFeeding.Add(new XElement("WP_ref_loc_nr", location));

            return this;
        }

        public ServiceOrderFeedingBuilder With_OrderLineProduct(string prodCode, string prodNumber, string prodValue)
        {

            productFeeding = new XElement("ProductFeeding");

            if (prodCode != null)
            {
                productFeeding.Add(new XElement("ProductCode", prodCode));
            }

            if (prodNumber != null)
            {
                productFeeding.Add(new XElement("OrderProduct_Number", prodNumber));
            }

            if (prodValue != null)
            {
                productFeeding.Add(new XElement("OrderProduct_Value", prodValue));
            }

            orderLineFeeding.Add(productFeeding);

            return this;
        }

        public ServiceOrderFeedingBuilder With_OrderLineProduct(string packNr, string totalOnly, string reject, string preanQty, string peanVal, string materialId)
        {

            productFeeding = new XElement("ProductFeeding");

            if (packNr != null)
            {
                productFeeding.Add(new XElement("WP_pack_nr", packNr));
            }

            if (totalOnly != null)
            {
                productFeeding.Add(new XElement("WP_total_only", totalOnly));
            }

            if (reject != null)
            {
                productFeeding.Add(new XElement("WP_reject", reject));
            }

            if (preanQty != null)
            {
                productFeeding.Add(new XElement("WP_prean_qty", preanQty));
            }

            if (peanVal != null)
            {
                productFeeding.Add(new XElement("WP_prean_value", peanVal));
            }

            if (materialId != null)
            {
                productFeeding.Add(new XElement("WP_material_id", materialId));
            }

            orderLineFeeding.Add(productFeeding);

            return this;
        }

        public ServiceOrderFeedingBuilder With_OrderLineProduct(string packNr, string totalOnly, string currency, string peanVal, string reject)
        {

            productFeeding = new XElement("ProductFeeding");

            if (packNr != null)
            {
                productFeeding.Add(new XElement("WP_pack_nr", packNr));
            }

            if (totalOnly != null)
            {
                productFeeding.Add(new XElement("WP_total_only", totalOnly));
            }

            if (currency != null)
            {
                productFeeding.Add(new XElement("CurrencyID", currency));
            }

            if (peanVal != null)
            {
                productFeeding.Add(new XElement("WP_prean_value", peanVal));
            }

            if (reject != null)
            {
                productFeeding.Add(new XElement("WP_reject", reject));
            }

            orderLineFeeding.Add(productFeeding);

            return this;
        }

        public ServiceOrderFeedingBuilder With_OrderLineService(string serviceCode)
        {

            serviceFeeding = new XElement("ServiceFeeding");

            serviceFeeding.Add(new XElement("ServiceCode", serviceCode));

            orderLineFeeding.Add(serviceFeeding);

            return this;
        }

        public XDocument Build()
        {
            return documentElement;
        }
    }
}
