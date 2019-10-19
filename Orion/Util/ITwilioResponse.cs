using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.Util
{
    public interface ITwilioResponse
    {
        MessageProcessingType ProcessingType();
    }
}
