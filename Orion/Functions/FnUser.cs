using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Orion.Util;
using OrionEntities;
using Orion.Data;
using System;
using System.Threading.Tasks;

namespace Orion.Functions
{
    public static class FnUser
    {
        [FunctionName(nameof(UserRegister))]
        public static async Task<IActionResult> UserRegister(
             [OrchestrationTrigger]IDurableOrchestrationContext context,
             [Table(nameof(User))]CloudTable userTable,
             [Table(nameof(SlideShow))]CloudTable slideShowTable,
            ILogger log)
        {
            var userClient = new DataAccess<User>(userTable);
            var slideShowClient = new DataAccess<SlideShow>(slideShowTable);
            OrionEntities.User user = null;

            var twilioResponse = context.GetInput<TwilioResponse>();
            var newPin = Verification.NewPIN();
            user = await userClient.GetAsync(nameof(User), twilioResponse.From);

            if (user == null) // Register user
            {
                user = new OrionEntities.User()
                {
                    RowKey = twilioResponse.From,
                    Timestamp = DateTime.UtcNow,
                    PIN = newPin,
                    PhoneNumber = twilioResponse.From
                };

                await userClient.InsertAsync(user);
                log.Log(LogLevel.Information, $"User {user.PhoneNumber} is added to storage");
            }
            else
            {
                user.PIN = newPin;
                await slideShowClient.InsertAsync(new SlideShow
                {
                    PIN = user.PIN,
                    Status = UserStatus.Active,
                    RowKey = $"{user.PhoneNumber}{user.PIN}",
                    Phone = user.PhoneNumber
                });
                await userClient.ReplaceAsync(user);
            }


                TwilioUtil.Notify(user.RowKey, String.Format(TwilioMessages.VerifyPhone, newPin), log);

            return user != null
                ? (ActionResult)new OkObjectResult(user)
                : new BadRequestObjectResult(TwilioMessages.NotAbleToRegister);
        }
        [FunctionName(nameof(UserVerify))]
        public static async Task<IActionResult> UserVerify(
           [OrchestrationTrigger]IDurableOrchestrationContext context,
           [Table(nameof(User))]CloudTable userTable,
           [Table(nameof(SlideShow))]CloudTable slideShowTable,
           ILogger log)
        {
            var userClient = new DataAccess<User>(userTable);
            var slideShowClient = new DataAccess<SlideShow>(slideShowTable);
            var twilioResponse = context.GetInput<TwilioResponse>();

            User user = null;
            var verified = false;

            var pin = twilioResponse.Body;

            user = await userClient.GetAsync(nameof(User), twilioResponse.From);

            if (user != null) // Verify user
            {

                if (string.Equals(user.PIN, pin, StringComparison.OrdinalIgnoreCase))
                {
                    verified = true;
                   

                    // Check if number has been used before
                    var slideShow = await slideShowClient.GetAsync(nameof(SlideShow), $"{user.PhoneNumber}{user.PIN}");

                    if (slideShow == null)
                    {
                        await slideShowClient.InsertAsync(new SlideShow
                        {
                            PIN = user.PIN,
                            Status = UserStatus.Active,
                            RowKey = $"{user.PhoneNumber}{user.PIN}",
                            Phone = user.PhoneNumber
                        });
                    }

                    TwilioUtil.Notify(user.RowKey, String.Format(TwilioMessages.AccountVerified, user.PIN), log);
                }
                else
                {
                    TwilioUtil.Notify(user.RowKey, TwilioMessages.InvalidPin, log);
                }

            }

            return verified == true
               ? (ActionResult)new OkObjectResult(user)
               : new BadRequestObjectResult(TwilioMessages.NotAbleToRegister);
        }

        [FunctionName(nameof(UserHelp))]
        public static async Task<IActionResult> UserHelp(
          [OrchestrationTrigger]IDurableOrchestrationContext context,
           ILogger log)
        {

            var twilioResponse = context.GetInput<TwilioResponse>();
            TwilioUtil.Notify(twilioResponse.From, TwilioMessages.MoreText, log);
            bool helpSent = await Task.FromResult(true);
            return helpSent == true
               ? (ActionResult)new OkObjectResult(true)
               : new BadRequestObjectResult(TwilioMessages.NotAbleToRegister);
        }

    }
}
