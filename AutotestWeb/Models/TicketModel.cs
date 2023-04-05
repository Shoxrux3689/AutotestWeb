namespace AutotestWeb.Models
{
    public class TicketModel
    {
        public List<QuestionModel>? Ticket { get; set; }
        public DateTime? Date { get; set; }

        public TicketModel()
        {
            Ticket = new List<QuestionModel>();
        }
    }
}
