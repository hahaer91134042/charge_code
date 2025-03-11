using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ResponseInfoModel 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public class IResponseInfo<INFO> : ResponseDAO
    {
        public INFO info { get; set; }
        public IResponseInfo(bool successful) : base(successful)
        {
        }
    }
}