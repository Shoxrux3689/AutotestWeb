using AutotestWeb.Repositories;

namespace AutotestWeb.Models.Services;

public class TicketsService
{
    private readonly TicketRepository _ticketRepository;
    private readonly QuestionsService _questionsService;
    private readonly CorrectAnswerRepository _correctAnswerRepository;
    private readonly InCorrectAnswerRepository _inCorrectAnswerRepository;
    public TicketsService(
        TicketRepository ticketRepository,
        QuestionsService questions,
        CorrectAnswerRepository canswer, 
        InCorrectAnswerRepository icanswer) 
    {
        _ticketRepository = ticketRepository;
        _questionsService = questions;
        _correctAnswerRepository = canswer;
        _inCorrectAnswerRepository = icanswer;
    }

    public List<TicketModel> FormaTickets(string language)
    {
        var questions = _questionsService.ReadQuestion(language);

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

    public void Update(TicketResult ticketResult, long questionId, bool isAnswer)
    {
        
        if (isAnswer)
        {
            if (!ticketResult.CorrectAnswers.Contains(questionId))
            {
                ticketResult.CorrectAnswers.Add(questionId);
                _correctAnswerRepository.AddAnswer(ticketResult, questionId);
            }
        }
        else
        {
            _inCorrectAnswerRepository.AddAnswer(ticketResult, questionId);
        }
        
        _ticketRepository.UpdateTicket(ticketResult);
    }
}


