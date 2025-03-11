using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_EcPay
{
    /// <summary>
    /// IConnectSet 的摘要描述
    /// </summary>
    public interface IEcPayConnectSet : IEcPayCheck
    {
        string url();
    }
}
