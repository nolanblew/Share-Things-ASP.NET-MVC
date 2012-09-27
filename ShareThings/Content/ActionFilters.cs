using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShareThings.ActionFilters
{
    public class AuthorizeFilter : ActionFilterAttribute
    {
        public AuthorizeFilter()
        {
            Admin = false;
        }

        public bool Admin { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!ShareThings.Content.SessionThings.UserLoggedIn(Admin))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {{ "Controller", "User" },
                                              { "Action", "Login" },
                                              { "redirectUrl", "~/" + filterContext.HttpContext.Request.RawUrl } });
            }
            base.OnActionExecuting(filterContext);
        }
    }


    public class SingleUserFilter : ActionFilterAttribute
    {
        public SingleUserFilter()
        {
            AdminHasAccess = false;
        }

        public int RequestedId { get; set; }
        public bool AdminHasAccess { get; set; }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (ShareThings.Content.SessionThings.LoggedInUser.id != RequestedId && !AdminHasAccess)
            {
                filterContext.Result = new HttpNotFoundResult();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}