using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class charManager : MonoBehaviour
{
    public string ipaddress = "192.168.2.104";

    public int port = 8899;

    public Socket clientSocket;

    public UIInput textInput;
    public UILabel chatLabel;
    private byte[] data = new byte[1024];
    public string msg = ""; //消息容器

    private Thread t;

	// Use this for initialization
	void Start ()
	{
	    ConnectToServer();
	}
	
	// Update is called once per frame
	void Update () {
	    if (!string.IsNullOrEmpty(msg))
	    {
	        chatLabel.text += "\n" + msg;
	        msg = "";   //清空消息
	    }
	}

    void ConnectToServer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //发起连接
        //写法1
        clientSocket.Connect(IPAddress.Parse(ipaddress), port);
        //写法2
        //clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipaddress),  port));

        //创建一个新的线程接收消息
        t = new Thread(ReceiveMessage);
        t.Start();
    }

    void ReceiveMessage()
    {
        while (true)
        {
            if (clientSocket.Connected == false)
            {
                break;
            }
            int len = clientSocket.Receive(data);
            msg = Encoding.UTF8.GetString(data, 0, len);
        }
    }

    void SendMessage(string msg)
    {
        byte[] data = Encoding.UTF8.GetBytes(msg);
        clientSocket.Send(data);
    }

    public void OnSendButtonClick()
    {
        string val = textInput.value;
        SendMessage(val);
        textInput.value = "";
    }

    void OnDestroy()
    {
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }
}
