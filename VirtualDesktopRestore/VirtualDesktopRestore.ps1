#
# Script.ps1
#

# Create a new virtual desktop.
# Based on: https://superuser.com/questions/995236/how-to-create-new-virtual-desktops-in-a-script-to-launch-multiple-applications-i
<#
$KeyShortcut = Add-Type -MemberDefinition @"
[DllImport("user32.dll")]
static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
//WIN + CTRL + D: Create a new desktop
public static void CreateVirtualDesktopInWin10()
{
    //Key down
    keybd_event((byte)0x5B, 0, 0, UIntPtr.Zero); //Left Windows key 
    keybd_event((byte)0x11, 0, 0, UIntPtr.Zero); //CTRL
    keybd_event((byte)0x44, 0, 0, UIntPtr.Zero); //D
    //Key up
    keybd_event((byte)0x5B, 0, (uint)0x2, UIntPtr.Zero);
    keybd_event((byte)0x11, 0, (uint)0x2, UIntPtr.Zero);
    keybd_event((byte)0x44, 0, (uint)0x2, UIntPtr.Zero);
}
"@ -Name CreateVirtualDesktop -UsingNamespace System.Threading -PassThru
   $KeyShortcut::CreateVirtualDesktopInWin10()
#>

#Get-Desktop 1 | Switch-Desktop

# Create a new virtual desktop.
# Based on: https://github.com/MScholtes/PSVirtualDesktop#:~:text=Get-DesktopName%20-Desktop%20desktop%20Get%20name%20of%20virtual%20desktop.,and%20not%20with%20Powershell%20Core%207.1%20and%20up%21
# New-Desktop | Set-DesktopName -Name "The new one"


# ======================== Function Definitions =======================

# Classwork
function Setup-Classwork {
    New-Desktop | Set-DesktopName -Name "Classwork"
}

# Research - XR Ed Survey
function Setup-Research-XR-Ed-Survey {
    New-Desktop | Set-DesktopName -Name "Research - XR Survey"
}

# Research - AR Paper
function Setup-Research-AR-Paper {
    New-Desktop | Set-DesktopName -Name "Research - AR Paper"
}

# Coding
function Setup-Coding {
    New-Desktop | Set-DesktopName -Name "Coding"
}

# GSA
function Setup-GSA {
    New-Desktop | Set-DesktopName -Name "GSA"
}

# Video Production
function Setup-Video-Production {
    New-Desktop | Set-DesktopName -Name "Video Production"
}

# OA
function Setup-OA {
    New-Desktop | Set-DesktopName -Name "OA"
}

function Create-Browser-Window {
    # Define the parameter as an array of strings.
    param (
        [string[]]$URLsToOpen
    )

    # Based on: https://stackoverflow.com/questions/40493141/how-to-utilize-powershell-to-open-an-application-with-a-command
    #     Starts a new window.
    #     -PassThru is needed to enable output to be stored in the variable. See: https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.management/start-process?view=powershell-7.2
    $NewEdge = Start-Process -FilePath "C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe" -PassThru

    # Start a new tab for each URL to open.
    foreach ($URLParam in $URLsToOpen) {
        # Based on: https://stackoverflow.com/questions/57053312/how-to-open-edge-using-powershell-variable
        Start-Process microsoft-edge:$URLParam
    }

    # Return the handle to the new browser window.
    return $NewEdge
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
    # -PassThru is needed to enable output to be stored in the variable. See: https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.management/start-process?view=powershell-7.2
    $NewDesktop = New-Desktop | Set-DesktopName -Name "W" -PassThru
    $NewEdge = Create-Browser-Window -URLsToOpen http://google.com, http://aa.com
    $NewEdge[0].MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
}
elseif ($Today -eq "Thursday") {
    New-Desktop | Set-DesktopName -Name "Th"
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