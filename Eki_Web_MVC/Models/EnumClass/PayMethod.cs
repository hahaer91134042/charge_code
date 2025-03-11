using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    /// <summary>
    /// PayCreditType 的摘要描述
    /// </summary>
    public enum PayMethod
    {
        /// <summary>
        /// 基本信用卡付款(一次性)
        /// </summary>
        Credit = 0,

        /// <summary>
        /// Line pay 付款方式
        /// </summary>
        LinePay=1,

        /// <summary>
        /// 約定信用卡付款
        /// 幕後傳遞
        /// </summary>
        Credit_Agree = 2
    }
}
