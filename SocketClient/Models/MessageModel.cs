using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient.Models
{
    /// <summary>
    /// messagemodel
    /// </summary>
    public class MessageModel
    {
        public string Name { get; set; }

        public string StrMsg { get; set; }

        public string StrSendTime { get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); } }
    }
}
