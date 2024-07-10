import json
import random
import struct
import time
import tkinter as tk

# send modes: 
#   -2: end game
#   -1: restart game
#    0: left click
#    1: set flag

def Lock():
    global locked
    global btns
    locked = True
    for i in range(len(btns)):
        btns[i]["state"] = "disabled"

def left(btn_num):
    global locked
    global recived
    global btns

    if locked == True:
        return
    # if btns[btn_num]["state"] == "disabled":
    #     return

    if btns[btn_num]["text"] == "F":
        return

    print(btn_num)
    sendDict = {"btnNr": btn_num, "mode": 0}
    recived = str(sendDict)
    s = recived.encode('ascii')
    
    f.write(struct.pack('I', len(s)) + s)   # Write str length and str
    f.seek(0)                               # EDIT: This is also necessary


    n = struct.unpack('I', f.read(4))[0]    # Read str length
    s = f.read(n).decode('ascii')           # Read str
    s = json.loads(s)

    print(s)
    print(type(s))

    btn_nr = 0
    recived = s
    boardsize = s["Boardsize"]

    if s["GameState"] == 1:
        Lock()
        label.config(text="Winner Winner Chicken Dinner")
        restartBtn["height"] = 1
        restartBtn["width"] = 6
        restartBtn.pack(side='right', anchor='w')
        endBtn["height"] = 1
        endBtn["width"] = 6
        endBtn.pack(side='left', anchor='e')

    elif s["GameState"] == -1:
        Lock()
        label.config(text="You lost")
        restartBtn["height"] = 1
        restartBtn["width"] = 6
        restartBtn.pack(side='right', anchor='w')
        endBtn["height"] = 1
        endBtn["width"] = 6
        endBtn.pack(side='left', anchor='e')

    print(len(btns))

    for y in range(boardsize[1]):
        for x in range(boardsize[0]):
            btns[btn_nr]['text'] = s["Nums"][y][x]
            btns[btn_nr]["bg"] = "SystemButtonFace"
            if s["Nums"][y][x] == "M":
                btns[btn_nr]["bg"] = "red"
            if s["Flaglist"][y][x] == 1:
                btns[btn_nr]["bg"] = "blue"
                btns[btn_nr]["text"] = "F"
                
            btn_nr += 1

    f.seek(0)

def right(btn_num):
    global locked
    global recived
    global btns

    if locked == True:
        return
    print(btn_num)
    sendDict = {"btnNr": btn_num, "mode": 1}
    recived = str(sendDict)
    s = recived.encode('ascii')
    
    f.write(struct.pack('I', len(s)) + s)   # Write str length and str
    f.seek(0)                               # EDIT: This is also necessary


    n = struct.unpack('I', f.read(4))[0]    # Read str length
    s = f.read(n).decode('ascii')           # Read str
    s = json.loads(s)

    print(s)
    print(type(s))

    btn_nr = 0
    recived = s
    boardsize = s["Boardsize"]
    for y in range(boardsize[1]):
        for x in range(boardsize[0]):
            btns[btn_nr]['text'] = s["Nums"][y][x]
            btns[btn_nr]["bg"] = "SystemButtonFace"
            if s["Nums"][y][x] == "M":
                btns[btn_nr]["bg"] = "red"
                label.config(text="You loose.")
            if s["Flaglist"][y][x] == 1:
                btns[btn_nr]["bg"] = "blue"
                btns[btn_nr]["text"] = "F"
                
            btn_nr += 1

    f.seek(0)

def restart():
    global btn_nr
    global btns
    global label
    btn_nr = -1
    btns.clear()
    
    sendDict = {"mode": -1}
    recived = str(sendDict)
    s = recived.encode('ascii')
    
    f.write(struct.pack('I', len(s)) + s)   # Write str length and str
    f.seek(0)                               # EDIT: This is also necessary


    n = struct.unpack('I', f.read(4))[0]    # Read str length
    s = f.read(n).decode('ascii')           # Read str
    s = json.loads(s)

    print(s)
    print(type(s))
    for button in btns:
        button.grid_forget()
    btns.clear()
    label.config(text="")

    boardsize = s["Boardsize"]

    for y in range(boardsize[1]):
        for x in range(boardsize[0]):
            btn_nr += 1
            print(btn_nr)

            btns.append(tk.Button(text=s["Nums"][y][x], height=1, width=2))

            btns[btn_nr].grid(row=y, column=x)
            btns[btn_nr].bind("<Button-1>", lambda e,c=btn_nr:left(c))
            btns[btn_nr].bind("<Button-2>", lambda e,c=btn_nr:right(c))
            btns[btn_nr].bind("<Button-3>", lambda e,c=btn_nr:right(c))


    label = tk.Label(master, text= "")
    label.grid(row=boardsize[1] + 1, columnspan=100)

    frame_btn = tk.Frame(master)
    frame_btn.grid(row=boardsize[1] + 2, columnspan=100)
    

def end():
    sendDict = {"mode": -2}
    recived = str(sendDict)
    s = recived.encode('ascii')
    
    f.write(struct.pack('I', len(s)) + s)   # Write str length and str
    f.seek(0)                               # EDIT: This is also necessary


    n = struct.unpack('I', f.read(4))[0]    # Read str length
    s = f.read(n).decode('ascii')           # Read str
    s = json.loads(s)

    print(s)
    print(type(s))
    master.destroy()


locked = False

f = open(r'\\.\pipe\NPtest', 'r+b', 0)
i = 1

master=tk.Tk()
master.title("Minesweeper")
# master.geometry("350x275")

btn_nr = -1
btns = []

sendDict = {"btnNr": -1, "mode": 0}
recived = str(sendDict)
s = recived.encode('ascii')
f.write(struct.pack('I', len(s)) + s)   # Write str length and str
f.seek(0)                               # EDIT: This is also necessary
print(s)

n = struct.unpack('I', f.read(4))[0]    # Read str length
s = f.read(n).decode('ascii')           # Read str
recived = s
f.seek(0)                               # Important!!!
s = json.loads(s)
print(s)
print(type(s))

print(s["Nums"])
boardsize = s["Boardsize"]

master.grid(widthInc=40, heightInc=20)

for y in range(boardsize[1]):
    for x in range(boardsize[0]):
        btn_nr += 1
        print(btn_nr)

        btns.append(tk.Button(text=s["Nums"][y][x], height=1, width=2))

        btns[btn_nr].grid(row=y, column=x)
        btns[btn_nr].bind("<Button-1>", lambda e,c=btn_nr:left(c))
        btns[btn_nr].bind("<Button-2>", lambda e,c=btn_nr:right(c))
        btns[btn_nr].bind("<Button-3>", lambda e,c=btn_nr:right(c))


label = tk.Label(master, text= "")
label.grid(row=boardsize[1] + 1, columnspan=100)

frame_btn = tk.Frame(master)
frame_btn.grid(row=boardsize[1] + 2, columnspan=100)

restartBtn = tk.Button(frame_btn, height=0, width=0, text="restart", command=restart)
endBtn = tk.Button(frame_btn, height=0, width=0, text="end")

master.mainloop()