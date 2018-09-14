using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Cwc.Localization;
using Cwc.Feedings;
using Cwc.BaseData.Classes;
using Cwc.Sync;

/// <summary>
/// Summary description for WebServiceFeedings
/// </summary>
[WebService(Namespace = "WebServices.WebServiceFeedings")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class WebServiceFeedings : System.Web.Services.WebService
{

    public WebServiceFeedings()
    {
        System.Diagnostics.Debug.WriteLine("WebServiceFeedings: created.");
        ConfigurationKeySet.Load();
		SyncConfiguration.LoadExportMappings();
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    [SoapDocumentMethod(Action = "SendFeeding")]
    [WebMethod]
    public void SendFeeding(string Login, string Password, string Hash, string Content,
        out string ValidationResult, out bool Warning, out string ErrorMessage, out string ResponseContent, out string ResponseHash)
    {
        SyncConfiguration.LoadExportMappings();
        System.Diagnostics.Debug.WriteLine("WebServiceFeedings: SendFeeding start.");
        ValidationResult = FeedingValidationResult.Failed.ToString();
        Warning = false;
        ErrorMessage = string.Empty;
        ResponseContent = string.Empty;
        ResponseHash = string.Empty;

        try
        {
            HttpContext context = this.Context;

            DeliveredFeeding deliveredFeeding = new DeliveredFeeding(DeliveryMethod.SOAP);
            deliveredFeeding.Login = Login;
            deliveredFeeding.Password = Password;
            deliveredFeeding.Hash = Hash;
            deliveredFeeding.Content = Content;

            ValidatedFeedingResponse[] responses = FeedingFacade.ValidatedFeedingService.ProcessValidatedFeedings(
                new DeliveredFeeding[] { deliveredFeeding },
                context.Request.TotalBytes,
                context.Request.UserHostAddress,
                null);

            if (responses == null || responses.Length != 1)
            {
                ErrorMessage = LocalizationFacade.UniqueInstance.ResourceServicesGetString("Cwc.Feedings.WarningQueryDoesNotHaveAFeeding", typeof(FeedingFacade));
                return;
            }

            ValidatedFeedingResponse response = responses[0];
            ValidatedFeeding validatedFeeding = response.ValidatedFeeding;
            ValidationResult = response.ValidationResult.ToString();
            Warning = validatedFeeding != null ? validatedFeeding.Warning : false;
            ErrorMessage = response.ErrorMessage;
            ResponseContent = response.EncryptedContent;
            ResponseHash = response.Hash;
        }
        catch (Exception exn)
        {
            ErrorMessage = exn.Message;
        }
        finally
        {
            System.Diagnostics.Debug.WriteLine("WebServiceFeedings: SendFeeding end.");
        }
    }
}

