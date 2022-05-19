using SocketServer;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace superSocketServer
{
    //class Program
    //{
    //    static AppServer appServer { get; set; }
    //    static void Main(string[] args)
    //    {
    //        appServer = new AppServer();


    //        //Setup the appServer
    //        if (!appServer.Setup(2012)) //Setup with listening port
    //        {
    //            Console.WriteLine("Failed to setup!");
    //            Console.ReadKey();
    //            return;
    //        }

    //        //Try to start the appServer
    //        if (!appServer.Start())
    //        {
    //            Console.WriteLine("Failed to start!");
    //            Console.ReadKey();
    //            return;
    //        }


    //        Console.WriteLine("The server started successfully, press key 'q' to stop it!");

    //        //1.
    //        appServer.NewSessionConnected += new SessionHandler<AppSession>(appServer_NewSessionConnected);
    //        appServer.SessionClosed += appServer_NewSessionClosed;

    //        //2.
    //        appServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(appServer_NewRequestReceived);

    //        while (Console.ReadKey().KeyChar != 'q')
    //        {
    //            Console.WriteLine();
    //            continue;
    //        }

    //        //Stop the appServer
    //        appServer.Stop();

    //        Console.WriteLine("The server was stopped!");
    //        Console.ReadKey();
    //    }

    //    //1.
    //    static void appServer_NewSessionConnected(AppSession session)
    //    {
    //        Console.WriteLine($"服务端得到来自客户端的连接成功");

    //        var count = appServer.GetAllSessions().Count();
    //        Console.WriteLine("服务端当前连接数目:" + count);
    //        Console.WriteLine($"当前连入服务端的sessionid:{session.SessionID}");
    //        session.Send("---- Welcome to SuperSocket Telnet Server---");
    //    }

    //    static void appServer_NewSessionClosed(AppSession session, CloseReason aaa)
    //    {
    //        Console.WriteLine($"服务端失去来自客户端的连接,SessionID:" + session.SessionID +"原因:"+ aaa.ToString()) ;
    //        var count = appServer.GetAllSessions().Count();
    //        Console.WriteLine(count);
    //    }

    //    //2.  
    //    /// <summary>
    //    /// 服务端接收客户端消息处理
    //    /// </summary>
    //    /// <param name="session"></param>
    //    /// <param name="requestInfo"></param>
    //    static void appServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
    //    {
    //        Console.WriteLine("返回给客户端的消息Key:  " + requestInfo.Key);
    //        session.Send("返回给客户端的消息Body:" + requestInfo.Body);
    //    }





    //}

    /// <summary>
    /// branch3
    /// </summary>
    class Program
    {
        public static AppServer webSocketServer = new AppServer();
        static void Main(string[] args)
        {
            Console.WriteLine("服务端");

            webSocketServer.NewSessionConnected += WebSocketServer_NewSessionConnected;
            webSocketServer.NewMessageReceived += WebSocketServer_NewMessageReceived;
            if (!webSocketServer.Setup("192.168.12.149", 1992))
            {
                Console.WriteLine("设置服务监听失败！");
            }
            if (!webSocketServer.Start())
            {
                Console.WriteLine("启动服务监听失败！");
            }
            Console.WriteLine("启动服务监听成功！");
            Console.WriteLine("按任意键结束。。。");
            Console.ReadKey();
            webSocketServer.Dispose();
        }

        private static void WebSocketServer_NewSessionConnected(ExtendSession session)
        {
            string msg = $"{ DateTime.Now.ToString("HH: mm:ss")}客户端：{ GetwebSocketSessionName(session)}加入";
            Console.WriteLine($"{ msg}");
            session.StrNickName = "jack";
            SendToAll(session, msg);
        }

        private static void WebSocketServer_NewMessageReceived(ExtendSession session, string value)
        {
            string msg = $"{ DateTime.Now.ToString("HH: mm:ss")}服务端收到客户端：{ GetwebSocketSessionName(session)} 发送数据：{ value}";
            Console.WriteLine($"{ msg}");
            //webSocketServer.GetSessions()
            SendToAll(session, value);
        }

        private static void WebSocketServer_SessionClosed(ExtendSession session, SuperSocket.SocketBase.CloseReason value)
        {
            string msg = $"{ DateTime.Now.ToString("HH: mm:ss")}客户端：{ GetwebSocketSessionName(session)}关闭，原因：{ value}";
            Console.WriteLine($"{ msg}");
            SendToAll(session, msg);
        }

        ///
        /// 获取webSocketSession的名称
        ///

        ///
        public static string GetwebSocketSessionName(ExtendSession webSocketSession)
        {
            return HttpUtility.UrlDecode(webSocketSession.SessionID);
        }

        ///
        /// 广播，同步推送消息给所有的客户端
        ///
        ///
        ///
        public static void SendToAll(ExtendSession webSocketSession, string msg)
        {
            foreach (var item in webSocketSession.AppServer.GetAllSessions())
            {
                item.Send(msg);
            }
        }
    }
}