namespace CWC.AutoTests.ObjectBuilder.DailyDataBuilders
{
    public class DailyDataFacade
    {        
        public static DailyStopBuilder DailyStop
        {
            get
            {
                return new DailyStopBuilder();                
            }
        }

        public static DaiPackBuilder DaiPack
        {
            get
            {
                return new DaiPackBuilder();                
            }
        }

        public static HisPackBuilder HisPack
        {
            get
            {
                return new HisPackBuilder();                
            }
        }

        public static DaiCoinBuilder DaiCoin
        {
            get
            {
                return new DaiCoinBuilder();                
            }
        }

        public static DailyServiceBuilder DailyService
        {
            get
            {
                return new DailyServiceBuilder();               
            }
        }

        public static DaiServicingCodeBuilder DaiServicindCode
        {
            get
            {
                return new DaiServicingCodeBuilder();                
            }
        }
    }
}
