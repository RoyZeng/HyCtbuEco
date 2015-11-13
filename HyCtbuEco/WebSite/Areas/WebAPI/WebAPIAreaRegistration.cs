using System.Web.Mvc;

namespace HyCtbuEco2015.Areas.WebAPI
{
    public class WebAPIAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WebAPI";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WebAPI_default",
                "WebAPI/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
                , new[] { "WebSite.Areas.WebApi.Controllers" }
            );
        }
    }
}
