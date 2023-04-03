using System.ComponentModel.DataAnnotations;

namespace AutotestWeb.Models
{
    public class CreateUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public IFormFile? Photo { get; set; }
    }
}
