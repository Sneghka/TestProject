using CWC.AutoTests.Helpers.Transport;

namespace CWC.AutoTests.Helpers
{
    public class HelperFacade
    {
        private static readonly object locker = new object();
        private static BasicExportHelper basicExportHelper;
        private static BillingHelper billingHelper;
        private static CashCenterHelper cashCenterHelper;
        private static ConfigurationKeysHelper configurationKeysHelper;
        private static ContractHelper contractHelper;
        private static EntityToXmlConverterHelper entityToXmlConverterHelper;
        private static FeedingHelper feedingHelper;
        private static MasterRouteHelper masterRoutetHelper;
        private static OrderExportHelper orderExportHelper;
        private static ReplicationHelper replicationHelper;
        private static ServiceOrderListTestsHelper serviceOrderListTestsHelper;
        private static TransportHelper transportHelper;         
        private static TransportExportHelper transportExportHelper;
        private static VisitMonitorHelper visitMonitorHelper;
        private static PreannauncementAndBillingHelper preannauncementAndBillingHelper;

        public static BasicExportHelper BasicExportHelper
        {
            get
            {
                lock (locker)
                {
                    if (basicExportHelper == null)
                    {
                        basicExportHelper = new BasicExportHelper();
                    }
                }
                return basicExportHelper;
            }
        }

        public static BillingHelper BillingHelper
        {
            get
            {
                lock (locker)
                {
                    if (billingHelper == null)
                    {
                        billingHelper = new BillingHelper();
                    }
                }
                return billingHelper;
            }
        }

        public static CashCenterHelper CashCenterHelper
        {
            get
            {
                lock (locker)
                {
                    if (cashCenterHelper == null)
                    {
                        cashCenterHelper = new CashCenterHelper();
                    }
                }
                return cashCenterHelper;
            }
        }

        public static ConfigurationKeysHelper ConfigurationKeysHelper
        {
            get
            {
                lock (locker)
                {
                    configurationKeysHelper = new ConfigurationKeysHelper();
                }
                return configurationKeysHelper;
            }
        }

        public static ContractHelper ContractHelper
        {
            get
            {
                lock (locker)
                {
                    if (contractHelper == null)
                    {
                        contractHelper = new ContractHelper();
                    }
                }
                return contractHelper;
            }
        }

        public static EntityToXmlConverterHelper EntityToXmlConverterHelper
        {
            get
            {
                lock (locker)
                {
                    entityToXmlConverterHelper = new EntityToXmlConverterHelper();
                }
                return entityToXmlConverterHelper;
            }
        }        

        public static FeedingHelper FeedingHelper
        {
            get
            {
                lock (locker)
                {
                    if (feedingHelper == null)
                    {
                        feedingHelper = new FeedingHelper();
                    }
                }
                return feedingHelper;
            }
        }

        public static MasterRouteHelper MasterRouteHelper
        {
            get
            {
                lock (locker)
                {
                    masterRoutetHelper = new MasterRouteHelper();
                }

                return masterRoutetHelper;
            }
        }        

        public static OrderExportHelper OrderExportHelper
        {
            get
            {
                lock (locker)
                {
                    if (orderExportHelper == null)
                    {
                        orderExportHelper = new OrderExportHelper();
                    }
                }
                return orderExportHelper;
            }
        }

        public static PreannauncementAndBillingHelper PreannauncementAndBillingHelper
        {
            get
            {
                lock (locker)
                {
                    if (preannauncementAndBillingHelper == null)
                    {
                        preannauncementAndBillingHelper = new PreannauncementAndBillingHelper();
                    }
                }

                return preannauncementAndBillingHelper;
            }
        }

        public static ReplicationHelper ReplicationHelper
        {
            get
            {
                lock (locker)
                {
                    if (replicationHelper == null)
                    {
                        replicationHelper = new ReplicationHelper();
                    }
                }
                return replicationHelper;
            }
        }

        public static ServiceOrderListTestsHelper ServiceOrderListTestsHelper
        {
            get
            {
                lock (locker)
                {
                    serviceOrderListTestsHelper = new ServiceOrderListTestsHelper();
                }
                return serviceOrderListTestsHelper;
            }
        }

        public static TransportHelper TransportHelper
        {
            get
            {
                lock (locker)
                {
                    if (transportHelper == null)
                    {
                        transportHelper = new TransportHelper();
                    }
                }
                return transportHelper;
            }
        }

        public static TransportExportHelper TransportExportHelper
        {
            get
            {
                lock (locker)
                {
                    transportExportHelper = new TransportExportHelper();
                }
                return transportExportHelper;
            }
        }

        public static VisitMonitorHelper VisitMonitorHelper
        {
            get
            {
                lock (locker)
                {
                    visitMonitorHelper = new VisitMonitorHelper();
                }
                return visitMonitorHelper;
            }
        }
    }
}
