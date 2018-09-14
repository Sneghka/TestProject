using Cwc.Constants;
using Cwc.Transport;
using Cwc.Transport.Model;
using Cwc.Ordering;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CWC.AutoTests.Tests.Transport.PackageLifeCycle
{
    public class PlcFixture : IDisposable
    {
        public Order orderDelv;
        public Order orderRepl;
        public Order orderColl;
        public PackageLifeCycleProcessingJobSettings setting;
        
        public PlcFixture()
        {
            var location = DataFacade.Location.Take(x => x.Code == "SP02").Build();
            var servTypeDelv = DataFacade.ServiceType.Take(x => x.Code == "DELV").Build();
            var servTypeRepl = DataFacade.ServiceType.Take(x => x.Code == "REPL").Build();
            var servTypeColl = DataFacade.ServiceType.Take(x => x.Code == "COLL").Build();
            orderDelv = DataFacade.Order.New(DateTime.Today, location, servTypeDelv.Code).SaveToDb().Build();
            orderRepl = DataFacade.Order.New(DateTime.Today, location, servTypeRepl.Code).SaveToDb().Build();
            orderColl = DataFacade.Order.New(DateTime.Today, location, servTypeColl.Code).SaveToDb().Build();
            setting = new PackageLifeCycleProcessingJobSettings();
            setting.PreviousStarted = DateTime.Now.Date;
        }

        public void CreateLinks(TransportOrder order)
        {
            var servTypes = new[] { DataFacade.ServiceType.Take(x => x.Code == "COLL").Build(), DataFacade.ServiceType.Take(x => x.Code == "DELV").Build(), DataFacade.ServiceType.Take(x => x.Code == "SERV").Build() };
            foreach (var item in servTypes)
            {
                var transportOrderLink = new TransportOrderIntegrationJobTransportOrderLink();
                transportOrderLink.OrderID = item.OldType == ServiceTypeConstants.Deliver ? order.Code : Utils.ValueGenerator.GenerateString("SP", 10);
                transportOrderLink.TransportOrderID = order.ID;
                transportOrderLink.ServiceTypeID = item.ID;
                TransportFacade.TransportOrderIntegrationJobTransportOrderLinkService.Save(transportOrderLink, null, null);
            }
        }

        public void Dispose()
        {
            DataFacade.Order.DeleteMany(new[] { orderDelv, orderRepl });
        }
    }
}
