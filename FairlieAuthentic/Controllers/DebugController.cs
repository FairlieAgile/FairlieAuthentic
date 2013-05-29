using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlieAuthentic.Controllers
{
    [AllowAnonymous]
    public class DebugController : Controller
    {
        //
        // GET: /Debug/
        [AllowAnonymous]
        public IEnumerable<string> Get()
        {
            string[] debug = { ConfigurationManager.AppSettings["fa:AllowedAudience"] };
            return debug;
        }

    }
}
