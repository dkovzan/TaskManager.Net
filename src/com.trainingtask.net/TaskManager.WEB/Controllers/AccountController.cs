using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using TaskManager.Resources;
using TaskManager.WEB.ViewModels;
using log4net;
using System.Reflection;

namespace TaskManager.WEB.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ILog _logger;
        public AccountController(IMapper mapper) : base(mapper)
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            _logger.Info("GET Account/SignUp");

            var errorMessage = TempData["ErrorMessage"];

            if (errorMessage != null)
            {
                ViewBag.ErrorMessage = errorMessage;
            }

            return View("SignUp");
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "Login, Password, ConfirmPassword")] NewUser user)
        {
            _logger.Info($"POST Account/SignUp for {user}");

            if (!ModelState.IsValid)
            {
                return View("SignUp", user);
            }

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var identityUser = new IdentityUser() { UserName = user.Login };
            var result = manager.Create(identityUser, user.Password);

            _logger.Info($"{user} is successfully created!");

            if (result.Succeeded)
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(identityUser, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);

                _logger.Info($"{user} is successfully signed in!");

                return RedirectToAction("SignIn");
            }
            else
            {
                var error = result.Errors.FirstOrDefault();

                TempData["ErrorMessage"] = error;

                _logger.Info($"Error during authorization: {error}");

                return RedirectToAction("SignIn");
            }
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            _logger.Info("GET Account/SignIn");

            if (User.Identity.IsAuthenticated)
            {
                _logger.Info("User is authenticated.");

                return RedirectToAction(controllerName: "Project", actionName: "List");
            }

            return View("SignIn");
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn([Bind(Include = "Login, Password")] ExistingUser user)
        {
            _logger.Info($"POST Account/SignIn for {user}");

            if (!ModelState.IsValid)
            {
                return View("SignIn", user);
            }

            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var identityUser = userManager.Find(user.Login, user.Password);

            if (identityUser != null)
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(identityUser, DefaultAuthenticationTypes.ApplicationCookie);

                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);

                _logger.Info($"{user} is successfully authenticated!");

                return RedirectToAction(controllerName: "Project", actionName: "List");
            }
            else
            {
                _logger.Info($"{user} is not found!");

                ViewBag.ErrorMessage = CommonResource.WrongLoginOrPassword;
                return View("SignIn");
            }
        }

        public ActionResult LogOut()
        {
            _logger.Info("GET Account/LogOut");

            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();

            _logger.Info("User is successfully logged out.");

            return RedirectToAction("SignIn");
        }
    }
}