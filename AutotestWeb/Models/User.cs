using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutotestWeb.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? PhotoPath { get; set; }
        public List<Result> Results { get; set; }

        public List<List<long>>? CorrectAnswers { get; set; }
        public string Language = "lotin";

        
    }
}
