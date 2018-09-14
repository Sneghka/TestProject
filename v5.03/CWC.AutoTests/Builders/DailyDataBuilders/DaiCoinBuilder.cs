using Cwc.BaseData;
using Cwc.Transport.Model;
using CWC.AutoTests.Model;
using System;

namespace CWC.AutoTests.ObjectBuilder.DailyDataBuilders
{
    public class DaiCoinBuilder
    {
        DailyStopProduct daiCoin;
        public DaiCoinBuilder()
        {            
        }

        public DaiCoinBuilder With_DaiDate(DateTime value)
        {
            daiCoin.Date = value;
            daiCoin.DayNumber = (int)value.DayOfWeek;
            return this;
        }        

        public DaiCoinBuilder With_Time(string value)
        {
            daiCoin.ArrivalTime = value;
            return this;
        }

        public DaiCoinBuilder With_Product(Cwc.BaseData.Product value)
        {
            int productCode;
            var result = Int32.TryParse(value.ProductCode, out productCode);
            if (!result)
            {
                throw new InvalidCastException("Product code contains symbols! It should have digits only!");
            }

            daiCoin.CoinType = productCode;
            return this;
        }

        public DaiCoinBuilder With_Product(DataModel.Product value)
        {
            //daiCoin.coin_nr = value.WP_id;
            int productCode;
            var result = Int32.TryParse(value.ProductCode, out productCode);
            if (!result)
            {
                throw new InvalidCastException("Product code contains symbols! It should have digits only!");
            }

            daiCoin.CoinType = productCode;
            return this;
        }

        public DaiCoinBuilder With_ProductID(string value/*int value*/)
        {
            int productCode;
            var result = Int32.TryParse(value, out productCode);
            if (!result)
            {
                throw new InvalidCastException("Product code contains symbols! It should have digits only!");
            }

            daiCoin.CoinType = productCode;
            return this;
        }

        public DaiCoinBuilder With_Location(Location value)
        {
            daiCoin.LocationID = value.ID;
            return this;
        }

        public DaiCoinBuilder With_Location(decimal value)
        {
            daiCoin.LocationID = value;
            return this;
        }

        public DaiCoinBuilder With_AmountDel(int value)
        {
            daiCoin.AmountDelivered = value;
            return this;
        }

        public DaiCoinBuilder With_AmountCol(int value)
        {
            daiCoin.AmountCollected = value;
            return this;
        }

        public DaiCoinBuilder With_Site(Site value)
        {
            daiCoin.BranchCode = value.Branch_cd;
            return this;
        }

        public DaiCoinBuilder With_MasterRoute(string value)
        {
            daiCoin.RouteNumber = value;
            return this;
        }

        public DaiCoinBuilder New()
        {
            daiCoin = new DailyStopProduct();            
            daiCoin.NumberType = 1;            
            return this;
        }

        public DailyStopProduct Build()
        {
            return this.daiCoin;
        }

        public DaiCoinBuilder SaveToDb()
        {
            using (var context = new AutomationTransportDataContext())
            {
                context.DailyStopProducts.Add(daiCoin);
                context.SaveChanges();
            }
            return this;
        }
    }
}
