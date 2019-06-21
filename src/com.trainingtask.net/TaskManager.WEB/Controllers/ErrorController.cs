using System.Web.Mvc;

namespace TaskManager.WEB.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/ServerError")]
        public ActionResult Index()
        {
            return View("ServerError");
        }
    }
}