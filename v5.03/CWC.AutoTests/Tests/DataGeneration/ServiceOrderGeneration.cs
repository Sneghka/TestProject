using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Edsson.WebPortal.AutoTests.Tests.DataGeneration
{
    public class ServiceOrderGeneration
    {
        [Fact(DisplayName = "test order creation")]
        public void CreateServiceOrder()
        {
            var s = new ServiOrderHelper();

            s.CreateServiceOrder();
        }
    }
}
