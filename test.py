# Creates a window with a button in it. Clicking the button displays the command line arguments passed to the script. The first argument will always be the filepath of the script.
import sys
import tkinter as tk

root= tk.Tk()

canvas1 = tk.Canvas(root, width = 300, height = 300)
canvas1.pack()

def hello ():  
    btnText = 'Argument List:' + str(sys.argv)
    label1 = tk.Label(root, text= btnText, fg='green', font=('helvetica', 12, 'bold'))
    canvas1.create_window(150, 200, window=label1)
    
button1 = tk.Button(text='Click Me',command=hello, bg='brown',fg='white')
canvas1.create_window(150, 150, window=button1)

root.mainloop()