using Cwc.BaseData;
using Cwc.Security;
using Cwc.Transport;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.Tests.Fixtures;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD.BaseDataCrudTests
{
    [Collection("MyCollection")]
    public class CITProcessSettingCrudTests:IClassFixture<CashPointFixture>, IDisposable
    {
        string code;
        int number;
        CashPointFixture _fixture;

        public CITProcessSettingCrudTests(CashPointFixture fixture)
        {
            code = $"1314{ new Random().Next(4000, 9999)}";
            number = Int32.Parse(code);
            _fixture = fixture;

        }

        public void Dispose()
        {
            using (var context = new AutomationTransportDataContext())
            {
                context.CitProcessSettingLinks.RemoveRange(context.CitProcessSettingLinks);
                context.CitProcessSettings.RemoveRange(context.CitProcessSettings.Where(cit => cit.ServicePointID == _fixture.Location.ID));
                context.SaveChanges();
            }
            using (var context = new AutomationCashPointDataContext())
            {
                context.CashPointTypes.RemoveRange(context.CashPointTypes.Where(cpt => cpt.Name.StartsWith(code)));
                context.SaveChanges();
            }
        }

        [Fact(DisplayName = "CITProcessSetings CRUD - CITProcessSetings was created successfully")]

        public void VerifyThatCITProcessSetingsWasCreatedSuccessfully()
        {

            var citProcessSettings = DataFacade.CitProcessSettings.New().
                With_LocationTypeID(_fixture.LocationType.ID).
                With_CustomerID(_fixture.Customer.ID).
                With_CashPointTypeID(_fixture.CashPointType.ID).
                With_ServicePointID(_fixture.Location.ID).
                With_IsNotifyUponReschedule(false).
                With_IsAdditionalDeliveryRequired(false).
                SaveToDb().
                Build();

            try
            {
                var citProcessSettingsCreated = DataFacade.CitProcessSettings.Take(cit => cit.CustomerID == citProcessSettings.CustomerID &&
                    cit.LocationTypeID == citProcessSettings.LocationTypeID &&
                    cit.CashPointTypeID == citProcessSettings.CashPointTypeID && 
                    cit.ServicePointID == citProcessSettings.ServicePointID && 
                    cit.VisitAddressID == citProcessSettings.VisitAddressID).Build();

                Assert.True(citProcessSettings.ID == citProcessSettingsCreated.ID, "CITProcessSetting Wasn't created.");
                Assert.True(citProcessSettings.LocationTypeID == citProcessSettingsCreated.LocationTypeID, "CITProcessSetting Wasn't created.");
                Assert.True(citProcessSettings.CashPointTypeID == citProcessSettingsCreated.CashPointTypeID, "CITProcessSetting Wasn't created.");
                Assert.True(citProcessSettings.ServicePointID == citProcessSettingsCreated.ServicePointID, "CITProcessSetting Wasn't created.");
                Assert.True(citProcessSettings.IsNotifyUponReschedule == citProcessSettingsCreated.IsNotifyUponReschedule, "CITProcessSetting Wasn't created.");
                Assert.True(citProcessSettings.IsAdditionalDeliveryRequired == citProcessSettingsCreated.IsAdditionalDeliveryRequired, "CITProcessSetting Wasn't created.");
            }

            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "CITProcessSetings CRUD - CITProcessSetings was updated successfully")]

        public void VerifyThatCITProcessSetingsWasUpdatedSuccessfully()
        {

            var citProcessSettings = DataFacade.CitProcessSettings.New().
                With_LocationTypeID(_fixture.LocationType.ID).
                With_CustomerID(_fixture.Customer.ID).
                With_CashPointTypeID(_fixture.CashPointType.ID).
                With_ServicePointID(_fixture.Location.ID).
                With_IsNotifyUponReschedule(false).
                With_IsAdditionalDeliveryRequired(false).
                SaveToDb().
                Build();

            try
            {
                var citProcessSettingsCreated =  DataFacade.CitProcessSettings.Take(cit => cit.CustomerID == citProcessSettings.CustomerID &&
                        cit.LocationTypeID == citProcessSettings.LocationTypeID &&
                        cit.CashPointTypeID == citProcessSettings.CashPointTypeID &&
                        cit.ServicePointID == citProcessSettings.ServicePointID &&
                        cit.VisitAddressID == citProcessSettings.VisitAddressID);

                citProcessSettingsCreated.With_IsAdditionalDeliveryRequired(true).
                    With_DefaultAdditionalDeliveryServiceType(58).
                    SaveToDb();

                var citProcessSettingsUpdated = DataFacade.CitProcessSettings.Take(cit => cit.ID == citProcessSettings.ID).Build();

                Assert.False(citProcessSettings.IsAdditionalDeliveryRequired == citProcessSettingsUpdated.IsAdditionalDeliveryRequired, "CITProcessSetings was updated");
                Assert.False(citProcessSettings.DefaultAdditionalDeliveryServiceTypeId == citProcessSettingsUpdated.DefaultAdditionalDeliveryServiceTypeId, "CITProcessSetings was updated");

            }

            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "CITProcessSetings CRUD - CITProcessSetings was deleted successfully")]

        public void VerifyThatCITProcessSetingsWasDeletedSuccessfully()
        {

            var citProcessSettings = DataFacade.CitProcessSettings.New().
                With_LocationTypeID(_fixture.LocationType.ID).
                With_CustomerID(_fixture.Customer.ID).
                With_CashPointTypeID(_fixture.CashPointType.ID).
                With_ServicePointID(_fixture.Location.ID).
                With_IsNotifyUponReschedule(false).
                With_IsAdditionalDeliveryRequired(false).
                SaveToDb().
                Build();

            try
            {
                DataFacade.CitProcessSettings.Delete(cit => cit.CustomerID == citProcessSettings.CustomerID &&
                        cit.LocationTypeID == citProcessSettings.LocationTypeID &&
                        cit.CashPointTypeID == citProcessSettings.CashPointTypeID &&
                        cit.ServicePointID == citProcessSettings.ServicePointID &&
                        cit.VisitAddressID == citProcessSettings.VisitAddressID);

                using (var context = new TransportDataContext())
                {
                    var result = context.CitProcessSettings.FirstOrDefault(cit => cit.ID == citProcessSettings.ID);

                    Assert.True(result == null, "CIT Process Settings wasn't deleted");
                }
            }

            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "CITProcessSetings CRUD - When CITProcessSetings was Saved with is default = 'true' and ServicePoint isn't empty Then system shows error message")]

        public void VerifyThatCITProcessSetingsCannotBeSavedWhenIsDefaultIsTrueAndServicePointIsNotEmpty()
        {

            var citProcessSettings = DataFacade.CitProcessSettings.New().
                With_ServicePointID(_fixture.Location.ID).
                With_IsDefault(true).
                With_IsNotifyUponReschedule(false).
                With_IsAdditionalDeliveryRequired(false);
            
            try
            {
                var result = TransportFacade.CitProcessSettingService.Save(citProcessSettings);

                Assert.True(!result.IsSuccess, "CIT Process Setting was created without");
                Assert.Equal("Default setting cannot be defined for specific level.", result.Messages.FirstOrDefault());

            }

            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "CITProcessSetings CRUD - When CITProcessSetings was created without company, location type, cash point type, visit address or service point Then system shows error message")]

        public void VerifyThatCITProcessSetingsCannotBeCreatedWithoutCompanyLocationCashPointTypeVisitAddressOrServicePoint()
        {

            var citProcessSettings = DataFacade.CitProcessSettings.New().
                With_IsNotifyUponReschedule(false).
                With_IsAdditionalDeliveryRequired(false);

            try
            {
                var result = TransportFacade.CitProcessSettingService.Save(citProcessSettings);

                Assert.True(!result.IsSuccess, "CIT Process Setting was created without company, location type, cash point type, visit address or service point.");
                Assert.Equal("Please define company, location type, cash point type, visit address or service point.", result.Messages.FirstOrDefault());
            }

            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "CITProcessSetings CRUD - When CITProcessSetings was created without Is Notify Upon Reschedule Then system shows error message")]

        public void VerifyThatCITProcessSetingsCannotBeCreatedWithoutIsNotifyUponReschedule()
        {

            var citProcessSettings = DataFacade.CitProcessSettings.New().
                With_LocationTypeID(_fixture.LocationType.ID).
                With_CustomerID(_fixture.Customer.ID).
                With_CashPointTypeID(_fixture.CashPointType.ID).
                With_ServicePointID(_fixture.Location.ID).
                With_IsAdditionalDeliveryRequired(false);

            try
            {
                var result = TransportFacade.CitProcessSettingService.Save(citProcessSettings);

                Assert.True(!result.IsSuccess, "CIT Process Setting was created without Is Notify Upon Reschedule");
                Assert.Equal("Value of property 'Cwc.Transport.Model.CitProcessSetting.IsNotifyUponReschedule' is not specified.", result.Messages.FirstOrDefault());
            }

            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "CITProcessSetings CRUD - When CITProcessSetings was created without Is Additional Delivery Required Then system shows error message")]

        public void VerifyThatCITProcessSetingsCannotBeCreatedWithoutIsAdditionalDeliveryRequired()
        {

            var citProcessSettings = DataFacade.CitProcessSettings.New().
                With_LocationTypeID(_fixture.LocationType.ID).
                With_CustomerID(_fixture.Customer.ID).
                With_CashPointTypeID(_fixture.CashPointType.ID).
                With_ServicePointID(_fixture.Location.ID).
                With_IsNotifyUponReschedule(false);

            try
            {
                var result = TransportFacade.CitProcessSettingService.Save(citProcessSettings);

                Assert.True(!result.IsSuccess, "CIT Process Setting was created without Is Additional Delivery Required");
                Assert.Equal("Value of property 'Cwc.Transport.Model.CitProcessSetting.IsAdditionalDeliveryRequired' is not specified.", result.Messages.FirstOrDefault());
            }

            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "CITProcessSetings CRUD - When CITProcessSetings was created with exists combination of Company, Location type, Visit Address, Cash point type, Service point Then system shows error message")]

        public void VerifyThatCITProcessSetingsDublicateCannotBeCreated()
        {

            var citProcessSettingsSecond= DataFacade.CitProcessSettings.New().
                With_LocationTypeID(_fixture.LocationType.ID).
                With_CustomerID(_fixture.Customer.ID).
                With_CashPointTypeID(_fixture.CashPointType.ID).
                With_ServicePointID(_fixture.Location.ID).
                With_IsNotifyUponReschedule(false).
                With_IsAdditionalDeliveryRequired(false);

            try
            {

                var citProcessSettingsFirst = DataFacade.CitProcessSettings.New().
                    With_LocationTypeID(_fixture.LocationType.ID).
                    With_CustomerID(_fixture.Customer.ID).
                    With_CashPointTypeID(_fixture.CashPointType.ID).
                    With_ServicePointID(_fixture.Location.ID).
                    With_IsNotifyUponReschedule(false).
                    With_IsAdditionalDeliveryRequired(false).
                    SaveToDb();

                var result = TransportFacade.CitProcessSettingService.Save(citProcessSettingsSecond);

                Assert.True(!result.IsSuccess, "CIT Process Setting was created");
                Assert.Equal("Setting with the same combination of Company, Location type, Visit Address, Cash point type, Service point already exists.", result.Messages.FirstOrDefault());
            }

            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "CITProcessSetings CRUD - When CITProcessSetings was created with existing Service Point Then system shows error message")]

        public void VerifyThatCITProcessSetingsCannotBeCreateWithExistingServicePoint()
        {

            var citProcessSettingsSecond = DataFacade.CitProcessSettings.New().
                With_ServicePointID(_fixture.Location.ID).
                With_IsNotifyUponReschedule(false).
                With_IsAdditionalDeliveryRequired(false);

            try
            {

                var citProcessSettingsFirst = DataFacade.CitProcessSettings.New().
                    With_ServicePointID(_fixture.Location.ID).
                    With_IsNotifyUponReschedule(false).
                    With_IsAdditionalDeliveryRequired(false).
                    SaveToDb();

                var userId = SecurityFacade.LoginService.GetAdministratorLogin();
                var userParams = new UserParams(userId);
                var result = TransportFacade.CitProcessSettingService.Save(citProcessSettingsSecond, userParams, null);

                Assert.True(!result.IsSuccess, "CIT Process Setting was created with the same Service Point");
                Assert.Equal("Setting with the same Service point already exists.", result.Messages.FirstOrDefault()); // Test is correct Message is not correct(CIT site 3.3.2 1d)
            }

            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "CITProcessSetings CRUD - When CITProcessSetings was created with Additional Delivery Required = True And Default Delyvery = Empty Then system shows error message")]

        public void VerifyThatCITProcessSetingsCannotBeCreateWithIsAdditionalDeliveryRequiredTrueAndDefaultDelyveryEmpty()
        {

            var citProcessSettings = DataFacade.CitProcessSettings.New().
                With_ServicePointID(_fixture.Location.ID).
                With_IsNotifyUponReschedule(false).
                With_IsAdditionalDeliveryRequired(true);

            try
            {
                var result = TransportFacade.CitProcessSettingService.Save(citProcessSettings);

                Assert.True(!result.IsSuccess, "CIT Process Setting was created.");
                Assert.Equal("Default delivery service type must be specified when additional delivery order is required.", result.Messages.FirstOrDefault()); 
            }

            catch
            {
                throw;
            }
        }
    }
}
