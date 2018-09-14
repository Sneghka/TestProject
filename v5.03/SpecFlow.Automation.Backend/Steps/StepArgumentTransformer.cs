using Cwc.BaseData;
using Cwc.Ordering;
using Specflow.Automation.Backend.Hooks;
using System;
using TechTalk.SpecFlow;

namespace SpecFlow.Automation.BackEnd.Steps
{
    [Binding]
    public class StepArgumentTransformer
    {
        [StepArgumentTransformation(@"(\d+) day(?:s)?")]
        public DateTime TransformIntToDateTime(int days)
        {
            return DateTime.Today.AddDays(days);
        }

        [StepArgumentTransformation(@"'(\w+)' customer")]
        public Customer TransformStringToCustomer(string code)
        {
            return BaseDataConfigurationHooks.CustomerDict.ContainsKey(code)
                ? BaseDataConfigurationHooks.CustomerDict[code]
                : throw new ArgumentNullException(code, "There is no customer with provided code in the Customer dictionary.");
        }

        [StepArgumentTransformation(@"'(\w+)' location")]
        public Location TransformStringToLocation(string code)
        {
            return BaseDataConfigurationHooks.LocationDict.ContainsKey(code)
                ? BaseDataConfigurationHooks.LocationDict[code]
                : throw new ArgumentNullException(code, "There is no location with provided code in the Location dictionary.");
        }

        [StepArgumentTransformation(@"'(\w+)' cash center")]
        [StepArgumentTransformation(@"'(\w+)' CIT")]
        public Site TransformStringToSite(string name)
        {
            return BaseDataConfigurationHooks.SiteDict.ContainsKey(name)
                ? BaseDataConfigurationHooks.SiteDict[name]
                : throw new ArgumentNullException(name, "There is no site with provided name in the Site dictionary.");
        }

        [StepArgumentTransformation(@"'(\w+)' generic status")]
        public GenericStatus TransformStringToGenericStatus(string status)
        {
            return (GenericStatus)Enum.Parse(typeof(GenericStatus), status);
        }

        [StepArgumentTransformation(@"(^new$|^created$)")]
        public bool TransformStringToBool(string type)
        {
            if (String.Equals(type, "new"))
                return true;
            if (String.Equals(type, "created"))
                return false;
            throw new InvalidOperationException("Only 'new' or 'created' arguments are expected.");
        }
    }
}
