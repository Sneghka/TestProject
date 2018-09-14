using System;
using System.Linq;
using Cwc.BaseData;
using Cwc.CashCenter;
using System.Linq.Expressions;
using CWC.AutoTests.Model;

namespace CWC.AutoTests.ObjectBuilder
{
	public class StockOwnerBuilder
	{		
		StockOwner entity;

		public StockOwnerBuilder()
		{
		}

		public StockOwnerBuilder With_Code(string value)
		{
			if (entity != null) 
			{
				entity.Code = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOwnerBuilder With_ConsignmentTaker(bool value)
		{
			if (entity != null) 
			{
				entity.ConsignmentTaker = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOwnerBuilder With_MatchingAllowed(bool value)
		{
			if (entity != null) 
			{
				entity.MatchingAllowed = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOwnerBuilder With_MaximumRecycleStock(decimal? value)
		{
			if (entity != null) 
			{
				entity.MaximumRecycleStock = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOwnerBuilder With_MinimumRecycleStock(decimal? value)
		{
			if (entity != null) 
			{
				entity.MinimumRecycleStock = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOwnerBuilder With_DateCreated(DateTime value)
		{
			if (entity != null) 
			{
				entity.SetDateCreated(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOwnerBuilder With_DateUpdated(DateTime value)
		{
			if (entity != null) 
			{
				entity.SetDateUpdated(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOwnerBuilder With_Customer_id(decimal value)
		{
			if (entity != null) 
			{
				entity.Customer_id = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOwnerBuilder With_Author_id(int value)
		{
			if (entity != null) 
			{
				entity.SetAuthor_id(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOwnerBuilder With_Editor_id(int? value)
		{
			if (entity != null) 
			{
				entity.SetEditor_id(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOwnerBuilder With_Customer(Customer value)
		{
			if (entity != null) 
			{
				entity.Customer = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOwnerBuilder With_ID(int value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}
        	
		public StockOwnerBuilder New()
		{
			entity = new StockOwner();
			return this;
		}

		public static implicit operator StockOwner(StockOwnerBuilder ins)
		{
			return ins.Build();
		}

		public StockOwner Build()
		{
			return entity;
		}

		public StockOwnerBuilder SaveToDb()
		{
            var result = CashCenterFacade.StockOwnerService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new Exception($"Error on saving stock owner record: {result.GetMessage()}");
            }
			return this;
		}

		public StockOwnerBuilder Take(Expression<Func<StockOwner, bool>> expression)
		{            
            using (var context = new AutomationCashCenterDataContext())
            {
                entity = context.StockOwners.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("Stock owner with provided criteria was not found.");
                }
            }
            return this;
		}
	}
}