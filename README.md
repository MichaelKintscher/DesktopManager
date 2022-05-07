# DesktopManager
App for managing Windows 11 desktops

The script opens a window with a button in it. Clicking the button displays the number of virtual desktops you currently have open.

To use this with Elgato Stream Deck:

Prerequisits
- Install Python if not already installed: https://www.python.org/downloads/
- Install Elgato Stream Deck if not already installed: https://www.elgato.com/en/downloads
- Install Python Virtual Desktop Accessor (pyvda) if not already installed: https://github.com/mrob95/pyvda

Instructions
- Clone this repository to a location on your computer. Make note of where the "main.py" file is, as you will need this file path later.
- In Stream Deck, add a "Open" command to the button you want to launch this app (the open command is under the "System" tab  on the right).
- For the "App / File" field, specify the file path of the "main.py" file. Be sure to include the file name "main.py" at the end of the path, and to enclose the path in double quotes (this is all done automatically for you if you use the browse button to select the file).
- You can now add any arguments you would like in the same "App / File" field. Manually edit the value and add any arguments AFTER the closing double quote, separated by a space. Each argument needs to be separated by a space. If a single argument has a space in it, you will need to wrap that argument in double quotes.
- Press the button on your Stream Deck to launch the app! When you press the button, you will see the number of virtual desktops you currently have open!

Testing
Current script is a proof of concept. The script opens a window with a button in it. Clicking the button displays the command line arguments passed to the script. The first argument will always be the filepath of the script.
- Follow the Instructions above, but use "test.py" in place of "main.py".
- Press the button on your Stream Deck to launch the app! When you press the button, you will see the arguments you gave it displayed below (you will likely have to expand the window to see them all).

In future, this will be updated to do more useful things!
