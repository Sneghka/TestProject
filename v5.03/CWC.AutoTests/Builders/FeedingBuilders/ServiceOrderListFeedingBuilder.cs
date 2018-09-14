using Cwc.Ordering;
using System.Xml.Linq;

namespace CWC.AutoTests.ObjectBuilder.FeedingBuilder
{
    public class ServiceOrderListFeedingBuilder
    {
        XDocument documentElement;
        XElement serviceOrder;
        XElement serviceOrderList;
        XElement deliveryProducts;
        XElement collectPackages;
        XElement materials;
        XElement package;
        XElement services;
        XElement service, totalElement, materialElement, productElement;

        public ServiceOrderListFeedingBuilder()
        {
            documentElement = new XDocument(new XElement("DocumentElement"));
        }

        public ServiceOrderListFeedingBuilder New(string mapperName = "")
        {
            serviceOrderList = new XElement("ServiceOrderList", new XAttribute("mapper", mapperName));
            documentElement.Root.Add(serviceOrderList);
            return this;
        }

        public ServiceOrderListFeedingBuilder With_ServiceOrder()
        {
            serviceOrder = new XElement("ServiceOrder");
            serviceOrderList.Add(serviceOrder);
            return this;
        }

        public ServiceOrderListFeedingBuilder With_Number(string number)
        {
            if (number != null)
            {
                serviceOrder.Add(new XElement("Number", number));
            }

            return this;
        }

        public ServiceOrderListFeedingBuilder With_ServiceDate(string serviceDate)
        {
            if (serviceDate != null)
            {
                serviceOrder.Add(new XElement("ServiceDate", serviceDate));
            }

            return this;
        }

        public ServiceOrderListFeedingBuilder With_LocationCode(string locationCode)
        {
            if (locationCode != null)
            {
                serviceOrder.Add(new XElement("LocationCode", locationCode));
            }

            return this;
        }

        public ServiceOrderListFeedingBuilder With_PickLocationCode(string pickLocationCode)
        {
            if (pickLocationCode != null)
            {
                serviceOrder.Add(new XElement("PickupLocationCode", pickLocationCode));
            }

            return this;
        }

        public ServiceOrderListFeedingBuilder With_ServiceTypeCode(string serviceTypeCode)
        {
            if (serviceTypeCode != null)
            {
                serviceOrder.Add(new XElement("ServiceTypeCode", serviceTypeCode));
            }

            return this;
        }

        public ServiceOrderListFeedingBuilder With_CustomerReference(string customer)
        {
            if (customer != null)
            {
                serviceOrder.Add(new XElement("CustomerReference", customer));
            }

            return this;
        }

        public ServiceOrderListFeedingBuilder With_BankReference(string bank)
        {
            if (bank != null)
            {
                serviceOrder.Add(new XElement("BankReference", bank));
            }

            return this;
        }

        public ServiceOrderListFeedingBuilder With_CitReference(string cit)
        {
            if (cit != null)
            {
                serviceOrder.Add(new XElement("CITReference", cit));
            }

            return this;
        }

        public ServiceOrderListFeedingBuilder With_Comments(string comments)
        {
            if (comments != null)
            {
                serviceOrder.Add(new XElement("Comments", comments));
            }

            return this;
        }

        public ServiceOrderListFeedingBuilder With_CurrencyCode(string currency)
        {
            if (currency != null)
            {
                serviceOrder.Add(new XElement("CurrencyCode", currency));
            }

            return this;
        }

        public ServiceOrderListFeedingBuilder With_CancelReasonCode(int? reasonCode)
        {
            if (reasonCode != null)
            {
                serviceOrder.Add(new XElement("CancelReasonCode", reasonCode));
            }

            return this;
        }

        public ServiceOrderListFeedingBuilder With_CancelRemark(string remark)
        {
            if (remark != null)
            {
                serviceOrder.Add(new XElement("CancelRemark", remark));
            }

            return this;
        }

        public ServiceOrderListFeedingBuilder With_GenericStatus(GenericStatus genericStatus)
        {            
            serviceOrder.Add(new XElement("GenericStatus", genericStatus.ToString()));            
            return this;
        }

        public ServiceOrderListFeedingBuilder With_DeliveryProducts()
        {
            deliveryProducts = new XElement("DeliveryProducts");
            serviceOrder.Add(deliveryProducts);

            return this;
        }

        public ServiceOrderListFeedingBuilder With_Product()
        {
            productElement = new XElement("Product");
            deliveryProducts.Add(productElement);

            return this;
        }

        public ServiceOrderListFeedingBuilder With_ProductCode(string productCode)
        {
            if (productCode != null)
            {
                productElement.Add(new XElement("ProductCode", productCode));
            }
            return this;
        }

        public ServiceOrderListFeedingBuilder With_ProductQuantity(int? amount)
        {
            if (amount != null)
            {
                productElement.Add(new XElement("Quantity", amount));
            }
            else
            {
                productElement.Add(new XElement("Quantity", null));
            }
            return this;
        }

        public ServiceOrderListFeedingBuilder With_ProductValue(int? amount)
        {
            if (amount != null)
            {
                productElement.Add(new XElement("Value", amount));
            }
            else
            {
                productElement.Add(new XElement("Value", null));
            }
            return this;
        }

        public ServiceOrderListFeedingBuilder With_Product(string productCode, int? quantity, decimal? value)
        {

            var productElement = new XElement("Product");

            if (productCode != null)
            {
                productElement.Add(new XElement("ProductCode", productCode));
            }

            if (quantity != null)
            {
                productElement.Add(new XElement("Quantity", quantity));
            }
            if (value != null)
            {
                productElement.Add(new XElement("Value", value));
            }

            deliveryProducts.Add(productElement);

            return this;
        }

        public ServiceOrderListFeedingBuilder With_CollectedPackages()
        {
            collectPackages = new XElement("CollectedPackages");
            serviceOrder.Add(collectPackages);

            return this;
        }

        public ServiceOrderListFeedingBuilder With_Package()
        {
            package = new XElement("Package"); 
            collectPackages.Add(package);

            return this;
        }

        public ServiceOrderListFeedingBuilder With_PackageNumber(string packageNumber)
        {
            if (packageNumber != null)
            {
                package.Add(new XElement("PackageNumber", packageNumber));
            }
            return this;
        }

        public ServiceOrderListFeedingBuilder With_Materials()
        {
            materials = new XElement("Materials");
            package.Add(materials);

            return this;
        }

        public ServiceOrderListFeedingBuilder With_Material()
        {
            materialElement = new XElement("Material");
            materials.Add(materialElement);

            return this;
        }

        public ServiceOrderListFeedingBuilder With_MaterialID(string materialCode)
        {
            if (materialCode != null)
            {
                materialElement.Add(new XElement("MaterialID", materialCode));
            }
            return this;
        }

        public ServiceOrderListFeedingBuilder With_MaterialQuantity(int materialCount)
        {
            materialElement.Add(new XElement("Quantity", materialCount));
            return this;
        }


        public ServiceOrderListFeedingBuilder With_Total()
        {
            totalElement = new XElement("Total");
            package.Add(totalElement);

            return this;
        }


        public ServiceOrderListFeedingBuilder With_TotalValue(decimal value)
        {
            if (value != 0)
            {
                totalElement.Add(new XElement("Value", value));
            }
            return this;
        }

        public ServiceOrderListFeedingBuilder With_TotalCurrency(string currency)
        {
            if (currency != null)
            {
                totalElement.Add(new XElement("CurrencyCode", currency));
            }
            return this;
        }

        public ServiceOrderListFeedingBuilder With_Services()
        {

            services = new XElement("Services");

            serviceOrder.Add(services);

            return this;
        }

        public ServiceOrderListFeedingBuilder With_Service()
        {
            service = new XElement("Service");
            services.Add(service);
            return this;
        }


        public ServiceOrderListFeedingBuilder With_ServicingCode(string servicingCode)
        {
            if (servicingCode != null)
            {
                service.Add(new XElement("ServiceCode", servicingCode));
            }
            return this;
        }

        public XDocument Build()
        {
            return documentElement;
        }
    }
}
