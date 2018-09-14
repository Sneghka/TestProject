using Cwc.BaseData;
using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specflow.Automation.Backend.Utils
{
    public static class TableExtensions
    {
        public static Dictionary<string, object> ToDictionary(this Table table)
        {
            var dictionary = new Dictionary<string, object>();

            if (String.Join(",", table.Header.ToArray()) == "Key,Value")
            {
                foreach (var row in table.Rows)
                {
                    dictionary.Add(row[0], row[1]);
                }
            }
            else
            {
                foreach (var pair in table.Rows[0])
                {                    
                    dictionary.Add(pair.Key, pair.Value);                    
                }                
            }            
            return dictionary;
        }

        public static List<DeliveryProductSpecification> ToDeliveryContentList(this Table table)
        {
            return table.CreateSet<DeliveryProductSpecification>().ToList();            
        }

        public static List<Location> ToLocationList(this Table table)
        {
            List<Location> list = new List<Location>();

            foreach (var row in table.Rows)
            {
                if (row.ContainsKey("Location"))
                {
                    list.Add(row["Location"].ConvertToLocation());
                }
                else
                {
                    throw new InvalidOperationException("Table does not contain 'Location' column.");
                }
            }

            return list;
        }
    }
}
