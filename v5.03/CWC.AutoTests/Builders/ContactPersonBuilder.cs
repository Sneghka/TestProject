using Cwc.BaseData;
using Cwc.BaseData.Model;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ContactPersonBuilder
	{		
		DataBaseParams _dbParams;
		ContactPerson entity;

		public ContactPersonBuilder()
		{
			 			 
		}

		public ContactPersonBuilder With_FirstName(String value)
		{
			if (entity != null) 
			{
				entity.FirstName = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContactPersonBuilder With_MiddleName(String value)
		{
			if (entity != null) 
			{
				entity.MiddleName = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContactPersonBuilder With_LastName(String value)
		{
			if (entity != null) 
			{
				entity.LastName = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContactPersonBuilder With_Email(String value)
		{
			if (entity != null) 
			{
				entity.Email = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContactPersonBuilder With_PhoneNumber(String value)
		{
			if (entity != null) 
			{
				entity.PhoneNumber = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContactPersonBuilder With_FaxNumber(String value)
		{
			if (entity != null) 
			{
				entity.FaxNumber = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContactPersonBuilder With_IsPreferred(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsPreferred = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContactPersonBuilder With_Type(ContactPersonType? value)
		{
			if (entity != null) 
			{
				entity.Type = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContactPersonBuilder With_JobTitle(String value)
		{
			if (entity != null) 
			{
				entity.JobTitle = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContactPersonBuilder With_OtherPhone(String value)
		{
			if (entity != null) 
			{
				entity.OtherPhone = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContactPersonBuilder With_Address(BaseAddress value)
		{
			if (entity != null) 
			{
				entity.Address = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContactPersonBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public ContactPersonBuilder New()
		{
			entity = new ContactPerson();
						
			return this;
		}

		public static implicit operator ContactPerson(ContactPersonBuilder ins)
		{
			return ins.Build();
		}

		public ContactPerson Build()
		{
			return entity;
		}

		public ContactPersonBuilder SaveToDb()
		{
            var result = BaseDataFacade.ContactPersonService.Save(entity,_dbParams);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(string.Format("ContactPerson saving failed.Reason: {0}",result.GetMessage()));
            }

			return this;
		}

		public ContactPersonBuilder Take(Expression<Func<ContactPerson, bool>> expression)
		{
            using (var context = new AutomationBaseDataContext())
            {
                entity = context.ContactPersons.Where(expression).FirstOrDefault();
                if (entity == null)
                {
                    throw new ArgumentNullException("ContactPerson wasn't found by provided criteria");
                }
            }
            return this;
        }

        public void Delete(Expression<Func<ContactPerson, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var contactPerson = context.ContactPersons.Where(expression).Select(x => x.ID).First();
                
                var result = BaseDataFacade.ContactPersonService.Delete(contactPerson);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"ContactPerson deletion failed. Reason: {result.GetMessage()}");
                }
            }
        }
    }
}