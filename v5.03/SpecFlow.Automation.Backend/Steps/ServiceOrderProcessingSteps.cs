using Cwc.BaseData;
using Cwc.Ordering;
using CWC.AutoTests.Helpers;
using Specflow.Automation.Backend.Model;
using Specflow.Automation.Backend.Utils;
using System;
using TechTalk.SpecFlow;

namespace Specflow.Automation.Backend.Steps
{
    [Binding]
    public class ServiceOrderProcessingSteps
    {
        ServiceOrderContainer serviceOrderContainer;

        public ServiceOrderProcessingSteps(ServiceOrderContainer container)
        {
            serviceOrderContainer = container;
        }

        [Given(@"(\w+) service order with service date in (.*), (.*) and following content is created for (.*)")]
        public void GivenServiceOrderIsCreated(string serviceType, DateTime serviceDate, GenericStatus status, Location location, Table table)
        {
            var serviceTypeCode = serviceType.ConvertToServiceTypeCode();
            var deliveryContent = table.ToDeliveryContentList();
            serviceOrderContainer.ServiceOrder = HelperFacade.TransportHelper.CreateServiceOrderWithContentLite(serviceTypeCode, serviceDate, status, location, deliveryContent);
        }
    }
}
