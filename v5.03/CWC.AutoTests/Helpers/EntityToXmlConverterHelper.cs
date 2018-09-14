using Cwc.BaseData;
using Cwc.Transport.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CWC.AutoTests.Helpers
{
    public class EntityToXmlConverterHelper
    {
        /// <summary>
        /// Method to convert collection of customers to the CustomerList XMl for feeding
        /// </summary>
        /// <param name="customers"></param>
        /// <returns>converted CustomerList</returns>
        public XDocument ConvertToFeeding(IEnumerable<Customer> customerList)
        {
            XDocument documentElement =
              new XDocument(
                  new XElement("DocumentElement",
                      new XElement("CompanyList",
                      from company in customerList
                      select new XElement("Company",
                            company.ReferenceNumber != null ? new XElement("Number", company.ReferenceNumber) : new XElement("Number", string.Empty),
                            company.Name != null ? new XElement("Name", company.Name) : new XElement("Name", string.Empty),
                            company.Enabled == true ? new XElement("IsEnabled", "yes") : new XElement("IsEnabled", "no"),
                            company.Abbrev != null ? new XElement("Abbreviation", company.Abbrev) : new XElement("Abbreviation", string.Empty),
                            company.Website != null ? new XElement("Website", company.Website) : new XElement("Website", string.Empty),
                            company.E_mail != null ? new XElement("Email", company.E_mail) : new XElement("Email", string.Empty),
                            company.Phone != null ? new XElement("TelephoneNumber", company.Phone) : new XElement("TelephoneNumber", string.Empty),
                            company.ParentCustomer != null ? new XElement("ParentCompanyNumber", BaseDataFacade.CustomerService.LoadCustomerById(company.ParentCustomer.Value).ReferenceNumber) : new XElement("ParentCompanyNumber", string.Empty),
                            company.Csgrp_cd != null ? new XElement("CompanyGroupCode", company.Csgrp_cd) : new XElement("CompanyGroupCode", string.Empty),
                            company.Debtor_nr != null ? new XElement("DebtorNumber", BaseDataFacade.CustomerService.LoadCustomerById(company.Debtor_nr.Value).ReferenceNumber) : new XElement("DebtorNumber", string.Empty),
                            company.AffiliatedBankID != null ? new XElement("AffiliatedBankNumber", BaseDataFacade.CustomerService.LoadCustomerById(company.AffiliatedBankID.Value).ReferenceNumber) : new XElement("AffiliatedBankNumber", string.Empty),
                            company.InvoiceReference != null ? new XElement("InvoiceReference", company.InvoiceReference) : new XElement("InvoiceReference", string.Empty),
                            company.InboundReference != null ? new XElement("InboundReference", company.InboundReference) : new XElement("InboundReference", string.Empty),
                            company.OutboundReference != null ? new XElement("OutboundReference", company.OutboundReference) : new XElement("OutboundReference", string.Empty),
                            company.PostalAddress == null ? null : new XElement("PostalAddress",
                                company.PostalAddress.Street != null ? new XElement("Street", company.PostalAddress.Street) : new XElement("Street", string.Empty),
                                company.PostalAddress.PostalCode != null ? new XElement("PostalCode", company.PostalAddress.PostalCode) : new XElement("PostalCode", string.Empty),
                                company.PostalAddress.City != null ? new XElement("City", company.PostalAddress.City) : new XElement("City", string.Empty),
                                company.PostalAddress.State != null ? new XElement("State", company.PostalAddress.State) : new XElement("State", string.Empty),
                                company.PostalAddress.Country != null ? new XElement("Country", company.PostalAddress.Country) : new XElement("Country", string.Empty),
                                company.PostalAddress.ExtraAddressInfo != null ? new XElement("ExtraAddressInfo", company.PostalAddress.ExtraAddressInfo) : new XElement("ExtraAddressInfo", string.Empty)),
                            company.VisitAddress == null ? null : new XElement("VisitAddress",
                                company.VisitAddress.Street != null ? new XElement("Street", company.VisitAddress.Street) : new XElement("Street", String.Empty),
                                company.VisitAddress.PostalCode != null ? new XElement("PostalCode", company.VisitAddress.PostalCode) : new XElement("PostalCode", string.Empty),
                                company.VisitAddress.City != null ? new XElement("City", company.VisitAddress.City) : new XElement("City", string.Empty),
                                company.VisitAddress.State != null ? new XElement("State", company.VisitAddress.State) : new XElement("State", string.Empty),
                                company.VisitAddress.Country != null ? new XElement("Country", company.VisitAddress.Country) : new XElement("Country", string.Empty),
                                company.VisitAddress.ExtraAddressInfo != null ? new XElement("ExtraAddressInfo", company.VisitAddress.ExtraAddressInfo) : new XElement("ExtraAddressInfo", string.Empty)
                            )
                        )
                    )
                )
            );
            return documentElement;
        }

        /// <summary>
        /// Method to convert collection of locations to LocationList feeding
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        public XDocument ConvertToFeeding(IEnumerable<Location> locationList)
        {
            XDocument documentElement =
               new XDocument(
                   new XElement("DocumentElement",
                       new XElement("LocationList",
                           from location in locationList
                           select new XElement("Location",
                           location.Level == Cwc.BaseData.Enums.LocationLevel.ServicePoint ? new XElement("Level", "Service point") : new XElement("Level", "Visit address"),
                           location.VisitAddressID == null ? new XElement("VisitAddressCode", location.VisitAddressID) : new XElement("VisitAddressCode", DataFacade.Location.Take(x => x.IdentityID == location.VisitAddressID).Build().Code ?? String.Empty),
                           location.Code != null ? new XElement("Code", location.Code) : null,
                           location.Name != null ? new XElement("Name", location.Name) : null,
                           new XElement("IsEnabled", location.Enabled ? "yes" : "no"),
                           location.Company != null ? new XElement("CompanyNumber", location.Company.ReferenceNumber) : null,
                           location.LocationType != null ? new XElement("LocationTypeCode", location.LocationType.ltCode) : null,
                           location.LocationTypeID != null ? new XElement("LocationTypeCode", location.LocationTypeID) : null,
                           location.HandlingType != null ? new XElement("HandlingType", location.HandlingType) : null,
                           location.CashPointNumber != null ? new XElement("ATMID", location.CashPointNumber) : null,
                           location.CashPointTypeID != null ? new XElement("ATMTypeCode", DataFacade.CashPointType.Take(x => x.ID == location.CashPointTypeID).Build().Name) : null,
                           location.BankAccount != null ? new XElement("BankAccountNumber", location.BankAccount.Number) : null,
                           location.Abbreviation != null ? new XElement("Abbreviation", location.Abbreviation) : null,
                           location.PreCrediting != null ? new XElement("PreCrediting", location.PreCrediting.Value) : null,
                           location.ParentLocation != null ? new XElement("MainLocationCode", location.ParentLocation.Code) : null,
                           new XElement("IsDetailsInherited", location.DetailsInherited),
                           new XElement("IsSharedSubLocation", location.SharedSubLocation),
                           location.InvoiceReference != null ? new XElement("InvoiceReference", location.InvoiceReference) : null,
                           location.InboundReference != null ? new XElement("InboundReference", location.InboundReference) : null,
                           location.OutboundReference != null ? new XElement("OutboundReference", location.OutboundReference) : null,
                           location.NotesSite != null ? new XElement("NotesCCSite", location.NotesSite.Branch_cd) : null,
                           location.CoinsSite != null ? new XElement("CoinsCCSite", location.CoinsSite.Branch_cd) : null,
                           location.BranchID.HasValue == true ? new XElement("DefaultCITSite", (BaseDataFacade.SiteService.Load(location.BranchID.Value))?.Branch_cd) : null,
                           location.ServicingDepotCoinsID.HasValue == true ? new XElement("CoinsCITSite", BaseDataFacade.SiteService.Load(location.ServicingDepotCoinsID.Value) == null ? location.ServicingDepotCoins.Branch_cd : BaseDataFacade.SiteService.Load(location.ServicingDepotCoinsID.Value).Branch_cd) : null,
                           location.ServicingDepotCoins != null ? new XElement("CoinsCITSite", location.ServicingDepotCoins.Branch_cd) : null,
                           location.OrderingDepartment != null ? new XElement("OrderingDepartmentName", location.OrderingDepartment.Name) : null,
                           location.BaseAddress == null ? null : new XElement("Address",
                               new XElement("Street", location.BaseAddress.Street != null ? location.BaseAddress.Street : string.Empty),
                               new XElement("PostalCode", location.BaseAddress.PostalCode != null ? location.BaseAddress.PostalCode : string.Empty),
                               new XElement("City", location.BaseAddress.City != null ? location.BaseAddress.City : string.Empty),
                               new XElement("State", location.BaseAddress.State != null ? location.BaseAddress.State : string.Empty),
                               new XElement("Country", location.BaseAddress.Country != null ? location.BaseAddress.Country : string.Empty),
                               new XElement("ExtraAddressInfo", location.BaseAddress.ExtraAddressInfo != null ? location.BaseAddress.ExtraAddressInfo : string.Empty)
                             )
                           //servicingTimeWindows != null ? new XElement("LocationServicingTimeWindows",
                           //from servicingTimeWindow in servicingTimeWindows
                           //select new XElement("LocationServicingTimeWindow",
                           //servicingTimeWindow.ServiceType != null ? new XElement("Servicetype", servicingTimeWindow.ServiceType) : null,
                           //servicingTimeWindow.OrderType != null ? new XElement("Ordertype", servicingTimeWindow.OrderType) : null,
                           //new XElement("TimeFrom", TimeSpan.FromHours(servicingTimeWindow.TimeFrom)),
                           //new XElement("TimeTo", TimeSpan.FromHours(servicingTimeWindow.TimeTo)),
                           //new XElement("SecondTimeFrom", servicingTimeWindow.TimeSecondFrom),
                           //new XElement("SecondTimeTo", servicingTimeWindow.TimeSecondTo),
                           //servicingTimeWindow.Weekday != null ? new XElement("Weekday", (int)servicingTimeWindow.Weekday) : null
                           //     )
                           //) : null
                       )
                   )
               )
           );

            return documentElement;
        }

        public XDocument ConvertToFeeding(IEnumerable<MasterRouteBuilder> routeBuildersList, string emptyTagName)
        {
            XDocument documentElement =
                new XDocument(
                    new XElement("DocumentElement",
                        new XElement("MasterRouteList",
                            from item in routeBuildersList
                            let route = item.Build()
                            let stopList = item.linkedStops
                            select new XElement("MasterRoute",
                                new XElement("Code", route.Code),
                                new XElement("DistancePlanned", route.DistancePlanned),
                                new XElement("CrewRequired", route.CrewRequired),
                                new XElement("Description", route.Description),
                                new XElement("ReferenceNumber", route.ReferenceNumber),
                                emptyTagName != "Weekday" ? new XElement("Weekday", (int)route.WeekdayNumber) : new XElement("Weekday", ""),
                                emptyTagName != "CITSiteCode" ? new XElement("CITSiteCode", route.Branch.Branch_cd) : new XElement("CITSiteCode", ""),                           
                                stopList.Count == 0 ? null : new XElement("MasterRouteStops",
                                 from stop in stopList
                                 select new XElement("MasteRouteStop",
                                 stop.SequenceNumber != 0 ? new XElement("SequenceNumber", stop.SequenceNumber) : null,
                                 stop.Location_id != 0 ? new XElement("LocationCode", DataFacade.Location.Take(x => x.ID == stop.Location_id)?.Build().Code) : null,
                                 stop.ArrivalTime != default(TimeSpan) ? new XElement("ArrivalTime", stop.ArrivalTime.ToString(@"hh\:mm")) : null,
                                 stop.OnSiteTime != default(TimeSpan) ? new XElement("OnSiteTime", stop.OnSiteTime.ToString(@"hh\:mm")) : null,
                                 stop.DepartureTime != default(TimeSpan) ? new XElement("DepartureTime", stop.DepartureTime.ToString(@"hh\:mm")) : null
                                 )
                             )
                         )
                     )
                  )
              );
            return documentElement;
        }

        public XDocument ConvertToFeeding(IEnumerable<StockContainerBuilder> stockContainerList, string emptyTagName)
        {
            XDocument documentElement =
                new XDocument(
                    new XElement("DocumentElement",
                        new XElement("StockContainerList",
                        from item in stockContainerList
                        let stockContainer = item.Build()
                        select new XElement("StockContainer",
                        stockContainer.Number != null ? new XElement("Number", stockContainer.Number) : null,
                        stockContainer.LocationFrom_id != null ? new XElement("LocationContainer", $"{stockContainer.LocationFrom_id}") : null,
                            stockContainer.SecondNumber != null ? new XElement("SecondNumber", stockContainer.SecondNumber) : null,
                            stockContainer.StockPositions != null ? new XElement("StockPositions",
                                from stockPosition in stockContainer.StockPositions
                                select new XElement("StockPosition",
                                stockPosition.Value != 0 ? new XElement("Value", $"{stockPosition.Value}") : null,
                                stockPosition.Currency_id != null ? new XElement("CurrencyCode", stockPosition.Currency_id) : null,
                                stockPosition.Quantity != 0 ? new XElement("Quantity", stockPosition.Quantity) : null,
                                stockPosition.Weight != 0 ? new XElement("Weight", stockPosition.Weight) : null)
                                ) : null
                            )
                        )
                    )
                );
            return documentElement;
        }
    }
}
