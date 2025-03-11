using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevLibs
{
    public abstract class DbOperationModel
    {
        //protected int id = 0;
        //因為要能夠把ID的DbRowKey分開設定所以子類別get set 要加上base.Id來設定Id
        //[DbRowKey("Id", DbAction.NOINSERT, false)]
        public int Id { get; set; }


    }
}
