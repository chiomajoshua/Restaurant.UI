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
        public AuthenticationService(IConfiguration configuration, IHttpClientService httpClientService, ILogger<AuthenticationService> logger)
        {
            _logger = logger;
            _httpClientService = httpClientService;
            _configuration = configuration;
        }

        public async Task<Response<LoginResponse>> Authenticate(string email, string password)
        {
            try
            {
                var fullURL = $"{_configuration.GetSection("BaseUrl").Value}api/auth/login";
                var response = await _httpClientService.MakeHttpCall(_httpClientService.BuildRequest(httpMethod: HttpMethod.Post, uri: fullURL, new LoginRequest { Email = email, Password = password}));
                var responseData = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {                    
                    _logger.LogInformation("Authenticate Response ----> ", $"Logon Successful for {email} at {DateTime.Now}");
                    return new Response<LoginResponse> { ResponseCode = (int)response.StatusCode, Message = "Login Successful", Data = JsonConvert.DeserializeObject<LoginResponse>(responseData) };
                }
                _logger.LogInformation("Authenticate Response ----> ", $"Logon Failed For {email} at {DateTime.Now}");

                var respHeaders = response.Headers;
                return new Response<LoginResponse> { ResponseCode = (int)response.StatusCode, Message = responseData};
            }
            catch (Exception ex)
            {
                _logger.LogError("Authenticate Error", ex.Message);
                return new Response<LoginResponse> { ResponseCode = 500, Message = "Incorrect Email/Password" };
            }
        }

    }
}