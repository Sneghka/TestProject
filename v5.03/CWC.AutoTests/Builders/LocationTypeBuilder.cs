using Cwc.BaseData;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class LocationTypeBuilder : IDisposable
    {
        AutomationBaseDataContext _context;
        DataBaseParams _dbParams;
        Cwc.BaseData.LocationType entity;

        public LocationTypeBuilder()
        {
            _dbParams = new DataBaseParams();
            _context = new AutomationBaseDataContext();
        }

        public LocationTypeBuilder With_ltCode(String value)
        {
            if (entity != null)
            {
                entity.ltCode = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public LocationTypeBuilder With_ltDesc(String value)
        {
            if (entity != null)
            {
                entity.ltDesc = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public LocationTypeBuilder With_Category(LocationTypeCategory? value)
        {
            if (entity != null)
            {
                entity.Category = value.Value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public LocationTypeBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public LocationTypeBuilder New()
        {
            entity = new Cwc.BaseData.LocationType();

            return this;
        }

        public static implicit operator Cwc.BaseData.LocationType(LocationTypeBuilder ins)
        {
            return ins.Build();
        }

        public Cwc.BaseData.LocationType Build()
        {

            return entity;
        }

        public LocationTypeBuilder SaveToDb()
        {
            var result = BaseDataFacade.LocationTypeService.Save(entity, _dbParams);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(string.Format("LocationType saving failed. Reason: {0}", result.GetMessage()));
            }

            return this;
        }

        public LocationTypeBuilder Take(Expression<Func<Cwc.BaseData.LocationType, bool>> expression)
        {
            entity = _context.LocationTypes.FirstOrDefault(expression);

            if (entity == null)
            {
                throw new ArgumentNullException("Location type with provided criteria wasn't found");
            }

            return this;
        }

        public void DeleteMany(Expression<Func<Cwc.BaseData.LocationType, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var locationTypes = context.LocationTypes.Where(expression).Select(x => x.ID).ToArray();

                if (locationTypes.Length == 0)
                {
                    throw new ArgumentNullException("Location types with provided criteria weren't found");
                }

                var result = BaseDataFacade.LocationTypeService.Delete(locationTypes, _dbParams);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Location Types deletion failed. Reason: {result.GetMessage()}");
                }
            }
        }

        public void DeleteOne(Expression<Func<Cwc.BaseData.LocationType, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var locationType = context.LocationTypes.FirstOrDefault(expression);

                if (locationType == null)
                {
                    throw new ArgumentNullException("Location type with provided criteria wasn't found");
                }

                var result = BaseDataFacade.LocationTypeService.Delete(new int[] { locationType.ID }, _dbParams);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Location Type deletion failed. Reason: {result.GetMessage()}");
                }
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}