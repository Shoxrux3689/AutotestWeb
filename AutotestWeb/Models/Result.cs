using AutotestWeb.Models.Services;
using System.Security.Cryptography.X509Certificates;

namespace AutotestWeb.Models
{
    public class Result
    {
        public long CorrectCount { get; set; }
        public long InCorrectCount { get; set; }

        public Result() 
        {
            CorrectCount = 0;
            InCorrectCount = 0;
        }
    }
}
