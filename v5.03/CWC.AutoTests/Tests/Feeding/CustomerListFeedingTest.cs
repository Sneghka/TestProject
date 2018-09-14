using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.BaseData.Enums;
using Cwc.Feedings;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.AutoTests
{
    [Collection("MyCollection")]
    public class CustomerListFeedingTest : IDisposable
    {
        AutomationBaseDataContext _context;
        AutomationFeedingDataContext feedingContext;
        string refNumber, name;
        public CustomerListFeedingTest()
        {
            var configKey = HelperFacade.ConfigurationKeysHelper.GetKey(k => k.Name == ConfigurationKeyName.UseCwcCSMasterDataValidation);
            HelperFacade.ConfigurationKeysHelper.Update(configKey, "False");
            ConfigurationKeySet.Load();
            _context = new AutomationBaseDataContext();
            feedingContext = new AutomationFeedingDataContext();
            refNumber = $"1101{new Random().Next(0, 99999)}";
            name = "AutotestManagement";
        }

        [Fact(DisplayName = "Customer List - When CustomerList doesn't contain Name tag Then System doesn't import current customer")]
        public void VerifyFailedImportWithoutmandatoryNameFields()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Enabled(true).
                Build();
            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.Equal("OK", response.Body.ValidationResult);
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Mandatory attribute 'Name' is not submitted") && m.Action == null && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Customers.Where(c=>c.ReferenceNumber == refNumber).Any());
        }

        [Fact(DisplayName = "Customer List - When CustomerList doesn't contain Number tag Then System doesn't import current customer")]
        public void VerifyFailedImportWithoutmandatoryNumberFields()
        {
            var customer = DataFacade.Customer.New().
                With_Name(refNumber).
                With_Enabled(true).
                Build();
            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.Equal("OK", response.Body.ValidationResult);
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("") && m.Message.Contains("Mandatory attribute 'Number' is not submitted") && m.Action == null && m.Result == ValidatedFeedingLogResult.Failed).Any());
        }

        [Fact(DisplayName = "Customer List - When Number is not numeric Then System doesn't allow to import current customer")]
        public void VerifyThatImportFailedWithNonNumericNumber()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber+"A").
                With_Name(name).
                With_Enabled(true).
                Build();
            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.Equal("OK", response.Body.ValidationResult);
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Company number must be numeric") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == refNumber).Any());
        }

        [Fact(DisplayName = "Customer List - When WebSIte is incorrect Then System doesn't allow to import current customer")]
        public void VerifyThatImportFailedWithIncorrectWebSite()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_Website("qwqw").
                Build();
            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.Equal("OK", response.Body.ValidationResult);
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("URL is not valid") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == refNumber).Any());
        }

        [Fact(DisplayName = "Customer List - When Affiliated bank account is non-unique Then System doesn't allow to import current customer")]
        public void VerifyThatImportFailedWIthNonUniqueAffiliatedBankAccount()
        {
            var affiliatedBank = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber + "1").
                With_RecordType(Cwc.Security.CustomerRecordType.Bank).
                With_Name(name).
                With_IBANBankIdentifier(refNumber.ToUpper().
                Substring(0,4)).
                SaveToDb();

            var customerWithAcc = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber + "2").
                With_Name(name).With_Enabled(true).
                With_AffiliatedBank(affiliatedBank).
                SaveToDb();

            var cusWithNonUnique = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(false).
                With_AffiliatedBankID(affiliatedBank.Build().Cus_nr);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { cusWithNonUnique.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.Equal("OK", response.Body.ValidationResult);
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Company with given Affiliated Bank already exists") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == refNumber).Any());
        }

        [Fact(DisplayName = "Customer List - When Customer is saved with parent company that caused circular reference Then System doesn't allow to import current customer")]
        public void VerifyThatImportWithCircularReferenceFailed()
        {
            var firstCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(false).
                SaveToDb();

            var secondCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber+"1").
                With_Name(name).
                With_Enabled(true).
                With_ParentCustomer(firstCustomer).
                SaveToDb();

            firstCustomer.With_ParentCustomer(secondCustomer).With_Enabled(true);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { firstCustomer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("Link between selected Customer and selected Parent Customer will cause recursive-cycling linkage") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == refNumber && c.Enabled).Any());
            Assert.Equal("OK", response.Body.ValidationResult);
        }

        [Fact(DisplayName = "Customer List - When Customer is saved with non existed Parent Customer Then System doesn't allow to import current customer")]
        public void VerfifyThatImportWithNinExistedParentCustomerFailed()
        {
            var tempCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber+"1").
                With_Name(name).
                With_Enabled(false).
                SaveToDb();

            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_ParentCustomer(tempCustomer).
                With_Enabled(false);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });

            DataFacade.Customer.Delete(c => c.ReferenceNumber == refNumber + "1" && c.RecordType == 0);

            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());
            var assertMsg = $"Customer with provided Reference Number ‘{refNumber + "1"}’ does not exist";

            Assert.Equal("OK", response.Body.ValidationResult);
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == refNumber && c.Enabled).Any());
        }

        [Fact(DisplayName = "Customer List - When Customer is saved with non existed Debtor Then System doesn't allow to import current customer")]
        public void VerfifyThatImportWithNinExistedDebtorFailed()
        {
            var tempCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber + "1").
                With_Name(name).
                With_Enabled(false).
                With_RecordType(Cwc.Security.CustomerRecordType.Debtor).
                SaveToDb();

            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Debtor_nr(tempCustomer.Build().Cus_nr).
                With_Enabled(false);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });

            DataFacade.Customer.Delete(c => c.ReferenceNumber == refNumber + "1");

            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());
            var assertMsg = $"Customer with provided Reference Number ‘{refNumber + "1"}’ does not exist";

            Assert.Equal("OK", response.Body.ValidationResult);
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == refNumber && c.Enabled).Any());
        }

        [Fact(DisplayName = "Customer List - When Customer is saved with non existed Bank Then System doesn't allow to import current customer")]
        public void VerfifyThatImportWithNinExistedBankFailed()
        {
            var tempCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber + "1").
                With_Name(name).With_Enabled(false).
                With_IBANBankIdentifier(refNumber.Reverse().ToString().ToUpper().Substring(0, 4)).
                With_RecordType(Cwc.Security.CustomerRecordType.Bank).
                SaveToDb();

            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_AffiliatedBankID(tempCustomer.Build().Cus_nr).
                With_Enabled(false);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });

            DataFacade.Customer.Delete(c => c.ReferenceNumber == refNumber + "1");

            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());
            var assertMsg = $"Customer with provided Reference Number ‘{refNumber + "1"}’ does not exist";

            Assert.Equal("OK", response.Body.ValidationResult);
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == refNumber && c.Enabled).Any());
        }

        [Fact(DisplayName = "Customer List - When Customer is saved with non existed CompanyGroup Then System doesn't allow to import current customer")]
        public void VerfifyThatImportWithNinExistedCompanyGroupFailed()
        {

            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Csgrp_cd(refNumber).
                With_Enabled(false);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());
            var assertMsg = $"Customer Group with provided Code ‘{refNumber}’ does not exist.";

            Assert.Equal("OK", response.Body.ValidationResult);
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains(assertMsg) && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == refNumber && c.Enabled).Any());
        }

        [Fact(DisplayName = "Customer List - When Abbreviation is not set Then System fills it with Name")]
        public void VerifyThatSystemReplacesEmptyAbbrevValueWithName()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.True(_context.Customers.Where(c=>c.ReferenceNumber == refNumber && c.Abbrev == name).Any());
        }
        
        [Fact(DisplayName = "Customer List - When Region/bank/Custoemr/Debtor exists with the sane ref number Then System updates only customer")]
        public void VerifyThatSystemUpdatesExactlyCustomer()
        {
            var bank = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_IBANBankIdentifier(refNumber.Reverse().ToString().ToUpper().Substring(0, 4)).
                With_RecordType(Cwc.Security.CustomerRecordType.Bank).
                SaveToDb();

            var debtor = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(false).
                With_RecordType(Cwc.Security.CustomerRecordType.Debtor).
                SaveToDb();    

            var region = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(false).
                With_RecordType(Cwc.Security.CustomerRecordType.Region).
                SaveToDb();

            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(false).
                SaveToDb();

            customer.With_Name(name + "Updated");
            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.True(_context.Customers.Where(c=>c.ReferenceNumber == refNumber && c.Name == name + "Updated" && c.RecordType == 0).Any());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == refNumber && c.Name == name + "Updated" && c.RecordType != 0).Any());
        }

        [Fact(DisplayName = "Customer List - When all fields are valid Then System creates customer")]
        public void VerifyThatSystemCreatesCustomerProperly()
        {
            var abbrev = "abbrev";
            var website = @"www.website.com";
            var invoice = "invoice";
            var outbound = "outbound";
            var inbound = "inbound";
            var email = "email@gmail.com";
            var phone = "+38090-900-90-90";

            var parentCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber + "1").
                With_Name(name).
                SaveToDb();

            var affiliatedbank = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber + "1").
                With_Name(name).
                With_IBANBankIdentifier(refNumber.Reverse().ToString().ToUpper().Substring(0, 4)).
                With_RecordType(Cwc.Security.CustomerRecordType.Bank).
                SaveToDb();

            var debtor = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber + "1").
                With_Name(name).
                With_RecordType(Cwc.Security.CustomerRecordType.Debtor).
                SaveToDb();

            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(false).
                With_Abbrev(abbrev).
                With_Website(website).
                With_E_mail(email).
                With_Phone(phone).
                With_ParentCustomer(parentCustomer).
                With_AffiliatedBank(affiliatedbank).
                With_Debtor(debtor).
                With_InvoiceReference(invoice).
                With_InboundReference(inbound).
                With_OutboundReference(outbound).
                Build();
            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.Equal("OK", response.Body.ValidationResult);
            Assert.True(_context.Customers.ToArray()
                .Where(c => c.ReferenceNumber == refNumber 
                && c.Name == name 
                && c.Abbrev == abbrev 
                && c.ParentCustomer == parentCustomer.Build().Cus_nr 
                && c.AffiliatedBankID == affiliatedbank.Build().Cus_nr
                && c.Debtor_nr == debtor.Build().Cus_nr 
                && c.InvoiceReference == invoice 
                && c.InboundReference == inbound 
                && c.OutboundReference == outbound
                && c.Website == website
                && c.E_mail == email
                && c.Phone == phone).Any(), "Customer was not created or one of the properties was not set!");

        }

        [Fact(DisplayName = "Customer List - When feeding is sent with empty non mandatory tag Then System replaces existed value with empty (null) value")]
        public void VerifyThanSystemReplacesEmptyTagsProperly()
        {
            var abbrev = "abbrev";
            var website = @"www.website.com";
            var invoice = "invoice";
            var outbound = "outbound";
            var inbound = "inbound";

            var parentCustomer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber + "1").
                With_Name(name).
                SaveToDb();

            var affiliatedbank = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber + "1").
                With_Name(name).
                With_IBANBankIdentifier(refNumber.Reverse().ToString().ToUpper().Substring(0, 4)).
                With_RecordType(Cwc.Security.CustomerRecordType.Bank).
                SaveToDb();

            var debtor = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber + "1").
                With_Name(name).
                With_RecordType(Cwc.Security.CustomerRecordType.Debtor).
                SaveToDb();

            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(false).
                With_Abbrev(abbrev).
                With_Website(website).
                With_ParentCustomer(parentCustomer).
                With_AffiliatedBank(affiliatedbank).
                With_Debtor(debtor).
                With_InvoiceReference(invoice).
                With_InboundReference(inbound).
                With_OutboundReference(outbound);

            customer.With_Abbrev(null).
                With_Website(null).
                With_ParentCustomerID(null).
                With_AffiliatedBankID(null).
                With_Debtor_nr(null).
                With_InvoiceReference(null).
                With_InboundReference(null).
                With_OutboundReference(null);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.Equal("OK", response.Body.ValidationResult);
            Assert.True(_context.Customers.Where(c=>c.ReferenceNumber == refNumber && c.Abbrev == name && c.Website == null && c.ParentCustomer == null && c.AffiliatedBankID == null && c.Debtor_nr == null && c.InvoiceReference == null
            && c.InboundReference == null && c.OutboundReference == null).Any());
        }

        [Fact(DisplayName = "Customer List - When all mandatory VisitAddres fileds are filled Then System creates Visit address for current customer")]
        public void VerifyThatSystemSavesVisitAddressProperly()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_VisitAddress(DataFacade.Address.New().
                    With_Country(refNumber).
                    With_State(refNumber).
                    With_City(refNumber).
                    With_Street(refNumber).
                    With_ExtraAddressInfo(refNumber).
                    With_PostalCode(refNumber).
                    With_Purpose(Cwc.BaseData.Purpose.Visit));

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());
            var customerID = DataFacade.Customer.Take(c => c.ReferenceNumber == refNumber && c.RecordType == 0).Build().Cus_nr;

            Assert.True(_context.BaseAddresses.ToArray().Where(a=>a.Country == refNumber && a.State == refNumber && a.City == refNumber && a.Street == refNumber && a.ExtraAddressInfo == refNumber
                                     && a.PostalCode == refNumber && a.ObjectID == customerID && a.ObjectClassID.Contains("Cwc.BaseData.Customer") && a.Purpose == 0).Any());
        }

        [Fact(DisplayName = "Customer List - When all mandatory PostalAddres fileds are filled Then System creates Visit address for current customer")]
        public void VerifyThatSystemSavesPostalAddressProperly()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_PostalAddress(DataFacade.Address.New().
                    With_Country(refNumber).
                    With_State(refNumber).
                    With_City(refNumber).
                    With_Street(refNumber).
                    With_ExtraAddressInfo(refNumber).
                    With_PostalCode(refNumber).
                    With_Purpose(Cwc.BaseData.Purpose.Postal));

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());
            var customerID = DataFacade.Customer.Take(c => c.ReferenceNumber == refNumber && c.RecordType == 0).Build().Cus_nr;


            Assert.True(_context.BaseAddresses.ToArray().Where(a => a.Country == refNumber && a.State == refNumber && a.City == refNumber && a.Street == refNumber && a.ExtraAddressInfo == refNumber
                                     && a.PostalCode == refNumber && a.ObjectID == customerID && a.ObjectClassID.Contains("Cwc.BaseData.Customer") && a.Purpose == Purpose.Postal).Any());
        }

        [Fact(DisplayName = "Customer List - When Street is not specified Then System doesn't allow to save current customer and location")]
        public void VerifyThatImportFailedWhenStreetNotSpecified()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_PostalAddress(DataFacade.Address.New().With_City(refNumber));

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("City and Street are mandatory upon specifying address data")).Any());
            Assert.False(_context.Customers.Where(c=>c.ReferenceNumber == refNumber && c.RecordType == 0).Any());
        }

        [Fact(DisplayName = "Customer List - When City is not specified Then System doesn't allow to save current customer and location")]
        public void VerifyThatImportFailedWhenCityNotSpecified()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_PostalAddress(DataFacade.Address.New().With_Street(refNumber));

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(refNumber) && m.Message.Contains("City and Street are mandatory upon specifying address data")).Any());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == refNumber && c.RecordType == 0).Any());
        }

        [Fact(DisplayName = "Customer List - Verify that System updates customer properly")]
        public void VerifyThatCustomerUpdatedProperly()
        {
            var abbrev = "abbrev";
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true).
                SaveToDb();

            customer.With_Abbrev(abbrev).With_Enabled(false);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.True(_context.Customers.Where(c=>c.ReferenceNumber == refNumber && c.Abbrev == abbrev && !c.Enabled).Any());
        }

        [Fact(DisplayName = "Customer List - When new Customer is created Then System creates Log message with Action = create")]
        public void verifyThatSystemCreatesProperLogOnCreation()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());
            var asserMsg = $"Company with Number ‘{refNumber}’ has been successfully created.";

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(asserMsg) && m.Result == 0 && m.Action == 0).Any());
        }

        [Fact(DisplayName = "Customer List - When new Customer is created Then System creates Log message with Action = update")]
        public void verifyThatSystemCreatesProperLogOnUpdating()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true).
                SaveToDb();

            customer.With_Enabled(false);
            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());
            var asserMsg = $"Company with Number ‘{refNumber}’ has been successfully updated.";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(asserMsg) && m.Result == 0 && m.Action == ValidatedFeedingActionType.Update).Any());
        }

        [Fact(DisplayName = "Customer List - When feeding is submitted with empty addresses Then System deletes Then")]
        public void VerifyThatSystemDeletesAddressesWhenAddressFieldsAreEmpty()
        {
            var customer = DataFacade.Customer.New().With_ReferenceNumber(refNumber).With_Name(name).With_Enabled(true).
                                                                                                With_VisitAddress(DataFacade.Address.New()
                                                                                                        .With_Country(refNumber)
                                                                                                        .With_State(refNumber)
                                                                                                        .With_City(refNumber)
                                                                                                        .With_Street(refNumber)
                                                                                                        .With_ExtraAddressInfo(refNumber)
                                                                                                        .With_PostalCode(refNumber)
                                                                                                        .With_Purpose(Purpose.Visit)).
                                                                                                With_PostalAddress(DataFacade.Address.New()
                                                                                                        .With_Country(refNumber)
                                                                                                        .With_State(refNumber)
                                                                                                        .With_City(refNumber)
                                                                                                        .With_Street(refNumber)
                                                                                                        .With_ExtraAddressInfo(refNumber)
                                                                                                        .With_PostalCode(refNumber)
                                                                                                        .With_Purpose(Purpose.Postal))
                                                                                                .SaveToDb();

            customer.With_VisitAddress(DataFacade.Address.New()).With_PostalAddress(DataFacade.Address.New());
            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.False(_context.BaseAddresses.Where(a => a.Country == refNumber).Any());
        }

        [Fact(DisplayName = "Customer List - When feeding contains Postal Address and doesn't contain Visit Then System create Postal and deletes Visit")]
        public void VerifyThatSystemCreatesAndDeletesAddressesAtTheSaneTime()
        {
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_VisitAddress(DataFacade.Address.New().
                    With_Country(refNumber).
                    With_State(refNumber).
                    With_City(refNumber).
                    With_Street(refNumber).
                    With_ExtraAddressInfo(refNumber).
                    With_PostalCode(refNumber).
                    With_Purpose(Purpose.Visit)).SaveToDb();

            customer.With_PostalAddress(DataFacade.Address.New().
                        With_Country(refNumber).
                        With_State(refNumber).
                        With_City(refNumber).
                        With_Street(refNumber).
                        With_ExtraAddressInfo(refNumber).
                        With_PostalCode(refNumber).
                        With_Purpose(Purpose.Postal)).
                        With_VisitAddress(DataFacade.Address.New());

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.True(_context.BaseAddresses.Where(c=>c.Country == refNumber && c.Purpose == Purpose.Postal).Any());
            Assert.False(_context.BaseAddresses.Where(c => c.Country == refNumber && c.Purpose == 0).Any());
        }

        [Fact(DisplayName = "Customer List - When Customer List contains both valid and invalid customers Then System saves valid and rejected invalid")]
        public void VerifThatSystemSavesValidCustomersAndRejectedInvalid()
        {
            var customer1 = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true);

            var customer2 = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber+"1").
                With_Enabled(true);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer1.Build(), customer2.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.True(_context.Customers.Where(c=>c.ReferenceNumber == refNumber && c.RecordType == 0).Any());
            Assert.False(_context.Customers.Where(c => c.ReferenceNumber == refNumber+"1" && c.RecordType == 0).Any());
        }

        [Fact(DisplayName = "Customer List - When Customer List Contains both valid and invalid customers then System logs then properly")]
        public void VerifyThatSystemLogsBothCreatedAndrejectedCustomers()
        {
            var customer1 = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true);

            var customer2 = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber + "1").
                With_Enabled(true);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer1.Build(), customer2.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m=>m.Message.Contains(refNumber) && m.Message.Contains("created") && m.Result == 0 && m.Action == 0).Any());
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m=>m.Message.Contains(refNumber+"1") && m.Result == ValidatedFeedingLogResult.Failed && m.Action == null).Any());
        }

        [Fact(DisplayName = "Customer List - When feeding doesn't contain address tag Then this value is skipped")]
        public void VerifyWhenAddressTagIsOmmitedThemValueIsNotDeleted()
        {
            var customer = DataFacade.Customer.New().With_ReferenceNumber(refNumber).With_Name(name).With_Enabled(false).
                                                                                                With_VisitAddress(DataFacade.Address.New()
                                                                                                        .With_Country(refNumber)
                                                                                                        .With_State(refNumber)
                                                                                                        .With_City(refNumber)
                                                                                                        .With_Street(refNumber)
                                                                                                        .With_ExtraAddressInfo(refNumber)
                                                                                                        .With_PostalCode(refNumber)
                                                                                                        .With_Purpose(Purpose.Visit)).
                                                                                                With_PostalAddress(DataFacade.Address.New()
                                                                                                        .With_Country(refNumber)
                                                                                                        .With_State(refNumber)
                                                                                                        .With_City(refNumber)
                                                                                                        .With_Street(refNumber)
                                                                                                        .With_ExtraAddressInfo(refNumber)
                                                                                                        .With_PostalCode(refNumber)
                                                                                                        .With_Purpose(Purpose.Postal))
                                                                                                .SaveToDb();

            var feeding = $@"<DocumentElement>
                              <CompanyList>
                                <Company>
                                  <Number>{refNumber}</Number>
                                  <Name>AutotestManagement</Name>
                                  <IsEnabled>yes</IsEnabled>
                                </Company>
                              </CompanyList>
                            </DocumentElement>";

            var response = HelperFacade.FeedingHelper.SendFeeding(feeding);

            Assert.True(_context.BaseAddresses.ToArray().Where(a=>a.Country == refNumber && a.ObjectID == customer.Build().Cus_nr && a.Purpose == Purpose.Visit).Any());
            Assert.True(_context.BaseAddresses.ToArray().Where(a => a.Country == refNumber && a.ObjectID == customer.Build().Cus_nr && a.Purpose == Purpose.Postal).Any());
        }

        [Fact(DisplayName = "Customer List - When feeding doesn't contain base tag Then this value is skipped")]
        public void VerifyWhenBaseTagIsOmittedThemValueIsNotDeleted()
        {
            var abbrev = "abbrev";
            var website = @"www.website.com";
            var invoice = "invoice";
            var outbound = "outbound";
            var inbound = "inbound";
            var email = "9999@gmail.com";
            var phone = "+38090-900-90-90";
            var parentCustomer = DataFacade.Customer
                .New()
                .With_ReferenceNumber(refNumber + "1")
                .With_Name(name)
                .SaveToDb();
            var affiliatedbank = DataFacade.Customer
                .New()
                .With_ReferenceNumber(refNumber + "1")
                .With_Name(name)
                .With_IBANBankIdentifier(refNumber.Reverse().ToString().ToUpper().Substring(0, 4))
                .With_RecordType(Cwc.Security.CustomerRecordType.Bank)
                .SaveToDb();
            var debtor = DataFacade.Customer
                .New()
                .With_ReferenceNumber(refNumber + "1")
                .With_Name(name)
                .With_RecordType(Cwc.Security.CustomerRecordType.Debtor)
                .SaveToDb();
            var customer = DataFacade.Customer
                .New()
                .With_ReferenceNumber(refNumber)
                .With_Name(name)
                .With_Enabled(false)
                .With_Abbrev(abbrev)
                .With_Website(website)
                .With_Phone(phone)
                .With_E_mail(email)
                .With_ParentCustomer(parentCustomer)
                .With_AffiliatedBank(affiliatedbank)
                .With_Debtor(debtor)
                .With_InvoiceReference(invoice)
                .With_InboundReference(inbound)
                .With_OutboundReference(outbound)
                .SaveToDb();

            var feeding = $@"<DocumentElement>
                              <CompanyList>
                                <Company>
                                  <Number>{refNumber}</Number>
                                  <Name>AutotestManagement</Name>
                                  <IsEnabled>yes</IsEnabled>
                                </Company>
                              </CompanyList>
                            </DocumentElement>";

            var response = HelperFacade.FeedingHelper.SendFeeding(feeding);

            Assert.True(_context.Customers.ToArray()
                .Where(c => c.ReferenceNumber == refNumber
                && c.Abbrev == abbrev 
                && c.Website == website 
                && c.ParentCustomer == parentCustomer.Build().Cus_nr 
                && c.InboundReference == inbound
                && c.InvoiceReference == invoice
                && c.OutboundReference == outbound
                && c.Enabled                
                && c.Phone == phone
                && c.E_mail == email).Any(), "Customer was updated incorrectly.");
        }


        [Fact(DisplayName = "Customer List - Customer with InvoiceReference, InboundReference, OutboundReference up to 50 symbols can be created")]
        public void VerifyThatCustomerCanBeCreatedWithReferenceFields()
        {
            var reference = "12345123451234512345123451234512345123451234512345";
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_InvoiceReference(reference).
                With_InboundReference(reference).
                With_OutboundReference(reference);

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            var asserMsg = $"Company with Number ‘{refNumber}’ has been successfully created.";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(asserMsg) && m.Result == 0 && m.Action == 0).Any());
        }

        [Fact (DisplayName = "Customer List - Customer with OutboundReference with more then 50 symbols cannot be created")]
        public void VerifyThatCustomerWithOutboundReferenceMoreThenLimitCannotBeCreated()
        {
            var reference = "12345123451234512345123451234512345123451234512345";
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_OutboundReference(reference + "1");

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            var assertMsg = $"Error for Company with Number ‘{refNumber}’:Value of property 'Outbound Reference' is too long: '{reference + "1"}'";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(assertMsg)).Any(), "Message is incorrect");
        }

        [Fact(DisplayName = "Customer List - Customer with InboundReference with more then 50 symbols cannot be created")]
        public void VerifyThatCustomerWithInboundReferenceMoreThenLimitCannotBeCreated()
        {
            var reference = "12345123451234512345123451234512345123451234512345";
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_InboundReference(reference + "1");

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            var assertMsg = $"Error for Company with Number ‘{refNumber}’:Value of property 'Inbound Reference' is too long: '{reference + "1"}'";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(assertMsg)).Any(), "Message is incorrect");
        }
        [Fact(DisplayName = "Customer List - Customer with InvoiceReference with more then 50 symbols cannot be created")]
        public void VerifyThatCustomerWithInvoiceReferenceMoreThenLimitCannotBeCreated()
        {
            var reference = "12345123451234512345123451234512345123451234512345";
            var customer = DataFacade.Customer.New().
                With_ReferenceNumber(refNumber).
                With_Name(name).
                With_Enabled(true).
                With_InvoiceReference(reference + "1");

            var convertedCustomer = HelperFacade.EntityToXmlConverterHelper.ConvertToFeeding(new[] { customer.Build() });
            var response = HelperFacade.FeedingHelper.SendFeeding(convertedCustomer.ToString());

            var assertMsg = $"Error for Company with Number ‘{refNumber}’:Value of property 'Invoice Reference' is too long: '{reference + "1"}'";
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(l => l.Message.Contains(assertMsg)).Any(), "Message is incorrect");
        }

        public void Dispose()
        {
            feedingContext.ValidatedFeedingLogs.RemoveRange(feedingContext.ValidatedFeedingLogs);
            _context.Customers.RemoveRange(_context.Customers.Where(c => c.Name == "AutotestManagement"));
            feedingContext.SaveChanges();
            _context.SaveChanges();
            _context.Dispose();
            feedingContext.Dispose();
        }
    }
}
