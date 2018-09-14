using Cwc.BaseData;
using CWC.AutoTests.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.ObjectBuilder.DailyDataBuilders
{
    public class DaiPackBuilder
    {
        dai_pack daiPack;
        public DaiPackBuilder()
        {
            daiPack = new dai_pack();
            daiPack.day_nr = 1;
        }

        public DaiPackBuilder With_DaiDate(DateTime value)
        {
            daiPack.dai_date = value;
            return this;
        }

        public DaiPackBuilder With_FrLocation(Location value)
        {
            daiPack.fr_loc_nr = value.ID;
            return this;
        }

        public DaiPackBuilder With_ToLocation(Location value)
        {
            daiPack.to_loc_nr = value.ID;
            return this;
        }

        public DaiPackBuilder With_ArrivalTime(string value)
        {
            daiPack.time_a = value;
            return this;
        }

        public DaiPackBuilder With_PackNumber(string value)
        {
            daiPack.pack_nr = value;
            return this;
        }

        public DaiPackBuilder With_Site(Site value)
        {
            daiPack.branch_cd = value.Branch_cd;
            return this;
        }

        public DaiPackBuilder With_BagType(int value)
        {
            daiPack.bgtyp_nr = value;
            return this;
        }

        public DaiPackBuilder with_MasterRoute(string routeCode)
        {
            daiPack.mast_cd = routeCode;
            return this;
        }

        public DaiPackBuilder SaveToDb()
        {
            using (var context = new ModelContext())
            {
                context.dai_packs.Add(daiPack);
                context.SaveChanges();
            }

            return this;
        }
        
    }
}
