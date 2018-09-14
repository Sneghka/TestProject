using System;
using Cwc.BaseData;
using Cwc.Common;
using System.Linq.Expressions;
using CWC.AutoTests.Model;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
	public class ExchangeRateBuilder
	{
		DataBaseParams _dbParams;
		ExchangeRate entity;

		public ExchangeRateBuilder()
		{
			 _dbParams = new DataBaseParams();
		}

		public ExchangeRateBuilder With_Rate(Decimal value)
		{
			if (entity != null) 
			{
				entity.Rate = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ExchangeRateBuilder With_ExchangeDate(DateTime value)
		{
			if (entity != null) 
			{
				entity.ExchangeDate = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ExchangeRateBuilder With_CurrencyFromID(String value)
		{
			if (entity != null) 
			{
				entity.CurrencyFromID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ExchangeRateBuilder With_CurrencyToID(String value)
		{
			if (entity != null) 
			{
				entity.CurrencyToID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ExchangeRateBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public ExchangeRateBuilder New()
		{
			entity = new ExchangeRate();
						
			return this;
		}

		public static implicit operator ExchangeRate(ExchangeRateBuilder ins)
		{
			return ins.Build();
		}

		public ExchangeRate Build()
		{
			return entity;
		}

		public ExchangeRateBuilder SaveToDb()
		{
            var result = BaseDataFacade.ExchangeRateService.Save(entity,null);
            if (!result.IsSuccess)
            {
                throw new NullReferenceException($"ExchangeRate was not saved. Reason: {result.GetMessage()}");
            }
			return this;
		}

		public ExchangeRateBuilder Take(Expression<Func<ExchangeRate, bool>> expression)
		{
            using (var context = new AutomationBaseDataContext())
            {
                var result = context.ExchangeRates.FirstOrDefault(expression);
                if(result == null)
                {
                    throw new NullReferenceException("Exchange Rate is not found");
                }
            }
            return this;
		}

        public void Delete(Expression<Func<ExchangeRate, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var exchangeRates = context.ExchangeRates.First(expression);

                if (exchangeRates == null)
                {
                    throw new ArgumentNullException("ExchangeRate with given criteria wasn't found");
                }

                var deleteResult = BaseDataFacade.ExchangeRateService.Delete(new int[] { exchangeRates.ID }, new DataBaseParams());

                if (!deleteResult.IsSuccess)
                {
                    throw new InvalidOperationException($"ExchangeRate wasn't deleted. Reason: {deleteResult.GetMessage()}");
                }
            }
        }

	}
}