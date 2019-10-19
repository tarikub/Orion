# Serverless App from Design to Production: A Case Study 

The goal of the project is to learn about serverless technologies and service orchestration using Azure Functions. To learn more about the process and design considerations for the product you can check the summary article on medium.com. A great byproduct of the learning process is a Roku channel app that is currently published in the Roku Channel store. 

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. 

### Prerequisites

* [Visual Studio 2019 with Azude Dev Tools](hhttps://visualstudio.microsoft.com/vs/) 
* [Eclipse with BrightScript plugin](https://www.eclipse.org/) 
* [ngrok](https://ngrok.com)
* Roku steaming box or TV (optional)


### Running Locally 

#### Set Up

1. Sign up for a free [Twilio account](www.twilio.com/referral/gJ8V2m)
2. Clone this Git Repo
3. Create a new file `local.settings.json` with the following content and add it under the `Orion\Orion\` folder 
```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "accountSid": "",
    "authToken": "",
    "appPhone": ""
  }
}
```
4. update the `accountSid`, `authToken` & `appPhone` from the [Twilio Console](https://www.twilio.com/console) and the [numbers page](https://twilio.com/console/phone-numbers/incoming) respectively
5. Create a new file `config.json` with the following content and add it under the `Orion\Roku\ChannelApp\config` folder 

```
{
    "ApiServer": "", 
    "ChannelPhoneNumber": ""
}
```
*APiServer* - Update with value from `Running Locally` steps
*ChannelPhoneNumber*: Update with value with your Twilio phone number

#### Running Locally
1. Run VS debugger (F5) and check the port that the application is running under usually 7071)
1. Download ngRok and run the command `ngRok http 7071`
1. Jot down the public web endpoint (note the `/api/` URL needs to be in the webhook link)
1. Go to Twilio's [numbers page](https://twilio.com/console/phone-numbers/incoming) and update the WebHook for incoming SMS with the link form grok + `\api\process` it should look something like `http://4623e~.ngrok.io/api/process`
1. Text `Register` to your Twilio number - you should get a response from your function
1. Respond back with the `pin` forwarded to you from the function and the application should now be ready to accept your SMS photos
1. Text couple of photos to your number for testing 


#### Running on Roku Box (optional)
1. [Turn dev features](https://blog.roku.com/developer/developer-setup-guide) on for your Roku streaming box or TV
1. [Set up your Eclipse environment](https://developer.roku.com/docs/developer-program/getting-started/ide-support.md) for BrightScript Development
1. Create a new Eclipse BrightScript project from `Existing Source` and choose the folder `Orion\Roku\ChannelApp`
1. Deploy to your streaming box or TV by right clicking the project and selecting the export menu
1. Make sure the function & ngRok is running locally and you have completed the steps from `Running Locally` before deploying your Roku app to see the images from your SMS feed

## Built With

* [Azure Functions](https://azure.microsoft.com/en-us/services/functions/)
* [Twilio](https://www.twilio.com/)
* [BrightScript](https://developer.roku.com/docs/references/brightscript/language/brightscript-language-reference.md)


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Tip of the hat to @jeremylikness for the awesome presentation on Durable Functions at VS Live San Diego 2019.
