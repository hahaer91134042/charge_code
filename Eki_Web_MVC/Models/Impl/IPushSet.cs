﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// IPushSet 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public interface IPushSet : IFcmSet
    {
        string socketID();
    }
}
