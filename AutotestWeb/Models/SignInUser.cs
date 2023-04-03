using System.ComponentModel.DataAnnotations;

namespace AutotestWeb.Models
{
    public class SignInUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
