using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EkiSocket
{
    public static class SocketExt
    {
        #region ISocketMsg
        public static SocketMsgShell toSocketMsg(this ISocketMsg msg) => new SocketMsgShell
        {
            Method = msg.socketMethod(),
            Content = msg
        };
        public static void send(this IWebSocketConnection socket, ISocketMsg msg)
        {
            socket.Send(msg.toSocketMsg().toJsonString());
        }
        #endregion

    }
}