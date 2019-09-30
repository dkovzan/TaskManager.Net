using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using TaskManager.Resources;
using TaskManager.WEB.ViewModels;

namespace TaskManager.WEB.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IMapper mapper) : base(mapper)
        {
        }

        [HttpGet]
        public ActionResult SignUp()
        {
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
            if (!ModelState.IsValid)
            {
                return View("SignUp", user);
            }

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var identityUser = new IdentityUser() { UserName = user.Login };
            var result = manager.Create(identityUser, user.Password);

            if (result.Succeeded)
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(identityUser, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);

                return RedirectToAction("SignIn");
            }
            else
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
                return RedirectToAction("SignIn");
            }
        }

        [HttpGet]
        public ActionResult SignIn()

        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(controllerName: "Project", actionName: "List");
            }

            return View("SignIn");
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn([Bind(Include = "Login, Password")] ExistingUser user)
        {
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

                return RedirectToAction(controllerName: "Project", actionName: "List");
            }
            else
            {
                ViewBag.ErrorMessage = CommonResource.WrongLoginOrPassword;
                return View("SignIn");
            }
        }

        public ActionResult LogOut()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("SignIn");
        }
    }
}