using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Todo.Web.Context;

namespace Todo.Web
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly ISessionContext _sessionContext;

        public SessionAuthorizeAttribute()
        {
            _sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return _sessionContext.CurrentUser != null;
        }
    }
}