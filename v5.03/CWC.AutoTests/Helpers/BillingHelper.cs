using Cwc.BaseData;
using Cwc.Billing;
using Cwc.Billing.Model;
using Cwc.Common;
using Cwc.Contracts;
using Cwc.Contracts.Enums;
using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace CWC.AutoTests.Helpers
{
    public class BillingHelper
    {
        BillingContext context;

        public BillingHelper()
        {
            this.context = new AutomationBillingContext();
        }

        public void RunBillingJob()
        {
            BillingJobSettings settings = null;
            using (var context = new AutomationBillingContext())
            {
                var dbParams = context.GetDatabaseParams();
                settings = BillingsFacade.BillingJobSettingsService.Load(dbParams);    
                           
                try
                {
                    BillingsFacade.BillingJobService.RunBillingJob(settings, null, context);
                }
                catch
                {
                    throw;
                }               
            }
        }

        public static void UpdateBillingJobSettingsWithNewCompany(Customer company)
        {
            BillingJobSettings settings = null;
            using (var context = new AutomationBillingContext())
            {
                var dbParams = context.GetDatabaseParams();
                settings = BillingsFacade.BillingJobSettingsService.Load(dbParams);
                var settingsCompanyLink = context.BillingJobSettingsCompanyLinks.Where(bl => bl.CompanyID == company.ID).FirstOrDefault();
                if (settingsCompanyLink == null)
                {
                    BillingsFacade.BillingJobSettingsCompanyLinkService.LinkCompaniesToBillingJobSettings(settings.ID, new decimal[] { company.ID }, dbParams);
                    BillingsFacade.BillingJobSettingsService.Save(settings, dbParams);
                }
            }
        }
        
        /// <summary>
        /// Edit "Date created" attribute of custom location
        /// </summary>
        /// <param name="location">Location to edit</param>
        /// <param name="newDate">New date value</param>
        public void EditDateCreatedOfLocation(Location location, DateTime newDate)
        {
            location.SetDateCreated(newDate);
            var result = BaseDataFacade.LocationService.Save(location, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Editing of location date created has failed. Reason: { result.GetMessage() }.");
            }

            return;
        }

        public List<decimal> UnlinkAllLocationsExceptOneFromCompany(decimal locationID, decimal companyID)
        {
            using (var context = new AutomationBaseDataContext())
            {
                List <decimal> idsList = new List<decimal> ();
                var locationList = context.Locations.Where(l => l.CompanyID == companyID && l.ID != locationID).ToList();
                foreach (var location in locationList)
                {
                    location.CompanyID = 0;
                    idsList.Add(location.ID);
                }

                context.SaveChanges();
                return idsList;
            }            
        }

        public void LinkLocationsToCompany(List<decimal> idsList, decimal companyID)
        {
            using (var context = new AutomationBaseDataContext())
            {                
                context.Locations.Where(l => idsList.Contains(l.ID))
                    .ToList()
                    .ForEach(l => l.CompanyID = companyID);

                context.SaveChanges();
            }

            return;
        }

        public BilledCase GetBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, DateTime date)
        {
            var billedCase = context.BilledCases.Where(b => 
                                                            b.ContractID == contractID 
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit 
                                                            && DbFunctions.TruncateTime(b.DateCreated) == date)
                                                            .OrderBy(b => b.DateCreated)
                                                            .FirstOrDefault();
            return billedCase;
        }

        /// <summary>
        /// Find billed case for 'price per service order', 'price per cancelled service order', 'price per service order packing start' rules
        /// </summary>
        /// <param name="contractID">Contract ID</param>
        /// <param name="priceRule">Price rule</param>
        /// <param name="unit">Unit of measure</param>
        /// <param name="serviceOrderID">Service order ID</param>
        /// <returns>Billed case</returns>
        public BilledCase GetServiceOrderBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, int serviceOrderID)
        {
            if (priceRule != PriceRule.PricePerServiceOrder && priceRule != PriceRule.PricePerCancelledServiceOrder && 
                priceRule != PriceRule.PricePerServiceOrderPackingStart && priceRule != PriceRule.CorrectivePricePerServiceOrderForColocatedLocationGroups)
            {
                throw new InvalidOperationException("Only 'price per service order' and 'price per cancelled service order' rules are allowed!");
            }

            if (unit != UnitOfMeasure.ServiceOrder)
            {
                throw new InvalidOperationException("Only 'service order' unit of measure is allowed!");
            }

            var billedCase = context.BilledCases.Where(b =>
                                                            b.ContractID == contractID
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit
                                                            && b.ServiceOrderID == serviceOrderID)
                                                            .SingleOrDefault();
            return billedCase;
        }

        public List<BilledCase> GetServiceOrderBilledCases(int contractID, PriceRule priceRule, UnitOfMeasure unit, int serviceOrderID)
        {
            if (priceRule != PriceRule.PricePerServiceOrder && priceRule != PriceRule.PricePerCancelledServiceOrder &&
                priceRule != PriceRule.PricePerServiceOrderPackingStart && priceRule != PriceRule.CorrectivePricePerServiceOrderForColocatedLocationGroups)
            {
                throw new InvalidOperationException("Only 'price per service order' and 'price per cancelled service order' rules are allowed!");
            }

            if (unit != UnitOfMeasure.ServiceOrder)
            {
                throw new InvalidOperationException("Only 'service order' unit of measure is allowed!");
            }

            var billedCaseList = context.BilledCases.Where(b =>
                                                                b.ContractID == contractID
                                                                && b.PriceRule == priceRule
                                                                && b.UnitOfMeasure == unit
                                                                && b.ServiceOrderID == serviceOrderID)
                                                                .ToList();
            return billedCaseList;
        }

        /// <summary>
        /// Find billed case for 'price per ordered product' rule
        /// </summary>
        /// <param name="contractID">Contract ID</param>
        /// <param name="priceRule">Price rule</param>
        /// <param name="unit">Unit of measure</param>
        /// <param name="serviceOrderID">Service order ID</param>
        /// <param name="productID">Product ID</param>
        /// <returns>Billed case</returns>
        public BilledCase GetOrderedProductBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, int serviceOrderID, int productID)
        {
            if (priceRule != PriceRule.PricePerOrderedProduct || unit != UnitOfMeasure.Product)
            {
                throw new InvalidOperationException("Only 'price per ordered product' rule and 'product' unit of measure are allowed!");
            }

            var billedCase = context.BilledCases.Where(b =>
                                                            b.ContractID == contractID
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit
                                                            && b.ServiceOrderID == serviceOrderID
                                                            && b.ProductID == productID)
                                                            .SingleOrDefault();
            return billedCase;
        }

        public BilledCase GetPeriodBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, Location location, int serviceTypeID)
        {
            var billedCase = context.BilledCases.Where(b =>
                                                            b.ContractID == contractID
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit
                                                            && b.ServiceTypeID == serviceTypeID)                                                            
                                                            .SingleOrDefault();
            return billedCase;
        }

        /// <summary>
        /// Get billed case for "fixed price per period for location and service type" rule
        /// </summary>
        /// <param name="contractID"></param>
        /// <param name="priceRule"></param>
        /// <param name="unit"></param>
        /// <param name="location"></param>
        /// <param name="serviceTypeID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public BilledCase GetPeriodBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, Location location, int serviceTypeID,
            DateTime StartDate, DateTime EndDate)
        {            
            var billedCase = context.BilledCases.Where(b =>
                                                            b.ContractID == contractID
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit
                                                            && b.ServiceTypeID == serviceTypeID
                                                            && b.LocationID == location.IdentityID
                                                            && DbFunctions.TruncateTime(b.PeriodBilledFrom) == StartDate
                                                            && DbFunctions.TruncateTime(b.PeriodBilledTo) == EndDate)
                                                            .SingleOrDefault();
            return billedCase;
        }

        /// <summary>
        /// Get billed case for "fixed price per period for company" rule
        /// </summary>
        /// <param name="contractID"></param>
        /// <param name="priceRule"></param>
        /// <param name="unit"></param>
        /// <param name="location"></param>
        /// <param name="serviceTypeID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public BilledCase GetPeriodBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, int companyID,
            DateTime StartDate, DateTime EndDate)
        {
            var billedCase = context.BilledCases.Where(b =>
                                                            b.ContractID == contractID
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit
                                                            && b.CompanyID == companyID
                                                            && DbFunctions.TruncateTime(b.PeriodBilledFrom) == StartDate
                                                            && DbFunctions.TruncateTime(b.PeriodBilledTo) == EndDate)
                                                            .SingleOrDefault();
            return billedCase;
        }

        public BillingLine GetBillingLine(int billedCaseID)
        {
            var billingLine = context.BillingLines.Where(b => b.BilledCaseID == billedCaseID).SingleOrDefault();
            return billingLine;
        }

        public BillingLine GetBillingLineByPriceLine(int priceLineID)
        {
            var billingLine = context.BillingLines.Where(b => b.PriceLineID == priceLineID).SingleOrDefault();
            return billingLine;
        }

        public BilledCaseDailyStop GetBilledCaseDailyStopByStop(int dailyStopID)
        {
            var billedCaseDailyStop = context.BilledCaseDailyStops.Where(b => b.DailyStopID == dailyStopID).SingleOrDefault();
            return billedCaseDailyStop;
        }

        public BilledCaseDailyStop GetBilledCaseDailyStopByCase(int billedCaseID)
        {
            var billedCaseDailyStop = context.BilledCaseDailyStops.Where(b => b.BilledCaseID == billedCaseID).SingleOrDefault();
            return billedCaseDailyStop;
        }

        /// <summary>
        /// Find billed case for 'visits per day per customer' rule
        /// </summary>
        /// <param name="contractID">Contract ID</param>
        /// <param name="priceRule">Price rule</param>
        /// <param name="unit">Unit of measure</param>
        /// <param name="date">Date billed</param>
        /// <returns>Billed case</returns>
        public BilledCase GetVisitsPerDayBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, DateTime date)
        {
            if ((priceRule != PriceRule.PricePerVisitPerCompany || unit != UnitOfMeasure.LocationStop) &&
                (priceRule != PriceRule.InsuranceFeePerTransportedContainer || unit != UnitOfMeasure.LocationContainer))
            {
                throw new InvalidOperationException("Only 'price per service order' and 'price per cancelled service order' rules are allowed!");
            }
            var billedCase = context.BilledCases.Where(b => 
                                                            b.ContractID == contractID 
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit 
                                                            && DbFunctions.TruncateTime(b.DateBilled) == date)                                                            
                                                            .SingleOrDefault();
            return billedCase;
        }

        /// <summary>
        /// Find billed cases for 'visits per day per customer' rule for multiple dates
        /// </summary>
        /// <param name="contractID">Contract ID</param>
        /// <param name="priceRule">Price rule</param>
        /// <param name="unit">Unit of measure</param>
        /// <param name="startDate">Start date billed</param>
        /// <param name="endDate">End date billed</param>
        /// <returns>Billed case</returns>
        public List<BilledCase> GetVisitsPerDayBilledCases(int contractID, PriceRule priceRule, UnitOfMeasure unit, DateTime startDate, DateTime endDate)
        {
            if ((priceRule != PriceRule.PricePerVisitPerCompany || unit != UnitOfMeasure.LocationStop) &&
                (priceRule != PriceRule.InsuranceFeePerTransportedContainer || unit != UnitOfMeasure.Container))
            {
                throw new InvalidOperationException("Price rule and unit of measure combination is invalid! Please check SD.");
            }
            var billedCaseList = context.BilledCases.Where(b => 
                                                                b.ContractID == contractID 
                                                                && b.PriceRule == priceRule
                                                                && b.UnitOfMeasure == unit 
                                                                && DbFunctions.TruncateTime(b.DateBilled) >= startDate 
                                                                && DbFunctions.TruncateTime(b.DateBilled) <= endDate)
                                                                .OrderBy(b => b.DateBilled)
                                                                .ToList();
            return billedCaseList;
        }

        /// <summary>
        /// Find billed case for '4.6.2 Visit at location' use-case
        /// </summary>
        /// <param name="contractID">Contract ID</param>
        /// <param name="priceRule">Price rule</param>
        /// <param name="unit">Unit of measure</param>
        /// <param name="stopID">Daily stop ID</param>
        /// <returns>Billed case</returns>
        public BilledCase GetVisitAtLocationBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, int stopID)
        {
            if ((priceRule != PriceRule.PricePerVisit || unit != UnitOfMeasure.LocationStop) 
                && (priceRule != PriceRule.PavemenTransportFee || (unit != UnitOfMeasure.LocationStop && unit != UnitOfMeasure.LocationItem 
                    && unit != UnitOfMeasure.LocationKg)) 
                && (priceRule != PriceRule.PricePerVisitingTime || (unit != UnitOfMeasure.LocationHours && unit != UnitOfMeasure.LocationMinutes))
                && (priceRule != PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit || unit != UnitOfMeasure.LocationStop))
            {
                throw new InvalidOperationException("Price rule and unit of measure combination is invalid! Please check SD.");
            }
            var billedCase = context.BilledCases.Where(b => 
                                                            b.ContractID == contractID 
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit 
                                                            && b.DailyStopID == stopID)                                                                        
                                                            .SingleOrDefault();
            return billedCase;
        }

        public List<BilledCase> GetVisitAtLocationBilledCases(int contractID, PriceRule priceRule, UnitOfMeasure unit, int stopID)
        {
            if (priceRule != PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit || unit != UnitOfMeasure.LocationStop)
            {
                throw new InvalidOperationException("Price rule and unit of measure combination is invalid! Please check SD.");
            }
            var billedCaseList = context.BilledCases.Where(b =>
                                                                b.ContractID == contractID
                                                                && b.PriceRule == priceRule
                                                                && b.UnitOfMeasure == unit
                                                                && b.DailyStopID == stopID)                                                                
                                                                .ToList();
            return billedCaseList;
        }

        /// <summary>
        /// Find billed case for '4.6.3 Visit at visit address' use-case
        /// </summary>
        /// <param name="contractID">Contract ID</param>
        /// <param name="priceRule">Price rule</param>
        /// <param name="unit">Unit of measure</param>
        /// <param name="visitAddressID">Visit address ID</param>
        /// <param name="routeDate">Route date</param>
        /// <returns>Billed case</returns>
        public BilledCase GetVisitAtVisitAddressBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, int visitAddressID, 
            DateTime routeDate)
        {
            if ((priceRule != PriceRule.PricePerVisit || unit != UnitOfMeasure.VisitAddressStop) 
                && (priceRule != PriceRule.PavemenTransportFee || (unit != UnitOfMeasure.VisitAddressStop && unit != UnitOfMeasure.VisitAddressItem 
                    && unit != UnitOfMeasure.VisitAddressKg)) 
                && (priceRule != PriceRule.PricePerVisitingTime || (unit != UnitOfMeasure.VisitAddressHours && unit != UnitOfMeasure.VisitAddressMinutes))
                && (priceRule != PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit || unit != UnitOfMeasure.VisitAddressStop))
            {
                throw new InvalidOperationException("Price rule and unit of measure combination is invalid! Please check SD.");
            }
            var billedCase = context.BilledCases.Where(b =>
                                                            b.ContractID == contractID
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit
                                                            && b.VisitAddressID == visitAddressID
                                                            && b.DateBilled == routeDate)                                                                        
                                                            .SingleOrDefault(); // review after business logic is updated for multiple visits at the same visit address in the same day
            return billedCase;
        }

        public List<BilledCase> GetVisitAtVisitAddressBilledCases(int contractID, PriceRule priceRule, UnitOfMeasure unit, int visitAddressID,
            DateTime routeDate)
        {
            if (priceRule != PriceRule.PricePerOrderTypeAndServiceTypeDuringVisit || unit != UnitOfMeasure.VisitAddressStop)
            {
                throw new InvalidOperationException("Price rule and unit of measure combination is invalid! Please check SD.");
            }
            var billedCaseList = context.BilledCases.Where(b =>
                                                            b.ContractID == contractID
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit
                                                            && b.VisitAddressID == visitAddressID
                                                            && b.DateBilled == routeDate)
                                                            .ToList();
            return billedCaseList;
        }

        /// <summary>
        /// Find billed case for '4.6.4 Container type at location' use-case
        /// </summary>
        /// <param name="contractID">Contract ID</param>
        /// <param name="priceRule">Price rule</param>
        /// <param name="unit">Unit of measure</param>
        /// <param name="stopID">Daily stop ID</param>
        /// <param name="containerTypeID">Container type ID</param>
        /// <returns>Billed case</returns>
        public BilledCase GetContainerTypeAtLocationBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, int stopID, int containerTypeID)
        {
            if (((priceRule != PriceRule.PricePerCollectedContainer && priceRule != PriceRule.PricePerDeliveredContainer && 
                priceRule != PriceRule.PricePerCollectedAndDeliveredContainer) || (unit != UnitOfMeasure.LocationContainer && 
                unit!= UnitOfMeasure.LocationValue))
                && (priceRule != PriceRule.PavemenTransportFee || unit != UnitOfMeasure.LocationContainer))                
            {
                throw new InvalidOperationException("Price rule and unit of measure combination is invalid! Please check SD.");
            }
            var billedCase = context.BilledCases.Where(b =>
                                                            b.ContractID == contractID
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit
                                                            && b.DailyStopID == stopID
                                                            && b.ContainerTypeID == containerTypeID)
                                                            .SingleOrDefault();
            return billedCase;
        }

        /// <summary>
        /// Find billed case for '4.6.5 Container type at visit address' use-case
        /// </summary>
        /// <param name="contractID">Contract ID</param>
        /// <param name="priceRule">Price rule</param>
        /// <param name="unit">Unit of measure</param>
        /// <param name="routeCode">Route code (mast_cd)</param>
        /// <param name="visitAddressID">Visit address ID</param>
        /// <param name="containerTypeID">Container type ID</param>
        /// <returns>Billed case</returns>
        public BilledCase GetContainerTypeAtVisitAddressBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, string routeCode,
            int visitAddressID, int containerTypeID)
        {
            if (((priceRule != PriceRule.PricePerCollectedContainer && priceRule != PriceRule.PricePerDeliveredContainer &&
                priceRule != PriceRule.PricePerCollectedAndDeliveredContainer) || (unit != UnitOfMeasure.VisitAddressContainer &&
                unit != UnitOfMeasure.VisitAddressValue))
                && (priceRule != PriceRule.PavemenTransportFee || unit != UnitOfMeasure.VisitAddressContainer))
            {
                throw new InvalidOperationException("Price rule and unit of measure combination is invalid! Please check SD.");
            }
            var billedCase = context.BilledCases.Where(b =>
                                                            b.ContractID == contractID
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit
                                                            && b.MasterRouteCode == routeCode
                                                            && b.VisitAddressID == visitAddressID
                                                            && b.ContainerTypeID == containerTypeID)
                                                            .SingleOrDefault();
            return billedCase;
        }

        public BilledCase GetPricePerDeliveredItemAtLocationBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, int stopID,
            int? productGroupID = null)
        {
            var billedCase = context.BilledCases.Where(b =>
                                                            b.ContractID == contractID
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit
                                                            && b.DailyStopID == stopID
                                                            && b.ProductGroupID == productGroupID)
                                                            .SingleOrDefault();
            return billedCase;
        }

        public BilledCase GetPricePerDeliveredItemAtVisitAddressBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, string routeCode,
            int visitAddressID, int? productGroupID = null)
        {
            var billedCase = context.BilledCases.Where(b =>
                                                            b.ContractID == contractID
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit
                                                            && b.MasterRouteCode == routeCode
                                                            && b.VisitAddressID == visitAddressID
                                                            && b.ProductGroupID == productGroupID)
                                                            .SingleOrDefault();
            return billedCase;
        }

        public BilledCase GetServicingJobAtLocationBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, int stopID, int? servicingJobID = null)
        {
            var billedCase = context.BilledCases.Where(b =>
                                                            b.ContractID == contractID
                                                            && b.PriceRule == priceRule
                                                            && b.UnitOfMeasure == unit
                                                            && b.DailyStopID == stopID
                                                            && b.DailyStopJobID == servicingJobID)
                                                            .SingleOrDefault();
            return billedCase;
        }

        public BilledCase GetTransportIncidentBilledCase(int contractID, PriceRule priceRule, UnitOfMeasure unit, int callID)
        {
            var billedCase = context.BilledCases.Where(x =>
                                                            x.ContractID == contractID
                                                            && x.PriceRule == priceRule
                                                            && x.UnitOfMeasure == unit
                                                            && x.CallID == callID 
                                                            )
                                                            .SingleOrDefault();

            return billedCase;
        }

        /// <summary>
        /// Get date of the first day of the week of the provided date
        /// </summary>
        /// <param name="dateProvided">Provided date</param>
        /// <returns>First day of the week date</returns>
        public DateTime GetFirstDayOfTheWeekDate(DateTime dateProvided)
        {
            var date = dateProvided;
            var currentDay = dateProvided.DayOfWeek; // from Sunday (0) to Saturday (6) 
            var firstWeekDay = (DayOfWeek)FormatSettingsProvider.GetFirstDayOfWeekFromWeekday(FormatSettingsProvider.FirstWeekday); // from Monday (0) to Sunday (6) 
            var diff = currentDay - firstWeekDay;
            return date.AddDays(diff == -1 ? -6 : -diff);
        }

        /// <summary>
        /// Set FirstWeekDay of CWC format settings record
        /// </summary>
        /// <param name="weekday">Weekday to set (from Monday (0) to Sunday (6))</param>
        public void SetFirstDayOfTheWeek(Weekday weekday)
        {
            using (var context = new AutomationBillingContext())
            {
                context.Database.ExecuteSqlCommand("UPDATE dbo.WP_FormatSettings SET FirstWeekDay = @weekDay;", new SqlParameter("weekDay", (int)weekday));
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get value of CWC WeekdayName enumeration for particular date
        /// </summary>
        /// <param name="date">Date to define weekday name</param>
        /// <returns>WeekdayName value</returns>
        public WeekdayName GetWeekDayName(DateTime date)
        {
            return MatchingScheduleLine.GetWeekdayName(date);
        }

        public void SetZeroValueBillingFlag(bool flag)
        {
            using (var context = new AutomationBillingContext())
            {
                context.Database.ExecuteSqlCommand($"UPDATE Cwc_Billing_BillingJobSettings SET IsSaveBillingLinesWithZeroValue = @Flag;",
                    new SqlParameter("Flag", flag));
                context.SaveChanges();
            }
        }

        public void SetBillCollectedOrdersFlag(bool flag)
        {
            using (var context = new AutomationBillingContext())
            {                
                context.Database.ExecuteSqlCommand($"UPDATE Cwc_Billing_BillingJobSettings SET IsBillCollectedTransportOrder = @Flag;",
                    new SqlParameter("Flag", flag));
                context.SaveChanges();
            }
        }

        public BillingJobLog GetBillingJobLogRecord()
        {
            return context.BillingJobLogs.OrderByDescending(l => l.ID).First();
        }

        public object GetEntity(string entityName, string code)
        {
            object entity;

            switch (entityName)
            {  
                case ("CallCategory"):
                    using (var context = new AutomationCallManagementDataContext())
                    {
                        entity = context.CallCategories.FirstOrDefault(x => x.Name == code);
                        if (entity == null)
                        {
                            throw new KeyNotFoundException($"Call category with name = '{code}' is not found.");
                        }
                    }                       
                    break;

                case ("SolutionCode"):
                    using (var context = new AutomationCallManagementDataContext())
                    {
                        entity = context.SolutionCodes.FirstOrDefault(x => x.Code == code);
                        if (entity == null)
                        {
                            throw new KeyNotFoundException($"Solution code with code = '{code}' is not found.");
                        }
                    }
                    break;

                default: throw new InvalidOperationException($"Provided entity name '{entityName}' is invalid.");
            }

            return entity;
        }

        //public void ClearTestData(BillingContext billingContext = null)
        //{
        //    using (var newContext = new AutomationBillingContext())
        //    {
        //        var context = billingContext ?? newContext;                
        //        context.BillingJobLogs.RemoveRange(context.BillingJobLogs);                
        //        context.BilledCaseDailyStops.RemoveRange(context.BilledCaseDailyStops);
        //        context.BilledCases.RemoveRange(context.BilledCases);
        //        context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.Cwc_Billing_BillingLineHistoricalFields");
        //        context.BillingLines.RemoveRange(context.BillingLines);                
        //        context.SaveChanges(context, "Error on saving changes in BillingHelper on ClearTestData step.);
        //    }
        //}

        public void ClearTestData(AutomationBillingContext billingContext = null)
        {
            using (var newContext = new AutomationBillingContext())
            {
                var context = billingContext ?? newContext;
                context.PreannouncementExportedBillingLines.RemoveRange(context.PreannouncementExportedBillingLines);
                context.SaveChanges();
                context.ExportedBillingLines.RemoveRange(context.ExportedBillingLines);
                context.BillingLines.RemoveRange(context.BillingLines);
                context.SaveChanges();
                context.BilledCaseDailyStops.RemoveRange(context.BilledCaseDailyStops);
                context.SaveChanges();
                context.BilledCases.RemoveRange(context.BilledCases);
                context.SaveChanges();
                context.BillingJobLogs.RemoveRange(context.BillingJobLogs);
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.Cwc_Billing_BillingLineHistoricalFields");
                context.SaveChanges();             
            }
        }
    }
}
