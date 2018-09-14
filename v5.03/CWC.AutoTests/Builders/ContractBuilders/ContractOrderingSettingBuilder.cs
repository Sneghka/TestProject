using Cwc.Contracts;
using Cwc.Contracts.Model;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ContractOrderingSettingBuilder : IDisposable
	{		
		ContractOrderingSetting entity;
        AutomationContractDataContext context;        
        List<OrderingSettingLooseProductsLink> looseProductsLinkList = new List<OrderingSettingLooseProductsLink>();
        List<OrderingSettingProductConversionSetting> productConversionSettingList = new List<OrderingSettingProductConversionSetting>();

		public ContractOrderingSettingBuilder()
		{
            context = new AutomationContractDataContext();
        }

        public ContractOrderingSettingBuilder With_IsNew(bool value)
        {
            if (entity != null)
            {
                entity.SetIsNew(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

		public ContractOrderingSettingBuilder With_LeadTime(int value)
		{
			if (entity != null) 
			{
				entity.LeadTime = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_CITLeadTime(int value)
		{
			if (entity != null) 
			{
				entity.CITLeadTime = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_CCLeadTime(int value)
		{
			if (entity != null) 
			{
				entity.CCLeadTime = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_Period(int value)
		{
			if (entity != null) 
			{
				entity.Period = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_CutOffTime(Double value)
		{
			if (entity != null) 
			{
				entity.CutOffTime = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_MaxValue(decimal? value)
		{
			if (entity != null) 
			{
				entity.MaxValue = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_AllowTotalPreannouncement(bool value)
		{
			if (entity != null) 
			{
				entity.AllowTotalPreannouncement = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_IsNotes(bool value)
		{
			if (entity != null) 
			{
				entity.IsNotes = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

        public ContractOrderingSettingBuilder With_IsNotesLooseProductDelivery(bool value)
        {
            if (entity != null)
            {
                entity.IsNotesLooseProductDelivery = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractOrderingSettingBuilder With_IsCoins(bool value)
		{
			if (entity != null) 
			{
				entity.IsCoins = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}
        public ContractOrderingSettingBuilder With_IsCoinsLooseProductDelivery(bool value)
        {
            if (entity != null)
            {
                entity.IsCoinsLooseProductDelivery = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractOrderingSettingBuilder With_IsConsumables(bool value)
		{
			if (entity != null) 
			{
				entity.IsConsumables = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_IsSpecialCoins(bool value)
		{
			if (entity != null) 
			{
				entity.IsSpecialCoins = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_IsServicingCodes(bool value)
		{
			if (entity != null) 
			{
				entity.IsServicingCodes = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_IsPreAnnouncement(bool value)
		{
			if (entity != null) 
			{
				entity.IsPreAnnouncement = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_AllowComments(bool value)
		{
			if (entity != null) 
			{
				entity.AllowComments = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_AskForAnother(bool value)
		{
			if (entity != null) 
			{
				entity.AskForAnother = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_IsRelease(bool value)
		{
			if (entity != null) 
			{
				entity.IsRelease = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_EnableProducts(EnableProducts value)
		{
			if (entity != null) 
			{
				entity.EnableProducts = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

        public ContractOrderingSettingBuilder With_EnableLooseProducts(EnableProducts value)
        {
            if (entity != null)
            {
                entity.EnableLooseProducts = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractOrderingSettingBuilder With_IsNoteLooseProductDelivery(bool value)
        {
            if (entity != null)
            {
                entity.IsNotesLooseProductDelivery = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractOrderingSettingBuilder With_IsCoinLooseProductDelivery(bool value)
        {
            if (entity != null)
            {
                entity.IsCoinsLooseProductDelivery = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public ContractOrderingSettingBuilder With_Contract_id(int value)
		{
			if (entity != null) 
			{
				entity.SetContractID(value);
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_ServiceType_id(int value)
		{
			if (entity != null) 
			{
				entity.ServiceType_id = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_LocationType_code(string value)
		{
			if (entity != null) 
			{
				entity.LocationType_code = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_Location_id(decimal? value)
		{
			if (entity != null) 
			{
				entity.Location_id = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_AllowCustomerReference(bool value)
		{
			if (entity != null) 
			{
				entity.AllowCustomerReference = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_AllowBankReference(bool value)
		{
			if (entity != null) 
			{
				entity.AllowBankReference = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_AllowCitReference(bool value)
		{
			if (entity != null) 
			{
				entity.AllowCitReference = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}		        		

		public ContractOrderingSettingBuilder With_IsLatestRevision(bool value)
		{
			if (entity != null) 
			{
				entity.IsLatestRevision = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

        public void With_ContractProductConversionSettings(List<OrderingSettingProductConversionSetting> productConversionSettingsList)
        {
            if (entity != null)
            {
                foreach(var setting in productConversionSettingsList) {
                    var rslt = ContractsFacade.OrderingSettingProductConversionSettingService.Save(setting, null, context);
                    if (!rslt.IsSuccess)
                    {
                        throw new Exception(rslt.GetMessage());
                    }
                }
                //return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }



        public ContractOrderingSettingBuilder With_RevisionNumber(int? value)
        {
            if (entity != null)
            {
                entity.SetRevisionNumber(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractOrderingSettingBuilder With_RevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.RevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_ReplacedRevisionNumber(int? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionNumber = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_ReplacedRevisionDate(DateTime? value)
		{
			if (entity != null) 
			{
				entity.ReplacedRevisionDate = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_AuthorID(int? value)
		{
			if (entity != null) 
			{
				entity.AuthorID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_LatestRevisionID(int? value)
		{
			if (entity != null) 
			{
				entity.LatestRevisionID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_Contract(Contract value)
		{
			if (entity != null) 
			{
				entity.Contract = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ContractOrderingSettingBuilder With_ID(int value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}
					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

        public ContractOrderingSettingBuilder With_ContractProductsSettings(List<ContractProductSetting> value)
        {
            if (entity != null)
            {
                entity.SetContractProductsSettings(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractOrderingSettingBuilder With_LooseProductsLinks(List<OrderingSettingLooseProductsLink> value)
        {
            looseProductsLinkList = value;
            return this;
        }

        public ContractOrderingSettingBuilder With_ConversionProductSettings(List<OrderingSettingProductConversionSetting> value)
        {
            productConversionSettingList = value;
            return this;
        }

        public ContractOrderingSettingBuilder With_OrderingSettingServicingJobs(List<OrderingSettingServicingJob> value)
        {
            if (entity != null)
            {
                entity.SetOrderingSettingServicingJobs(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractOrderingSettingBuilder New()
		{
            entity = new ContractOrderingSetting();				
			return this;
		}

		public static implicit operator ContractOrderingSetting(ContractOrderingSettingBuilder ins)
		{
			return ins.Build();
		}

		public ContractOrderingSetting Build()
		{
			return entity;
		}

		public ContractOrderingSettingBuilder SaveToDb(bool isClearingRequired = false)
		{
            if (isClearingRequired)
            {
                HelperFacade.ContractHelper.ClearOrderingSettings(entity.Contract_id);
            }
            
            var result = ContractsFacade.ContractOrderingSettingsService.RevisionControlOnUpdate(entity, entity.Contract_id, null, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Contract ordering setting saving failed. Reason: { result.GetMessage() }");
            }

            if (looseProductsLinkList.Count != 0)
            {
                foreach(var item in looseProductsLinkList)
                {
                    result = ContractsFacade.OrderingSettingLooseProductsLinkService.Save(item);
                    if (!result.IsSuccess)
                    {
                        throw new InvalidOperationException($"Contract ordering setting loose product link saving failed. Reason: { result.GetMessage() }");
                    }
                }                
            }

            if (productConversionSettingList.Count != 0)
            {
                foreach (var item in productConversionSettingList)
                {
                    result = ContractsFacade.OrderingSettingProductConversionSettingService.Save(item);
                    if (!result.IsSuccess)
                    {
                        throw new InvalidOperationException($"Contract ordering setting product conversion setting saving failed. Reason: { result.GetMessage() }");
                    }
                }
            }

            return this;
        }


        public ContractOrderingSettingBuilder Take(Expression<Func<ContractOrderingSetting, bool>> expression)
		{
            entity = context.ContractOrderingSetting.FirstOrDefault(expression);

            if (entity == null)
            {
                throw new InvalidOperationException("Contract ordering setting is not found!");
            }

            return this;
        }

        public IEnumerable<ContractOrderingSetting> LoadAllOrderingSettings(Func<ContractOrderingSetting, bool> expression)
        {
            return context.ContractOrderingSetting.Where(expression).ToArray();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}