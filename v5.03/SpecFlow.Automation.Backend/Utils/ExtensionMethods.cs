using Cwc.BaseData;
using Cwc.CallManagement;
using Cwc.CashCenter;
using Cwc.Contracts.Enums;
using Specflow.Automation.Backend.Hooks;
using System;

namespace Specflow.Automation.Backend.Utils
{
    public static class ExtensionMethods
    {
        public static string ConvertToServiceTypeCode(this string type)
        {
            switch (type)
            {
                case "Delivery":
                    return BaseDataConfigurationHooks.DeliverCode;
                case "Collection":
                    return BaseDataConfigurationHooks.CollectCode;
                case "Replenishment":
                    return BaseDataConfigurationHooks.ReplenishmentCode;
                case "Servicing":
                    return BaseDataConfigurationHooks.ServicingCode;
                default:
                    throw new InvalidOperationException("Provided service type is not supported.");
            }
        }
                  

        public static StockOrderType? ConvertToStockOrderType(this string type)
        {
            switch (type)
            {
                case "regular":
                    return null;
                case "notes loose":
                    return StockOrderType.NotesLooseProducts;
                case "coins loose":
                    return StockOrderType.CoinsLooseProducts;
                case "barcoded":
                    return StockOrderType.BarcodedProducts;
                default:
                    throw new InvalidOperationException("Provided stock order type is not supported.");
            }
        }

        public static Customer ConvertToCustomer(this string customerCode)
        {
            return BaseDataConfigurationHooks.CustomerDict.ContainsKey(customerCode)
                ? BaseDataConfigurationHooks.CustomerDict[customerCode]
                : throw new ArgumentNullException(customerCode, "There is no customer with provided code in the Customer dictionary.");
        }

        public static Location ConvertToLocation(this string locationCode)
        {
            return BaseDataConfigurationHooks.LocationDict.ContainsKey(locationCode)
                ? BaseDataConfigurationHooks.LocationDict[locationCode]
                : throw new ArgumentNullException(locationCode, "There is no location with provided code in the Location dictionary.");
        }

        public static Requestor ConvertToRequestor(this string requestor)
        {
            return BaseDataConfigurationHooks.RequestorDict.ContainsKey(requestor)
                ? BaseDataConfigurationHooks.RequestorDict[requestor]
                : throw new ArgumentNullException(requestor, "There is no requestor with provided code in the Location dictionary.");
        }

        public static PriceRule ConvertToPriceRule(this string priceRuleName)
        {
            if (!Enum.TryParse(priceRuleName, out PriceRule priceRule))
            {
                throw new InvalidCastException($"Error on parsing {priceRuleName} enumeration.");
            }
            return priceRule;
        }

        public static UnitOfMeasure ConvertToUnitOfMeasure(this string unitOfMeasureName)
        {
            if (!Enum.TryParse(unitOfMeasureName, out UnitOfMeasure unitOfMeasure))
            {
                throw new InvalidCastException($"Error on parsing {unitOfMeasureName} enumeration.");
            }
            return unitOfMeasure;
        }

        public static PriceRuleLevelName ConvertToPriceRuleLevelName(this string priceRuleLevelNameStr)
        {
            if (!Enum.TryParse(priceRuleLevelNameStr, out PriceRuleLevelName priceRuleLevelName))
            {
                throw new InvalidCastException($"Error on parsing {priceRuleLevelNameStr} enumeration.");
            }
            return priceRuleLevelName;
        }

        public static PriceRuleLevelValueType ConvertToPriceRuleLevelValueType(this string priceRuleLevelValueTypeName)
        {
            if (!Enum.TryParse(priceRuleLevelValueTypeName, out PriceRuleLevelValueType priceRuleLevelValueType))
            {
                throw new InvalidCastException($"Error on parsing {priceRuleLevelValueTypeName} enumeration.");
            }
            return priceRuleLevelValueType;
        }
    }
}
