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
    public class BankAccountCrudTests: IDisposable
    {
        string defaultNumber, name;

        public BankAccountCrudTests()
        {
            defaultNumber = $"1101{new Random().Next(4000, 9999)}";
            name = "AutoTestManagement";            
        }

        public void Dispose()
        {
            using (var context = new AutomationBaseDataContext())
            {
                context.Customers.RemoveRange(context.Customers.Where(c => c.ReferenceNumber == defaultNumber));
                context.BankAccountAuditEvents.RemoveRange(context.BankAccountAuditEvents.Where(baae => baae.BankAccountID == (context.BankAccounts.Where(ba => ba.Number.StartsWith(defaultNumber)).Select(x=>x.ID).FirstOrDefault())));
                context.BankAccounts.RemoveRange(context.BankAccounts.Where(ba => ba.Number.StartsWith(defaultNumber)));
                context.SaveChanges();
            }
        }

        [Fact(DisplayName ="BankAccount CRUD - BankAccount was created successfully")]

        public void VerifyThatBankAccountWasCreatedSuccessfully()
        {
            var bankAccount = DataFacade.BankAccount.New(defaultNumber).
                With_HolderName(name).
                SaveToDb().
                Build();

            var bankAccountCreated = DataFacade.BankAccount.Take(ba => ba.Number == defaultNumber).Build();

            Assert.True(bankAccount.HolderName == bankAccountCreated.HolderName,"BankAccount wasn't created. Problem is with HolderName");
        }

        [Fact(DisplayName = "BankAccount CRUD - BankAccount was updated successfully")]

        public void VerifyThatBankAccountWasUpdatedSuccessfully()
        {
            var bankAccount = DataFacade.BankAccount.New(defaultNumber).
                With_HolderName(name).
                SaveToDb().
                Build();

            var bankAccountCreated = DataFacade.BankAccount.Take(ba => ba.Number == defaultNumber);
            bankAccountCreated.With_HolderName(name + "1").SaveToDb();

            var bankAccountUpdated = DataFacade.BankAccount.Take(ba => ba.Number == defaultNumber).Build();

            Assert.False(bankAccount.HolderName == bankAccountUpdated.HolderName, "BankAccount wasn't updated. Problem is with HolderName");
        }

        [Fact(DisplayName = "BankAccount CRUD - When BankAccount was created without number Then system shows error message")]

        public void VerifyThatBankAccountCannotBeCreatedWithoutNumber()
        {
            var bankAccount = DataFacade.BankAccount.New(string.Empty).
                With_HolderName(name);
            
            var result = BaseDataFacade.BankAccountService.Save(bankAccount);

            Assert.False(result.IsSuccess, "BankAccount was created without Number");
            Assert.Equal("Please, specify Number", result.Messages.FirstOrDefault());
        }

        [Fact(DisplayName = "BankAccount CRUD - When BankAccount was created without HolderName Then system shows error message")]

        public void VerifyThatBankAccountCannotBeCreatedWithoutHolderName()
        {
            var bankAccount = DataFacade.BankAccount.New(defaultNumber).
                With_HolderName(string.Empty);

            var result = BaseDataFacade.BankAccountService.Save(bankAccount);

            Assert.False(result.IsSuccess, "BankAccount was created without HolderName");
            Assert.Equal("Please, specify Holder Name", result.Messages.FirstOrDefault());
        }

        [Fact(DisplayName = "BankAccount CRUD - When BankAccount was created with existed number Then system shows error message")]

        public void VerifyThatBankAccountDublicateCannotBeCreated()
        {
            var bankAccountFirst = DataFacade.BankAccount.New(defaultNumber).
                With_HolderName(name).
                SaveToDb();

            var bankAccountSecond = DataFacade.BankAccount.New(defaultNumber).
                With_HolderName(name);

            var result = BaseDataFacade.BankAccountService.Save(bankAccountSecond);

            Assert.False(result.IsSuccess, "BankAccount was created with existed number");
            Assert.Equal($"Bank Account with given Number '{defaultNumber}' already exists.", result.Messages.FirstOrDefault());
        }

        [Fact(DisplayName = "BankAccount CRUD - When Contra Bank Account for this bank already exists Then system shows error message")]

        public void VerifyThatContraBankAccountForThisBankAlreadyExists()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(defaultNumber).
                With_Name("ABCDE").
                With_Abbrev("abbrev").
                With_IBANBankIdentifier("AAAA").
                With_RecordType(CustomerRecordType.Bank).
                SaveToDb().
                Build();

            var bankAccountFirst = DataFacade.BankAccount.New(defaultNumber).
                With_HolderName(name).
                With_IsContra(true).
                With_Customer(customer.ID).
                SaveToDb();

            var bankAccountSecond = DataFacade.BankAccount.New(defaultNumber + "1").
                With_IsContra(true).
                With_Customer(customer.ID).
                With_HolderName(name);

            var result = BaseDataFacade.BankAccountService.Save(bankAccountSecond);

            Assert.False(result.IsSuccess, "BankAccount was created");
            Assert.Equal("Contra Bank Account for this bank already exists.", result.Messages.FirstOrDefault());
        }



        //    [Fact(DisplayName = "BankAccount CRUD - BankAccount was approved successfully")]

        //    public void VerifyThatBankAccountWasApprovedSuccessfully()
        //    {
        //        var bankAccount = DataFacade.BankAccount.New(defaultNumber).
        //            With_HolderName(name).
        //            SaveToDb().
        //            Build();

        //        DataFacade.BankAccount.Approve(x => x.Number == defaultNumber);

        //        Assert.True(bankAccount.Status == BankAccountStatus.Approved,$"BankAccount wasn't approved. Bank Account Status - '{bankAccount.Status}'.");
        //    }

        //    [Fact(DisplayName = "BankAccount CRUD - BankAccount was deactivated successfully")]

        //    public void VerifyThatBankAccountWasDeactivatedSuccessfully()
        //    {

        //        var bankAccount = DataFacade.BankAccount.New(defaultNumber).
        //            With_HolderName(name).
        //            SaveToDb().
        //            Build();

        //        DataFacade.BankAccount.Deactivate(x => x.Number == defaultNumber);

        //        Assert.True(bankAccount.Status == BankAccountStatus.Inactivated, $"BankAccount wasn't approved. Bank Account Status - '{bankAccount.Status}'.");
        //    }

        //    [Fact(DisplayName = "BankAccount CRUD - BankAccount was activated successfully")]

        //    public void VerifyThatBankAccountWasActivatedSuccessfully()
        //    {
        //        var bankAccount = DataFacade.BankAccount.New(defaultNumber).
        //            With_HolderName(name).
        //            SaveToDb().
        //            Build();

        //        DataFacade.BankAccount.Approve(x => x.Number == defaultNumber);
        //        DataFacade.BankAccount.Deactivate(x => x.Number == defaultNumber);
        //        DataFacade.BankAccount.Activate(x => x.Number == defaultNumber);

        //        Assert.True(bankAccount.Status == BankAccountStatus.Registered, $"BankAccount wasn't approved. Bank Account Status - '{bankAccount.Status}'.");
        //    }
    }
}
