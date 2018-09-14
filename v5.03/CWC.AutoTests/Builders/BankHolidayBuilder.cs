using Cwc.BaseData;
using Cwc.Localization;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class BankHolidayBuilder
	{		
		BankHoliday entity;

		public BankHolidayBuilder()
		{			 
		}

        public BankHolidayBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BankHolidayBuilder With_Date(DateTime value)
        {
            if (entity != null)
            {
                entity.Date = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BankHolidayBuilder With_Description(String value)
		{
			if (entity != null) 
			{
				entity.Description = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

        public BankHolidayBuilder With_BankHolidaySettingID(int value)
        {
            if (entity != null)
            {
                entity.BankingHolidaySettingId = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BankHolidayBuilder New()
		{
			entity = new BankHoliday();				
			return this;
		}

		public static implicit operator BankHoliday(BankHolidayBuilder ins)
		{
			return ins.Build();
		}

		public BankHoliday Build()
		{
			return entity;
		}

		public BankHolidayBuilder SaveToDb()
		{
            var result = BaseDataFacade.BankHolidayService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Bank holiday saving failed. Reason: { result.GetMessage() }");
            }

            return this;
        }

		public BankHolidayBuilder Take(Expression<Func<BankHoliday, bool>> expression, AutomationBaseDataContext modelContext = null)
		{
            using (var newContext = new AutomationBaseDataContext())
            {
                var context = modelContext ?? newContext;
                var found = context.BankHolidays.FirstOrDefault(expression);
                if (found == null)
                {
                    throw new InvalidOperationException("Bank holiday is not found!");
                }
                entity = BaseDataFacade.BankHolidayService.Load(found.ID, null);
            }

            return this;
        }

        public void SaveBankHoliday(Location location, DateTime date, AutomationBaseDataContext modelContext = null)
        {
            using (var newContext = new AutomationBaseDataContext())
            {
                var context = modelContext ?? newContext;
                BankHolidaySetting bankHolidaySetting;
                if (context.BankHolidaySettings.Any(s => s.Location == location.ID))
                {
                    bankHolidaySetting = DataFacade.BankHolidaySetting.Take(s => s.Location == location.ID).Build();
                    this.New()
                            .With_Date(date)
                            .With_Description("Test bank holiday")
                            .With_BankHolidaySettingID(bankHolidaySetting.ID)
                            .SaveToDb();
                }
                else
                {
                    bankHolidaySetting = DataFacade.BankHolidaySetting.New()
                                                                            .With_IsDefault(false)
                                                                            .With_Location(location.ID)
                                                                            .SaveToDb()
                                                                            .Build();
                    this.New()
                            .With_Date(date)
                            .With_Description("Test bank holiday")
                            .With_BankHolidaySettingID(bankHolidaySetting.ID)
                            .SaveToDb();
                }
            }
        }
    }
}