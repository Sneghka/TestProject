using Cwc.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.ObjectBuilder
{
    public class DataFacade
    {
        public static BankAccountBuilder BankAccount
        {
            get
            {
                return new BankAccountBuilder();                
            }
        }

        public static BankHolidayBuilder BankHoliday
        {
            get
            {
                return new BankHolidayBuilder();
            }
        }

        public static BankHolidaySettingBuilder BankHolidaySetting
        {
            get
            {
                return new BankHolidaySettingBuilder();
            }
        }

        public static BaseAddressBuilder Address
        {
            get
            {
                return new BaseAddressBuilder();                
            }
        }

        public static CallBuilder Call
        {
            get
            {
                return new CallBuilder();
            }
        }

        public static CashCenterProcessSettingBuilder CashCenterProcessSetting
        {
            get
            {
                return new CashCenterProcessSettingBuilder();
            }
        }

        public static CashCenterSiteSettingBuilder CashCenterSiteSetting
        {
            get
            {
                return new CashCenterSiteSettingBuilder();
            }
        }

        public static CashPointBuilder CashPoint
        {
            get
            {
                return new CashPointBuilder();                
            }
        }

        public static CashPointModelBuilder CashPointModel
        {
            get
            {
                return new CashPointModelBuilder();                
            }
        }

        public static CashPointTypeBuilder CashPointType
        {
            get
            {
                return new CashPointTypeBuilder();
            }
        }

        public static CitProcessingHistoryBuilder CitProcessingHistory
        {
            get
            {
                return new CitProcessingHistoryBuilder();
            }
        }

        public static CitProcessSettingServicingTimeWindowBuilder CitProcessSettingServicingTimeWindow
        {
            get
            {
                return new CitProcessSettingServicingTimeWindowBuilder();
            }
        }

        public static CitProcessingHistoryExceptionBuilder CitProcessingHistoryException
        {
            get
            {
                return new CitProcessingHistoryExceptionBuilder();
            }
        }

        public static CitProcessSettingBuilder CitProcessSettings
        {
            get
            {
                return new CitProcessSettingBuilder();
            }
        }
        public static ContactPersonBuilder ContactPerson
        {
            get
            {
                return new ContactPersonBuilder();
            }
        }

        public static ContainerTypeBuilder ContainerType
        {
            get
            {
                return new ContainerTypeBuilder();                
            }
        }

        public static ContractBuilder Contract
        {
            get
            {
                return new ContractBuilder();                
            }
        }

        public static ContractOrderingSettingBuilder ContractOrderingSetting
        {
            get
            {                
                return new ContractOrderingSettingBuilder();                
            }
        }

        public static ExchangeRateBuilder ExchangeRate
        {
            get
            {
                return new ExchangeRateBuilder();
            }
        }

        public static LooseProductLinkBuilder LooseProductLink
        {
            get
            {
                return new LooseProductLinkBuilder();
            }
        }

        public static OrderImportFormatBJobSettingsBuilder OrderImportFormatBJobSettings
        {
            get
            {
                return new OrderImportFormatBJobSettingsBuilder();
            }
        }

        public static ProductConversionSettingsBuilder ProductConversionSetting
        {
            get
            {
                return new ProductConversionSettingsBuilder();
            }
        }      


        public static ContractProductSettingBuilder ContractProductSetting
        {
            get
            {
                return new ContractProductSettingBuilder();                
            }
        }

        public static CustomerBankAccountBuilder CustomerBankAccount
        {
            get
            {
                return new CustomerBankAccountBuilder();                
            }
        }

        public static CustomerBuilder Customer
        {
            get
            {
                return new CustomerBuilder();                
            }
        }

        public static GroupBuilder Group
        {
            get
            {
                return new GroupBuilder();                
            }
        }

        public static LocationBuilder Location
        {
            get
            {
                return new LocationBuilder();                
            }
        }

        public static LocationTypeBuilder LocationType
        {
            get
            {
                return new LocationTypeBuilder();                
            }
        }

        public static MaterialBuilder Material
        {
            get
            {
                return new MaterialBuilder();                
            }
        }

        public static MasterRouteBuilder MasterRoute
        {
            get
            {
                return new MasterRouteBuilder();                
            }
        }

        public static MasterRouteStopBuilder MasterRouteStop
        {
            get
            {
                return new MasterRouteStopBuilder();                
            }
        }

        public static OrderBuilder Order
        {
            get
            {
                return new OrderBuilder();                
            }
        }

        public static OrderingSettingServicingJobBuilder OrderingSettingServicingJob
        {
            get
            {
                return new OrderingSettingServicingJobBuilder();                
            }
        }

        public static PreannouncementBillingExportSettingGroupBuilder PreannouncementBillingExportSettingGroup
        {
            get
            {
                return new PreannouncementBillingExportSettingGroupBuilder();
            }
        }

        public static PreannouncementBillingExportSettingGroupCompanyLinkBuilder PreannouncementBillingExportSettingGroupCompanyLink
        {
            get
            {
                return new PreannouncementBillingExportSettingGroupCompanyLinkBuilder();
            }
        }

        public static PriceLineBuilder PriceLine
        {
            get
            {
                return new PriceLineBuilder();                
            }
        }

        public static PriceLineLevelBuilder PriceLineLevel
        {
            get
            {
                return new PriceLineLevelBuilder();                
            }
        }

        public static PriceLineUnitsRangeBuilder PriceLineUnitsRange
        {
            get
            {
                return new PriceLineUnitsRangeBuilder();
            }
        }

        public static ProductBuilder Product
        {
            get
            {
                return new ProductBuilder();
            }
        }

        public static ProductGroupBuilder ProductGroup
        {
            get
            {
                return new ProductGroupBuilder();                
            }
        }
        public static ReplicationPartyBuilder ReplicationParty
        {
            get
            {
                return new ReplicationPartyBuilder();
            }
        }

        public static ScheduleLineBuilder ScheduleLine
        {
            get
            {
                return new ScheduleLineBuilder();                
            }
        }

        public static ScheduleSettingBuilder ScheduleSetting
        {
            get
            {
                return new ScheduleSettingBuilder();                
            }
        }

        public static ServiceTypeBuilder ServiceType
        {
            get
            {
                return new ServiceTypeBuilder();                
            }
        }

        public static ServicingCodeBuilder ServicingCode
        {
            get
            {
                return new ServicingCodeBuilder();
                
            }
        }

        public static SiteBuilder Site
        {
            get
            {
                return new SiteBuilder();                
            }
        }        
                
        public static StockContainerBuilder StockContainer
        {
            get
            {
                return new StockContainerBuilder();                
            }
        }

        public static StockLocationBuilder StockLocation
        {
            get
            {
                return new StockLocationBuilder();
            }
        }

        public static StockLocationSettingBuilder StockLocationSetting
        {
            get
            {
                return new StockLocationSettingBuilder();
            }
        }

        public static StockLocationTypeBuilder StockLocationType
        {
            get
            {
                return new StockLocationTypeBuilder();
            }
        }

        public static StockOrderBuilder StockOrder
        {
            get
            {
                return new StockOrderBuilder();                
            }
        }

        public static StockOwnerBuilder StockOwner
        {
            get
            {
                return new StockOwnerBuilder();
            }
        }

        public static StockPositionBuilder StockPosition
        {
            get
            {
                return new StockPositionBuilder();                
            }
        }

        public static StockPositionCashCenterBuilder StockPositionCashCenter
        {
            get
            {
                return new StockPositionCashCenterBuilder();
            }
        }

        public static StreamBuilder Stream
        {
            get
            {
                return new StreamBuilder();
            }
        }

        public static StreamLocationLinkBuilder StreamLocationLink
        {
            get
            {
                return new StreamLocationLinkBuilder();
            }
        }

        public static TransportOrderBuilder TransportOrder
        {
            get
            {
                return new TransportOrderBuilder();                
            }
        }
        
        public static TransportOrderProductBuilder TransportOrderProduct
        {
            get
            {
                return new TransportOrderProductBuilder();                
            }
        }

        public static TransportOrderServBuilder TransportOrderServ
        {
            get
            {
                return new TransportOrderServBuilder();                
            }
        }

        public static WorkstationBuilder Workstation
        {
            get
            {
                return new WorkstationBuilder();
            }
        }
    }
}
