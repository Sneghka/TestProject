using System;
using System.Linq;
using Cwc.BaseData;
using Cwc.Common;
using Cwc.Contracts;
using CWC.AutoTests.DataModel;
using Cwc.Contracts.Model;

namespace CWC.AutoTests.ObjectBuilder
{
	public class PriceLineUnitsRangeBuilder
	{		
		PriceLineUnitsRange entity;

		public PriceLineUnitsRangeBuilder()
		{			
		}

		public PriceLineUnitsRangeBuilder With_UnitsFrom(Decimal value)
		{
			if (entity != null) 
			{
				entity.UnitsFrom = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineUnitsRangeBuilder With_UnitsTo(Decimal value)
		{
			if (entity != null) 
			{
				entity.UnitsTo = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineUnitsRangeBuilder With_Price(Decimal value)
		{
			if (entity != null) 
			{
				entity.Price = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineUnitsRangeBuilder With_PriceLineID(Int32 value)
		{
			if (entity != null) 
			{
				entity.SetPriceLineID(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineUnitsRangeBuilder With_AuthorID(Int32? value)
		{
			if (entity != null) 
			{
				entity.AuthorID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineUnitsRangeBuilder With_LatestRevisionID(Int32? value)
		{
			if (entity != null) 
			{
				entity.LatestRevisionID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineUnitsRangeBuilder With_IsLatestRevision(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsLatestRevision = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineUnitsRangeBuilder With_RevisionNumber(Int32? value)
		{
			if (entity != null) 
			{
				entity.SetRevisionNumber(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineUnitsRangeBuilder With_RevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.RevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineUnitsRangeBuilder With_ReplacedRevisionNumber(Int32? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionNumber = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineUnitsRangeBuilder With_ReplacedRevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineUnitsRangeBuilder With_Contract(Contract value)
		{
			if (entity != null) 
			{
				entity.Contract = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PriceLineUnitsRangeBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public PriceLineUnitsRangeBuilder New()
		{
			entity = new PriceLineUnitsRange();				
			return this;
		}

		public static implicit operator PriceLineUnitsRange(PriceLineUnitsRangeBuilder ins)
		{
			return ins.Build();
		}

		public PriceLineUnitsRange Build()
		{
			return entity;
		}
	}
}