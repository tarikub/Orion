'********************************************************************
'**  SMS Slideshow - Main
'********************************************************************

sub Main()
    RunScreenSaver()
end sub

sub RunScreenSaver()
    input = CreateObject("roInput")
    input.setMessagePort(m.port)

    screen = CreateObject("roSGScreen")
    displaySize = createObject("roDeviceInfo").GetDisplaySize()

    m.port = CreateObject("roMessagePort")
    screen.setMessagePort(m.port)

    newID = "global" + Stri(Rnd(10000))

    glb = screen.getGlobalNode()
    glb.id = newID

    glb.AddField("SecCounterField", "int", true)
    glb.SecCounterField = 100

    glb.AddField("CurrentlyDisplaying", "int", true)
    glb.CurrentlyDisplaying = 0

    glb.AddField("TimeLabel", "string", true)
    glb.TimeLabel = ""

    glb.AddField("ConnectionStatus", "string", true)
    glb.ConnectionStatus = ""

    glb.AddField("CurrentMonth", "string", true)
    glb.CurrentMonth = ""

    glb.AddField("CurrentDate", "int", true)
    glb.CurrentDate = 0

    glb.AddField("CurrentYear", "int", true)
    glb.CurrentYear = 0

    glb.AddField("CurrentHour", "int", true)
    glb.CurrentHour = -1

    glb.AddField("CurrentMin", "int", true)
    glb.CurrentMin = 0

    glb.AddField("phone", "string", true)
    glb.Phone = ""

    glb.AddField("Pin", "string", true)
    glb.Pin = ""

    glb.AddField("UpdateSlideShow", "boolean", true)
    glb.UpdateSlideShow = false

    glb.AddField("DisplayWidth", "int", true)
    glb.DisplayWidth = displaySize.w

    glb.AddField("DisplayHeight", "int", true)
    glb.DisplayHeight = displaySize.h

    glb.observeField("SecCounterField", m.port)

    glb.AddField("SlideShowImages", "string", true)
    glb.SlideShowImages = ""

    scene = screen.CreateScene("slideShowScene")
    screen.show()

    scene.animControl = "start"

    secCounter = 10
    sec = 0
    min = 0
    hour = -1
    month = ""
    sec = 0
    year = 0

    while(true)
        msg = wait(1000, m.port)
        if (msg <> invalid)
            msgType = type(msg)
            if msgType = "roSGScreenEvent"
                if msg.isScreenClosed() then return
            else if msgType = "roInputEvent"
                info = msg.getInfo()
                ProcessDeepLink(info)
            end if
        else


            glb.SecCounterField = secCounter

            nowDate = CreateObject("roDateTime")
            nowDate.Mark()
            nowDate.ToLocalTime()

            hour = nowDate.GetHours()
            min = nowDate.GetMinutes()
            sec = nowDate.GetSeconds()
            date = nowDate.GetMonth()
            year = nowDate.getYear()
            month = nowDate.asDateString("short-month")

            secCounter = sec

            if glb.CurrentHour <> hour or glb.UpdateSlideShow = true then
                getSlideShowImages(glb)
            end if

            glb.CurrentHour = hour
            glb.CurrentMin = min
            glb.CurrentYear = year
            glb.CurrentMonth = month
            glb.CurrentDate = date

            glb.TimeLabel = nowDate.GetWeekDay()

        end if
    end while
end sub

function getSlideShowImages(glb)
    storedphone = RegRead("Phone", "_data")
    storedPin = RegRead("Pin", "_data")

    phone = glb.Phone
    pin = glb.Pin

    if phone <> invalid then
        phone = storedphone
        glb.Phone = storedphone
    end if

    if pin <> invalid then
        pin = storedPin
        glb.Pin = storedPin
    end if

    if pin <> invalid and phone <> invalid
        apiURL = getConfigValue("ApiServer")
        port = createobject("roMessagePort")
        xfer = createobject("roURLTransfer")

        xfer.SetCertificatesFile("common:/certs/ca-bundle.crt")
        xfer.AddHeader("X-Roku-Reserved-Dev-Id", "")
        xfer.InitClientCertificates()
        xfer.AddHeader("Content-Type", "application/json")
        xfer.AddHeader("phonenumber", phone)
        xfer.AddHeader("pin", pin)

        xfer.EnableEncodings(true)
        xfer.EnableFreshConnection(true)
        xfer.SetUrl(apiURL)
        xfer.setport(port)

        timer = createobject("roTimeSpan")
        timer.mark()

        postJSON = FormatJson({})
        xfer.AsyncPostFromString(postJSON)

        while true
            event = Wait(100, port) '100 millisecond pause
            if Type(event) = "roUrlEvent" then
                responseheaders = event.GetResponseHeaders()
                if event.GetResponseCode() = 200 then
                    glb.SlideShowImages = event.getstring()
                    exit while
                else
                    xfer.asynccancel()
                    glb.ConnectionStatus = getConfigValue("InvalidPhoneOrPin")
                end if
            else
                'print "waiting for data"
            end if
            if timer.totalmilliseconds() > 30000 then

                print "excedded 30sec"
                exit while
            end if
        end while
    end if

end function
