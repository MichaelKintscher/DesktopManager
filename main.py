#
# ========================== Script Arguments ===============================
#       This script takes two arguments:
#           - (string) The name of the new virtual desktop to create. The
#               The new desktop will be given the default Task View name
#               ("Desktop [#]") if no name is provided.
#           - (string) "TEST" to enable test mode (not case sensitive). Any
#               other value (or no value at all) will default to NOT run
#               test mode. Test mode provides a GUI to trigger the script
#               and view script output.
# ===========================================================================

from asyncio.windows_events import NULL
from contextlib import nullcontext
import os
import sys
import subprocess
import json
import tkinter as tk
from pyvda import AppView, get_apps_by_z_order, VirtualDesktop, get_virtual_desktops

# ====================== Virtual Desktop Functions ==========================

def getNumDesktops():
    return len(get_virtual_desktops())

def getDesktop(name=NULL):
    # Default to NULL if no name is given.
    desktop = NULL

    # Get the list of desktops.
    desktops = get_virtual_desktops()
    # Search the list for the first desktop with the matching name.
    for d in desktops:
        if (d.name == name):
            desktop = d
            break

    return desktop

def getDesktopIndex(name=NULL):
    # Default to NULL if no matching desktop is found.
    desktopNum = NULL

    desktop = getDesktop(name)
    if (desktop != NULL):
        desktopNum = desktop.number
    
    return desktopNum

def desktopExists(name=NULL):
    desktopIndex = getDesktopIndex(name)
    return desktopIndex != NULL

def normalizeDesktopNames():
    # Get the list of desktops with empty names.
    desktops = get_virtual_desktops()
    
    # Set the name to what appears in the task view.
    [desktop.rename("Desktop " + str(desktop.number)) for desktop in desktops if desktop.name == '']


def createDesktop(name=NULL):
    newDesktop = VirtualDesktop.create()
    
    # Rename the new desktop , if a name is provided.
    if (name != NULL):
        newDesktop.rename(name)
    else:
        newDesktop.rename("Desktop " + str(newDesktop.number))

    return newDesktop

def switchToDesktop(name=NULL):
    # If no name is given, default to the first desktop.
    if (name == NULL):
        VirtualDesktop(0).go()
    else:
        # A name was given. Find the desktop index matching that name.
        desktopIndex = getDesktopIndex(name)

        # If a desktop with the given name cannot be found, default to the first desktop.
        if (desktopIndex == NULL):
            VirtualDesktop(0).go()
        else:
            # A desktop with the given name was found. Switch to the target desktop.
            VirtualDesktop(desktopIndex).go()

# FUTURE DEVELOPMENT - this is intended to work with windows NOT on the current virtual
#   desktop - but this does not appear possible in the current version of pyvda library.
#def moveWindowToDesktop(window, desktopName=NULL):
#    # Move the specified window to the specified desktop.
#    desktop = getDesktop(desktopName)
#
#    # If no desktop was specified, default to the current desktop.
#    if (desktop == NULL):
#        desktop = VirtualDesktop.current()
#    print(window)
#    
#    if (type(window) is int):
#        windowAppView = AppView(window)
#    else:
#        windowAppView = window
#    
#    windowAppView.move(VirtualDesktop(desktop.number))
    

# ===========================================================================

# ======================= App Management Functions ==========================

def getWorkspaceApps(workspaceName):
    # Initialize the list of apps to empty.
    apps = ['https://google.com']

    # Return an empty list if no workspace name is given.
    if (workspaceName == NULL):
        return apps

    # Load the json from the json config file.
    workspaceFile = open('workspaces.json')
    workspaceData = json.load(workspaceFile)

    #for i in workspaceData['workspaces']:
    #    print(i['name'] + " " + str(i['apps']))

    # Get the workspace(s) with a matching name to the given workspace name.
    workspaces = [wksp for wksp in workspaceData['workspaces']
                 if wksp['name'] == workspaceName]

    # If at least one workspace with a matching name was found, get the list of
    #   apps for the workspace. Defaults to the first matching workspace if
    #   multiple were found.
    if (len(workspaces) > 0):
        apps = workspaces[0]['apps']

    return apps

def launchApps(apps):
    # Determine if the app command is an app, a website, or a file.
    # Websites will include the HTTP or HTTPS protocol.
    websites = [app for app in apps
                if "https://" in app[:8]
                    or "http://" in app[:8]]

    # Custom URI protocls will include "://" near the beginning. websites also
    #   include this, however, so items already in the websites list will have
    #   to be removed.
    customUris = [app for app in apps
                  if "://" in app
                    and "https://" not in app
                    and "http://" not in app]

    # Apps will include the .exe extension for executable. Note that this will not
    #   necessarily appear at the end, since command line arguments may be given.
    applications = [app for app in apps
                    if ".exe" in app]

    # Files will be anything that remains.
    files = [app for app in apps
             if app not in websites
                and app not in customUris
                and app not in applications]

    # Note that "websites" and "applications" are not guaranteed to be mutually
    #   exclusive based on the above filters. All valid commnds *should* fall into
    #   only one list. Even when launching a web browser and specifying a URL in the
    #   same command, the command starts with the web browser location or reference
    #   (such as msedge)... NOT the http or https protocol.

    # To address the case where a valid URL may contain ".exe", remove any items
    #   from the "applications" list that also appear in the "websites" list.
    applications = [app for app in applications
                    if app not in websites]

    print('Attempting to launch websites: ' + str(websites))

    # Now, launch each website. Website URLs must be preceded by the "explorer"
    #   command, which launches the URL in the default browser.
    [subprocess.run('explorer "' + website + '"')
        for website in websites]

    print('Attempting to launch custom URIs: ' + str(customUris))

    # Launch each custom uri. Custom uri protocols must be preceded by the "start"
    #   command. Windows's dispatcher will then select an app to handle the URI
    #   based on what (if any) apps have registered to handle the custom protocol.
    [subprocess.run('start ' + customUri)
        for customUri in customUris]

    print('Attempting to launch apps: ' + str(applications))

    # Launch each application.
    [subprocess.run(app)
        for app in applications]

    print('Attempting to open files: ' + str(files))

    # Open each file. File paths must be enclosed in quotation marks. Windows will
    #   then open the app the file type is associated with.
    [os.startfile('"' + file + '"')
        for file in files]


# ===========================================================================

# ==================== Script Args Parsing Functions ========================

def getDesktopNameArg():
    # Default to NULL if no second argument is provided.
    name = NULL

    # Second argument will be the desktop name (first is always the script file path).
    if (len(sys.argv) > 1):
        name = sys.argv[1]

    return name

def getTestModeArg():
    # Default to false if no third argument is provided.
    testMode = False

    # Third argument will be test mode. This is not case sensitive.
    if (len(sys.argv) > 2):
        testMode = sys.argv[2].upper() == "TEST"

    return testMode

# ===========================================================================

def restoreWorkspace(desktopName):
    # Create the desktop if it does not yet exist.
    print("Does ", desktopName, " desktop exist? ", desktopExists(desktopName))
    if (desktopExists(desktopName) == False):
        # Update the desktop name from the created desktop. (This addresses the
        #     case where the provided name was NULL, so a name was generated).
        desktopName = createDesktop(desktopName).name

    # Switch to the desktop.
    switchToDesktop(desktopName)

    # Get and launch the apps for the workspace.
    apps = getWorkspaceApps(desktopName)
    launchApps(apps)

    return apps

def testMode(desktopName):
    # Create a UI window for displaying output for testing purposes.
    root= tk.Tk()

    canvas1 = tk.Canvas(root, width = 300, height = 300)
    canvas1.pack()

    def hello (desktopName):
        #Restore the desktop.
        apps = restoreWorkspace(desktopName)

        # Move the current window to the desktop (which is the window created by this code since the user just clicked the button).
        #root.focus()
        #moveWindowToDesktop(root.winfo_id(), desktopName)

        btnText = 'You opened ' + str(apps)  + ' on ' + desktopName + '!'
        print(btnText)
        label1 = tk.Label(root, text= btnText, fg='green', font=('helvetica', 12, 'bold'))
        canvas1.create_window(150, 200, window=label1)
        
    button1 = tk.Button(text='Create Desktop',command=lambda: hello(desktopName), bg='brown',fg='white')
    canvas1.create_window(150, 150, window=button1)

    root.mainloop()

def main():

    # Normalize the desktop names by setting them to what the user sees in
    #   Task View. This is necessary because desktops have a default name
    #   of the empty string until they are renamed.
    normalizeDesktopNames()

    # Get the desktop name from the script's args.
    desktopName = getDesktopNameArg()

    # Determing if test mode is enabled.
    if (getTestModeArg()):
        testMode(desktopName)
    else:
        # Switch to the desktop if it already exists, or restore it if it does not.
        if (desktopExists(desktopName)):
            switchToDesktop(desktopName)
        else:
            restoreWorkspace(desktopName)

# Switch between these if you want to always force test mode when using a debugger. "main()" accepts the test mode argument
#   passed to the script, whereas "testMode()" always runs test mode.
main()
#testMode('')