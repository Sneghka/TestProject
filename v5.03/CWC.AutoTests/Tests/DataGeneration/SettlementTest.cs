using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Edsson.WebPortal.AutoTests.Tests.DataGeneration
{
    public class SettlementTest
    {
        string name = $"SPD{DateTime.Now.Ticks}";
        
        StockContainer[] sc;

        [Fact(DisplayName = "SC gen test")]
        public void Create()
        {

            Location loca = BaseDataFacade.LocationService.Load(39992321069);
            var par = new DataBaseParams();
            var n = name;

            sc = new StockContainer[1000000];

            for (int i = 0; i < sc.Length; i++)
            {
                sc[i] = new StockContainer
                {
                    LocationCode = "1101-SP101",
                    LocationFrom_id = 39992321069,
                    IsPreCrediting = true,
                    TotalValue = 1000,
                    StockLocation_id = 124

                };

                sc[i].SetNumber(n);
                sc[i].SetPreannouncementType(PreannouncementType.CustomerElectronic);
                sc[i].SetType(StockContainerType.Deposit);
                sc[i].SetStatus(SealbagStatus.Received);

                var res = CashCenterFacade.StockContainerService.Save(sc[i], par);

                sc[i] = new StockContainer
                {
                    LocationCode = "1101-SP101",
                    LocationFrom_id = 39992321069,
                    IsPreCrediting = true,
                    TotalValue = 1000,
                    StockLocation_id = 124
                };

                sc[i].SetNumber(n);
                sc[i].SetPreannouncementType(null);
                sc[i].SetType(StockContainerType.Deposit);
                sc[i].SetStatus(SealbagStatus.Captured);

                res = CashCenterFacade.StockContainerService.Save(sc[i], par);
            }
        }
    }
}
