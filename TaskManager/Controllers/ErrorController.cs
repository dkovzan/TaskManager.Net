using log4net;
using System;
using System.Reflection;
using System.Web.Mvc;

namespace TaskManager.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILog _logger;

        public ErrorController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
        public ActionResult ClientError()
        {
            return View("ClientError");
        }

        public ActionResult ServerError()
        {
            return View("ServerError");
        }
    }
}