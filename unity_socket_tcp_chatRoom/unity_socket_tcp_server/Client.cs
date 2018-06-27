using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _022_聊天室_tcp_服务器端
{
    class Client
    {
        private Socket clientSocket;
        private Thread t;
        private byte[] data = new byte[1024];

        public Client(Socket s)
        {
            clientSocket = s;
            //启动一个线程处理客户端的数据接收
            t = new Thread(ReceiveMessage);
            t.Start();
        }

        private void ReceiveMessage()
        {
            //一直接收客户端数据
            while (true)
            {
                //接收数据之前判断socket连接是否断开
                if (clientSocket.Poll(10, SelectMode.SelectRead))   //判断连接是否断开
                {   
                    clientSocket.Close();
                    Console.WriteLine("一个客户端断开连接");
                    break;  //跳出循环终止线程的执行
                }

                int len = clientSocket.Receive(data);
                string msg = Encoding.UTF8.GetString(data,0,len);
                Program.BroadCastMessage(msg);
                //接受到数据时将数据发送到客户端
                Console.WriteLine("收到消息:" + msg);
            }
        }

        public void SendMessage(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            clientSocket.Send(data);
        }

        public bool Connected
        {
            get { return clientSocket.Connected; }
        }
    }
}
