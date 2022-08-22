using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MatrixMemory.Models;

public static class PlayerData
{
    private static readonly string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                    Path.DirectorySeparatorChar + "MatrixMemory";
    private static bool CheckDir()
    {
        if (!Directory.Exists(_path))
        {
            Console.WriteLine("123");
            Directory.CreateDirectory(_path);
            return false;
        }

        return true;
    }

    public static async Task AddPlayer(Player player)
    {
        CheckDir();
        var json = JsonSerializer.Serialize<Player>(player);
        var playerFile = _path + Path.DirectorySeparatorChar + player.UserName + ".json";
        
        if (!File.Exists(playerFile))
        {
            await File.WriteAllTextAsync(playerFile, json);
        }
        else
        {
            throw  new ArgumentException($"User {player.UserName} already exists");
        }
    }

    public static async Task<Player?> IsPlayerValid(Player player)
    {
        if (!CheckDir())
        {
            throw new ArgumentException("There are no players registered");
        }

        var playerFile = _path + Path.DirectorySeparatorChar + player.UserName + ".json";
        if (!File.Exists(playerFile))
        {
            throw new ArgumentException("Username is invalid");
        }

        var playerString = await File.ReadAllTextAsync(playerFile);
        
        var realUser = JsonSerializer.Deserialize(playerString, typeof(Player));
        if ((realUser as Player)!.Password.Equals(player.Password))
        {
            return realUser as Player;
        }
        else
        {
            return null;
        }
    }

    public static string EncryptPassword(string password)
    {
        var data = System.Text.Encoding.ASCII.GetBytes(password);
        data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
        var hash = System.Text.Encoding.ASCII.GetString(data);
        return hash;
    }

    public static async void SavePlayer(Player player)
    {
        if (!CheckDir())
        {
            throw new ArgumentException("There are no players registered");
        }

        var playerFile = _path + Path.DirectorySeparatorChar + player.UserName + ".json";
        if (!File.Exists(playerFile))
        {
            throw new ArgumentException("Username is invalid");
        }

        var playerJson = JsonSerializer.Serialize(player);
        
        await File.WriteAllTextAsync(playerFile, playerJson);

    }
}