using DV.FrenteLoja.Core.MapperConfig;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Linq;
using static DV.FrenteLoja.Util.JsonNetResult;

namespace DV.FrenteLoja
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //iniciar Registro de mapeamentos
            AutoMapperConfig.RegisterMappings();            
            BaseConfig.ConfigurarDependencias();

            //Mapper.Initialize(c => c.AddProfile<MappingProfile>());
            ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            ValueProviderFactories.Factories.Add(new JsonDotNetValueProviderFactory());
        }

        protected internal void Application_BeginRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current.Request;
            if (context.CurrentExecutionFilePath.ToLower().Contains("elmah"))
            {
                var urlHelper = new UrlHelper(context.RequestContext);
                Response.Redirect(urlHelper.Action("Index", "Log"));
            }
        }
       }
}
