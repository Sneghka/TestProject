using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.CashCenter.Enums;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class CashCenterSiteSettingBuilder
    {
        CashCenterSiteSetting entity;               

        public CashCenterSiteSettingBuilder With_AuthorId(int? value)
        {
            if (entity != null)
            {
                entity.SetAuthorId(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_EditorId(int? value)
        {
            if (entity != null)
            {
                entity.SetEditorId(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_SiteId(int? value)
        {
            if (entity != null)
            {
                entity.SetSiteId(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_DateCreated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateCreated(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_DateUpdated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateUpdated(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_ConfirmationReception(ConfirmationReceptionEnum value)
        {
            if (entity != null)
            {
                entity.ConfirmationReception = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_DualControlReception(bool value)
        {
            if (entity != null)
            {
                entity.DualControlReception = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_DualControlReceptionCompleted(bool value)
        {
            if (entity != null)
            {
                entity.DualControlReceptionCompleted = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_DualControlUnpacking(bool value)
        {
            if (entity != null)
            {
                entity.DualControlUnpacking = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_DualControlCounting(bool value)
        {
            if (entity != null)
            {
                entity.DualControlCounting = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_DualControlCapturing(bool value)
        {
            if (entity != null)
            {
                entity.DualControlCapturing = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_AllowCrossCheck(bool value)
        {
            if (entity != null)
            {
                entity.AllowCrossCheck = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_ValidateCustomerReferences(bool value)
        {
            if (entity != null)
            {
                entity.ValidateCustomerReferences = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_CountingServiceTypeID(int? value)
        {
            if (entity != null)
            {
                entity.CountingServiceTypeID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_OutboundServiceTypeID(int? value)
        {
            if (entity != null)
            {
                entity.OutboundServiceTypeID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_InternalServiceTypeID(int? value)
        {
            if (entity != null)
            {
                entity.InternalServiceTypeID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_DispatchServiceTypeID(int? value)
        {
            if (entity != null)
            {
                entity.DispatchServiceTypeID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_AtmPickList(AtmPickList value)
        {
            if (entity != null)
            {
                entity.AtmPickList = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_InterbankServiceTypeID(int? value)
        {
            if (entity != null)
            {
                entity.InterbankServiceTypeID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsSkipDynamicBatchesGeneration(bool value)
        {
            if (entity != null)
            {
                entity.IsSkipDynamicBatchesGeneration = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_SkipStreamChangeWarning(bool value)
        {
            if (entity != null)
            {
                entity.SkipStreamChangeWarning = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_SkipBulkHCRegistration(bool value)
        {
            if (entity != null)
            {
                entity.SkipBulkHCRegistration = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsRedirectToMother(bool value)
        {
            if (entity != null)
            {
                entity.IsRedirectToMother = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }        

        public CashCenterSiteSettingBuilder With_SiteSubType(int? value)
        {
            if (entity != null)
            {
                entity.SetSiteSubType(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_AlertDiscrepancyZeroValue(bool value)
        {
            if (entity != null)
            {
                entity.AlertDiscrepancyZeroValue = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_NumberFormatID(int? value)
        {
            if (entity != null)
            {
                entity.NumberFormatID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_PermanentNumberFormatID(int? value)
        {
            if (entity != null)
            {
                entity.PermanentNumberFormatID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsPackingBanknotes(bool value)
        {
            if (entity != null)
            {
                entity.IsPackingBanknotes = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsFastWayCapturing(bool value)
        {
            if (entity != null)
            {
                entity.IsFastWayCapturing = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsAllowManualBatches(bool value)
        {
            if (entity != null)
            {
                entity.IsAllowManualBatches = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsAllowMixedBatches(bool value)
        {
            if (entity != null)
            {
                entity.IsAllowMixedBatches = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsReceiveWithoutLocationFrom(bool value)
        {
            if (entity != null)
            {
                entity.IsReceiveWithoutLocationFrom = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsRepackInnersQuickReception(bool value)
        {
            if (entity != null)
            {
                entity.IsRepackInnersQuickReception = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsReconcileRepackCapturing(bool value)
        {
            if (entity != null)
            {
                entity.IsReconcileRepackCapturing = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsReconcileRepackCounting(bool value)
        {
            if (entity != null)
            {
                entity.IsReconcileRepackCounting = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsOverwriteBankAccount(bool value)
        {
            if (entity != null)
            {
                entity.IsOverwriteBankAccount = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsPresortingAllowed(bool value)
        {
            if (entity != null)
            {
                entity.IsPresortingAllowed = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_NumberFormatBanknotesID(int? value)
        {
            if (entity != null)
            {
                entity.NumberFormatBanknotesID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsContainersExpectedBlocking(bool value)
        {
            if (entity != null)
            {
                entity.IsContainersExpectedBlocking = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsContainersInReceptionBlocking(bool value)
        {
            if (entity != null)
            {
                entity.IsContainersInReceptionBlocking = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsContainersInCountingBlocking(bool value)
        {
            if (entity != null)
            {
                entity.IsContainersInCountingBlocking = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsContainersMissingBlocking(bool value)
        {
            if (entity != null)
            {
                entity.IsContainersMissingBlocking = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsContainersOnHoldBlocking(bool value)
        {
            if (entity != null)
            {
                entity.IsContainersOnHoldBlocking = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsBatchesInProgressBlocking(bool value)
        {
            if (entity != null)
            {
                entity.IsBatchesInProgressBlocking = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsStockOwnersUnsealedBlocking(bool value)
        {
            if (entity != null)
            {
                entity.IsStockOwnersUnsealedBlocking = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsDiscrepanciesInProgressBlocking(bool value)
        {
            if (entity != null)
            {
                entity.IsDiscrepanciesInProgressBlocking = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsInboundStockOwnersBlocking(bool value)
        {
            if (entity != null)
            {
                entity.IsInboundStockOwnersBlocking = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsOutboundStockOwnersBlocking(bool value)
        {
            if (entity != null)
            {
                entity.IsOutboundStockOwnersBlocking = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_DayClosureEmails(string value)
        {
            if (entity != null)
            {
                entity.DayClosureEmails = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_DayClosureNotificationInterval(int value)
        {
            if (entity != null)
            {
                entity.DayClosureNotificationInterval = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_AutomaticallyConfirmPacking(bool value)
        {
            if (entity != null)
            {
                entity.AutomaticallyConfirmPacking = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsRepackSecondNumberRequired(bool value)
        {
            if (entity != null)
            {
                entity.IsRepackSecondNumberRequired = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsRepackPermanentNumberRequired(bool value)
        {
            if (entity != null)
            {
                entity.IsRepackPermanentNumberRequired = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsCancelCountAllowed(bool value)
        {
            if (entity != null)
            {
                entity.IsCancelCountAllowed = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsReCountAllowed(bool value)
        {
            if (entity != null)
            {
                entity.IsReCountAllowed = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsCloseFormAllowed(bool value)
        {
            if (entity != null)
            {
                entity.IsCloseFormAllowed = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_PrintCrossCheckBatchReceipt(bool value)
        {
            if (entity != null)
            {
                entity.PrintCrossCheckBatchReceipt = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_AllowSecondStepCounting(int value)
        {
            if (entity != null)
            {
                entity.AllowSecondStepCounting = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_UseGeneralDayClosure(bool value)
        {
            if (entity != null)
            {
                entity.UseGeneralDayClosure = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_AllowSubstringRecognition(bool value)
        {
            if (entity != null)
            {
                entity.AllowSubstringRecognition = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_AllowMultipleMachineCountResults(bool value)
        {
            if (entity != null)
            {
                entity.AllowMultipleMachineCountResults = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsCancelUnpackingAllowed(bool value)
        {
            if (entity != null)
            {
                entity.IsCancelUnpackingAllowed = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsCancelCapturingAllowed(bool value)
        {
            if (entity != null)
            {
                entity.IsCancelCapturingAllowed = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsSkipCountResultsEditing(bool value)
        {
            if (entity != null)
            {
                entity.IsSkipCountResultsEditing = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_LooseProductsOrdersBagTypeID(int? value)
        {
            if (entity != null)
            {
                entity.LooseProductsOrdersBagTypeID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsDualControlManualStockCorrection(bool value)
        {
            if (entity != null)
            {
                entity.IsDualControlManualStockCorrection = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_IsRouteValidation(bool value)
        {
            if (entity != null)
            {
                entity.IsRouteValidation = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_UKBarcodeFirstPosition(int? value)
        {
            if (entity != null)
            {
                entity.UKBarcodeFirstPosition = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_UKBarcodeRangeFrom(int? value)
        {
            if (entity != null)
            {
                entity.UKBarcodeRangeFrom = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterSiteSettingBuilder With_UKBarcodeRangeTo(int? value)
        {
            if (entity != null)
            {
                entity.UKBarcodeRangeTo = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }       

        public CashCenterSiteSettingBuilder With_IsPickListShowWeight(bool value)
        {
            if (entity != null)
            {
                entity.IsPickListShowWeight = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        //public CashCenterSiteSettingBuilder With_IsValidateStockOwnerLooseOnly(bool value)
        //{
        //    if (entity != null)
        //    {
        //        entity.IsValidateStockOwnerLooseOnly = value;
        //        return this;
        //    }
        //    throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        //}

        public CashCenterSiteSettingBuilder With_ID(int value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public CashCenterSiteSettingBuilder New()
        {
            entity = new CashCenterSiteSetting();
            entity.SetSiteType(0); // Cash Center
            entity.IsSkipDynamicBatchesGeneration = false;
            entity.SkipStreamChangeWarning = false;
            entity.SkipBulkHCRegistration = false;
            entity.IsSkipCountResultsEditing = false;
            entity.IsRedirectToMother = false;
            entity.IsPresortingAllowed = false;
            entity.IsCancelUnpackingAllowed = true;
            entity.IsCancelCapturingAllowed = true;
            entity.IsCancelCountAllowed = true;
            entity.IsReCountAllowed = true;
            entity.IsCloseFormAllowed = true;
            entity.IsContainersExpectedBlocking = false;
            entity.IsContainersInReceptionBlocking = false;
            entity.IsContainersInCountingBlocking = false;
            entity.IsContainersOnHoldBlocking = false;
            entity.IsContainersMissingBlocking = false;
            entity.IsBatchesInProgressBlocking = false;
            entity.UseGeneralDayClosure = false;
            entity.AllowSecondStepCounting = (int)AllowSecondStepCounting.OnlyWithoutUsingForm;
            entity.PrintCrossCheckBatchReceipt = false;
            entity.IsDualControlManualStockCorrection = false;
            entity.IsPickListShowWeight = true;
            entity.IsDispatchScanAllowed = false;
            entity.IsAutomaticallyConfirmAssigningToDispatchOrder = false;
            return this;
        }

        public static implicit operator CashCenterSiteSetting(CashCenterSiteSettingBuilder ins)
        {
            return ins.Build();
        }

        public CashCenterSiteSetting Build()
        {
            return entity;
        }

        public CashCenterSiteSettingBuilder SaveToDb()
        {
            var result = CashCenterFacade.SiteSettingService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new Exception($"Error on saving site setting record: {result.GetMessage()}");
            }
            return this;
        }

        public CashCenterSiteSettingBuilder Take(Expression<Func<CashCenterSiteSetting, bool>> condition)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                entity = context.CashCenterSiteSettings.FirstOrDefault(condition);
                if (entity == null)
                {
                    throw new InvalidOperationException("Cash center site setting record was not found.");
                }
            }
            return this;
        }

        public void Delete(Expression<Func<CashCenterSiteSetting, bool>> condition)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                var cashCenterSiteSettings = context.CashCenterSiteSettings.Where(condition).Select(x => x.ID).ToArray();
                if (cashCenterSiteSettings.Length == 0)
                {
                    throw new InvalidOperationException("Cash center site setting records were not found.");
                }               

                var result = CashCenterFacade.SiteSettingService.Delete(cashCenterSiteSettings, null);
                if(!result.IsSuccess)
                {
                    throw new Exception($"Error on deletion site setting records: {result.GetMessage()}");
                }
            }
        }

        public CashCenterSiteSetting GetSettings(Expression<Func<Site, bool>> condition)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                var site = context.Sites.FirstOrDefault(condition);
                if (site == null)
                {
                    throw new InvalidOperationException("Site wasn't found");
                }
                entity = CashCenterFacade.SiteSettingService.MatchCashCenterSiteSettings(site.Branch_nr);

                if (entity == null)
                {
                    throw new InvalidOperationException("Site Settings weren't found");
                }
            }
            return entity;
        }

        public CashCenterSiteSetting CreateChild(Expression<Func<CashCenterSiteSetting, bool>> condition)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                entity = context.CashCenterSiteSettings.FirstOrDefault(condition);
                if (entity == null)
                {
                    throw new InvalidOperationException("Cash center site setting record was not found.");
                }
                var result = CashCenterFacade.SiteSettingService.CreateChild(entity, null);

                return result;
            }
        }
    }
}
