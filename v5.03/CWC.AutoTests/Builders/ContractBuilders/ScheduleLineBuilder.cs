using Cwc.Contracts;
using Cwc.Security;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ScheduleLineBuilder
	{		
		ScheduleLine entity;

		public ScheduleLineBuilder()
		{			
		}

        public ScheduleLineBuilder With_IsNew(bool value)
        {
            if (entity != null)
            {
                entity.SetIsNew(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ScheduleLineBuilder With_WeekdayName(WeekdayName? value)
		{
			if (entity != null) 
			{
				entity.WeekdayName = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_WeekdayNumberInMonth(WeekdayNumberInMonth? value)
		{
			if (entity != null) 
			{
				entity.WeekdayNumberInMonth = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_OrderType(OrderType value)
		{
			if (entity != null) 
			{
				entity.OrderType = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_PeriodNumber(Int32 value)
		{
			if (entity != null) 
			{
				entity.PeriodNumber = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_PeriodUnits(PeriodUnits value)
		{
			if (entity != null) 
			{
				entity.PeriodUnits = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_SurchargePercentage(Int32 value)
		{
			if (entity != null) 
			{
				entity.SurchargePercentage = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_SurchargeValue(Decimal value)
		{
			if (entity != null) 
			{
				entity.SurchargeValue = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_CancellationPercentage(Int32 value)
		{
			if (entity != null) 
			{
				entity.CancellationPercentage = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_CancellationValue(Decimal value)
		{
			if (entity != null) 
			{
				entity.CancellationValue = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}
        
		public ScheduleLineBuilder With_ScheduleSetting_id(Int32 value)
		{
			if (entity != null) 
			{
				entity.SetScheduleSettingID(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_MasterRoute_id(Int32? value)
		{
			if (entity != null) 
			{
				entity.MasterRoute_id = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_IsLatestRevision(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsLatestRevision = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_RevisionNumber(Int32? value)
		{
			if (entity != null) 
			{
				entity.SetRevisionNumber(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_RevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.RevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_ReplacedRevisionNumber(Int32? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionNumber = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_ReplacedRevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_AuthorID(Int32? value)
		{
			if (entity != null) 
			{
				entity.AuthorID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_LatestRevisionID(Int32? value)
		{
			if (entity != null) 
			{
				entity.LatestRevisionID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_Contract(Contract value)
		{
			if (entity != null) 
			{
				entity.Contract = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ScheduleLineBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public ScheduleLineBuilder New()
		{
			entity = new ScheduleLine();
            entity.InitDefault(SecurityFacade.LoginService.GetAdministratorLogin());

            return this;
		}

		public static implicit operator ScheduleLine(ScheduleLineBuilder ins)
		{
			return ins.Build();
		}

		public ScheduleLine Build()
		{
			return entity;
		}

		public ScheduleLineBuilder SaveToDb(bool isClearingRequired = false)
		{
            if (entity.Contract == null)
            {
                throw new NullReferenceException("Contract property is not defined in schedule line instance.");
            }

            if (isClearingRequired)
            {
                HelperFacade.ContractHelper.ClearScheduleLines(entity.ScheduleSetting_id);
            }
            
            var result = ContractsFacade.ScheduleLineService.RevisionControlOnUpdate(entity, entity.Contract.ID, null, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Contract product setting saving failed. Reason: { result.GetMessage() }");
            }

            return this;
        }

		public ScheduleLineBuilder Take(Expression<Func<ScheduleLine, bool>> expression, AutomationContractDataContext modelContext = null)
		{
            using (var newContext = new AutomationContractDataContext())
            {
                var context = modelContext != null ? modelContext : newContext;
                var found = context.ScheduleLines.Where(expression).FirstOrDefault();
                if (found == null)
                {
                    throw new InvalidOperationException("Contract product setting is not found!");
                }

                entity = ContractsFacade.ScheduleLineService.Load(found.ID, null);
            }

            return this;
        }
    }
}