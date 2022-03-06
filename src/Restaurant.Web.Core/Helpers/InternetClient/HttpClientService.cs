using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Restaurant.Web.Core.Helpers.Autofac;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Web.Core.Helpers.InternetClient
{
    public interface IHttpClientService : IAutoDependencyCore
    {
        Task<HttpResponseMessage> MakeHttpCall(HttpRequestMessage httpRequestMessage);
        HttpRequestMessage BuildRequest(HttpMethod httpMethod, string uri, object content = null, Dictionary<string, string> customHeaders = null);
    }
    public class HttpClientService : IHttpClientService
    {
        private readonly ILogger<HttpClientService> _logger;
        private readonly HttpClient _httpClient;

        public HttpClientService(ILogger<HttpClientService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> MakeHttpCall(HttpRequestMessage httpRequestMessage)
        {
            try
            {
                return await _httpClient.SendAsync(httpRequestMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public HttpRequestMessage BuildRequest(HttpMethod httpMethod, string uri, object content = null, Dictionary<string, string> customHeaders = null)
        {
            var request = new HttpRequestMessage(httpMethod, new Uri(uri));
            

            if (content != null) request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            
            request.Content.Headers.Clear();
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (customHeaders != null)
            {
                foreach (var header in customHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            return request;
        }
    }
}