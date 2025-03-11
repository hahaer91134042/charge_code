using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    /// <summary>
    /// IConvertData 的摘要描述
    /// </summary>
    public interface IConvertData<T>
    {
        T convertData();
    }
}
