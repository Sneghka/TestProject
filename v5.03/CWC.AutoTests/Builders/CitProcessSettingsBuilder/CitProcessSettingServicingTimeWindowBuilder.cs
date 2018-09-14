using Cwc.BaseData;
using Cwc.Common;
using Cwc.Contracts;
using Cwc.Transport;
using Cwc.Transport.Model;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class CitProcessSettingServicingTimeWindowBuilder
	{

		DataBaseParams _dbParams;
		CitProcessSettingServicingTimeWindow entity;

		public CitProcessSettingServicingTimeWindowBuilder()
		{
			 _dbParams = new DataBaseParams();
		}

		public CitProcessSettingServicingTimeWindowBuilder With_IsDefault(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsDefault = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CitProcessSettingServicingTimeWindowBuilder With_OrderType(OrderType? value)
		{
			if (entity != null) 
			{
				entity.OrderType = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CitProcessSettingServicingTimeWindowBuilder With_TimeFrom(Double value)
		{
			if (entity != null) 
			{
				entity.TimeFrom = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CitProcessSettingServicingTimeWindowBuilder With_TimeTo(Double value)
		{
			if (entity != null) 
			{
				entity.TimeTo = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CitProcessSettingServicingTimeWindowBuilder With_TimeSecondFrom(TimeSpan? value)
		{
			if (entity != null) 
			{
				entity.TimeSecondFrom = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CitProcessSettingServicingTimeWindowBuilder With_TimeSecondTo(TimeSpan? value)
		{
			if (entity != null) 
			{
				entity.TimeSecondTo = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CitProcessSettingServicingTimeWindowBuilder With_Weekday(WeekDays? value)
		{
			if (entity != null) 
			{
				entity.Weekday = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CitProcessSettingServicingTimeWindowBuilder With_ServiceTypeID(Int32? value)
		{
			if (entity != null) 
			{
				entity.ServiceTypeID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CitProcessSettingServicingTimeWindowBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public CitProcessSettingServicingTimeWindowBuilder New()
		{
			entity = new CitProcessSettingServicingTimeWindow();
						
			return this;
		}

		public static implicit operator CitProcessSettingServicingTimeWindow(CitProcessSettingServicingTimeWindowBuilder ins)
		{
			return ins.Build();
		}

		public CitProcessSettingServicingTimeWindow Build()
		{
			return entity;
		}

        public CitProcessSettingServicingTimeWindowBuilder SaveToDb()
        {

            var result = TransportFacade.CitProcessSettingServicingTimeWindowService.Save(entity);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Cit Processing Setting saving failed. Reason: {result.GetMessage()}");
            }
            return this;


        }

		public CitProcessSettingServicingTimeWindowBuilder Take(Expression<Func<CitProcessSettingServicingTimeWindow, bool>> expression)
        {
            using (var context = new AutomationTransportDataContext())
            {
                entity = context.CitProcessSettingServicingTimeWindows.FirstOrDefault(expression);

                if (entity == null)
                {
                    throw new ArgumentNullException("Cit Process Settings by provided criteria wasn't found");
                }

            }

            return this;
        }

        public void Delete(Expression<Func<CitProcessSettingServicingTimeWindow, bool>> expression)
        {
            using (var context = new AutomationTransportDataContext())
            {
                var citProcessSettings = context.CitProcessSettingServicingTimeWindows.FirstOrDefault(expression);

                if (citProcessSettings == null)
                {
                    throw new ArgumentNullException("Cit Process Settings by provided criteria wasn't found");
                }

                var result = TransportFacade.CitProcessSettingServicingTimeWindowService.Delete(citProcessSettings);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Cit Process Settings deletion failed. Reason: {result.GetMessage()}");
                }
            }
        }

        public void Dispose()
		{
	
		}
	}
}