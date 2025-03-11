using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    /// <summary>
    /// InvoiceType 的摘要描述
    /// </summary>
    public enum InvoiceType
    {
        /// <summary>
        /// 不索取
        /// </summary>
        None = 0,

        /// <summary>
        ///捐贈
        /// </summary>
        Donate = 1,

        /// <summary>
        /// 紙本
        /// </summary>
        Paper = 2,

        /// <summary>
        /// 手機載具
        /// </summary>
        Phone = 3,

        /// <summary>
        /// 自然人憑證
        /// </summary>
        Certificate = 4,

        /// <summary>
        /// ezPay載具
        /// </summary>
        ezPay = 5
    }
}
