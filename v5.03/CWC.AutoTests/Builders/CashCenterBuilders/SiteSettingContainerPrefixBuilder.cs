using Cwc.CashCenter;
using Cwc.Common;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class SiteSettingContainerPrefixBuilder
    {
        CashCenterSiteSettingContainerPrefix _prefix;
        DataBaseParams dbParams = new DataBaseParams();
        AutomationCashCenterDataContext _context = new AutomationCashCenterDataContext();
        public SiteSettingContainerPrefixBuilder()
        {
            _prefix = new CashCenterSiteSettingContainerPrefix();
        }
        public SiteSettingContainerPrefixBuilder WithPrefix(string prefix)
        {
            _prefix.ContainerPrefix = prefix;

            return this;
        }

        public SiteSettingContainerPrefixBuilder WithAutostart(bool autostart)
        {
            _prefix.IsAutostartCounting = autostart;

            return this;
        }

        public SiteSettingContainerPrefixBuilder WithLocation(decimal? location)
        {
            _prefix.LocationSubstitutionID = location;

            return this;
        }

        public SiteSettingContainerPrefixBuilder WithSiteSettings(int siteSettingsId)
        {
            _prefix.SetCashCenterSiteSettingID(siteSettingsId);

            return this;
        }

        public CashCenterSiteSettingContainerPrefix Biuld()
        {
            return _prefix;
        }

        public static implicit operator CashCenterSiteSettingContainerPrefix(SiteSettingContainerPrefixBuilder ins)
        {
            return ins.Biuld();
        }

        public SiteSettingContainerPrefixBuilder SaveToDb()
        {

            var entity = _context.CashCenterSiteSettingContainerPrefixes.Where(
                p => p.ContainerPrefix == _prefix.ContainerPrefix && 
                p.LocationSubstitutionID == _prefix.LocationSubstitutionID &&
                p.CashCenterSiteSettingID == _prefix.CashCenterSiteSettingID
                ).FirstOrDefault();

            if (entity == null)
            {
                var saveResult = CashCenterFacade.SiteSettingContainerPrefixService.Save(_prefix, dbParams);

                if (!saveResult.IsSuccess)
                {
                    throw new InvalidOperationException($"Save Result was unsuccessfull, message \n{saveResult.GetMessage()}");
                }

                return this;
            }

            _prefix.SetID(entity.ID);

            var updateResult = CashCenterFacade.SiteSettingContainerPrefixService.Save(_prefix, dbParams);

            if (!updateResult.IsSuccess)
            {
                throw new InvalidOperationException($"Save Result was unsuccessfull, message \n{updateResult.GetMessage()}");
            }

            return this;
        }
    }
}
