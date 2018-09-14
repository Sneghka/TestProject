using Cwc.CashCenter;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class StreamLocationLinkBuilder
	{		
		StreamLocationLink entity;

		public StreamLocationLinkBuilder()
		{
		}

		public StreamLocationLinkBuilder With_StreamID(int value)
		{
			if (entity != null) 
			{
				entity.StreamID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamLocationLinkBuilder With_Location_id(decimal value)
		{
			if (entity != null) 
			{
				entity.Location_id = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StreamLocationLinkBuilder With_ID(long value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public StreamLocationLinkBuilder New()
		{
			entity = new StreamLocationLink();						
			return this;
		}

		public static implicit operator StreamLocationLink(StreamLocationLinkBuilder ins)
		{
			return ins.Build();
		}

		public StreamLocationLink Build()
		{
			return entity;
		}

		public StreamLocationLinkBuilder SaveToDb()
		{
            var result = CashCenterFacade.StreamLocationLinkService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new Exception($"Error on saving stream location link record: {result.GetMessage()}");
            }
            return this;
        }

		public StreamLocationLinkBuilder Take(Expression<Func<StreamLocationLink, bool>> expression)
		{
            using (var context = new AutomationCashCenterDataContext())
            {
                var entity = context.StreamLocationLinks.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("Stream location link with provided criteria was not found.");
                }
            }

            return this;
        }
	}
}