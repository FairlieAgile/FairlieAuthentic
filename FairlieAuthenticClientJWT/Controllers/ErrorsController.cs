using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlieAuthenticClientJWT.Controllers
{
    [AllowAnonymous]
    public class ErrorsController : Controller
    {
        public ActionResult General(Exception exception)
        {
            ViewBag.ErrorCode = Response.StatusCode;
            ViewBag.Message = "Error Happened";

            //you should log your exception here

            return View("Index");
        }
    }
}
