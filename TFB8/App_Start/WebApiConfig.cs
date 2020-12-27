using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TFB8
{
    using Interfaces;
    using Services;
    using System.Web.Services.Description;
    using Unity;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new Unity.UnityContainer();
            container.RegisterType(typeof(IDisciplineService), typeof(DisciplineService));
            config.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            //config.DependencyResolver.BeginScope();
            //config.DependencyResolver.GetService(typeof(DisciplineService));
            //config.Services.Add(typeof(IDisciplineService), new DisciplineService());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
