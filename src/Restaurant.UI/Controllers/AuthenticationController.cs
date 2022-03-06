using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Web.Core.Interfaces;
using Restaurant.Web.Data.Models.Authentication;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Login));

            var result = await _authenticationService.Authenticate(loginRequest);
            if (result.ResponseCode == (int)HttpStatusCode.OK)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, result.Data.Email)
                };
                Token = result.Data.Token;
                HttpContext.Session.SetString("_email", result.Data.Email);
                HttpContext.Session.SetString("_token", result.Data.Token);


                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction(nameof(Login));
        }
    }
}