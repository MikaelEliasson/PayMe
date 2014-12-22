using FluentSecurity;
using PayMe.Core;
using PayMe2.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PayMe2.Security;

namespace PayMe2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SetupSecurity();

        }

        private void SetupSecurity()
        {
            SecurityConfigurator.Configure(configuration =>
            {
                // Let FluentSecurity know how to get the authentication status of the current user
                configuration.GetAuthenticationStatusFrom(() => HttpContext.Current.User.Identity.IsAuthenticated);

                // This is where you set up the policies you want FluentSecurity to enforce on your controllers and actions
                configuration.For<HomeController>().Ignore();
                configuration.For<AccountController>().DenyAuthenticatedAccess();
                //configuration.For<AccountController>(x => x.ChangePassword()).DenyAnonymousAccess();
                configuration.For<AccountController>(x => x.LogOff()).DenyAnonymousAccess();

                configuration.For<AbscenseController>().AddPolicy(new ShouldBelongToInstancePolicy());
                configuration.For<ExpenseController>().AddPolicy(new ShouldBelongToInstancePolicy());
                configuration.For<TransferController>().AddPolicy(new ShouldBelongToInstancePolicy());
                configuration.For<CategoryController>().AddPolicy(new ShouldBelongToInstancePolicy());
                configuration.For<InstanceController>().DenyAnonymousAccess();
                configuration.For<InstanceController>(x => x.Details(default(Guid))).AddPolicy(new ShouldBelongToInstancePolicy());

                configuration.Advanced.ModifySecurityContext(context =>
                {
                    var id = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current)).Values["instanceId"];
                    if (id != null)
                    {
                        context.Data.InstanceId = Guid.Parse(id.ToString());
                    }
                }
               );
            });

            GlobalFilters.Filters.Add(new HandleSecurityAttribute(), -1);
        }
    }
}
