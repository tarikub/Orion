'********************************************************************
'**  SMS Slideshow - Message Util
'********************************************************************
function GetMessageValue(key)
    configSettings = ReadFile("pkg:/message/message.json")
    if configSettings <> invalid then
        if configSettings[key] <> invalid then
            return configSettings[key]
        else
            return ""
        end if
    end if
    return ""
end function