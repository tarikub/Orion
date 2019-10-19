using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Orion.Util;
using System;
using System.Threading.Tasks;

namespace Orion.Functions
{
    public static class FnOrchestrator
    {
        [FunctionName(nameof(Process))]
        public static async Task<IActionResult> Process(
          [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
          [DurableClient]IDurableClient client,
          ILogger log)
        {
            var res = TwilioUtil.Get(req);
            var msgProcessed = false;

            switch (res.ProcessingType())
            {
                case MessageProcessingType.Register:
                    {
                        msgProcessed = true;
                        await client.StartNewAsync(nameof(FnUser.UserRegister), res);
                        break;
                    }
                case MessageProcessingType.UploadMedia:
                    {
                        msgProcessed = true;
                        await client.StartNewAsync(nameof(FnSlideShow.UploadMedia), res);
                        break;
                    }
                case MessageProcessingType.Reset:
                    {
                        msgProcessed = true;
                        await client.StartNewAsync(nameof(FnSlideShow.Reset), res);
                        break;
                    }
                case MessageProcessingType.Delete:
                    {
                        msgProcessed = true;
                        await client.StartNewAsync(nameof(FnSlideShow.Delete), res);
                        break;
                    }
                case MessageProcessingType.More:
                    {
                        msgProcessed = true;
                        await client.StartNewAsync(nameof(FnUser.UserHelp), res);
                        break;
                    }
                default:
                    {
                        msgProcessed = true;
                        if (!String.IsNullOrEmpty(res.Body))
                        {
                            await client.StartNewAsync(nameof(FnUser.UserVerify), res);
                        }
                        break;
                    }
            }

            return msgProcessed == true
                ? (ActionResult)new OkObjectResult(msgProcessed)
                : new BadRequestObjectResult(TwilioMessages.NotAbleToRegister);
        }
    }
}
