#
# ========================== Script Arguments ===============================
#       This script takes one argument:
#           - (string) The name of the new virtual desktop to create. The
#               The new desktop will be given the default Task View name
#               ("Desktop [#]") if no name is provided.
# ===========================================================================

from asyncio.windows_events import NULL
from contextlib import nullcontext
import sys
import subprocess
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

def moveCurrentWindowToDesktop(name=NULL):
    currentWindow = AppView.current()
    desktop = getDesktop(name)
    currentWindow.move(VirtualDesktop(desktop.number))
    

# ===========================================================================

# ======================= App Management Functions ==========================

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
                    and app not in websites]

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

    # Now, launch each website. Website URLs must be preceded by the "explorer"
    #   command, which launches the URL in the default browser.
    [subprocess.run('explorer "' + website + '"')
        for website in websites]

    # Launch each custom uri. Custom uri protocols must be preceded by the "start"
    #   command. Windows's dispatcher will then select an app to handle the URI
    #   based on what (if any) apps have registered to handle the custom protocol.
    [subprocess.run('start ' + customUri)
        for customUri in customUris]

    # Launch each application. Application paths must be enclosed in quotation
    #   marks.
    [subprocess.run('"' + app + '"')
        for app in applications]

    # Open each file. File paths must be enclosed in quotation marks. Windows will
    #   then open the app
    [subprocess.run('"' + file + '"')
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

# ===========================================================================

root= tk.Tk()

canvas1 = tk.Canvas(root, width = 300, height = 300)
canvas1.pack()

def hello ():  
    # Get the desktop name from the script's args.
    desktopName = getDesktopNameArg()
    
    # Create the desktop if it does not yet exist.
    print("Does ", desktopName, " desktop exist? ", desktopExists(desktopName))
    if (desktopExists(desktopName) == False):
        # Update the desktop name from the created desktop. (This addresses the
        #     case where the provided name was NULL, so a name was generated).
        desktopName = createDesktop(desktopName).name

    # Move the current window to the desktop (which is the window created by this code since the user just clicked the button).
    moveCurrentWindowToDesktop(desktopName)
    # Switch to the desktop.
    switchToDesktop(desktopName)

    #subprocess.run('"C:\Program Files (x86)\Microsoft\Edge\Application\msedge_proxy.exe"  --profile-directory=Default --app-id=edcmabgkbicempmpgmniellhbjopafjh --app-url=https://app.clickup.com/')

    btnText = 'You have ' + str(getNumDesktops()) + ' virtual desktops open!'
    print(btnText)
    label1 = tk.Label(root, text= btnText, fg='green', font=('helvetica', 12, 'bold'))
    canvas1.create_window(150, 200, window=label1)
    
button1 = tk.Button(text='Create Desktop',command=hello, bg='brown',fg='white')
canvas1.create_window(150, 150, window=button1)

root.mainloop()