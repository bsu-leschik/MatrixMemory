namespace MatrixMemory.Models;

public class User
{
    public string UserName { get; set; }
    public int Password { get; set; }
    public int[] LastGame { get; set; }
    public int[] Statistics { get; set; }

    public User(string userName, string password, int[] lastGame, int[] statistics)
    {
        UserName = userName;
        Password = password.GetHashCode();
        LastGame = lastGame;
        Statistics = statistics;
    }
}