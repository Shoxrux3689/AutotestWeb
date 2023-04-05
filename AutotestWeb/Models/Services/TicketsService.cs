namespace AutotestWeb.Models.Services;

public static class TicketsService
{
    public static List<List<QuestionModel>> Tickets { get; set; }


    public static List<List<QuestionModel>> FormaTickets(string language)
    {
        var questions = QuestionsService.ReadQuestion(language);

        var tickets = new List<List<QuestionModel>>();

        for (int i = 0; i < questions.Count / 10; i++)
        {
            var ticket = new List<QuestionModel>();
            for (int j = i * 10; j < i * 10 + 10; j++)
            {
                ticket.Add(questions[j]);
            }
            tickets.Add(ticket);
        }

        return tickets;
    }
}
