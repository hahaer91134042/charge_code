using DevLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Eki_Web_MVC
{
    public class FileController : IBaseViewController<FileModel>
    {
        //產生驗證碼
        public ActionResult GetValidateCode()
        {

            var vCode = ValidateCode.creat(4, RandomString.NumberString);

            //Session["ValidateCode"] = code;
            //Log.d($"Validate Code->{vCode.code}");

            vCode.saveSession();


            return File(vCode.img, @"image/jpeg");
        }
    }
}