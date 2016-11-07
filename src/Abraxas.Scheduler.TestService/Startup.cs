using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Swashbuckle.Application;

namespace Abraxas.Scheduler.TestService
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            var loDummyType = typeof(Abraxas.Scheduler.Core.Controllers.JobController);
            string lsTypeName = loDummyType.FullName;

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Json Output erzwingen
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            config.EnableSwagger(c =>
            {
                //c.RootUrl(req => "http://localhost:9788/api");
                c.SingleApiVersion("v1", "abr Scheduler");

            }).EnableSwaggerUi();

            appBuilder.UseWebApi(config);

            appBuilder.UseNancy();

        }

    }
}
