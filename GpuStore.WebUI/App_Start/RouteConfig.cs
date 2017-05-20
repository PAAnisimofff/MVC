using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GpuStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(null, "", new
                  {
                      controller = "Card",
                      action = "List",
                      manufacturer = (string)null,
                      page = 1
                  }
              );
            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Card", action = "List", manufacturer = (string)null },
                constraints: new { page = @"\d+" }
            );
            routes.MapRoute(null,
                "{manufacturer}",
                new { controller = "Card", action = "List", page = 1 }
            );
            routes.MapRoute(null,
                "{manufacturer}/Page{page}",
                new { controller = "Card", action = "List" },
                new { page = @"\d+" }
            );
            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
