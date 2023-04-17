using AutotestWeb.Models;
using Microsoft.Data.Sqlite;

namespace AutotestWeb.Repositories;

public class InCorrectAnswerRepository
{
    private readonly SqliteConnection _connection;

    public InCorrectAnswerRepository()
    {
        _connection = new SqliteConnection("Data source = autotest.db");

        _connection.Open();
    }

    private void CreateAnswerTable()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS not_answer_table(user_id TEXT, ticket_id INTEGER, question_id BIGINTEGER)";
        command.ExecuteNonQuery();
    }

    public void AddAnswer(TicketResult ticketResult, long questionId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"INSERT INTO not_answer_table(user_id, ticket_id, question_id) " +
            $"VALUES ('{ticketResult.UserId}', {ticketResult.TicketIndex}, {questionId})";
        command.ExecuteNonQuery();
    }
    private void DeleteAnswerTable(string userId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"DELETE FROM not_answer_table WHERE user_id = '{userId}'";
        command.ExecuteNonQuery();
    }

    public int GetAnswerCount(string userId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM not_answer_table WHERE user_id = '{userId}'";
        var reader = command.ExecuteReader();
        var answers = new List<long>();

        while (reader.Read())
        {
            answers.Add(reader.GetInt64(2));
        }
        reader.Close();

        return answers.Count;
    }
}
