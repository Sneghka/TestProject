using Cwc.BaseData;
using Cwc.BaseData.Enums;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class CustomerBankAccountBuilder
    {
        CustomerBankAccount cba;
        DataBaseParams dbParams;
        AutomationBaseDataContext context;

        public CustomerBankAccountBuilder()
        {
            cba = new CustomerBankAccount();
            dbParams = new DataBaseParams();
            context = new AutomationBaseDataContext();
            cba.IsDefault = false;
        }

        public CustomerBankAccountBuilder With_Customer(Customer value)
        {
            cba.CompanyID = value.ID;
            return this;
        }

        public CustomerBankAccountBuilder With_Customer(decimal value)
        {
            cba.CompanyID = value;
            return this;
        }

        public CustomerBankAccountBuilder With_BankAccount(int value)
        {
            cba.BankAccountID = value;
            return this;
        }

        public CustomerBankAccountBuilder With_IsDefault(bool value)
        {
            cba.IsDefault = value;
            return this;
        }

        public CustomerBankAccountBuilder With_Purpose(BankAccountPurposeType value)
        {
            cba.Purpose = value;
            return this;
        }

        public CustomerBankAccountBuilder With_Purpose(string value)
        {
            cba.MaterialTypeCode = value;
            return this;
        }

        public static implicit operator CustomerBankAccount(CustomerBankAccountBuilder ins)
        {
            return ins.Build();
        }

        public CustomerBankAccount Build()
        {
            return cba;
        }

        public CustomerBankAccountBuilder SaveToDb()
        {
            var temp = this.cba;

            var result = BaseDataFacade.CustomerBankAccountService.Save(cba, dbParams);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Customer Bank Account saving failed. Reason: {result.GetMessage()}");
            }

            return this;
        }

        public CustomerBankAccountBuilder Take(Func<CustomerBankAccount, bool> expression)
        {
            var temp = context.CustomerBankAccounts.Where(expression).FirstOrDefault();

            if (temp == null)
            {
                throw new ArgumentNullException("Customer Bank Account with provided criteria is not found");
            }

            cba = BaseDataFacade.CustomerBankAccountService.Load(temp.ID, dbParams);

            return this;
        }
    }
}
