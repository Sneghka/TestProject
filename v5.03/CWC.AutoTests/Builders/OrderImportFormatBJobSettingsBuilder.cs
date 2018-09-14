using Cwc.Integration.OrderImportFormatB;
using Cwc.Integration.OrderImportFormatB.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class OrderImportFormatBJobSettingsBuilder
	{
		OrderImportFormatBJobSettings entity;

		public OrderImportFormatBJobSettingsBuilder()
		{

		}

		public OrderImportFormatBJobSettingsBuilder With_PostcodesIncomingFileFolder(String value)
		{
			if (entity != null) 
			{
				entity.PostcodesIncomingFileFolder = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_IncomingFileFolder(String value)
		{
			if (entity != null) 
			{
				entity.IncomingFileFolder = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_IncomingFilePrefix(String value)
		{
			if (entity != null) 
			{
				entity.IncomingFilePrefix = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_LastFileSequenceNumber(Int32 value)
		{
			if (entity != null) 
			{
				entity.LastFileSequenceNumber = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_IsCreateVisitAddress(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsCreateVisitAddress = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_VisitAddressPrefix(String value)
		{
			if (entity != null) 
			{
				entity.VisitAddressPrefix = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_VisitAddressSuffixMask(String value)
		{
			if (entity != null) 
			{
				entity.VisitAddressSuffixMask = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_VisitAddressLastSuffix(Int32 value)
		{
			if (entity != null) 
			{
				entity.VisitAddressLastSuffix = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_ServicePointPrefix(String value)
		{
			if (entity != null) 
			{
				entity.ServicePointPrefix = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_ServicePointSuffixMask(String value)
		{
			if (entity != null) 
			{
				entity.ServicePointSuffixMask = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_ServicePointLastSuffix(Int32 value)
		{
			if (entity != null) 
			{
				entity.ServicePointLastSuffix = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_OutgoingFileFolder(String value)
		{
			if (entity != null) 
			{
				entity.OutgoingFileFolder = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_OutgoingFilePrefix(String value)
		{
			if (entity != null) 
			{
				entity.OutgoingFilePrefix = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_CompanyID(Int32 value)
		{
			if (entity != null) 
			{
				entity.CompanyID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_OrderingDepartmentID(Int32 value)
		{
			if (entity != null) 
			{
				entity.OrderingDepartmentID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_ServiceTypeID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ServiceTypeID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_LocationTypeID(Int32 value)
		{
			if (entity != null) 
			{
				entity.LocationTypeID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_HandlingTypeID(String value)
		{
			if (entity != null) 
			{
				entity.HandlingTypeID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_CashPointTypeID(Int32? value)
		{
			if (entity != null) 
			{
				entity.CashPointTypeID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderImportFormatBJobSettingsBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public OrderImportFormatBJobSettingsBuilder New()
		{
			entity = new OrderImportFormatBJobSettings();
						
			return this;
		}

		public static implicit operator OrderImportFormatBJobSettings(OrderImportFormatBJobSettingsBuilder ins)
		{
			return ins.Build();
		}

		public OrderImportFormatBJobSettings Build()
		{
			return entity;
		}

		public OrderImportFormatBJobSettingsBuilder SaveToDb()
		{
            var result = OrderImportFormatBFacade.OrderImportFormatBJobSettingsService.Save(entity);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Order Import Format B Job Settings saving failed. Reason: { result.GetMessage() }");
            }
            return this;
        }

        public OrderImportFormatBJobSettingsBuilder Take(Expression<Func<OrderImportFormatBJobSettings, bool>> expression)
		{
            using (var context = new OrderImportFormatBDataContext())
            {
                entity = context.OrderImportFormatBJobSettings.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("Bag Type with provided criteria wasn't found");
                }
            }
            return this;
        }

		public void Dispose()
		{
		}
	}
}