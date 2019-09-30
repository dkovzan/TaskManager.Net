using AutoMapper;
using log4net;
using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TaskManager.WEB.Helpers;

namespace TaskManager.WEB.Controllers
{
    public class ApiController : BaseController
    {
        private readonly ILog _logger;
        public ApiController(IMapper mapper) : base(mapper)
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType); ;
        }

        public ActionResult SetCulture(string culture, string returnUrl)
        {
            _logger.Info($"GET Api/SetCulture?culture={culture}&returnUrl={returnUrl}");

            culture = CultureHelper.GetImplementedCulture(culture);

            var cookie = Request.Cookies["_culture"];

            if (cookie != null)
                cookie.Value = culture;
            else
            {
                cookie = new HttpCookie("_culture") {Value = culture, Expires = DateTime.Now.AddYears(1)};
            }

            Response.Cookies.Add(cookie);

            _logger.Info($"Cookie with culture {cookie.Value} and expiration date {cookie.Expires} is added to response.");

            return Redirect(returnUrl);
        }

        
    }
}