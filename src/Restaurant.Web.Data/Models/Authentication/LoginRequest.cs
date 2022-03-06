using System.ComponentModel.DataAnnotations;

namespace Restaurant.Web.Data.Models.Authentication
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}