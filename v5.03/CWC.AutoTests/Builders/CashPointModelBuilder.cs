using Cwc.Coin;
using Cwc.Common;
using Cwc.Security;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class CashPointModelBuilder
    {
        AutomationCoinDataContext _context;
        DataBaseParams _dbParams;
        MachineModel entity;

        public CashPointModelBuilder()
        {
            _dbParams = new DataBaseParams();
            _context = new AutomationCoinDataContext();
        }

        public CashPointModelBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashPointModelBuilder With_Name(String value)
        {
            if (entity != null)
            {
                entity.Name = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashPointModelBuilder With_Description(String value)
        {
            if (entity != null)
            {
                entity.Description = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public CashPointModelBuilder New()
        {
            entity = new MachineModel();

            return this;
        }

        public static implicit operator MachineModel(CashPointModelBuilder ins)
        {
            return ins.Build();
        }

        public MachineModel Build()
        {

            return entity;
        }

        public CashPointModelBuilder SaveToDb()
        {
            var cashPointModel = entity;

            var result = CoinFacade.MachineModelService.SaveMachineModel(cashPointModel, _dbParams);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(string.Format("Cash Point Model saving failed. Reason: {0}", result.GetMessage()));
            }

            return this;
        }

        public CashPointModelBuilder Take(Expression<Func<MachineModel, bool>> expression)
        {
            LoginResult login;
            login = SecurityFacade.LoginService.GetAdministratorLogin();
            var found = _context.MachineModels.Where(expression).FirstOrDefault();

            if (found == null)
            {
                throw new ArgumentNullException("Cash Point Model with provided criteria wasn't found");
            }

            entity = CoinFacade.MachineModelService.LoadMachineModel(login, found.ID);
            return this;
        }
    }
}
