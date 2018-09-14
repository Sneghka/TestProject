using System;
using System.Linq;
using Cwc.CashCenter;
using System.Linq.Expressions;
using CWC.AutoTests.Model;

namespace CWC.AutoTests.ObjectBuilder
{
	public class StockLocationBuilder
	{		
		StockLocation entity;

		public StockLocationBuilder()
		{
		}

		public StockLocationBuilder With_Number(String value)
		{
			if (entity != null) 
			{
				entity.SetNumber(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_ReferenceNumber(String value)
		{
			if (entity != null) 
			{
				entity.ReferenceNumber = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_Description(String value)
		{
			if (entity != null) 
			{
				entity.Description = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_Status(StockLocationStatus value)
		{
			if (entity != null) 
			{
				entity.SetStatus(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_DateCreated(DateTime value)
		{
			if (entity != null) 
			{
				entity.SetDateCreated(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_DateUpdated(DateTime value)
		{
			if (entity != null) 
			{
				entity.SetDateUpdated(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_ParentStockLocation_id(Int32? value)
		{
			if (entity != null) 
			{
				entity.ParentStockLocation_id = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_StockOwner_id(Int32? value)
		{
			if (entity != null) 
			{
				entity.SetStockOwnerID(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_Site_id(Int32 value)
		{
			if (entity != null) 
			{
				entity.SetSiteID(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_Location_id(Decimal? value)
		{
			if (entity != null) 
			{
				entity.Location_id = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_Workstation_id(Int32? value)
		{
			if (entity != null) 
			{
				entity.Workstation_id = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_Author_id(Int32? value)
		{
			if (entity != null) 
			{
				entity.SetAuthor_id(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_Editor_id(Int32? value)
		{
			if (entity != null) 
			{
				entity.SetEditor_id(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_StockLocationTypeID(Int32 value)
		{
			if (entity != null) 
			{
				entity.SetStockLocationType(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_DefaultStockOwnerID(Int32? value)
		{
			if (entity != null) 
			{
				entity.SetDefaultStockOwnerID(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public StockLocationBuilder New()
		{
			entity = new StockLocation();						
			return this;
		}

		public static implicit operator StockLocation(StockLocationBuilder ins)
		{
			return ins.Build();
		}

		public StockLocation Build()
		{
			return entity;
		}

		public StockLocationBuilder SaveToDb()
		{
            var result = CashCenterFacade.StockLocationService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new Exception($"Error on saving stock location record: {result.GetMessage()}");
            }
            return this;
        }

		public StockLocationBuilder Take(Expression<Func<StockLocation, bool>> expression)
		{           
            using (var context = new AutomationCashCenterDataContext())
            {
                entity = context.StockLocations.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("Stock location with provided criteria was not found.");
                }
            }
            return this;
        }
	}
}