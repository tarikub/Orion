'********************************************************************
'**  SMS Slideshow - Time Util
'********************************************************************
function getFormattedTime(hour, min)
    ampm = "AM"

    if hour > 12 then
        hour = hour - 12
        ampm = "PM"
    end if

    if hour = 0 then
        hour = 12
        ampm = "AM"
    end if

    formattedHour = Str(hour)
    formattedMin = min.ToStr()

    if min < 10 then
        formattedMin = 0.ToStr() + formattedMin
    end if

    return formattedHour + ":" + formattedMin + " " + ampm
end function