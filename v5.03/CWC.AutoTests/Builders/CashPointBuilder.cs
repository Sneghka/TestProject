using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.BaseData.Model;
using Cwc.Coin;
using Cwc.Coin.ConstantNames;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class CashPointBuilder : IDisposable
    {
        AutomationCoinDataContext _context;
        DataBaseParams _dbParams;
        CoinMachine entity;

        public CashPointBuilder()
        {
            ConfigurationKeySet.Load();
            _dbParams = new DataBaseParams(new System.Data.SqlClient.SqlConnection(DataBaseHelper.GetConnectionString()));
            _context = new AutomationCoinDataContext();
        }

        #region DataInit
        public CashPointBuilder With_ID(Int32 value)
        {
            entity.ID = value;

            return this;
        }

        public CashPointBuilder WithNumber(String value)
        {
            entity.Number = value;

            return this;
        }

        public CashPointBuilder WithType(CashPointType value)
        {
            entity.Type = value.ID;

            return this;
        }

        public CashPointBuilder WithModel(MachineModel value)
        {
            entity.MachineModelId = value.ID;

            return this;
        }

        public CashPointBuilder WithStatus(Int32 value)
        {
            entity.Status = value;

            return this;
        }

        public CashPointBuilder WithDateCreated(DateTime value)
        {
            var change = _context.CashPoints.FirstOrDefault(c => c.Number == entity.Number);
            change.DateCreated = value;
            _context.SaveChanges();

            return this;
        }

        public CashPointBuilder WithDateUpdated(DateTime value)
        {
            var change = _context.CashPoints.FirstOrDefault(c => c.Number == entity.Number);
            change.DateUpdated = value;
            _context.SaveChanges();

            return this;
        }

        public CashPointBuilder WithLocationID(Decimal value)
        {
            entity.SetLocationID(value);

            return this;
        }

        public CashPointBuilder WithLocation(Location value)
        {
            entity.SetLocationID(value.ID);

            return this;
        }

        public CashPointBuilder WithOwner(Decimal value)
        {
            entity.OwnerId = value;

            return this;
        }

        public CashPointBuilder WithSupplier(Decimal value)
        {
            entity.SupplierId = value;

            return this;
        }

        public CashPointBuilder WithName(String value)
        {
            entity.Name = value;

            return this;
        }

        public CashPointBuilder WithOptimization(OrderCreation value)
        {
            entity.Optimization = value;

            return this;
        }

        public CashPointBuilder WithIndividualStock(Boolean value)
        {
            entity.IndividualStockConfiguration = value;

            return this;
        }

        public CashPointBuilder WithRemainderOfStock(Int32 value)
        {
            entity.RemainderOfStockPerCassette = value;

            return this;
        }

        public CashPointBuilder WithOrderMandatory(Boolean value)
        {
            entity.OrderMandatory = value;

            return this;
        }

        public CashPointBuilder WithForceConfirmation(Boolean value)
        {
            entity.ForceConfirmation = value;

            return this;
        }

        public CashPointBuilder WithReplenishment(Cwc.Coin.ConstantNames.Enums.CoinMachine.ReplenishmentMethod value)
        {
            entity.Replenishment = value;

            return this;
        }

        public CashPointBuilder WithStockExpiration(AllowStockExpirationType value)
        {
            entity.AllowStockExpiration = value;

            return this;
        }

        public CashPointBuilder WithManualTransactions(Boolean value)
        {
            entity.IsAllowManualTransactions = value;

            return this;
        }

        public CashPointBuilder WithCollectOnlyServType(ServiceType value)
        {            
            entity.ServiceTypeCollectOnlyId = value.ID;
            return this;
        }

        public CashPointBuilder WithCollectAndDeliveryServType(ServiceType value)
        {
            entity.ServiceTypeCollectDeliver = value.ID;
            entity.CollectDeliverServiceTypeCode = value.Code;
            entity.CollectDeliverServiceTypeId = value.ID;

            return this;
        }

        public CashPointBuilder New()
        {
            entity = new CoinMachine();
            
            return this;
        }

        public static implicit operator CoinMachine(CashPointBuilder ins)
        {
            return ins.Build();
        }

        public CoinMachine Build()
        {
            return entity;
        }

        public CashPointBuilder SaveToDb()
        {
            var cashPoint = entity;

            if (_dbParams.Connection.State == System.Data.ConnectionState.Closed)
            {
                _dbParams.Connection.Open();
            }

            var result = CoinFacade.CoinMachineService.SaveCoinMachine(cashPoint, true, _dbParams);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(string.Format("Cash Point saving failed. Reason: {0}", result.GetMessage()));
            }

            return this;

        }

        public CashPointBuilder Take(Func<CoinMachine, bool> expression)
        {
            var found = _context.CashPoints.FirstOrDefault(expression);

            if (found == null)
            {
                throw new ArgumentNullException("Cash Point with provided criteria wasn't found");
            }

            return this;
        }

        public void Delete(Func<CoinMachine, bool> expression)
        {
            var found = _context.CashPoints.FirstOrDefault(expression);

            if (found == null)
            {
                throw new ArgumentNullException("Cash Point with provided criteria wasn't found");
            }

            var res = CoinFacade.CoinMachineService.Delete(new int[] { found.ID }, _dbParams);

            if (!res.IsSuccess)
            {
                throw new InvalidOperationException("Cash Point wasn't deleted");
            }

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
#endregion