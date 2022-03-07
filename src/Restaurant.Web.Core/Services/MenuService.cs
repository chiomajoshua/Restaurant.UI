using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Restaurant.Web.Core.Helpers.InternetClient;
using Restaurant.Web.Core.Interfaces;
using Restaurant.Web.Data;
using Restaurant.Web.Data.Models.Menu;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Restaurant.Web.Core.Services
{
    public class MenuService : IMenuService
    {
        private readonly ILogger<MenuService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientService _httpClientService;
        private readonly Dictionary<string, string> _header = new Dictionary<string, string>();
        public MenuService(IConfiguration configuration, IHttpClientService httpClientService, ILogger<MenuService> logger)
        {
            _logger = logger;
            _httpClientService = httpClientService;
            _configuration = configuration;
        }

        public async Task<Response<IEnumerable<MenuResponse>>> GetMenu(string token)
        {
            try
            {
                _header.Add("Token", token);
                var fullURL = $"{_configuration.GetSection("BaseUrl").Value}api/menu/getmenu";
                var response = await _httpClientService.MakeHttpCall(_httpClientService.BuildRequest(httpMethod: HttpMethod.Get, uri: fullURL, customHeaders: _header));
                var responseData = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return new Response<IEnumerable<MenuResponse>> { ResponseCode = (int)response.StatusCode, Message = "Menu Retrieved", Data = JsonConvert.DeserializeObject<IEnumerable<MenuResponse>>(responseData) };
                }
                return new Response<IEnumerable<MenuResponse>> { ResponseCode = (int)response.StatusCode, Message = responseData };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetMenu Error", ex.Message);
                return new Response<IEnumerable<MenuResponse>> { ResponseCode = 500, Message = ex.Message };
            }
        }
    }
}