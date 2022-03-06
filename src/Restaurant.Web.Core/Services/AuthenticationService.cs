using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Restaurant.Web.Core.Helpers.InternetClient;
using Restaurant.Web.Core.Interfaces;
using Restaurant.Web.Data;
using Restaurant.Web.Data.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Restaurant.Web.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientService _httpClientService;
        private readonly Dictionary<string, string> _header = new Dictionary<string, string>();
        public AuthenticationService(IConfiguration configuration, IHttpClientService httpClientService, ILogger<AuthenticationService> logger)
        {
            _logger = logger;
            _httpClientService = httpClientService;
            _configuration = configuration;
        }

        public async Task<Response<LoginResponse>> Authenticate(LoginRequest loginRequest)
        {
            try
            {
                var fullURL = $"{_configuration.GetSection("BaseUrl").Value}api/auth/login";
                var response = await _httpClientService.MakeHttpCall(_httpClientService.BuildRequest(httpMethod: HttpMethod.Post, uri: fullURL, loginRequest));
                var responseData = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {                    
                    _logger.LogInformation("Authenticate Response ----> ", $"Logon Successful for {loginRequest.Email} at {DateTime.Now}");
                    return new Response<LoginResponse> { ResponseCode = (int)response.StatusCode, Message = "Login Successful", Data = JsonConvert.DeserializeObject<LoginResponse>(responseData) };
                }
                _logger.LogInformation("Authenticate Response ----> ", $"Logon Failed For {loginRequest.Email} at {DateTime.Now}");

                var respHeaders = response.Headers;
                return new Response<LoginResponse> { ResponseCode = (int)response.StatusCode, Message = "Login ul"};
            }
            catch (Exception ex)
            {
                _logger.LogError("Authenticate Error", ex.Message);
                return new Response<LoginResponse> { ResponseCode = 500, Message = "Incorrect Email/Password" };
            }
        }


        //private static string ResponseMessage(HttpResponseMessage httpResponseMessage)
        //{
        //    Stream receiveStream = httpResponseMessage.GetResponseStream();
        //    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
        //    return readStream.ReadToEnd();
        //}
    }
}