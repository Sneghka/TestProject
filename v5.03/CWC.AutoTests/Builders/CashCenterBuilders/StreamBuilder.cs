using Cwc.BaseData;
using Cwc.CashCenter;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class StreamBuilder
	{		
		Stream entity;

		public StreamBuilder()
		{			 
		}

		public StreamBuilder With_Name(string value)
		{
			if (entity != null) 
			{
				entity.Name = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamBuilder With_Description(string value)
		{
			if (entity != null) 
			{
				entity.Description = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamBuilder With_StockOwner_id(int value)
		{
			if (entity != null) 
			{
				entity.StockOwner_id = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamBuilder With_Site_id(int value)
		{
			if (entity != null) 
			{
				entity.SetSiteID(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamBuilder With_Process(StreamProcess value)
		{
			if (entity != null) 
			{
				entity.SetProcess(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamBuilder With_DestinationLocationID(decimal? value)
		{
			if (entity != null) 
			{
				entity.DestinationLocationID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamBuilder With_DestinationStockLocationID(int? value)
		{
			if (entity != null) 
			{
				entity.DestinationStockLocationID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamBuilder With_DateCreated(DateTime value)
		{
			if (entity != null) 
			{
				entity.SetDateCreated(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamBuilder With_DateUpdated(DateTime value)
		{
			if (entity != null) 
			{
				entity.SetDateUpdated(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamBuilder With_AuthorID(int? value)
		{
			if (entity != null) 
			{
				entity.SetAuthorID(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamBuilder With_EditorID(int? value)
		{
			if (entity != null) 
			{
				entity.SetEditorID(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamBuilder With_ID(int value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public StreamBuilder New()
		{
			entity = new Stream();						
			return this;
		}

		public static implicit operator Stream(StreamBuilder ins)
		{
			return ins.Build();
		}

		public Stream Build()
		{
			return entity;
		}

		public StreamBuilder SaveToDb()
		{
            var result = CashCenterFacade.StreamService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new Exception($"Error on saving stream record: {result.GetMessage()}");
            }
            return this;
        }

		public StreamBuilder Take(Expression<Func<Stream, bool>> expression)
		{
            using (var context = new AutomationCashCenterDataContext())
            {                
                var entity = context.Streams.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("Stream with provided criteria wasn't found");
                }                
            }

            return this;
        }
	}
}