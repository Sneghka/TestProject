using Cwc.Transport;
using Cwc.Transport.Model;
using Specflow.Automation.Backend.Hooks;
using System;

namespace Specflow.Automation.Backend.Helpers
{
    public class OrderCitAllocationHelper
    {

        public OrderCitAllocationHelper()
        {
        }

        public void RunOrderCitAllocation()
        {
            try
            {
                TransportFacade.OrderCitAllocationManagementService.AllocateServiceOrdersToTransportOrders(new Cwc.Common.DataBaseParams());
            }
            catch
            {
                throw;
            }
        }

        public int? GetTransportOrderServiceTypeId(string ServiceType, TransportOrder transportOrder )
        {
            switch (ServiceType)
            {
                case "Delivery":
                    return transportOrder.DeliveryServiceTypeId;
                case "Collection":
                    return transportOrder.CollectServiceTypeId;                
                case "Servicing":
                    return transportOrder.ServicingServiceTypeId;
                default:
                    throw new InvalidOperationException("Any Service Type is not linked to the transport order");
            }
        }

    }
}
