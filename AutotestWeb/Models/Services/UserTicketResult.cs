namespace AutotestWeb.Models.Services
{
    public class UserTicketResult
    {
        public List<int> CorrectAnswer { get; set; }

        public UserTicketResult()
        {
            CorrectAnswer = new List<int>(5);
        }
    }
}
