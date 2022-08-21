namespace MatrixMemory.Models;

public class Player
{
    public string UserName { get; set; }

    public int Password { get; set; }

    public int[]? LastGame { get; set; }

    public int[]? Statistics { get; set; }

    public Player(string userName, string password, int[]? lastGame = null, int[]? statistics = null)
    {
        UserName = userName;
        Password = password.GetHashCode();
        LastGame = lastGame;
        Statistics = statistics;
    }
}