using System;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC
{
    public abstract class RequestDAO:IBaseDAO
    {

        protected string cleanXssStr(string input) => TextUtil.cleanHtmlFragmentXss(input);

        public virtual Boolean cleanXss()
        {
            return false;
        }

        //判斷資料是否全部有效
        public virtual Boolean isValid()
        {
            return false;
        }

        public virtual Boolean isEmpty()
        {
            return true;
        }
    }
}

