using Cwc.CashCenter;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class StockLocationSettingBuilder
	{		
		StockLocationSetting entity;

		public StockLocationSettingBuilder()
		{
		}

		public StockLocationSettingBuilder With_IsConsignmentStock(bool value)
		{
			if (entity != null) 
			{
				entity.IsConsignmentStock = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsAssigningToRepack(bool value)
		{
			if (entity != null) 
			{
				entity.IsAssigningToRepack = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsCreatingRepack(bool value)
		{
			if (entity != null) 
			{
				entity.IsCreatingRepack = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsRebaggingAllowed(bool value)
		{
			if (entity != null) 
			{
				entity.IsRebaggingAllowed = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsRepackRebaggingAllowed(bool value)
		{
			if (entity != null) 
			{
				entity.IsRepackRebaggingAllowed = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsImplicitRetrieval(bool value)
		{
			if (entity != null) 
			{
				entity.IsImplicitRetrieval = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsWorkstationAllowed(bool value)
		{
			if (entity != null) 
			{
				entity.IsWorkstationAllowed = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsStockOwnerAllowed(bool value)
		{
			if (entity != null) 
			{
				entity.IsStockOwnerAllowed = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsVerified(bool value)
		{
			if (entity != null) 
			{
				entity.IsVerified = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsFits(bool value)
		{
			if (entity != null) 
			{
				entity.IsFits = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsUnfits(bool value)
		{
			if (entity != null) 
			{
				entity.IsUnfits = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsRejects(bool value)
		{
			if (entity != null) 
			{
				entity.IsRejects = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsCounterfeits(bool value)
		{
			if (entity != null) 
			{
				entity.IsCounterfeits = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsAfits(bool value)
		{
			if (entity != null) 
			{
				entity.IsAfits = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsStained(bool value)
		{
			if (entity != null) 
			{
				entity.IsStained = value;
				return this;
			}				
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsDetonated(bool value)
		{
			if (entity != null) 
			{
				entity.IsDetonated = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_DateCreated(DateTime value)
		{
			if (entity != null) 
			{
				entity.SetDateCreated(value);
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_DateUpdated(DateTime value)
		{
			if (entity != null) 
			{
				entity.SetDateUpdated(value);
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_MaxValue(decimal value)
		{
			if (entity != null) 
			{
				entity.MaxValue = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsBlockingMaxValue(bool value)
		{
			if (entity != null) 
			{
				entity.IsBlockingMaxValue = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_StockLocationID(int? value)
		{
			if (entity != null) 
			{
				entity.StockLocationID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_StockLocationTypeID(int? value)
		{
			if (entity != null) 
			{
				entity.StockLocationTypeID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_AuthorID(int value)
		{
			if (entity != null) 
			{
				entity.SetAuthorID(value);
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_EditorID(int? value)
		{
			if (entity != null) 
			{
				entity.SetEditorID(value);
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsAllowedSkipCountUnsealing(bool value)
		{
			if (entity != null) 
			{
				entity.IsAllowedSkipCountUnsealing = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsOpsAreaStock(bool value)
		{
			if (entity != null) 
			{
				entity.IsOpsAreaStock = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsIndirectProductionAllowed(bool value)
		{
			if (entity != null) 
			{
				entity.IsIndirectProductionAllowed = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsPostponedUnsealingAllowed(bool value)
		{
			if (entity != null) 
			{
				entity.IsPostponedUnsealingAllowed = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_IsNotifySecurityDepartmentAboutInventoryDiscrepancy(bool value)
		{
			if (entity != null) 
			{
				entity.IsNotifySecurityDepartmentAboutInventoryDiscrepancy = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_InventoryCheckServiceTypeID(int? value)
		{
			if (entity != null) 
			{
				entity.InventoryCheckServiceTypeID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public StockLocationSettingBuilder With_ID(int value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public StockLocationSettingBuilder New()
		{
			entity = new StockLocationSetting();						
			return this;
		}

		public static implicit operator StockLocationSetting(StockLocationSettingBuilder ins)
		{
			return ins.Build();
		}

		public StockLocationSetting Build()
		{
			return entity;
		}

		public StockLocationSettingBuilder SaveToDb()
		{
            var result = CashCenterFacade.StockLocationSettingService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new Exception($"Error on saving stock location setting record: {result.GetMessage()}");
            }
            return this;
        }

		public StockLocationSettingBuilder Take(Expression<Func<StockLocationSetting, bool>> expression)
		{
            using (var context = new AutomationCashCenterDataContext())
            {
                entity = context.StockLocationSettings.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("Stock location setting with provided criteria was not found.");
                }
            }
            return this;
        }
	}
}