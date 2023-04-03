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

        //getquestionbyindex funksiyani yozishim kere
    }
}
