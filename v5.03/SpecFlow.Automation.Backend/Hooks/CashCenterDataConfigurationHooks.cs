using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.CashCenter;
using Cwc.CashCenter.Classes;
using Cwc.Common;
using Cwc.Security;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Linq;
using TechTalk.SpecFlow;
using Z.EntityFramework.Plus;

namespace Specflow.Automation.Backend.Hooks
{
    [Binding, Scope(Tag = "cashcenter-data-generation-required")]
    public class CashCenterDataConfigurationHooks
    {
        #region Configuration strings        
        private const string noteWorkstationName = "automation-note-ws";
        private const string coinWorkstationName = "automation-coin-ws";
        private const string foreignWorkstationName = "automation-foreign-ws";
        private const string bankNoteStockOwnerCode = "bank-note-so";
        private const string bankCoinStockOwnerCode = "bank-coin-so";
        private const string bankForeignStockOwnerCode = "bank-foreign-so";
        private const string defaultStockOwnerCode = "default-so";
        private const string noteSiteInboundStreamName = "automation-note-inbound";
        private const string noteSiteOutboundStreamName = "automation-note-outbound";
        private const string coinSiteInboundStreamName = "automation-coin-inbound";
        private const string coinSiteOutboundStreamName = "automation-coin-outbound";
        private const string foreignSiteInboundStreamName = "automation-foreign-inbound";
        private const string foreignSiteOutboundStreamName = "automation-foreign-outbound";
        private const string noteSiteLevelTypeName = "automation-note-site-type";
        private const string noteOperationalAreaLevelTypeName = "automation-note-operational-area-type";
        private const string noteOperationalLocationLevelTypeName = "automation-note-operational-location-type";
        private const string coinSiteLevelTypeName = "automation-coin-site-type";
        private const string coinOperationalAreaLevelTypeName = "automation-coin-operational-area-type";
        private const string coinOperationalLocationLevelTypeName = "automation-coin-operational-location-type";
        private const string foreignSiteLevelTypeName = "automation-foreign-site-type";
        private const string foreignOperationalAreaLevelTypeName = "automation-foreign-operational-area-type";
        private const string foreignOperationalLocationLevelTypeName = "automation-foreign-operational-location-type";
        private const string noteSiteLevelSLName = "automation-note-site-sl";
        private const string noteOperationalAreaLevelSLName = "automation-note-op-area-sl";
        private const string noteOperationalLocationLevelSLName = "automation-note-op-loc-sl";
        private const string coinSiteLevelSLName = "automation-coin-site-sl";
        private const string coinOperationalAreaLevelSLName = "automation-coin-op-area-sl";
        private const string coinOperationalLocationLevelSLName = "automation-coin-op-loc-sl";
        private const string foreignSiteLevelSLName = "automation-foreign-site-sl";
        private const string foreignOperationalAreaLevelSLName = "automation-foreign-op-area-sl";
        private const string foreignOperationalLocationLevelSLName = "automation-foreign-op-loc-sl";
        private const string notePackingLineName = "Note packing line";
        private const string coinPackingLineName = "Coin packing line";
        private const string foreignPackingLineName = "Foreign packing line";
        #endregion

        #region Configuration entities
        private static DateTime today = DateTime.Today;        
        private static int userId = SecurityFacade.LoginService.GetAdministratorLogin().UserID;
        private static ServiceType countingServiceType = DataFacade.ServiceType.Take(t => t.OldType == "Counting");
        public static ServiceType OutboundServiceType { get; } = DataFacade.ServiceType.Take(t => t.OldType == "Pick and pack");
        private static ServiceType internalServiceType = DataFacade.ServiceType.Take(t => t.OldType == "Internal");
        private static ServiceType dispatchServiceType = DataFacade.ServiceType.Take(t => t.OldType == "Dispatching");
        private static Workstation noteWorkstation;
        private static Workstation coinWorkstation;
        private static Workstation foreignWorkstation;
        private static StockOwner bankNoteStockOwner;
        private static StockOwner bankCoinStockOwner;
        private static StockOwner bankForeignStockOwner;
        private static StockOwner defaultStockOwner;
        private static Stream noteSiteInboundStream;
        private static Stream noteSiteOutboundStream;
        private static Stream coinSiteInboundStream;
        private static Stream coinSiteOutboundStream;
        private static Stream foreignSiteInboundStream;
        private static Stream foreignSiteOutboundStream;
        private static StockLocationType noteSiteLevelType;
        private static StockLocationType noteOperationalAreaLevelType;
        private static StockLocationType noteOperationalLocationLevelType;
        private static StockLocationType coinSiteLevelType;
        private static StockLocationType coinOperationalAreaLevelType;
        private static StockLocationType coinOperationalLocationLevelType;
        private static StockLocationType foreignSiteLevelType;
        private static StockLocationType foreignOperationalAreaLevelType;
        private static StockLocationType foreignOperationalLocationLevelType;
        private static StockLocation noteSiteLevelSL;
        private static StockLocation noteOperationalAreaLevelSL;
        private static StockLocation noteOperationalLocationLevelSL;
        private static StockLocation coinSiteLevelSL;
        private static StockLocation coinOperationalAreaLevelSL;
        private static StockLocation coinOperationalLocationLevelSL;
        private static StockLocation foreignSiteLevelSL;
        private static StockLocation foreignOperationalAreaLevelSL;
        private static StockLocation foreignOperationalLocationLevelSL;
        private static CashCenterSiteSetting noteCCSiteSetting;
        private static CashCenterSiteSetting coinCCSiteSetting;
        private static CashCenterSiteSetting foreignCCSiteSetting;
        private static CashCenterProcessSetting noteCompanyProcessSetting;
        private static CashCenterProcessSetting coinCompanyProcessSetting;
        private static CashCenterProcessSetting foreignCompanyProcessSetting;
        private static PackingLine notePackingLine;
        private static PackingLine coinPackingLine;
        private static PackingLine foreignPackingLine;
        #endregion

        [BeforeFeature(Order = 1)]
        public static void Init()
        {
            if (!BaseDataConfigurationHooks.IsBaseDataConfigured)
            {
                BaseDataConfigurationHooks.ConfigureBaseData();
            }
        }

        [BeforeFeature(Order = 2)]
        public static void ConfigureWorkstations()
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                try
                {
                    noteWorkstation = context.Workstations.AsNoTracking().FirstOrDefault(w => w.Name == noteWorkstationName);
                    if (noteWorkstation == null)
                    {
                        noteWorkstation = SaveWorkstation(noteWorkstationName);
                    }

                    coinWorkstation = context.Workstations.AsNoTracking().FirstOrDefault(w => w.Name == coinWorkstationName);
                    if (coinWorkstation == null)
                    {
                        coinWorkstation = SaveWorkstation(coinWorkstationName);
                    }

                    foreignWorkstation = context.Workstations.AsNoTracking().FirstOrDefault(w => w.Name == foreignWorkstationName);
                    if (foreignWorkstation == null)
                    {
                        foreignWorkstation = SaveWorkstation(foreignWorkstationName);
                    }

                    Workstation SaveWorkstation(string name)
                    {
                        return DataFacade.Workstation.New()
                           .With_AuthorID(userId)
                           .With_EditorID(userId)
                           .With_DateCreated(today)
                           .With_DateUpdated(today)
                           .With_Name(name)
                           .With_Description(name)
                           .SaveToDb();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 3)]
        public static void ConfigureStockOwners()
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                try
                {
                    bankNoteStockOwner = context.StockOwners.AsNoTracking().FirstOrDefault(s => s.Code == bankNoteStockOwnerCode);
                    if (bankNoteStockOwner == null)
                    {
                        bankNoteStockOwner = SaveStockOwner(bankNoteStockOwnerCode, BaseDataConfigurationHooks.NoteStockOwnerCompany.ID);
                    }

                    bankCoinStockOwner = context.StockOwners.AsNoTracking().FirstOrDefault(s => s.Code == bankCoinStockOwnerCode);
                    if (bankCoinStockOwner == null)
                    {
                        bankCoinStockOwner = SaveStockOwner(bankCoinStockOwnerCode, BaseDataConfigurationHooks.CoinStockOwnerCompany.ID);
                    }

                    bankForeignStockOwner = context.StockOwners.AsNoTracking().FirstOrDefault(s => s.Code == bankForeignStockOwnerCode);
                    if (bankForeignStockOwner == null)
                    {
                        bankForeignStockOwner = SaveStockOwner(bankForeignStockOwnerCode, BaseDataConfigurationHooks.ForeignStockOwnerCompany.ID);
                    }

                    defaultStockOwner = context.StockOwners.AsNoTracking().FirstOrDefault(s => s.Code == defaultStockOwnerCode);
                    if (defaultStockOwner == null)
                    {
                        defaultStockOwner = SaveStockOwner(defaultStockOwnerCode, BaseDataConfigurationHooks.CashCenterOwnerCompany.ID);
                    }

                    StockOwner SaveStockOwner(string code, decimal customerId)
                    {
                        return DataFacade.StockOwner.New()
                            .With_Author_id(userId)
                            .With_Editor_id(userId)
                            .With_DateCreated(today)
                            .With_DateUpdated(today)
                            .With_Code(code)
                            .With_ConsignmentTaker(true)
                            .With_MatchingAllowed(true)
                            .With_Customer_id(customerId)
                            .SaveToDb();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }             
        
        [BeforeFeature(Order = 4)]
        public static void ConfigureStockLocationTypes()
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                try
                {
                    noteSiteLevelType = context.StockLocationTypes.AsNoTracking().FirstOrDefault(s => s.Name == noteSiteLevelTypeName);
                    if (noteSiteLevelType == null)
                    {
                        noteSiteLevelType = SaveStockLocationType(noteSiteLevelTypeName, StockLocationTypeLevel.Site, BaseDataConfigurationHooks.NoteCashCenter.ID);
                    }

                    noteOperationalAreaLevelType = context.StockLocationTypes.AsNoTracking().FirstOrDefault(s => s.Name == noteOperationalAreaLevelTypeName);
                    if (noteOperationalAreaLevelType == null)
                    {
                        noteOperationalAreaLevelType = SaveStockLocationType(noteOperationalAreaLevelTypeName, StockLocationTypeLevel.OperationalArea, BaseDataConfigurationHooks.NoteCashCenter.ID);
                    }

                    noteOperationalLocationLevelType = context.StockLocationTypes.AsNoTracking().FirstOrDefault(s => s.Name == noteOperationalLocationLevelTypeName);
                    if (noteOperationalLocationLevelType == null)
                    {
                        noteOperationalLocationLevelType = SaveStockLocationType(noteOperationalLocationLevelTypeName, StockLocationTypeLevel.OperationalLocation, BaseDataConfigurationHooks.NoteCashCenter.ID);
                    }

                    coinSiteLevelType = context.StockLocationTypes.AsNoTracking().FirstOrDefault(s => s.Name == coinSiteLevelTypeName);
                    if (coinSiteLevelType == null)
                    {
                        coinSiteLevelType = SaveStockLocationType(coinSiteLevelTypeName, StockLocationTypeLevel.Site, BaseDataConfigurationHooks.CoinCashCenter.ID);
                    }

                    coinOperationalAreaLevelType = context.StockLocationTypes.AsNoTracking().FirstOrDefault(s => s.Name == coinOperationalAreaLevelTypeName);
                    if (coinOperationalAreaLevelType == null)
                    {
                        coinOperationalAreaLevelType = SaveStockLocationType(coinOperationalAreaLevelTypeName, StockLocationTypeLevel.OperationalArea, BaseDataConfigurationHooks.CoinCashCenter.ID);
                    }

                    coinOperationalLocationLevelType = context.StockLocationTypes.AsNoTracking().FirstOrDefault(s => s.Name == coinOperationalLocationLevelTypeName);
                    if (coinOperationalLocationLevelType == null)
                    {
                        coinOperationalLocationLevelType = SaveStockLocationType(coinOperationalLocationLevelTypeName, StockLocationTypeLevel.OperationalLocation, BaseDataConfigurationHooks.CoinCashCenter.ID);
                    }

                    foreignSiteLevelType = context.StockLocationTypes.AsNoTracking().FirstOrDefault(s => s.Name == foreignSiteLevelTypeName);
                    if (foreignSiteLevelType == null)
                    {
                        foreignSiteLevelType = SaveStockLocationType(foreignSiteLevelTypeName, StockLocationTypeLevel.Site, BaseDataConfigurationHooks.ForeignCashCenter.ID);
                    }

                    foreignOperationalAreaLevelType = context.StockLocationTypes.AsNoTracking().FirstOrDefault(s => s.Name == foreignOperationalAreaLevelTypeName);
                    if (foreignOperationalAreaLevelType == null)
                    {
                        foreignOperationalAreaLevelType = SaveStockLocationType(foreignOperationalAreaLevelTypeName, StockLocationTypeLevel.OperationalArea, BaseDataConfigurationHooks.ForeignCashCenter.ID);
                    }

                    foreignOperationalLocationLevelType = context.StockLocationTypes.AsNoTracking().FirstOrDefault(s => s.Name == foreignOperationalLocationLevelTypeName);
                    if (foreignOperationalLocationLevelType == null)
                    {
                        foreignOperationalLocationLevelType = SaveStockLocationType(foreignOperationalLocationLevelTypeName, StockLocationTypeLevel.OperationalLocation, BaseDataConfigurationHooks.ForeignCashCenter.ID);
                    }

                    StockLocationType SaveStockLocationType(string name, StockLocationTypeLevel level, int siteId)
                    {
                        return DataFacade.StockLocationType.New()
                           .With_AuthorID(userId)
                           .With_EditorID(userId)
                           .With_DateCreated(today)
                           .With_DateUpdated(today)
                           .With_Name(name)
                           .With_Description(name)
                           .With_Level(level)
                           .With_SiteID(siteId)
                           .SaveToDb();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 5)]
        public static void ConfigureStockLocationSettings()
        {
            using (var context = new AutomationCashCenterDataContext())
            {                
                var noteOperationalAreaLevelSLS = DataFacade.StockLocationSetting
                    .Take(s => s.StockLocationTypeID == noteOperationalAreaLevelType.ID)
                    .With_AuthorID(userId)
                    .With_EditorID(userId)
                    .With_DateCreated(today)
                    .With_DateUpdated(today)
                    .With_IsStockOwnerAllowed(true)
                    .SaveToDb();

                var noteOperationalLocationLevelSLS = DataFacade.StockLocationSetting
                    .Take(s => s.StockLocationTypeID == noteOperationalLocationLevelType.ID)
                    .With_AuthorID(userId)
                    .With_EditorID(userId)
                    .With_DateCreated(today)
                    .With_DateUpdated(today)
                    .With_IsStockOwnerAllowed(true)
                    .With_IsWorkstationAllowed(true)
                    .SaveToDb();

                var coinOperationalAreaLevelSLS = DataFacade.StockLocationSetting
                    .Take(s => s.StockLocationTypeID == coinOperationalAreaLevelType.ID)
                    .With_AuthorID(userId)
                    .With_EditorID(userId)
                    .With_DateCreated(today)
                    .With_DateUpdated(today)
                    .With_IsStockOwnerAllowed(true)
                    .SaveToDb();

                var coinOperationalLocationLevelSLS = DataFacade.StockLocationSetting
                    .Take(s => s.StockLocationTypeID == coinOperationalLocationLevelType.ID)
                    .With_AuthorID(userId)
                    .With_EditorID(userId)
                    .With_DateCreated(today)
                    .With_DateUpdated(today)
                    .With_IsStockOwnerAllowed(true)
                    .With_IsWorkstationAllowed(true)
                    .With_IsOpsAreaStock(true)
                    .SaveToDb();

                var foreignOperationalAreaLevelSLS = DataFacade.StockLocationSetting
                    .Take(s => s.StockLocationTypeID == foreignOperationalAreaLevelType.ID)
                    .With_AuthorID(userId)
                    .With_EditorID(userId)
                    .With_DateCreated(today)
                    .With_DateUpdated(today)
                    .With_IsStockOwnerAllowed(true)
                    .SaveToDb();

                var foreignOperationalLocationLevelSLS = DataFacade.StockLocationSetting
                    .Take(s => s.StockLocationTypeID == foreignOperationalLocationLevelType.ID)
                    .With_AuthorID(userId)
                    .With_EditorID(userId)
                    .With_DateCreated(today)
                    .With_DateUpdated(today)
                    .With_IsStockOwnerAllowed(true)
                    .With_IsWorkstationAllowed(true)
                    .SaveToDb();
            }
        }

        [BeforeFeature(Order = 6)]
        public static void ConfigureStockLocations()
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                try
                {
                    noteSiteLevelSL = context.StockLocations.AsNoTracking().FirstOrDefault(s => s.ReferenceNumber == noteSiteLevelSLName);
                    if (noteSiteLevelSL == null)
                    {
                        noteSiteLevelSL = DataFacade.StockLocation.New()
                           .With_Author_id(userId)
                           .With_Editor_id(userId)                           
                           .With_ReferenceNumber(noteSiteLevelSLName)
                           .With_Description(noteSiteLevelSLName)                           
                           .With_Site_id(BaseDataConfigurationHooks.NoteCashCenter.ID)
                           .With_StockLocationTypeID(noteSiteLevelType.ID)
                           .With_DefaultStockOwnerID(defaultStockOwner.ID)
                           .With_DateUpdated(today)
                           .SaveToDb();
                    }

                    noteOperationalAreaLevelSL = context.StockLocations.AsNoTracking().FirstOrDefault(s => s.ReferenceNumber == noteOperationalAreaLevelSLName);
                    if (noteOperationalAreaLevelSL == null)
                    {
                        noteOperationalAreaLevelSL = DataFacade.StockLocation.New()
                           .With_Author_id(userId)
                           .With_Editor_id(userId)
                           .With_ReferenceNumber(noteOperationalAreaLevelSLName)
                           .With_Description(noteOperationalAreaLevelSLName)
                           .With_Site_id(BaseDataConfigurationHooks.NoteCashCenter.ID)
                           .With_StockLocationTypeID(noteOperationalAreaLevelType.ID)
                           .With_StockOwner_id(bankNoteStockOwner.ID)
                           .With_Location_id(BaseDataConfigurationHooks.NoteCashCenter.Loc_nr)
                           .With_DateUpdated(today)
                           .SaveToDb();
                    }

                    noteOperationalLocationLevelSL = context.StockLocations.AsNoTracking().FirstOrDefault(s => s.ReferenceNumber == noteOperationalLocationLevelSLName);
                    if (noteOperationalLocationLevelSL == null)
                    {
                        noteOperationalLocationLevelSL = DataFacade.StockLocation.New()
                           .With_Author_id(userId)
                           .With_Editor_id(userId)
                           .With_ReferenceNumber(noteOperationalLocationLevelSLName)
                           .With_Description(noteOperationalLocationLevelSLName)
                           .With_Site_id(BaseDataConfigurationHooks.NoteCashCenter.ID)
                           .With_StockLocationTypeID(noteOperationalLocationLevelType.ID)
                           .With_StockOwner_id(bankNoteStockOwner.ID)                           
                           .With_ParentStockLocation_id(noteOperationalAreaLevelSL.ID)
                           .With_Workstation_id(noteWorkstation.ID)
                           .With_DateUpdated(today)
                           .SaveToDb();
                    }

                    coinSiteLevelSL = context.StockLocations.AsNoTracking().FirstOrDefault(s => s.ReferenceNumber == coinSiteLevelSLName);
                    if (coinSiteLevelSL == null)
                    {
                        coinSiteLevelSL = DataFacade.StockLocation.New()
                           .With_Author_id(userId)
                           .With_Editor_id(userId)
                           .With_ReferenceNumber(coinSiteLevelSLName)
                           .With_Description(coinSiteLevelSLName)
                           .With_Site_id(BaseDataConfigurationHooks.CoinCashCenter.ID)
                           .With_StockLocationTypeID(coinSiteLevelType.ID)
                           .With_DefaultStockOwnerID(defaultStockOwner.ID)
                           .With_DateUpdated(today)
                           .SaveToDb();
                    }

                    coinOperationalAreaLevelSL = context.StockLocations.AsNoTracking().FirstOrDefault(s => s.ReferenceNumber == coinOperationalAreaLevelSLName);
                    if (coinOperationalAreaLevelSL == null)
                    {
                        coinOperationalAreaLevelSL = DataFacade.StockLocation.New()
                           .With_Author_id(userId)
                           .With_Editor_id(userId)
                           .With_ReferenceNumber(coinOperationalAreaLevelSLName)
                           .With_Description(coinOperationalAreaLevelSLName)
                           .With_Site_id(BaseDataConfigurationHooks.CoinCashCenter.ID)
                           .With_StockLocationTypeID(coinOperationalAreaLevelType.ID)
                           .With_StockOwner_id(bankCoinStockOwner.ID)
                           .With_Location_id(BaseDataConfigurationHooks.CoinCashCenter.Loc_nr)
                           .With_DateUpdated(today)
                           .SaveToDb();
                    }

                    coinOperationalLocationLevelSL = context.StockLocations.AsNoTracking().FirstOrDefault(s => s.ReferenceNumber == coinOperationalLocationLevelSLName);
                    if (coinOperationalLocationLevelSL == null)
                    {
                        coinOperationalLocationLevelSL = DataFacade.StockLocation.New()
                           .With_Author_id(userId)
                           .With_Editor_id(userId)
                           .With_ReferenceNumber(coinOperationalLocationLevelSLName)
                           .With_Description(coinOperationalLocationLevelSLName)
                           .With_Site_id(BaseDataConfigurationHooks.CoinCashCenter.ID)
                           .With_StockLocationTypeID(coinOperationalLocationLevelType.ID)
                           .With_StockOwner_id(bankCoinStockOwner.ID)                           
                           .With_ParentStockLocation_id(coinOperationalAreaLevelSL.ID)
                           .With_Workstation_id(coinWorkstation.ID)
                           .With_DateUpdated(today)
                           .SaveToDb();
                    }

                    foreignSiteLevelSL = context.StockLocations.AsNoTracking().FirstOrDefault(s => s.ReferenceNumber == foreignSiteLevelSLName);
                    if (foreignSiteLevelSL == null)
                    {
                        foreignSiteLevelSL = DataFacade.StockLocation.New()
                           .With_Author_id(userId)
                           .With_Editor_id(userId)
                           .With_ReferenceNumber(foreignSiteLevelSLName)
                           .With_Description(foreignSiteLevelSLName)
                           .With_Site_id(BaseDataConfigurationHooks.ForeignCashCenter.ID)
                           .With_StockLocationTypeID(foreignSiteLevelType.ID)
                           .With_DefaultStockOwnerID(defaultStockOwner.ID)
                           .With_DateUpdated(today)
                           .SaveToDb();
                    }

                    foreignOperationalAreaLevelSL = context.StockLocations.AsNoTracking().FirstOrDefault(s => s.ReferenceNumber == foreignOperationalAreaLevelSLName);
                    if (foreignOperationalAreaLevelSL == null)
                    {
                        foreignOperationalAreaLevelSL = DataFacade.StockLocation.New()
                           .With_Author_id(userId)
                           .With_Editor_id(userId)
                           .With_ReferenceNumber(foreignOperationalAreaLevelSLName)
                           .With_Description(foreignOperationalAreaLevelSLName)
                           .With_Site_id(BaseDataConfigurationHooks.ForeignCashCenter.ID)
                           .With_StockLocationTypeID(foreignOperationalAreaLevelType.ID)
                           .With_StockOwner_id(bankForeignStockOwner.ID)
                           .With_Location_id(BaseDataConfigurationHooks.ForeignCashCenter.Loc_nr)
                           .With_DateUpdated(today)
                           .SaveToDb();
                    }

                    foreignOperationalLocationLevelSL = context.StockLocations.AsNoTracking().FirstOrDefault(s => s.ReferenceNumber == foreignOperationalLocationLevelSLName);
                    if (foreignOperationalLocationLevelSL == null)
                    {
                        foreignOperationalLocationLevelSL = DataFacade.StockLocation.New()
                           .With_Author_id(userId)
                           .With_Editor_id(userId)
                           .With_ReferenceNumber(foreignOperationalLocationLevelSLName)
                           .With_Description(foreignOperationalLocationLevelSLName)
                           .With_Site_id(BaseDataConfigurationHooks.ForeignCashCenter.ID)
                           .With_StockLocationTypeID(foreignOperationalLocationLevelType.ID)
                           .With_StockOwner_id(bankForeignStockOwner.ID)                           
                           .With_ParentStockLocation_id(foreignOperationalAreaLevelSL.ID)
                           .With_Workstation_id(foreignWorkstation.ID)
                           .With_DateUpdated(today)
                           .SaveToDb();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 7)]
        public static void ConfigureStreams()
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                try
                {
                    noteSiteInboundStream = context.Streams.AsNoTracking().FirstOrDefault(s => s.Name == noteSiteInboundStreamName);
                    if (noteSiteInboundStream == null)
                    {
                        noteSiteInboundStream = DataFacade.Stream.New()
                            .With_AuthorID(userId)
                            .With_EditorID(userId)
                            .With_DateCreated(today)
                            .With_DateUpdated(today)
                            .With_Name(noteSiteInboundStreamName)
                            .With_Description(noteSiteInboundStreamName)
                            .With_Process(StreamProcess.Inbound)
                            .With_StockOwner_id(bankNoteStockOwner.ID)
                            .With_Site_id(BaseDataConfigurationHooks.NoteCashCenter.ID)                            
                            .With_DestinationStockLocationID(noteOperationalAreaLevelSL.ID)
                            .SaveToDb();
                    }

                    noteSiteOutboundStream = context.Streams.AsNoTracking().FirstOrDefault(s => s.Name == noteSiteOutboundStreamName);
                    if (noteSiteOutboundStream == null)
                    {
                        noteSiteOutboundStream = DataFacade.Stream.New()
                            .With_AuthorID(userId)
                            .With_EditorID(userId)
                            .With_DateCreated(today)
                            .With_DateUpdated(today)
                            .With_Name(noteSiteOutboundStreamName)
                            .With_Description(noteSiteOutboundStreamName)
                            .With_Process(StreamProcess.Outbound)
                            .With_StockOwner_id(bankNoteStockOwner.ID)
                            .With_Site_id(BaseDataConfigurationHooks.NoteCashCenter.ID)                            
                            .SaveToDb();
                    }

                    coinSiteInboundStream = context.Streams.AsNoTracking().FirstOrDefault(s => s.Name == coinSiteInboundStreamName);
                    if (coinSiteInboundStream == null)
                    {
                        coinSiteInboundStream = DataFacade.Stream.New()
                            .With_AuthorID(userId)
                            .With_EditorID(userId)
                            .With_DateCreated(today)
                            .With_DateUpdated(today)
                            .With_Name(coinSiteInboundStreamName)
                            .With_Description(coinSiteInboundStreamName)
                            .With_Process(StreamProcess.Inbound)
                            .With_StockOwner_id(bankCoinStockOwner.ID)
                            .With_Site_id(BaseDataConfigurationHooks.CoinCashCenter.ID)                            
                            .With_DestinationStockLocationID(coinOperationalAreaLevelSL.ID)
                            .SaveToDb();
                    }

                    coinSiteOutboundStream = context.Streams.AsNoTracking().FirstOrDefault(s => s.Name == coinSiteOutboundStreamName);
                    if (coinSiteOutboundStream == null)
                    {
                        coinSiteOutboundStream = DataFacade.Stream.New()
                            .With_AuthorID(userId)
                            .With_EditorID(userId)
                            .With_DateCreated(today)
                            .With_DateUpdated(today)
                            .With_Name(coinSiteOutboundStreamName)
                            .With_Description(coinSiteOutboundStreamName)
                            .With_Process(StreamProcess.Outbound)
                            .With_StockOwner_id(bankCoinStockOwner.ID)
                            .With_Site_id(BaseDataConfigurationHooks.CoinCashCenter.ID)
                            .SaveToDb();
                    }

                    foreignSiteInboundStream = context.Streams.AsNoTracking().FirstOrDefault(s => s.Name == foreignSiteInboundStreamName);
                    if (foreignSiteInboundStream == null)
                    {
                        foreignSiteInboundStream = DataFacade.Stream.New()
                            .With_AuthorID(userId)
                            .With_EditorID(userId)
                            .With_DateCreated(today)
                            .With_DateUpdated(today)
                            .With_Name(foreignSiteInboundStreamName)
                            .With_Description(foreignSiteInboundStreamName)
                            .With_Process(StreamProcess.Inbound)
                            .With_StockOwner_id(bankForeignStockOwner.ID)
                            .With_Site_id(BaseDataConfigurationHooks.ForeignCashCenter.ID)                            
                            .With_DestinationStockLocationID(foreignOperationalAreaLevelSL.ID)
                            .SaveToDb();
                    }

                    foreignSiteOutboundStream = context.Streams.AsNoTracking().FirstOrDefault(s => s.Name == foreignSiteOutboundStreamName);
                    if (foreignSiteOutboundStream == null)
                    {
                        foreignSiteOutboundStream = DataFacade.Stream.New()
                            .With_AuthorID(userId)
                            .With_EditorID(userId)
                            .With_DateCreated(today)
                            .With_DateUpdated(today)
                            .With_Name(foreignSiteOutboundStreamName)
                            .With_Description(foreignSiteOutboundStreamName)
                            .With_Process(StreamProcess.Outbound)
                            .With_StockOwner_id(bankForeignStockOwner.ID)
                            .With_Site_id(BaseDataConfigurationHooks.ForeignCashCenter.ID)
                            .SaveToDb();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 8)]
        public static void ConfigureStreamLocationLinks()
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                try
                {
                    var noteLocation1Id = BaseDataConfigurationHooks.NoteLocation1.ID;
                    var noteLocation2Id = BaseDataConfigurationHooks.NoteLocation2.ID;
                    var coinLocation1Id = BaseDataConfigurationHooks.CoinLocation1.ID;
                    var coinLocation2Id = BaseDataConfigurationHooks.CoinLocation2.ID;
                    var foreignLocationID = BaseDataConfigurationHooks.ForeignLocation.ID;
                    var noteCoinLocationID = BaseDataConfigurationHooks.NoteCoinLocation.ID;
                    var noteForeignLocationID = BaseDataConfigurationHooks.NoteForeignLocation.ID;
                    var coinForeignLocationID = BaseDataConfigurationHooks.CoinForeignLocation.ID;
                    var noteCoinForeignLocationID = BaseDataConfigurationHooks.NoteCoinForeignLocation.ID;
                    var outOfServiceLocationID = BaseDataConfigurationHooks.OutOfServiceLocation.ID;
                    var externalLocationID = BaseDataConfigurationHooks.ExternalLocation.ID;
                    var noteSiteInboundStreamID = noteSiteInboundStream.ID;
                    var noteSiteOutboundStreamID = noteSiteOutboundStream.ID;
                    var coinSiteInboundStreamID = coinSiteInboundStream.ID;
                    var coinSiteOutboundStreamID = coinSiteOutboundStream.ID;
                    var foreignSiteInboundStreamID = foreignSiteInboundStream.ID;
                    var foreignSiteOutboundStreamID = foreignSiteOutboundStream.ID;

                    // Note site inbound stream
                    if (!context.StreamLocationLinks.Any(l => l.StreamID == noteSiteInboundStreamID))
                    {
                        SaveStreamLocationLink(noteLocation1Id, noteSiteInboundStreamID);                    
                        SaveStreamLocationLink(noteLocation2Id, noteSiteInboundStreamID);
                        SaveStreamLocationLink(noteCoinLocationID, noteSiteInboundStreamID);
                        SaveStreamLocationLink(noteForeignLocationID, noteSiteInboundStreamID);
                        SaveStreamLocationLink(noteCoinForeignLocationID, noteSiteInboundStreamID);
                    }
                    // Note site outbound stream
                    if (!context.StreamLocationLinks.Any(l => l.StreamID == noteSiteOutboundStreamID))
                    {
                        SaveStreamLocationLink(noteLocation1Id, noteSiteOutboundStreamID);
                        SaveStreamLocationLink(noteLocation2Id, noteSiteOutboundStreamID);
                        SaveStreamLocationLink(noteCoinLocationID, noteSiteOutboundStreamID);
                        SaveStreamLocationLink(noteForeignLocationID, noteSiteOutboundStreamID);
                        SaveStreamLocationLink(noteCoinForeignLocationID, noteSiteOutboundStreamID);
                    }
                    // Coin site inbound stream
                    if (!context.StreamLocationLinks.Any(l => l.StreamID == coinSiteInboundStreamID))
                    {
                        SaveStreamLocationLink(coinLocation1Id, coinSiteInboundStreamID);
                        SaveStreamLocationLink(coinLocation2Id, coinSiteInboundStreamID);
                        SaveStreamLocationLink(noteCoinLocationID, coinSiteInboundStreamID);
                        SaveStreamLocationLink(coinForeignLocationID, coinSiteInboundStreamID);
                        SaveStreamLocationLink(noteCoinForeignLocationID, coinSiteInboundStreamID);
                    }
                    // Coin site outbound stream
                    if (!context.StreamLocationLinks.Any(l => l.StreamID == coinSiteOutboundStreamID))
                    {
                        SaveStreamLocationLink(coinLocation1Id, coinSiteOutboundStreamID);
                        SaveStreamLocationLink(coinLocation2Id, coinSiteOutboundStreamID);
                        SaveStreamLocationLink(noteCoinLocationID, coinSiteOutboundStreamID);
                        SaveStreamLocationLink(coinForeignLocationID, coinSiteOutboundStreamID);
                        SaveStreamLocationLink(noteCoinForeignLocationID, coinSiteOutboundStreamID);
                    }
                    // Foreign site inbound stream
                    if (!context.StreamLocationLinks.Any(l => l.StreamID == foreignSiteInboundStreamID))
                    {
                        SaveStreamLocationLink(foreignLocationID, foreignSiteInboundStreamID);
                        SaveStreamLocationLink(noteForeignLocationID, foreignSiteInboundStreamID);
                        SaveStreamLocationLink(coinForeignLocationID, foreignSiteInboundStreamID);
                        SaveStreamLocationLink(noteCoinForeignLocationID, foreignSiteInboundStreamID);
                    }
                    // Foreign site outbound stream
                    if (!context.StreamLocationLinks.Any(l => l.StreamID == foreignSiteOutboundStreamID))
                    {
                        SaveStreamLocationLink(foreignLocationID, foreignSiteOutboundStreamID);
                        SaveStreamLocationLink(noteForeignLocationID, foreignSiteOutboundStreamID);
                        SaveStreamLocationLink(coinForeignLocationID, foreignSiteOutboundStreamID);
                        SaveStreamLocationLink(noteCoinForeignLocationID, foreignSiteOutboundStreamID);
                    }

                    void SaveStreamLocationLink(decimal locationId, int streamId)
                    {
                        DataFacade.StreamLocationLink.New()
                            .With_Location_id(locationId)
                            .With_StreamID(streamId)
                            .SaveToDb();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 9)]
        public static void ConfigureCashCenterSiteSettings()
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                try
                {
                    noteCCSiteSetting = context.CashCenterSiteSettings.AsNoTracking().FirstOrDefault(s => s.SiteId == BaseDataConfigurationHooks.NoteCashCenter.ID);
                    if (noteCCSiteSetting == null)
                    {
                        noteCCSiteSetting = SaveCashCenterSiteSetting(0, BaseDataConfigurationHooks.NoteCashCenter.ID); // 0 = Notes
                    }

                    coinCCSiteSetting = context.CashCenterSiteSettings.AsNoTracking().FirstOrDefault(s => s.SiteId == BaseDataConfigurationHooks.CoinCashCenter.ID);
                    if (coinCCSiteSetting == null)
                    {
                        coinCCSiteSetting = SaveCashCenterSiteSetting(1, BaseDataConfigurationHooks.CoinCashCenter.ID); // 1 = Coins
                    }

                    foreignCCSiteSetting = context.CashCenterSiteSettings.AsNoTracking().FirstOrDefault(s => s.SiteId == BaseDataConfigurationHooks.ForeignCashCenter.ID);
                    if (foreignCCSiteSetting == null)
                    {
                        foreignCCSiteSetting = SaveCashCenterSiteSetting(2, BaseDataConfigurationHooks.ForeignCashCenter.ID); // 2 = Notes and coins
                    }

                    CashCenterSiteSetting SaveCashCenterSiteSetting(int subType, int siteId)
                    {
                        return  DataFacade.CashCenterSiteSetting.New()
                            .With_AuthorId(userId)
                            .With_EditorId(userId)
                            .With_DateCreated(today)
                            .With_DateUpdated(today)
                            .With_SiteSubType(subType)
                            .With_SiteId(siteId)
                            .With_CountingServiceTypeID(countingServiceType.ID)
                            .With_OutboundServiceTypeID(OutboundServiceType.ID)
                            .With_InternalServiceTypeID(internalServiceType.ID)
                            .With_DispatchServiceTypeID(dispatchServiceType.ID)
                            .With_AtmPickList(Cwc.CashCenter.Enums.AtmPickList.GeneratePerPackage)
                            .With_LooseProductsOrdersBagTypeID(BaseDataConfigurationHooks.LooseProductsBagType.ID)
                            .SaveToDb();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 10)]
        public static void ConfigureCashCenterSiteSettingLinks()
        {            
            string[] codes = { "NOTE", "COIN", "CHEQUE" };
            var noteCCSiteSettingID = noteCCSiteSetting.ID;
            var coinCCSiteSettingID = coinCCSiteSetting.ID;
            var foreignCCSiteSettingID = foreignCCSiteSetting.ID;
            
            using (var context = new AutomationCashCenterDataContext())
            {
                try
                {
                    // Material type links
                    if (!context.CashCenterSiteSettingMaterialTypeLinks.Any(l => l.CashCenterSiteSettingID == noteCCSiteSettingID))
                    {
                        foreach (var code in codes)
                        {
                            AddCashCenterSiteSettingMaterialTypeLink(code, noteCCSiteSettingID);
                        }                        
                    }
                    if (!context.CashCenterSiteSettingMaterialTypeLinks.Any(l => l.CashCenterSiteSettingID == coinCCSiteSettingID))
                    {
                        foreach (var code in codes)
                        {
                            AddCashCenterSiteSettingMaterialTypeLink(code, coinCCSiteSettingID);
                        }
                    }
                    if (!context.CashCenterSiteSettingMaterialTypeLinks.Any(l => l.CashCenterSiteSettingID == foreignCCSiteSettingID))
                    {                        
                        foreach (var code in codes)
                        {
                            AddCashCenterSiteSettingMaterialTypeLink(code, foreignCCSiteSettingID);
                        }
                    }

                    void AddCashCenterSiteSettingMaterialTypeLink(string code, int siteSettingId)
                    {
                        var link = new CashCenterSiteSettingMaterialTypeLink
                        {
                            MaterialTypeCode = code,
                            CashCenterSiteSettingID = siteSettingId
                        };
                        
                        context.CashCenterSiteSettingMaterialTypeLinks.Add(link);
                    }

                    // Qualification type links
                    var types = new QualificationType[4] { QualificationType.Fit, QualificationType.Unfit, QualificationType.Rejected, QualificationType.Counterfeit };

                    if (!context.CashCenterSiteSettingQualificationTypes.Any(l => l.CashCenterSiteSettingID == noteCCSiteSettingID))
                    {                        
                        foreach (var type in types)
                        {
                            AddCashCenterSiteSettingQualificationType(type, noteCCSiteSettingID);
                        }
                    }
                    if (!context.CashCenterSiteSettingQualificationTypes.Any(l => l.CashCenterSiteSettingID == coinCCSiteSettingID))
                    {
                        foreach (var type in types)
                        {
                            AddCashCenterSiteSettingQualificationType(type, coinCCSiteSettingID);
                        }
                    }
                    if (!context.CashCenterSiteSettingQualificationTypes.Any(l => l.CashCenterSiteSettingID == foreignCCSiteSettingID))
                    {
                        foreach (var type in types)
                        {
                            AddCashCenterSiteSettingQualificationType(type, foreignCCSiteSettingID);
                        }
                    }

                    void AddCashCenterSiteSettingQualificationType(QualificationType type, int siteSettingId)
                    {
                        var link = new CashCenterSiteSettingQualificationType
                        {
                            QualificationType = (int)type,
                            IsMachineCountResult = type == QualificationType.Fit ? true : false,
                            IsBanknoteRegistration = type == QualificationType.Counterfeit ? true : false
                        };
                        link.SetCashCenterSiteSettingID(siteSettingId);
                        context.CashCenterSiteSettingQualificationTypes.Add(link);
                    }

                    // Transport sticker links
                    if (!context.CashCenterSiteSettingBagTypeLinks.Any(l => l.CashCenterSiteSettingID == noteCCSiteSettingID))
                    {
                        AddCashCenterSiteSettingBagTypeLink(noteCCSiteSettingID);
                    }
                    if (!context.CashCenterSiteSettingBagTypeLinks.Any(l => l.CashCenterSiteSettingID == coinCCSiteSettingID))
                    {
                        AddCashCenterSiteSettingBagTypeLink(coinCCSiteSettingID);
                    }
                    if (!context.CashCenterSiteSettingBagTypeLinks.Any(l => l.CashCenterSiteSettingID == foreignCCSiteSettingID))
                    {
                        AddCashCenterSiteSettingBagTypeLink(foreignCCSiteSettingID);
                    }

                    void AddCashCenterSiteSettingBagTypeLink(int siteSettingId)
                    {
                        var transportStickerLink = new CashCenterSiteSettingBagTypeLink
                        {
                            PrintTransportStickers = (int)PrintTransportStickers.AtPickingOnly,
                            TransportStickerType = (int)TransportStickerType.TransportSticker,
                            IsLooseProducts = false,
                            IsDefault = true,
                            CashCenterSiteSettingID = siteSettingId,
                            StickerText = "test",
                            IsShowReference = true,
                            IsShowValue = true
                        };
                        context.CashCenterSiteSettingBagTypeLinks.Add(transportStickerLink);
                    }

                    context.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 11)]
        public static void ConfigureCashCenterProcessSettings()
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                try
                {
                    noteCompanyProcessSetting = context.CashCenterProcessSettings.AsNoTracking().FirstOrDefault(s => s.CustomerId == BaseDataConfigurationHooks.NoteCompany.ID);
                    if (noteCompanyProcessSetting == null)
                    {
                        noteCompanyProcessSetting = DataFacade.CashCenterProcessSetting.New()
                            .With_AuthorId(userId)
                            .With_EditorId(userId)                            
                            .With_CustomerId(BaseDataConfigurationHooks.NoteCompany.ID)                            
                            .SaveToDb();
                    }

                    coinCompanyProcessSetting = context.CashCenterProcessSettings.AsNoTracking().FirstOrDefault(s => s.CustomerId == BaseDataConfigurationHooks.CoinCompany.ID);
                    if (coinCompanyProcessSetting == null)
                    {
                        coinCompanyProcessSetting = DataFacade.CashCenterProcessSetting.New()
                            .With_AuthorId(userId)
                            .With_EditorId(userId)
                            .With_CustomerId(BaseDataConfigurationHooks.CoinCompany.ID)
                            .SaveToDb();
                    }

                    foreignCompanyProcessSetting = context.CashCenterProcessSettings.AsNoTracking().FirstOrDefault(s => s.CustomerId == BaseDataConfigurationHooks.ForeignCompany.ID);
                    if (foreignCompanyProcessSetting == null)
                    {
                        foreignCompanyProcessSetting = DataFacade.CashCenterProcessSetting.New()
                            .With_AuthorId(userId)
                            .With_EditorId(userId)
                            .With_CustomerId(BaseDataConfigurationHooks.ForeignCompany.ID)
                            .SaveToDb();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 12)]
        public static void ConfigureCashCenterProcessSettingLinks()
        {            
            string[] materialTypes = { "NOTE", "COIN", "CHEQUE" };
            var noteCompanyProcessSettingID = noteCompanyProcessSetting.ID;
            var coinCompanyProcessSettingID = coinCompanyProcessSetting.ID;
            var foreignCompanyProcessSettingID = foreignCompanyProcessSetting.ID;

            using (var context = new AutomationCashCenterDataContext())
            {
                try
                {
                    #region CashCenterProcessSettingMaterialTypeLinks

                    if (!context.CashCenterProcessSettingMaterialTypeLinks.Any(l => l.CashCenterProcessSettingID == noteCompanyProcessSettingID))
                    {
                        SaveMaterialTypeLinks(noteCompanyProcessSettingID);                            
                    }
                    if (!context.CashCenterProcessSettingMaterialTypeLinks.Any(l => l.CashCenterProcessSettingID == coinCompanyProcessSettingID))
                    {
                        SaveMaterialTypeLinks(coinCompanyProcessSettingID);
                    }
                    if (!context.CashCenterProcessSettingMaterialTypeLinks.Any(l => l.CashCenterProcessSettingID == foreignCompanyProcessSettingID))
                    {
                        SaveMaterialTypeLinks(foreignCompanyProcessSettingID);
                    }

                    /// local function to create material type links for a process setting record
                    void SaveMaterialTypeLinks(int processSettingId)
                    {
                        foreach (var type in materialTypes)
                        {
                            var link = new CashCenterProcessSettingMaterialTypeLink();
                            link.SetMaterialTypeCode(type);
                            link.SetCashCenterProcessSettingID(processSettingId);
                            context.CashCenterProcessSettingMaterialTypeLinks.Add(link);
                        }
                    }
                    #endregion

                    #region CashCenterProcessSettingProductLinks
                    if (!context.CashCenterProcessSettingProductLinks.Any(l => l.CashCenterProcessSettingId == noteCompanyProcessSettingID) ||
                        !context.CashCenterProcessSettingProductLinks.Any(l => l.CashCenterProcessSettingId == coinCompanyProcessSettingID) ||
                        !context.CashCenterProcessSettingProductLinks.Any(l => l.CashCenterProcessSettingId == foreignCompanyProcessSettingID))
                    {
                        var simpleNoteArray = new string[]
                        {
                            BaseDataConfigurationHooks.Eur10Product.ProductCode,
                            BaseDataConfigurationHooks.Eur100Product.ProductCode,
                            BaseDataConfigurationHooks.Gbp10Product.ProductCode,
                            BaseDataConfigurationHooks.Gbp100Product.ProductCode                            
                        };
                        var simpleCoinArray = new string[]
                        {
                            BaseDataConfigurationHooks.Eur1Product.ProductCode,
                            BaseDataConfigurationHooks.Eur2Product.ProductCode
                        };
                        var complexCoinArray = new string[]
                        {
                            BaseDataConfigurationHooks.Eur1PackProduct.ProductCode,
                            BaseDataConfigurationHooks.Eur2PackProduct.ProductCode
                        };
                        var foreignNoteArray = new string[]
                        {
                            BaseDataConfigurationHooks.Usd10Product.ProductCode,
                            BaseDataConfigurationHooks.Usd100Product.ProductCode
                        };
                        var foreignCoinArray = new string[]
                        {
                            BaseDataConfigurationHooks.Usd1Product.ProductCode                            
                        };
                        var barcodedNoteArray = new string[]
                        {                            
                            BaseDataConfigurationHooks.Eur10BarcodedProduct.ProductCode,
                            BaseDataConfigurationHooks.Eur100BarcodedProduct.ProductCode,
                            BaseDataConfigurationHooks.Usd10BarcodedProduct.ProductCode
                        };
                        var barcodedCoinArray = new string[]
                        {
                            BaseDataConfigurationHooks.Eur1BarcodedProduct.ProductCode,                            
                            BaseDataConfigurationHooks.Usd1BarcodedProduct.ProductCode
                        };
                        var looseArray = new string[]
                        {
                            BaseDataConfigurationHooks.Eur1LooseProduct.ProductCode,
                            BaseDataConfigurationHooks.Usd1LooseProduct.ProductCode,
                            BaseDataConfigurationHooks.Eur20LooseProduct.ProductCode,
                            BaseDataConfigurationHooks.Usd20LooseProduct.ProductCode
                        };                        
                        var simpleNotesProductsBagTypeId = BaseDataConfigurationHooks.SimpleNotesProductsBagType.ID;
                        var simpleCoinsProductsBagTypeId = BaseDataConfigurationHooks.SimpleCoinsProductsBagType.ID;
                        var complexCoinsProductsBagTypeId = BaseDataConfigurationHooks.ComplexCoinsProductsBagType.ID;
                        var foreignCurrencyNotesProductsBagTypeId = BaseDataConfigurationHooks.ForeignCurrencyNotesProductsBagType.ID;
                        var foreignCurrencyCoinsProductsBagTypeId = BaseDataConfigurationHooks.ForeignCurrencyCoinsProductsBagType.ID;
                        var barcodedNotesProductsBagTypeId = BaseDataConfigurationHooks.BarcodedNotesProductsBagType.ID;
                        var barcodedCoinsProductsBagTypeId = BaseDataConfigurationHooks.BarcodedCoinsProductsBagType.ID;
                        var looseProductsBagTypeId = BaseDataConfigurationHooks.LooseProductsBagType.ID;                        

                        if (!context.CashCenterProcessSettingProductLinks.Any(l => l.CashCenterProcessSettingId == noteCompanyProcessSettingID))
                        {
                            SaveProcessSettingProductLinks(noteCompanyProcessSettingID);                            
                        }
                        if (!context.CashCenterProcessSettingProductLinks.Any(l => l.CashCenterProcessSettingId == coinCompanyProcessSettingID))
                        {
                            SaveProcessSettingProductLinks(coinCompanyProcessSettingID);                            
                        }
                        if (!context.CashCenterProcessSettingProductLinks.Any(l => l.CashCenterProcessSettingId == foreignCompanyProcessSettingID))
                        {
                            SaveProcessSettingProductLinks(foreignCompanyProcessSettingID);                            
                        }
                       

                        /// local function to create a set of product links for a process setting record
                        void SaveProcessSettingProductLinks(int processSettingId)
                        {
                            foreach (var code in simpleNoteArray)
                            {
                                AddLink(code, simpleNotesProductsBagTypeId);
                            }
                            foreach (var code in simpleCoinArray)
                            {
                                AddLink(code, simpleCoinsProductsBagTypeId);
                            }
                            foreach (var code in complexCoinArray)
                            {
                                AddLink(code, complexCoinsProductsBagTypeId);
                            }
                            foreach (var code in foreignNoteArray)
                            {
                                AddLink(code, foreignCurrencyNotesProductsBagTypeId);
                            }
                            foreach (var code in foreignCoinArray)
                            {
                                AddLink(code, foreignCurrencyCoinsProductsBagTypeId);
                            }
                            foreach (var code in barcodedNoteArray)
                            {
                                AddLink(code, barcodedNotesProductsBagTypeId);
                            }
                            foreach (var code in barcodedCoinArray)
                            {
                                AddLink(code, barcodedCoinsProductsBagTypeId);
                            }
                            foreach (var code in looseArray)
                            {
                                AddLink(code, looseProductsBagTypeId);
                            }                            

                            /// nested local function to create one link
                            void AddLink(string code, int bagTypeId)
                            {
                                var link = new CashCenterProcessSettingProductLink
                                {
                                    ProductCode = code,
                                    BagTypeId = bagTypeId
                                };
                                link.SetCashCenterProcessSettingId(processSettingId);
                                context.CashCenterProcessSettingProductLinks.Add(link);
                            }
                        }
                    }
                    #endregion

                    #region CashCenterProcessSettingBagTypeMaterialTypeLinks

                    var sizeAllocationBagTypeId = BaseDataConfigurationHooks.SizeAllocationBagType.ID;

                    if (!context.AutomationCashCenterProcessSettingBagTypeMaterialTypeLinks.Any(l => l.CashCenterProcessSettingID == noteCompanyProcessSettingID))
                    {
                        SaveBagTypeMaterialTypeLinks(noteCompanyProcessSettingID);
                    }
                    if (!context.AutomationCashCenterProcessSettingBagTypeMaterialTypeLinks.Any(l => l.CashCenterProcessSettingID == coinCompanyProcessSettingID))
                    {
                        SaveBagTypeMaterialTypeLinks(coinCompanyProcessSettingID);
                    }
                    if (!context.AutomationCashCenterProcessSettingBagTypeMaterialTypeLinks.Any(l => l.CashCenterProcessSettingID == foreignCompanyProcessSettingID))
                    {
                        SaveBagTypeMaterialTypeLinks(foreignCompanyProcessSettingID);
                    }

                    void SaveBagTypeMaterialTypeLinks(int processSettingId)
                    {                        
                        foreach (var type in materialTypes)
                        {
                            var link = new AutomationCashCenterProcessSettingBagTypeMaterialTypeLink
                            {
                                BagTypeID = sizeAllocationBagTypeId,
                                MaterialTypeID = type,
                                MinimumQuantity = 1,
                                MaximumQuantity = 400
                            };
                            link.SetCashCenterProcessSettingID(processSettingId);
                            context.AutomationCashCenterProcessSettingBagTypeMaterialTypeLinks.Add(link);

                            //context.Database.ExecuteSqlCommand(
                            //    $"INSERT {typeof(CashCenterProcessSettingBagTypeMaterialTypeLink).GetTableName()} " +
                            //    $"(CashCenterProcessSettingID, BagTypeID, MaterialTypeID, MinimumQuantity, MaximumQuantity) " +
                            //    $"VALUES (@processSetting, @bagType, @materialType, @minQty, @maxQty);",
                            //    new SqlParameter("processSetting", processSettingId),
                            //    new SqlParameter("bagType", sizeAllocationBagTypeId),
                            //    new SqlParameter("materialType", type),
                            //    new SqlParameter("minQty", 1),                   
                            //    new SqlParameter("maxQty", 400));
                        }
                    }
                    #endregion
                                        
                    context.SaveChanges();                    
                }
                catch
                {
                    throw;
                }
            }
        }

        [BeforeFeature(Order = 13)]
        public static void ConfigureCashCenterPackingLines()
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                try
                {
                    notePackingLine = context.PackingLines.AsNoTracking().FirstOrDefault(s => s.Description == notePackingLineName);
                    if (notePackingLine == null)                    
                    {
                        notePackingLine = new PackingLine { DateFrom = today, Description = notePackingLineName, PackingMode = PackingMode.PreGeneratedUnits, PackingType = PackingType.BatchBySingleOperator };
                        notePackingLine.SetAuthor(userId);
                        notePackingLine.SetDateCreated(today);
                        notePackingLine.SetDateUpdated(today);
                        notePackingLine.SetNumber(GetMaxNumber(null) + 1);
                        context.PackingLines.Add(notePackingLine);
                        context.SaveChanges();
                    }

                    coinPackingLine = context.PackingLines.AsNoTracking().FirstOrDefault(s => s.Description == coinPackingLineName);
                    if (coinPackingLine == null)
                    {
                        coinPackingLine = new PackingLine { DateFrom = today, Description = coinPackingLineName, PackingMode = PackingMode.PreGeneratedUnits, PackingType = PackingType.BatchBySingleOperator };
                        coinPackingLine.SetAuthor(userId);
                        coinPackingLine.SetDateCreated(today);
                        coinPackingLine.SetDateUpdated(today);
                        coinPackingLine.SetNumber(GetMaxNumber(null) + 1);
                        context.PackingLines.Add(coinPackingLine);
                        context.SaveChanges();
                    }

                    foreignPackingLine = context.PackingLines.AsNoTracking().FirstOrDefault(s => s.Description == foreignPackingLineName);
                    if (foreignPackingLine == null)
                    {
                        foreignPackingLine = new PackingLine { DateFrom = today, Description = foreignPackingLineName, PackingMode = PackingMode.PreGeneratedUnits, PackingType = PackingType.BatchBySingleOperator };
                        foreignPackingLine.SetAuthor(userId);
                        foreignPackingLine.SetDateCreated(today);
                        foreignPackingLine.SetDateUpdated(today);
                        foreignPackingLine.SetNumber(GetMaxNumber(null) + 1);
                        context.PackingLines.Add(foreignPackingLine);
                        context.SaveChanges();
                    }
                    

                    if (!context.PackingLineWorkstationLinks.Any(l => l.PackingLineId == notePackingLine.ID))
                    {
                        var link = new PackingLineWorkstationLink { WorkstationId = noteWorkstation.ID };
                        link.SetNumberInLine(1);
                        link.SetPackingLine(notePackingLine.ID);
                        context.PackingLineWorkstationLinks.Add(link);
                    }

                    if (!context.PackingLineWorkstationLinks.Any(l => l.PackingLineId == coinPackingLine.ID))
                    {
                        var link = new PackingLineWorkstationLink { WorkstationId = coinWorkstation.ID };
                        link.SetNumberInLine(1);
                        link.SetPackingLine(coinPackingLine.ID);
                        context.PackingLineWorkstationLinks.Add(link);
                    }

                    if (!context.PackingLineWorkstationLinks.Any(l => l.PackingLineId == foreignPackingLine.ID))
                    {
                        var link = new PackingLineWorkstationLink { WorkstationId = foreignWorkstation.ID };
                        link.SetNumberInLine(1);
                        link.SetPackingLine(foreignPackingLine.ID);
                        context.PackingLineWorkstationLinks.Add(link);
                    }
                    context.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        [AfterFeature]
        public static void ClearCashCenterData()
        {
            var today = DateTime.Today;

            using (var context = new AutomationCashCenterDataContext())
            {
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_Reconciliation_DiscrepancySOProduct]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_Discrepancy]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_BaseData_DayClosureHistory]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_BaseData_ProcessingHistory]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_CashCenter_ProcessingSession]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_CashCenter_StockContainerNumberHistory]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_CashCenter_ProcessingRemark]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_CashCenter_DailyHeaderCard]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_CashCenter_Banknote]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_CashCenter_HeaderCardStockContainerLink]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_CashCenter_CountResultLine]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_CashCenter_CountResult]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_CashCenter_PreannouncementContainerLink]");                
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_OrderAllocationLog]");
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[WP_CashCenter_StockTransactionLine]");
                context.StockTransactions.Where(x => x.DateCreated >= today).Delete(x => x.BatchSize = 1000);
                context.ContainersBatches.Delete();
                context.RawMachineCountResults.Where(x => x.DateCreated >= today).Delete();
                context.StockContainers.Where(x => x.DateCreated >= today).Delete(x => x.BatchSize = 100);
                context.StockOrders.Where(x => x.DateCreated >= today).Delete(x => x.BatchSize = 100);
                context.OrdersBatches.Where(x => x.DateCreated >= today).Delete();
                context.SiteStockHistoryFlowTotals.Where(x => x.DateCreated >= today).Delete(x => x.BatchSize = 100);
                context.SiteStockMovementHistories.Where(x => x.DateCreated >= today).Delete(x => x.BatchSize = 100);
                context.SiteStockPositionHistories.Where(x => x.DateCreated >= today).Delete(x => x.BatchSize = 100);
                context.SiteStockHistories.Where(x => x.DateCreated >= today).Delete();
                //context.StockPositions.Where(x => x.DateCreated >= today).Delete(x => x.BatchSize = 100); // blocked by missing setters in the corresponding property of StockPosition class
                context.StockPositions.Delete(x => x.BatchSize = 1000);
                context.SaveChanges();
            }
        }    

        private static int GetMaxNumber(DataBaseParams dbParams)
        {
            using (var context = new AutomationCashCenterDataContext())
            {
                var maxNumber = (int)context.PackingLines.Max(x => x.Number);
                return maxNumber < 0 ? 0 : maxNumber;
            }
        }
    }
}
