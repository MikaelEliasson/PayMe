using FluentSecurity;
using FluentSecurity.Policy;
using PayMe.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;

namespace PayMe2.Security
{
    public class ShouldBelongToInstancePolicy : ISecurityPolicy
    {
        public FluentSecurity.PolicyResult Enforce(FluentSecurity.ISecurityContext context)
        {
            Guid instanceId = context.Data.InstanceId;

            var identity = HttpContext.Current.User.Identity;
            if (!identity.IsAuthenticated)
            {
                return PolicyResult.CreateFailureResult(new DenyAnonymousAccessPolicy(), "Not authenticated");
            }
            using (var db = Context.Create())
            {
                var userId = Guid.Parse(identity.GetUserId());
                var membership = db.UserToInstanceMappings.FirstOrDefault(m => m.UserId == userId);
                if (membership == null)
                {
                    return PolicyResult.CreateFailureResult(this, "Not authenticated");
                }
            }


            return PolicyResult.CreateSuccessResult(this);
        }


    }

    public class ShouldBelongToInstancePolicyViolationHandler : IPolicyViolationHandler
    {
        public System.Web.Mvc.ActionResult Handle(PolicyViolationException exception)
        {
            return new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Account", action = "NoAccess" }));
        }
    }

    public class DenyAnonymousAccessPolicyViolationHandler : IPolicyViolationHandler
    {
        public System.Web.Mvc.ActionResult Handle(PolicyViolationException exception)
        {
            return new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = HttpContext.Current.Request.Url.PathAndQuery }));
        }
    }
}