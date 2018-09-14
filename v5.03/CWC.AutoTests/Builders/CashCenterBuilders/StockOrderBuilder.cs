using Cwc.CashCenter;
using Cwc.Ordering;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class StockOrderBuilder
	{		
		StockOrder entity;

		public StockOrderBuilder With_Number(String value)
		{
			if (entity != null) 
			{
				entity.SetNumber(value);

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_ReferenceNumber(String value)
		{
			if (entity != null) 
			{
				entity.ReferenceNumber = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_ServiceDate(DateTime value)
		{
			if (entity != null) 
			{
				entity.ServiceDate = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_Status(StockOrderStatus value)
		{
			if (entity != null) 
			{
				entity.SetStatus(value);

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_TotalQuantity(Int32 value)
		{
			if (entity != null) 
			{
				entity.SetTotalQuantity(value);

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_TotalValue(Decimal value)
		{
			if (entity != null) 
			{
				entity.SetTotalValue(value);

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_TotalWeight(Decimal value)
		{
			if (entity != null) 
			{
				entity.SetTotalWeight(value);

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_DateCreated(DateTime value)
		{
			if (entity != null) 
			{
				entity.SetDateCreated(value);

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_DateUpdated(DateTime value)
		{
			if (entity != null) 
			{
				entity.SetDateUpdated(value);

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_ServiceType_id(Int32 value)
		{
			if (entity != null) 
			{
				entity.ServiceType_id = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_CITDepotID(Int32? value)
		{
			if (entity != null) 
			{
				entity.CITDepotID = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_Site_id(Int32 value)
		{
			if (entity != null) 
			{
				entity.Site_id = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_LocationFrom_id(Decimal? value)
		{
			if (entity != null) 
			{
				entity.LocationFrom_id = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_LocationTo_id(Decimal? value)
		{
			if (entity != null) 
			{
				entity.LocationTo_id = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_AuthorId(Int32? value)
		{
			if (entity != null) 
			{
				entity.AuthorId = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_EditorId(Int32? value)
		{
			if (entity != null) 
			{
				entity.EditorId = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_OrdersBatchId(Int64? value)
		{
			if (entity != null) 
			{
				entity.OrdersBatchId = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_MovementTypeID(Int32? value)
		{
			if (entity != null) 
			{
				entity.MovementTypeID = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_ServiceOrder_id(String value)
		{
			if (entity != null) 
			{
				entity.ServiceOrder_id = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_IsBilled(Boolean value)
		{
			if (entity != null) 
			{
				entity.SetIsBilled(value);

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_CustomerID(Decimal value)
		{
			if (entity != null) 
			{
				entity.CustomerID = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_StockLocationID(Int32? value)
		{
			if (entity != null) 
			{
				entity.StockLocationID = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockOrderBuilder With_ID(Int64 value)
		{
			if (entity != null) 
			{
				entity.ID = value;

				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public StockOrderBuilder New()
		{
		    entity = new StockOrder();
            entity.SetDateCreated(DateTime.Now);
            entity.SetDateUpdated(DateTime.Now);
			return this;
		}

		public static implicit operator StockOrder(StockOrderBuilder ins)
		{
			return ins.Build();
		}

		public StockOrder Build()
		{
			return entity;
		}

		public StockOrderBuilder SaveToDb()
		{
            var result = CashCenterFacade.StockOrderService.Save(entity, null);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Stock order saving failed. Reason: { result.GetMessage() }");
            }

            return this;
        }

		public StockOrderBuilder Take(Func<StockOrder, bool> expression)
		{
            using (var context = new AutomationCashCenterDataContext())
            {
                entity = context.StockOrders.FirstOrDefault(expression);

                if (entity == null)
                {
                    throw new InvalidOperationException("Contract product setting is not found!");
                }

            }

            return this;
        }
        
        public StockOrderBuilder InitDefault(Order orderFrom)
        {
            if (orderFrom == null)
            {
                throw new ArgumentNullException("Order from must be specified");
            }

            var orderLocation = DataFacade.Location.Take(x => x.Code == orderFrom.LocationCode).Build();
            var serviceType = DataFacade.ServiceType.Take(x => x.Code == orderFrom.ServiceTypeCode).Build();
            var site = orderLocation.NotesSiteID.HasValue ? orderLocation.NotesSiteID : DataFacade.Site.InitDefault().SaveToDb().Build().Branch_nr;

            this.New().With_ServiceDate(orderFrom.ServiceDate)
                .With_ServiceOrder_id(orderFrom.ID)
                .With_ServiceType_id(serviceType.ID)
                .With_LocationTo_id(orderLocation.ID)
                .With_Status(StockOrderStatus.Registered)
                .With_Site_id(site.Value)
                .With_CustomerID(orderLocation.CompanyID);

            return this;
        }		
	}
}