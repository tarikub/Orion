'********************************************************************
'**  SMS Slideshow - File Util
'********************************************************************
function ReadFile(file)
    configFile = ReadAsciiFile(file)
    if configFile <> "" then
        return ParseJson(configFile)
    end if
    return invalid
end function