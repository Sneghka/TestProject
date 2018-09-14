using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CWC.AutoTests.Builders.FeedingBuilders
{
    public class LocationServicingTimeWindowFeedingBuilder
    {
        XDocument documentElement;
        XElement locationList;
        XElement location;
        XElement address;
        XElement locationServiceTimeWindows;
        XElement locationServiceTimeWindow;
        XElement defaultCITSite;
        public LocationServicingTimeWindowFeedingBuilder()
        {
            documentElement = new XDocument(new XElement("DocumentElement"));
        }

        public LocationServicingTimeWindowFeedingBuilder New(string mapper = "")
        {
            locationList = new XElement("LocationList", new XAttribute("mapper", mapper));
            documentElement.Root.Add(locationList);
            location = new XElement("Location");
            locationList.Add(location);
            return this;
        }
        
        public LocationServicingTimeWindowFeedingBuilder With_Level(string level)
        {
            if (level != null)
            {
                location.Add(new XElement("Level", level));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_VisitAddressCode(string visitAddressCode)
        {
            if (visitAddressCode != null)
            {
                location.Add(new XElement("VisitAddressCode", visitAddressCode));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_Code(string code)
        {
            if (code != null)
            {
                location.Add(new XElement("Code", code));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_Name(string name)
        {
            if (name != null)
            {
                location.Add(new XElement("Name", name));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_IsEnabled(bool? isEnabled)
        {
            if (isEnabled != null)
            {
                if (isEnabled == true)
                {
                    location.Add(new XElement("IsEnabled", "yes"));
                }
                else
                {
                    location.Add(new XElement("IsEnabled", "no"));
                }
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_CompanyNumber(string companyNumber)
        {
            if(companyNumber != null)
            {
                location.Add(new XElement("CompanyNumber",companyNumber));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_LocationTypeCode(string locationTypeCode)
        {
            if(locationTypeCode != null)
            {
                location.Add(new XElement("LocationTypeCode", locationTypeCode));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_HandlingType(string handlingType)
        {
            if(handlingType != null)
            {
                location.Add(new XElement("HandlingType", handlingType));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_AtmId (string atmId)
        {
            if(atmId != null)
            {
                location.Add(new XElement("ATMID", atmId));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_AtmTypeCode(string atmTypeCode)
        {
            if(atmTypeCode != null)
            {
                location.Add(new XElement("ATMTypeCode", atmTypeCode));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_BankAccountNumber(string bankAccountNumber)
        {
            if(bankAccountNumber != null)
            {
                location.Add(new XElement("BankAccountNumber", bankAccountNumber));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_Abbreviation(string abbrev)
        {
            if(abbrev != null)
            {
                location.Add(new XElement("Abbreviation", abbrev));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_PreCrediting(string preCrediting)
        {
            if(preCrediting != null)
            {
                location.Add(new XElement("PreCrediting",preCrediting));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_MainLocationCode(string mainLocationCode)
        {
            if(mainLocationCode != null)
            {
                location.Add(new XElement("MainLocationCode", mainLocationCode));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_IsDetailsInherited(string isDetailsInherited)
        {
            if(isDetailsInherited != null)
            {
                location.Add(new XElement("IsDetailsInherited",isDetailsInherited));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_IsSharedSubLocation(string isSharedSubLocation)
        {
            if (isSharedSubLocation != null)
            {
                location.Add(new XElement("IsSharedSubLocation", isSharedSubLocation));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_InvoiceReference(string invoiceReference)
        {
            if(invoiceReference != null)
            {
                location.Add(new XElement("InvoiceReference", invoiceReference));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_InboundReference(string inboundReference)
        {
            if(inboundReference != null)
            {
                location.Add(new XElement("InboundReference", inboundReference));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_OutboundReference(string outboundReference)
        {
            if (outboundReference != null)
            {
                location.Add(new XElement("OutboundReference", outboundReference));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_NotesCCSite(string notesCCSite)
        {
            if (notesCCSite != null)
            {
                location.Add(new XElement("NotesCCSite",notesCCSite));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_CoinsCCSite(string coinsCCSite)
        {
            if (coinsCCSite != null)
            {
                location.Add(new XElement("CoinsCCSite", coinsCCSite));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_ForeignCurrencyCCSite(string foreignCurrencyCCSite)
        {
            if(foreignCurrencyCCSite != null)
            {
                location.Add(new XElement("ForeignCurrencyCCSite",foreignCurrencyCCSite));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_DefaultCITSite(string defaultCITSite)
        {
            if(defaultCITSite != null)
            {
                location.Add(new XElement("DefaultCITSite",defaultCITSite));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_CoinsCITSite(string coinsCITSite)
        {
            if(coinsCITSite != null)
            {
                location.Add(new XElement("CoinsCITSite", coinsCITSite));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_OrderingDepartmentName(string orderingDepartmentName)
        {
            if (orderingDepartmentName != null)
            {
                location.Add(new XElement("OrderingDepartmentName", orderingDepartmentName));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_Address()
        {
            address = new XElement("Address");
            location.Add(address);
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_Street(string street)
        {
            if(street != null)
            {
                address.Add(new XElement("Street", street));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_PostalCode(string postalCode)
        {
            if(postalCode != null)
            {
                address.Add(new XElement("PostalCode", postalCode));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_City(string city)
        {
            if(city != null)
            {
                address.Add(new XElement("City", city));
            }
            return this;   
        }

        public LocationServicingTimeWindowFeedingBuilder With_State(string state)
        {
            if (state != null)
            {
                address.Add(new XElement("State", state));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_Country(string country)
        {
            if(country != null)
            {
                address.Add(new XElement("Country", country));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_ExtraAddressInfo(string extraAddressInfo)
        {
            if(extraAddressInfo != null)
            {
                address.Add(new XElement("ExtraAddressInfo", extraAddressInfo));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_LocationServiceTimeWindows()
        {
            locationServiceTimeWindows = new XElement("LocationServiceTimeWindows");
            location.Add(locationServiceTimeWindows);
            locationServiceTimeWindow = new XElement("LocationServiceTimeWindow");
            locationServiceTimeWindows.Add(locationServiceTimeWindow);
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_Servicetype(string serviceType)
        {
            if(serviceType != null)
            {
                locationServiceTimeWindow.Add(new XElement("Servicetype", serviceType));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_Ordertype(string orderType)
        {
            if(orderType != null)
            {
                locationServiceTimeWindow.Add(new XElement("Ordertype", orderType));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_TimeFrom(string timeFrom)
        {
            if(timeFrom != null)
            {
                locationServiceTimeWindow.Add(new XElement("TimeFrom", timeFrom));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_TimeTo(string timeTo)
        {
            if (timeTo != null)
            {
                locationServiceTimeWindow.Add(new XElement("TimeTo", timeTo));
            }
            return this;
        }
        public LocationServicingTimeWindowFeedingBuilder With_SecondTimeFrom(string secondTimeFrom)
        {
            if (secondTimeFrom != null)
            {
                locationServiceTimeWindow.Add(new XElement("SecondTimeFrom", secondTimeFrom));
            }
            return this;
        }
        public LocationServicingTimeWindowFeedingBuilder With_SecondTimeTo(string secondTimeTo)
        {
            if (secondTimeTo != null)
            {
                locationServiceTimeWindow.Add(new XElement("SecondTimeTo", secondTimeTo));
            }
            return this;
        }

        public LocationServicingTimeWindowFeedingBuilder With_Weekday(int? weekday)
        {
            if(weekday != null)
            {
                locationServiceTimeWindow.Add(new XElement("Weekday", weekday));
            }
            return this;
        }
        public XDocument Build()
        {
            return documentElement;
        }
    }
}
