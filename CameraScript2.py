
import bpy
from bpy.utils import register_class, unregister_class
import socket
import time
import threading
import re
import math

#pi = 3.14159265

hostname = socket.gethostname()
local_ip = socket.gethostbyname(hostname)

bl_info = {
    "name" : "AR Camera",
    "author" : "bmericd",
    "description" : "AR Camera for Blender",
    "blender" : (2,93,0),
    "location" : "View3D",
    "warning" : "",
    "category" : "Generic"
}


#scene.camera.rotation_mode = 'XYZ'
#scene.camera.rotation_euler[0] = 
HEADERSIZE = 10

msg = "welcome"
print(f'{len(msg):<{HEADERSIZE}}'+msg)

scene = bpy.data.scenes["Scene"]
isEnabled = False
disconnected = False
firstInit = True
class ARCamera_Panel(bpy.types.Panel):
    bl_idname = "ARCamera_Panel"
    bl_label = "AR Camera"
    bl_category = "AR Camera"
    bl_space_type = "VIEW_3D"
    bl_region_type="UI"
    
    def draw(self,context):
        layout = self.layout
        row = layout.row()
        global isEnabled
        global firstInit
        if disconnected == True or firstInit:
            row.operator('view3d.cursor_center',text =  "Start Capturing", icon="RADIOBUT_OFF")
        else:
            row.operator('view3d.cursor_center',text = "Stop Capturing",icon="RADIOBUT_ON")
        if firstInit == True:
            firstInit = False

class ARCamera_Operator(bpy.types.Operator):
    bl_idname = "view3d.cursor_center"
    bl_label = "Simple operator"
    bl_description = "Capture Camera Orientation from Android Device"
    
    def execute(self,context):
        #bpy.ops.view3d.snap_cursor_to_center()
        stop_thread=False
        global isEnabled
        global disconnected
        if isEnabled == True:    
            isEnabled = False
            stop_thread = True
            disconnected = True;
            #thread.join()
            #print( isEnabled)
            return {'FINISHED'}
        
        if isEnabled == False:
            for id in range(0,1):
                thread = threading.Thread(target=thread_update, args=(id,lambda: stop_thread))
                thread.start()
                isEnabled = True
                disconnected = False
                print( "Connected")
            return {'FINISHED'}
                

classes = (ARCamera_Operator,ARCamera_Panel)

#register, unregister = bpy.utils.register_classes_factory(classes)

def register():
    for cls in classes:
        register_class(cls)
        
def unregister():
    for cls in classes:
        unregister_class(cls)
        



def thread_update(id, stop):
    global disconnected
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    s.bind(('192.168.42.136',5500))
    s.listen(5)
    clientsocket, adress = s.accept()
    while(True):    
        print(f"Connection from {adress} running in thread {format(id)}")
        msg = clientsocket.recv(48).decode("ASCII")
        print(msg)
        if stop == False:
            print("Exiting Thread")
            break
        if disconnected == True:
            s.shutdown(socket.SHUT_RDWR)
            s.close()
            print( "Disconnected")
            break;
        #print(int(msg)+1)
        try:
            print(msg[0:8]+"    "+msg[8:16]+"    "+msg[16:24])
            scene.camera.location = [float(msg[24:32]),float(msg[32:40]),float(msg[40:48])]
            #scene.camera.location.x = float(msg[24:32])
            scene.camera.rotation_euler=[float(msg[0:8]),-float(msg[16:24]),float(msg[8:16])]
        except ValueError:
            print("Not float")
        #time.sleep(0.1)

register()
#thread = threading.Thread(target=thread_update)
#thread.start()

    
