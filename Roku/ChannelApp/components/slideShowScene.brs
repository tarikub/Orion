'********************************************************************
'**  SMS Slideshow - Slideshow Component
'********************************************************************
function onTimeUpdated() as Void
    lblDayofWeek = m.top.findNode("lblDayofWeek")
    lblTime = m.top.findNode("lblTime")
    recOpacity = m.top.findNode("recOpacity") 
    
            
    if m.global.CurrentHour <> -1 
        lblDayofWeek.Text = m.global.CurrentMonth
        formattedTime = getFormattedTime(m.global.CurrentHour, m.global.CurrentMin)
        lblTime.Text = formattedTime
    end if
    
    'Dimming at night

    if m.global.CurrentHour  < 7 or m.global.CurrentHour  > 20 then
        recOpacity.Opacity = 0.9
    else
        recOpacity.Opacity = Abs((m.global.CurrentHour + (m.global.CurrentMin /60) - 12)/12)
    end if
    

    if m.global.CurrentMin mod 2 = 0 and m.global.SecCounterField = 0 then
        updateImage()
    end if
    
end function

function updateImage() 
    if len(m.global.SlideShowImages) > 0 then

        images = ParseJson(m.global.SlideShowImages)
        
        
        if m.global.CurrentlyDisplaying > (images.Count() -1) then
            m.global.CurrentlyDisplaying = 0
        end if
        
        print images[m.global.CurrentlyDisplaying]
        
        lblImageCounter = m.top.findNode("lblImageCounter")

        if  m.global.ConnectionStatus <> "" then
            lblImageCounter.Text = m.global.ConnectionStatus
        else
            lblImageCounter.Text = str(m.global.CurrentlyDisplaying + 1) + " /" + str(images.Count())
        end if

        m.top.loadDisplayMode = "scaleToFit"
        m.top.backgroundURI = images[m.global.CurrentlyDisplaying]
        
        m.global.CurrentlyDisplaying = m.global.CurrentlyDisplaying + 1
        m.global.UpdateSlideShow = false
    end if
end function

function init()
    m.top.backgroundURI = "pkg:/images/bg.jpg"
    m.top.setFocus(true)

    m.global.observeField("SecCounterField", "onTimeUpdated")
    m.global.SecCounterField = 0
    
    lblDayofWeek = m.top.findNode("lblDayofWeek")
    lblTime = m.top.findNode("lblTime")
    recOpacity = m.top.findNode("recOpacity") 
    lblImageCounter = m.top.findNode("lblImageCounter")
    
    lblDayofWeek.Width = m.global.DisplayWidth * 0.9
    lblTime.Width = m.global.DisplayWidth * .9
    
    lblTime.Height = m.global.DisplayHeight * .7
    lblDayofWeek.Height = m.global.DisplayHeight * .7 + 0.08 *  m.global.DisplayHeight
    lblImageCounter.Height = m.global.DisplayHeight * .7 + 0.16 *  m.global.DisplayHeight
        
    recOpacity.Height = m.global.DisplayHeight
    recOpacity.Width = m.global.DisplayWidth
    
    recOpacity.Opacity = 0
    
    lblDayofWeek.font.size =  0.05 * m.global.DisplayWidth '~60 for 1280
    lblTime.font.size = 0.04 * m.global.DisplayWidth  '~50 for 1280
    lblImageCounter.font.size =  0.02 * m.global.DisplayWidth
    
end function

sub showPhoneDialog()
    dialog = createObject("roSGNode", "KeyboardDialog")
    dialog.backgroundUri = "pkg:/images/dlg.png"
    dialog.title = "Enter Your Phone Number"
    dialog.optionsDialog = false
    dialog.buttons=["OK","Cancel"]
    dialog.observeField("buttonSelected","goToPin")
    dialog.message = GetMessageValue("SendMessageTo") + " " + getConfigValue("ChannelPhoneNumber")
    m.top.dialog = dialog
    m.top.setFocus(TRUE)
end sub

    sub showPinDialog()
    dialog = createObject("roSGNode", "KeyboardDialog")
    dialog.backgroundUri = "pkg:/images/dlg.png"
    dialog.title = "Enter your Pin"
    dialog.optionsDialog = false
    dialog.buttons=["OK","Cancel"]
    dialog.observeField("buttonSelected","savePin")
    dialog.message = "Enter the pin we sent you"
    m.top.dialog = dialog
    m.top.setFocus(TRUE)
end sub

function goToPin() 
        if m.top.dialog.buttonSelected = 0 then
        phone =  m.top.dialog.text

        if Len(phone) >= 10
            phone = "1" + phone 'US number support
            RegWrite("Phone", phone, "_data")
            m.global.UpdateSlideShow = true
            m.top.dialog.close = true
            showPinDialog()
        else
            m.top.dialog.message =  GetMessageValue("ValidPhoneNumber")
        end if
        
    else
        m.top.dialog.close = true
    end if
end function

function savePin() 
        if m.top.dialog.buttonSelected = 0 then
        pin =  m.top.dialog.text
        storedphone = RegRead("Phone", "_data")
        if Len(pin) > 0
            RegWrite("Pin", pin, "_data")
            m.top.dialog.close = true
        else
            m.top.dialog.message =  GetMessageValue("ValidPin")
        end if
        
    else
        m.top.dialog.close = true
    end if
end function

function onKeyEvent(key as String, press as Boolean) as Boolean
    if press then
    if key = "OK"
        showPhoneDialog()
        return true
    end if
    end if
    return false
end function