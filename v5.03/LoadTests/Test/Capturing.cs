using Edsson.WebPortal.AutoTests.Core;
using Edsson.WebPortal.AutoTests.Helpers.Inbound;
using Edsson.WebPortal.AutoTests.Pages.Inbound;
using Edsson.WebPortal.AutoTests.Tests;
using System.Collections.Generic;
using System.Linq;

namespace LoadTests.Test
{
    public class Capturing : LoadTest
    {
        private const int preannouncementsPerUser = 10;

        public List<string> Preanns { get; set; }

        public Capturing() : base()
        {
            var sessionsCount = this.Pages.Count;
            SetupSettings();
            this.Preanns = (new DataGenerationHelper()).GenerateDepositNumberList(sessionsCount * preannouncementsPerUser, Configuration.LocationCode);
        }

        private void SetupSettings()
        {
        }

        public void TestFastWay()
        {
            this.RunInParrallelPerUserSession(TestFastWay);
        }

        protected override object GetParameters(BasePage page)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("page", page);
            var preannouncements = this.Preanns.Skip(this.Pages.IndexOf(page) * preannouncementsPerUser).Take(preannouncementsPerUser).ToList();
            dictionary.Add("preannouncements", preannouncements);
            return dictionary;
        }

        private void TestFastWay(object parameters)
        {
            var page = this.ParseParameter<BasePage>("page", parameters);
            var preannouncements = this.ParseParameter<List<string>>("preannouncements", parameters);

            var capturingPage = page.OpenPage<CapturingPage>();
            CapturingFormHelper capturingFormHelper = new CapturingFormHelper(capturingPage);
            //capturingFormHelper.CaptureDeposits(preannouncements, Cwc.CashCenter.CapturingMode.NoBatch, true); 
        }

        private T ParseParameter<T>(string key, object parameters)
        {
            return (T)(parameters as Dictionary<string, object>)[key];
        }
    }
}
