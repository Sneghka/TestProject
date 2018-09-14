using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.Common;
using Cwc.Common.Metadata;
using Cwc.Common.UI;
using Cwc.Feedings;
using Cwc.Integration;
using Cwc.Security;
using Edsson.WebPortal.AutoTests.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using static Edsson.WebPortal.AutoTests.ObjectBuilder.GetCapturingResultBuilder;

namespace Edsson.WebPortal.AutoTests.ObjectBuilder
{
    public class GetCapturingResultBuilder
    {

        ModelContext _context = new ModelContext();
        IQueryable<CapturedContainerData> _query;

        List<CapturedContainerData> _data;
        public void Init()
        {
            var objectClassSC = MetadataHelper.LoadClassIDByType(new DataBaseParams(), typeof(StockContainer));

            _query = (from sc in _context.WP_CashCenter_StockContainer
                      join ph in (from phs in _context.WP_BaseData_ProcessingHistory.Where(ph => ph.ObjectClassID == 10000430 && ph.ProcessName == 2 && ph.ProcessPhase == 1) group phs by phs.ObjectID into g select g.OrderByDescending(t => t.DateCreated).FirstOrDefault()) on sc.id equals ph.ObjectID
                      join w in _context.WP_BaseData_Workstation on ph.WorkstationID equals w.id
                      join u in _context.WP_User on ph.AuthorID equals u.id
                      join l in _context.locations on sc.LocationFrom_id equals l.loc_nr
                      join c in _context.customers on l.cus_nr equals c.cus_nr
                      join sl in _context.WP_CashCenter_StockLocation on sc.StockLocation_id equals sl.id into slsc
                      from slocations in slsc.DefaultIfEmpty()
                      join b in _context.branches on slocations.Site_id equals b.branch_nr into slb
                      from sites in slb.DefaultIfEmpty()
                      join cb in _context.WP_CashCenter_ContainersBatch on sc.CapturingBatch_id equals cb.id into sccb
                      from cotainerBatch in sccb.DefaultIfEmpty()
                      join schcl in _context.WP_CashCenter_HeaderCardStockContainerLink on sc.id equals schcl.StockContainer_id into stockHeader
                      from stocklinks in stockHeader.DefaultIfEmpty()
                      join dhc in _context.WP_CashCenter_DailyHeaderCard.Where(d=>d.CountingType == (int)CountingType.Single) on stocklinks.DailyHeaderCardID equals dhc.Id
                      join hc in _context.WP_CashCenter_HeaderCard on dhc.HeaderCardID equals hc.id
                      select new CapturedContainerData
                      {
                          ID = sc.id,
                          BatchID = cotainerBatch.Number,
                          Location = sites.branch_cd,
                          CreditAccountNumber = sc.BankAccountNumber,
                          Reference = sc.AutomateID,
                          PreparationDate = sc.TillID,
                          StructuredInformation = sc.CustomerReference1,
                          Information = sc.CustomerReference2,
                          Barcode = string.IsNullOrEmpty(sc.PermanentNumber) ? sc.Number : sc.PermanentNumber,
                          ParentBarcode = sc.ParentContainer_id != null ? CashCenterFacade.StockContainerService.Load((long)sc.ParentContainer_id, new DataBaseParams()).Number : string.Empty,
                          IdentifierNumber = !string.IsNullOrEmpty(hc.Number) ? hc.Number : string.Empty,
                          LocationCode = l.ref_loc_nr,
                          LocationFromID = l.loc_nr,
                          CustomerCode = c.cus_nr,
                          PickupDate = sc.DateCollected != null ? sc.DateCollected.ToString() : string.Empty,
                          Timestamp = ph.DateCreated.ToString(),
                          TimestampDateTime = ph.DateCreated,
                          Hostname = w.Name,
                          ExecutedBy = u.Login        
                      });

        }


        #region DataInit
        XDocument _content;
        const string GetCapturingResultTag = "GetCapturingResult";
        const string CapturingDateTimeFromTag = "CapturingDateTimeFrom";
        const string CapturingDateTimeToTag = "CapturingDateTimeTo";
        const string CustomersTag = "Customers";
        const string CustomerNumberTag = "CustomerNumber";
        const string LocationsFromTag = "LocationsFrom";
        const string CodeTag = "Code";
        const string ContainerNumberTag = "ContainerNumber";
        const string CapturingBatchNumberTag = "CapturingBatchNumber";
        const string IdentifierNumberTag = "IdentifierNumber";
        const string CCSiteCodeTag = "CCSiteCode";
        #endregion

        #region Builders
        public GetCapturingResultBuilder()
        {
            this.Init();
            _content = new XDocument();
            _content.Document.Add(new XElement(GetCapturingResultTag));
        }

        public GetCapturingResultBuilder WithCapturingDateTimeFrom(DateTime dateTime)
        {
            var formattedDate = dateTime.ToString("yyyy-MM-ddThh:mm:ss");
            
            _content.Root.Add(new XElement(CapturingDateTimeFromTag, formattedDate));

            _query = _query.Where(q => q.TimestampDateTime > dateTime);

            return this;
        }

        public GetCapturingResultBuilder WithCapturingDateTimeTo(DateTime dateTime)
        {
            var formattedDate = dateTime.ToString("yyyy-MM-ddThh:mm:ss");
            _content.Root.Add(new XElement(CapturingDateTimeToTag, formattedDate));

            _query = _query.Where(q => q.TimestampDateTime < dateTime);

            return this;
        }

        public GetCapturingResultBuilder WithCustomer(IEnumerable<string> customers)
        {
            var cutomerElement = new XElement(CustomersTag);

            if (customers.Count() > 0)
            {
                foreach (string item in customers)
                {
                    var foundLocations = from l in _context.locations
                                   join c in _context.customers on l.cus_nr equals c.cus_nr
                                   where c.ReferenceNumber.Equals(item)
                                   select l.loc_nr;

                    _query = _query.Where(q => foundLocations.Contains(q.LocationFromID));

                    cutomerElement.Add(new XElement(CustomerNumberTag, item));
                }
            }

            _content.Root.Add(cutomerElement);

            return this;
        }

        public GetCapturingResultBuilder WithLocation(IEnumerable<string> locations)
        {
            var locationElement = new XElement(LocationsFromTag);

            if (locations.Count() > 0)
            {
                foreach (string item in locations)
                {
                    var foundLocations = from l in _context.locations
                                         where l.ref_loc_nr.Equals(item)
                                         select l.loc_nr;

                    _query = _query.Where(q => foundLocations.Contains(q.LocationFromID));

                    locationElement.Add(new XElement(CodeTag, item));
                }
            }

            _content.Root.Add(locationElement);

            return this;
        }

        public GetCapturingResultBuilder WithContainerNumber(string number)
        {
            _content.Root.Add(new XElement(ContainerNumberTag, number));

            _query = _query.Where(q => q.Barcode.Equals(number));

            return this;
        }

        public GetCapturingResultBuilder WithCapturingBatchNumber(string batchNumber)
        {
            _content.Root.Add(new XElement(CapturingBatchNumberTag, batchNumber));

            _query = _query.Where(q => q.BatchID.Equals(batchNumber));

            return this;
        }

        public GetCapturingResultBuilder WithIdentifierNumberNumber(string identifierNumber)
        {
            _content.Root.Add(new XElement(IdentifierNumberTag, identifierNumber));

            _query = _query.Where(q => q.IdentifierNumber.Equals(identifierNumber));

            return this;
        }

        public GetCapturingResultBuilder WithCCSite(string siteCode)
        {
            _content.Root.Add(new XElement(CCSiteCodeTag, siteCode));

            _query = _query.Where(q =>q.Location.Equals(siteCode));
            return this;
        }

        #endregion

        public ReturnValue SendFeedingAndGetResponse()
        {
            var requestEncryptedResult = this.PrepareData();

            var decryptedResut = this.TryDecryptResponse(requestEncryptedResult);

            var doc = XDocument.Parse(decryptedResut);
            var result = _query.ToList<CapturedContainerData>();

            _data = (from q in result
                     select new CapturedContainerData
                     {
                         

                     }).ToList();

            return new ReturnValue { Doc = doc };
        }

        #region Private Methods
        private string PrepareData()
        {
            var address = @"http://portal/v5.03_HQ//WebServiceFeedings.asmx?op=SendFeeding";

            LoginResult lr = SecurityFacade.LoginService.GetAdministratorLogin();

            var result = FeedingFacade.ValidatedFeedingEncryptorService.GetFeedingEncryptedXMLTags(lr.UserID, _content.ToString());

            var xmlRequest = string.Format(
@"<?xml version='1.0' encoding='utf-8'?>
<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
  <soap:Body>
    <SendFeeding xmlns='WebServices.WebServiceFeedings'>
      {0}
    </SendFeeding>
  </soap:Body>
</soap:Envelope>", result.Value);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlRequest);

            var response = PerformRequestAndGetResponse(address, doc);

            return response;
        }

        private string PerformRequestAndGetResponse(string address, XmlDocument doc)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(address);
            webRequest.Headers.Add("SOAPAction", "SendFeeding");
            webRequest.Headers.Add("Content-lenght", doc.OuterXml.Length.ToString(CultureInfo.InvariantCulture));
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";

            var requestStream = webRequest.GetRequestStream();
            doc.Save(requestStream);
            requestStream.Close();

            WebResponse response = webRequest.GetResponse();

            var responseStream = response.GetResponseStream();
            var streamReader = new StreamReader(responseStream);
            var strResponse = streamReader.ReadToEnd();
            return strResponse;
        }

        private string TryDecryptResponse(string encryptedResponse)
        {
            string decryptedResponse = string.Empty;

            LoginResult lr = SecurityFacade.LoginService.GetAdministratorLogin();
            XmlDocument responseDoc = new XmlDocument();
            responseDoc.LoadXml(encryptedResponse);
            XPathNavigator navigator = responseDoc.CreateNavigator();

            if (navigator.NameTable != null)
            {
                XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
                manager.AddNamespace("wsf", "WebServices.WebServiceFeedings");
                manager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                XPathNodeIterator it = navigator.Select("soap:Envelope/soap:Body/wsf:SendFeedingResponse/wsf:ResponseContent", manager);

                if (it.MoveNext())
                {
                    string encryptedXml = it.Current.Value;
                    UserAccount user = SecurityFacade.UserService.GetUser(lr.UserID);
                    var res = SecurityFacade.UserService.LoadHashPassword(user.ID);
                    if (res.IsSuccess)
                    {
                        string decryptedXml = DecryptorFacade.UniqueInstance.GetDecryptedXML(encryptedXml, user.Login, res.Value);
                        it.Current.InnerXml = decryptedXml;
                        decryptedResponse = responseDoc.InnerXml;
                    }
                }
            }

            return !string.IsNullOrEmpty(decryptedResponse) ? decryptedResponse : encryptedResponse;
        }

        private void AddAmounts(CapturedContainerData data)
        {
            data.DeclaredAmounts = new List<DeclaredAmount>();

            var stockPositionResult = CashCenterFacade.DiscrepancyService.GetExpectedDataSetForDepositCountingDiscrepancy(data.LocationFromID, data.Barcode);

            if (!stockPositionResult.IsSuccess)
            {
                throw new InvalidOperationException(stockPositionResult.GetMessage());
            }

            var positions = stockPositionResult.Value.StockPositions;

            var position = from pos in positions
                           where pos.IsTotal
                           group pos by pos.Currency_id
                                                       into positionsGroup
                           select
                               new DeclaredAmount()
                               {
                                   IsTotalBool = positionsGroup.First().IsTotal,
                                   Amount = positionsGroup.Sum(p => p.Value),
                                   Weight = positionsGroup.Sum(p => p.Weight),
                                   Currency = positionsGroup.Key
                               };


        }

        private void AddAnnomalies()
        {

        }
        #endregion
    }

    public class ReturnValue
    {
        public CapturedContainerData capturedData { get; set; }
        public XDocument Doc { get; set; }
        public List<StockPosition> stockPositions { get; set; }
    }
}
