using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Orion.Util;
using OrionEntities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Orion.Data;
using System.Collections.Generic;

namespace Orion.Functions
{
    public static class FnSlideShow
    {
        const string FEED_DELIMITER = " ";
        [FunctionName(nameof(SlideShowFeed))]
        public static async Task<IActionResult> SlideShowFeed(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Table(nameof(SlideShow))]CloudTable slideShowTable,
            ILogger log)
        {
            var phone = req.Headers["PhoneNumber"];
            var pin = req.Headers["pin"];
            SlideShow slideShow = null;

            if (!String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(phone))
            {
                var slideShowClient = new DataAccess<SlideShow>(slideShowTable);
                slideShow = await slideShowClient.GetAsync(nameof(SlideShow), $"{phone}{pin}");

                log.Log(LogLevel.Information, $"Feed requested for {phone} :: {pin}");
            }


            return (slideShow != null && string.Equals(slideShow.PIN, pin, StringComparison.OrdinalIgnoreCase))
                ? (ActionResult)new OkObjectResult(slideShow.Medias.Split(FEED_DELIMITER).Where(media => !string.IsNullOrWhiteSpace(media)).ToList())
                : new BadRequestObjectResult("Error processing feed");
        }

        [FunctionName(nameof(UploadMedia))]
        public static async Task<IActionResult> UploadMedia(
            [OrchestrationTrigger]IDurableOrchestrationContext context,
            [Table(nameof(SlideShow))]CloudTable slideShowTable,
            [Table(nameof(User))]CloudTable userTable,
           ILogger log)
        {

            var twilioResponse = context.GetInput<TwilioResponse>();
            var slideShowClient = new DataAccess<SlideShow>(slideShowTable);
            var userClient = new DataAccess<User>(userTable);

            var user = await userClient.GetAsync(nameof(User), twilioResponse.From);

            if (user != null) // Verify user
            {
                var slideShow = await slideShowClient.GetAsync(nameof(SlideShow), $"{user.PhoneNumber}{user.PIN}");

                if (slideShow != null && !string.IsNullOrEmpty(twilioResponse.Medias))
                {
                    slideShow.Medias += $"{twilioResponse.Medias}{FEED_DELIMITER}";
                    await slideShowClient.ReplaceAsync(slideShow);
                    log.Log(LogLevel.Information, $"Media updated for user {twilioResponse.From}");
                    TwilioUtil.Notify(user.RowKey, TwilioMessages.PhotoIsAddedToSlideShow, log);
                }
            }
            return new OkObjectResult(true);
        }

        [FunctionName(nameof(Reset))]
        public static async Task<IActionResult> Reset(
        [OrchestrationTrigger]IDurableOrchestrationContext context,
        [Table(nameof(SlideShow))]CloudTable slideShowTable,
        [Table(nameof(User))]CloudTable userTable,
        ILogger log)
        {

            var twilioResponse = context.GetInput<TwilioResponse>();
            var slideShowClient = new DataAccess<SlideShow>(slideShowTable);
            var userClient = new DataAccess<User>(userTable);

            var user = await userClient.GetAsync(nameof(User), twilioResponse.From);

            if (user != null) // Verify user
            {
                var slideShow = await slideShowClient.GetAsync(nameof(SlideShow), $"{user.PhoneNumber}{user.PIN}");

                if (slideShow != null)
                {
                    var images = slideShow.Medias.Split(FEED_DELIMITER).Where(media => !string.IsNullOrWhiteSpace(media));

                    slideShow.Medias = "";
                    await slideShowClient.ReplaceAsync(slideShow);
                    log.Log(LogLevel.Information, $"Media resetted for user {twilioResponse.From}");
                }

                TwilioUtil.Notify(user.RowKey, TwilioMessages.ResettedAccount, log);
            }
            return new OkObjectResult(true);
        }

        [FunctionName(nameof(Delete))]
        public static async Task<IActionResult> Delete(
        [OrchestrationTrigger]IDurableOrchestrationContext context,
        [Table(nameof(SlideShow))]CloudTable slideShowTable,
        [Table(nameof(User))]CloudTable userTable,
        ILogger log)
        {

            var twilioResponse = context.GetInput<TwilioResponse>();
            var slideShowClient = new DataAccess<SlideShow>(slideShowTable);
            var userClient = new DataAccess<User>(userTable);

            var user = await userClient.GetAsync(nameof(User), twilioResponse.From);

            if (user != null) // Verify user
            {
                var slideShow = await slideShowClient.GetAsync(nameof(SlideShow), $"{user.PhoneNumber}{user.PIN}");

                if (slideShow != null && !string.IsNullOrEmpty(slideShow.Medias))
                {
                    var images = slideShow.Medias.Split(FEED_DELIMITER).Where(media => !string.IsNullOrWhiteSpace(media));
                    var checkidxToDelete = twilioResponse.Body.Split(FEED_DELIMITER).Where(userText => int.TryParse(userText, out var indexToDelete)).FirstOrDefault();

                    if (int.TryParse(checkidxToDelete, out int numToDelete) && numToDelete > 0 && numToDelete <= images.Count())
                    {
                        var idxToDelete = --numToDelete;
                        var updatedList = new List<string>(images);
                        var removedMsgURL = updatedList[idxToDelete];
                        updatedList.RemoveAt(idxToDelete);
                        slideShow.Medias = String.Join(FEED_DELIMITER, updatedList);
                        await slideShowClient.ReplaceAsync(slideShow);

                        log.Log(LogLevel.Information, $"Media deleted for user {twilioResponse.From}");
                        TwilioUtil.Notify(user.RowKey, TwilioMessages.PhotoDeleted, log);
                    }
                }
            }
            return new OkObjectResult(true);
        }
    }
}
