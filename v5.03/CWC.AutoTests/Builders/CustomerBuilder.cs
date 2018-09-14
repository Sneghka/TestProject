using Cwc.BaseData;
using Cwc.Common;
using Cwc.Security;
using CWC.AutoTests.Model;
using CWC.AutoTests.Utils;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class CustomerBuilder : IDisposable
    {
        AutomationBaseDataContext _context;
        DataBaseParams _dbParams;
        Customer entity;

        public CustomerBuilder()
        {
            _dbParams = new DataBaseParams();
            _context = new AutomationBaseDataContext();
        }

        public CustomerBuilder With_Abbrev(string value)
        {
            entity.Abbrev = value;
            return this;
        }

        public CustomerBuilder With_PO_PostalCode(string value)
        {
            entity.Pcode_pb = value;
            return this;
        }

        public CustomerBuilder With_PO_Box(string value)
        {
            entity.Pobox = value;
            return this;
        }

        public CustomerBuilder With_PO_City(string value)
        {
            entity.Town_pb = value;
            return this;
        }

        public CustomerBuilder With_Enabled(bool value)
        {
            entity.Enabled = value;
            return this;
        }

        public CustomerBuilder With_National(bool? value)
        {
            entity.National = value;
            return this;
        }

        public CustomerBuilder With_MsCode(string value)
        {
            entity.MsCode = value;
            return this;
        }

        public CustomerBuilder With_Csgrp_cd(string value)
        {
            entity.Csgrp_cd = value;
            return this;
        }

        public CustomerBuilder With_Name(string value)
        {
            entity.Name = value;

            return this;
        }

        public CustomerBuilder With_Cus_cd(string value)
        {
            entity.Cus_cd = value;

            return this;
        }

        public CustomerBuilder With_RecordType(CustomerRecordType value)
        {
            entity.RecordType = value;

            return this;
        }

        public CustomerBuilder With_Chamber(string value)
        {
            entity.Chamber = value;

            return this;
        }

        public CustomerBuilder With_Website(string value)
        {
            entity.Website = value;

            return this;
        }

        public CustomerBuilder With_ParentCustomerID(decimal? value)
        {
            entity.ParentCustomer = value;

            return this;
        }

        public CustomerBuilder With_ParentCustomer(Customer value)
        {
            entity.ParentCompany = value;

            this.With_ParentCustomerID(entity.ParentCompany.Cus_nr);

            return this;
        }

        public CustomerBuilder With_InvoiceReference(string value)
        {
            entity.InvoiceReference = value;

            return this;
        }

        public CustomerBuilder With_InboundReference(string value)
        {
            entity.InboundReference = value;

            return this;
        }

        public CustomerBuilder With_OutboundReference(string value)
        {
            entity.OutboundReference = value;

            return this;
        }

        public CustomerBuilder With_Code(string value)
        {
            entity.Code = value;

            return this;
        }

        public CustomerBuilder With_VisitAddress(BaseAddressBuilder address)
        {

            entity.VisitAddress = address;

            return this;
        }

        public CustomerBuilder With_PostalAddress(BaseAddressBuilder address)
        {

            entity.PostalAddress = address;

            return this;
        }

        public CustomerBuilder With_IBANBankIdentifier(string value)
        {
            entity.IBANBankIdentifier = value;

            return this;
        }

        public CustomerBuilder With_LedgerCode(string value)
        {
            entity.LedgerCode = value;

            return this;
        }

        public CustomerBuilder With_VATCode(string value)
        {
            entity.VATCode = value;

            return this;
        }

        public CustomerBuilder With_Debtor_nr(decimal? value)
        {
            entity.Debtor_nr = value;

            return this;
        }

        public CustomerBuilder With_Debtor(Customer value)
        {
            if (value != null)
            {
                this.With_Debtor_nr(value.Cus_nr);
            }
            else
            {
                this.With_Debtor_nr(null);
            }

            return this;
        }

        public CustomerBuilder With_PreferredLanguage(int value)
        {
            entity.PreferredLanguage = value;

            return this;
        }

        public CustomerBuilder With_ReferenceNumber(string value)
        {
            entity.ReferenceNumber = value;

            return this;
        }

        public CustomerBuilder With_OutboundStreamIDForNotes(string value)
        {
            entity.OutboundStreamIDForNotes = value;

            return this;
        }

        public CustomerBuilder With_OutboundStreamIDForCoins(string value)
        {
            entity.OutboundStreamIDForCoins = value;

            return this;
        }

        public CustomerBuilder With_AffiliatedBankID(decimal? value)
        {
            entity.AffiliatedBankID = value;

            return this;
        }

        public CustomerBuilder With_AffiliatedBank(CustomerBuilder value)
        {
            entity.AffiliatedBank = value;
            this.With_AffiliatedBankID(entity.AffiliatedBank.Cus_nr);
            return this;
        }

        public CustomerBuilder With_IBANFormatID(int? value)
        {
            entity.IBANFormatID = value;
            return this;
        }

        public CustomerBuilder With_Repr(string value)
        {
            entity.Repr = value;
            return this;
        }

        public CustomerBuilder With_E_mail(string value)
        {
            entity.E_mail = value;
            return this;
        }

        public CustomerBuilder With_Phone(string value)
        {
            entity.Phone = value;
            return this;
        }

        public CustomerBuilder With_ChequeFormatID(int? value)
        {
            entity.ChequeFormatID = value;
            return this;
        }

        public CustomerBuilder New()
        {
            object locker = new object();
            lock (locker)
            {
                entity = new Customer();
            }
            using (var context = new AutomationBaseDataContext())
            {
                context.Customers.Take(context.Customers.Select(x => x.IdentityID).First());
            }

            return this;
        }

        public static implicit operator Customer(CustomerBuilder ins)
        {
            return ins.Build();
        }

        public Customer Build()
        {
            return entity;
        }

        public CustomerBuilder SaveToDb()
        {
            object locker = new object();
            var customer = entity;

            lock (locker)
            {
                var result = BaseDataFacade.CustomerService.SaveCustomer(customer, _dbParams);

                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException(string.Format("Customer saving failed. Reason: {0}", result.GetMessage()));
                }
            }

            return this;

        }

        public CustomerBuilder Take(Expression<Func<Customer, bool>> expression, bool asNoTracking = false)
        {
            object locker = new object();
            lock (locker)
            {
                entity = asNoTracking ? _context.Customers.AsNoTracking().FirstOrDefault(expression) : _context.Customers.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("Customer with provided criteria wasn't found");
                }
            }

            return this;
        }

        public void Delete(Expression<Func<Customer, bool>> expression)
        {
            object locker = new object();
            lock (locker)
            {
                var customers = _context.Customers.Where(expression);

                if (customers == null)
                {
                    throw new ArgumentNullException("Customer with provided criteria wasn't found");
                }

                if (customers.Any(x=>x.Enabled))
                {
                    var enabledCustomers = customers.Where(x => x.Enabled).Select(x=>x.ID).ToArray();

                    BaseDataFacade.CustomerService.EnableDisableCustomers(enabledCustomers);
                }
                

                var res = BaseDataFacade.CustomerService.DeleteCustomerById(SecurityFacade.LoginService.GetAdministratorLogin(), customers.Select(x=>x.ID).ToArray());

                if (!res.IsSuccess)
                {
                    throw new InvalidOperationException($"Customers weren't deleted. Reson: {res.GetMessage()}");
                }
            }

        }

        public CustomerBuilder InitDefault()
        {
            this.New()
                .With_Abbrev(ValueGenerator.GenerateString("SP", 6))
                .With_Code(ValueGenerator.GenerateNumber())
                .With_Name(ValueGenerator.GenerateString("SP", 6))
                .With_ReferenceNumber(ValueGenerator.GenerateNumber())
                .With_Csgrp_cd("1001");

            return this;
        }

        public void Dispose()
        {
            BaseDataFacade.CustomerService.Delete(entity);
            _context.Dispose();
        }
    }
}