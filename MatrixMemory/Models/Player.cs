using System.Collections;
using System.Text.Json.Serialization;

namespace MatrixMemory.Models;

public class Player
{
    public string UserName { get; set; }

    public string Password { get; set; }

    [JsonConverter(typeof(GameSaveJsonConverter))]
    public GameSave? LastGame { get; set; }

    public ArrayList? Statistics { get; set; }

    public Player(string userName, string password, ArrayList? statistics = null, GameSave? lastGame = null)
    {
        UserName = userName;
        Password = password;
        LastGame = lastGame;
        Statistics = statistics;
    }
}