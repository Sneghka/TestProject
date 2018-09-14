using Cwc.BaseData;
using Cwc.BaseData.Model;
using Cwc.Common;
using Cwc.Transport;
using Cwc.Transport.Model;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class CitProcessSettingBuilder
    {
        DataBaseParams _dbParams;
        CitProcessSetting entity;

        public CitProcessSettingBuilder()
        {
            _dbParams = new DataBaseParams();
        }

        public CitProcessSettingBuilder With_BasicStopDuration(Int32 value)
        {
            if (entity != null)
            {
                entity.BasicStopDuration = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_DefaultAdditionalDeliveryServiceType(Int32 value)
        {
            if (entity != null)
            {
                entity.DefaultAdditionalDeliveryServiceTypeId = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_IsAdditionalDeliveryRequired(Boolean value)
        {
            if (entity != null)
            {
                entity.IsAdditionalDeliveryRequired = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_IsDefault(Boolean value)
        {
            if (entity != null)
            {
                entity.IsDefault = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_IsServicing24x7(Boolean value)
        {
            if (entity != null)
            {
                entity.IsServicing24x7 = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_IsNotifyUponReschedule(Boolean value)
        {
            if (entity != null)
            {
                entity.IsNotifyUponReschedule = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }        

        public CitProcessSettingBuilder With_CustomerID(Decimal? value)
        {
            if (entity != null)
            {
                entity.CustomerID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_LocationTypeID(Int32? value)
        {
            if (entity != null)
            {
                entity.LocationTypeID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_ServicePointID(Decimal? value)
        {
            if (entity != null)
            {
                entity.ServicePointID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_ServicePoint(Location value)
        {
            if (entity != null)
            {
                entity.ServicePoint = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_CashPointTypeID(Int32? value)
        {
            if (entity != null)
            {
                entity.CashPointTypeID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_OnHoldDaysNumber(int? value)
        {
            if (entity != null)
            {
                entity.OnHoldDaysNumber = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }
        
        public CitProcessSettingBuilder With_CashPointType(CashPointType value)
        {
            if (entity != null)
            {
                entity.CashPointType = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_VisitAddressID(Decimal? value)
        {
            if (entity != null)
            {
                entity.VisitAddressID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_VisitAddress(Location value)
        {
            if (entity != null)
            {
                entity.VisitAddress = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_DateCreated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateCreated(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_DateUpdated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateUpdated(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_AuthorID(Int32? value)
        {
            if (entity != null)
            {
                entity.SetAuthorID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_EditorID(Int32? value)
        {
            if (entity != null)
            {
                entity.SetEditorID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder With_CitProcessSettingServicingTimeWindow (CitProcessSettingServicingTimeWindow value)
        {
            
            if (entity != null)
            {
                entity.CitProcessSettingServicingTimeWindows.Add(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public CitProcessSettingBuilder New()
        {
            entity = new CitProcessSetting();

            return this;
        }

        public static implicit operator CitProcessSetting(CitProcessSettingBuilder ins)
        {
            return ins.Build();
        }

        public CitProcessSetting Build()
        {
            return entity;
        }

        public CitProcessSettingBuilder SaveToDb()
        {
            var result = TransportFacade.CitProcessSettingService.Save(entity);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Cit Processing Setting saving failed. Reason: {result.GetMessage()}");
            }
            return this;
        }

        public CitProcessSettingBuilder Take(Expression<Func<CitProcessSetting, bool>> expression)
        {
            using (var context = new AutomationTransportDataContext())
            {
                entity = context.CitProcessSettings.FirstOrDefault(expression);

                if (entity == null)
                {
                    throw new ArgumentNullException("Cit Process Settings by provided criteria wasn't found");
                }

            }

            return this;
        }

        public void Delete(Expression<Func<CitProcessSetting, bool>> expression)
        {
            using (var context = new AutomationTransportDataContext())
            {
                var citProcessSettings = context.CitProcessSettings.FirstOrDefault(expression);

                if(citProcessSettings == null)
                {
                    throw new ArgumentNullException("Cit Process Settings by provided criteria wasn't found");
                }

                var result = TransportFacade.CitProcessSettingService.Delete(citProcessSettings);
                if (!result.IsSuccess)
                {
                    throw new InvalidOperationException($"Cit Process Settings deletion failed. Reason: {result.GetMessage()}");
                }
            }
        }

        public CitProcessSettingBuilder MatchByLocation(Location location)
        {
            entity = TransportFacade.CitProcessSettingService.MatchCitProcessSetting(null, location);

            return this;
        }

        public CitProcessSettingBuilder With_ReplenishentConfiguration(int replenishmentServiceType, int? addedCollectionType = null, int? addedDeliveryType = null, int? addedSeervicingType = null)
        {
            var replenishmentConfig = new CitProcessSettingReplenishmentConfiguration
            {
                CitProcessSettingID = entity.ID,
                ReplenishmentTypeID = replenishmentServiceType
            };

            if (addedCollectionType.HasValue)
            {
                replenishmentConfig.CollectionTypeID = addedCollectionType.Value;
            }

            if (addedDeliveryType.HasValue)
            {
                replenishmentConfig.DeliveryTypeID = addedDeliveryType.Value;
            }

            if (addedSeervicingType.HasValue)
            {
                replenishmentConfig.ServicingTypeID = addedSeervicingType.Value;
            }

            var result = TransportFacade.CitProcessSettingReplenishmentConfigurationService.Save(replenishmentConfig);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Replenishment Configuration saving failed. Reason: {result.GetMessage()}");
            }

            return this;
        }

        public CitProcessSettingBuilder RemoveReplenishmentConfigurationFromSetting()
        {
            var matchedReplenConfig = TransportFacade.CitProcessSettingReplenishmentConfigurationService.LoadBy(x => x.CitProcessSettingID == entity.ID);
            if (matchedReplenConfig.Any())
            {
                var replConfig = TransportFacade.CitProcessSettingReplenishmentConfigurationService.DeleteBy(x => x.CitProcessSettingID == entity.ID);
            }
          
            return this;
        }

    }
}