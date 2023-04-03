namespace AutotestWeb.Models.Services
{
    public class UsersService
    {
        public static List<User> Users = new List<User>();

        public static User? GetCurrentUser(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey("UserId"))
            {
                var userId = context.Request.Cookies["UserId"];
                var user = UsersService.Users.FirstOrDefault(u => u.Id == userId);

                return user;
            }

            return null;
        }

        public static bool IsLoggedIn(HttpContext context)
        {
            if (!context.Request.Cookies.ContainsKey("UserId"))
                return false;

            var userId = context.Request.Cookies["UserId"];
            var user = UsersService.Users.FirstOrDefault(u => u.Id == userId);

            return user != null;
        }

        public static void Change(EditUser editUser, HttpContext context)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Id == context.Request.Cookies["UserId"])
                {
                    Users[i].Username = editUser.Username;
                    Users[i].Password = editUser.Password;
                    Users[i].Name = editUser.Name;
                }
            }
        }
    }
}
