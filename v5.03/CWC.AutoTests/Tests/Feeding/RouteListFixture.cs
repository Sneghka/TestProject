using Cwc.BaseData;
using CWC.AutoTests.ObjectBuilder;
using System;

namespace CWC.AutoTests.Tests.Feeding
{
    //[Xunit.CollectionDefinition("RouteListFeeding")]
    public class RouteListFixture
    {
         public Site Site { get; set; } = DataFacade.Site.Take(x => x.Branch_cd == "SP");
         public Location Location1 { get; set; } = DataFacade.Location.Take(x => x.Code == "SP02").Build();
         public Location Location2 { get; set; } = DataFacade.Location.Take(x => x.Code == "1101-SP10").Build();
         public Location VisitAddressLocation { get; set; } = DataFacade.Location.Take(x => x.Code == "JGVISIT").Build();
         public TimeSpan StopArrivalTime { get; set; } = new TimeSpan(10, 0, 0);
         public TimeSpan StopOnSIteTime { get; set; } = new TimeSpan(0, 10, 0);
         public TimeSpan TravelTime { get; set; } = new TimeSpan(0, 15, 0);
        public TimeSpan DepartureTime { get; set; } = new TimeSpan(10, 10, 0);
    }
}
