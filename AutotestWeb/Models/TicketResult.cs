namespace AutotestWeb.Models
{
    public class TicketResult
    {
        public string? UserId { get; set; }
        public int TicketIndex { get; set; }
        public List<long> CorrectAnswers { get; set; } = new List<long>();
        public DateTime Date { get; set; }
        
        public TicketResult()
        {
            CorrectAnswers = new List<long>();
        }
    }
}
