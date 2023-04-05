using AutotestWeb.Models.Services;
using System.Security.Cryptography.X509Certificates;

namespace AutotestWeb.Models
{
    public class Result
    {
        public int CorrectCount { get; set; }
        public int QuestionCount { get; set; }
        public int InCorrectCount { get; set; }

        public Result() 
        {
            CorrectCount = 0;
            QuestionCount = QuestionsService.ReadQuestion("lotin").Count;
            InCorrectCount = 0;
        }
    }
}
