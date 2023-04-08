﻿using AutotestWeb.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutotestWeb.Models;

namespace AutotestWeb.Controllers
{
    public class TicketsController : Controller
    {

        public IActionResult ChooseLanguage()
        {
            if (!UsersService.IsLoggedIn(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Index(string language, int page = 1)
        {
            if (!UsersService.IsLoggedIn(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            var tickets = TicketsService.FormaTickets(language);
            ViewBag.Tickets = tickets;
            var id = HttpContext.Request.Cookies["UserId"];
            var user = UsersService.Users.FirstOrDefault(u => u.Id == id);
            
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
            if (!UsersService.IsLoggedIn(HttpContext))
            {
                return RedirectToAction("SignIn", "Users");
            }

            int ticketIndex = (id - 1) / 10;
            var user = UsersService.GetCurrentUser(HttpContext);
            var ticket = TicketsService.FormaTickets(user.Language)[ticketIndex];

            if (id > id * 10 + 10)
            {
                return RedirectToAction("Index", "Tickets");
            }

            var question = ticket?.Ticket.FirstOrDefault(x => x.Id == id);
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
                    
                    if (answer)
                    {
                        if (user.TicketResults[ticketIndex].CorrectAnswers.Contains(question.Id))
                        {
                            user.TicketResults[ticketIndex].CorrectAnswers.Remove(question.Id);
                            user.Results.CorrectCount++;
                        }
                    }
                    else
                    {
                        user.Results.InCorrectCount++;
                    }
                }
            }

            return View(user);
        }

        //getquestionbyindex funksiyani yozishim kere
    }
}
