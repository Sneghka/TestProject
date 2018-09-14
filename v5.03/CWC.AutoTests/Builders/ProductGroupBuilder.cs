using Cwc.BaseData;
using Cwc.BaseData.Model;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ProductGroupBuilder
	{		
		ProductGroup entity;

		public ProductGroupBuilder()
		{
		}

		public ProductGroupBuilder With_Code(String value)
		{
			if (entity != null) 
			{
				entity.Code = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ProductGroupBuilder With_Description(String value)
		{
			if (entity != null) 
			{
				entity.Description = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ProductGroupBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public ProductGroupBuilder New()
		{
			entity = new ProductGroup();				
			return this;
		}

		public static implicit operator ProductGroup(ProductGroupBuilder ins)
		{
			return ins.Build();
		}

		public ProductGroup Build()
		{
			return entity;
		}

		public ProductGroupBuilder SaveToDb()
		{
            var result = BaseDataFacade.ProductGroupService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Product group saving failed. Reason: { result.GetMessage() }");
            }

            return this;
        }

        public void Delete(Expression<Func<ProductGroup, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var productGroup = context.ProductGroups.First(expression);
                if (productGroup == null)
                {
                    throw new ArgumentNullException("Product group is not found!");
                }
               
                var result = BaseDataFacade.ProductGroupService.Delete(productGroup.ID, null);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Product Group deletion failed. Reason: {result.GetMessage()}");
                }
            }

        }

        public ProductGroupBuilder Take(Func<ProductGroup, bool> expression)
		{
            using (var context = new AutomationBaseDataContext())
            {
                var productGroup = context.ProductGroups.FirstOrDefault(expression);

                if (productGroup == null)
                {
                    throw new ArgumentNullException("productGroup", "Product group is not found!");
                }

                entity = productGroup;
            }

            return this;
        }
	}
}