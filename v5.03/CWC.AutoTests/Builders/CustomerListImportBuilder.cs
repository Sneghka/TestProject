using Cwc.Feedings;
using Cwc.Localization;
using Cwc.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Edsson.WebPortal.AutoTests.ObjectBuilder
{
    public class CustomerListImportBuilder
    {
        public void Init()
        {
            string xml = string.Format(
                @"<?xml version='1.0' standalone='yes'?>
                                                <DocumentElement>
                                                  <CompanyList mapper = "">
                                                    <Company >
                                                      <Number >C0001</ Number>
                                                      <Name >Company name</ Name>
                                                      <IsEnabled >yes</ IsEnabled>
                                                    </Company>
                                                  </CompanyList>
                                                </DocumentElement>"
                                                );
            var lr = SecurityFacade.LoginService.GetAdministratorLogin();

            var result = FeedingFacade.ValidatedFeedingEncryptorService.GetFeedingEncryptedXMLTags(lr.UserID, xml);


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

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlRequest);

            var innerNodes = doc.DocumentElement.ChildNodes[0].ChildNodes[0].ChildNodes;

            var pass = innerNodes.Item(1).InnerText;
            var hash = innerNodes.Item(2).InnerText;
            var cont = innerNodes.Item(3).InnerText;

            var serv = new SendFedding.WebServiceFeedings();

            bool warning;
            string message, content, resphash, validres;

           

            var deliveredFeeding = new DeliveredFeeding(DeliveryMethod.SOAP);
            deliveredFeeding.Login = "admin";
            deliveredFeeding.Password = pass;
            deliveredFeeding.Hash = hash;
            deliveredFeeding.Content = cont;

            ValidatedFeedingResponse[] responses = FeedingFacade.ValidatedFeedingService.ProcessValidatedFeedings(
                new DeliveredFeeding[] { deliveredFeeding },
                100,
                "192.168.138",
                null);

            if (responses == null || responses.Length != 1)
            {
                message = LocalizationFacade.UniqueInstance.ResourceServicesGetString("Cwc.Feedings.WarningQueryDoesNotHaveAFeeding", typeof(FeedingFacade));
                return;
            }

            ValidatedFeedingResponse response = responses[0];
            ValidatedFeeding validatedFeeding = response.ValidatedFeeding;
            validres = validatedFeeding.ValidationResult.ToString();
            warning = validatedFeeding.Warning;
            message = validatedFeeding.ErrorMessage;
            content = response.EncryptedContent;
            resphash = response.Hash;



        }
    }
}
