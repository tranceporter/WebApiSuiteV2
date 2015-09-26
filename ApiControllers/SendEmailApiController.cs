using SendGrid;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPISuite.Models;
using WebAPISuite.Providers;

namespace WebAPISuite.ApiControllers
{
    public class SendEmailApiController : ApiController
    {
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public HttpResponseMessage Test()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "This is working!");
        }

        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public HttpResponseMessage GetClientSettings([FromUri]string clientName)
        {
            using (var clientContext = new ClientContext())
            {
                var client = clientContext.Clients.SingleOrDefault(c => c.Name.Equals(clientName, System.StringComparison.InvariantCultureIgnoreCase));
                if (client == null || client.ClientSettings == null)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }

                var settings = client.ClientSettings;

                return Request.CreateResponse(HttpStatusCode.OK, new { 
                    settings.EnableFileUpload, 
                    settings.GoogleAdwordsEnabled,
                    settings.GoogleConversionId,
                    settings.GoogleConversionColour,
                    settings.GoogleConversionCurrency,
                    settings.GoogleConversionValue,
                    settings.GoogleConversionLabel,
                    settings.GoogleConversionLanguage,
                    settings.GoogleRemarketingOnly
                });
            }
        }

        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> SendEmail()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, "Unsupported media type.");
            }

            // Read the file and form data.
            var provider = new MultipartFormDataMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            if (string.IsNullOrWhiteSpace(provider.FormData["email"]))
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed);
            }

            // Extract the fields from the form data.
            var stringbuilder = new StringBuilder();
            stringbuilder.AppendLine(string.Format("Name: {0}", provider.FormData["name"]));
            stringbuilder.AppendLine(string.Format("Email: {0}", provider.FormData["email"]));
            stringbuilder.AppendLine(string.Format("Phone: {0}", provider.FormData["phone"]));
            stringbuilder.AppendLine(string.Format("Message: {0}", provider.FormData["message"]));

            var clientName = provider.FormData["clientName"];
            if (string.IsNullOrWhiteSpace(clientName))
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            using (var clientContext = new ClientContext())
            {
                var client = clientContext.Clients.SingleOrDefault(c => c.Name.Equals(clientName, System.StringComparison.InvariantCultureIgnoreCase));
                if (client == null || client.ClientSettings == null)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }

                var transportWeb = new Web("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");

                var message = CreateSendGridMessage(client.Email, provider.FormData["email"], provider.FormData["name"], stringbuilder.ToString().Replace("\r\n", "<br/>"), 
                    provider.Files, client.ClientSettings);

                await transportWeb.DeliverAsync(message);

                if (client.ClientSettings.AutoReplyToCustomer)
                {
                    message = CreateSendGridMessage(provider.FormData["email"], client.Email, client.Name, "Thank you for contacting us! We will be in touch with you shortly.", provider.Files, client.ClientSettings, true);
                    await transportWeb.DeliverAsync(message);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private SendGridMessage CreateSendGridMessage(string emailTo, string emailFrom, string name, string message, List<UploadedFile> files, ClientSetting clientSettings, bool autoReply = false)
        {
            // Create the email object first, then add the properties.
            SendGridMessage myMessage = new SendGridMessage();

            myMessage.AddTo(emailTo);

            myMessage.From = new MailAddress(emailFrom, name);

            myMessage.Subject = string.IsNullOrWhiteSpace(clientSettings.SubjectLine) ?
                autoReply ? 
                "Thank you for contacting us" :
                string.Format("{0} wants to get in touch with you!", name) :
                clientSettings.SubjectLine;

            myMessage.Html = message;

            if (clientSettings.EnableFileUpload && !autoReply)
            {
                foreach (var file in files)
                {
                    myMessage.AddAttachment(new MemoryStream(file.FileBytes), file.FileName);
                }
            }

            return myMessage;
            
        }
    }
}
