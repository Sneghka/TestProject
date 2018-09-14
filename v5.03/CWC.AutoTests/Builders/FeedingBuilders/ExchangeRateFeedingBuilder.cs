using System.Xml.Linq;

namespace CWC.AutoTests.Builders.FeedingBuilders
{
    public class ExchangeRateFeedingBuilder
    {
        XDocument documentElement;
        XElement exchangeRateList;
        XElement exchangeRate;

        public ExchangeRateFeedingBuilder()
        {
            documentElement = new XDocument(new XElement("DocumentElement"));
        }

        public ExchangeRateFeedingBuilder New(string mapperName = "")
        {
            exchangeRateList = new XElement("ExchangeRateList", new XAttribute("mapper", mapperName));
            documentElement.Root.Add(exchangeRateList);
            return this;
        }
        
        public ExchangeRateFeedingBuilder With_ExchangeRate()
        {
            exchangeRate = new XElement("ExchangeRate");
            exchangeRateList.Add(exchangeRate);
            return this;
        }

        public ExchangeRateFeedingBuilder With_Rate(string rate)
        {
            exchangeRate.Add(new XElement("Rate", rate));
            return this;
        }

        public ExchangeRateFeedingBuilder With_ExchangeDate(string exchangeDate)
        {
            exchangeRate.Add(new XElement("ExchangeDate", exchangeDate));
            return this;
        }

        public ExchangeRateFeedingBuilder With_CurrencyFromCode(string currencyFromCode)
        {
            exchangeRate.Add(new XElement("CurrencyFromCode", currencyFromCode));
            return this;
        }
        public XDocument Build()
        {
            return documentElement;
        }
    }
}
