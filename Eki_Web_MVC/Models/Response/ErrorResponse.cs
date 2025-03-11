using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public class ErrorResponse : ResponseDAO
    {
        public ErrorResponse(EkiErrorCode code=EkiErrorCode.E004) : base(code,false)
        {

        }
    }
}