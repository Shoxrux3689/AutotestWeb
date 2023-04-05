using AutotestWeb.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutotestWeb.Models;

namespace AutotestWeb.Controllers
{
    public class TicketsController : Controller
    {

        private readonly List<QuestionModel> QuestionsLotin;

        public TicketsController() 
        {
            var json = System.IO.File.ReadAllText("JsonData/uzlotin.json");
            QuestionsLotin = JsonConvert.DeserializeObject<List<QuestionModel>>(json);
        }

        public IActionResult ChooseLanguage()
        {
            if (!UsersService.IsLoggedIn(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Index(string language)
        {
            if (!UsersService.IsLoggedIn(HttpContext))
            {
                return RedirectToAction("Index", "Home");
            }

            var tickets = QuestionsLotin.Count / 10;
            ViewBag.Tickets = tickets;
            var id = HttpContext.Request.Cookies["UserId"];
            var user = UsersService.Users.FirstOrDefault(u => u.Id == id);
            
            if (user != null)
            {
                user.Language = language;
            }

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

            var question = ticket?.FirstOrDefault(x => x.Id == id);
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

        //getquestionbyindex funksiyani yozishim kere
    }
}
