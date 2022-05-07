import sys
import tkinter as tk
from pyvda import AppView, get_apps_by_z_order, VirtualDesktop, get_virtual_desktops

# ====================== Virtual Desktop Functions ==========================

def getNumDesktops():
    return len(get_virtual_desktops())

# ===========================================================================

root= tk.Tk()

canvas1 = tk.Canvas(root, width = 300, height = 300)
canvas1.pack()

def hello ():  
    btnText = 'You have ' + str(getNumDesktops()) + ' virtual desktops open!'
    print(btnText)
    label1 = tk.Label(root, text= btnText, fg='green', font=('helvetica', 12, 'bold'))
    canvas1.create_window(150, 200, window=label1)
    
button1 = tk.Button(text='Click Me',command=hello, bg='brown',fg='white')
canvas1.create_window(150, 150, window=button1)

root.mainloop()