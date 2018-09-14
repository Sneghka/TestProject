using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ServiceTypeBuilder
    {        
        ServiceType entity;

        public ServiceTypeBuilder()
        {
        }

        public ServiceTypeBuilder With_Code(String value)
        {
            if (entity != null)
            {
                entity.Code = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ServiceTypeBuilder With_Name(String value)
        {
            if (entity != null)
            {
                entity.Name = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ServiceTypeBuilder With_OldType(String value)
        {
            if (entity != null)
            {
                entity.OldType = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ServiceTypeBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public ServiceTypeBuilder New()
        {
            entity = new ServiceType();
            return this;
        }

        public static implicit operator ServiceType(ServiceTypeBuilder ins)
        {
            return ins.Build();
        }

        public ServiceType Build()
        {
            return entity;
        }

        public ServiceTypeBuilder SaveToDb(DataBaseParams dbParams)
        {
            var temp = entity;
            var res = BaseDataFacade.ServiceTypeService.Save(temp, null);
            if (!res.IsSuccess)
            {
                throw new InvalidOperationException($"ServiceType saving failed. Reason: {res.GetMessage()}");
            }

            return this;
        }

        public ServiceTypeBuilder Take(Func<ServiceType, bool> expression, bool asNoTracking = false)
        {
            using (var context = new AutomationBaseDataContext())
            {
                entity = asNoTracking ? context.ServiceTypes.AsNoTracking().FirstOrDefault(expression) : context.ServiceTypes.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("ServiceType wasn't found by provided criteria");
                }               
            }

            return this;
        }

        public void DeleteMany(Expression<Func<ServiceType, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var serviceTypes = context.ServiceTypes.Where(expression).Select(x => x.ID).ToArray();

                if (serviceTypes.Length == 0)
                {
                    throw new ArgumentNullException("Service Types with provided criteria weren't found");
                }

                var result = BaseDataFacade.ServiceTypeService.Delete(serviceTypes, null);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Service Types deletion failed. Reason: {result.GetMessage()}");
                }
            }

        }

        public void DeleteOne(Expression<Func<ServiceType, bool>> expression)
        {
            using (var context = new BaseDataContext())
            {
                var serviceType = context.ServiceTypes.First(expression);

                if (serviceType == null)
                {
                    throw new ArgumentNullException("Service Types with provided criteria wasn't found");
                }

                var result = BaseDataFacade.ServiceTypeService.Delete(new int[] {serviceType.ID}, null);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"ServiceType deletion failed. Reason: {result.GetMessage()}");
                }
            }

        }
    }
}