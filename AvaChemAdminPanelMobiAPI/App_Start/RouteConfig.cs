using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using AvaChemAdminPanelMobiAPI.Common_File;
using Microsoft.AspNet.FriendlyUrls;

namespace AvaChemAdminPanelMobiAPI
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            List<RouteSettings> customRoutes = Routes.CustomRoutes();
            foreach (RouteSettings customRoute in customRoutes)
            {
                routes.MapPageRoute(
                    routeName: customRoute.RouteName,
                    routeUrl: customRoute.RouteUrl,
                    physicalFile: customRoute.PhysicalFile
                );
            }

            RouteTable.Routes.Add(new Route(Routes.JOB_REPORT + "/{ReportName}", new HttpHandlerRoute("~/Report.ashx")));

            var settings = new FriendlyUrlSettings
            {
                AutoRedirectMode = RedirectMode.Off
            };
            routes.EnableFriendlyUrls(settings);
        }
    }

    public class HttpHandlerRoute : IRouteHandler
    {

        private string _VirtualPath = null;

        public HttpHandlerRoute(string virtualPath)
        {
            _VirtualPath = virtualPath;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            IHttpHandler httpHandler = (IHttpHandler)System.Web.Compilation.BuildManager.CreateInstanceFromVirtualPath(_VirtualPath, typeof(IHttpHandler));
            return httpHandler;
        }
    }
}
