using AutotestWeb.Models.Services;
using AutotestWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutotestWeb.Controllers;

public class QuestionController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Questions = QuestionsService.ReadQuestion("lotin");

        return View();
    }

    public IActionResult GetQuestionById(int id, int? choiceId = null)
    {
        if (!UsersService.IsLoggedIn(HttpContext))
        {
            return RedirectToAction("SignIn", "Users");
        }

        int ticketIndex = (id - 1) / 10;
        var user = UsersService.GetCurrentUser(HttpContext);
        var _questions = QuestionsService.ReadQuestion(user.Language);

        if (id > id * 10 + 10)
        {
            return RedirectToAction("Index", "Tickets");
        }
        
        var question = _questions?.FirstOrDefault(x => x.Id == id);
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

            if (choiceId != null)
            {
                var answer = question.Choices[(int)choiceId].Answer;

                ViewBag.Answer = answer;
                ViewBag.ChoiceId = choiceId;

                if (answer)
                {
                    if (user.CorrectAnswers[ticketIndex].Contains(question.Id))
                    {
                        user.CorrectAnswers[ticketIndex].Remove(question.Id);
                    }
                }
            }
        }
        
        return View();
    }
}
