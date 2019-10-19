using System;

namespace Orion.Util
{
    public class TwilioResponse : ITwilioResponse
    {
        public string From { get; set; }
        public string DateCreated { get; set; }
        public string Body { get; set; }
        public string MessageSid { get; set; }
        public string Medias { get; set; }

        public MessageProcessingType ProcessingType()
        {
            var routes = Enum.GetNames(typeof(MessageProcessingType));
            var rtnProcessType = MessageProcessingType.None;
            // User messages
            foreach(var route in routes)
            {
                if(Body.ToLower().Contains(route.ToLower()))
                {
                    if( Enum.TryParse(route, out rtnProcessType))
                    {
                        return rtnProcessType;
                    }
                }
            }
            // Media is attached
            if(rtnProcessType == MessageProcessingType.None)
            {
                if(!String.IsNullOrEmpty(Medias))
                {
                    rtnProcessType = MessageProcessingType.UploadMedia;
                }
            }
            return rtnProcessType;
        }
    }
}
