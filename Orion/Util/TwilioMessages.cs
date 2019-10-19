namespace Orion.Util
{
    public static class TwilioMessages
    {
        private const string AppName = "SmsScreenSaver";
        private const string More = "Text MORE for more options";
        public static string Register = "Text REGISTER to start sending photos to your screensaver.";
        public static string NotFound = $"Phone number not found in the system. Please register. {More}";
        public static string NotAbleToRegister = $"Unable to register. {More}";
        public static string MessageProcessed = "Message is processed";
        public static string PhotoIsAddedToSlideShow = "Photo is added to slideshow";
        public static string PhotoDeleted = "Photo has been deleted from slideshow";
        public static string InvalidPin = "Please enter a valid pin.";
        public static string ResettedAccount = "We have resetted your account.";
        public static string VerifyPhone = $"The Roku App {AppName} needs to verify your phone number. Please enter {{0}} to verify your phone number. {More}";
        public static string InvalidPIN = $"PIN entered is not valid. Please enter {{0}} to verify your phone number. {More}";
        public static string AccountVerified = $"Account has been verified. You can now send pictures by texting to your Roku screen saver application. {More}";
        public static string NoMedia = $"No Media file available. {More}";
        public static string MoreText =
            $"Forward photos to this number and your photos will show up on your Roku {AppName} app." +
            "Text 'Register' to sign up. \n " +
            "Text 'Delete' and photo number to remove photo from slideshow (e.g. delete 1) \n" +
            "Text 'Reset' to remove all photos and reset account. \n"; 
    }
}
