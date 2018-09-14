using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.Common;
using Cwc.Common.Extensions.Data;
using CWC.AutoTests.Core;
using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CWC.AutoTests.Helpers
{
    public class SettingsHelper
    {
        /// <summary>
        /// Set up cash center site and process settings
        /// </summary>
        /// <param name="siteSettings">Dictionary with site setting property and flag keyvalue pairs</param>
        /// <param name="processSettings">Dictionary with process setting property and flag keyvalue pairs</param>
        /// <param name="dbParams">Database parameters</param>
        public static void SetUpCashCenterSettings(Dictionary<string, bool> siteSettings, Dictionary<string, bool> processSettings, DataBaseParams dbParams)
        {
            var result = EditSiteSettings(siteSettings, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(result.GetMessage());
            }

            result = EditProcessSettings(processSettings, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(result.GetMessage());
            }
        }
        
        /// <summary>
        /// Edit cash center site settings
        /// </summary>
        /// <param name="settings">Dictionary with site setting property and flag keyvalue pairs</param>
        /// <param name="dbParams">Database parameters</param>
        /// <returns>Result</returns>
        private static Result EditSiteSettings(Dictionary<string, bool> settings, DataBaseParams dbParams)
        {
            Site site = BaseDataFacade.SiteService.GetByCode(Configuration.SiteCode);
            CashCenterSiteSetting siteSettings = CashCenterFacade.SiteSettingService.MatchCashCenterSiteSettings(site.ID);
            var dataBaseExecutor = new DataBaseExecutor(dbParams);
            var isCommit = false;
            try
            {
                dataBaseExecutor.OpenConnection();
                dataBaseExecutor.BeginTransaction();                            
                foreach (var setting in settings)
                {
                    siteSettings.GetType().GetProperty(setting.Key).SetValue(siteSettings, setting.Value);
                }

                var result = CashCenterFacade.SiteSettingService.Save(siteSettings, null);
                if (!result.IsSuccess)
                {
                    return result;
                }

                isCommit = true;
                return new Result(true);
            }

            finally
            {
                dataBaseExecutor.EndTransaction(isCommit);
                dataBaseExecutor.CloseConnection();
            }
        }

        /// <summary>
        /// Edit cash center process settings
        /// </summary>
        /// <param name="settings">Dictionary with process setting property and flag keyvalue pairs</param>
        /// <param name="dbParams">Database parameters</param>
        /// <returns>Result</returns>
        private static Result EditProcessSettings(Dictionary<string, bool> settings, DataBaseParams dbParams)
        {           
            var processSettings = GetProcessSettings();            
            var dataBaseExecutor = new DataBaseExecutor(dbParams);
            var isCommit = false;
            try
            {
                dataBaseExecutor.OpenConnection();
                dataBaseExecutor.BeginTransaction();

                foreach (var setting in settings)
                {
                    processSettings.GetType().GetProperty(setting.Key).SetValue(processSettings, setting.Value);
                }

                var result = CashCenterFacade.ProcessSettingService.Save(processSettings, null);
                if (!result.IsSuccess)
                {
                    return result;
                }

                isCommit = true;
                return new Result(true);
            }

            finally
            {
                dataBaseExecutor.EndTransaction(isCommit);
                dataBaseExecutor.CloseConnection();
            }
        }        

        /// <summary>
        /// Get id of a location with received code for matching process settings
        /// </summary>
        /// <param name="locationCode"> Location code of main location for testing </param>
        /// <returns> Location Id </returns> 
        public static decimal? GetLocationId(string locationCode)
        {
            var whereConditions = new WhereConditions();
            whereConditions.And("ref_loc_nr = @location_code", locationCode);
            var dataTable = DataBaseHelper.Select(typeof(Location).GetTableName(), null, whereConditions.Sql, whereConditions.ParamsAsDictionary, null);
            DataRow[] rows = dataTable.Select();

            if (rows != null)
            {
                return DataUtils.GetDecimal(rows[0]["loc_nr"]);
            }

            else
            {
                return null;
            }
        }

        // Get mandatory location attribute
        public static decimal GetMandatoryLocationId(string locationCode)
        {
            var whereConditions = new WhereConditions();
            whereConditions.And("ref_loc_nr = @location_code", locationCode);
            var dataTable = DataBaseHelper.Select(typeof(Location).GetTableName(), null, whereConditions.Sql, whereConditions.ParamsAsDictionary, null);
            DataRow[] rows = dataTable.Select();

            if (rows != null)
            {
                return DataUtils.GetDecimal(rows[0]["loc_nr"]);
            }

            else
            {
                throw new Exception("Location has not been found!");
            }
        }

        /// <summary>
        /// Get customer id for process settings matching
        /// </summary>
        /// <returns>Customer id</returns>
        public static decimal GetCustomerId()
        {
            using (var context = new AutomationBaseDataContext())
            {
                var customerResult = context.Customers.FirstOrDefault(x => x.ReferenceNumber == Configuration.CompanyCode && x.Name == Configuration.CompanyName);
                if (customerResult == null)
                {
                    throw new ArgumentNullException($"Customer with Code = {Configuration.CompanyCode} and Name = {Configuration.CompanyName} wasn't found");
                }

                return customerResult.ID;
            }
        }


        /// <summary>
        /// Get customer referenceNumber for process settings matching
        /// </summary>
        /// <returns>Customer id</returns>
        public static string GetCustomerRefNr()
        {
            using (var context = new AutomationBaseDataContext())
            {
                var customerResult = context.Customers.FirstOrDefault(x => x.ReferenceNumber == Configuration.CompanyCode && x.Name == Configuration.CompanyName);
                if (customerResult == null)
                {
                    throw new ArgumentNullException($"Customer with Code = {Configuration.CompanyCode} and Name = {Configuration.CompanyName} wasn't found");
                }

                return customerResult.ReferenceNumber;
            }
        }
        /// <summary>
        /// Match process settings
        /// </summary>
        /// <returns></returns>
        private static CashCenterProcessSetting GetProcessSettings()
        {
            decimal? location = GetLocationId(Configuration.LocationCode);
            decimal company = GetCustomerId();
            var processSettings = CashCenterFacade.ProcessSettingService.MatchCashCenterProcessSetting(company, location, null);
            return processSettings;
        }
    }
}
