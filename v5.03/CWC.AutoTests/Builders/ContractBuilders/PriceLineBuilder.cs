using Cwc.Contracts;
using Cwc.Contracts.Enums;
using Cwc.Contracts.Model;
using Cwc.Security;
using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class PriceLineBuilder
	{		
		PriceLine entity;

		public PriceLineBuilder()
		{			 
		}
        public PriceLineBuilder With_IsNew(bool value)
        {
            if (entity != null)
            {
                entity.SetIsNew(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineBuilder With_PriceRule(PriceRule value)
		{
			if (entity != null) 
			{
				entity.PriceRule = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_UnitOfMeasure(UnitOfMeasure value)
		{
			if (entity != null) 
			{
				entity.UnitOfMeasure = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}       

        public PriceLineBuilder With_ReferenceCode(String value)
		{
			if (entity != null) 
			{
				entity.ReferenceCode = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_Description(String value)
		{
			if (entity != null) 
			{
				entity.Description = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_Units(Decimal value)
		{
			if (entity != null) 
			{
				entity.Units = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_Price(Decimal value)
		{
			if (entity != null) 
			{
				entity.Price = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_MinBillableUnits(Decimal? value)
		{
			if (entity != null) 
			{
				entity.MinBillableUnits = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_MaxBillableUnits(Decimal? value)
		{
			if (entity != null) 
			{
				entity.MaxBillableUnits = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_IsApplySurcharges(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsApplySurcharges = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_IsDecreasePriceProportionally(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsDecreasePriceProportionally = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_IsRangePriceBasedOnTotal(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsRangePriceBasedOnTotal = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_DebtorID(Int32? value)
		{
			if (entity != null) 
			{
				entity.DebtorID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_BankAccountID(Int32? value)
		{
			if (entity != null) 
			{
				entity.BankAccountID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_AuthorID(Int32? value)
		{
			if (entity != null) 
			{
				entity.AuthorID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_ContractID(Int32 value)
		{
			if (entity != null) 
			{
				entity.SetContractID(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_LatestRevisionID(Int32? value)
		{
			if (entity != null) 
			{
				entity.LatestRevisionID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_IsLatestRevision(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsLatestRevision = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_RevisionNumber(Int32? value)
		{
			if (entity != null) 
			{
				entity.SetRevisionNumber(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_RevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.RevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_ReplacedRevisionNumber(Int32? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionNumber = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_ReplacedRevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_Contract(Contract value)
		{
			if (entity != null) 
			{
				entity.Contract = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

        public PriceLineBuilder With_PriceLineLevels(List<PriceLineLevel> value)
        {
            if (entity != null)
            {
                entity.PriceLineLevels = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineBuilder With_PriceLineUnitsRanges(List<PriceLineUnitsRange> value)
        {
            if (entity != null)
            {
                entity.PriceLineUnitsRanges = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineBuilder New()
		{
			entity = new PriceLine();				
			return this;
		}

		public static implicit operator PriceLine(PriceLineBuilder ins)
		{
			return ins.Build();
		}

		public PriceLine Build()
		{
			return entity;
		}

		public PriceLineBuilder SaveToDb()
		{
            var result = ContractsFacade.PriceLineService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Price line saving failed. Reason: { result.GetMessage() }");
            }

            return this;
		}

		public PriceLineBuilder Take(Expression<Func<PriceLine, bool>> expression, AutomationContractDataContext dataContext = null)
		{
            using (var newContext = new AutomationContractDataContext())
            {
                var context = dataContext ?? newContext;
                entity = context.PriceLines.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new InvalidOperationException("Price line is not found!");
                }                
            }

			return this;
		}
    }
}