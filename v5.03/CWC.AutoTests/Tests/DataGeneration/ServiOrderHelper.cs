using Cwc.BaseData;
using Cwc.Ordering;
using Cwc.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edsson.WebPortal.AutoTests.Tests.DataGeneration
{
    class ServiOrderHelper
    {
        int count = 0;
        public void CreateServiceOrder()
        {
            var order = new Order();
            order.ID = this.SetOrderNumber();
            order.Location = BaseDataFacade.LocationService.LoadLocationByCode("SP02");
            order.GenericStatus = GenericStatus.Registered;
            order.ServiceDate = DateTime.Now.AddDays(4);
            order.OrderType = Cwc.Contracts.OrderType.AtRequest;
            order.ServiceTypeCode = DateTime.Now.Ticks % 2 == 0 ?
                BaseDataFacade.ServiceTypeService.LoadByCode("COLL", new Cwc.Common.DataBaseParams()).Code :
                BaseDataFacade.ServiceTypeService.LoadByCode("DELV", new Cwc.Common.DataBaseParams()).Code;
            order.Currency = BaseDataFacade.CurrencyService.Load(BaseDataFacade.CurrencyService.GetDefaultCurrencyID(), new Cwc.Common.DataBaseParams());
            order.EditorID = SecurityFacade.LoginService.GetAdministratorLogin().UserID;
            order.DateUpdated = DateTime.Now;
            order.SetWPID(2000000);
            order.IdentityID = 2000000;
            order.SetIsNew(true);
            order.CustomerID = BaseDataFacade.CustomerService.LoadCustomerByReferenceNumber("1101", CustomerRecordType.Company, new Cwc.Common.DataBaseParams()).Cus_nr;
            order.SetLocationCode(order.Location.LtCode);
            order.SetBranchCode("SP");

            var soline = order.CreateNewOrderLine($"{order.ID}-1");
            soline.ServiceType = order.ServiceTypeCode;
            soline.LocationID = BaseDataFacade.LocationService.LoadLocationByCode("SP02").ID;
            soline.LocationToNumber = BaseDataFacade.LocationService.LoadLocationByCode("JG02").ID;

            OrderLineProduct sop = new OrderLineProduct(soline, "NOTE");
            sop.CurrencyCode = "EUR";
            sop.ProductCode = "25";

            var res = OrderingFacade.OrderService.SaveOrder(order, SecurityFacade.LoginService.GetAdministratorLogin().UserID);

            if (!res.IsSuccess)
            {
                throw new InvalidOperationException(res.GetMessage());
            }

        }

        private string SetOrderNumber()
        {
            return $"SPO{DateTime.Now.ToString("yyyyMMdd")}{count++}";
        }
    }
}
