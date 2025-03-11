using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// IRuleCheck 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public interface IRuleCheck<T>
    {
        bool isInRule(T factor);
    }
}
