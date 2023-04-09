using Microsoft.Data.Sqlite;
using Autotest.Data.Extensions;

namespace Autotest.Data.Repositories;
/*
public class UserRepostiroy
{
    private readonly SqliteConnection _connection;
    private SqliteCommand _command;

    public UserRepostiroy()
    {
        _connection = new SqliteConnection(AutotestDataConstants.DatabaseConnectionString);
        _connection.Open();
        _command = _connection.CreateCommand();
        
        CreateUserTable();
    }

    private void CreateUserTable()
    {
        _command = _connection.CreateCommand();
        _command.CommandText = "CREATE TABLE IF NOT EXISTS users (id INTEGER NOT NULL, username TEXT UNIQUE, name TEXT NOT NULL, email TEXT UNIQUE, password TEXT NOT NULL, photo_url TEXT)";
        _command.ExecuteNonQuery();
    }

    public List<User> GetUsers()
    {
        var users = new List<User>();

        _command = _connection.CreateCommand();
        _command.CommandText = "SELECT * FROM users;";
        var reader = _command.ExecuteReader();

        while (reader.Read())
        {
            users.Add(reader.GetUser());
        }

        return users;
    }
}
*/