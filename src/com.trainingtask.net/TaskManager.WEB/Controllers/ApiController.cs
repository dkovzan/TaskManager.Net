using AutoMapper;
using System;
using System.Web;
using System.Web.Mvc;
using TaskManager.WEB.Helpers;

namespace TaskManager.WEB.Controllers
{
    public class ApiController : BaseController
    {
        public ApiController(IMapper mapper) : base(mapper)
        {
        }

        [ValidateAntiForgeryToken]
        public ActionResult SetCulture(string culture)
        {
            culture = CultureHelper.GetImplementedCulture(culture);

            var cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;
            else
            {
                cookie = new HttpCookie("_culture") {Value = culture, Expires = DateTime.Now.AddYears(1)};
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("List", "Employee");
        }

        
    }
}