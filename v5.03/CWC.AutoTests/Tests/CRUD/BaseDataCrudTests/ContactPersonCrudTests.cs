using Cwc.BaseData;
using Cwc.Security;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Data;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD.BaseDataCrudTests
{
    [Collection("MyCollection")]
    public class ContactPersonCrudTests : IDisposable
    {

        string name, code, email;

        public ContactPersonCrudTests()
        {
            name = "AutoTestManagement";
            email = "automation@edsson.com";
            code = $"1101{new Random().Next(4000, 9999)}";
        }

        public void Dispose()
        {
            using (var context = new AutomationBaseDataContext())
            {
                context.ContactPersons.RemoveRange(context.ContactPersons.Where(cp => cp.FirstName.StartsWith(code) && cp.LastName.StartsWith(name) && cp.Email.StartsWith(email)));
                context.SaveChanges();
            }
        }

        [Fact(DisplayName = "ContactPerson CRUD - ContactPerson was created successfully")]
        public void VerifyTHatContactPersonWasCreatedSuccessfully()
        {
            var contactPerson = DataFacade.ContactPerson.New().
                With_FirstName(code).
                With_LastName(name).
                With_IsPreferred(false).
                With_Email(email).
                SaveToDb().
                Build();

            var contactPersonCreated = DataFacade.ContactPerson.Take(cp => cp.FirstName == code && cp.LastName == name && cp.Email == email).Build();

            Assert.True(contactPerson.FirstName == contactPersonCreated.FirstName, "ContractPerson wasn't created");
            Assert.True(contactPerson.LastName == contactPersonCreated.LastName, "ContractPerson wasn't created");
            Assert.True(contactPerson.IsPreferred == contactPersonCreated.IsPreferred, "ContractPerson wasn't created");
            Assert.True(contactPerson.Email == contactPersonCreated.Email, "ContractPerson wasn't created");
        }

        [Fact(DisplayName = "ContactPerson CRUD - ContactPerson was updated successfully")]
        public void VerifyTHatContactPersonWasUpdatedSuccessfully()
        {
            var contactPerson = DataFacade.ContactPerson.New().
                With_FirstName(code).
                With_LastName(name).
                With_IsPreferred(false).
                With_Email(email).
                SaveToDb().
                Build();

            var contactPersonCreated = DataFacade.ContactPerson.Take(cp => cp.FirstName == code && cp.LastName == name && cp.Email == email);
            contactPersonCreated.With_FirstName(code + "1").
                With_LastName(name + "1").SaveToDb();

            var contactPersonUpdated = DataFacade.ContactPerson.Take(cp => cp.FirstName == (code + "1") && cp.LastName == (name + "1")).Build();

            Assert.False(contactPerson.FirstName == contactPersonUpdated.FirstName, "FirstName wasn't updated");
            Assert.False(contactPerson.LastName == contactPersonUpdated.LastName, "LastName wasn't updated");
        }

        [Fact(DisplayName = "ContactPerson CRUD - ContactPerson was deleted successfully")]
        public void VerifyTHatContactPersonWasDeletedSuccessfully()
        {
            var contactPerson = DataFacade.ContactPerson.New().
               With_FirstName(code).
               With_LastName(name).
               With_Email(email).
               With_IsPreferred(false).
               SaveToDb();

            DataFacade.ContactPerson.Delete(cp => cp.FirstName == code && cp.LastName == name && cp.Email == email);

            using (var context = new BaseDataContext())
            {
                var result = context.ContactPersons.FirstOrDefault(cp => cp.FirstName == code && cp.LastName == name && cp.Email == email);
                Assert.True(result == null, "Contact Person wasn't deleted");
            }
        }

        [Fact(DisplayName = "ContactPerson CRUD - When ContactPerson was created with incorrect email Then system shows error message")]
        public void VerifyTHatContactPersonCannotBeCreatedWithIncorrectEmail()
        {
            email = "123e";
            var contactPerson = DataFacade.ContactPerson.New().
                With_FirstName(code).
                With_LastName(name).
                With_IsPreferred(false).
                With_Email(email);

            var userID = SecurityFacade.LoginService.GetAdministratorLogin().UserID;
            var result = BaseDataFacade.ContactPersonService.UserSave(contactPerson.Build(), userID, null);

            Assert.False(result.IsSuccess, "ContactPerson Saving should be unsuccessful");
            Assert.Equal("Please provide a valid e-mail address.", result.Messages.First());
        }
    }
}
