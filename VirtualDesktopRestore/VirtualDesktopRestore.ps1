#
# Script.ps1
#

# ====================== Config Input Functions =======================

function Get-Config-Input {
    $ConfigFileName = "settings.json"
    $ConfigData = $null

    # Check if the file exists.
    if (Test-Path -Path $ConfigFileName -PathType Leaf) {
        # File exists, so read it.
        $ConfigData = Get-Content $ConfigFileName | ConvertFrom-Json
    }
    else {
        # File does not exist, so create it.
        New-Item -Path $ConfigFileName -ItemType File
    }
}


# ======================== Workspace Functions ========================

# Classwork
function Setup-Research-Classwork {
    # Create the new desktop.
    $NewDesktop = New-Desktop | Set-DesktopName -Name "Classwork" -PassThru

    # Launch Slack. Based on: https://stackoverflow.com/questions/32146706/slack-url-to-open-a-channel-from-browser
    #    Team: 
    #    Channel: general
    $NewSlack = Start-Process "slack://channel?id=C02T2MKE4EN&team=T02SN3DJ8DV" -PassThru

    # Move the new apps to the new desktop.
    $NewSlack[0].MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
    $NewEdge[0].MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
}

# Research - XR Ed Survey
function Setup-Research-XR-Ed-Survey {
    # Create the new desktop.
    $NewDesktop = New-Desktop | Set-DesktopName -Name "Research - XR Survey" -PassThru

    # Launch Edge to display the following websites:
    #    - Google Chat
    #    - Google Drive, Team Drive, XR Ed Survey Paper
    $NewEdge = Create-Browser-Window -URLsToOpen https://mail.google.com/chat/u/1/#chat/dm/ihPWGgAAAAE, https://drive.google.com/drive/u/1/folders/0AOPuz4T3MGdNUk9PVA

    # Move the new apps to the new desktop.
    $NewEdge[0].MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
}

# Research - AR Paper
function Setup-Research-AR-Paper {
    # Create the new desktop.
    $NewDesktop = New-Desktop | Set-DesktopName -Name "Research - AR Paper" -PassThru

    # Launch Edge to display the following websites:
    #    - Google Chat
    #    - Google Drive, Team Drive, Thesis - Michael Kintscher
    $NewEdge = Create-Browser-Window -URLsToOpen https://mail.google.com/chat/u/1/#chat/dm/ihPWGgAAAAE, https://drive.google.com/drive/u/1/folders/0AJbDQ0JQLWIiUk9PVA

    # Move the new apps to the new desktop.
    $NewEdge[0].MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
}

# Coding
function Setup-Coding {
    # Create the new desktop.
    $NewDesktop = New-Desktop | Set-DesktopName -Name "Coding" -PassThru

    # Launch Edge to display the following websites:
    #    - GitHub
    $NewEdge = Create-Browser-Window -URLsToOpen https://github.com/MichaelKintscher

    # Move the new apps to the new desktop.
    $NewEdge[0].MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
}

# GSA
function Setup-Work-GSA {
    # Create the new desktop.
    $NewDesktop = New-Desktop | Set-DesktopName -Name "GSA" -PassThru

    # Launch Edge to display the following websites:
    #    - Upcoming Zoom Meetings
    #    - Canvas
    $NewEdge = Create-Browser-Window -URLsToOpen https://asu.zoom.us/meeting#/upcoming, https://canvas.asu.edu/courses/106062

    # Move the new apps to the new desktop.
    $NewEdge[0].MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
}

# GRADient
function Setup-GRADient {
    # Create the new desktop.
    $NewDesktop = New-Desktop | Set-DesktopName -Name "GRADient" -PassThru

    # Launch Slack. Based on: https://stackoverflow.com/questions/32146706/slack-url-to-open-a-channel-from-browser
    #    Team: GRADient
    #    Channel: general
    $NewSlack = Start-Process "slack://channel?id=CCKSDQ57G&team=TCJLVA6TF" -PassThru

    # Launch Edge to display the following websites:
    #    - Google Drive, Team Drive, GRADient
    $NewEdge = Create-Browser-Window -URLsToOpen https://drive.google.com/drive/u/1/folders/0AIalcm3xUMfBUk9PVA

    # Move the new apps to the new desktop.
    $NewSlack[0].MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
    $NewEdge[0].MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
}

# Video Production
function Setup-Video-Production {
    # Create the new desktop.
    $NewDesktop = New-Desktop | Set-DesktopName -Name "Video Production" -PassThru | Switch-Desktop
}

# NOAC OA
function Setup-Work-OA-NOAC {
    # Create the new desktop.
    $NewDesktop = New-Desktop | Set-DesktopName -Name "OA" -PassThru

    # Launch Slack. Based on: https://stackoverflow.com/questions/32146706/slack-url-to-open-a-channel-from-browser
    #    Team: 
    #    Channel: general
    $NewSlack = Start-Process "slack://channel?id=C061MCJLU&team=T061MGC1W" -PassThru

    # Move the new apps to the new desktop.
    $NewSlack[0].MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
}

function Setup-Cross-Workspace-Apps {

    #[Diagnostics.Process[]]$Apps

    # Launch Signal.
    $NewSignal = Start-Process "[Signal install location]" -PassThru
    
    # Launch Calendar.
    $NewCalendar = Start-Process outlookcal: -PassThru

    # Launch Email.
    $NewEmail = Start-Process "ms-unistore-email://" -PassThru

    # Pin each app.
    #foreach ($newApp in $Apps) {
        #Pin-Application ($newApp[0].MainWindowHandle)
    #}
}

function Create-Browser-Window {
    # Define the parameter as an array of strings.
    param (
        [string[]]$URLsToOpen
    )

    # Based on: https://stackoverflow.com/questions/40493141/how-to-utilize-powershell-to-open-an-application-with-a-command
    #     Starts a new window.
    #     -PassThru is needed to enable output to be stored in the variable. See: https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.management/start-process?view=powershell-7.2
    $NewEdge = Start-Process -FilePath "[Edge install location]" -PassThru

    # Start a new tab for each URL to open.
    foreach ($URLParam in $URLsToOpen) {
        # Based on: https://stackoverflow.com/questions/57053312/how-to-open-edge-using-powershell-variable
        Start-Process microsoft-edge:$URLParam
    }

    # Return the handle to the new browser window.
    return $NewEdge
}


# ========================== Test Functions ===========================

function Test-Browser-Launch {
    # -PassThru is needed to enable output to be stored in the variable. See: https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.management/start-process?view=powershell-7.2
    $NewDesktop = New-Desktop | Set-DesktopName -Name "W" -PassThru
    $NewEdge = Create-Browser-Window -URLsToOpen http://google.com, http://aa.com
    #Start-Sleep -Milliseconds 2000
    $NewEdge.MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
    #Move-Window -Desktop ($NewDesktop) -Hwnd ($NewEdge.MainWindowHandle) | Switch-Desktop
    Write-Output $NewDesktop
    Write-Output $NewEdge
    Pin-Window ($NewEdge.MainWindowHandle)
}

function Test-App-Launch {
    # Launch Email.
    $NewEmail = Start-Process -FilePath "C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe" -PassThru
    Write-Output $NewEmail[0].MainWindowHandle
}


# =========================== Main Function ===========================

# Get today's day of the week.
$Today = Get-Date -Format "dddd"

if ($Today -eq "Sunday") {
    New-Desktop | Set-DesktopName -Name "Su"
}
elseif ($Today -eq "Monday") {
    New-Desktop | Set-DesktopName -Name "M"
}
elseif ($Today -eq "Tuesday") {
    New-Desktop | Set-DesktopName -Name "T"
}
elseif ($Today -eq "Wednesday") {
    New-Desktop | Set-DesktopName -Name "W"
}
elseif ($Today -eq "Thursday") {
    
}
elseif ($Today -eq "Friday") {
    New-Desktop | Set-DesktopName -Name "F"
}
elseif ($Today -eq "Saturday") {
    New-Desktop | Set-DesktopName -Name "Sa"
}
else {
    New-Desktop | Set-DesktopName -Name "U"
}

Get-Config-Input

#Test-App-Launch
#Test-Browser-Launch
#Setup-Cross-Workspace-Apps