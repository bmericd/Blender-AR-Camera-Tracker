using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class GetOrientation : MonoBehaviour
{
    public Text textX, textY, textZ, textRotX, textRotY, textRotZ;
    public Camera arCamera;

    private TcpClient client;
    private NetworkStream stream;
    private StreamWriter writer;
    private Thread t1;
    public Button button;

    string posX, posY, posZ, rotX, rotY, rotZ, fullOrientation;
    // Start is called before the first frame update
    void Start()
    {


        //string hostName = Dns.GetHostName();
        //Console.WriteLine(hostName);

        //string hostIP = Dns.GetHostEntry(hostName).AddressList[1].ToString();
        //Console.WriteLine(hostIP);
        //textZ.text = hostIP;
        //client = new TcpClient("192.168.43.163", 5500);


        //client = new TcpClient("192.168.42.35", 5500);
        //stream = client.GetStream();
        //writer = new StreamWriter(stream);

        //t1 = new Thread(Thread_Update);
        //t1.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //textX.text = "X: " + arCamera.transform.position.x.ToString();
        //textY.text = "Y: " + arCamera.transform.position.y.ToString();
        //textZ.text = "Z: " + arCamera.transform.position.z.ToString();

        //textRotX.text = "X: " + arCamera.transform.rotation.eulerAngles.x.ToString();
        //textRotY.text = "Y: " + arCamera.transform.rotation.eulerAngles.y.ToString();
        //textRotZ.text = "Z: " + arCamera.transform.rotation.eulerAngles.z.ToString();

        textRotX.text= (360 - arCamera.transform.rotation.eulerAngles.x + 90).ToString();
        textRotY.text= arCamera.transform.rotation.eulerAngles.y.ToString();
        textRotZ.text= arCamera.transform.rotation.eulerAngles.z.ToString();

        //sendData = Encoding.ASCII.GetBytes(message);

        /*
       
        */

        /*
        posX = String.Format("{0:+0.00000;-0.00000;+0.00000}", arCamera.transform.position.x).Replace(',','.');
        posY = String.Format("{0:+0.00000;-0.00000;+0.00000}", arCamera.transform.position.y);
        posZ = String.Format("{0:+0.00000;-0.00000;+0.00000}", arCamera.transform.position.z);
        rotX = String.Format("{0:+0.00000;-0.00000;+0.00000}", arCamera.transform.rotation.eulerAngles.x);
        rotY = String.Format("{0:+0.00000;-0.00000;+0.00000}", arCamera.transform.rotation.eulerAngles.y);
        rotZ = String.Format("{0:+0.00000;-0.00000;+0.00000}", arCamera.transform.rotation.eulerAngles.z);

        //Thread.Sleep(100);
        int byteCount = Encoding.ASCII.GetByteCount(posX + 1);
        byte[] sendData = new byte[byteCount];
        sendData = Encoding.ASCII.GetBytes(posX);
        writer.Write(Encoding.ASCII.GetString(sendData));
        writer.Flush();
        */
    }

    public void Thread_Start()
    {
        client = new TcpClient("192.168.42.136", 5500);
        stream = client.GetStream();
        writer = new StreamWriter(stream);

        t1 = new Thread(Thread_Update);
        t1.Start();
    }

    public void Thread_Stop()
    {
        t1.Abort();
        client.Close();
        stream.Close();
        writer.Close();
    }
    void Thread_Update()
    {
        while (true)
        {
            /*
            posX = arCamera.transform.position.x.ToString().Replace(',','.').Substring(0,7);
            posY = arCamera.transform.position.y.ToString().Replace(',','.').Substring(0, 7);
            posZ = arCamera.transform.position.z.ToString().Replace(',','.').Substring(0, 7);
            rotX = arCamera.transform.rotation.eulerAngles.x.ToString().Replace(',','.').Substring(0, 7);
            rotY = arCamera.transform.rotation.eulerAngles.y.ToString().Replace(',', '.').Substring(0, 7);
            rotZ = arCamera.transform.rotation.eulerAngles.z.ToString().Replace(',', '.').Substring(0, 7);
            */

            posX = String.Format("{0:+0.00000;-0.00000;+0.00000}", arCamera.transform.position.x).Replace(',', '.');
            posY = String.Format("{0:+0.00000;-0.00000;+0.00000}", arCamera.transform.position.y).Replace(',', '.');
            posZ = String.Format("{0:+0.00000;-0.00000;+0.00000}", arCamera.transform.position.z).Replace(',', '.');
            rotX = String.Format("{0:+0.00000;-0.00000;+0.00000}", (360-arCamera.transform.rotation.eulerAngles.x+90) * Mathf.Deg2Rad).Replace(',', '.');
            rotY = String.Format("{0:+0.00000;-0.00000;+0.00000}", -arCamera.transform.rotation.eulerAngles.y * Mathf.Deg2Rad).Replace(',', '.');
            rotZ = String.Format("{0:+0.00000;-0.00000;+0.00000}", arCamera.transform.rotation.eulerAngles.z * Mathf.Deg2Rad).Replace(',', '.');

            fullOrientation = rotX + rotY + rotZ+ posX+posY+posZ;

            Thread.Sleep(50);
            int byteCount = Encoding.ASCII.GetByteCount(fullOrientation + 1);
            byte[] sendData = new byte[byteCount];
            sendData = Encoding.ASCII.GetBytes(fullOrientation);
            writer.Write(Encoding.ASCII.GetString(sendData));
            writer.Flush();
        }

    }


}