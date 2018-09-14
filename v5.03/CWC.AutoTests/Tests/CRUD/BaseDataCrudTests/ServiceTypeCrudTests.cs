using Cwc.BaseData;
using Cwc.Constants;
using Cwc.Contracts;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Data;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD.BaseDataCrudTests
{    
    public class ServiceTypeCrudTests : IDisposable
    {
        string defaultNumber, name;
        //int number;
        decimal denomination, weight;

        public ServiceTypeCrudTests()
        {
            defaultNumber = $"1101{new Random().Next(4000, 9999)}";
            //number = Int32.Parse(defaultNumber);
            denomination = new Random().Next(1, 1000);
            weight = new Random().Next(1, 1000);
            name = "AutoTestManagement";
        }

        public void Dispose()
        {
            using (var context = new AutomationBaseDataContext())
            {
                context.ServiceTypes.RemoveRange(context.ServiceTypes.Where(st => st.Code.StartsWith(defaultNumber)));
                context.SaveChanges();
            }
        }

        [Theory(DisplayName = "ServiceType CRUD - ServiceType was created successfully")]
        #region InlineData
        [InlineData(ServiceTypeConstants.Deliver)]
        [InlineData(ServiceTypeConstants.Collect)]
        [InlineData(ServiceTypeConstants.Counting)]
        [InlineData(ServiceTypeConstants.Dispatchig)]
        [InlineData(ServiceTypeConstants.DispatchingToCentralBank)]
        [InlineData(ServiceTypeConstants.Interbank)]
        [InlineData(ServiceTypeConstants.Internal)]
        [InlineData(ServiceTypeConstants.Inventory)]
        [InlineData(ServiceTypeConstants.PickAndPack)]
        [InlineData(ServiceTypeConstants.ReceiveFromBank)]
        [InlineData(ServiceTypeConstants.ReceiveFromCentralBank)]
        [InlineData(ServiceTypeConstants.ReceiveFromSite)]
        [InlineData(ServiceTypeConstants.ReceiveFromThirdparty)]
        [InlineData(ServiceTypeConstants.Replenishment)]
        [InlineData(ServiceTypeConstants.Servicing)]
        #endregion
        public void VerifyThatServiceTypeWasCreatedSuccessfully(string Type)
        {
            var serviceType = DataFacade.ServiceType.New().
                With_Code(defaultNumber).
                With_Name(name).
                With_OldType(Type).
                SaveToDb(null).
                Build();

            var serviceTypeCreated = DataFacade.ServiceType.Take(st => st.Code == defaultNumber).Build();

            Assert.True(serviceType.Name == serviceTypeCreated.Name,"Service Type wasn't created. Problem is with Name");
            Assert.True(serviceType.OldType == serviceTypeCreated.OldType, "Service Type wasn't created. Problem is with Old Type");

        }

        [Theory(DisplayName = "ServiceType CRUD - ServiceType was deleted successfully")]
        #region InlineData
        [InlineData(ServiceTypeConstants.Deliver)]
        [InlineData(ServiceTypeConstants.Collect)]
        [InlineData(ServiceTypeConstants.Counting)]
        [InlineData(ServiceTypeConstants.Dispatchig)]
        [InlineData(ServiceTypeConstants.DispatchingToCentralBank)]
        [InlineData(ServiceTypeConstants.Interbank)]
        [InlineData(ServiceTypeConstants.Internal)]
        [InlineData(ServiceTypeConstants.Inventory)]
        [InlineData(ServiceTypeConstants.PickAndPack)]
        [InlineData(ServiceTypeConstants.ReceiveFromBank)]
        [InlineData(ServiceTypeConstants.ReceiveFromCentralBank)]
        [InlineData(ServiceTypeConstants.ReceiveFromSite)]
        [InlineData(ServiceTypeConstants.ReceiveFromThirdparty)]
        [InlineData(ServiceTypeConstants.Replenishment)]
        [InlineData(ServiceTypeConstants.Servicing)]
        #endregion
        public void VerifyThatServiceTypeWasDeletedSuccessfully(string Type)
        {
            var serviceType = DataFacade.ServiceType.New().
                With_Code(defaultNumber).
                With_Name(name).
                With_OldType(Type).
                SaveToDb(null);

            DataFacade.ServiceType.DeleteMany(st => st.Code == defaultNumber);

            using (var context = new AutomationBaseDataContext())
            {
                var result = context.ServiceTypes.FirstOrDefault(st => st.Code == defaultNumber);
                Assert.True(result == null, "Service Type wasn't deleted");
            }
        }

        [Theory(DisplayName = "ServiceType CRUD - ServiceType was updated successfully")]
        #region InlineData
        [InlineData(ServiceTypeConstants.Deliver)]
        [InlineData(ServiceTypeConstants.Collect)]
        [InlineData(ServiceTypeConstants.Counting)]
        [InlineData(ServiceTypeConstants.Dispatchig)]
        [InlineData(ServiceTypeConstants.DispatchingToCentralBank)]
        [InlineData(ServiceTypeConstants.Interbank)]
        [InlineData(ServiceTypeConstants.Internal)]
        [InlineData(ServiceTypeConstants.Inventory)]
        [InlineData(ServiceTypeConstants.PickAndPack)]
        [InlineData(ServiceTypeConstants.ReceiveFromBank)]
        [InlineData(ServiceTypeConstants.ReceiveFromCentralBank)]
        [InlineData(ServiceTypeConstants.ReceiveFromSite)]
        [InlineData(ServiceTypeConstants.ReceiveFromThirdparty)]
        [InlineData(ServiceTypeConstants.Replenishment)]
        [InlineData(ServiceTypeConstants.Servicing)]
        #endregion
        public void VerifyThatServiceTypeWasUpdatedSuccessfully(string Type)
        {
            var serviceType = DataFacade.ServiceType.New().
                With_Code(defaultNumber).
                With_Name(name).
                With_OldType(Type).
                SaveToDb(null).
                Build();

            var serviceTypeCreated = DataFacade.ServiceType.Take(st => st.Code == defaultNumber);
            if (Type == "Deliver")
            {
                serviceTypeCreated.With_Name(name + "1").With_OldType("Collect").SaveToDb(null);
            }
            else
            {
                serviceTypeCreated.With_Name(name + "1").With_OldType("Deliver").SaveToDb(null);
            }

            var serviceTypeUpdated = DataFacade.ServiceType.Take(st => st.Code == defaultNumber).Build();

            Assert.False(serviceType.Name == serviceTypeUpdated.Name,"Service Type wasn't updated. Problem is with Name");
            Assert.False(serviceType.OldType == serviceTypeUpdated.OldType, "Service Type wasn't updated. Problem is with Old Type");
        }

        [Fact(DisplayName = "ServiceType CRUD - When Code is Empty Then system shows error message")]

        public void VerifyThatServiceTypeCannotBeCreatedWithoutCode()
        {
            var serviceType = DataFacade.ServiceType.New().
                With_Code(string.Empty).
                With_Name(name).
                With_OldType("Deliver");

            var result = BaseDataFacade.ServiceTypeService.Save(serviceType, null);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Value of property 'Code' is not specified.", result.Messages.First());
        }

        [Fact(DisplayName = "ServiceType CRUD - When Name is Empty Then system shows error message")]

        public void VerifyThatServiceTypeCannotBeCreatedWithoutName()
        {
            var serviceType = DataFacade.ServiceType.New().
                With_Code(defaultNumber).
                With_Name(string.Empty).
                With_OldType("Deliver");

            var result = BaseDataFacade.ServiceTypeService.Save(serviceType, null);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Value of property 'Name' is not specified.", result.Messages.First());
        }

        [Fact(DisplayName = "ServiceType CRUD - When Type is Empty Then system shows error message")]

        public void VerifyThatServiceTypeCannotBeCreatedWithoutType()
        {
            var serviceType = DataFacade.ServiceType.New().
                With_Code(defaultNumber).
                With_Name(name).
                With_OldType(string.Empty);

            var result = BaseDataFacade.ServiceTypeService.Save(serviceType, null);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Value of property 'Cwc.BaseData.Classes.ServiceType.OldType' is not specified.", result.Messages.First());
        }

        [Fact(DisplayName = "ServiceType CRUD - When Code is already exists Then system shows error message")]
        public void VerifyThatServiceTypeCannotBeCreatedWithTheSameCode()
        {
            var serviceType = DataFacade.ServiceType.New().
                With_Code(defaultNumber).
                With_Name(name).
                With_OldType("Deliver").SaveToDb(null);

            var serviceTypeSecond = DataFacade.ServiceType.New().
                With_Code(defaultNumber).
                With_Name(name).
                With_OldType("Deliver");

            var result = BaseDataFacade.ServiceTypeService.Save(serviceTypeSecond, null);

            Assert.False(result.IsSuccess, "Result should be unsuccessful");
            Assert.Equal("Service Type with specified code already exists.", result.Messages.First());
        }

        [Theory(DisplayName = "ServiceType CRUD - When ServiceType was deleted with reference Then system shows error message")]
        #region InlineData
        [InlineData(ServiceTypeConstants.Deliver)]
        [InlineData(ServiceTypeConstants.Collect)]
        [InlineData(ServiceTypeConstants.Counting)]
        [InlineData(ServiceTypeConstants.Dispatchig)]
        [InlineData(ServiceTypeConstants.DispatchingToCentralBank)]
        [InlineData(ServiceTypeConstants.Interbank)]
        [InlineData(ServiceTypeConstants.Internal)]
        [InlineData(ServiceTypeConstants.Inventory)]
        [InlineData(ServiceTypeConstants.PickAndPack)]
        [InlineData(ServiceTypeConstants.ReceiveFromBank)]
        [InlineData(ServiceTypeConstants.ReceiveFromCentralBank)]
        [InlineData(ServiceTypeConstants.ReceiveFromSite)]
        [InlineData(ServiceTypeConstants.ReceiveFromThirdparty)]
        [InlineData(ServiceTypeConstants.Replenishment)]
        [InlineData(ServiceTypeConstants.Servicing)]
        #endregion

        public void VerifyThatServiceTypeWithReferenceCannotBeDeleted(string type)
        {
            var serviceType = DataFacade.ServiceType.New().
                With_Code(defaultNumber).
                With_Name(name).
                With_OldType(type).
                SaveToDb(null).
                Build();

            var customer = DataFacade.Customer.InitDefault().
                With_ReferenceNumber(defaultNumber).
                SaveToDb().
                Build();

            var companyContract = DataFacade.Contract.New().
                With_IsDefault(false).
                With_Number(defaultNumber).                
                With_Currency_code("EUR").
                With_Customer_id(customer.ID).
                With_Date(DateTime.Now).
                With_EffectiveDate(DateTime.Now).
                With_StartDate(DateTime.Now).
                With_EndDate(DateTime.Now.AddDays(30)).
                With_InterestRate(0).
                With_CustomerType(CustomerType.Direct).
                With_IsLatestRevision(true).
                SaveToDb().
                Build();

            try
            {
                DataFacade.ContractOrderingSetting.New().
                    With_Contract_id(companyContract.ID).
                    With_ServiceType_id(serviceType.ID).
                    With_LocationType_code(null).
                    With_Location_id(null).
                    With_AllowTotalPreannouncement(true).
                    With_IsCoins(true).
                    With_IsConsumables(true).
                    With_IsLatestRevision(true).
                    With_IsNotes(true).
                    With_IsPreAnnouncement(true).
                    With_IsServicingCodes(true).
                    With_CCLeadTime(0).
                    With_CITLeadTime(0).
                    With_CutOffTime(0.145).
                    With_LeadTime(0).
                    With_IsCoinsLooseProductDelivery(false).
                    With_IsNotesLooseProductDelivery(false).
                    SaveToDb(true);

                DataFacade.ScheduleSetting.New().
                   With_Contract_id(companyContract.ID).
                   With_ServiceTypeID(serviceType.ID).
                   With_PeriodStartDate(DateTime.Now.Date).
                   With_IsLatestRevision(true).
                   SaveToDb();

                using (var context = new AutomationBaseDataContext())
                {
                    var serviceTypeCreated = context.ServiceTypes.Where(st => st.Code == defaultNumber).Select(x => x.ID).ToArray();

                    var result = BaseDataFacade.ServiceTypeService.Delete(serviceTypeCreated, null);
                    Assert.False(result.IsSuccess, "Result should be unsuccessful");
                    Assert.Equal("One or more objects 'Service Type' cannot be deleted. There are other entities linked to it that may loose integrity.", result.Messages.First());
                }
            }

            catch
            {
                throw;
            }

            finally
            {
                using (var context = new AutomationContractDataContext())
                {
                    context.ContractOrderingSetting.RemoveRange(context.ContractOrderingSetting.Where(cos => cos.Contract_id == companyContract.ID && cos.ServiceType_id == serviceType.ID));
                    context.SaveChanges();
                    context.Customers.RemoveRange(context.Customers.Where(c => c.ID == customer.ID));
                    context.SaveChanges();
                    context.ScheduleSettings.RemoveRange(context.ScheduleSettings.Where(ss => ss.Contract_id == companyContract.ID && ss.ServiceTypeID == serviceType.ID));
                    context.SaveChanges();
                    for (var i = 0 ; i <= 2 ; i++)
                    {
                        context.Contracts.RemoveRange(context.Contracts.Where(contruct => contruct.ID == (companyContract.ID + i)));                        
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
