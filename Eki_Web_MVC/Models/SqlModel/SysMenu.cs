using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC
{
    [DbTableSet("SysMenu")]
    public class SysMenu : BaseDbDAO,IConvertResponse<object>
    {
        [DbRowKey("ParentId", DbAction.Update)]
        public int ParentId { get; set; } = 0;//預設最先開始的是0(從0開始找)
        [DbRowKey("GroupLv",DbAction.Update)]
        public int GroupLv { get; set; }
        [DbRowKey("Name",DbAction.Update)]
        public string Name { get; set; }
        [DbRowKey("Path", DbAction.Update)]
        public string Path { get; set; } = "";
        [DbRowKey("Type", DbAction.Update)]
        public int Type { get; set; } = 0;
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("uDate", RowAttribute.NowTime, DbAction.UpdateOnly, true)]
        public DateTime uDate { get; set; }
        [DbRowKey("Uid", RowAttribute.Guid, true)]
        public Guid Uid { get; set; }
        [DbRowKey("Sort",DbAction.Update)]
        public int Sort { get; set; }

        public object convertToResponse()
            => new
            {
                Id,
                ParentId,
                GroupLv,
                Name,
                Path,
                Type,
                beEnable,
                Sort
            };

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);
        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
        public override bool Update() => EkiSQL.ekicps.update(this);
        public override bool Delete() => EkiSQL.ekicps.delete(this);
    }
}