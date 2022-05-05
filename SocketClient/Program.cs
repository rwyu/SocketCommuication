using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace superSocketClient
{
    class Program
    {
        static Socket socketClient { get; set; }
        static void Main(string[] args)
        {
            //创建实例
            socketClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse("192.168.12.148");
            IPEndPoint point = new IPEndPoint(ip, 2012);
            //进行连接
            socketClient.Connect(point);


            //接收服务器端发送的消息
            Thread thread = new Thread(Recive);
            thread.IsBackground = true;
            thread.Start();


            //给服务器发送数据
            Thread thread2 = new Thread(Send);
            thread2.IsBackground = true;
            thread2.Start();

            Console.ReadKey();
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        static void Recive()
        {
            //  为什么用telnet客户端可以，但这个就不行。
            while (true)
            {
                //获取发送过来的消息
                byte[] buffer = new byte[1024 * 1024 * 2];
                var effective = socketClient.Receive(buffer);
                if (effective == 0)
                {
                    break;
                }
                var str = Encoding.UTF8.GetString(buffer, 0, effective);
                Console.WriteLine("来自服务器 --- " + str);
                Thread.Sleep(2000);
            }
        }


        static void Send()
        {      
            int i=1;
            while (true)
            {
                string strSend;
                if (Console.ReadLine()!=String.Empty)
                {
                    strSend = Console.ReadLine();
                } 
                var buffter = Encoding.UTF8.GetBytes($" 客户端:发送的第{i}条消息" + "\r\n");
                var temp = socketClient.Send(buffter);
                i++;
                Thread.Sleep(10000);
            }

        }
    }
}