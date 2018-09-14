using CWC.AutoTests.Model;
using Cwc.Replication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.Helpers
{
    public class ReplicationHelper
    {
        public static void UpdateReplicationPartyRole(ReplicationRole? role)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var replicationParty = context.ReplicationParties.First(x => x.PartyType == PartyType.Local);

                replicationParty.Role = role;
                context.SaveChanges();
            }
        }

        public static void UpdateReplicationPartyIsActiveFlag(bool flag)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var replicationParty = context.ReplicationParties.First(x => x.PartyType == PartyType.Local);

                replicationParty.IsActive = flag;
                context.SaveChanges();
            }
        }
    }
}
