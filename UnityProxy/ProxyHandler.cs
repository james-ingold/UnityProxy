using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace UnityProxy
{
    public class ProxyHandler : DelegatingHandler
    {
        protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Get)
                request.Content = null;

            var content = request.Content;
            var result = content.ReadAsStringAsync().Result;
            var obj = JsonConvert.DeserializeObject<CustomRequest>(result);
            UriBuilder forwardUri = new UriBuilder(obj.Url);
            //strip off the proxy port and replace with an Http port
            forwardUri.Port = 80;
            request.RequestUri = forwardUri.Uri;
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpClient client = new HttpClient();
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return response;
        }
    }

    public class CustomRequest
    {
        public string Action { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Appname { get; set; }
        public string AppUserID { get; set; }
        public string PatientID { get; set; }
        public string Token { get; set; }
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public string Parameter3 { get; set; }
        public string Parameter4 { get; set; }
        public string Parameter5 { get; set; }
        public string Parameter6 { get; set; }
        public string Data { get; set; }
    }
}