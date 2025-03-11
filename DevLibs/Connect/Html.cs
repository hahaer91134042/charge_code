using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DevLibs
{
    public class Html
    {
        public string Document;

        public bool connectGet(string url)
        {
            try
            {
                var request = WebRequest.CreateHttp(url);
                request.Method = HttpMethod.Get.Method;
                using (var reader=new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    Document = reader.ReadToEnd();
                }

                    return true;
            }catch(Exception e) { }
            return false;
        }

    }
}
