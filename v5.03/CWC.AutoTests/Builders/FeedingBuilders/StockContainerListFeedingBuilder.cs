using System;
using System.Xml.Linq;

namespace CWC.AutoTests.Builders.FeedingBuilders
{
    public class StockContainerListFeedingBuilder   
    {
        XDocument documentElement;
        XElement stockContainer;
        XElement stockContainerList;
        XElement stockPositionList;
        XElement stockPosition;

        public StockContainerListFeedingBuilder()
        {
            documentElement = new XDocument(new XElement("DocumentElement"));
        }

        public StockContainerListFeedingBuilder New()
        {
            stockContainerList = new XElement("StockContainerList");
            documentElement.Root.Add(stockContainerList);
            return this;
        }
        public StockContainerListFeedingBuilder With_StockContainer(string contentType)
        {
            stockContainer = new XElement("StockContainer", new XAttribute("ContentType", contentType));
            stockContainerList.Add(stockContainer);
            return this;
        }

        public StockContainerListFeedingBuilder With_Number(string number)
        {
            if (number != null)
            {
                stockContainer.Add(new XElement("Number", number));
            }

            return this;
        }

        public StockContainerListFeedingBuilder With_LocationFromCode(string locationFromCode)
        {
            if (locationFromCode != null)
            {
                stockContainer.Add(new XElement("LocationFromCode", locationFromCode));
            }

            return this;
        }

        public StockContainerListFeedingBuilder With_SecondNumber(string secondNumber)
        {
            if (secondNumber != null)
            {
                stockContainer.Add(new XElement("SecondNumber", secondNumber));
            }

            return this;
        }

        public StockContainerListFeedingBuilder With_DateCollected(DateTime dateCollected)
        {
            if (dateCollected != null)
            {
                stockContainer.Add(new XElement("DateCollected", dateCollected.ToString("yyyy-MM-dd")));
            }

            return this;
        }

        public StockContainerListFeedingBuilder With_StockPositionList()
        {
            stockPositionList = new XElement("StockPositions");
            stockContainer.Add(stockPositionList);

            return this;
        }

        public StockContainerListFeedingBuilder With_StockPosition()
        {
            stockPosition = new XElement("StockPosition");
            stockPositionList.Add(stockPosition);

            return this;
        }

        public StockContainerListFeedingBuilder With_Value(decimal value)
        {
            if (value != 0)
            {
                stockPosition.Add(new XElement("Value", value));
            }
            return this;
        }

        public StockContainerListFeedingBuilder With_CurrencyCode(string currencyCode)
        {
            if (currencyCode != null)
            {
                stockPosition.Add(new XElement("CurrencyCode", currencyCode));
            }
            return this;
        }

        public StockContainerListFeedingBuilder With_Weight(decimal weight)
        {
            if (weight != 0)
            {
                stockPosition.Add(new XElement("Weight", weight));
            }
            return this;
        }

        public StockContainerListFeedingBuilder With_IsTotal(bool isTotal = false)
        {
            if(isTotal != true)
            {
                stockPosition.Add(new XElement("IsTotal", "No"));
            }
            else
            {
                stockPosition.Add(new XElement("IsTotal", "Yes"));
            }
            
            return this;
        }

        public XDocument Build()
        {
            return documentElement;
        }
    }
}
