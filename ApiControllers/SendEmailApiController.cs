using SendGrid;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
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

            // Extract the fields from the form data.
            var stringbuilder = new StringBuilder();
            stringbuilder.AppendLine(string.Format("Name: {0}", provider.FormData["name"]));
            stringbuilder.AppendLine(string.Format("Email: {0}", provider.FormData["email"]));
            stringbuilder.AppendLine(string.Format("Phone: {0}", provider.FormData["phone"]));
            stringbuilder.AppendLine(string.Format("Message: {0}", provider.FormData["message"]));

            // Create the email object first, then add the properties.
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo("shreyas.zanpure@gmail.com");
            myMessage.AddTo("mw@martinwrightdesign.com");
            myMessage.From = new MailAddress(provider.FormData["email"], provider.FormData["name"]);
            myMessage.Subject = string.Format("{0} wants to get in touch with you!", provider.FormData["name"]);
            myMessage.Html = stringbuilder.ToString().Replace("\r\n", "<br/>");

            foreach (var file in provider.Files)
            {
                myMessage.AddAttachment(new MemoryStream(file.FileBytes), file.FileName);
            }

            var transportWeb = new Web("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");

            // Send the email.
            await transportWeb.DeliverAsync(myMessage);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
