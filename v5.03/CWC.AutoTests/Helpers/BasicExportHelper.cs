using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CWC.AutoTests.Helpers
{
    public class BasicExportHelper : IDisposable
    {
        AutomationBaseDataContext _context;
        public BasicExportHelper()
        {
            _context = new AutomationBaseDataContext();
        }

        /// <summary>
        /// Method to find all export items for specific entity
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns>list of specific export items</returns>
        private XDocument[] FindExportedEntity(string entityName)
        {            
            var xdocList = new List<XDocument>();
            var result = _context.ExportItems.Where(e=>e.FileName.StartsWith(entityName)).ToArray();

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

        /// <summary>
        /// Factory method to process found entities and process one of them (if exists with gived criteria)
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="identifier"></param>
        /// <param name="action"></param>
        /// <returns>Dictionary of mapped enitty</returns>
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
                    case "customer":
                        result = MapSpecificEntity(identifier, "cus_nr", descendants, atr, action);
                        break;

                    case "location":
                        result = MapSpecificEntity(identifier, "ref_loc_nr", descendants, atr, action);
                        break;

                    case "Material":
                        result = MapSpecificEntity(identifier, "materialID", descendants, atr, action);
                        break;

                    case "Product":
                        result = MapSpecificEntity(identifier, "ProductCode", descendants, atr, action);
                        break;

                    case "ProdContent":
                        result = MapSpecificEntity(identifier, "ProductCode", descendants, atr, action);
                        break;

                    case "ServiceOrder":
                        result = MapSpecificEntity(identifier, "Order_ID", descendants, atr, action);
                        break;

                    case "SOline":
                        result = MapSpecificEntity(identifier, "Order_ID", descendants, atr, action);
                        break;

                    case "SOProduct":
                        result = MapSpecificEntity(identifier+"-1", "Orderline_ID", descendants, atr, action);
                        break;

                    case "master":
                        result = MapSpecificEntity(identifier, "mast_cd", descendants, atr, action);
                        break;

                    case "mas_line":
                        result = MapSpecificEntity(identifier, "mast_cd", descendants, atr, action);
                        break;
 
                    default:
                        throw new ArgumentNullException($"{entityName} with identifier wasn't found");
                }

                if (result != null)
                {
                    return result;
                }

            }

            throw new ArgumentNullException($"{entityName} with specified criter is not found");
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
            var resIdentifier = descendants.Where(d => d.Name == tagName).Single().Value;
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
            _context.Dispose();
        }
    }
}
