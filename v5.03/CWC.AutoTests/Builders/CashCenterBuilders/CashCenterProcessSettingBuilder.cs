using Cwc.BaseData;
using Cwc.BaseData.Model;
using Cwc.CashCenter;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class CashCenterProcessSettingBuilder
    {
        CashCenterProcessSetting entity;
                    
        public CashCenterProcessSettingBuilder GetSettings(Func<Location, bool> expression)
        {
            Location location;

            using (var context = new AutomationBaseDataContext())
            {
                location = context.Locations.FirstOrDefault(expression);
                if (location == null)
                {
                    throw new InvalidOperationException("Location for specified expression wasn't found");
                }
                entity = CashCenterFacade.ProcessSettingService.MatchCashCenterProcessSetting(null, location.ID, null);
                if (entity == null)
                {
                    throw new InvalidOperationException("Proper process settings weren't found");
                }
                return this;
            }
        }

        public CashCenterProcessSettingBuilder With_Default(bool value)
        {
            if (entity != null)
            {
                entity.SetDefault(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_DateCreated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateCreated(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_DateUpdated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateUpdated(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_ConfirmationUnpacking(ConfirmationUnpackingEnum value)
        {
            if (entity != null)
            {
                entity.ConfirmationUnpacking = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsCapturingAllowed(bool value)
        {
            if (entity != null)
            {
                entity.IsCapturingAllowed = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsCaptureTotals(bool value)
        {
            if (entity != null)
            {
                entity.IsCaptureTotals = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsCaptureWeight(bool value)
        {
            if (entity != null)
            {
                entity.IsCaptureWeight = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsForceBulkCount(bool value)
        {
            if (entity != null)
            {
                entity.IsForceBulkCount = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_AuthorId(int? value)
        {
            if (entity != null)
            {
                entity.SetAuthorId(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_EditorId(int? value)
        {
            if (entity != null)
            {
                entity.SetEditorId(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_CustomerId(decimal? value)
        {
            if (entity != null)
            {
                entity.SetCustomerId(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_LocationTypeCode(string value)
        {
            if (entity != null)
            {
                entity.SetLocationTypeCode(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_LocationId(decimal? value)
        {
            if (entity != null)
            {
                entity.SetLocationId(value);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_VisitAddressId(int value)
        {
            if (entity != null)
            {
                entity.VisitAddressID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }
        public CashCenterProcessSettingBuilder With_VisitAddress(Location value)
        {
            if (entity != null)
            {
                entity.VisitAddress = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_AllocationMethod(AllocationMethod value)
        {
            if (entity != null)
            {
                entity.AllocationMethod = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsApplyStockUnit(bool value)
        {
            if (entity != null)
            {
                entity.IsApplyStockUnit = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsReconcileMotherDeposit(bool value)
        {
            if (entity != null)
            {
                entity.IsReconcileMotherDeposit = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsElectronicPreannouncement(bool value)
        {
            if (entity != null)
            {
                entity.IsElectronicPreannouncement = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_AnomalyCodeID(int? value)
        {
            if (entity != null)
            {
                entity.AnomalyCodeID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsMotherDepositOnly(bool value)
        {
            if (entity != null)
            {
                entity.IsMotherDepositOnly = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsSeparateCapturing(bool value)
        {
            if (entity != null)
            {
                entity.IsSeparateCapturing = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsDeclaredValueMandatory(bool value)
        {
            if (entity != null)
            {
                entity.IsDeclaredValueMandatory = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsDeclaredValuesCounting(bool value)
        {
            if (entity != null)
            {
                entity.IsDeclaredValuesCounting = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsCaptureChequeDetails(bool value)
        {
            if (entity != null)
            {
                entity.IsCaptureChequeDetails = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_PackingBarcodeFormatID(int? value)
        {
            if (entity != null)
            {
                entity.PackingBarcodeFormatID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsBankAccountLeading(bool value)
        {
            if (entity != null)
            {
                entity.IsBankAccountLeading = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsCountInnersDirectly(bool value)
        {
            if (entity != null)
            {
                entity.IsCountInnersDirectly = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsShowReferencesCounting(bool value)
        {
            if (entity != null)
            {
                entity.IsShowReferencesCounting = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsShowInnersCounting(bool value)
        {
            if (entity != null)
            {
                entity.IsShowInnersCounting = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsDeclaredValueMandatoryCapturing(bool value)
        {
            if (entity != null)
            {
                entity.IsDeclaredValueMandatoryCapturing = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsAutomaticallyConfirmCapturing(bool value)
        {
            if (entity != null)
            {
                entity.IsAutomaticallyConfirmCapturing = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsShowCustomerReference(bool value)
        {
            if (entity != null)
            {
                entity.IsShowCustomerReference = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsShowBankReference(bool value)
        {
            if (entity != null)
            {
                entity.IsShowBankReference = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsShowCitReference(bool value)
        {
            if (entity != null)
            {
                entity.IsShowCitReference = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_CustomerReferenceText(string value)
        {
            if (entity != null)
            {
                entity.CustomerReferenceText = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsValidateBankAccount(bool value)
        {
            if (entity != null)
            {
                entity.IsValidateBankAccount = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsShowHolderName(bool value)
        {
            if (entity != null)
            {
                entity.IsShowHolderName = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsValidateBankAccountCounting(bool value)
        {
            if (entity != null)
            {
                entity.IsValidateBankAccountCounting = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsShowHolderNameCounting(bool value)
        {
            if (entity != null)
            {
                entity.IsShowHolderNameCounting = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsBankAccountLeadingCounting(bool value)
        {
            if (entity != null)
            {
                entity.IsBankAccountLeadingCounting = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_IsDisableInnersLocationFrom(bool value)
        {
            if (entity != null)
            {
                entity.IsDisableInnersLocationFrom = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_CashPointTypeId(int value)
        {
            if (entity != null)
            {
                entity.CashPointTypeID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_CashPointType(CashPointType value)
        {
            if (entity != null)
            {
                entity.CashPointType = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder With_ID(int value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CashCenterProcessSettingBuilder New()
        {
            entity = new CashCenterProcessSetting();
            entity.IsShowReferencesCounting = true;
            entity.IsShowInnersCounting = true;
            entity.IsShowCustomerReference = true;
            entity.IsShowBankReference = true;
            entity.IsShowCitReference = true;
            entity.IsDisableInnersLocationFrom = false;
            return this;
        }

        public static implicit operator CashCenterProcessSetting(CashCenterProcessSettingBuilder ins)
        {
            return ins.Build();
        }

        public CashCenterProcessSetting Build()
        {
            return entity;
        }               

        public CashCenterProcessSettingBuilder SaveToDb()
        {           
            var result = CashCenterFacade.ProcessSettingService.Save(entity, null);
            if (!result.IsSuccess)
            {
                throw new Exception($"Error on saving process setting record: {result.GetMessage()}");
            }            
            return this;
        }

        public CashCenterProcessSettingBuilder Take(Expression<Func<CashCenterProcessSetting, bool>> expression)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                entity = context.CashCenterProcessSettings.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new InvalidOperationException("Cash center process setting record was not found.");
                }
            }
            return this;
        }

        public void Delete(Expression<Func<CashCenterProcessSetting, bool>> expression)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                entity = context.CashCenterProcessSettings.FirstOrDefault(expression);
                if (entity == null)
                {
                    throw new InvalidOperationException("Cash center process setting record was not found.");
                }

                var result = CashCenterFacade.ProcessSettingService.Delete(entity, null);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Cash center process setting deletion failed. Reason: {result.GetMessage()}");
                }
            }
        }
    }
}
