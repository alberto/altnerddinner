using System;
using System.Web.Mvc;
using NerdDinner.Infrastructure;
using NerdDinner.Models;

namespace NerdDinner.Controllers {

    [HandleError]
    public class AccountController : Controller {
        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth) {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
        }

        public IFormsAuthentication FormsAuth {
            get;
            private set;
        }

        public ActionResult LogOn() {

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userName, string password, bool rememberMe, string returnUrl) {

            if (!ValidateLogOn(userName, password)) {
                ViewData["rememberMe"] = rememberMe;
                return View();
            }

            FormsAuth.SignIn(userName, rememberMe);
            if (!String.IsNullOrEmpty(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff() {

            FormsAuth.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register() {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(string userName, string email) {
            if (!ValidateRegistration(userName, email))
            {
                return View();
            }

            FormsAuth.SignIn(userName, false /* createPersistentCookie */);
            return RedirectToAction("Index", "Home");
        }

        private bool ValidateLogOn(string userName, string password) {
            if (String.IsNullOrEmpty(userName)) {
                ModelState.AddModelError("username", "You must specify a username.");
            }

            return ModelState.IsValid;
        }

        private bool ValidateRegistration(string userName, string email) {
            if (String.IsNullOrEmpty(userName)) {
                ModelState.AddModelError("username", "You must specify a username.");
            }
            if (String.IsNullOrEmpty(email)) {
                ModelState.AddModelError("email", "You must specify an email address.");
            }

            return ModelState.IsValid;
        }

        }
    }