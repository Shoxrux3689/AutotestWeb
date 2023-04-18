using AutotestWeb.Repositories;

namespace AutotestWeb.Models.Services;

public class UsersService
{
    private readonly UserRepository _userRepository;
    private readonly CorrectAnswerRepository _correctAnswerRepository;
    private readonly InCorrectAnswerRepository _inCorrectAnswerRepository;

    public UsersService(UserRepository userRepository, CorrectAnswerRepository correct, InCorrectAnswerRepository inCorrect)
    {
        _userRepository = userRepository;
        _correctAnswerRepository = correct;
        _inCorrectAnswerRepository = inCorrect;
    }


    public User? GetCurrentUser(HttpContext context)
    {
        if (IsLoggedIn(context))
        {
            var userId = context.Request.Cookies["UserId"];

            var user = _userRepository.GetUserById(userId);
            user!.Results = new Result();
            user.Results.CorrectCount = _correctAnswerRepository.GetAnswerCount(userId);
            user.Results.InCorrectCount = _inCorrectAnswerRepository.GetAnswerCount(userId);
            user.TicketResults = Lists(user.Id);

            return user;
        }

        return null;
    }

    public bool IsLoggedIn(HttpContext context)
    {
        if (!context.Request.Cookies.ContainsKey("UserId"))
            return false;

        var userId = context.Request.Cookies["UserId"];
        var user = _userRepository.GetUserById(userId);

        return user != null;
    }

    public bool GetLogin(HttpContext context, SignInUser signInUser)
    {
        var user = _userRepository.GetUserByUsername(signInUser.Username);

        if (user != null || user.Password != signInUser.Password)
            return false;

        context.Response.Cookies.Append("UserId", user.Id);

        return true;
    }

    public void Change(EditUser editUser, HttpContext context)
    {
        var users = _userRepository.GetUsers();

        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].Id == context.Request.Cookies["UserId"])
            {
                users[i].Username = editUser.Username;
                users[i].Password = editUser.Password;
                users[i].Name = editUser.Name;
            }
        }
    }

    public void Registration(CreateUser createUser, HttpContext httpContext)
    {
        var user = new User()
        {
            Id = Guid.NewGuid().ToString(),
            Name = createUser.Name,
            Username = createUser.Username,
            Password = createUser.Password,
            PhotoPath = SavePhoto(createUser.Photo!),
            Results = new Result(),
            CurrentTicketIndex = 1,
        };

        var tr = Lists(user.Id);
        user.TicketResults = tr;

        _userRepository.AddUser(user);

        httpContext.Response.Cookies.Append("UserId", user.Id);
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

    public List<TicketResult> Lists(string id)
    {
        var list = new List<TicketResult>();
        for (int i = 1; i <= 70; i++)
        {
            var lists = new TicketResult()
            {
                TicketIndex = i,
                UserId = id,
            };

            list.Add(lists);
        }

        return list;
    }

    public void ClearResults(HttpContext httpContext)
    {
        var user = GetCurrentUser(httpContext);

        user!.TicketResults.Clear();
        user.Results = new Result();
        user.TicketResults = Lists(user.Id);
    }
}
