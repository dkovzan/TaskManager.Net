using System.Web.Mvc;

namespace TaskManager.WEB.Controllers
{
    public class ErrorController : Controller
    {
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