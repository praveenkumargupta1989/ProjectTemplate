using ACIPL.Template.Server.Services.Attributes;
using System.Web.Http;

namespace ACIPL.Template.Server.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new GlobalExceptionAttribute());
            config.Filters.Add(new AuthorizeRequestAttribute());
        }
    }
}
