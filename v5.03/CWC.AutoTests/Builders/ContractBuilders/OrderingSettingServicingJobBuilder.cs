using Cwc.Contracts;
using Cwc.Contracts.Model;
using System;

namespace CWC.AutoTests.ObjectBuilder
{
    public class OrderingSettingServicingJobBuilder
	{		
		OrderingSettingServicingJob entity;

		public OrderingSettingServicingJobBuilder()
		{			 
		}

        public OrderingSettingServicingJobBuilder With_IsNew(bool value)
        {
            if (entity != null)
            {
                entity.SetIsNew(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public OrderingSettingServicingJobBuilder With_ServiceCode(String value)
		{
			if (entity != null) 
			{
				entity.ServiceCode = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderingSettingServicingJobBuilder With_OrderingSettingsId(Int32 value)
		{
			if (entity != null) 
			{
				entity.OrderingSettingsId = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderingSettingServicingJobBuilder With_IsLatestRevision(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsLatestRevision = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderingSettingServicingJobBuilder With_RevisionNumber(Int32? value)
		{
			if (entity != null) 
			{
				entity.SetRevisionNumber(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderingSettingServicingJobBuilder With_RevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.RevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderingSettingServicingJobBuilder With_ReplacedRevisionNumber(Int32? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionNumber = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderingSettingServicingJobBuilder With_ReplacedRevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderingSettingServicingJobBuilder With_AuthorID(Int32? value)
		{
			if (entity != null) 
			{
				entity.AuthorID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderingSettingServicingJobBuilder With_LatestRevisionID(Int32? value)
		{
			if (entity != null) 
			{
				entity.LatestRevisionID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderingSettingServicingJobBuilder With_Contract(Contract value)
		{
			if (entity != null) 
			{
				entity.Contract = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public OrderingSettingServicingJobBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public OrderingSettingServicingJobBuilder New()
		{
			entity = new OrderingSettingServicingJob();				
			return this;
		}

		public static implicit operator OrderingSettingServicingJob(OrderingSettingServicingJobBuilder ins)
		{
			return ins.Build();
		}

		public OrderingSettingServicingJob Build()
		{
			return entity;
		}

		public OrderingSettingServicingJobBuilder SaveToDb()
		{
            var result = ContractsFacade.OrderingSettingServicingJobService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Ordering setting servicing job saving failed. Reason: { result.GetMessage() }");
            }

            return this;
        }        

        //public OrderingSettingServicingJobBuilder Take(Func<Cwc_Contracts_OrderingSettingServicingJob, bool> expression)
        //{
        //          var found = _context.Cwc_Contracts_OrderingSettingServicingJob.Where(expression).FirstOrDefault();

        //          if (found == null)
        //          {
        //              throw new InvalidOperationException("Contract product setting is not found!");
        //          }

        //          entity = ContractsFacade.OrderingSettingServicingJobService.Load(found.id, _dbParams);

        //          return this;
        //      }
    }
}