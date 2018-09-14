using Cwc.CashCenter;

namespace Specflow.Automation.Backend.Helpers
{
    public class CashCenterOrderAllocationHelper
    {
        public CashCenterOrderAllocationHelper()
        {
        }

        public void RunCashCenterOrderAllocation()
        {
            try
            {
                CashCenterFacade.OrderAllocationManagementService.AllocateServiceOrdersToOutboundOrders(new Cwc.Common.DataBaseParams());
            }
            catch
            {
                throw;
            }
        }
    }
}
