using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CWC.AutoTests.Helpers.Transport
{
    public class TransportExportHelper : IDisposable
    {
        AutomationTransportDataContext context;

        public TransportExportHelper()
        {
            context = new AutomationTransportDataContext();
        }

        private XDocument[] FindExportedEntity(string entityName)
        {
            var xdocList = new List<XDocument>();
            var result = context.TransportOrderExportItems.Where(e => e.FileName.StartsWith(entityName)).ToArray();

            if (!result.Any())
            {
                throw new ArgumentNullException($"No {entityName} entities was found");
            }

            foreach (var item in result)
            {
                xdocList.Add(XDocument.Load(new System.IO.MemoryStream(item.FileData)));
            }

            return xdocList.ToArray();
        }

        public Dictionary<string, string> MapEntity(string entityName, string identifier, int action)
        {
            var result = new Dictionary<string, string>();

            foreach (var item in FindExportedEntity(entityName))
            {
                var entTag = item.Root.Elements().Where(x => x.Name == entityName).First();
                var atr = entTag.Attribute(XName.Get("act")).Value;
                var descendants = entTag.Descendants();

                switch (entityName)
                {

                    case "ServiceOrder":
                        result = MapSpecificEntity(identifier, "Order_ID", descendants, atr, action);
                        break;

                    case "SOline":
                        result = MapSpecificEntity(identifier, "Order_ID", descendants, atr, action);
                        break;

                    case "SOProduct":
                        result = MapSpecificEntity(identifier + "-1", "Orderline_ID", descendants, atr, action);
                        break;

                    case "SOService":
                        result = MapSpecificEntity(identifier + "-1", "OrderLine_ID", descendants, atr, action);
                        break;

                    default:
                        throw new ArgumentNullException($"{entityName} with identifier wasn't found");
                }
            }

            return result;
        }

        /// <summary>
        /// Method to process specific entity
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="tagName"></param>
        /// <param name="descendants"></param>
        /// <param name="atr"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private Dictionary<string, string> MapSpecificEntity(string identifier, string tagName, IEnumerable<XElement> descendants, string atr, int action)
        {
            var resIdentifier = descendants.Single(d => d.Name == tagName).Value;
            int tempRes;
            var parseRes = int.TryParse(atr, out tempRes);

            if (parseRes && (tempRes == action) && (resIdentifier == identifier))
            {
                return descendants.ToDictionary(x => x.Name.LocalName, y => y.Value);
            }

            return null;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
