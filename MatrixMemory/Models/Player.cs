using System;
using System.Text.Json.Serialization;

namespace MatrixMemory.Models;

public class Player
{
    [JsonInclude]
    public string UserName { get; set; }

    [JsonInclude]
    public string Password { get; set; }

    [JsonInclude]
    public int[]? LastGame { get; set; }

    [JsonInclude]
    public int[]? Statistics { get; set; }

    public Player(string userName, string password, int[]? lastGame = null, int[]? statistics = null)
    {
        UserName = userName;
        Password = password;
        LastGame = lastGame;
        Statistics = statistics;
    }
}