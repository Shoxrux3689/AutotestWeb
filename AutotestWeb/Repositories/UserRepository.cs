using AutotestWeb.Models;
using Microsoft.Data.Sqlite;

namespace AutotestWeb.Repositories;

public class UserRepository
{
    private readonly SqliteConnection _connection;
    
    public UserRepository()
    {
        _connection = new SqliteConnection("Data source = autotest.db");
        
        _connection.Open();

        CreateUserTable();
    }

    public void CreateUserTable()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS users(id TEXT UNIQUE, username TEXT NOT NULL, password TEXT, name TEXT, photo_url TEXT, current_ticket_index INTEGER, language TEXT)";
        command.ExecuteNonQuery();
    }

    public void AddUser(User user)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"INSERT INTO users(id, username, password, name, photo_url, current_ticket_index, language) " +
            $"VALUES ('{user.Id}', '{user.Username}', '{user.Password}', '{user.Name}', '{user.PhotoPath}', {user.CurrentTicketIndex}, '{user.Language}')";
        command.ExecuteNonQuery();
    }

    public User? GetUserById(string id)
    {
        return GetUser("id", id);
    }
    public User? GetUserByUsername(string username)
    {
        return GetUser("id", username);
    }

    public User? GetUser(string paramName, string paramValue)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM users WHERE {paramName} = @p";
        command.Parameters.AddWithValue("p", paramValue);
        command.Prepare();
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var user = new User
            {
                Id = (string)reader["id"],
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Name = reader.GetString(3),
                PhotoPath = reader.GetString(4),
                CurrentTicketIndex = reader.GetInt32(5),
                Language = reader.GetString(6),
            };
            
            reader.Close();

            return user;
        }
        reader.Close();

        return null;
    }

    public List<User> GetUsers()
    {
        var users = new List<User>();
        var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM users";
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            users.Add(new User 
            { 
                Id = (string)reader["id"], 
                Username = reader.GetString(1), 
                Password = reader.GetString(2),
                Name = reader.GetString(3),
                PhotoPath = reader.GetString(4),
                CurrentTicketIndex = reader.GetInt32(5),
                Language = reader.GetString(6),
            });
        }

        reader.Close();

        return users;
    }

    private void UpdateUser(User user)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "UPDATE users SET username = @username, password = @password," +
            " name = @name, photo_url = @foto, current_ticket_index = index WHERE id = @id";
        command.Parameters.AddWithValue("id", user.Id);
        command.Parameters.AddWithValue("username", user.Username);
        command.Parameters.AddWithValue("password", user.Password);
        command.Parameters.AddWithValue("name", user.Name);
        command.Parameters.AddWithValue("foto", user.PhotoPath);
        command.Parameters.AddWithValue("index", user.CurrentTicketIndex);
        command.Prepare();
        command.ExecuteNonQuery();
    }
}
