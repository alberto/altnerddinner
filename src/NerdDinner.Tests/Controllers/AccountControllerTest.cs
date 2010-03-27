using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Moq;
using NerdDinner.Models;
using NUnit.Framework;
using NerdDinner.Controllers;

namespace NerdDinner.Tests.Controllers {

    [TestFixture]
    public class AccountControllerTest {
        [Test]
        public void LoginGet() {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.LogOn();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void LoginPostRedirectsHomeIfLoginSuccessfulButNoReturnUrlGiven() {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.LogOn("someUser", "goodPass", true, null);

            // Assert
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void LoginPostRedirectsToReturnUrlIfLoginSuccessfulAndReturnUrlGiven() {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectResult result = (RedirectResult)controller.LogOn("someUser", "goodPass", false, "someUrl");

            // Assert
            Assert.AreEqual("someUrl", result.Url);
        }

        [Test]
        public void LoginPostReturnsViewIfUsernameNotSpecified() {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.LogOn("", "somePass", false, null);

            // Assert
            Assert.AreEqual(false, result.ViewData["rememberMe"]);
            Assert.AreEqual("You must specify a username.", result.ViewData.ModelState["username"].Errors[0].ErrorMessage);
        }

        [Test]
        public void LogOff() {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.LogOff();

            // Assert
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void RegisterPostRedirectsHomeIfRegistrationSuccessful() {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Register("someUser", "email");

            // Assert
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void RegisterPostReturnsViewIfEmailNotSpecified() {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("username", "");

            // Assert
            Assert.AreEqual("You must specify an email address.", result.ViewData.ModelState["email"].Errors[0].ErrorMessage);
        }

        [Test]
        public void RegisterPostReturnsViewIfUsernameNotSpecified() {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("", "email");

            // Assert
            Assert.AreEqual("You must specify a username.", result.ViewData.ModelState["username"].Errors[0].ErrorMessage);
        }

        private static AccountController GetAccountController() {
            IFormsAuthentication formsAuth = new MockFormsAuthenticationService();
            AccountController controller = new AccountController(formsAuth);
            ControllerContext controllerContext = new ControllerContext(new MockHttpContext(), new RouteData(), controller);
            controller.ControllerContext = controllerContext;
            return controller;
        }

        public class MockFormsAuthenticationService : IFormsAuthentication {
            public void SignIn(string userName, bool createPersistentCookie) {
            }

            public void SignOut() {
            }
        }

        public class MockIdentity : IIdentity {
            public string AuthenticationType {
                get {
                    return "MockAuthentication";
                }
            }

            public bool IsAuthenticated {
                get {
                    return true;
                }
            }

            public string Name {
                get {
                    return "someUser";
                }
            }
        }

        public class MockPrincipal : IPrincipal {
            IIdentity _identity;

            public IIdentity Identity {
                get {
                    if (_identity == null) {
                        _identity = new MockIdentity();
                    }
                    return _identity;
                }
            }

            public bool IsInRole(string role) {
                return false;
            }
        }

        public class MockMembershipUser : MembershipUser {
            public override bool ChangePassword(string oldPassword, string newPassword) {
                return newPassword.Equals("newPass");
            }
        }

        public class MockHttpContext : HttpContextBase {
            private IPrincipal _user;

            public override IPrincipal User {
                get {
                    if (_user == null) {
                        _user = new MockPrincipal();
                    }
                    return _user;
                }
                set {
                    _user = value;
                }
            }
        }

        public class MockMembershipProvider : MembershipProvider {
            string _applicationName;

            public override string ApplicationName {
                get {
                    return _applicationName;
                }
                set {
                    _applicationName = value;
                }
            }

            public override bool EnablePasswordReset {
                get {
                    return false;
                }
            }

            public override bool EnablePasswordRetrieval {
                get {
                    return false;
                }
            }

            public override int MaxInvalidPasswordAttempts {
                get {
                    return 0;
                }
            }

            public override int MinRequiredNonAlphanumericCharacters {
                get {
                    return 0;
                }
            }

            public override int MinRequiredPasswordLength {
                get {
                    return 6;
                }
            }

            public override string Name {
                get {
                    return null;
                }
            }

            public override int PasswordAttemptWindow {
                get {
                    return 3;
                }
            }

            public override MembershipPasswordFormat PasswordFormat {
                get {
                    return MembershipPasswordFormat.Clear;
                }
            }

            public override string PasswordStrengthRegularExpression {
                get {
                    return null;
                }
            }

            public override bool RequiresQuestionAndAnswer {
                get {
                    return false;
                }
            }

            public override bool RequiresUniqueEmail {
                get {
                    return false;
                }
            }

            public override bool ChangePassword(string username, string oldPassword, string newPassword) {
                throw new NotImplementedException();
            }

            public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer) {
                throw new NotImplementedException();
            }

            public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, Object providerUserKey, out MembershipCreateStatus status) {
                MockMembershipUser user = new MockMembershipUser();

                if (username.Equals("someUser") && password.Equals("goodPass") && email.Equals("email")) {
                    status = MembershipCreateStatus.Success;
                }
                else {
                    // the 'email' parameter contains the status we want to return to the user
                    status = (MembershipCreateStatus)Enum.Parse(typeof(MembershipCreateStatus), email);
                }

                return user;
            }

            public override bool DeleteUser(string username, bool deleteAllRelatedData) {
                throw new NotImplementedException();
            }

            public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords) {
                throw new NotImplementedException();
            }

            public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords) {
                throw new NotImplementedException();
            }

            public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords) {
                throw new NotImplementedException();
            }

            public override int GetNumberOfUsersOnline() {
                throw new NotImplementedException();
            }

            public override string GetPassword(string username, string answer) {
                throw new NotImplementedException();
            }

            public override string GetUserNameByEmail(string email) {
                throw new NotImplementedException();
            }

            public override MembershipUser GetUser(Object providerUserKey, bool userIsOnline) {
                throw new NotImplementedException();
            }

            public override MembershipUser GetUser(string username, bool userIsOnline) {
                return new MockMembershipUser();
            }

            public override string ResetPassword(string username, string answer) {
                throw new NotImplementedException();
            }

            public override bool UnlockUser(string userName) {
                throw new NotImplementedException();
            }

            public override void UpdateUser(MembershipUser user) {
                throw new NotImplementedException();
            }

            public override bool ValidateUser(string username, string password) {
                return password.Equals("goodPass");
            }

        }
    }
}
