using Cwc.Ordering.Model;
using CWC.AutoTests.Model;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace CWC.AutoTests.Helpers
{
    public class BasicFileWorkHelper
    {

        public BasicFileWorkHelper()
        {
            
        }
        public void RemoveExportedFile(string entityName, string day, string filePath)
        {

            var result = FindExportedFile(entityName, day);
            try
            {
                File.Delete(Path.Combine(filePath, result.ResultingFileName));
            }
            catch
            {
                Thread.Sleep(100); //Thread should be removed after an error with access will be investigated. 
            }
        }

        private OrderExportJobLog FindExportedFile(string entityName, string day)
        {
            using (var context = new AutomationOrderingDataContext())
            {
                var result = context.OrderExportLog.Where(e => e.ResultingFileName.StartsWith(entityName) && e.ResultingFileName.Contains(day)).ToArray().OrderByDescending(oe => oe.DateCreated).First();

                if (result == null)
                {
                    throw new ArgumentNullException($"File wasn't exported");
                }
                return result;
            }
        }

        private XDocument FindExportedEntity(string entityName, string day, string filePath)
        {
            XDocument fileTemp;
            var result = FindExportedFile(entityName, day);
            using (var stream = File.Open(Path.Combine(filePath, result.ResultingFileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = XmlReader.Create(stream))
            {
                fileTemp = XDocument.Load(reader);
            }
            return fileTemp;
        }

        public XElement GetEntity(string entityName, string day, string filePath)
        {
            var item = FindExportedEntity(entityName, day, filePath);
            var tagName = entityName.Replace(" ", "");

            var entTag = GetTag($"{tagName}List", item.Root);
            var entTagS = GetTag($"{tagName}", entTag);
            return entTagS;
        }
        public static XElement GetTag(string tagName, XElement serviceOrderExported)
        {
            return serviceOrderExported.Elements().Where(x => x.Name == tagName).First();
        }
    }
}
