using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WebSocket4Net;

namespace superSocketClient
{
    //class Program
    //{
    //    static Socket socketClient { get; set; }
    //    static void Main(string[] args)
    //    {
    //        //创建实例
    //        socketClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
    //        IPAddress ip = IPAddress.Parse("192.168.12.148");
    //        IPEndPoint point = new IPEndPoint(ip, 2012);
    //        //进行连接
    //        socketClient.Connect(point);


    //        //接收服务器端发送的消息
    //        Thread thread = new Thread(Recive);
    //        thread.IsBackground = true;
    //        thread.Start();


    //        //给服务器发送数据
    //        Thread thread2 = new Thread(Send);
    //        thread2.IsBackground = true;
    //        thread2.Start();

    //        Console.ReadKey();
    //    }

    //    /// <summary>
    //    /// 接收消息
    //    /// </summary>
    //    /// <param name="o"></param>
    //    static void Recive()
    //    {
    //        //  为什么用telnet客户端可以，但这个就不行。
    //        while (true)
    //        {
    //            //获取发送过来的消息
    //            byte[] buffer = new byte[1024 * 1024 * 2];
    //            var effective = socketClient.Receive(buffer);
    //            if (effective == 0)
    //            {
    //                break;
    //            }
    //            var str = Encoding.UTF8.GetString(buffer, 0, effective);
    //            Console.WriteLine("来自服务器 --- " + str);
    //            Thread.Sleep(2000);
    //        }
    //    }


    //    static void Send()
    //    {       
    //        int i=1;
    //        while (Console.ReadLine() != String.Empty)
    //        {
    //            string strSend= Console.ReadLine();

    //            var buffter = Encoding.UTF8.GetBytes($" Client sent {i}th message:+{strSend}\r\n");
    //            var temp = socketClient.Send(buffter);
    //            i++;
    //            Thread.Sleep(10000);
    //        }

    //    }
    //}
    class Program
    {
        public static WebSocket webSocket4Net = null;
        static void Main(string[] args)
        {
            Console.WriteLine("客户端");
            webSocket4Net = new WebSocket("ws://192.168.12.149:1992");
            webSocket4Net.Opened += WebSocket4Net_Opened;
            webSocket4Net.MessageReceived += WebSocket4Net_MessageReceived;
            webSocket4Net.Open();
            Console.WriteLine("客户端连接成功！");
            Thread thread = new Thread(ClientSendMsgToServer);
            thread.IsBackground = true;
            thread.Start();
            
            Console.WriteLine("按任意键结束。。。");
            Console.ReadKey();
            webSocket4Net.Dispose();
        }

        public static void ClientSendMsgToServer()
        {
            int i = 88;
            while (true)
            {
                //Console.WriteLine($"客户端发送数据{i++}");
                webSocket4Net.Send($"{ i++}");
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        }

        private static void WebSocket4Net_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine($"服务端回复数据:{ e.Message}！");
        }

        private static void WebSocket4Net_Opened(object sender, EventArgs e)
        {
            webSocket4Net.Send($"客户端准备发送数据！");
        }
    }
}