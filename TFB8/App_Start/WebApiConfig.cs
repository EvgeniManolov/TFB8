namespace TFB8
{
    using System.Web.Http;
    using Interfaces;
    using Services;
    using Unity;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new Unity.UnityContainer();
            container.RegisterType(typeof(IDisciplineService), typeof(DisciplineService));
            container.RegisterType(typeof(ISemesterService), typeof(SemesterService));
            config.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
           
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
