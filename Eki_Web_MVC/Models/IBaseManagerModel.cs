using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC
{
    /// <summary>
    /// 基本上類似於Manager的 資料模型 存取資料 主要使用在view model上面
    /// </summary>
    public abstract class IBaseManagerModel
    {
        public interface IModelConfig
        {
            object[] modelConstructor();
        }

    }
}