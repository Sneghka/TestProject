using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.BaseData.Model;
using Cwc.Contracts;
using Cwc.Ordering;
using Cwc.Transport;
using Cwc.Security;
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
    public class TransportOrderServiceInterfaceTests : IClassFixture<TransportOrderJobFixture>, IDisposable
    {
        string defaultDate, defaultOrderID, defaultFileName;
        Cwc.BaseData.Model.ServicingCode defaultServiceCode;
        DateTime defaultTransportOrderDate;
        TransportOrderJobFixture fixture;
        DataModel.ModelContext context;
        public TransportOrderServiceInterfaceTests(TransportOrderJobFixture fixt)
        {
            context = new DataModel.ModelContext();
            fixture = fixt;
            defaultDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            defaultOrderID = $"{DateTime.Now.ToString("yyyyMMddhhmmssfff")}";
            defaultFileName = $"SOService-{defaultOrderID}";
            defaultServiceCode = DataFacade.ServicingCode.Take(x => x.Code != null).Build();
            defaultTransportOrderDate = DataFacade.TransportOrder.DefineApplicableTransportOrderDate(fixture.defaultLocation, fixture.deliveryServiceType, OrderType.AtRequest);          
        }

        [Theory(DisplayName = "When transport order service is not existed Then System doesn't allow to update or delete it")]
        [InlineData(1)]
        [InlineData(2)]
        public void VerifyThatSystemDoentAllowToUpdateOrDeleteTransportOrderService(int action)
        {
            var orderService = TransportOrderInterfaceFacade.TransportOrderService.New(action)
                .WithOrderLineID(defaultOrderID)
                .WithServicingCode(defaultServiceCode.Code)
                .WithIsServicePerformed(false)
                .WithIsServicePlanned(true).SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundMsg = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(m => m.FileName.StartsWith(defaultFileName));

            Assert.NotNull(foundMsg);
            Assert.Equal($"Service code {defaultServiceCode.Code} does not found for transport order with Code {defaultOrderID.Substring(0, defaultOrderID.Length - 2)}.", foundMsg.Message);

        }

        [Fact(DisplayName = "When transport order service with {orderline, servcode} is already exists Then System doesn't allow to import it")]
        public void VerifyThatSystemDoesntAllowToImportNonUniqueTransportOrderService()
        {
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();
            var author = SecurityFacade.LoginService.GetAdministratorLogin();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation).With_Site(fixture.defaultLocation.BranchID).With_ServiceType(fixture.deliveryServiceType)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(defaultTransportOrderDate).With_ServiceDate(serviceOrder.ServiceDate)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).SaveToDb().Build();

            var transportOrderService = DataFacade.TransportOrderServ.New()
                .With_IsPerformed(false)
                .With_IsPlanned(true)
                .With_Service(defaultServiceCode.Code)
                .With_TransportOrderID(transportOrer.ID)
                .With_AuthorID(author.UserID)
                .With_EditorID(author.UserID)
                .SaveToDb();

            var orderService = TransportOrderInterfaceFacade.TransportOrderService.New(0)
                .WithOrderLineID($"{transportOrer.Code}-1")
                .WithServicingCode(defaultServiceCode.Code)
                .WithIsServicePerformed(false)
                .WithIsServicePlanned(true)
                .SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundMsg = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(m => m.FileName.StartsWith(defaultFileName));
            var foundTransportServices = context.Cwc_Transport_TransportOrderServs.Where(s=>s.TransportOrderID == transportOrer.ID);

            Assert.NotNull(foundMsg);
            Assert.Equal($"Service code {defaultServiceCode.Code} already linked to transport order with Code {transportOrer.Code}.", foundMsg.Message);
            Assert.Equal(1, foundTransportServices.Count());
        }

        [Fact(DisplayName = "When transport order service - servcode is not existed in System Then System doesn't allow to create it it")]
        public void VerifyThatSystemDoesntAllowToImportTransportOrderServiceWithNonExistedServiceCode()
        {
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_Site(fixture.defaultLocation.BranchID)
                .With_ServiceType(fixture.deliveryServiceType)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(defaultTransportOrderDate)
                .With_ServiceDate(serviceOrder.ServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder).SaveToDb().Build();

            var orderService = TransportOrderInterfaceFacade.TransportOrderService.New(0)
                .WithOrderLineID($"{transportOrer.Code}-1")
                .WithServicingCode("1122334455")
                .WithIsServicePerformed(false)
                .WithIsServicePlanned(true).SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundMsg = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(m => m.FileName.StartsWith(defaultFileName));
            var foundTransportServices = context.Cwc_Transport_TransportOrderServs.Where(s => s.TransportOrderID == transportOrer.ID);

            Assert.NotNull(foundMsg);
            Assert.Equal($"There is no service code 1122334455.", foundMsg.Message);
            Assert.Equal(0, foundTransportServices.Count());
        }

        [Fact(DisplayName = "When transport order service is valid Then System creates it")]
        public void VerifyThatSystemCreatesTrasnportOrderServiceProperly()
        {
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_Site(fixture.defaultLocation.BranchID)
                .With_ServiceType(fixture.deliveryServiceType)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(defaultTransportOrderDate)
                .With_ServiceDate(serviceOrder.ServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder)
                .SaveToDb().Build();

            var orderService = TransportOrderInterfaceFacade.TransportOrderService.New(0)
                .WithOrderLineID($"{transportOrer.Code}-1")
                .WithServicingCode(defaultServiceCode.Code)
                .WithIsServicePerformed(false)
                .WithIsServicePlanned(true).SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundTransportService = context.Cwc_Transport_TransportOrderServs.FirstOrDefault(x => x.TransportOrderID == transportOrer.ID);

            Assert.NotNull(foundTransportService);
            Assert.Equal(defaultServiceCode.Code, foundTransportService.ServiceID);
            Assert.Equal(false, foundTransportService.IsPerformed);
            Assert.Equal(true, foundTransportService.IsPlanned);
        }

        [Fact(DisplayName = "When transport order service is valid Then System updates it")]
        public void VerifyThatSystemUpdatesTrasnportOrderServiceProperly()
        {
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();
            var author = SecurityFacade.LoginService.GetAdministratorLogin();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation)
                .With_Site(fixture.defaultLocation.BranchID)
                .With_ServiceType(fixture.deliveryServiceType)
                .With_OrderType(OrderType.AtRequest)
                .With_TransportDate(defaultTransportOrderDate)
                .With_ServiceDate(serviceOrder.ServiceDate)
                .With_Status(TransportOrderStatus.Planned)
                .With_ServiceOrder(serviceOrder)
                .SaveToDb().Build();

            var transportOrderService = DataFacade.TransportOrderServ.New()
                .With_IsPerformed(false)
                .With_IsPlanned(true)
                .With_Service(defaultServiceCode.Code)
                .With_TransportOrderID(transportOrer.ID)
                .With_AuthorID(author.UserID)
                .With_EditorID(author.UserID)
                .SaveToDb();

            var orderService = TransportOrderInterfaceFacade.TransportOrderService.New(1)
                .WithOrderLineID($"{transportOrer.Code}-1")
                .WithServicingCode(defaultServiceCode.Code)
                .WithIsServicePerformed(true)
                .WithIsServicePlanned(false).SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundTransportService = context.Cwc_Transport_TransportOrderServs.FirstOrDefault(x => x.TransportOrderID == transportOrer.ID);

            Assert.NotNull(foundTransportService);
            Assert.Equal(defaultServiceCode.Code, foundTransportService.ServiceID);
            Assert.Equal(true, foundTransportService.IsPerformed);
            Assert.Equal(false, foundTransportService.IsPlanned);
        }

        [Fact(DisplayName = "When transport order service is valid Then System deletes it")]
        public void VerifyThatSystemDeletesTrasnportOrderServiceProperly()
        {
            var serviceOrder = DataFacade.Order.Take(x => x.GenericStatus == GenericStatus.Registered).Build();
            var author = SecurityFacade.LoginService.GetAdministratorLogin();

            var transportOrer = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation).With_Site(fixture.defaultLocation.BranchID).With_ServiceType(fixture.deliveryServiceType)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(defaultTransportOrderDate).With_ServiceDate(serviceOrder.ServiceDate)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).SaveToDb().Build();

            var transportOrderService = DataFacade.TransportOrderServ.New()
                .With_IsPerformed(false)
                .With_IsPlanned(true)
                .With_Service(defaultServiceCode.Code)
                .With_TransportOrderID(transportOrer.ID)
                .With_AuthorID(author.UserID)
                .With_EditorID(author.UserID)
                .SaveToDb();

            var orderService = TransportOrderInterfaceFacade.TransportOrderService.New(2)
                .WithOrderLineID($"{transportOrer.Code}-1")
                .WithServicingCode(defaultServiceCode.Code)
                .WithIsServicePerformed(false)
                .WithIsServicePlanned(true).SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundTransportService = context.Cwc_Transport_TransportOrderServs.FirstOrDefault(x => x.TransportOrderID == transportOrer.ID);

            Assert.Null(foundTransportService);
        }

        [Fact(DisplayName = "When transport order service is already performed Then System doesn't allow to delete it")]
        public void VerifyWhenServiceIsAlreadyPerformedThenSystemDoesntAllowToUpdateIt()
        {
            var serviceOrder = DataFacade.Order.Take(x => x.ID != null).Build();

            var transportOrder = DataFacade.TransportOrder.New()
                .With_Location(fixture.defaultLocation).With_Site(fixture.defaultLocation.BranchID).With_ServiceType(fixture.deliveryServiceType)
                .With_OrderType(OrderType.AtRequest).With_TransportDate(defaultTransportOrderDate).With_ServiceDate(serviceOrder.ServiceDate)
                .With_Status(TransportOrderStatus.Planned).With_ServiceOrder(serviceOrder).SaveToDb().Build();

            var transportOrderServiceCreated = DataFacade.TransportOrderServ.New()
                .With_TransportOrderID(transportOrder.ID)
                .With_IsPlanned(true)
                .With_IsPerformed(true)
                .With_Service(defaultServiceCode.Code)
                .SaveToDb();

            var transportOrderServiceDeleted = TransportOrderInterfaceFacade.TransportOrderService.New(2)
                .WithOrderLineID($"{transportOrder.Code}-1")
                .WithServicingCode(defaultServiceCode.Code)
                .WithIsServicePerformed(true)
                .WithIsServicePlanned(true).SaveToFolder(fixture.folderPath, defaultFileName);

            TransportFacade.TransportOrderIntegrationJobService.ProcessFiles(fixture.settings);

            var foundMsg = context.Cwc_Transport_TransportOrderIntegrationJobImportLogs.FirstOrDefault(x => x.FileName.StartsWith(defaultFileName));
            var foundNonDeletedService = context.Cwc_Transport_TransportOrderServs.FirstOrDefault(x => x.TransportOrderID == transportOrder.ID);

            Assert.NotNull(foundMsg);
            Assert.NotNull(foundNonDeletedService);
            Assert.Equal($"It is not allowed to unassign performed service {defaultServiceCode.DisplayCaption} from transport order {transportOrder.Code}.", foundMsg.Message.TrimEnd());
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
