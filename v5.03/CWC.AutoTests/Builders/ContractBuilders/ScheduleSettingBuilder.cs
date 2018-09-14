using Cwc.Contracts;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ScheduleSettingBuilder
	{		
		ScheduleSetting entity;

		public ScheduleSettingBuilder()
		{			
		}

        public ScheduleSettingBuilder With_IsNew(bool value)
        {
            if (entity != null)
            {
                entity.SetIsNew(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ScheduleSettingBuilder With_Location_id(Decimal? value)
		{
			if (entity != null) 
			{
				entity.Location_id = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_ServiceTypeID(Int32? value)
		{
			if (entity != null) 
			{
				entity.ServiceTypeID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_Contract_id(Int32 value)
		{
			if (entity != null) 
			{
				entity.SetContractID(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_ScheduleLines(List<ScheduleLine> value)
		{
			if (entity != null) 
			{
				entity.ScheduleLines = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_PeriodStartDate(DateTime value)
		{
			if (entity != null) 
			{
				entity.PeriodStartDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_IsLatestRevision(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsLatestRevision = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_RevisionNumber(Int32? value)
		{
			if (entity != null) 
			{
				entity.SetRevisionNumber(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_RevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.RevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_ReplacedRevisionNumber(Int32? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionNumber = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_ReplacedRevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_AuthorID(Int32? value)
		{
			if (entity != null) 
			{
				entity.AuthorID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_LatestRevisionID(Int32? value)
		{
			if (entity != null) 
			{
				entity.LatestRevisionID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_Contract(Contract value)
		{
			if (entity != null) 
			{
				entity.Contract = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleSettingBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public ScheduleSettingBuilder New()
		{
			entity = new ScheduleSetting();				
			return this;
		}

		public static implicit operator ScheduleSetting(ScheduleSettingBuilder ins)
		{
			return ins.Build();
		}

		public ScheduleSetting Build()
		{
			return entity;
		}

		public ScheduleSettingBuilder SaveToDb(bool isClearingRequired = false)
		{
            if (isClearingRequired)
            {
                HelperFacade.ContractHelper.ClearScheduleSettings(entity.Contract_id);
            }
            
            var result = ContractsFacade.ScheduleSettingService.RevisionControlOnUpdate(entity, entity.Contract_id, null, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Contract product setting saving failed. Reason: { result.GetMessage() }");
            }

            if (entity.ScheduleLines.Any())
            {
                foreach (var line in entity.ScheduleLines)
                {
                    var newLine = DataFacade.ScheduleLine.New()                   
                        .With_ScheduleSetting_id(entity.ID)
                        .With_WeekdayName(line.WeekdayName)
                        .With_Contract(line.Contract)                                                
                        .With_IsLatestRevision(true)                                               
                        .With_OrderType(OrderType.AtRequest)
                        .With_PeriodNumber(1)
                        .With_PeriodUnits(PeriodUnits.Weeks)
                        .With_SurchargePercentage(0)
                        .With_SurchargeValue(0)
                        .With_CancellationPercentage(0)
                        .With_CancellationValue(0)
                        .SaveToDb();
                }
            }

            return this;
        }

		public ScheduleSettingBuilder Take(Expression<Func<ScheduleSetting, bool>> expression, AutomationContractDataContext modelContext = null)
		{
            using (var newContext = new AutomationContractDataContext())
            {
                var context = modelContext ?? newContext;
                var found = context.ScheduleSettings.FirstOrDefault(expression);
                if (found == null)
                {
                    throw new InvalidOperationException("Contract product setting is not found!");
                }

                entity = ContractsFacade.ScheduleSettingService.Load(found.ID, null);
            }

            return this;
        }        
    }
}