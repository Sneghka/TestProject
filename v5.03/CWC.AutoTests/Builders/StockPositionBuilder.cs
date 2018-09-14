using Cwc.BaseData.Classes;
using Cwc.BaseData.Model;
using Cwc.Coin;
using Cwc.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{    
    using Cwc.Coin.ConstantNames;
    using CWC.AutoTests.Model;

    public class StockPositionBuilder : IDisposable
    {
        AutomationCoinDataContext _context;
        DataBaseParams _dbParams;
        CashPointStockPosition entity;
        static List<CashPointStockPosition> _stockPositions;

        static StockPositionBuilder()
        {
            _stockPositions = new List<CashPointStockPosition>();
        }
        public StockPositionBuilder()
        {
            ConfigurationKeySet.Load();
            _dbParams = new DataBaseParams();
            _context = new AutomationCoinDataContext();
        }

        #region DataInit
        public StockPositionBuilder WithMachineType(CashPointType value)
        {
            entity.MachineType = value.ID;

            return this;
        }

        public StockPositionBuilder WithMachineModel(MachineModel value)
        {
            entity.MachineModelId = value.ID;

            return this;
        }

        public StockPositionBuilder WithType(Enums.StockPosition.Type value)
        {
            entity.Type = value;

            return this;
        }

        public StockPositionBuilder WithMaterial(String value)
        {
            entity.MaterialId = value;

            return this;
        }

        public StockPositionBuilder WithMaterialID(Cwc.BaseData.Material value)
        {
            entity.MaterialId = Convert.ToString(value.ID);

            return this;
        }

        public StockPositionBuilder WithProduct(String value)
        {
            entity.ProductCode = value;

            return this;
        }
        public StockPositionBuilder WithQuantity(Int32 value)
        {
            entity.Quantity = value;

            return this;
        }

        public StockPositionBuilder WithValue(Decimal value)
        {
            entity.Value = value;

            return this;
        }
        
        public StockPositionBuilder WithWeight(Decimal value)
        {
            entity.Weight = value;

            return this;
        }

        public StockPositionBuilder WithDateCreated(DateTime value)
        {
            entity.DateCreated = value;
            //using (var context = new ModelContext())
            //{
            //    var change = context.WP_StockPosition.FirstOrDefault(s => s.MachineType == entity.MachineType);
            //    change.DateCreated = value;
            //    context.SaveChanges();
            //}

            return this;
        }

        public StockPositionBuilder WithDateUpdated(DateTime value)
        {
            entity.DateUpdated = value;
            //using (var context = new ModelContext())
            //{
            //    var change = context.WP_StockPosition.FirstOrDefault(s => s.MachineType == entity.MachineType);
            //    change.DateUpdated = value;
            //    context.SaveChanges();
            //}

            return this;
        }

        public StockPositionBuilder WithCoinMachineID(Int32 value)
        {
            entity.CoinMachineId = value;

            return this;
        }

        public StockPositionBuilder WithCoinMachine(CoinMachine value)
        {
            entity.CoinMachineId = value.ID;

            return this;
        }

        public StockPositionBuilder WithIndicator(Enums.StockPosition.Indicator value)
        {
            entity.Indicator = value;

            return this;
        }

        public StockPositionBuilder WithDirection(Direction value)
        {
            entity.Direction = value;

            return this;
        }

        public StockPositionBuilder WithCollectGreenValue(Decimal value)
        {
            entity.CollectGreenValue = value;

            return this;
        }

        public StockPositionBuilder WithCollectOrangeValue(Decimal value)
        {
            entity.CollectOrangeValue = value;

            return this;
        }

        public StockPositionBuilder WithIssueGreenValue(Decimal value)
        {
            entity.IssueGreenValue = value;

            return this;
        }

        public StockPositionBuilder WithIssueOrangeValue(Decimal value)
        {
            entity.IssueOrangeValue = value;

            return this;
        }

        public StockPositionBuilder WithTotals(Enums.StockPosition.Totals value)
        {
            entity.Totals = value;

            return this;
        }

        public StockPositionBuilder WithCapacity(Int32 value)
        {
            entity.Capacity = value;

            return this;
        }

        public StockPositionBuilder WithCassetteNumber(Int32 value)
        {
            entity.CassetteNumber = value;

            return this;
        }

        public StockPositionBuilder WithRefillProduct(String value)
        {
            entity.RefillProduct = value;

            return this;
        }

        public StockPositionBuilder WithPriority(Priority value)
        {
            entity.Priority = value;

            return this;
        }

        public StockPositionBuilder WithIsGrandTotal(Boolean value)
        {
            entity.IsGrandTotal = value;

            return this;
        }

        public StockPositionBuilder WithCurrency(String value)
        {
            entity.CurrencyID = value;

            return this;
        }

        public StockPositionBuilder WithResidualCashPercentage(Int32 value)
        {
            entity.ResidualCashPercentage = value;

            return this;
        }

        public StockPositionBuilder WithCassetteExternalNumber(String value)
        {
            entity.CassetteExternalNumber = value;

            return this;
        }

        public StockPositionBuilder WithIsMixed(Boolean value)
        {
            entity.IsMixed = value;

            return this;
        }

        public StockPositionBuilder WithIsNew(bool value)
        {
            entity.SetIsNew(value);

            return this;
        }

        public StockPositionBuilder WithParentMixedStockPositionID(Int32 value)
        {
            entity.ParentMixedStockPositionID = value;

            return this;
        }

        public StockPositionBuilder WithMaterialType(String value)
        {
            entity.MaterialTypeCode = value;

            return this;
        }

        public StockPositionBuilder WithIsOptimized(Boolean value)
        {
            entity.IsOptimized = value;

            return this;
        }

        public StockPositionBuilder WithCounterfeits(Boolean value)
        {
            entity.IsCounterfeits = value;

            return this;
        }

        public StockPositionBuilder New()
        {
            entity = new CashPointStockPosition();

            return this;
        }

        public static implicit operator CashPointStockPosition(StockPositionBuilder ins)
        {
            return ins.Build();
        }

        public CashPointStockPosition Build()
        {
            return entity;
        }

        public StockPositionBuilder SaveToDb()
        {
            var stockPosition = entity;

            var result = CoinFacade.StockPositionService.Save(stockPosition, new DataBaseParams());

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(string.Format("Stock Position saving failed. Reason: {0}", result.GetMessage()));
            }

            return this;

        }

        public StockPositionBuilder Take(Func<CashPointStockPosition, bool> expression)
        {
            entity = _context.StockPositions.FirstOrDefault(expression);

            if (entity == null)
            {
                throw new ArgumentNullException("Stock Position with provided criteria wasn't found");
            }

            return this;
        }

        public void Delete(Func<CashPointStockPosition, bool> expression)
        {
            entity = _context.StockPositions.FirstOrDefault(expression);

            if (entity == null)
            {
                throw new ArgumentNullException("Stock Position with provided criteria wasn't found");
            }

            var res = CoinFacade.StockPositionService.Delete(new int[] { entity.ID }, _dbParams);

            if (!res.IsSuccess)
            {
                throw new InvalidOperationException("Stock Position wasn't deleted");
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
#endregion