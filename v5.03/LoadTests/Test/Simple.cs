using Edsson.WebPortal.AutoTests.Core;
using Edsson.WebPortal.AutoTests.Pages.Inbound;
using Edsson.WebPortal.AutoTests.Tests;
using System.Threading;

namespace LoadTests.Test
{
    public class Simple : LoadTest
    {
        public void Open()
        {
            this.RunInParrallelPerUserSession(Open);
        }

        private void Open(object prms)
        {
            var capturingPage = ((BasePage)prms).OpenPage<CapturingPage>();
            Thread.Sleep(1000);
            capturingPage.ScanDeposit(Thread.CurrentThread.ManagedThreadId.ToString() + "_deposit");            
        }
    }
}
