using Cwc.BaseData;
using Cwc.Transport.Model;
using CWC.AutoTests.DataModel;
using CWC.AutoTests.Model;
using System;

namespace CWC.AutoTests.ObjectBuilder.DailyDataBuilders
{
    public class HisPackBuilder
    {
        PackageLifeCycle hisPack;
        public HisPackBuilder()
        {            
        }

        public HisPackBuilder With_Date(DateTime value)
        {
            hisPack.ActionDate = value;           
            return this;
        }

        public HisPackBuilder With_Time(string value)
        {
            hisPack.ActionTime = value;
            return this;
        }

        public HisPackBuilder With_RealDate(DateTime value)
        {
            hisPack.RealDate = value;
            return this;
        }

        public HisPackBuilder With_RealTime(string value)
        {
            hisPack.RealTime = value;
            return this;
        }

        public HisPackBuilder With_Status(string value)
        {
            hisPack.ActionStatus = value;
            return this;
        }

        public HisPackBuilder With_FrLocation(Location value = null)
        {
            if (value != null)
            {
                hisPack.FromLocationNumber = value.ID;
            }
            
            return this;
        }

        public HisPackBuilder With_ToLocation(Location value = null)
        {
            if (value != null)
            {
                hisPack.ToLocationNumber = value.ID;
            }

            return this;
        }

        public HisPackBuilder With_ToLocation(decimal value)
        {

            hisPack.ToLocationNumber = value;

            return this;
        }

        public HisPackBuilder With_Site(Site value)
        {
            hisPack.SiteId = value.Branch_cd;
            return this;
        }

        public HisPackBuilder With_Site(int value)
        {
            var site = BaseDataFacade.SiteService.Load(value);
            if (site != null)
            {
                hisPack.SiteId = site.Branch_cd;
            }

            return this;
        }

        public HisPackBuilder With_Site(branch value)
        {
            hisPack.SiteId = value.branch_cd;
            return this;
        }

        public HisPackBuilder With_SiteCode(string value)
        {
            hisPack.SiteId = value;
            return this;
        }

        public HisPackBuilder With_PackVal(decimal value)
        {
            hisPack.Value = value;
            return this;
        }

        public HisPackBuilder With_BagType(int value)
        {
            hisPack.BagTypeNumber = value;
            return this;
        }

        public HisPackBuilder With_PackNr(string value)
        {
            hisPack.PackageNumber = value;
            return this;
        }

        public HisPackBuilder With_OrderID(string value)
        {
            hisPack.TransportOrderCode = value;
            return this;
        }

        public HisPackBuilder With_MasterRoute(string value)
        {
            hisPack.RouteNumberCode = value;
            return this;
        }

        public HisPackBuilder With_NewDeliveryDate(DateTime date)
        {
            hisPack.NewDeliveryDate = date;
            return this;
        }

        public HisPackBuilder New()
        {
            hisPack = new PackageLifeCycle();            
            return this;
        }

        public PackageLifeCycle Build()
        {
            return this.hisPack;
        }

        public static implicit operator PackageLifeCycle(HisPackBuilder ins)
        {
            return ins.Build();
        }

        public HisPackBuilder SaveToDb()
        {
            using (var context = new AutomationTransportDataContext())
            {
                context.PackageLifeCycles.Add(hisPack);
                context.SaveChanges();
            }

            return this;
        }

        public HisPackBuilder InitDefault(TransportOrder order)
        {
            if (order == null)
            {
                throw new InvalidOperationException("His_pack cannot be created from an empty Transport Order");
            }

            var location = DataFacade.Location.Take(x => x.ID == order.LocationID).Build();

            this.hisPack = DailyDataFacade.HisPack.New()
                .With_Date(order.TransportDate)
                .With_ToLocation(location)
                .With_FrLocation(location)
                .With_MasterRoute(order.MasterRouteCode)
                .With_OrderID(order.Code)
                .With_PackNr(Utils.ValueGenerator.GenerateString("SP", 10))
                .With_SiteCode(DataFacade.Site.Take(x => x.ID == location.BranchID).Build().Branch_cd)
                .With_Time(order.StopArrivalTime?.ToString("hhmmss"))
                .With_RealDate(order.TransportDate)
                .With_RealTime(order.StopArrivalTime?.ToString("hhmmss"));

            return this;
        }
    }
}
