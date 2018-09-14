using Cwc.BaseData;
using Cwc.BaseData.Model;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class CashPointTypeBuilder : IDisposable
    {
        AutomationCoinDataContext _context;
        DataBaseParams _dbParams;
        CashPointType entity;

        public CashPointTypeBuilder()
        {
            _dbParams = new DataBaseParams();
            _context = new AutomationCoinDataContext();
        }

        public CashPointTypeBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashPointTypeBuilder With_Number(Int32 value)
        {
            if (entity != null)
            {
                entity.Number = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashPointTypeBuilder With_Name(String value)
        {
            if (entity != null)
            {
                entity.Name = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashPointTypeBuilder With_Description(String value)
        {
            if (entity != null)
            {
                entity.Description = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashPointTypeBuilder With_IsCollect(Boolean value)
        {
            if (entity != null)
            {
                entity.IsCollect = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashPointTypeBuilder With_IsIssue(Boolean value)
        {
            if (entity != null)
            {
                entity.IsIssue = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashPointTypeBuilder With_IsRecycle(Boolean value)
        {
            if (entity != null)
            {
                entity.IsRecycle = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashPointTypeBuilder With_HandlingType(String value)
        {
            if (entity != null)
            {
                entity.HandlingType = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashPointTypeBuilder With_UseInOptimization(Boolean value)
        {
            if (entity != null)
            {
                entity.IsUseInOptimization = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashPointTypeBuilder New()
        {
            entity = new CashPointType();

            return this;
        }

        public static implicit operator CashPointType(CashPointTypeBuilder ins)
        {
            return ins.Build();
        }

        public CashPointType Build()
        {

            return entity;
        }

        public CashPointTypeBuilder SaveToDb()
        {
            var result = BaseDataFacade.CashPointTypeService.Save(entity);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(string.Format("Cash Point Type saving failed. Reason: {0}", result.GetMessage()));
            }

            return this;
        }

        public CashPointTypeBuilder Take(Expression<Func<CashPointType, bool>> expression)
        {
            entity = _context.CashPointTypes.FirstOrDefault(expression);

            if (entity == null)
            {
                throw new ArgumentNullException("Location type with provided criteria wasn't found");
            }

            return this;
        }

        public void Delete(Expression<Func<CashPointType, bool>> expression)
        {
            using (var context = new AutomationCoinDataContext())
            {
                var cashPointType = context.CashPointTypes.Where(expression).Select(x => x.ID).First();

                var result = BaseDataFacade.CashPointTypeService.Delete(cashPointType);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"CashPointType deletion failed. Reason: {result.GetMessage()}");
                }
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
