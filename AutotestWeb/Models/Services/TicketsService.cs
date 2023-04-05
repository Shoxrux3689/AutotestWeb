namespace AutotestWeb.Models.Services;

public static class TicketsService
{
    public static List<TicketModel> Tickets { get; set; }
    public static DateTime? Date { get; set; }


    public static List<TicketModel> FormaTickets(string language)
    {
        var questions = QuestionsService.ReadQuestion(language);

        var tickets = new List<TicketModel>();

        for (int i = 0; i < questions.Count / 10; i++)
        {
            var ticket = new TicketModel();
            for (int j = i * 10; j < i * 10 + 10; j++)
            {
                ticket.Ticket.Add(questions[j]);
            }
            tickets.Add(ticket);
        }

        return tickets;
    }
}
