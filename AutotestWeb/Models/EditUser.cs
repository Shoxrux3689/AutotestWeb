using System.ComponentModel.DataAnnotations;

namespace AutotestWeb.Models
{
    public class EditUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
