using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC
{
    public class MenuSession:IBaseDAO
    {
        public Item Main { get; set; }
        public List<MenuSession> Sub { get; set; }

        public class Item
        {
            [DbRowKey("Id")]
            public int Id { get; set; }
            //[DbRowKey("ParentId", DbAction.Update)]
            //public int ParentId { get; set; } = 0;//預設最先開始的是0(從0開始找)
            [DbRowKey("GroupLv", DbAction.Update)]
            public int GroupLv { get; set; }
            [DbRowKey("Type")]
            public int Type { get; set; } = 0;//目前0=前台控制單元 1=系統控制單元
            [DbRowKey("Name", DbAction.Update)]
            public string Name { get; set; }
            [DbRowKey("Path", DbAction.Update)]
            public string Path { get; set; } = "";
            [DbRowKey("cDate", RowAttribute.CreatTime, true)]
            public DateTime cDate { get; set; }
            [DbRowKey("Uid", RowAttribute.Guid, true)]
            public Guid Uid { get; set; }
            [DbRowKey("Sort", DbAction.Update)]
            public int Sort { get; set; }
        }
    }
}