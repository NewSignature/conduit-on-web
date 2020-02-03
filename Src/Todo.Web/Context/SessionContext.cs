using System;
using System.Web;
using System.Web.Security;
using Todo.Data.Entities;

namespace Todo.Web.Context
{
    public class SessionContext : ISessionContext
    {
        // instance members
        public User CurrentUser
        {
            get => HttpContext.Current.Session[UserSessionKey] as User;
            set
            {
                HttpContext.Current.Session[UserSessionKey] = value;
                //SaveSessionCookie(value.Username);
            }
        }

        // private consts for key lookup
        private const string UserSessionKey = "userKey";

        // other methods
        /*void SaveSessionCookie(string username)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(username, isPersistent: true, (int)TimeSpan.FromMinutes(120).TotalMinutes);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true,
                Expires = ticket.Expiration
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }*/
    }
}
