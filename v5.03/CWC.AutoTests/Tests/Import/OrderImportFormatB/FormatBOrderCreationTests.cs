using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Edsson.WebPortal.AutoTests.Tests.Import.OrderImportFormatB
{
    public class FormatBOrderCreationTests
    {
        [Fact(DisplayName = "Create file for import test")]
        public void CreateFile()
        {
            var service = new FileCreationService();
            //service.CreateLeadingRecord();
            //service.CreateOrderRecord();
            //service.CreateOrderItemRecord();
            //service.CreateOrderDeliveryInformationRecord();
            //service.CreateCloseRecord();

            //var firstRecord = service.file;


            var ser = new Cwc.Integration.OrderImportFormatB.Services.Impl.OrderImportFormatBJobService();

            ser.ProcessImportExpressDeliveryOrder(service.settings);
        }
    }
}
