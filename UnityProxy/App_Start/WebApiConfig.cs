using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace UnityProxy
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(name: "Proxy", routeTemplate: "{ *path}", handler:
                HttpClientFactory.CreatePipeline
                (
                    innerHandler: new HttpClientHandler(), // will never get here if proxy is doing its job
                    handlers: new DelegatingHandler[]
                    { new ProxyHandler() }
                    ),
                      defaults: new { path = RouteParameter.Optional },
                      constraints:
                                null
                );
        }
    }
}
