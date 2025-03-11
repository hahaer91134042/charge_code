using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// BaseModel 的摘要描述
/// </summary>
namespace DevLibs
{
    public abstract class BaseDbDAO : DbOperationModel
    {

        //[Newtonsoft.Json.JsonIgnore()]
        [DbRowKey("Id", RowAttribute.PrimaryKey, DbAction.Static, false)]
        public new int Id { get { return base.Id; } set { base.Id = value; } }

        public abstract bool CreatById(int id);
        public abstract int Insert(bool isReturnId = false);
        public virtual bool CreatByUniqueId(string uniqueId)
        {
            return false;
        }
        public virtual bool Update()
        {
            return false;
        }
        public virtual bool Delete()
        {
            return false;
        }


        public virtual Boolean isEmpty()
        {
            return true;
        }

    }
}
