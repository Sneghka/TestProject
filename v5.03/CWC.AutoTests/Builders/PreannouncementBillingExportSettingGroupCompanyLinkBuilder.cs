using Cwc.Billing;
using Cwc.Billing.Model;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class PreannouncementBillingExportSettingGroupCompanyLinkBuilder
	{

		PreannouncementBillingExportSettingGroupCompanyLink entity;

		public PreannouncementBillingExportSettingGroupCompanyLinkBuilder()
		{
		}

		public PreannouncementBillingExportSettingGroupCompanyLinkBuilder With_GroupID(Int32 value)
		{
			if (entity != null) 
			{
				entity.GroupID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PreannouncementBillingExportSettingGroupCompanyLinkBuilder With_CompanyID(Int32 value)
		{
			if (entity != null) 
			{
				entity.CompanyID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}
	
		public PreannouncementBillingExportSettingGroupCompanyLinkBuilder New()
		{
			entity = new PreannouncementBillingExportSettingGroupCompanyLink();
						
			return this;
		}

		public static implicit operator PreannouncementBillingExportSettingGroupCompanyLink(PreannouncementBillingExportSettingGroupCompanyLinkBuilder ins)
		{
			return ins.Build();
		}

		public PreannouncementBillingExportSettingGroupCompanyLink Build()
		{
			return entity;
		}

		public PreannouncementBillingExportSettingGroupCompanyLinkBuilder SaveToDb()
		{
            var result = BillingFacade.PreannouncementBillingExportSettingGroupCompanyLinkService.Save(entity);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"PreannouncementBillingExportSettingGroupCompanyLink wasn't saved. Reason: {result.GetMessage()}");
            }
            return this;
		}

		public PreannouncementBillingExportSettingGroupCompanyLinkBuilder Take(Expression<Func<PreannouncementBillingExportSettingGroupCompanyLink, bool>> expression)
		{
            using (var contest = new AutomationBillingContext())
            {
                var entity = contest.PreannouncementBillingExportSettingGroupCompanyLinks.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("PreannouncementBillingExportSettingGroupCompanyLink wasn't found");
                }
                return this;
            }            
		}

        public void Delete(Expression<Func<PreannouncementBillingExportSettingGroupCompanyLink, bool>> expression)
        {
            var entity = Take(expression);
            var result = BillingFacade.PreannouncementBillingExportSettingGroupCompanyLinkService.Delete(entity);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"PreannouncementBillingExportSettingGroupCompanyLink wasn't deleted. Reason: {result.GetMessage()}");
            }

        }
	}
}