using System;
using System.Linq;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace superSocketServer
{
    class Program
    {
        static AppServer appServer { get; set; }
        static void Main(string[] args)
        {
            appServer = new AppServer();


            //Setup the appServer
            if (!appServer.Setup(2012)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }


            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            //1.
            appServer.NewSessionConnected += new SessionHandler<AppSession>(appServer_NewSessionConnected);
            appServer.SessionClosed += appServer_NewSessionClosed;

            //2.
            appServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(appServer_NewRequestReceived);
            
            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }

        //1.
        static void appServer_NewSessionConnected(AppSession session)
        {
            Console.WriteLine($"服务端得到来自客户端的连接成功");

            var count = appServer.GetAllSessions().Count();
            Console.WriteLine("服务端当前连接数目:" + count);
            Console.WriteLine($"当前连入服务端的sessionid:{session.SessionID}");
            session.Send("---- Welcome to SuperSocket Telnet Server---");
        }

        static void appServer_NewSessionClosed(AppSession session, CloseReason aaa)
        {
            Console.WriteLine($"服务端失去来自客户端的连接,SessionID:" + session.SessionID +"原因:"+ aaa.ToString()) ;
            var count = appServer.GetAllSessions().Count();
            Console.WriteLine(count);
        }

        //2.  
        /// <summary>
        /// 服务端接收客户端消息处理
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        static void appServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            Console.WriteLine("返回给客户端的消息Key:  " + requestInfo.Key);
            session.Send("返回给客户端的消息Body:" + requestInfo.Body);
        }



    }
}