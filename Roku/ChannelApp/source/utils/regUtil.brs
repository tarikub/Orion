'********************************************************************
'**  SMS Slideshow - Registry Util
'********************************************************************
function RegRead(key, section = invalid)
    if section = invalid then
        section = "Default"
    end if
    sec = CreateObject("roRegistrySection", section)
    if sec.Exists(key) then
        return sec.Read(key)
    end if
    return invalid
end function

function RegWrite(key, val, section = invalid)
    if section = invalid then
        section = "Default"
    end if
    sec = CreateObject("roRegistrySection", section)
    sec.Write(key, val)
    sec.Flush() 'commit it
end function

function RegDelete(key, section = invalid)
    if section = invalid then
        section = "Default"
    end if
    sec = CreateObject("roRegistrySection", section)
    sec.Delete(key)
    sec.Flush()
end function