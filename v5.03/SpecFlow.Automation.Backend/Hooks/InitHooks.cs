using CWC.AutoTests.Helpers;
using Specflow.Automation.Backend.ValueRetrievers;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specflow.Automation.Backend.Hooks
{
    [Binding]
    public class InitHooks
    {
        [BeforeTestRun]
        public static void Init()
        {
            Service.Instance.RegisterValueRetriever(new ProductValueRetriever());
            ReplicationHelper.UpdateReplicationPartyRole(null);
            ReplicationHelper.UpdateReplicationPartyIsActiveFlag(false);
        }
    }
}
