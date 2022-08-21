using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace MatrixMemory.Models;

public static class PlayerData
{
    private static readonly string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                    Path.DirectorySeparatorChar + "MatrixMemory";

    private static void CheckDir()
    {
        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }
    }

    public static async Task AddPlayer(Player player)
    {
        CheckDir();
        var json = JsonSerializer.Serialize<Player>(player);
        var userFile = _path + Path.DirectorySeparatorChar + player.UserName + ".json";
        
        if (!File.Exists(userFile))
        {
            await File.WriteAllTextAsync(userFile, json);
        }
        else
        {
            throw  new ArgumentException($"User {player.UserName} already exists");
        }
    }
}