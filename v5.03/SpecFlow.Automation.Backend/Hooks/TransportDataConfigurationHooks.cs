using CWC.AutoTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Specflow.Automation.Backend.Hooks
{
    [Binding]
    public class TransportDataConfigurationHooks
    {
        [BeforeFeature] 
        [AfterFeature]
        [Scope(Tag = "transport-data-clearing-required")]
        public static void ClearTransportData()
        {
            HelperFacade.TransportHelper.ClearTestData();
        }
    }
}
