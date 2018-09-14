using Cwc.BaseData.Model;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ServicingCodeBuilder
	{		
		ServicingCode entity;

		public ServicingCodeBuilder()
		{			 
		}

		public ServicingCodeBuilder With_Code(String value)
		{
			if (entity != null) 
			{
				entity.Code = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ServicingCodeBuilder With_Description(String value)
		{
			if (entity != null) 
			{
				entity.Description = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ServicingCodeBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public ServicingCodeBuilder New()
		{
			entity = new ServicingCode();				
			return this;
		}

		public static implicit operator ServicingCode(ServicingCodeBuilder ins)
		{
			return ins.Build();
		}

		public ServicingCode Build()
		{
			return entity;
		}

		public ServicingCodeBuilder SaveToDb()
		{
            using (var context = new AutomationBaseDataContext())
            {
                context.ServicingCodes.Add(entity);
                context.SaveChanges();
            }

            return this;
        }

		public ServicingCodeBuilder Take(Func<ServicingCode, bool> expression)
		{
            using (var context = new AutomationBaseDataContext())
            {

                var servicingCode = context.ServicingCodes.FirstOrDefault(expression);

                if (servicingCode == null)
                {
                    throw new ArgumentNullException("servicingCode", "Servicing code is not found!");
                }

                entity = servicingCode;
            }

            return this;
        }        
	}
}