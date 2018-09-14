using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.Security;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD.BaseDataCrudTests
{
    [Collection("MyCollection")]

    public class CustomerCrudTests : IDisposable
    {
        LoginResult login;
        AutomationTransportDataContext _context;
        string defaultNumber;
        public CustomerCrudTests()
        {
            ConfigurationKeySet.Load();
            login = SecurityFacade.LoginService.GetAdministratorLogin();
            _context = new AutomationTransportDataContext();
            defaultNumber = $"1101{new Random().Next(4000, 9999)}";
        }

        [Theory(DisplayName = "Customer CRUD - When Customer is saved without Code Then System shows error message")]
        [InlineData(CustomerRecordType.Region)]
        [InlineData(CustomerRecordType.Company)]
        [InlineData(CustomerRecordType.Debtor)]
        [InlineData(CustomerRecordType.Bank)]
        public void VerifyThatCustomerWithEmptyCodeCannotBeCreated(CustomerRecordType type)
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(string.Empty).
                With_Name("name").
                With_Abbrev("abbrev").

                With_RecordType(type);

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, customer);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Please, specify Number", result.Messages.First());
        }

        [Theory(DisplayName = "Customer CRUD - When Customer is saved without Name Then System shows error message")]
        [InlineData(CustomerRecordType.Region)]
        [InlineData(CustomerRecordType.Company)]
        [InlineData(CustomerRecordType.Debtor)]
        [InlineData(CustomerRecordType.Bank)]
        public void VerifyThatCustomerWithEmptyNameCannotBeCreated(CustomerRecordType type)
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name(string.Empty).
                With_Abbrev("abbrev").
                With_RecordType(type);

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, customer);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Please, specify Name", result.Messages.First());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == defaultNumber && c.RecordType == type).Any());
        }

        [Theory(DisplayName = "Customer CRUD - When Customer is saved without Code and Name Then System shows error message")]
        [InlineData(CustomerRecordType.Region)]
        [InlineData(CustomerRecordType.Company)]
        [InlineData(CustomerRecordType.Debtor)]
        [InlineData(CustomerRecordType.Bank)]
        public void VerifyThatCustomerWithEmptyCodeAndNameCannotBeCreated(CustomerRecordType type)
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(string.Empty).
                With_Name(string.Empty).
                With_Abbrev("abbrev").
                With_RecordType(type);

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, customer);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Please, specify Number, Name", result.Messages.First());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == defaultNumber && c.RecordType == type).Any(), "Customer shouldnt be saved after error");
        }

        [Theory(DisplayName = "Customer CRUD - When Customer Number is not nuumeric Then System doesn't allow to save it")]
        [InlineData(CustomerRecordType.Region, "Region number must be numeric")]
        [InlineData(CustomerRecordType.Company, "Company number must be numeric")]
        [InlineData(CustomerRecordType.Debtor, "Debtor number must be numeric")]
        [InlineData(CustomerRecordType.Bank, "Bank number must be numeric" )]
        public void VerifyThatNumberShouldBeNumeric(CustomerRecordType type, string message)
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber("ABCDE").
                With_Name("ABCDE").
                With_Abbrev("abbrev").
                With_IBANBankIdentifier("AAAA").
                With_RecordType(type);

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, customer);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal(message, result.Messages.First());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == defaultNumber && c.RecordType == type).Any(), "Customer shouldnt be saved after error");
        }

        [Theory(DisplayName = "Customer CRUD - When Customer Number is not unique Then System doesn't allow to save it")]
        [InlineData(CustomerRecordType.Region, "Region with given Code already exists.")]
        [InlineData(CustomerRecordType.Company, "Company with given Number already exists")]
        [InlineData(CustomerRecordType.Debtor, "Debtor with given Number already exists")]
        [InlineData(CustomerRecordType.Bank, "Bank with given Number already exists")]
        public void VerifyThatNumberShouldBeUnique(CustomerRecordType type, string message)
        {
            var firstCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("ABCDE").With_Abbrev("abbrev").
                With_IBANBankIdentifier("AAAA").
                With_RecordType(type).
                SaveToDb();

            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("ABCDE").
                With_Abbrev("abbrev").
                With_RecordType(type);

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, customer);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal(message, result.Messages.First());
            Assert.Equal(1, _context.Customers.Where(c => c.ReferenceNumber == defaultNumber && c.RecordType == type).Count());
        }

        [Fact(DisplayName = "Customer CRUD - When Customer is cause reference violation Then System doesn't allow save it", Skip = "CWC8680 should be fixed")]
        public void VerifyThatSystemDoesntAllowReferenceViolation()
        {
            var firstCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("ABCDE").
                With_Abbrev("abbrev").
                SaveToDb();

            var secondCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber + "1").
                With_Name("ABCDE").
                With_Abbrev("abbrev").
                With_ParentCustomerID(firstCustomer.Build().ID).
                SaveToDb();

            firstCustomer.With_ParentCustomerID(secondCustomer.Build().ID);

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, firstCustomer);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal($"Parent company ‘{secondCustomer.Build().DisplayCaption}’ cannot be used, since it will cause circular references between companies.", result.Messages.First());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == defaultNumber + "1").Any(), "Customer shouldnt be saved after error");
        }

        [Fact(DisplayName = "Customer CRUD - When IBAN is not correspond to format [A-Z0-9]{4} Then System doesn't allow to save bank")]
        public void VerifyThatSystemDoesntAllowToSaveBankWithIncorrectIBAN()
        {
            var bank = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_RecordType(CustomerRecordType.Bank).
                With_IBANBankIdentifier(defaultNumber).
                With_Name("Name");

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, bank);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("IBAN bank identifier should be composed of 4 capital letters or digits.", result.Messages.First());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == defaultNumber).Any());
        }

        [Fact(DisplayName = "Customer CRUD - When Debtor is saved with non unique ledger code Then System shows error message")]
        public void VerifyThatSystemDoesntAllowToSaveNonnUniqueLedgerCode()
        {
            var firstDebtor = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("Name").
                With_RecordType(CustomerRecordType.Debtor).
                With_LedgerCode(defaultNumber).
                SaveToDb();

            var debtor = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber + "1").
                With_Name("Name").
                With_RecordType(CustomerRecordType.Debtor).
                With_LedgerCode(defaultNumber);

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, debtor);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Debtor with given Ledger Code already exists.", result.Messages.First());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == defaultNumber + "1").Any(), "Customer shouldnt be saved after error");
        }

        [Fact(DisplayName = "Customer CRUD - When Debtor is saved with non unique VAT code Then System shows error message")]
        public void VerifyThatSystemDoesntAllowToSaveNonnUniqueVatCode()
        {
            var firstDebtor = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("Name").
                With_RecordType(CustomerRecordType.Debtor).
                With_VATCode(defaultNumber).
                SaveToDb();

            var debtor = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber + "1").
                With_Name("Name").
                With_RecordType(CustomerRecordType.Debtor).
                With_VATCode(defaultNumber);

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, debtor);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Debtor with given VAT Code already exists.", result.Messages.First());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == defaultNumber + "1").Any(), "Customer shouldnt be saved after error");
        }

        [Fact(DisplayName = "Customer CRUD - When Customer is saved with non unique affiliated bank account Then error message is shown")]
        public void VerifyThatSystemDoesntAllowToSaveNonUniqueAffiliatedBank()
        {
            var bank = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("Name").
                With_RecordType(CustomerRecordType.Bank).
                With_IBANBankIdentifier("AAAA").
                SaveToDb();

            var firstCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("Name").
                With_AffiliatedBank(bank).
                SaveToDb();

            var nonUniqueCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber + "1").
                With_Name("Name").
                With_AffiliatedBank(bank);

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, nonUniqueCustomer);

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal("Company with given Affiliated Bank already exists.", result.Messages.First());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == defaultNumber + "1").Any(), "Customer shouldnt be saved after error");
        }

        [Theory(DisplayName = "Customer CRUD - When mandatory attributes are filled Then System creates Customer")]
        [InlineData(CustomerRecordType.Region)]
        [InlineData(CustomerRecordType.Company)]
        [InlineData(CustomerRecordType.Debtor)]
        [InlineData(CustomerRecordType.Bank)]
        public void VerifyThatCustomerCreatedProperly(CustomerRecordType type)
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("Name").
                With_RecordType(type).
                With_IBANBankIdentifier("AAAA").
                SaveToDb().
                Build();

            var customerCreated = DataFacade.Customer.Take(c => c.ReferenceNumber == defaultNumber).Build();
            Assert.True(customer.ReferenceNumber == customerCreated.ReferenceNumber,"Customer wasn't created");
        }

        [Fact(DisplayName = "Customer CRUD - When all new fields are valid Then System updates customer")]
        public void VerifyThatCustomerCouldBeEdited()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("Name").
                With_Abbrev("abbrev").
                SaveToDb();

            customer.With_Name("Updated").With_Abbrev("Updated");

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, customer);

            Assert.True(result.IsSuccess, "Result should be success");
            Assert.Equal(1, _context.Customers.Where(c => c.ReferenceNumber == defaultNumber && c.Name == "Updated" && c.Abbrev == "Updated").Count());
        }

        [Fact(DisplayName = "Customer CRUD - When Name is set to empty on editing Then System doesn't allow to edit this customer")]
        public void VerifyThatSystemDoesntAllowToEditCustomerWithAbsentName()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("Name").
                With_Abbrev("abbrev").
                SaveToDb();

            customer.With_Name(string.Empty).With_Abbrev("Updated");

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, customer);

            Assert.False(result.IsSuccess, "Result should not be success");
            Assert.Equal("Please, specify Name", result.Messages.First());
        }

        [Fact(DisplayName = "Customer CRUD - When address configured properly Then System creates it")]
        public void VerifyThatSystemCreatesCustomerAddress()
        {
            var postalAddress = DataFacade.Address.New().
                With_Country(defaultNumber).
                With_City(defaultNumber).
                With_Street(defaultNumber).
                With_Purpose(Purpose.Postal);

            var visitAddress = DataFacade.Address.New().
                With_Country(defaultNumber).
                With_City(defaultNumber).
                With_Street(defaultNumber).
                With_Purpose(Purpose.Visit);

            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("Name").
                With_Abbrev("abbrev").
                With_PostalAddress(postalAddress).
                With_VisitAddress(visitAddress);

            var result = BaseDataFacade.CustomerService.SaveCustomer(login, customer);

            Assert.True(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal(1, _context.BaseAddresses.ToArray().Where(a => a.City == defaultNumber && a.Country == defaultNumber && a.Street == defaultNumber && a.ObjectID == customer.Build().ID && a.Purpose == Purpose.Postal).Count());
            Assert.Equal(1, _context.BaseAddresses.ToArray().Where(a => a.City == defaultNumber && a.Country == defaultNumber && a.Street == defaultNumber && a.ObjectID == customer.Build().ID && a.Purpose == Purpose.Visit).Count());
        }

        [Theory(DisplayName = "Customer CRUD - When customer is not linked to any entity it can be deleted")]
        [InlineData(CustomerRecordType.Region)]
        [InlineData(CustomerRecordType.Company)]
        [InlineData(CustomerRecordType.Debtor)]
        public void VerifyThatCustomerCanBeDeleted(CustomerRecordType type)
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("Name").
                With_Abbrev("abbrev").
                With_RecordType(type).
                With_Enabled(false).
                SaveToDb();

            var result = BaseDataFacade.CustomerService.DeleteCustomerById(login, new[] { customer.Build().ID });

            Assert.True(result.IsSuccess);
            Assert.Equal(0, _context.Customers.Where(c => c.ReferenceNumber == defaultNumber).Count());
        }

        //[Fact(DisplayName = "Customer CRUD - When customer is not linked to any entity it can be deleted")] // Customer linked to BankIdentifier???
        //public void VerifyThatCustomerCanBeDeleted()
        //{
        //    var customer = DataFacade.Customer.New().
        //        With_ReferenceNumber(defaultNumber).
        //        With_Name("Name").
        //        With_Abbrev("abbrev").
        //        With_IBANBankIdentifier("AAAA").
        //        With_RecordType(CustomerRecordType.Bank).
        //        With_Enabled(false).
        //        SaveToDb();

        //    var result = BaseDataFacade.CustomerService.DeleteCustomerById(login, new[] { customer.Build().ID });

        //    Assert.True(result.IsSuccess);
        //    Assert.Equal(0, _context.Customers.Where(c => c.ReferenceNumber == defaultNumber).Count());
        //}

        [Fact(DisplayName = "Customer CRUD - When multiple customers are selected for deletion and not linked to any entities Then System deletes them")]
        public void VerifyThatMultipleCustomersCanBeDeleted()
        {
            var customerFirst = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("Name").
                With_Abbrev("abbrev").
                With_Enabled(false).
                SaveToDb();

            var customerSecond = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber + "1").
                With_Name("Name").
                With_Abbrev("abbrev").
                With_Enabled(false).
                SaveToDb();

            var result = BaseDataFacade.CustomerService.DeleteCustomerById(login, new[] { customerFirst.Build().ID, customerSecond.Build().ID });

            Assert.True(result.IsSuccess);
            Assert.Equal(0, _context.Customers.Where(c => c.ReferenceNumber == defaultNumber).Count());
            Assert.Equal(0, _context.Customers.Where(c => c.ReferenceNumber == defaultNumber + "1").Count());
        }

        [Fact(DisplayName = "Customer CRUD - When customer is enabled Then it cannot be deleted")]
        public void VerifyThatEnabledCustomerCannotBeDeleted()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("Name").
                With_Abbrev("abbrev").
                With_Enabled(true).
                SaveToDb();

            var result = BaseDataFacade.CustomerService.DeleteCustomerById(login, new[] { customer.Build().ID });

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal(result.Messages.First(), "Enabled company cannot be deleted.");
        }

        [Fact(DisplayName = "Customer CRUD - When customer is linked to location it cannot be deleted")]
        public void VerifyThatWhenCustomerIsLinkedToLocationItCannotBeDeleted()
        {
            var location = DataFacade.Location.New().
                InitDefault().
                SaveToDb().
                Build();

            try
            {
                var result = BaseDataFacade.CustomerService.Delete(location.CompanyID);

                Assert.False(result.IsSuccess, "Result should be unsuccessfull");
                Assert.Equal($"Customer '{location.CompanyID}' cannot be deleted. There are other entities linked to it that may lose integrity.", result.Messages.First());
            }
            catch
            {
                throw;
            }
            finally
            {
                _context.CitProcessSettingLinks.RemoveRange(_context.CitProcessSettingLinks);
                _context.Locations.RemoveRange(_context.Locations.Where(l => l.ID == location.CompanyID));
                _context.Customers.RemoveRange(_context.Customers.Where(c => c.ID == location.CompanyID));
                _context.SaveChanges();
            }
        }

        [Fact(DisplayName = "Customer CRUD - When customer is linked parent customer Then System doesn't allow to delete it ")]
        public void VerifyThatLinkedParentCustomerCannotBeDeleted()
        {
            var customerParent = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("Name").
                With_Abbrev("abbrev").
                With_Enabled(false).
                SaveToDb().
                Build();

            var customerChild = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber + "1").
                With_Name("Name").
                With_Abbrev("abbrev").
                With_ParentCustomer(customerParent).
                With_Enabled(false).
                SaveToDb();

            var result = BaseDataFacade.CustomerService.Delete( customerParent.ID );

            Assert.False(result.IsSuccess, "Result should be unsuccessfull");
            Assert.Equal($"Customer '{customerParent.ID}' cannot be deleted. There are other entities linked to it that may lose integrity.", result.Messages.First());
        }


        [Fact(DisplayName = "test")]
        public void Tets()
        {
            using (var customer = DataFacade.Customer.New().With_ReferenceNumber(defaultNumber).With_Name("Name").With_Csgrp_cd("1001").With_Abbrev("abbrev").With_Enabled(false).SaveToDb())
            {
                Assert.NotNull(_context.Customers.ToArray().FirstOrDefault(c => c.ReferenceNumber == customer.Build().ReferenceNumber));
            }

        }

        public void Dispose()
        {
            _context.BaseAddresses.RemoveRange(_context.BaseAddresses);
            _context.Customers.RemoveRange(_context.Customers.Where(c => c.ReferenceNumber.StartsWith(defaultNumber)));
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
