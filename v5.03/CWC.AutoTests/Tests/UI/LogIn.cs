using System;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Pages;
using Xunit;
using CWC.AutoTests.Core;

namespace CWC.AutoTests.Tests
{
    /// <summary>
    /// The log in page test.
    /// </summary>
    [Collection("Test Collection #1")]
    public class LogInPageTest : BaseTest, IClassFixture<TestFixture>
    {
        TestFixture fixture;

        public LogInPageTest(TestFixture setupClass)
        {
            this.fixture = setupClass;
        }

        /// <summary>
        /// User can log in to portal
        /// </summary>
        [Fact(DisplayName = "Log in Test")] 
        public void LogInTest()
        {
            Assert.True(fixture.HomePage.OperatorLbl.Displayed);            
        }
    }
}
