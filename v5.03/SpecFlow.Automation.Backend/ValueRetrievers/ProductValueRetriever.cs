using Cwc.BaseData;
using Specflow.Automation.Backend.Hooks;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow.Assist;

namespace Specflow.Automation.Backend.ValueRetrievers
{
    public class ProductValueRetriever : IValueRetriever
    {
        public virtual Product GetValue(string value)
        {
            if (BaseDataConfigurationHooks.ProductDict.ContainsKey(value))
            {
                return BaseDataConfigurationHooks.ProductDict[value];
            }
            else
            {
                throw new KeyNotFoundException($"There is no {value} product in the Product dictionary.");
            }            
        }

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return GetValue(keyValuePair.Value);
        }

        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return propertyType == typeof(Product);
        }
    }
}