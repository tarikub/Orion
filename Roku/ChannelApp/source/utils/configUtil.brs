'********************************************************************
'**  SMS Slideshow - Configuration Util
'********************************************************************
function getConfigValue(key)
    configSettings = ReadFile("pkg:/config/config.json")
    if configSettings <> invalid then
        if configSettings[key] <> invalid then
            return configSettings[key]
        else
            return ""
        end if
    end if
    return ""
end function