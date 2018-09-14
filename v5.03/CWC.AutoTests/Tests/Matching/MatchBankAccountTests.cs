using Cwc.BaseData;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.Matching
{
    public class MatchBankAccountTests : IDisposable
    {
        AutomationBaseDataContext context;

        public MatchBankAccountTests()
        {
            Cwc.BaseData.Classes.ConfigurationKeySet.Load();
            context = new AutomationBaseDataContext();
        }
       
        [Fact(DisplayName = "When location is not empty and it's bank account is not exmpty Then System matches this bank account")]
        public void VerifyThatSystemMatchesBankAccountByLocation()
        {
            var location = DataFacade.Location.Take(l => l.BankAccount_Id != null).Build();

            var matchingResult = BaseDataFacade.MatchingService.MatchBankAccount(location);

            Assert.Equal(location.BankAccount_Id, matchingResult.ID);
        }

        [Fact(DisplayName = "When location bank account is empty Then System takes Customer bank account")]
        public void VerifyThatSystemUsesCustomerBankAccountIfLocationBankAccountIsEmpty()
        {
            var locationtype = DataFacade.LocationType.Take(l => l.Category == (int)LocationTypeCategory.Retail).Build();
            var site = DataFacade.Site.Take(s => s.BranchType == BranchType.CITDepot && s.WP_IsExternal == false);
            var customer = DataFacade.Customer.New().With_ReferenceNumber($"1101{new Random().Next(1000, 999999)}").With_Name("delete").With_Abbrev("delete").With_Csgrp_cd("10").SaveToDb().Build();
            var bankAccount = DataFacade.BankAccount.Take(b => b.IsContra == false).Build();
            var customerBankAccount = DataFacade.CustomerBankAccount.With_Customer(customer).With_BankAccount(bankAccount.ID).With_IsDefault(true).SaveToDb();
            var location = DataFacade.Location.New()
                .With_Code("133199")
                .With_Name("delete")
                .With_Abbreviation("delete")
                .With_Company(customer)
                .With_ServicingDepot(site)
                .With_LocationTypeID(locationtype.ltCode)
                .With_HandlingType(BaseDataFacade.LocationNorType)
                .With_Level(Cwc.BaseData.Enums.LocationLevel.ServicePoint)
                .SaveToDb()
                .Build();

            var matchingResult = BaseDataFacade.MatchingService.MatchBankAccount(location);

            Assert.Equal(bankAccount.ID, matchingResult.ID);
        }

        [Fact(DisplayName = "When both Location and Customer contains bank account Then System selects Location -> Bank Account")]
        public void VerifyWhenBothCustomerAndLocationContainsBankAccountSystemSelectsLocation()
        {
            var location = DataFacade.Location.Take(l => l.BankAccount_Id != null).Build();
            var customer = DataFacade.Customer.Take(c => c.ID == location.CompanyID).Build();
            var bankAccount = DataFacade.BankAccount.Take(b => b.ID != location.BankAccount_Id).Build();

            var cba = !context.CustomerBankAccounts.Where(x => x.CompanyID == customer.ID).Any() ? DataFacade.CustomerBankAccount.With_Customer(customer).With_BankAccount(bankAccount.ID).With_IsDefault(true).SaveToDb() : null;

            var matchingResult = BaseDataFacade.MatchingService.MatchBankAccount(location);

            Assert.Equal(location.BankAccount_Id, matchingResult.ID);
        }

        [Fact(DisplayName = "When locations Bank Account is not set Then System uses Location -> Customers")]
        public void VerifyThatWhenLocationBankAccountISNotSetThenSystemUsesCustomers()
        {
            var location = DataFacade.Location.Take(l => l.BankAccount_Id == null).Build();
            var customer = DataFacade.Customer.Take(c => c.ID == location.CompanyID).Build();
            var bankAccount = DataFacade.BankAccount.Take(b => b.ID != location.BankAccount_Id).Build();

            var cba = !context.CustomerBankAccounts.Where(x => x.CompanyID == customer.ID).Any() ? DataFacade.CustomerBankAccount.With_Customer(customer).With_BankAccount(bankAccount.ID).With_IsDefault(true).SaveToDb() :
                DataFacade.CustomerBankAccount.Take(c => c.CompanyID == customer.ID && c.IsDefault);

            var matchingResult = BaseDataFacade.MatchingService.MatchBankAccount(location);

            Assert.Equal(cba.Build().BankAccountID, matchingResult.ID);

        }

        [Fact(DisplayName = "When System attempts to match bank account by customer Then it takes only default bank account")]
        public void VerifyThatSystemTakesOnleDefaultBankAccountFromCustomer()
        {
            var locationtype = DataFacade.LocationType.Take(l => l.Category == (int)LocationTypeCategory.Retail).Build();
            var site = DataFacade.Site.Take(s => s.BranchType == BranchType.CITDepot && s.WP_IsExternal == false);
            var customer = DataFacade.Customer.New().With_ReferenceNumber($"1101{new Random().Next(1000, 999999)}").With_Name("delete").With_Abbrev("delete").With_Csgrp_cd("10").SaveToDb().Build();

            var bankAccountFirst = DataFacade.BankAccount.Take(b => b.IsContra == false).Build();
            var bankAccountSecond = DataFacade.BankAccount.Take(b => b.ID != bankAccountFirst.ID).Build();

            var customerBankAccountDeault = DataFacade.CustomerBankAccount.With_Customer(customer).With_BankAccount(bankAccountFirst.ID).With_IsDefault(true).SaveToDb().Build();
            var customerBankAccountSecond = DataFacade.CustomerBankAccount.With_Customer(customer).With_BankAccount(bankAccountSecond.ID).With_IsDefault(false).SaveToDb().Build();
            var location = DataFacade.Location.New()
                .With_Code("133199")
                .With_Name("delete")
                .With_Abbreviation("delete")
                .With_Company(customer)
                .With_ServicingDepot(site)
                .With_LocationTypeID(locationtype.ltCode)
                .With_HandlingType(BaseDataFacade.LocationNorType)
                .With_Level(Cwc.BaseData.Enums.LocationLevel.ServicePoint)
                .SaveToDb()
                .Build();

            var matchingResult = BaseDataFacade.MatchingService.MatchBankAccount(location);

            Assert.Equal(bankAccountFirst.ID, matchingResult.ID);
        }

        [Fact(DisplayName = "When location is absent Then System attempts to find Bank Account by Customer")]
        public void VerifyThatBankAccountCanBeMatchedByCustomerOnly()
        {
            var customer = DataFacade.Customer.New().With_ReferenceNumber($"1101{new Random().Next(1000, 999999)}").With_Name("delete").With_Abbrev("delete").With_Csgrp_cd("10").SaveToDb().Build();

            var bankAccountDefault = DataFacade.BankAccount.Take(b => b.IsContra == false).Build();
            var bankAccountSecond = DataFacade.BankAccount.Take(b => b.ID != bankAccountDefault.ID).Build();

            var customerBankAccountDeault = DataFacade.CustomerBankAccount.With_Customer(customer).With_BankAccount(bankAccountDefault.ID).With_IsDefault(true).SaveToDb().Build();
            var customerBankAccountSecond = DataFacade.CustomerBankAccount.With_Customer(customer).With_BankAccount(bankAccountSecond.ID).With_IsDefault(false).SaveToDb().Build();

            var matchingResult = BaseDataFacade.MatchingService.MatchBankAccount(null, customer);

            Assert.Equal(bankAccountDefault.ID, matchingResult.ID);
        }


        [Fact(DisplayName = "When bank is not found Then System returns null")]
        public void VerifyWheBankAccountIsNotFoundThenSystemReturnsNull()
        {
            var locationtype = DataFacade.LocationType.Take(l => l.Category == (int)LocationTypeCategory.Retail).Build();
            var site = DataFacade.Site.Take(s => s.BranchType == BranchType.CITDepot && s.WP_IsExternal == false);
            var customer = DataFacade.Customer.New().With_ReferenceNumber($"1101{new Random().Next(1000, 999999)}").With_Name("delete").With_Abbrev("delete").With_Csgrp_cd("10").SaveToDb().Build();
            var location = DataFacade.Location.New()
               .With_Code("133199")
               .With_Name("delete")
               .With_Abbreviation("delete")
               .With_Company(customer)
               .With_ServicingDepot(site)
               .With_LocationTypeID(locationtype.ltCode)
               .With_HandlingType(BaseDataFacade.LocationNorType)
               .With_IsInheritFromVisitAddress(false)
               .With_Level(Cwc.BaseData.Enums.LocationLevel.ServicePoint)
               .SaveToDb();

            var matchingResult = BaseDataFacade.MatchingService.MatchBankAccount(location);

            Assert.Null(matchingResult);
        }

        public void Dispose()
        {
            var cusIds = context.Customers.Where(c => c.Name == "delete").Select(x => x.ID).ToArray();
            var locationIds = context.Locations.Where(l => cusIds.Contains(l.ID)).Select(x => x.ID).ToArray();

            context.CustomerBankAccounts.RemoveRange(context.CustomerBankAccounts.Where(cba => cusIds.Contains(cba.CompanyID)));
            context.Customers.RemoveRange(context.Customers.Where(c => cusIds.Contains(c.ID)));

            using (var transportContext = new AutomationTransportDataContext())
            {
                transportContext.CitProcessSettingLinks.RemoveRange(transportContext.CitProcessSettingLinks.Where(l => locationIds.Contains(l.LocationID)));
                transportContext.SaveChanges();
            }

            context.Locations.RemoveRange(context.Locations.Where(l => locationIds.Contains(l.ID)));
            context.SaveChanges();
            context.Dispose();
        }
            
    }
}
