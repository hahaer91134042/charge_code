﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// SocketMsg 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public class SocketReceiveMsg
    {
        public string time { get; set; }
        public string method { get { return socketMethod.ToString(); } set { socketMethod = value.toEnum<SocketMethod>(); } }
        //public string target { get; set; }
        public object request { get; set; }
        public SocketMethod socketMethod = SocketMethod.SendTo;
    }
}
