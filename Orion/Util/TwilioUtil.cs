using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Orion.Util
{
    public static class TwilioUtil
    {
        public static TwilioResponse Get(HttpRequest request)
        {
            var rtn = new TwilioResponse
            {
                Body = request.Form["Body"],
                DateCreated = request.Form["DateCreated"],
                MessageSid = request.Form["MessageSid"],
                From = request.Form["From"].ToString().Replace("+", "")
            };

            if (!string.IsNullOrEmpty(request.Form["NumMedia"]) && request.Form["NumMedia"] != "0")
            {
                rtn.Medias = "";
                var numMedia = int.Parse(request.Form["NumMedia"]);
                var strBuilder = new StringBuilder();
                for (int resourceId = 0; resourceId < numMedia; resourceId++)
                {
                    var mediaUrl = request.Form[$"MediaUrl{resourceId}"];
                    strBuilder.Append($"{mediaUrl} ");

                }
                rtn.Medias = strBuilder.ToString();
            }

            return rtn;
        }

        public static void Notify(string to, string body, ILogger log)
        {
            var accountSid = GetEnvironmentVariable("accountSid");
            var authToken = GetEnvironmentVariable("authToken");
            var appPhone = GetEnvironmentVariable("appPhone");

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
            body: body,
            from: new Twilio.Types.PhoneNumber(appPhone),
            to: new Twilio.Types.PhoneNumber(to)
        );
            if (!string.IsNullOrEmpty(message.Sid))
            {
                log.Log(LogLevel.Information, $"Message {body} sent to {to}");
            }
            else
            {
                log.Log(LogLevel.Information, $"Failed to send Message {body} sent to {to}");
            }
        }

        private static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, System.EnvironmentVariableTarget.Process);

        }
    }
}
