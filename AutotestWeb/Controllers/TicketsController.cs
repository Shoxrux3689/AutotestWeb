using AutotestWeb.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutotestWeb.Models;
using AutotestWeb.Repositories;

namespace AutotestWeb.Controllers;


public class TicketsController : Controller
{
    private readonly TicketsService _ticketsService;
    private readonly UsersService _usersService;

    public TicketsController(TicketsService ticketService, UsersService usersService)
    {
        _ticketsService = ticketService;
        _usersService = usersService;
    }
    public IActionResult ChooseLanguage()
    {
        if (!_usersService.IsLoggedIn(HttpContext))
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    public IActionResult Index(string language, int page = 1)
    {
        if (!_usersService.IsLoggedIn(HttpContext))
        {
            return RedirectToAction("Index", "Home");
        }

        var tickets = _ticketsService.FormaTickets(language);
        ViewBag.Tickets = tickets;

        var user = _usersService.GetCurrentUser(HttpContext);
        
        if (user != null)
        {
            user.Language = language;
        }

        if (page > 7 || page < 1)
        {
            page = 7;
        }
        ViewBag.Page = page;
        ViewBag.Language = language;

        return View(user);
    }

    public IActionResult GetQuestionById(int id, int? choiceId = null)
    {
        if (!_usersService.IsLoggedIn(HttpContext))
        {
            return RedirectToAction("SignIn", "Users");
        }

        int ticketIndex = (id - 1) / 10;
        var user = _usersService.GetCurrentUser(HttpContext);
        var ticket = _ticketsService.FormaTickets(user!.Language)[ticketIndex];

        if (id > id * 10 + 10)
        {
            return RedirectToAction("Index", "Tickets");
        }

        var question = ticket?.Ticket!.FirstOrDefault(x => x.Id == id);
        if (question == null)
        {
            ViewBag.QuestionId = id;
            ViewBag.isSuccess = false;
        }
        else
        {
            ViewBag.Question = question;
            ViewBag.isSuccess = true;

            ViewBag.IsAnswer = choiceId != null;
            user.CurrentTicketIndex = ticketIndex;

            if (choiceId != null)
            {
                var answer = question.Choices[(int)choiceId].Answer;

                ViewBag.Answer = answer;
                ViewBag.ChoiceId = choiceId;
                user.TicketResults[ticketIndex].Date = DateTime.Now;
                
                _ticketsService.Update(user.TicketResults[ticketIndex], question.Id, answer);
            }
        }

        return View(user);
    }

    //getquestionbyindex funksiyani yozishim kere
}
