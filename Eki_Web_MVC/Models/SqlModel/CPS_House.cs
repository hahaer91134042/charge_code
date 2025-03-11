using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_House")]
    public class CPS_House : BaseDbDAO,IConvertResponse<object>
    {
        [DbRowKey("CommunityId")]
        public int CommunityId { get; set; }
        [DbRowKey("Name",DbAction.Update)]
        public string Name { get; set; }
        [DbRowKey("LocData",DbAction.Update)]
        public string LocData 
        { 
            get 
            {
                if (locList.Count < 1)
                    return "";
                var s = String.Concat((from l in locList
                                   select $"{l.SerNum},"));
                s = s.Remove(s.Length - 1);
                return s;
            } 
            set 
            {
                if (value.Length > 0)
                {
                    locList = (from ser in value.Split(',')
                               join l in EkiSQL.ekicps.table<CPS_Location>() on ser equals l.SerNum
                               select l).toSafeList();
                }
                else
                {
                    locList = new List<CPS_Location>();
                }
            } 
        }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("Uid", RowAttribute.Guid, true)]
        public Guid Uid { get; set; }

        public List<CPS_Location> locList = new List<CPS_Location>();//該住戶連結的車位序號

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
        public override bool Update()
        {
            return EkiSQL.ekicps.update(this);
        }
        public override bool Delete()
        {
            return EkiSQL.ekicps.delete(this);
        }

        public object convertToResponse()
            => new
            {
                Id,
                Name,
                Loc=locList,
                beEnable,
                cDate
            };
    }
}