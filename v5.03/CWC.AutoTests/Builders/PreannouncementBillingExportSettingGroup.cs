using Cwc.BaseData;
using Cwc.Billing;
using Cwc.Billing.Model;
using Cwc.Security;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class PreannouncementBillingExportSettingGroupBuilder
	{
		PreannouncementBillingExportSettingGroup entity;

		public PreannouncementBillingExportSettingGroupBuilder()
		{
		}

		public PreannouncementBillingExportSettingGroupBuilder With_LastSequenceNumberBilling(Int32 value)
		{
			if (entity != null) 
			{
				entity.LastSequenceNumberBilling = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PreannouncementBillingExportSettingGroupBuilder With_LastSequenceNumberPreann(Int32 value)
		{
			if (entity != null) 
			{
				entity.LastSequenceNumberPreann = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public PreannouncementBillingExportSettingGroupBuilder With_GroupName(String value)
		{
			if (entity != null) 
			{
				entity.GroupName = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}
	
		public PreannouncementBillingExportSettingGroupBuilder New()
		{
			entity = new PreannouncementBillingExportSettingGroup();
						
			return this;
		}

		public static implicit operator PreannouncementBillingExportSettingGroup(PreannouncementBillingExportSettingGroupBuilder ins)
		{
			return ins.Build();
		}

		public PreannouncementBillingExportSettingGroup Build()
		{
			return entity;
		}

		public PreannouncementBillingExportSettingGroupBuilder SaveToDb()
		{
            var loginResult = SecurityFacade.LoginService.GetAdministratorLogin();
            UserParams userParams = new UserParams(loginResult);
            var result = BillingFacade.PreannouncementBillingExportSettingGroupService.Save(entity, userParams);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"PreannouncementBillingExportSettingGroup wasn't saved. Reason: {result.GetMessage()}");
            }
			return this;
		}

		public PreannouncementBillingExportSettingGroupBuilder Take(Expression<Func<PreannouncementBillingExportSettingGroup, bool>> expression)
		{
            using (var context = new AutomationBillingContext())
            {
                var entity = context.PreannouncementBillingExportSettingGroups.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new ArgumentNullException("PreannouncementBillingExportSettingGroup wasn't found");
                }
                return this;
            }            
		}

        public void Delete(Expression<Func<PreannouncementBillingExportSettingGroup, bool>> expression)
        {
            var entity = Take(expression);
            var result = BillingFacade.PreannouncementBillingExportSettingGroupService.Delete(entity);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"PreannouncementBillingExportSettingGroup wasn't deleted. Reason: {result.GetMessage()}");
            }
        }

	}
}