using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

/// <summary>
/// ListConvert 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public class ListConvert
    {
        public static DbObjList<T> convertToDbList<T, R>(List<R> list) where T : BaseDbDAO where R : IRequestConvert<T>
        {
            var objList = new DbObjList<T>();
            list.ForEach(data =>
            {
                objList.Add(data.convertToDbModel());
            });
            return objList;
        }
    }
}
