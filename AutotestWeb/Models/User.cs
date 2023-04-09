using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutotestWeb.Models;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string? PhotoPath { get; set; }
    public Result Results { get; set; }

    public List<TicketResult> TicketResults { get; set; }
    public int CurrentTicketIndex { get; set; }
    public string Language = "lotin";
    
    public User() 
    {
        TicketResults = new List<TicketResult>();
    }
}
