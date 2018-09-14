using CWC.AutoTests.Builders.FeedingBuilders;

namespace CWC.AutoTests.ObjectBuilder.FeedingBuilder
{
    public class FeedingBuilderFacade
    {
        static ServiceOrderFeedingBuilder serviceOrderFeedingBuilder;
        static ServiceOrderListFeedingBuilder serviceOrderListFeedingBuilder;
        static StockContainerListFeedingBuilder stockContainerListFeedingBuilder;
        static LocationServicingTimeWindowFeedingBuilder locationServicingTimeWindowFeedingBuilder;
        static ExchangeRateFeedingBuilder exchangeRateFeedingBuilder;

        static readonly object locker = new object();


        public static ExchangeRateFeedingBuilder ExchangeRateFeeding
        {
            get
            {
                lock (locker)
                {
                    exchangeRateFeedingBuilder = new ExchangeRateFeedingBuilder();
                }
                return exchangeRateFeedingBuilder;
            }
        }
        public static ServiceOrderFeedingBuilder ServiceOrderFeeding
        {
            get
            {
                lock (locker)
                {
                    serviceOrderFeedingBuilder = new ServiceOrderFeedingBuilder();
                }
                
                return serviceOrderFeedingBuilder;
            }
           
        }

        public static ServiceOrderListFeedingBuilder ServiceOrderListFeeding
        {
            get
            {
                lock (locker)
                {
                    serviceOrderListFeedingBuilder = new ServiceOrderListFeedingBuilder();
                }

                return serviceOrderListFeedingBuilder;
            }

        }

        public static LocationServicingTimeWindowFeedingBuilder LocationServicingTimeWindowFeeding
        {
            get
            {
                lock (locker)
                {
                    locationServicingTimeWindowFeedingBuilder = new LocationServicingTimeWindowFeedingBuilder();
                }

                return locationServicingTimeWindowFeedingBuilder;
            }

        }

        public static StockContainerListFeedingBuilder StockContainerListFeeding
        {
            get
            {
                lock (locker)
                {
                    stockContainerListFeedingBuilder = new StockContainerListFeedingBuilder();
                }

                return stockContainerListFeedingBuilder;
            }
        }   
    }
}
