using System;
using System.Web.Mvc;
using NerdDinner.Models;

namespace NerdDinner.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {
        private readonly IFormsAuthentication _formsAuth;

        public AccountController(IFormsAuthentication formsAuth)
        {
            _formsAuth = formsAuth;
        }

        public ActionResult LogOn()
        {

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userName, string password, bool rememberMe, string returnUrl)
        {

            if (!ValidateLogOn(userName, password))
            {
                ViewData["rememberMe"] = rememberMe;
                return View();
            }

            _formsAuth.SignIn(userName, rememberMe);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {
            _formsAuth.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(string userName, string email)
        {
            if (!ValidateRegistration(userName, email))
            {
                return View();
            }

            _formsAuth.SignIn(userName, false /* createPersistentCookie */);
            return RedirectToAction("Index", "Home");
        }

        private bool ValidateLogOn(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }

            return ModelState.IsValid;
        }

        private bool ValidateRegistration(string userName, string email)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }
            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", "You must specify an email address.");
            }

            return ModelState.IsValid;
        }
    }
}