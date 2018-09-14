using Cwc.CashCenter;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
	public class StockLocationTypeBuilder
	{		
		StockLocationType entity;

		public StockLocationTypeBuilder()
		{
		}

		public StockLocationTypeBuilder With_Name(string value)
		{
			if (entity != null) 
			{
				entity.Name = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationTypeBuilder With_Level(StockLocationTypeLevel value)
		{
			if (entity != null) 
			{
				entity.SetLevel(value);
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationTypeBuilder With_Description(string value)
		{
			if (entity != null) 
			{
				entity.Description = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationTypeBuilder With_DateCreated(DateTime value)
		{
			if (entity != null) 
			{
				entity.SetDateCreated(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationTypeBuilder With_DateUpdated(DateTime value)
		{
			if (entity != null) 
			{
				entity.SetDateUpdated(value);
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationTypeBuilder With_SiteID(int value)
		{
			if (entity != null) 
			{
				entity.SetSiteID(value);
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationTypeBuilder With_AuthorID(int value)
		{
			if (entity != null) 
			{
				entity.SetAuthorID(value);
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationTypeBuilder With_EditorID(int? value)
		{
			if (entity != null) 
			{
				entity.SetEditorID(value);
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationTypeBuilder With_ID(int value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public StockLocationTypeBuilder New()
		{
			entity = new StockLocationType();						
			return this;
		}

		public static implicit operator StockLocationType(StockLocationTypeBuilder ins)
		{
			return ins.Build();
		}

		public StockLocationType Build()
		{
			return entity;
		}

		public StockLocationTypeBuilder SaveToDb()
		{
            var result = CashCenterFacade.StockLocationTypeService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new Exception($"Error on saving stock location type record: {result.GetMessage()}");
            }
            return this;
        }

		public StockLocationTypeBuilder Take(Expression<Func<StockLocationType, bool>> expression)
		{
            using (var context = new AutomationCashCenterDataContext())
            {
                entity = context.StockLocationTypes.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("Stock location type with provided criteria was not found.");
                }
            }
            return this;
        }
	}
}