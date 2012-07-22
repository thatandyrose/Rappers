using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Rappers.Baseline.Helpers.Web
{
    public interface IManageAuthentication
    {
        void LogOn(string username, bool rememberMe);
        void LogOut();
        string LoggedInUserId();
    }

    public class FormsAuthenticationManager : IManageAuthentication
    {
        public void LogOn(string username, bool rememberMe)
        {
            FormsAuthentication.SetAuthCookie(username, rememberMe);
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        public string LoggedInUserId()
        {
            return HttpContext.Current.Request.IsAuthenticated ? ((GenericPrincipal) HttpContext.Current.User).Identity.Name : null;
        }
    }
}
