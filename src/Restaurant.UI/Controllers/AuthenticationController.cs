using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Web.Core.Interfaces;
using Restaurant.Web.Data.Models.Authentication;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;

namespace Restaurant.UI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        public static string Token { get; set; }

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public ActionResult Login() => View();


        [HttpPost]
        public JsonResult Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return Json(false);

            var result = _authenticationService.Authenticate(loginRequest.Email, loginRequest.Password).Result;
            if (result.ResponseCode == (int)HttpStatusCode.OK)
            {
                _ = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, result.Data.Email)
                };
                Token = result.Data.Token;
                HttpContext.Session.SetString("_email", result.Data.Email);
                HttpContext.Session.SetString("_token", result.Data.Token);
                return Json(true);
            }
            ViewBag.ErrorMessage = result.Message;
            return Json(false);
        }
    }
}