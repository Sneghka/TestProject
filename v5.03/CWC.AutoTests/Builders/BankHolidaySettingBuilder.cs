using Cwc.BaseData;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class BankHolidaySettingBuilder
	{		
		BankHolidaySetting entity;

		public BankHolidaySettingBuilder()
		{			
		}

		public BankHolidaySettingBuilder With_IsDefault(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsDefault = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public BankHolidaySettingBuilder With_Location(Decimal? value)
		{
			if (entity != null) 
			{
				entity.Location = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public BankHolidaySettingBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public BankHolidaySettingBuilder New()
		{
			entity = new BankHolidaySetting();				
			return this;
		}

		public static implicit operator BankHolidaySetting(BankHolidaySettingBuilder ins)
		{
			return ins.Build();
		}

		public BankHolidaySetting Build()
		{
			return entity;
		}

		public BankHolidaySettingBuilder SaveToDb()
		{
            var result = BaseDataFacade.BankHolidaySettingService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Bank holiday setting saving failed. Reason: { result.GetMessage() }");
            }

            return this;
        }

        public BankHolidaySettingBuilder Take(Expression<Func<BankHolidaySetting, bool>> expression, AutomationBaseDataContext modelContext = null)
        {
            using (var newContext = new AutomationBaseDataContext())
            {
                var context = modelContext ?? newContext;
                var found = context.BankHolidaySettings.FirstOrDefault(expression);
                if (found == null)
                {
                    throw new InvalidOperationException("Bank holiday setting is not found!");
                }
                entity = BaseDataFacade.BankHolidaySettingService.Load(found.ID, null);
            }

            return this;
        }        
	}
}