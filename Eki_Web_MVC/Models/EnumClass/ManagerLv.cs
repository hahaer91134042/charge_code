using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public enum ManagerLv
    {
        SysAdmin=99,
        SysDev=10,
        Sales=1,//業務
        Customer=0,//客服人員
        None=-1
    }
}