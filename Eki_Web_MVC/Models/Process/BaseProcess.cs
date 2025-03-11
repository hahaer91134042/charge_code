using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

/// <summary>
/// BaseProcess 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public abstract class BaseProcess : IRunable
    {
        public abstract void run();
    }
}
