using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityProxy;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using System.Diagnostics;
using Owin;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(UnityProxy.Console.Proxy))]

namespace UnityProxy.Console
{
    public class Proxy
    {
        static List<IDisposable> apps = new List<IDisposable>();

        public static void Start(string proxyAddress)
        {
            try
            {
                // Start OWIN proxy host 
                apps.Add(WebApp.Start<Proxy>(proxyAddress));
                Trace.WriteLine("Proxy server is running");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                    message += ":" + ex.InnerException.Message;
                Trace.TraceInformation(message);
            }
        }

        public static void Stop()
        {
            foreach (var app in apps)
            {
                if (app != null)
                    app.Dispose();
            }
        }

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder app)
        {
 
            //appBuilder.UseStaticFiles();
            // Configure Web API for self-host. 
            HttpConfiguration httpconfig = new HttpConfiguration();
            app.UseCors(CorsOptions.AllowAll);
            RegisterRoutes(httpconfig);
            app.UseWebApi(httpconfig);
        }

        private void RegisterRoutes(HttpConfiguration config)
        {
            //anything that needs to fall through needs to go in the pipeline
            config.Routes.MapHttpRoute(
            name: "Proxy",
            routeTemplate: "{*path}",
            handler: HttpClientFactory.CreatePipeline
                (
                    innerHandler: new HttpClientHandler(), // will never get here if proxy is doing its job
                    handlers: new DelegatingHandler[]
                    {
                        new ProxyHandler()
                    }
                ),
            defaults: new { path = RouteParameter.Optional },
            constraints: null);
        }
    }

}
