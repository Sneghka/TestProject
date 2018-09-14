using Cwc.BaseData;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class BankAccountBuilder : IDisposable
    {
        AutomationBaseDataContext _context;
        DataBaseParams _dbParams;
        BankAccount entity;

        public BankAccountBuilder()
        {
            _dbParams = new DataBaseParams();
            _context = new AutomationBaseDataContext();
        }

        public BankAccountBuilder With_IsIBAN(Boolean value)
        {
            if (entity != null)
            {
                entity.IsIBAN = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BankAccountBuilder With_HolderName(String value)
        {
            if (entity != null)
            {
                entity.HolderName = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BankAccountBuilder With_IsContra(Boolean value)
        {
            if (entity != null)
            {
                entity.IsContra = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BankAccountBuilder With_Customer(Decimal? value)
        {
            if (entity != null)
            {
                entity.Customer = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BankAccountBuilder With_BankIdentifierCodeID(Int32? value)
        {
            if (entity != null)
            {
                entity.BankIdentifierCodeID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BankAccountBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public BankAccountBuilder New(string number)
        {
            entity = new BankAccount(number);
            return this;
        }

        public static implicit operator BankAccount(BankAccountBuilder ins)
        {
            return ins.Build();
        }

        public BankAccount Build()
        {
            return entity;
        }

        public BankAccountBuilder SaveToDb()
        {
            var temp = entity;
            var res = BaseDataFacade.BankAccountService.Save(temp, _dbParams);
            if (!res.IsSuccess)
            {
                throw new InvalidOperationException($"Bank Account saving failed. Reason: {res.GetMessage()}");
            }
            return this;
        }

        public BankAccountBuilder Take(Expression<Func<BankAccount, bool>> expression)
        {
            var bankAccount = _context.BankAccounts.Where(expression).FirstOrDefault();

            if (bankAccount == null)
            {
                throw new ArgumentNullException("Bank Account with provided criteria wasn't found");
            }
            entity = BaseDataFacade.BankAccountService.Load(bankAccount.ID, null);
            return this;
        }

        //public void Approve(Expression<Func<WP_BaseData_BankAccount, bool>> expression)
        //{
        //    var userID = SecurityFacade.LoginService.GetAdministratorLogin().UserID;
        //    UserParams userParams = new UserParams(userID);

        //    using (var context = new DataModel.ModelContext())
        //    {
        //        var bankAccounts = context.WP_BaseData_BankAccounts.Where(expression).Select(x => x.id).ToArray();
        //        if (bankAccounts == null)
        //        {
        //            throw new ArgumentNullException("Bank Account with provided criteria wasn't found");
        //        }

        //        var res = BaseDataFacade.BankAccountService.ApproveBankAccount(bankAccounts, userParams, null);
        //        if (!res.IsSuccess)
        //        {
        //            throw new InvalidOperationException($"Bank Account Approvement failed. Reason: {res.GetMessage()}");
        //        }

        //    }
        //}

        //public void Activate(Expression<Func<WP_BaseData_BankAccount, bool>> expression)
        //{
        //    var userID = SecurityFacade.LoginService.GetAdministratorLogin().UserID;
        //    UserParams userParams = new UserParams(userID);

        //    using (var context = new DataModel.ModelContext())
        //    {
        //        var bankAccounts = context.WP_BaseData_BankAccounts.Where(expression).Select(x => x.id).ToArray();
        //        if (bankAccounts == null)
        //        {
        //            throw new ArgumentNullException("Bank Account with provided criteria wasn't found");
        //        }

        //        var res = BaseDataFacade.BankAccountService.ActivateBankAccount(bankAccounts, userParams, null);
        //        if (!res.IsSuccess)
        //        {
        //            throw new InvalidOperationException($"Bank Account Approvement failed. Reason: {res.GetMessage()}");
        //        }
        //    }
        //}
        //public void Deactivate(Expression<Func<WP_BaseData_BankAccount, bool>> expression)
        //{
        //    var userID = SecurityFacade.LoginService.GetAdministratorLogin().UserID;
        //    UserParams userParams = new UserParams(userID);

        //    using (var context = new DataModel.ModelContext())
        //    {
        //        var bankAccounts = context.WP_BaseData_BankAccounts.Where(expression).Select(x => x.id).ToArray();
        //        if (bankAccounts == null)
        //        {
        //            throw new ArgumentNullException("Bank Account with provided criteria wasn't found");
        //        }

        //        var res = BaseDataFacade.BankAccountService.DeactivateBankAccount(bankAccounts, userParams, null);
        //        if (!res.IsSuccess)
        //        {
        //            throw new InvalidOperationException($"Bank Account Approvement failed. Reason: {res.GetMessage()}");
        //        }
        //    }
        //}

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}