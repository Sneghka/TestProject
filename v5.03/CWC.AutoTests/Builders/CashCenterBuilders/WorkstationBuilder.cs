using Cwc.BaseData;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class WorkstationBuilder
	{		
		Workstation entity;

		public WorkstationBuilder()
		{
		}

		public WorkstationBuilder With_Name(String value)
		{
			if (entity != null) 
			{
				entity.Name = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public WorkstationBuilder With_Description(String value)
		{
			if (entity != null) 
			{
				entity.Description = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public WorkstationBuilder With_CameraID(Int32? value)
		{
			if (entity != null) 
			{
				entity.CameraID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public WorkstationBuilder With_DateCreated(DateTime value)
		{
			if (entity != null) 
			{
				entity.DateCreated = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public WorkstationBuilder With_DateUpdated(DateTime value)
		{
			if (entity != null) 
			{
				entity.DateUpdated = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public WorkstationBuilder With_AuthorID(Int32 value)
		{
			if (entity != null) 
			{
				entity.AuthorID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public WorkstationBuilder With_EditorID(Int32 value)
		{
			if (entity != null) 
			{
				entity.EditorID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public WorkstationBuilder With_Printer(String value)
		{
			if (entity != null) 
			{
				entity.Printer = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public WorkstationBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public WorkstationBuilder New()
		{
			entity = new Workstation();						
			return this;
		}

		public static implicit operator Workstation(WorkstationBuilder ins)
		{
			return ins.Build();
		}

		public Workstation Build()
		{
			return entity;
		}

		public WorkstationBuilder SaveToDb()
		{
            var result = BaseDataFacade.WorkstationService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new Exception($"Error on saving workstation record: {result.GetMessage()}");
            }
            return this;
        }

		public WorkstationBuilder Take(Expression<Func<Workstation, bool>> expression)
		{
            using (var context = new AutomationCashCenterDataContext())
            {
                var entity = context.Workstations.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("Workstation with provided criteria was not found.");
                }
            }

            return this;
        }
	}
}