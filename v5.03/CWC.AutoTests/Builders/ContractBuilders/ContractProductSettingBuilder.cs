using Cwc.Contracts;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ContractProductSettingBuilder
	{		
		ContractProductSetting entity;

		public ContractProductSettingBuilder()
		{			 
		}

        public ContractProductSettingBuilder With_IsNew(bool value)
        {
            if (entity != null)
            {
                entity.SetIsNew(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractProductSettingBuilder With_MaxQuantity(Int32? value)
		{
			if (entity != null) 
			{
				entity.MaxQuantity = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_PreDefinedQuantity(Int32? value)
		{
			if (entity != null) 
			{
				entity.PreDefinedQuantity = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_Product_code(String value)
		{
			if (entity != null) 
			{
				entity.Product_code = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_ContractOrderingSettings_id(Int32 value)
		{
			if (entity != null) 
			{
				entity.SetContractOrderingSettings_id(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_IsFixedContent(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsFixedContent = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_IsLatestRevision(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsLatestRevision = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_RevisionNumber(Int32? value)
		{
			if (entity != null) 
			{
				entity.SetRevisionNumber(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_RevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.RevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_ReplacedRevisionNumber(Int32? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionNumber = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_ReplacedRevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_AuthorID(Int32? value)
		{
			if (entity != null) 
			{
				entity.AuthorID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_LatestRevisionID(Int32? value)
		{
			if (entity != null) 
			{
				entity.LatestRevisionID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_Contract(Contract value)
		{
			if (entity != null) 
			{
				entity.Contract = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractProductSettingBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public ContractProductSettingBuilder New()
		{
			entity = new ContractProductSetting();				
			return this;
		}

		public static implicit operator ContractProductSetting(ContractProductSettingBuilder ins)
		{
			return ins.Build();
		}

		public ContractProductSetting Build()
		{
			return entity;
		}

		public ContractProductSettingBuilder SaveToDb()
		{
            var result = ContractsFacade.ContractProductSettingsService.Save(entity, null);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Contract product setting saving failed. Reason: { result.GetMessage() }");
            }
			
			return this;
		}

		public ContractProductSettingBuilder Take(Func<ContractProductSetting, bool> expression, AutomationContractDataContext modelContext = null)
		{
            using (var newContext = new AutomationContractDataContext())
            {
                var context = modelContext ?? newContext;
                var found = context.ContractProductSettings.FirstOrDefault(expression);
                if (found == null)
                {
                    throw new InvalidOperationException("Contract product setting is not found!");
                }

                entity = ContractsFacade.ContractProductSettingsService.Load(found.ID, null);
            }                

			return this;
		}
    }
}