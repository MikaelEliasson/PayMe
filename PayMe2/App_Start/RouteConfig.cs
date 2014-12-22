using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PayMe2
{

    public class GuidConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object value;
            if (!values.TryGetValue(parameterName, out value)) return false;
            if (value is Guid) return true;

            var stringValue = Convert.ToString(value);
            if (string.IsNullOrWhiteSpace(stringValue)) return false;

            Guid guidValue;
            if (!Guid.TryParse(stringValue, out guidValue)) return false;
            if (guidValue == Guid.Empty) return false;

            return true;
        }
    }

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "GroupRoutes",
                url: "{instanceId}/{controller}/{action}/{id}",
                defaults: new { controller = "Instance", id = UrlParameter.Optional },
                constraints: new { instanceId = new GuidConstraint() }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
