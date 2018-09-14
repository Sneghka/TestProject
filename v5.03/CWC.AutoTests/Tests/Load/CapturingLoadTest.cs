using Edsson.WebPortal.AutoTests.Core;
using Edsson.WebPortal.AutoTests.Helpers.Inbound;
using Edsson.WebPortal.AutoTests.Pages.Inbound;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Edsson.WebPortal.AutoTests.Tests
{
    public class CapturingLoadTest : LoadTest//, IClassFixture<LoadTestFixture>
    {

        //public CapturingLoadTest(LoadTestFixture fixture)
        //    : base(fixture)
        //{
        //    // once per test run
        //}

        [Fact]
        public void LoadTestOpenCapturingForm()
        {
            this.RunInParrallelPerUserSession(_LoadTestOpenCapturingForm);
        }

        private void _LoadTestOpenCapturingForm(object prms)
        {
            var capturingPage = ((BasePage)prms).OpenPage<CapturingPage>();
            Thread.Sleep(1000);
            capturingPage.ScanDeposit(Thread.CurrentThread.ManagedThreadId.ToString() + "_deposit");            
        }
    }
}
