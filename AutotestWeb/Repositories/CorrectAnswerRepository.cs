using AutotestWeb.Models;
using Microsoft.Data.Sqlite;
using System.Reflection.PortableExecutable;

namespace AutotestWeb.Repositories;

public class CorrectAnswerRepository
{
    private readonly SqliteConnection _connection;

    CorrectAnswerRepository()
    {
        _connection = new SqliteConnection("Data source = autotest.db");

        _connection.Open();
    }

    public void CreateAnswerTable()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS answer_table(user_id TEXT, ticket_id INTEGER, question_id BIGINTEGER)";
        command.ExecuteNonQuery();
    }

    public void AddAnswer(TicketResult ticketResult, long questionId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"INSERT INTO answer_table(user_id, ticket_id, question_id) " +
            $"VALUES ('{ticketResult.UserId}', {ticketResult.TicketIndex}, {questionId})";
        command.ExecuteNonQuery();
    }

    public void DeleteAnswerTable(string userId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"DELETE FROM answer_table WHERE user_id = '{userId}'";
        command.ExecuteNonQuery();
    }

    public List<long> GetTicketAnswers(TicketResult ticket)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM answer_table WHERE user_id = '{ticket.UserId}' AND ticket_id = {ticket.TicketIndex}";
        var reader = command.ExecuteReader();
        var answers = new List<long>();

        while (reader.Read())
        {
            answers.Add(reader.GetInt64(2));
        }
        reader.Close();

        return answers;
    }

    public int GetAnswerCount(string userId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM answer_table WHERE user_id = '{userId}'";
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
