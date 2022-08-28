using System.Collections;
using System.Text.Json.Serialization;

namespace MatrixMemory.Models;

public class Player
{
    [JsonInclude]
    public string UserName { get; set; }

    [JsonInclude]
    public string Password { get; set; }

    [JsonInclude]
    public string? LastGame { get; set; }

    [JsonInclude]
    public ArrayList? Statistics { get; set; }

    public Player(string userName, string password, ArrayList? statistics = null, string? lastGame = null)
    {
        UserName = userName;
        Password = password;
        LastGame = lastGame;
        Statistics = statistics;
    }
}