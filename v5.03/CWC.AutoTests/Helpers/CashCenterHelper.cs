using Cwc.BaseData;
using Cwc.Constants;
using Cwc.Ordering;
using CWC.AutoTests.ObjectBuilder;
using System;

namespace CWC.AutoTests.Helpers
{
    public class CashCenterHelper
    {
        public void CreatePickAndPackStockOrder(Order serviceOrder, Location location)
        {
            var dateTime = DateTime.Now;
            DataFacade.StockOrder.New()
                                        .With_Number($"SO3303{ dateTime.ToString("ddMMyyyyhhmmss")}")
                                        .With_ServiceDate(serviceOrder.ServiceDate)
                                        .With_Status(Cwc.CashCenter.StockOrderStatus.Completed)
                                        .With_TotalQuantity(0)
                                        .With_TotalValue(0)
                                        .With_TotalWeight(0)
                                        .With_IsBilled(false)
                                        .With_ServiceType_id(DataFacade.ServiceType.Take(s => s.OldType == ServiceTypeConstants.PickAndPack).Build().ID)
                                        .With_Site_id(DataFacade.Site.Take(s => s.Branch_cd == "JG").Build().ID)
                                        .With_CITDepotID(location.ServicingDepotID)
                                        .With_LocationTo_id(location.ID)
                                        .With_ServiceOrder_id(serviceOrder.ID)
                                        .With_CustomerID(location.CompanyID)
                                        .SaveToDb();
        }
    }
}
