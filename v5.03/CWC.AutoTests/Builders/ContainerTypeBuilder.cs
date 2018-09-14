using Cwc.BaseData;
using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ContainerTypeBuilder
	{
        BagType entity;
        List<BagTypeContentSetting> bagTypeContentList;
        List<BagTypeMaterialTypeLink> bagTypeMaterialTypeLinkList;

		public ContainerTypeBuilder With_Number(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContainerTypeBuilder With_Code(String value)
		{
			if (entity != null) 
			{
				entity.SetCode(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContainerTypeBuilder With_Description(String value)
		{
			if (entity != null) 
			{
				entity.Description = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContainerTypeBuilder With_MaxValue(Decimal value)
		{
			if (entity != null) 
			{
				entity.MaxValue = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContainerTypeBuilder With_RegularValue(Decimal value)
		{
			if (entity != null) 
			{
				entity.RegularValue = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContainerTypeBuilder With_MaxWeight(Decimal value)
		{
			if (entity != null) 
			{
				entity.MaxWeight = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContainerTypeBuilder With_RegularWeight(Decimal value)
		{
			if (entity != null) 
			{
				entity.RegularWeight = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContainerTypeBuilder With_MaxProductQuantity(Int32 value)
		{
			if (entity != null) 
			{
				entity.MaxProductQuantity = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContainerTypeBuilder With_IsAtmCassette(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsAtmCassette = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContainerTypeBuilder With_IsAllocationBySize(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsAllocationBySize = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContainerTypeBuilder With_BarcdIdent(String value)
		{
			if (entity != null) 
			{
				entity.BarcdIdent = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContainerTypeBuilder With_Repack(Boolean? value)
		{
			if (entity != null) 
			{
				entity.Repack = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}        

        public ContainerTypeBuilder New()
		{
			entity = new BagType();				
			return this;
		}

		public static implicit operator BagType(ContainerTypeBuilder ins)
		{
			return ins.Build();
		}

		public BagType Build()
		{
			return entity;
		}

        public ContainerTypeBuilder SaveToDb()
        {
            var result = BaseDataFacade.BagTypeService.Save(entity);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Container type saving failed. Reason: { result.GetMessage() }");
            }
            return this;
        }

        public ContainerTypeBuilder SaveToDb(List<BagTypeContentSetting> contentSettingsList, List<BagTypeMaterialTypeLink> materialTypeLinksList)
        {
            var result = BaseDataFacade.BagTypeService.Save(entity, contentSettingsList, materialTypeLinksList, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Container type saving failed. Reason: { result.GetMessage() }");
            }
            return this;
        }

        public ContainerTypeBuilder Take(Expression<Func<BagType, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                entity = context.BagTypes.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("Bag Type with provided criteria wasn't found");
                }
            }
            return this;
        }

        public void DeleteMany(Expression<Func<BagType, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var bagTypes = context.BagTypes.Where(expression).Select(x => x.ID).ToArray();

                if (bagTypes.Length == 0)
                {
                    throw new ArgumentNullException("Bag Types with provided criteria weren't found");
                }

                var result = BaseDataFacade.BagTypeService.DeleteBagTypes(bagTypes, null);

                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Bag Types weren't deleted. Reson: {result.GetMessage()}");
                }
            }
        }

        public void DeleteOne(Expression<Func<BagType, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var bagType = context.BagTypes.FirstOrDefault(expression);

                if (bagType == null)
                {
                    throw new ArgumentNullException("Bag Type with provided criteria wasn't found");
                }

                var result = BaseDataFacade.BagTypeService.Delete(bagType.ID);

                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Bag Type wasn't deleted. Reson: {result.GetMessage()}");
                }
            }
        }

    }
}