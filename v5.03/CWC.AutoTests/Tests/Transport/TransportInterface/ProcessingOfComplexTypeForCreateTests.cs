using Cwc.Contracts;
using Cwc.Ordering;
using Cwc.Transport;
using Cwc.Transport.Enums;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.TransportOrderInterfaceBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CWC.AutoTests.Tests.Transport.TransportInterface
{
    public class ProcessingOfComplexTypeForCreateTests : IClassFixture<TransportOrderJobFixture>, IDisposable
    {
        const string ServiceOrderPart = "ServiceOrder-";
        const string ServiceOrdeLinerPart = "SOline-";
        TransportOrderJobFixture fixture;
        string defaultFileName, defaultOrderID;
        DataModel.ModelContext context;

        public ProcessingOfComplexTypeForCreateTests(TransportOrderJobFixture fixt)
        {
            fixture = fixt;
            context = new DataModel.ModelContext();
            defaultFileName = $"{DateTime.Now.ToString("yyyyMMddhhmmssfff")}";
            defaultOrderID = $"SPO{DateTime.Now.Ticks}";
            fixture.CreateSettings(fixture.collectServiceType, fixture.deliveryServiceType, fixture.servicingServiceType);
        }

        [Fact(DisplayName = "When Order And Line Are Received In Que Then System Creates New Transport Order")]
        public void VerifyWhenBothServiceOrderAndLineArePresentInQueThenSystemCreatesNewTransportOrder()
        {
            var defaultDate = DateTime.Now;
            var arrivalTime = new TimeSpan(8, 0, 0);

            var transportOrder = TransportOrderInterfaceFacade.TransportOrder.New(0)
                .WithOrderID(defaultOrderID)
                .WithCusnr(fixture.defaultCustomer.ReferenceNumber)
                .WithServiceType_Code(fixture.deliveryServiceType.Code)
                .WithLocationID(fixture.defaultLocation.ID)
                .WithLocationCode(fixture.defaultLocation.Code)
                .WithOrder_Status(TransportOrderStatus.Registered.ToString())
                .WithServiceDate($"{defaultDate.ToString("yyyy-MM-dd")} 00:00:00")
                .WithOrder_Type((int)OrderType.AtRequest)
                .WithOrder_Level((int)OrderLevel.Location)
                .WithSiteCode(context.branches.First(x=>x.branch_nr == fixture.defaultLocation.BranchID).branch_cd)
                .WithDateCreated(defaultDate.ToString("yyyy-MM-dd hh:mm:ss"))
                .WithDateUpdated(defaultDate.ToString("yyyy-MM-dd hh:mm:ss"))
                .SaveToFolder(fixture.folderPath, ServiceOrderPart+defaultFileName);

            var transportOrderLine = TransportOrderInterfaceFacade.TransportOrderLine.New(0)
                .WithOrderLineID($"{defaultOrderID}-1")
                .WithOrderID(defaultOrderID)
                .WithOrderLineStatus(TransportOrderStatus.Planned.ToString())
                .WithlocationID(fixture.defaultLocation.ID)
                .WithMasterRoute(fixture.routeCode)
                .WithDaiDate(defaultDate.ToString("yyyy-MM-dd"))
                .WithArrivalTime($"{arrivalTime.ToString("hhmm")}")
                .WithDayNumber(1)
                .WithBranchNumber(fixture.defaultLocation.BranchID)
                .WithVisitSequence(1)
                .WithServiceType(fixture.deliveryServiceType.OldType)
                .WithOrderLineValue(1).
                SaveToFolder(fixture.folderPath, ServiceOrdeLinerPart+defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
