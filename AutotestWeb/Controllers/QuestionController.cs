using AutotestWeb.Models.Services;
using AutotestWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutotestWeb.Controllers;

public class QuestionController : Controller
{
    private readonly List<QuestionModel> QuestionsLotin;
    private readonly List<QuestionModel> QuestionsKiril;
    private readonly List<QuestionModel> QuestionsRus;
    public QuestionController() 
    {
        var json = System.IO.File.ReadAllText("JsonData/uzlotin.json");
        QuestionsLotin = JsonConvert.DeserializeObject<List<QuestionModel>>(json);

        var json1 = System.IO.File.ReadAllText("JsonData/uzkiril.json");
        QuestionsKiril = JsonConvert.DeserializeObject<List<QuestionModel>>(json1);
        
        var json2 = System.IO.File.ReadAllText("JsonData/rus.json");
        QuestionsRus = JsonConvert.DeserializeObject<List<QuestionModel>>(json2);
    }

    public IActionResult Index()
    {
        ViewBag.Questions = QuestionsLotin;

        return View();
    }

    public IActionResult GetQuestionById(int id, int ticketIndex, int? choiceId = null)
    {
        if (!UsersService.IsLoggedIn(HttpContext))
        {
            return RedirectToAction("SignIn", "Users");
        }

        var user = UsersService.GetCurrentUser(HttpContext);
        var _questions = QuestionLanguage(user.Language);


        if (HttpContext.Request.Cookies.ContainsKey("CurrentTicketIndex"))
        {
            var index = Convert.ToInt32(HttpContext.Request.Cookies["CurrentTicketIndex"]);

            if (id > index * 10 + 10)
            {
                var correctCount = Convert.ToInt32(HttpContext.Request.Cookies["CorrectAnswerCount"]);

                HttpContext.Response.Cookies.Delete("CurrentTicketIndex");
                HttpContext.Response.Cookies.Delete("CorrectAnswerCount");

                if (user != null)
                {
                    user.Results.Add(new Result
                    {
                        CorrectCount = correctCount,
                        QuestionCount = 10,
                        TicketIndex = index
                    });
                }

                return RedirectToAction(nameof(Result),
                    new { ticketIndex = index, correctCount = correctCount });
            }
        }

        
            HttpContext.Response.Cookies.Append("CurrentTicketIndex", ticketIndex.ToString());
        
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
                        if (!user.CorrectAnswers[ticketIndex].Contains(question.Id))
                        {
                            user.CorrectAnswers[ticketIndex].Add(question.Id);
                        }

                        if (HttpContext.Request.Cookies.ContainsKey("CorrectAnswerCount"))
                        {
                            var correctCount = Convert.ToInt32(HttpContext.Request.Cookies["CorrectAnswerCount"]);

                            HttpContext.Response.Cookies.Append("CorrectAnswerCount", $"{correctCount + 1}");
                        }
                        else
                        {
                            HttpContext.Response.Cookies.Append("CorrectAnswerCount", "1");
                        }
                    }
                }
            }
        
        return View();
    }
    public IActionResult Result(int ticketIndex, int correctCount)
    {
        ViewBag.TicketIndex = ticketIndex;
        ViewBag.CorrectCount = correctCount;

        return View();
    }

    private List<QuestionModel> QuestionLanguage(string language)
    {
        if (language == "kiril")
        {
            return QuestionsKiril;
        }
        else if (language == "lotin")
        {
            return QuestionsLotin;
        }
        else if (language == "rus")
        {
            return QuestionsRus;
        }

        return QuestionsLotin;
    }

}
