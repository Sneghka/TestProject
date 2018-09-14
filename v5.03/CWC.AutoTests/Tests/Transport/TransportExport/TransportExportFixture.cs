using Cwc.BaseData;
using Cwc.BaseData.Classes;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.Tests.Transport.TransportExport
{
    public class TransportExportFixture
    {
        public Location defaultLocation;
        public ServiceType defaultServiceType;
        public TransportExportFixture()
        {
            defaultLocation = DataFacade.Location.Take(l => l.Code == "SP02");
            defaultServiceType = DataFacade.ServiceType.Take(st => st.Code == "DELV");
        }
    }
}
