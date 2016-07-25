using System.Web.Mvc;
using System.Web.Routing;

namespace NgCookingMVC_BackEND
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
           
           // routes.MapRoute(
           //    name: "Recettes",
           //    url: "Recettes/Create/{id}",
           //    defaults: new { controller = "Recettes", action = "Create", id = UrlParameter.Optional }
           //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
