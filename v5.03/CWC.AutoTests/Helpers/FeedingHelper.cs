using Cwc.Common;
using Cwc.Feedings;
using Cwc.Security;
using CWC.AutoTests.WebServiceFeedings;
using System.Threading.Tasks;
using System.Xml;

namespace CWC.AutoTests.Helpers
{
    public class FeedingHelper
    {
        public SendFeedingResponse SendFeeding(string xml)
        {
            var lr = SecurityFacade.LoginService.GetAdministratorLogin();
            ValueResult<string> result =
                FeedingFacade.ValidatedFeedingEncryptorService.GetFeedingEncryptedXMLTags(lr.UserID, xml);

            var xmlRequest = string.Format(
                               @"<?xml version='1.0' encoding='utf-8'?>
                                <soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                                  <soap:Body>
                                    <SendFeeding xmlns='WebServices.WebServiceFeedings'>
                                      {0}
                                    </SendFeeding>
                                  </soap:Body>
                                </soap:Envelope>",
                                                result.Value);

            var doc = new XmlDocument();
            doc.LoadXml(xmlRequest);
            var feedingRoot = doc.DocumentElement.FirstChild.FirstChild.ChildNodes;
            var login = feedingRoot.Item(0).InnerText;
            var pass = feedingRoot.Item(1).InnerText;
            var hash = feedingRoot.Item(2).InnerText;
            var cont = feedingRoot.Item(3).InnerText;

            WebServiceFeedings.SendFeedingResponse response;

            using (var service = new WebServiceFeedings.WebServiceFeedingsSoapClient())
            {
                //service.SendFeeding(login, pass, hash, cont, out warning, out errrorMsg, out responseConent, out responseHash);                               
                var res = service.SendFeedingAsync(login, pass, hash, cont);
                Task.WaitAll(res);

                response = res.Result;
            }

            return response;
        }

    }
}
