using Cwc.BaseData;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class MaterialBuilder
    {        
        Cwc.BaseData.Material entity;

        public MaterialBuilder()
        {            
        }

        public MaterialBuilder With_WP_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }
            
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MaterialBuilder With_MaterialID(String value)
        {
            if (entity != null)
            {
                entity.MaterialID = value;
                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MaterialBuilder With_MaterialNumber(Int32? value)
        {
            if (entity != null)
            {
                entity.MaterialNumber = value;
                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MaterialBuilder With_Description(String value)
        {
            if (entity != null)
            {
                entity.Description = value;
                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MaterialBuilder With_Type(String value)
        {
            if (entity != null)
            {
                entity.Type = value;
                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MaterialBuilder With_Currency(String value)
        {
            if (entity != null)
            {
                entity.Currency = value;
                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MaterialBuilder With_Denomination(Decimal value)
        {
            if (entity != null)
            {
                entity.Denomination = value;
                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MaterialBuilder With_Weight(Decimal value)
        {
            if (entity != null)
            {
                entity.Weight = value;
                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MaterialBuilder With_MaximumValue(Int32? value)
        {
            if (entity != null)
            {
                entity.MaximumValue = value;
                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MaterialBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public MaterialBuilder New()
        {
            entity = new Cwc.BaseData.Material();
            return this;
        }

        public static implicit operator Cwc.BaseData.Material(MaterialBuilder ins)
        {
            return ins.Build();
        }

        public Cwc.BaseData.Material Build()
        {
            return entity;
        }

        public MaterialBuilder SaveToDb()
        {
            var res = BaseDataFacade.MaterialService.Save(entity, null);
            if (!res.IsSuccess)
            {
                throw new InvalidOperationException($"Material saving failed. Reason: {res.GetMessage()}");
            }

            return this;
        }

        public MaterialBuilder Take(Expression<Func<Cwc.BaseData.Material, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {               
                entity = context.Materials.FirstOrDefault(expression);

                if (entity == null)
                {
                    throw new ArgumentNullException("Material with given criteria wasn't found");
                }

                return this;
            }
        }

        public void DeleteMany(Expression<Func<Cwc.BaseData.Material, bool>> expression)
        { 
            using (var context = new AutomationBaseDataContext())
            {
                var expMaterials = context.Materials.Where(expression).Select(x => x.ID).ToArray();

                if (expMaterials.Length == 0)
                {
                    throw new ArgumentNullException("Materials with given criteria weren't found");
                }

                var deleteResult = BaseDataFacade.MaterialService.Delete(expMaterials, new DataBaseParams());

                if (!deleteResult.IsSuccess)
                {
                    throw new InvalidOperationException($"Materials weren't deleted. Reason: {deleteResult.GetMessage()}");
                }
            }            
        }


        public void DeleteOne(Expression<Func<Cwc.BaseData.Material, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var expMaterials = context.Materials.First(expression);

                if (expMaterials == null)
                {
                    throw new ArgumentNullException("Material with given criteria wasn't found");
                }

                var deleteResult = BaseDataFacade.MaterialService.Delete(new int[] {expMaterials.ID}, new DataBaseParams());

                if (!deleteResult.IsSuccess)
                {
                    throw new InvalidOperationException($"Material wasn't deleted. Reason: {deleteResult.GetMessage()}");
                }
            }
        }
    }
}