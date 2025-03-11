using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// OrderStatus 的摘要描述
/// </summary>
namespace Eki_Web_MVC.CPS
{
    public enum OrderStatus
    {
        /// <summary>
        /// 充電中
        /// </summary>
        Charging = 0,
        /// <summary>
        /// 已結帳
        /// </summary>
        CheckOut = 1,//已結帳 Checkout已經有資料 藍新未付過款
        /// <summary>
        /// 以完結 藍新付完款
        /// </summary>
        End = 2,
        /// <summary>
        /// 付款錯誤
        /// </summary>
        PayError = 3

    }
}
