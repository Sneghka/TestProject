using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class StockPositionCashCenterBuilder
    {
        DataBaseParams _dbParams;
        StockPosition entity;

        public StockPositionCashCenterBuilder()
        {
            _dbParams = new DataBaseParams();
        }

        public StockPositionCashCenterBuilder With_QualificationType(QualificationType value)
        {
            if (entity != null)
            {
                entity.QualificationType = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_Quantity(Int32 value)
        {
            if (entity != null)
            {
                entity.Quantity = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_Value(Decimal value)
        {
            if (entity != null)
            {
                entity.Value = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_Weight(Decimal value)
        {
            if (entity != null)
            {
                entity.Weight = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_Material_id(String value)
        {
            if (entity != null)
            {
                entity.Material_id = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_Product_id(String value)
        {
            if (entity != null)
            {
                entity.Product_id = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_StockContainer_id(Int64? value)
        {
            if (entity != null)
            {
                entity.StockContainer_id = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_StockLocation_id(Int32? value)
        {
            if (entity != null)
            {
                entity.StockLocation_id = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_Currency_id(String value)
        {
            if (entity != null)
            {
                entity.Currency_id = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_StockOwner_id(Int32? value)
        {
            if (entity != null)
            {
                entity.StockOwner_id = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_AuthorId(Int32? value)
        {
            if (entity != null)
            {
                entity.AuthorId = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_EditorId(Int32? value)
        {
            if (entity != null)
            {
                entity.EditorId = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_PositionMaterialType(String value)
        {
            if (entity != null)
            {
                entity.PositionMaterialType = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockPositionCashCenterBuilder With_ID(Int64 value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }
        //public StockPositionCashCenterBuilder With_IsTotal(Boolean value)
        //{
        //    if (entity != null)
        //    {
        //        entity.IsTotal = value;
        //        return this;
        //    }
        //    throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        //}


        public StockPositionCashCenterBuilder New()
        {
            entity = new StockPosition();

            return this;
        }

        public static implicit operator StockPosition(StockPositionCashCenterBuilder ins)
        {
            return ins.Build();
        }

        public StockPosition Build()
        {
            return entity;
        }

        public StockPositionCashCenterBuilder SaveToDb()
        {
            var stockPosition = entity;

            var result = CashCenterFacade.StockPositionService.Save(stockPosition, new DataBaseParams());

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(string.Format("Stock Position saving failed. Reason: {0}", result.GetMessage()));
            }

            return this;
        }

        public StockPositionCashCenterBuilder Take(Expression<Func<StockPosition, bool>> expression)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                entity = context.StockPositions.FirstOrDefault(expression);

                if (entity == null)
                {
                    throw new ArgumentNullException("Cash Center Stock Position with provided criteria wasn't found");
                }

                return this;
            }
        }

        public void Delete(Expression<Func<StockPosition, bool>> expression)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                entity = context.StockPositions.FirstOrDefault(expression);

                if (entity == null)
                {
                    throw new ArgumentNullException("Cash Center Stock Position with provided criteria wasn't found");
                }
                var result = CashCenterFacade.StockPositionService.Delete(new long[] { entity.ID }, _dbParams);
                {
                    throw new InvalidOperationException("Cash Center Stock Position wasn't deleted");
                }
            }
        }
    }
}