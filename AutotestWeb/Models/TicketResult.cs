namespace AutotestWeb.Models
{
    public class TicketResult
    {
        public List<long>? CorrectAnswers { get; set; }
        public DateTime? Date { get; set; }
        public long StartIndex { get; set; }
        
        public TicketResult()
        {
            CorrectAnswers = new List<long>();
            Date = null;
            StartIndex = 0;
        }
    }
}
