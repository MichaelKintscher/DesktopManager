#
# Script.ps1
#
#
# Uses PSVirtualDesktop. See: https://github.com/MScholtes/PSVirtualDesktop



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

    return $ConfigData
}



# ======================= Launch App Functions ========================

function Launch-Slack {
    # Define the parameters.
    param (
        $TeamID, $ChannelID
    )

    # Ensure Slack has a visible window running.
    $SlackProcess = Start-Process ("slack://channel?id=" + $ChannelID + "&team=" + $TeamID) -PassThru
    Sleep 3

    # Get the slack process with the window handle.
    $SlackProcess = Get-Process -Name ("Slack") | Where-Object {$_.MainWindowHandle -ne 0}
    if ($SlackProcess -eq $null) {
        Write-Host "WARNING - SlackProcess is null!"
    }
    else {
        Write-Host ("Slack has a MainWindowHandle of " + $SlackProcess.MainWindowHandle)
    }
    return $SlackProcess
}

function Launch-Visual-Studio {
    # Define the parameters.
    param (
        $InstallPath
    )

    # Ensure Visual Studio has a visible window running.
    $VisualStudioProcess = Start-Process $InstallPath -PassThru
    Sleep 3

    # Get the Visual Studio process with the window handle.
    if ($VisualStudioProcess -eq $null) {
        Write-Host "WARNING - VisualStudioProcess is null!"
    }
    else {
        Write-Host ("Visual Studio has a MainWindowHandle of " + $VisualStudioProcess.MainWindowHandle)
    }
    return $VisualStudioProcess
}

function Launch-Signal {
    # Define the parameters.
    param (
        $InstallPath
    )

    # Ensure Signal has a visible window running.
    $SignalProcess = Start-Process $InstallPath -PassThru
    Sleep 3

    # Get the Signal process with the window handle.
    if ($SignalProcess -eq $null) {
        Write-Host "WARNING - SignalProcess is null!"
    }
    else {
        Write-Host ("Signal has a MainWindowHandle of " + $SignalProcess.MainWindowHandle)
    }
    return $SignalProcess
}

function Launch-Calendar {
    # Define the parameters.
    param (
        $InstallPath
    )

    # Initialize the process handle to null.
    $CalendarProcess = $null

    if ($InstallPath -eq $null) {
        # No path given, so start the default Windows Calendar app.
        $CalendarProcess = Start-Process outlookcal: -PassThru
    }
    else {
        # Ensure Calendar has a visible window running.
        $CalendarProcess = Start-Process $InstallPath -PassThru
    }
    Sleep 3

    # Get the Calendar process with the window handle.
    if ($CalendarProcess -eq $null) {
        Write-Host "WARNING - CalendarProcess is null!"
    }
    else {
        Write-Host ("Calendar has a MainWindowHandle of " + $CalendarProcess.MainWindowHandle)
    }
    return $CalendarProcess
}

function Launch-Email {
    # Define the parameters.
    param (
        $InstallPath
    )

    # Initialize the process handle to null.
    $EmailProcess = $null

    if ($InstallPath -eq $null) {
        # No path given, so start the default Windows Mail app.
        $NewEmail = Start-Process "ms-unistore-email://" -PassThru
    }
    else {
        # Ensure Email has a visible window running.
        $EmailProcess = Start-Process $InstallPath -PassThru
    }
    Sleep 3

    # Get the Email process with the window handle.
    if ($EmailProcess -eq $null) {
        Write-Host "WARNING - EmailProcess is null!"
    }
    else {
        Write-Host ("Email has a MainWindowHandle of " + $EmailProcess.MainWindowHandle)
    }
    return $EmailProcess
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

    # Wait for everything to finish loading, so MainWindowHandle is not null.
    Sleep 3

    if ($NewEdge -eq $null) {
        Write-Host "WARNING - EdgeProcess is null!"
    }
    else {
        Write-Host ("Edge has a MainWindowHandle of " + $NewEdge.MainWindowHandle)
    }

    # Return the handle to the new browser window.
    return $NewEdge
}



# ======================== Workspace Functions ========================

function Setup-Workspaces {
    # Define the parameter.
    param (
        $WorkspaceData
    )

    # Setup each workspace.
    foreach ($WorkspaceInfo in $WorkspaceData.workspaces) {
        #$WorkspaceInfo = $WorkspaceData.workspaces[7]
        Setup-Worksapce -WorkspaceInfo $WorkspaceInfo
    }

    #Setup the cross-workspace.
    Setup-Cross-Workspace -CrossWorkspaceInfo $WorkspaceData.cross_workspace.apps
}

function Setup-Worksapce {
    # Define the parameter.
    param (
        $WorkspaceInfo
    )

    # Create the new desktop.
    Write-Host ("Setting up " + $WorkspaceInfo.name + " workspace.")
    $NewDesktop = New-Desktop | Set-DesktopName -Name $WorkspaceInfo.name -PassThru | Switch-Desktop

    # Set up each app.
    [Diagnostics.Process[]]$Apps = $null
    foreach ($AppInfo in $WorkspaceInfo.apps) {
        if ($AppInfo.name -eq "Slack") {
            Write-Host "Launching Slack..."
            $Apps += Launch-Slack -ChannelID ($AppInfo.channel_id) -TeamID ($AppInfo.team_id)
            Write-Host "Slack launched!"
        }
        elseif ($AppInfo.name -eq "Visual Studio") {
            Write-Host "Launching Visual Studio..."
            $Apps += Launch-Visual-Studio -InstallPath $AppInfo.path
            Write-Host "Visual Studio launched!"
        }
    }
        
    # Move each app to the new virtual desktop.
    if ($Apps.length -eq 0) {
        Write-Host "No apps to move."
    }
    else {
        Write-Host ("Moving " + $Apps.length + " apps")
    }
    foreach ($newApp in $Apps) {
        Write-Host (" - Moving " + $newApp.ProcessName + " to desktop " + (Get-DesktopName $NewDesktop))
        $newApp.MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
    }

    # Set up each webpage.
    [string[]]$UrlsToLaunch = $null
    foreach ($WebpageInfo in $WorkspaceInfo.webpages) {
        $UrlsToLaunch += $WebpageInfo.url
    }
    $NewEdge = Create-Browser-Window -URLsToOpen $UrlsToLaunch -PassThru
    Write-Host ("Moving " + $NewEdge.ProcessName + " to desktop " + (Get-DesktopName $NewDesktop))
    $NewEdge.MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop

    Write-Host ($WorkspaceInfo.name + " workspace setup complete!")
}

function Setup-Cross-Workspace {
    # Define the parameter.
    param (
        $CrossWorkspaceInfo
    )

    #Setup the cross-workspace.
    Write-Host "Setting up cross-workspace apps..."
    foreach ($AppInfo in $CrossWorkspaceInfo.apps) {
        if ($AppInfo.name -eq "Signal") {
            Write-Host "Launching Signal..."
            $Apps += Launch-Signal -InstallPath $AppInfo.path
            Write-Host "Signal launched!"
        }
        elseif ($AppInfo.name -eq "Calendar") {
            Write-Host "Launching Calendar..."
            $Apps += Launch-Calendar -InstallPath $AppInfo.path
            Write-Host "Calendar launched!"
        }
        elseif ($AppInfo.name -eq "Mail") {
            Write-Host "Launching Mail..."
            $Apps += Launch-Email -InstallPath $AppInfo.path
            Write-Host "Mail launched!"
        }
    }

    # Pin each app.
    #foreach ($newApp in $Apps) {
        #Pin-Application ($newApp[0].MainWindowHandle)
    #}

    Write-Host "Cross-workspace app setup complete!"
}



# ========================== Test Functions ===========================

function Test-Browser-Launch {
    # -PassThru is needed to enable output to be stored in the variable. See: https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.management/start-process?view=powershell-7.2
    $NewDesktop = New-Desktop | Set-DesktopName -Name "W" -PassThru
    $NewEdge = Create-Browser-Window -URLsToOpen http://google.com, http://aa.com
    #Start-Sleep -Milliseconds 2000
    $NewEdge.MainWindowHandle | Move-Window ($NewDesktop) | Switch-Desktop
    #Move-Window -Desktop ($NewDesktop) -Hwnd ($NewEdge.MainWindowHandle) | Switch-Desktop
    Write-Host $NewDesktop
    Write-Host $NewEdge
    Pin-Window ($NewEdge.MainWindowHandle)
}

function Test-App-Launch {
    # Launch Email.
    $NewEmail = Start-Process -FilePath "C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe" -PassThru
    Write-Host $NewEmail[0].MainWindowHandle
}



# =========================== Main Function ===========================

# Read the input data.
$WorkspaceData = Get-Config-Input
Write-Host "Config read!"

# Get today's day of the week.
$Today = Get-Date -Format "dddd"

$WorkspacesToSetUp = $null
[string[]]$TodaysWorkspaces = $null
if ($Today -eq "Sunday") {
    #New-Desktop | Set-DesktopName -Name "Su"
    $TodaysWorkspaces = "GSA", "Video Production"
}
elseif ($Today -eq "Monday") {
    #New-Desktop | Set-DesktopName -Name "M"
    $TodaysWorkspaces = "GSA", "Classwork", "Research - XR Survey", "Research - AR Paper"
}
elseif ($Today -eq "Tuesday") {
    #New-Desktop | Set-DesktopName -Name "T"
    $TodaysWorkspaces = "Classwork", "Research - XR Survey", "Research - AR Paper"
}
elseif ($Today -eq "Wednesday") {
    #New-Desktop | Set-DesktopName -Name "W"
    $TodaysWorkspaces = "Coding"
}
elseif ($Today -eq "Thursday") {
    #New-Desktop | Set-DesktopName -Name "Th"
    $TodaysWorkspaces = "OA"
}
elseif ($Today -eq "Friday") {
    #New-Desktop | Set-DesktopName -Name "F"
    $TodaysWorkspaces = "GRADient"
}
elseif ($Today -eq "Saturday") {
    #New-Desktop | Set-DesktopName -Name "Sa"
    $TodaysWorkspaces = "Coding"
}
else {
    #New-Desktop | Set-DesktopName -Name "U"
}

$WorkspacesToSetUp += $WorkspaceData.workspaces | Where-Object -FilterScript { $TodaysWorkspaces -contains $_.name }

Setup-Worksapce -WorkspaceInfo $WorkspacesToSetUp
Setup-Cross-Workspace -CrossWorkspaceInfo $WorkspaceData.cross_workspace
#Setup-Workspaces -WorkspaceData $WorkspaceData

#Test-App-Launch
#Test-Browser-Launch