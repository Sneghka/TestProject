using Cwc.Contracts;
using Cwc.Contracts.Enums;
using Cwc.Contracts.Model;
using System;

namespace CWC.AutoTests.ObjectBuilder
{
    public class PriceLineLevelBuilder
    {
        PriceLineLevel entity;

        public PriceLineLevelBuilder()
        {            
        }

        public PriceLineLevelBuilder With_LevelCaption(string value)
        {
            if (entity != null)
            {
                entity.LevelCaption = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_LevelName(PriceRuleLevelName value)
        {
            if (entity != null)
            {
                entity.SetPriceLevelName(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_IsRangeLevel(Boolean value)
        {
            if (entity != null)
            {
                entity.SetIsRangeLevel(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_LevelValueType(PriceRuleLevelValueType value)
        {
            if (entity != null)
            {
                entity.SetLevelValueType(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_Value(decimal? value)
        {
            if (entity != null)
            {
                entity.SetValue(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_ValueFrom(decimal? value)
        {
            if (entity != null)
            {
                entity.ValueFrom = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_ValueTo(decimal? value)
        {
            if (entity != null)
            {
                entity.ValueTo = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_SequenceNumber(Int32 value)
        {
            if (entity != null)
            {
                entity.SequenceNumber = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_PriceLineID(Int32 value)
        {
            if (entity != null)
            {
                entity.SetPriceLineID(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_AuthorID(Int32? value)
        {
            if (entity != null)
            {
                entity.AuthorID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_LatestRevisionID(Int32? value)
        {
            if (entity != null)
            {
                entity.LatestRevisionID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_IsLatestRevision(Boolean value)
        {
            if (entity != null)
            {
                entity.IsLatestRevision = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_RevisionNumber(Int32? value)
        {
            if (entity != null)
            {
                entity.SetRevisionNumber(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_RevisionDate(DateTime? value)
        {
            if (entity != null)
            {
                entity.RevisionDate = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_ReplacedRevisionNumber(Int32? value)
        {
            if (entity != null)
            {
                entity.ReplacedRevisionNumber = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_ReplacedRevisionDate(DateTime? value)
        {
            if (entity != null)
            {
                entity.ReplacedRevisionDate = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_Contract(Contract value)
        {
            if (entity != null)
            {
                entity.Contract = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public PriceLineLevelBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public PriceLineLevelBuilder New()
        {
            entity = new PriceLineLevel();
            return this;
        }

        public static implicit operator PriceLineLevel(PriceLineLevelBuilder ins)
        {
            return ins.Build();
        }

        public PriceLineLevel Build()
        {
            return entity;
        }        
    }
}