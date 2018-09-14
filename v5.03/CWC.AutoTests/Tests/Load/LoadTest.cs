using Edsson.WebPortal.AutoTests.Core;
using Edsson.WebPortal.AutoTests.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Edsson.WebPortal.AutoTests.Tests
{
    public abstract class LoadTest : IDisposable
    {
        //protected delegate void ThreadMethod(object parameters);

        public List<BasePage> Pages { get; set; }


        public LoadTest()
        {
            var userSessionsList = UserSessionHelper.GetUserSessionSettings();
            this.Pages = new List<BasePage>();

            foreach (var userSession in userSessionsList)
            {
                var driver = DriverHelper.InitDriver(Configuration.Browser);                
                var homePage = AuthorizationHelper.LogInToPortal(driver, userSession.Login, userSession.Password, userSession.Workstation);
                this.Pages.Add(homePage);
            }
	    }

        public LoadTest(LoadTestFixture fixture)
        {
        }

        protected void RunInParrallelPerUserSession(Action<object> method)
        {
            var threads = new List<Thread>();
            foreach (var page in this.Pages)
            {
                var thread = new Thread(new ParameterizedThreadStart(method));
                thread.Start(this.GetParameters(page));
                threads.Add(thread);
            }

            threads.ForEach(t => t.Join());
        }

        public void Run(string methodName)
        {
            var method = this.GetType().GetMethod(methodName);
            if (method == null)
            {
                throw new Exception("No such method " + methodName);
            }

            method.Invoke(this, null);
        }

        protected virtual object GetParameters(BasePage page)
        {
            return page;
        }

        public void Dispose()
        {
            foreach (var page in this.Pages)
            {
                DriverHelper.DisposeDriver(page.Driver);
            }
        }
    }
}
