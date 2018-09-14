using Cwc.BaseData;
using Cwc.Transport;
using Cwc.Transport.Services.Impl;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CWC.AutoTests.Tests.Transport.TransportInterface
{
    public class ContractedReplenishmentDefinitionTests
    {
        DataModel.ModelContext context;
        Location location;
        Customer customer;
        List<int> replTypes;
        public ContractedReplenishmentDefinitionTests()
        {
            context = new DataModel.ModelContext();
            location = DataFacade.Location.Take(l => l.Code == "SP02");
            customer = DataFacade.Customer.Take(c=>c.ReferenceNumber == "1101");
            replTypes = context.WP_ServiceTypes.Where(x => x.OldType == "Replenishment").Select(x=>x.id).ToList();
            DataFacade.CitProcessSettings.MatchByLocation(location).RemoveReplenishmentConfigurationFromSetting();
        }
    }
}
