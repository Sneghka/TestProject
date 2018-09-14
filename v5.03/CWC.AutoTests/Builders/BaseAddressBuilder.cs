using Cwc.BaseData;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class BaseAddressBuilder : IDisposable
    {
        AutomationBaseDataContext _context;
        DataBaseParams _dbParams;
        BaseAddress entity;

        public BaseAddressBuilder()
        {
            _dbParams = new DataBaseParams();
            _context = new AutomationBaseDataContext();
        }

        public BaseAddressBuilder With_Street(String value)
        {
            if (entity != null)
            {
                entity.Street = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BaseAddressBuilder With_PostalCode(String value)
        {
            if (entity != null)
            {
                entity.PostalCode = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BaseAddressBuilder With_City(String value)
        {
            if (entity != null)
            {
                entity.City = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BaseAddressBuilder With_State(String value)
        {
            if (entity != null)
            {
                entity.State = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BaseAddressBuilder With_Country(String value)
        {
            if (entity != null)
            {
                entity.Country = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BaseAddressBuilder With_ExtraAddressInfo(String value)
        {
            if (entity != null)
            {
                entity.ExtraAddressInfo = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BaseAddressBuilder With_Purpose(Purpose? value)
        {
            if (entity != null)
            {
                entity.Purpose = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BaseAddressBuilder With_ObjectID(Decimal value)
        {
            if (entity != null)
            {
                entity.ObjectID = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BaseAddressBuilder With_ObjectClassID(String value)
        {
            if (entity != null)
            {
                entity.ObjectClassID = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public BaseAddressBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public BaseAddressBuilder New()
        {
            entity = new BaseAddress();

            return this;
        }

        public static implicit operator BaseAddress(BaseAddressBuilder ins)
        {
            return ins.Build();
        }

        public BaseAddress Build()
        {
            return entity;
        }

        public BaseAddressBuilder SaveToDb(DataBaseParams dbParams)
        {
            var temp = entity;

            var res = BaseDataFacade.AddressService.Save(temp, _dbParams);

            if (!res.IsSuccess)
            {
                throw new InvalidOperationException($"Addres wasn't saved. Reason: {res.GetMessage()}");
            }

            return this;
        }

        public BaseAddressBuilder Take(Expression<Func<BaseAddress, bool>> expression)
        {
            var found = _context.BaseAddresses.Where(expression).First();

            if (found == null)
            {
                throw new ArgumentNullException("Address with provided criteria wasn't found");
            }

            entity = BaseDataFacade.AddressService.Load(found.ID, _dbParams);

            return this;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}