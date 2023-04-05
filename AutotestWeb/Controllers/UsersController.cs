﻿using AutotestWeb.Models;
using AutotestWeb.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace AutotestWeb.Controllers;

public class UsersController : Controller
{

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

        var user = new User()
        {
            Id = Guid.NewGuid().ToString(),
            Name = createUser.Name,
            Username = createUser.Username,
            Password = createUser.Password,
            PhotoPath = SavePhoto(createUser.Photo!),
            CorrectAnswers = Lists(),
        };

        UsersService.Users.Add(user);

        HttpContext.Response.Cookies.Append("UserId", user.Id);

        return RedirectToAction("Index", "Home");
    }

    public List<List<long>> Lists()
    {
        var questions = QuestionsService.ReadQuestion("lotin");
        var list = new List<List<long>>();
        for (int i = 0; i < questions.Count / 10; i++)
        {
            var lists = new List<long>();

            for (int j = i * 10; j < i * 10 + 10; j++)
            {
                lists.Add(questions[j].Id);
            }
            list.Add(lists);
        }

        return list;
    }

    private string SavePhoto(IFormFile file)
    {
        if (!Directory.Exists("wwwroot/UserImages"))
            Directory.CreateDirectory("wwwroot/UserImages");

        var fileName = Guid.NewGuid().ToString() + ".jpg";

        var ms = new MemoryStream();
        file.CopyTo(ms);
        System.IO.File.WriteAllBytes(Path.Combine("wwwroot", "UserImages", fileName), ms.ToArray());

        return "/UserImages/" + fileName;
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

        var user = UsersService.Users.FirstOrDefault(u => u.Username == signInUser.Username && u.Password == signInUser.Password);

        if (user == null)
        {
            ModelState.AddModelError("Username", "Username or Password is xato");
            return View();
        }

        HttpContext.Response.Cookies.Append("UserId", user.Id);

        return RedirectToAction("Profile");
    }

    public IActionResult Profile()
    {
        var user = UsersService.GetCurrentUser(HttpContext);
        if (user == null)
        {
            return RedirectToAction("SignIn");
        }

        return View(user);
    }

    public IActionResult ProfileEdit()
    {
        var user = UsersService.GetCurrentUser(HttpContext);
        
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
            return View("ProfileEdit", UsersService.GetCurrentUser(HttpContext));
        }

        if (!UsersService.IsLoggedIn(HttpContext))
        {
            return RedirectToAction("SignIn");
        }

        UsersService.Change(editUser, HttpContext);

        return RedirectToAction("Profile");
    }


    public IActionResult LogOut()
    {
        HttpContext.Response.Cookies.Delete("UserId");

        return RedirectToAction("SignIn");
    }
}
