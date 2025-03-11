using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Parser 的摘要描述
/// </summary>
namespace DevLibs
{
    public abstract class Parser<T>
    {
        public abstract void parse(T value);
    }
}
