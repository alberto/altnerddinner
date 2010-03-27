using System.Web.Security;
using NerdDinner.Models;

namespace NerdDinner.Infrastructure
{
    public class FormsAuthenticationService : IFormsAuthentication {
        public void SignIn(string userName, bool createPersistentCookie) {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
        public void SignOut() {
            FormsAuthentication.SignOut();
        }
    }
}