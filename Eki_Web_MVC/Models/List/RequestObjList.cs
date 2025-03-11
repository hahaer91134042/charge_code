using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

/// <summary>
/// DbObjList 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public class RequestObjList<M> : List<M> where M : RequestDAO
    {
        //public DbObjList<DBO> convertToDbObjList_v2<DBO>(Action<DBO> back = null) where DBO : BaseDbDAO
        //{
        //    return new DbObjList<DBO>((from data in this.AsEnumerable()
        //                               where typeof(ApiFeature_v2.IRequestConvert<DBO>).IsAssignableFrom(data.GetType())
        //                               select (DBO)data.GetType().GetMethod("convertToModel_v2").Invoke(data, new object[] { back })).toSafeList());
        //}
        public DbObjList<DBO> convertToDbObjList<DBO>() where DBO : BaseDbDAO
        {
            return new DbObjList<DBO>((from data in this.AsEnumerable()
                                       where typeof(IRequestConvert<DBO>).IsAssignableFrom(data.GetType())
                                       select (DBO)data.GetType().GetMethod("convertToDbModel").Invoke(data, null)).toSafeList());
        }

        public virtual void cleanXss()
        {
            foreach (var data in this)
                data.cleanXss();
        }

        public virtual bool isValid()
        {
            foreach (var data in this)
            {
                if (!data.isValid())
                    return false;
            }
            return true;
        }


    }
}
