using AutotestWeb.Models;
using AutotestWeb.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace AutotestWeb.Controllers;

public class UsersController : Controller
{
    private readonly UsersService _usersService;
    private readonly TicketsService _ticketsService;
    public UsersController(UsersService users, TicketsService _ti) 
    {
        _usersService = users;
        _ticketsService = _ti;
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }


    [HttpPost]
    public IActionResult SignUp(CreateUser createUser)
    {
        if (!ModelState.IsValid)
        {
            return View(createUser);
        }
        _usersService.Registration(createUser, HttpContext);

        return RedirectToAction("Index", "Home");
    }



    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SignIn(SignInUser signInUser)
    {
        if (!ModelState.IsValid)
        {
            return View(signInUser);
        }

        var isLog = _usersService.GetLogin(HttpContext, signInUser);

        if (!isLog)
        {
            ModelState.AddModelError("Username", "Username or Password is invalid or injury or kasal");
            return View();
        }

        return RedirectToAction("Profile");
    }

    public IActionResult Profile()
    {
        var user = _usersService.GetCurrentUser(HttpContext);
        if (user == null)
        {
            return RedirectToAction("SignIn");
        }

        ViewBag.Tickets = _ticketsService.FormaTickets("lotin");

        return View(user);
    }

    public IActionResult ProfileEdit()
    {
        var user = _usersService.GetCurrentUser(HttpContext);
        
        if (user == null)
        {
            return RedirectToAction("SignIn");
        }

        return View(user);
    }

    public IActionResult ProfileEdit2(EditUser editUser)
    {
        if (!ModelState.IsValid)
        {
            return View("ProfileEdit", _usersService.GetCurrentUser(HttpContext));
        }

        if (!_usersService.IsLoggedIn(HttpContext))
        {
            return RedirectToAction("SignIn");
        }

        _usersService.Change(editUser, HttpContext);

        return RedirectToAction("Profile");
    }

    public IActionResult ClearStats()
    {
        if (!_usersService.IsLoggedIn(HttpContext))
        {
            return RedirectToAction("SignIn");
        }

        _usersService.ClearResults(HttpContext);

        return View();
    }
    public IActionResult LogOut()
    {
        HttpContext.Response.Cookies.Delete("UserId");

        return RedirectToAction("SignIn");
    }
}
