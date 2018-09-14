using Cwc.BaseData;
using Cwc.Common;
using Cwc.Security;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CWC.AutoTests.Tests.Matching
{
    public class MatchDebtorTests
    {
        DataBaseParams prms;
        public MatchDebtorTests()
        {
            prms = new DataBaseParams();
        }

        [Fact(DisplayName = "When Customer Contains Debtor Then System matches it")]
        public void VerifyWhenCustomerContainsDebtorThenSystemmatchesIt()
        {
            var location = DataFacade.Location.Take(x => x.Code == "SP02").Build();
            var customer = DataFacade.Customer.Take(x => x.ID == location.CompanyID);
            var debtor = DataFacade.Customer.Take(x => x.RecordType == CustomerRecordType.Debtor).Build();

            customer.With_Debtor(debtor).SaveToDb();

            var result = BaseDataFacade.MatchingService.MatchDebtor(location, prms);

            Assert.Equal(result.ID, customer.Build().Debtor_nr);
        }

        [Fact(DisplayName = "When Customer doesn't Contain Debtor Then System returns null")]
        public void VerifyThatWhenCustomerDoesntContainDebtorThenSystemReturnsNull()
        {
            var location = DataFacade.Location.Take(x => x.Code == "SP02").Build();
            var customer = DataFacade.Customer.Take(x => x.ID == location.CompanyID);

            customer.With_Debtor(null).SaveToDb();

            var result = BaseDataFacade.MatchingService.MatchDebtor(location, prms);

            Assert.Null(result);
        }
    }
}
