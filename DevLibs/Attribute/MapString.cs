using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DevLibs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MapString:Attribute
    {
        public interface Root
        {
            string mapSymbol();
        }

        public int index;
        public string key = "";
        public string subSymbol = "";
        
        /// <summary>
        /// index from 0
        /// </summary>
        /// <param name="i"></param>
        public MapString([Range(0,100)] int i) { index = i; }
        
        /// <summary>
        /// Combine to key symbol value pair
        /// </summary>
        /// <param name="k">Key</param>
        /// <param name="sub">Symbol</param>
        public MapString(string k,string sub)
        {
            key = k;
            subSymbol = sub;
        }

    }
}