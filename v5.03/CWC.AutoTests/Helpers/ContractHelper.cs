using Cwc.BaseData;
using Cwc.Contracts;
using Cwc.Contracts.Enums;
using Cwc.Contracts.Model;
using Cwc.Security;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CWC.AutoTests.Helpers
{
    public class ContractHelper
    {
        public PriceLine CreatePriceLine(Contract contract, PriceRule priceRule, UnitOfMeasure unitOfMeasure, List<PriceLineLevel> priceLineLevelList,
            decimal units = 1, decimal price = 0,
            decimal? minBillableUnits = null, decimal? maxBillableUnits = null, bool isApplySurcharges = false,
            bool IsDecreasePriceProportionally = true, bool isRangePriceBasedOnTotal = true,
            int? bankAccountID = null, int? debtorID = null)
        {
            var userID = SecurityFacade.LoginService.GetAdministratorLogin().UserID;
            var reference = $"3303{ new Random().Next(1, 9999) }";
            var priceLine = DataFacade.PriceLine.New()
                .With_Contract(contract)
                .With_ContractID(contract.ID)
                .With_PriceRule(priceRule)
                .With_UnitOfMeasure(unitOfMeasure)
                .With_ReferenceCode(reference)
                .With_Description(reference)
                .With_Units(units)
                .With_Price(price)
                .With_MinBillableUnits(minBillableUnits)
                .With_MaxBillableUnits(maxBillableUnits)
                .With_IsApplySurcharges(isApplySurcharges)
                .With_IsDecreasePriceProportionally(IsDecreasePriceProportionally)
                .With_IsRangePriceBasedOnTotal(isRangePriceBasedOnTotal)
                .With_BankAccountID(bankAccountID)
                .With_DebtorID(debtorID)
                .With_AuthorID(userID)
                .With_IsLatestRevision(true)
                .With_PriceLineLevels(priceLineLevelList ?? new List<PriceLineLevel>())
                .SaveToDb();

            return priceLine;
        }

        /// <summary>
        /// Create price line with levels and ranges
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="priceRule"></param>
        /// <param name="unitOfMeasure"></param>
        /// <param name="priceLineLevelList"></param>
        /// <param name="unitsRangeList"></param>
        /// <param name="units"></param>
        /// <param name="price"></param>
        /// <param name="minBillableUnits"></param>
        /// <param name="maxBillableUnits"></param>
        /// <param name="isApplySurcharges"></param>
        /// <param name="IsDecreasePriceProportionally"></param>
        /// <param name="isRangePriceBasedOnTotal"></param>
        /// <param name="bankAccountID"></param>
        /// <param name="debtorID"></param>
        /// <returns></returns>
        public PriceLine CreatePriceLine(Contract contract, PriceRule priceRule, UnitOfMeasure unitOfMeasure, List<PriceLineLevel> priceLineLevelList,
            List<PriceLineUnitsRange> unitsRangeList, decimal units = 1, decimal price = 0,
            decimal? minBillableUnits = null, decimal? maxBillableUnits = null, bool isApplySurcharges = false,
            bool IsDecreasePriceProportionally = true, bool isRangePriceBasedOnTotal = true,
            int? bankAccountID = null, int? debtorID = null)
        {
            var reference = $"3303{ new Random().Next(1, 9999) }";
            var priceLine = DataFacade.PriceLine.New()
                .With_Contract(contract)
                .With_ContractID(contract.ID)
                .With_PriceRule(priceRule)
                .With_UnitOfMeasure(unitOfMeasure)
                .With_ReferenceCode(reference)
                .With_Description(reference)
                .With_Units(units)
                .With_Price(price)
                .With_MinBillableUnits(minBillableUnits)
                .With_MaxBillableUnits(maxBillableUnits)
                .With_IsApplySurcharges(isApplySurcharges)
                .With_IsDecreasePriceProportionally(IsDecreasePriceProportionally)
                .With_IsRangePriceBasedOnTotal(isRangePriceBasedOnTotal)
                .With_BankAccountID(bankAccountID)
                .With_DebtorID(debtorID)
                .With_IsLatestRevision(true)
                .With_PriceLineLevels(priceLineLevelList ?? new List<PriceLineLevel>())
                .With_PriceLineUnitsRanges(unitsRangeList ?? new List<PriceLineUnitsRange>())
                .SaveToDb();

            return priceLine;
        }

        /// <summary>
        /// Configure price line level entity
        /// </summary>
        /// <param name="priceRule">Price rule</param>
        /// <param name="unitOfMeasure">Unit of measure</param>
        /// <param name="levelName">Price rule level name</param>
        /// <param name="valueType">Price rule level value type</param>
        /// <param name="value">ID of "levelName" record</param>
        /// <param name="levelCaption">Code of "levelName" record</param>
        /// <returns></returns>
        public PriceLineLevel BuildPriceLineLevel(PriceRule priceRule, UnitOfMeasure unitOfMeasure, PriceRuleLevelName levelName,
            PriceRuleLevelValueType valueType, decimal? value, string levelCaption, bool isRangeLevel = false,
            object valueFrom = null, object valueTo = null)
        {
            var priceRuleLevelSettingsList = ContractsFacade.PriceRuleLevelSettingService.LoadPriceRuleLevelSettings(priceRule, unitOfMeasure);
            if (!isRangeLevel)
            {
                return DataFacade.PriceLineLevel.New()
                    .With_LevelName(levelName)
                    .With_LevelValueType(valueType)
                    .With_LevelCaption(levelCaption)
                    .With_Value(PriceLineLevel.ConvertValueToDecimal(value, value.GetType()))
                    .With_IsRangeLevel(isRangeLevel)
                    .With_SequenceNumber(priceRuleLevelSettingsList
                        .Where(
                            s => s.LevelName == levelName
                            && s.LevelValueType == valueType
                            && s.IsEnabled)
                        .Single()
                        .SequenceNumber)
                    .Build();
            }
            else
            {
                return DataFacade.PriceLineLevel.New()
                    .With_LevelName(levelName)
                    .With_LevelValueType(valueType)
                    .With_LevelCaption(levelCaption)
                    .With_ValueFrom(PriceLineLevel.ConvertValueToDecimal(valueFrom, valueFrom.GetType()))
                    .With_ValueTo(PriceLineLevel.ConvertValueToDecimal(valueTo, valueTo.GetType()))
                    .With_IsRangeLevel(isRangeLevel)
                    .With_SequenceNumber(priceRuleLevelSettingsList
                        .Where(
                            s => s.LevelName == levelName
                            && s.LevelValueType == valueType
                            && s.IsEnabled)
                        .Single()
                        .SequenceNumber)
                    .Build();
            }
        }

        public PriceLineUnitsRange BuildPriceLineUnitsRange(decimal unitsFrom, decimal unitsTo, decimal price)
        {
            return DataFacade.PriceLineUnitsRange.New()
                .With_UnitsFrom(unitsFrom)
                .With_UnitsTo(unitsTo)
                .With_Price(price)
                .Build();
        }

        public void SaveProportionateDecreasePriceLinesForAllMonths(int serviceTypeID, string serviceTypeCode, Contract contract, decimal units,
            decimal price)
        {
            foreach (var month in Enum.GetValues(typeof(MonthName)))
            {
                var serviceTypeLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerPeriodWithProportionateDecrease,
                UnitOfMeasure.ServiceOrder, PriceRuleLevelName.ServiceType, PriceRuleLevelValueType.ServiceType, serviceTypeID, serviceTypeCode);
                var monthLevel = HelperFacade.ContractHelper.BuildPriceLineLevel(PriceRule.PricePerPeriodWithProportionateDecrease,
                    UnitOfMeasure.ServiceOrder, PriceRuleLevelName.Month, PriceRuleLevelValueType.Month, (int)month, month.ToString());
                HelperFacade.ContractHelper.CreatePriceLine(contract, PriceRule.PricePerPeriodWithProportionateDecrease,
                    UnitOfMeasure.ServiceOrder, new List<PriceLineLevel> { serviceTypeLevel, monthLevel }, units: units, price: price);
            }
        }

        /// <summary>
        /// Delete all price lines with dependent entities in custom contract
        /// </summary>
        /// <param name="contractID">ID of contract</param>
        /// <param name="modelContext">EF context</param>
        public void ClearPriceLines(int contractID, AutomationContractDataContext modelContext = null)
        {
            using (var newContext = new AutomationContractDataContext())
            {
                var context = modelContext != null ? modelContext : newContext;
                var priceLinesQuery = context.PriceLines.Where(p => p.ContractID == contractID);
                var priceLineLevelsQuery = context.PriceLineLevels.Where(l => priceLinesQuery.Any(p => p.ID == l.PriceLineID));
                var priceLineUnitRangesQuery = context.PriceLineUnitsRanges.Where(r => priceLinesQuery.Any(p => p.ID == r.PriceLineID));
                context.PriceLineUnitsRanges.RemoveRange(priceLineUnitRangesQuery);
                context.PriceLineLevels.RemoveRange(priceLineLevelsQuery);
                context.SaveChanges();
                context.PriceLines.RemoveRange(priceLinesQuery);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Re-create ordering settings of main types (COLL, DELV, REPL, SERV, P&P) in custom contract
        /// </summary>
        /// <param name="contract">ID of contract to check</param>        
        public void ReconfigureOrderingSettings(int contractID)
        {
            int collectServiceTypeID, deliverServiceTypeID, replenishmentServiceTypeID, servicingServiceTypeID, pickAndPackServiceTypeID;

            try
            {
                collectServiceTypeID = DataFacade.ServiceType.Take(t => t.Code == "COLL", asNoTracking: true).Build().ID;
                deliverServiceTypeID = DataFacade.ServiceType.Take(t => t.Code == "DELV", asNoTracking: true).Build().ID;
                replenishmentServiceTypeID = DataFacade.ServiceType.Take(t => t.Code == "REPL", asNoTracking: true).Build().ID;
                servicingServiceTypeID = DataFacade.ServiceType.Take(t => t.Code == "SERV", asNoTracking: true).Build().ID;
                pickAndPackServiceTypeID = DataFacade.ServiceType.Take(t => t.OldType == "Pick and pack", asNoTracking: true).Build().ID;
            }
            catch (Exception)
            {
                throw new Exception("Error on matching service types! Please make sure that all required service types are present in database.");
            }

            this.ClearOrderingSettings(contractID);

            // Configure ordering settings for main service types: COLL, DELV, SERV, REPL, P&P
            // Handling COLL type               
            DataFacade.ContractOrderingSetting.New()
                .With_Contract_id(contractID)
                .With_ServiceType_id(collectServiceTypeID)
                .With_LeadTime(0)
                .With_CCLeadTime(0)
                .With_CITLeadTime(0)
                .With_Period(7)
                .With_CutOffTime(0.875)
                .With_AllowTotalPreannouncement(true)
                .With_IsNotes(false)
                .With_IsNotesLooseProductDelivery(false)
                .With_IsCoinsLooseProductDelivery(false)
                .With_IsCoins(false)
                .With_IsConsumables(false)
                .With_IsSpecialCoins(false)
                .With_IsServicingCodes(false)
                .With_IsPreAnnouncement(true)
                .With_AllowCustomerReference(true)
                .With_AllowBankReference(true)
                .With_AllowCitReference(true)
                .With_AllowComments(true)
                .With_AskForAnother(false)
                .With_IsRelease(false)
                .With_EnableProducts(EnableProducts.AllLinked)
                .With_IsLatestRevision(true)
                .SaveToDb();

            // Handling DELV type                
            DataFacade.ContractOrderingSetting.New()
                .With_Contract_id(contractID)
                .With_ServiceType_id(deliverServiceTypeID)
                .With_LeadTime(0)
                .With_CCLeadTime(0)
                .With_CITLeadTime(0)
                .With_Period(7)
                .With_CutOffTime(0.875)
                .With_AllowTotalPreannouncement(true)
                .With_IsNotes(true)
                .With_IsNotesLooseProductDelivery(false)
                .With_IsCoinsLooseProductDelivery(false)
                .With_IsCoins(true)
                .With_IsConsumables(true)
                .With_IsSpecialCoins(true)
                .With_IsServicingCodes(false)
                .With_IsPreAnnouncement(true)
                .With_AllowCustomerReference(true)
                .With_AllowBankReference(true)
                .With_AllowCitReference(true)
                .With_AllowComments(true)
                .With_AskForAnother(false)
                .With_IsRelease(false)
                .With_EnableProducts(EnableProducts.AllExeptLinked)
                .With_IsNoteLooseProductDelivery(true)
                .With_IsCoinLooseProductDelivery(true)
                .With_EnableLooseProducts(EnableProducts.AllLinked)
                .With_IsLatestRevision(true)                
                .SaveToDb();

            // Handling REPL type                
            DataFacade.ContractOrderingSetting.New()
                .With_Contract_id(contractID)
                .With_ServiceType_id(replenishmentServiceTypeID)
                .With_LeadTime(0)
                .With_CCLeadTime(0)
                .With_CITLeadTime(0)
                .With_Period(7)
                .With_CutOffTime(0.875)
                .With_AllowTotalPreannouncement(true)
                .With_IsNotes(true)
                .With_IsCoins(true)
                .With_IsNotesLooseProductDelivery(false)
                .With_IsCoinsLooseProductDelivery(false)
                .With_IsConsumables(true)
                .With_IsSpecialCoins(true)
                .With_IsServicingCodes(true)
                .With_IsPreAnnouncement(true)
                .With_AllowCustomerReference(true)
                .With_AllowBankReference(true)
                .With_AllowCitReference(true)
                .With_AllowComments(true)
                .With_AskForAnother(false)
                .With_IsRelease(false)
                .With_EnableProducts(EnableProducts.AllExeptLinked)
                .With_IsLatestRevision(true)
                .SaveToDb();

            // Handling SERV type                
            DataFacade.ContractOrderingSetting.New()
                .With_Contract_id(contractID)
                .With_ServiceType_id(servicingServiceTypeID)
                .With_LeadTime(0)
                .With_CCLeadTime(0)
                .With_CITLeadTime(0)
                .With_Period(7)
                .With_CutOffTime(0.875)
                .With_AllowTotalPreannouncement(false)
                .With_IsNotes(false)
                .With_IsCoins(false)
                .With_IsNotesLooseProductDelivery(false)
                .With_IsCoinsLooseProductDelivery(false)
                .With_IsConsumables(false)
                .With_IsSpecialCoins(false)
                .With_IsServicingCodes(true)
                .With_IsPreAnnouncement(false)
                .With_AllowCustomerReference(true)
                .With_AllowBankReference(true)
                .With_AllowCitReference(true)
                .With_AllowComments(true)
                .With_AskForAnother(false)
                .With_IsRelease(false)
                .With_EnableProducts(EnableProducts.AllLinked)
                .With_IsLatestRevision(true)
                .SaveToDb();

            // Handling "Pick and pack" type                
            DataFacade.ContractOrderingSetting.New()
                .With_Contract_id(contractID)
                .With_ServiceType_id(pickAndPackServiceTypeID)
                .With_LeadTime(0)
                .With_CCLeadTime(0)
                .With_CITLeadTime(0)
                .With_Period(7)
                .With_CutOffTime(0.875)
                .With_AllowTotalPreannouncement(false)
                .With_IsNotes(false)
                .With_IsCoins(false)
                .With_IsNotesLooseProductDelivery(false)
                .With_IsCoinsLooseProductDelivery(false)
                .With_IsConsumables(false)
                .With_IsSpecialCoins(false)
                .With_IsServicingCodes(false)
                .With_IsPreAnnouncement(false)
                .With_AllowCustomerReference(false)
                .With_AllowBankReference(false)
                .With_AllowCitReference(false)
                .With_AllowComments(false)
                .With_AskForAnother(false)
                .With_IsRelease(false)
                .With_EnableProducts(EnableProducts.AllExeptLinked)
                .With_IsLatestRevision(true)
                .SaveToDb();
        }

        /// <summary>
        /// Delete all ordering settings in custom contract
        /// </summary>
        /// <param name="contractID">ID of contract</param>
        /// <param name="modelContext">EF context</param>
        public void ClearOrderingSettings(int contractID)
        {
            using (var context = new AutomationContractDataContext())
            {
                var orderingSettingsQuery = context.ContractOrderingSetting.Where(s => s.Contract_id == contractID);

                var looseProductsQuery = context.OrderingSettingLooseProductsLinks.Where(l => orderingSettingsQuery.Any(s => s.ID == l.ContractOrderingSettingID));
                var productSettingsQuery = context.ContractProductSettings.Where(p => orderingSettingsQuery.Any(s => s.ID == p.ContractOrderingSettings_id));
                var servicingJobsQuery = context.OrderingSettingServicingJob.Where(j => orderingSettingsQuery.Any(s => s.ID == j.OrderingSettingsId));
                var conversionSettingsQuery = context.OrderingSettingProductConversionSettings.Where(p => orderingSettingsQuery.Any(s => s.ID == p.ContractOrderingSettingID));
                context.OrderingSettingLooseProductsLinks.RemoveRange(looseProductsQuery);
                context.ContractProductSettings.RemoveRange(productSettingsQuery);
                context.OrderingSettingServicingJob.RemoveRange(servicingJobsQuery);
                context.OrderingSettingProductConversionSettings.RemoveRange(conversionSettingsQuery);
                context.ContractOrderingSetting.RemoveRange(orderingSettingsQuery);
                context.SaveChanges();
            }
        }

        public void ConfigureOrderingSettingLooseProductLinkForDeliveryServiceType(int contractID, List<int> productIdList)
        {
            int  deliverServiceTypeID, orderingSettingsDeliveryTypeID;

            try
            {               
                deliverServiceTypeID = DataFacade.ServiceType.Take(t => t.Code == "DELV", asNoTracking: true).Build().ID;               
            }
            catch (Exception)
            {
                throw new Exception("Error on matching service types! Please make sure that all required service types are present in database.");
            }

            try
            {
                orderingSettingsDeliveryTypeID = DataFacade.ContractOrderingSetting.Take(t => t.Contract_id == contractID && t.ServiceType_id == deliverServiceTypeID).Build().ID;
            }
            catch {
                throw new Exception($"Contract Ordering Setting is not found for the contract with ID = {contractID}! Please make sure that the contract ordering settings are present in database.");
            }

            ClearLooseProductLink(orderingSettingsDeliveryTypeID);

            // Configure loose product link for DELV service type
            foreach (var id in productIdList) {
                DataFacade.LooseProductLink.New()
                                .With_ContractOrderingSettingId(orderingSettingsDeliveryTypeID)
                                .With_IsLatestRevisison(true)
                                .With_ProductId(id)
                                .SaveToDb();
            }       
        }

        public void ClearLooseProductLink(int contractID) {

            using (var context = new AutomationContractDataContext())
            {
                var orderingSettingsQuery = context.ContractOrderingSetting.Where(s => s.Contract_id == contractID);

                var looseProductsQuery = context.OrderingSettingLooseProductsLinks.Where(l => orderingSettingsQuery.Any(s => s.ID == l.ContractOrderingSettingID));
                context.OrderingSettingLooseProductsLinks.RemoveRange(looseProductsQuery);
                context.SaveChanges();
            }

        }

        /// <summary>
        /// Configure ordering department lead time settings
        /// </summary>
        /// <param name="location"> Location for matching</param>
        /// <param name="serviceType"> Service type for matching</param>
        /// <param name="currency"> Contract -> currency </param>
        /// <param name="orderingDepartmentLeadTime"> Lead time number</param>
        public void SetLeadTimeSettings(Location location, string serviceType, string currency, int orderingDepartmentLeadTime /*, int CCLeadTime, int CITLeadTime*/)
        {
            ContractOrderingSetting settings = ContractsFacade.ContractOrderingSettingsService.MatchContractOrderingSettings(
                location.CompanyID, DataFacade.ServiceType.Take(st => st.Code == serviceType).Build().ID, null, location.ID, currency, null, null, null, dbParams: null).Value;
            settings.LeadTime = orderingDepartmentLeadTime;
            settings.CCLeadTime = 0;
            settings.CITLeadTime = 0;
            var result = ContractsFacade.ContractOrderingSettingsService.RevisionControlOnUpdate(settings, settings.Contract_id, null, null);
            if (!result.IsSuccess)
            {
                throw new Exception(result.GetMessage());
            }
        }

        /// <summary>
        /// Re-create schedule settings in custom contract
        /// </summary>
        /// <param name="contractID">ID of contract</param>        
        public void ReconfigureScheduleSettings(Contract contract)
        {
            this.ClearScheduleSettings(contract.ID);

            var serviceTypeList = this.ConstructServiceTypeList();
            foreach(var serviceTypeID in serviceTypeList)
            {
                var scheduleSetting = DataFacade.ScheduleSetting.New()
                    .With_Contract_id(contract.ID)
                    .With_ServiceTypeID(serviceTypeID)
                    .With_PeriodStartDate(DateTime.Now.Date)
                    .With_IsLatestRevision(true)
                    .SaveToDb();

                this.ConfigureScheduleLines(scheduleSetting.Build().ID, contract);
            }
        }

        /// <summary>
        /// Create schedule lines of "ad-hoc" and "at-request" types 
        /// </summary>
        /// <param name="scheduleSettingID">ID of parent schedule setting</param>
        private void ConfigureScheduleLines(int scheduleSettingID, Contract contract)
        {
            foreach (var orderType in new List<OrderType> { OrderType.AdHoc, OrderType.AtRequest })
            {
                foreach (var weekday in Enum.GetValues(typeof(WeekdayName)))
                {
                    DataFacade.ScheduleLine.New()
                        .With_Contract(contract)
                        .With_ScheduleSetting_id(scheduleSettingID)
                        .With_IsLatestRevision(true)
                        .With_WeekdayName((WeekdayName)weekday)
                        .With_OrderType(orderType)
                        .With_PeriodNumber(1)
                        .With_PeriodUnits(PeriodUnits.Weeks)
                        .With_SurchargePercentage(0)
                        .With_SurchargeValue(0)
                        .With_CancellationPercentage(0)
                        .With_CancellationValue(0)
                        .SaveToDb();
                }
            }
        }

        /// <summary>
        /// Delete all schedule settings and schedule lines in custom contract
        /// </summary>
        /// <param name="contractID">ID of contract</param>
        /// <param name="context">EF context</param>
        public void ClearScheduleSettings(int contractID)
        {
            using (var context = new AutomationContractDataContext())
            {
                var scheduleSettingsQuery = context.ScheduleSettings.Where(s => s.Contract_id == contractID);
                var scheduleLinesQuery = context.ScheduleLines.Where(l => scheduleSettingsQuery.Any(s => s.ID == l.ScheduleSetting_id));
                context.ScheduleLines.RemoveRange(scheduleLinesQuery);
                context.ScheduleSettings.RemoveRange(scheduleSettingsQuery);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete all schedule schedule lines for particular schedule setting
        /// </summary>
        /// <param name="contractID">ID of schedule setting</param>
        public void ClearScheduleLines(int scheduleSettingID)
        {
            using (var context = new AutomationContractDataContext())
            {
                var scheduleLinesQuery = context.ScheduleLines.Where(l => l.ScheduleSetting_id == scheduleSettingID);
                context.ScheduleLines.RemoveRange(scheduleLinesQuery);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Edit "Effective date" attribute of custom contract
        /// </summary>
        /// <param name="companyContract">Contract to edit</param>
        /// <param name="newDate">New date value</param>
        public void EditEffectiveDateOfContract(Contract companyContract, DateTime newDate)
        {
            companyContract.EffectiveDate = newDate;
            var result = ContractsFacade.ContractService.Save(companyContract, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Editing of contract effective date has failed. Reason: { result.GetMessage() }.");
            }
            return;
        }

        /// <summary>
        /// Edit "End date" attribute of custom contract
        /// </summary>
        /// <param name="companyContract">Contract to edit</param>
        /// <param name="newDate">New date value</param>
        public void EditEndDateOfContract(Contract companyContract, DateTime? newDate)
        {
            companyContract.EndDate = newDate;
            var result = ContractsFacade.ContractService.Save(companyContract, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Editing of contract end date has failed. Reason: { result.GetMessage() }.");
            }
            return;
        }

        public void SaveBankHoliday(Location location, DateTime date)
        {
            var bankHoliday = new BankHolidaySetting { IsDefault = false, Location = location.ID };
        }

        /// <summary>
        /// Construct list of IDs belonging to four main service types used in testing: COLL, DELV, REPL, SERV
        /// </summary>
        /// <returns>List of int IDs</returns>
        private List<int> ConstructServiceTypeList()
        {
            int collectServiceTypeID, deliverServiceTypeID, replenishmentServiceTypeID, servicingServiceTypeID;

            try
            {
                collectServiceTypeID = DataFacade.ServiceType.Take(t => t.Code == "COLL", asNoTracking: true).Build().ID;
                deliverServiceTypeID = DataFacade.ServiceType.Take(t => t.Code == "DELV", asNoTracking: true).Build().ID;
                replenishmentServiceTypeID = DataFacade.ServiceType.Take(t => t.Code == "REPL", asNoTracking: true).Build().ID;
                servicingServiceTypeID = DataFacade.ServiceType.Take(t => t.Code == "SERV", asNoTracking: true).Build().ID;
            }
            catch (Exception)
            {
                throw new Exception("Error on matching service types! Please make sure that all required service types are present in database.");
            }

            return new List<int> { collectServiceTypeID, deliverServiceTypeID, replenishmentServiceTypeID, servicingServiceTypeID };
        }

        public void ClearTestData(AutomationContractDataContext modelContext = null)
        {
            using (var newContext = new AutomationContractDataContext())
            {
                var context = modelContext ?? newContext;
                context.PriceLineLevels.RemoveRange(context.PriceLineLevels);
                context.PriceLineUnitsRanges.RemoveRange(context.PriceLineUnitsRanges);
                context.SaveChanges();
                context.PriceLines.RemoveRange(context.PriceLines);
                context.SaveChanges();
            }
        }
    }
}
