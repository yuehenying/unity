using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _022_聊天室_tcp_服务器端
{
    class Program
    {   
        static List<Client> clientList = new List<Client>();

        public static void BroadCastMessage(string msg)
        {
            var notConnectedList = new List<Client>();
            foreach (var client in clientList)
            {
                if (client.Connected)
                {
                    client.SendMessage(msg);
                }
                else
                {
                    notConnectedList.Add(client);
                }
            }

            foreach (var temp in notConnectedList)
            {
                clientList.Remove(temp);
            }
        }
        static void Main(string[] args)
        {
            Socket tcpServer = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            tcpServer.Bind(new IPEndPoint(IPAddress.Parse("192.168.2.104"),8899));
            tcpServer.Listen(100);
            //广播消息
            
            Console.WriteLine("server running........");

            while (true)
            {
                Socket clientSocket = tcpServer.Accept();
                Console.WriteLine("a client is connected");
                Client client = new Client(clientSocket);   //每次连接的客户端的逻辑都放到Client类里
                clientList.Add(client);
            }    

        }
    }
}
