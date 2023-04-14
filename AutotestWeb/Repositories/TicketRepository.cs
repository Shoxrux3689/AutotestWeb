using AutotestWeb.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace AutotestWeb.Repositories;

public class TicketRepository
{
    private readonly SqliteConnection _connection;

    TicketRepository()
    {
        _connection = new SqliteConnection("Data source = autotest.db");

        _connection.Open();
    }

    private void CreateTicketTable()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS tickets(user_id TEXT NOT NULL, index INTEGER," +
            " date BIGINT, correct_answers_count INTEGER)";
        command.ExecuteNonQuery();
    }

    private void AddTicket(TicketResult ticketResult)
    {
        //date qosh
        var command = _connection.CreateCommand();
        command.CommandText = $"INSERT INTO tickets(user_id, index, correct_answers_count) " +
            $"VALUES('{ticketResult.UserId}', {ticketResult.TicketIndex}, {ticketResult.CorrectAnswersCount})";
        command.ExecuteNonQuery();
    }

    private void DeleteTicket(int ticketIndex, string userId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM tickets WHERE index = @i AND user_id = @u";
        command.Parameters.AddWithValue("i", ticketIndex);
        command.Parameters.AddWithValue("u", userId);
        command.Prepare();
        command.ExecuteNonQuery();
    }

    private void UpdateTicket(TicketResult ticket)
    {
        //date qosh
        var command = _connection.CreateCommand();
        command.CommandText = $"UPDATE tickets SET correct_answers_count = {ticket.CorrectAnswersCount}, WHERE user_id = '{ticket.UserId}' AND index = {ticket.TicketIndex}";
        command.ExecuteNonQuery();
    }

    public TicketResult? GetTicket(TicketResult ticket)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM tickets WHERE user_id = '{ticket.UserId}' AND index = {ticket.TicketIndex}";
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            return new TicketResult
            {
                UserId = reader.GetString(0),
                TicketIndex = reader.GetInt32(1),
                Date = DateTime.FromFileTime(reader.GetInt64(2)),
                CorrectAnswersCount = reader.GetInt32(3)
            };
        }
        reader.Close();

        return null;
    }

    public List<TicketResult> GetTicketList(TicketResult ticket)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM tickets WHERE user_id = '{ticket.UserId}'";
        var reader = command.ExecuteReader();

        var tickets = new List<TicketResult>();

        while (reader.Read())
        {
            tickets.Add(new TicketResult
            {
                UserId = reader.GetString(0),
                TicketIndex = reader.GetInt32(1),
                Date = DateTime.FromFileTime(reader.GetInt64(2)),
                CorrectAnswersCount = reader.GetInt32(3)
            });
        }
        reader.Close();

        return tickets;
    }
}

