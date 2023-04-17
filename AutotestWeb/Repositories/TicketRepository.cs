using AutotestWeb.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace AutotestWeb.Repositories;

public class TicketRepository
{
    private readonly SqliteConnection _connection;

    public TicketRepository()
    {
        _connection = new SqliteConnection("Data source = autotest.db");

        _connection.Open();

        CreateTicketTable();
    }

    public void CreateTicketTable()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS tickets(" +
                                "user_id TEXT NOT NULL, " +
                                "_index INTEGER, " +
                                "_date BIGINT, " +
                                "correct_answers_count INTEGER)";
        command.ExecuteNonQuery();
    }

    private void AddTicket(TicketResult ticketResult)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"INSERT INTO tickets(" +
            $"user_id, _index, " +
            $"_date, correct_answers_count) " +
            $"VALUES('{ticketResult.UserId}', {ticketResult.TicketIndex}, {ticketResult.Date.Ticks} {ticketResult.CorrectAnswers.Count})";
        command.ExecuteNonQuery();
    }

    private void DeleteTicket(int ticketIndex, string userId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM tickets WHERE _index = @i AND user_id = @u";
        command.Parameters.AddWithValue("i", ticketIndex);
        command.Parameters.AddWithValue("u", userId);
        command.Prepare();
        command.ExecuteNonQuery();
    }

    public void UpdateTicket(TicketResult ticket)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"UPDATE tickets SET correct_answers_count = {ticket.CorrectAnswers.Count}, _date = {ticket.Date.Ticks} WHERE user_id = '{ticket.UserId}' AND index = {ticket.TicketIndex}";
        command.ExecuteNonQuery();
    }

    public TicketResult? GetTicket(TicketResult ticket)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM tickets WHERE user_id = '{ticket.UserId}' AND _index = {ticket.TicketIndex}";
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var ticketResult = new TicketResult
            {
                UserId = reader.GetString(0),
                TicketIndex = reader.GetInt32(1),
                Date = DateTime.FromFileTime(reader.GetInt64(2)),
            };

            reader.Close();

            return ticketResult;
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
            });
        }
        reader.Close();

        return tickets;
    }
}

