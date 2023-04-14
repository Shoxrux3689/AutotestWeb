namespace AutotestWeb.Models
{
    public class TicketResult
    {
        public string UserId { get; set; }
        public int TicketIndex { get; set; }
        public List<long>? CorrectAnswers { get; set; }
        public int CorrectAnswersCount { get; set; }
        public DateTime? Date { get; set; }
        
        public TicketResult()
        {
            CorrectAnswers = new List<long>();
            Date = null;
        }
    }
}
