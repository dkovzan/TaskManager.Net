using System.Web.Mvc;

namespace TaskManager.WEB.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult ServerError()
        {
            return View("ServerError");
        }
    }
}